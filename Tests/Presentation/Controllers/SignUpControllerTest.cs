using Bogus;
using Domain.Errors;
using Domain.Models;
using Domain.Usecases;
using Moq;
using NUnit.Framework;
using Presentation.Controllers;
using Presentation.Exceptions;
using Presentation.Implementations;
using Presentation.Protocols;
using System;
using System.Threading.Tasks;
using Tests.Domain.Mocks;
using ValidationException = Domain.Errors.ValidationException;

namespace Tests.Presentation.Controllers
{
  [TestFixture]
  public class SignUpControllerTest
  {
    private Faker faker;
    private Mock<IValidation> validationMock;
    private Mock<IAddAccount> addAccountMock;
    private Mock<IDateTimeProvider> dateTimeProviderMock;
    private Mock<IAuthentication> authenticationMock;
    private SignUpController sut;
    private SignUpControllerRequest request;
    private Account account;

    private SignUpControllerRequest MockRequest()
    {
      request = new()
      {
        UserName = faker.Internet.UserName(),
        Email = faker.Internet.Email(),
        Password = faker.Internet.Password()
      };
      return request;
    }

    [SetUp]
    public void Setup()
    {
      faker = new Faker();
      validationMock = new Mock<IValidation>();
      addAccountMock = new Mock<IAddAccount>();
      dateTimeProviderMock = new Mock<IDateTimeProvider>();
      authenticationMock = new Mock<IAuthentication>();
      account = MockAccount.Mock();
      request = MockRequest();
      authenticationMock.Setup(authentication => authentication.Authenticate(It.IsAny<IAuthenticationInput>())).ReturnsAsync(account);
      sut = new SignUpController(validationMock.Object, addAccountMock.Object, dateTimeProviderMock.Object, authenticationMock.Object);
    }

    [Test]
    public async Task ShouldCallValidationWithCorrectValues()
    {
      await sut.Handle(request);
      validationMock.Verify(validation => validation.Validate(It.Is<object>(obj => obj == request)), Times.Once);
    }

    [Test]
    public async Task ShouldReturnBadRequestIfValidationThrows()
    {
      string exceptionMessage = faker.Random.Word();
      validationMock.Setup(validation => validation.Validate(It.IsAny<object>())).Throws(new ValidationException(exceptionMessage));
      IResponse response = await sut.Handle(request);
      Assert.Multiple(() =>
      {
        Assert.That(response.StatusCode, Is.EqualTo(400));
        Assert.That(response.Body, Is.InstanceOf<ValidationException>());
        ValidationException? exception = response.Body as ValidationException;
        Assert.That(exception?.Message, Is.EqualTo(exceptionMessage));
      });
    }

    [Test]
    public async Task ShouldCallAddAccountWithCorrectValues()
    {
      DateTime fakeDate = faker.Date.Recent();
      dateTimeProviderMock.Setup(dateTimeProvider => dateTimeProvider.UtcNow).Returns(fakeDate);
      IAddAccountInput addAccountInput = new AddAccountInput()
      {
        UserName = request.UserName!,
        Email = request.Email!,
        Password = request.Password!,
        AddedAt = fakeDate
      };
      await sut.Handle(request);
      addAccountMock.Verify(addAccount => addAccount.Add(It.Is<IAddAccountInput>(input =>
        input.UserName == request.UserName &&
        input.Email == request.Email &&
        input.Password == request.Password &&
        input.AddedAt == fakeDate
      )), Times.Once);
    }

    [Test]
    public async Task ShouldReturnConflictIfAddAccountThrowsEmailInUseException()
    {
      string exceptionMessage = faker.Random.Word();
      addAccountMock.Setup(addAccount => addAccount.Add(It.IsAny<IAddAccountInput>())).Throws(new EmailInUseException(exceptionMessage));
      IResponse response = await sut.Handle(request);
      Assert.Multiple(() =>
      {
        Assert.That(response.StatusCode, Is.EqualTo(409));
        Assert.That(response.Body, Is.InstanceOf<EmailInUseException>());
        EmailInUseException? exception = response.Body as EmailInUseException;
        Assert.That(exception?.Message, Is.EqualTo(exceptionMessage));
      });
    }

    [Test]
    public async Task ShouldReturnInternalServerErrorIfAddAccountThrows()
    {
      addAccountMock.Setup(addAccount => addAccount.Add(It.IsAny<IAddAccountInput>())).Throws(new Exception());
      IResponse response = await sut.Handle(request);
      Assert.Multiple(() =>
      {
        Assert.That(response.StatusCode, Is.EqualTo(500));
        Assert.That(response.Body, Is.InstanceOf<ServerException>());
        ServerException? exception = response.Body as ServerException;
        Assert.That(exception?.Message, Is.EqualTo("Internal Server Error"));
      });
    }

    [Test]
    public async Task ShouldCallAuthenticationWithCorrectValues()
    {
      IAuthenticationInput authenticationInput = new AuthenticationInput()
      {
        Email = request.Email!,
        Password = request.Password!
      };
      await sut.Handle(request);
      authenticationMock.Verify(authentication => authentication.Authenticate(It.Is<IAuthenticationInput>(input =>
        input.Email == request.Email &&
        input.Password == request.Password
      )), Times.Once);
    }

    [Test]
    public async Task ShouldReturnUnauthorizedIfAuthenticationThrowsInvalidCredentialsException()
    {
      string exceptionMessage = faker.Random.Word();
      authenticationMock.Setup(authentication => authentication.Authenticate(It.IsAny<IAuthenticationInput>())).Throws(new InvalidCredentialsException(exceptionMessage));
      IResponse response = await sut.Handle(request);
      Assert.Multiple(() =>
      {
        Assert.That(response.StatusCode, Is.EqualTo(401));
        Assert.That(response.Body, Is.InstanceOf<InvalidCredentialsException>());
        InvalidCredentialsException? exception = response.Body as InvalidCredentialsException;
        Assert.That(exception?.Message, Is.EqualTo(exceptionMessage));
      });
    }

    [Test]
    public async Task ShouldReturnInternalServerErrorIfAuthenticationThrows()
    {
      authenticationMock.Setup(authentication => authentication.Authenticate(It.IsAny<IAuthenticationInput>())).Throws(new Exception());
      IResponse response = await sut.Handle(request);
      Assert.Multiple(() =>
      {
        Assert.That(response.StatusCode, Is.EqualTo(500));
        Assert.That(response.Body, Is.InstanceOf<ServerException>());
        ServerException? exception = response.Body as ServerException;
        Assert.That(exception?.Message, Is.EqualTo("Internal Server Error"));
      });
    }

    [Test]
    public async Task ShouldReturnOkOnSuccess()
    {
      IResponse response = await sut.Handle(request);
      Assert.Multiple(() =>
      {
        Assert.That(response.StatusCode, Is.EqualTo(200));
        Assert.That(response.Body, Is.InstanceOf<Account>());
        Account? responseAccount = response.Body as Account;
        Assert.That(responseAccount?.UserName, Is.EqualTo(account.UserName));
        Assert.That(responseAccount?.AccessToken, Is.EqualTo(account.AccessToken));
      });
    }
  }
}
