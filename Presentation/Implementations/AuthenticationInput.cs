using Domain.Usecases;

namespace Presentation.Implementations
{
  public class AuthenticationInput : IAuthenticationInput
  {
    public required string Email { get; set; }

    public required string Password { get; set; }
  }
}
