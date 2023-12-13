using Presentation.Protocols;
using System;

namespace Presentation.Providers
{
  public class DateTimeProvider : IDateTimeProvider
  {
    public DateTime UtcNow => DateTime.UtcNow;
  }
}
