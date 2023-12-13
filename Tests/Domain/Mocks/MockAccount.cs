using Bogus;
using Domain.Models;

namespace Tests.Domain.Mocks
{
  public static class MockAccount
  {
    private static readonly Faker faker = new();

    public static Account Mock()
    {
      return new Account()
      {
        UserName = faker.Internet.UserName(),
        AccessToken = faker.Random.Uuid().ToString()
      };
    }
  }
}
