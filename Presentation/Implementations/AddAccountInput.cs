using Domain.Usecases;
using System;

namespace Presentation.Implementations
{
  public class AddAccountInput : IAddAccountInput
  {
    public required string UserName { get; set; }

    public required string Email { get; set; }

    public required string Password { get; set; }
    public required DateTime AddedAt { get; set; }
  }
}
