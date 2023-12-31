﻿using Domain.Errors;
using Domain.Models;
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

  public class SignUpController(
    IValidation validation,
    IAddAccount addAccount,
    IDateTimeProvider dateTimeProvider,
    IAuthentication authentication
  ) : IController<SignUpController, SignUpControllerRequest>
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
          Password = request.Password!,
          AddedAt = dateTimeProvider.UtcNow
        };
        await addAccount.Add(addAccountInput);
        IAuthenticationInput authenticationInput = new AuthenticationInput()
        {
          Email = request.Email!,
          Password = request.Password!
        };
        Account account = await authentication.Authenticate(authenticationInput);
        return HttpHelper.Ok(account);
      }
      catch (ValidationException validationException)
      {
        return HttpHelper.BadRequest(validationException);
      }
      catch (EmailInUseException emailInUseException)
      {
        return HttpHelper.Conflict(emailInUseException);
      }
      catch (InvalidCredentialsException invalidCredentialsException)
      {
        return HttpHelper.Unauthorized(invalidCredentialsException);
      }
      catch (Exception)
      {
        return HttpHelper.InternalServerError();
      }
    }
  }
}
