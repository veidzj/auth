using System;
using System.Threading.Tasks;

namespace Domain.Usecases
{
  public interface IAddAccountInput
  {
    string UserName { get; set; }
    string Email { get; set; }
    string Password { get; set; }
    DateTime AddedAt { get; set; }
  }

  public interface IAddAccount
  {
    Task Add(IAddAccountInput input);
  }
}
