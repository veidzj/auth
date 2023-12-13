using Domain.Errors;
using Presentation.Helpers;
using Presentation.Protocols;
using System;

namespace Presentation.Controllers
{
  public class SignUpControllerRequest
  {
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
  }

  public class SignUpController(IValidation validation)
  {
    public IResponse Handle(SignUpControllerRequest request)
    {
      try
      {
        validation.Validate(request);
        return HttpHelper.BadRequest(new Exception(""));
      }
      catch (ValidationException validationException)
      {
        return HttpHelper.BadRequest(validationException);
      }
    }
  }
}
