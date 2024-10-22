namespace Application.Usecases.Account.CreateAccountCommand;

public class CreateAccountResponse
{
    public Guid UserId { get; set; }
    public string Token { get; set; }
    public string RefreshToken { get; set; }
}