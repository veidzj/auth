﻿using Bogus;
using Domain.Errors;
using Domain.Usecases;
using Moq;
using NUnit.Framework;
using Presentation.Controllers;
using Presentation.Implementations;
using Presentation.Protocols;
using System;
using System.Threading.Tasks;
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
    private SignUpController sut;

    private SignUpControllerRequest MockRequest()
    {
      SignUpControllerRequest request = new()
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
      sut = new SignUpController(validationMock.Object, addAccountMock.Object, dateTimeProviderMock.Object);
    }

    [Test]
    public async Task ShouldCallValidationWithCorrectValues()
    {
      SignUpControllerRequest request = MockRequest();
      await sut.Handle(request);
      validationMock.Verify(v => v.Validate(It.Is<object>(obj => obj == request)), Times.Once);
    }

    [Test]
    public async Task ShouldReturnBadRequestIfValidationThrows()
    {
      SignUpControllerRequest request = MockRequest();
      string exceptionMessage = faker.Random.Word();
      validationMock.Setup(v => v.Validate(It.IsAny<object>())).Throws(new ValidationException(exceptionMessage));
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
      SignUpControllerRequest request = MockRequest();
      DateTime fakeDate = faker.Date.Recent();
      dateTimeProviderMock.Setup(m => m.UtcNow).Returns(fakeDate);
      IAddAccountInput addAccountInput = new AddAccountInput()
      {
        UserName = request.UserName!,
        Email = request.Email!,
        Password = request.Password!,
        AddedAt = fakeDate
      };
      await sut.Handle(request);
      addAccountMock.Verify(v => v.Add(It.Is<IAddAccountInput>(input =>
        input.UserName == request.UserName &&
        input.Email == request.Email &&
        input.Password == request.Password &&
        input.AddedAt == fakeDate
      )), Times.Once);
    }

    [Test]
    public async Task ShouldReturnConflictIfAddAccountThrowsEmailInUseException()
    {
      SignUpControllerRequest request = MockRequest();
      string exceptionMessage = faker.Random.Word();
      addAccountMock.Setup(a => a.Add(It.IsAny<IAddAccountInput>())).Throws(new EmailInUseException(exceptionMessage));
      IResponse response = await sut.Handle(request);
      Assert.Multiple(() =>
      {
        Assert.That(response.StatusCode, Is.EqualTo(409));
        Assert.That(response.Body, Is.InstanceOf<EmailInUseException>());
        EmailInUseException? exception = response.Body as EmailInUseException;
        Assert.That(exception?.Message, Is.EqualTo(exceptionMessage));
      });
    }
  }
}
