#if !NET481
using System.Collections.Generic;
using System.Threading.Tasks;
using pengdows.crud.fakeDb;
using Xunit;

namespace Dapper.Tests;

public sealed class FakeDbIssue2185Tests
{
    [Fact]
    public async Task ExecuteReaderAsyncDisposeAsyncDisposesCommand()
    {
        using var connection = new fakeDbConnection(new FakeDataStore());
        connection.EnqueueReaderResult(new[] { new Dictionary<string, object?> { ["Value"] = 1 } });
        connection.Open();

        await using (var reader = await connection.ExecuteReaderAsync("select 1"))
        {
            Assert.True(await reader.ReadAsync());
        }

        Assert.NotNull(connection.LastCreatedCommand);
        Assert.True(connection.LastCreatedCommand!.WasDisposed);
    }
}
#endif
