using System.Threading.Tasks;

namespace Presentation.Protocols
{
  public interface IController<Controller, Request>
  {
    Task<IResponse> Handle(Request request);
  }
}
