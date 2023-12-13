using System;

namespace Domain.Errors
{
  public class ValidationException : Exception
  {
    public ValidationException(string errorMessage) : base(errorMessage)
    {
      this.Source = GetType().Name;
    }
  }
}
