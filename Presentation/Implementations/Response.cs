using Presentation.Protocols;

namespace Presentation.Implementations
{
  public class Response : IResponse
  {
    public required int StatusCode { get; set; }

    public required object Body { get; set; }
  }
}
