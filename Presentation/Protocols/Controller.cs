using System.Threading.Tasks;

namespace Presentation.Protocols
{
  public interface Controller<T>
  {
    Task<Response> Handle(T request);
  }
}
