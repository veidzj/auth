using System;

namespace Presentation.Exceptions
{
  public class ServerException : Exception
  {
    public ServerException() : base("Internal Server Error")
    {
      this.Source = GetType().Name;
    }
  }
}
