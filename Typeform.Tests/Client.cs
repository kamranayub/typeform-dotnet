using Xunit;

namespace Typeform.Tests;

public class ClientTests
{
  [Fact]
  public void Client_Is_Created_Successfully()
  {
    var client = TypeformClient.CreateApi();

    Assert.NotNull(client);
  }
}