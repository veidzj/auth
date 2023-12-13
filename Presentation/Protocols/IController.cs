using System.Threading.Tasks;

namespace Presentation.Protocols
{
  public interface IController<C, R>
  {
    Task<IResponse> Handle(R request);
  }
}
