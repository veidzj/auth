namespace Presentation.Protocols
{
  public interface Response
  {
    int StatusCode { get; }
    object Body { get; }
  }
}
