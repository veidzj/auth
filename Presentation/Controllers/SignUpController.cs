using Presentation.Protocols;

namespace Presentation.Controllers
{
  public class SignUpControllerRequest
  {
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
  }

  public class SignUpController(Validation validation)
  {
    public void Handle(SignUpControllerRequest request)
    {
      validation.Validate(request);
    }
  }
}
