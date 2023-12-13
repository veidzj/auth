using Moq;
using NUnit.Framework;
using Presentation.Controllers;
using Presentation.Protocols;

namespace Tests.Presentation.Controllers
{
  [TestFixture]
  public class SignUpControllerTest
  {
    private Mock<Validation> validationMock;
    private SignUpController sut;

    [SetUp]
    public void Setup()
    {
      validationMock = new Mock<Validation>();
      sut = new SignUpController(validationMock.Object);
    }

    [Test]
    public void ShouldCallValidationWithCorrectValues()
    {
      SignUpControllerRequest request = new()
      {
        Username = "any_username",
        Email = "any_email@mail.com",
        Password = "any_password"
      };
      sut.Handle(request);
      validationMock.Verify(v => v.Validate(It.Is<object>(obj => obj == request)), Times.Once);
    }
  }
}
