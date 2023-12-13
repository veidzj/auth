using Domain.Errors;
using Domain.Usecases;
using Presentation.Helpers;
using Presentation.Implementations;
using Presentation.Protocols;
using System;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
  public class SignUpControllerRequest
  {
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
  }

  public class SignUpController(IValidation validation, IAddAccount addAccount)
  {
    public async Task<IResponse> Handle(SignUpControllerRequest request)
    {
      try
      {
        validation.Validate(request);
        IAddAccountInput addAccountInput = new AddAccountInput()
        {
          UserName = request.UserName!,
          Email = request.Email!,
          Password = request.Password!
        };
        await addAccount.Add(addAccountInput);
        return HttpHelper.BadRequest(new Exception(""));
      }
      catch (ValidationException validationException)
      {
        return HttpHelper.BadRequest(validationException);
      }
    }
  }
}
