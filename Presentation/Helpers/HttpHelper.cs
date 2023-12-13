using Presentation.Exceptions;
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

    public static IResponse Conflict(Exception exception)
    {
      return new Response()
      {
        StatusCode = 409,
        Body = exception
      };
    }

    public static IResponse InternalServerError()
    {
      return new Response()
      {
        StatusCode = 500,
        Body = new ServerException()
      };
    }
  }
}
