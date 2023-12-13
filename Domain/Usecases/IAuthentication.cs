using Domain.Models;
using System.Threading.Tasks;

namespace Domain.Usecases
{
  public interface IAuthenticationInput
  {
    string Email { get; }
    string Password { get; }
  }

  public interface IAuthentication
  {
    Task<Account> Authenticate(IAuthenticationInput input);
  }
}
