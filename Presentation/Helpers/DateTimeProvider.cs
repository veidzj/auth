using Presentation.Protocols;
using System;

namespace Presentation.Helpers
{
  public class DateTimeProvider : IDateTimeProvider
  {
    public DateTime UtcNow => DateTime.UtcNow;
  }
}
