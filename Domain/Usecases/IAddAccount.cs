using System.Threading.Tasks;

namespace Domain.Usecases
{
  public interface IAddAccountInput
  {
    string UserName { get; }
    string Email { get; }
    string Password { get; }
  }

  public interface IAddAccount
  {
    Task Add(IAddAccountInput input);
  }
}
