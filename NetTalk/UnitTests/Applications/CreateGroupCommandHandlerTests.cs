using Xunit;
using Xunit.Categories;

namespace UnitTests.Applications;

[UnitTest]
public class CreateCustomerCommandHandlerTests(NetTalkWriteDbFixture fixture) : IClassFixture<NetTalkWriteDbFixture>
{