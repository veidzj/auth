using System;

namespace Presentation.Protocols
{
  public interface IDateTimeProvider
  {
    DateTime UtcNow { get; }
  }
}
