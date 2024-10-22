using Application.Common;
using Domain.Account.Model;
using Domain.Account.Port;
using Domain.Common;
using FluentResults;
using FluentValidation;
using MediatR;

namespace Application.Usecases.Account.CreateAccountCommand;

public class CreateAccountCommandHandler(IUnitOfWork uow, IJWTGenerator jwtGenerator, IValidator<CreateAccountCommand> validator) : IRequestHandler<CreateAccountCommand, Result<CreateAccountResponse>>
{
    private readonly IUnitOfWork _uow = uow;
    private readonly IJWTGenerator _jwtGenerator = jwtGenerator;
    private readonly IValidator<CreateAccountCommand> _validator = validator;
    public async Task<Result<CreateAccountResponse>> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        var validation = await _validator.ValidateAsync(request);
        if (!validation.IsValid) 
            return validation.ToResult<CreateAccountResponse>();

        var repository = _uow.GetRepository<UserRepositoryPort>(); 
        
        //TODO: Hash the password
        var user = new User(request.Username, request.Email, request.Password);

        var jwt = _jwtGenerator.GenerateToken(user);
        var refreshTokenString = _jwtGenerator.GenerateRefreshToken();

        var refreshToken = new RefreshToken(
            refreshTokenString,
            expiresAt: DateTime.UtcNow.AddDays(7),
            createdAt: DateTime.UtcNow,
            null);

        user.AddRefreshToken(refreshToken);

        await repository.AddAsync(user, cancellationToken);

        await _uow.CompleteAsync(cancellationToken);


        return Result.Ok(new CreateAccountResponse
        {
            UserId = user.Id,
            Token = jwt,
            RefreshToken = refreshTokenString,
        });
    }
}
