namespace Presentation.Protocols
{
  public interface IResponse
  {
    int StatusCode { get; set; }
    object Body { get; set; }
  }
}
