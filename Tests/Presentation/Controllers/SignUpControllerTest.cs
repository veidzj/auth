using Bogus;
using Moq;
using NUnit.Framework;
using Presentation.Controllers;
using Presentation.Protocols;

namespace Tests.Presentation.Controllers
{
  [TestFixture]
  public class SignUpControllerTest
  {
    private Faker faker;
    private Mock<Validation> validationMock;
    private SignUpController sut;

    private SignUpControllerRequest MockRequest()
    {
      SignUpControllerRequest request = new()
      {
        Username = faker.Internet.UserName(),
        Email = faker.Internet.Email(),
        Password = faker.Internet.Password()
      };
      return request;
    }

    [SetUp]
    public void Setup()
    {
      faker = new Faker();
      validationMock = new Mock<Validation>();
      sut = new SignUpController(validationMock.Object);
    }

    [Test]
    public void ShouldCallValidationWithCorrectValues()
    {
      SignUpControllerRequest request = MockRequest();
      sut.Handle(request);
      validationMock.Verify(v => v.Validate(It.Is<object>(obj => obj == request)), Times.Once);
    }
  }
}
