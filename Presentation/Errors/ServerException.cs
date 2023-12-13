using System;

namespace Domain.Errors
{
  public class ServerException : Exception
  {
    public ServerException() : base("Internal Server Error")
    {
      this.Source = GetType().Name;
    }
  }
}
