using System;

namespace Domain.Errors
{
  public class InvalidCredentialsException : Exception
  {
    public InvalidCredentialsException(string errorMessage) : base(errorMessage)
    {
      this.Source = GetType().Name;
    }
  }
}
