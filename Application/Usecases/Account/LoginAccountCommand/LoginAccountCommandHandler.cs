using Application.Common;
using Domain.Account.Model;
using Domain.Account.Port;
using Domain.Common;
using FluentResults;
using FluentValidation;
using MediatR;

namespace Application.Usecases.Account.LoginAccountCommand;

public class LoginAccountCommandHandler(
    IUnitOfWork uow,
    IJWTGenerator jwtGenerator,
    ITokenRotationPolicy tokenRotationPolicy,
    IValidator<LoginAccountCommand> validator
    ) : IRequestHandler<LoginAccountCommand, Result<LoginAccountResponse>>
{
    private readonly IUnitOfWork _uow = uow;
    private readonly ITokenRotationPolicy _tokenRotationPolicy = tokenRotationPolicy;
    private readonly IValidator<LoginAccountCommand> _validator = validator;
    private readonly IJWTGenerator _jwtGenerator = jwtGenerator;
    public async Task<Result<LoginAccountResponse>> Handle(LoginAccountCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToResult<LoginAccountResponse>();

        var repository = _uow.GetRepository<UserRepositoryPort>();
        var user = await repository.GetUserByEmailAsync(request.Email);

        if (user == null)
            return Result.Fail(new Error("Invalid credentials")
                .WithMetadata("ErrorCode", "INVALID_CREDENTIALS"));

        //TODO: HASH PASSWORD CHECK
        if (request.Password != user.PasswordHash)
            return Result.Fail(new Error("Invalid credentials")
                .WithMetadata("ErrorCode", "INVALID_CREDENTIALS"));

        string jwt = _jwtGenerator.GenerateToken(user);
        var activeToken = user.RefreshTokens?.FirstOrDefault(x => x.IsActive);
        string refreshToken;
        if (activeToken != null && !_tokenRotationPolicy.ShouldRotateToken(activeToken))
        {
            refreshToken = activeToken.Token;
        }
        else
        {
            if (activeToken != null)
            {
                activeToken.InvalidateToken();
            }

            refreshToken = _jwtGenerator.GenerateRefreshToken();
            var newRefreshToken = new RefreshToken(
                token: refreshToken,
                expiresAt: DateTime.UtcNow.AddDays(7),
                createdAt: DateTime.UtcNow,
                revokedAt: null
            );

            user.AddRefreshToken(newRefreshToken);

            await _uow.CompleteAsync(cancellationToken);
        }
        return Result.Ok(new LoginAccountResponse
            (
                user.Id.ToString(),
                jwt,
                refreshToken
            )
         );
    }
}
