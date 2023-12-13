using System;

namespace Domain.Errors
{
  public class EmailInUseException : Exception
  {
    public EmailInUseException(string errorMessage) : base(errorMessage)
    {
      this.Source = GetType().Name;
    }
  }
}
