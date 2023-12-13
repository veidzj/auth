namespace Presentation.Protocols
{
  public interface IResponse
  {
    int StatusCode { get; }
    object Body { get; }
  }
}
