using Domain.Models;
using System.Threading.Tasks;

namespace Domain.Usecases
{
  public interface IAuthenticationInput
  {
    string Email { get; set; }
    string Password { get; set; }
  }

  public interface IAuthentication
  {
    Task<Account> Authenticate(IAuthenticationInput input);
  }
}
