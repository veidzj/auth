using Presentation.Implementations;
using Presentation.Protocols;
using System;

namespace Presentation.Helpers
{
  public static class HttpHelper
  {
    public static IResponse BadRequest(Exception exception)
    {
      return new Response()
      {
        StatusCode = 400,
        Body = exception
      };
    }
  }
}
