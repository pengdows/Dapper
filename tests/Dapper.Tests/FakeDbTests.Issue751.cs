#if !NET481
using pengdows.crud.fakeDb;
using Xunit;

namespace Dapper.Tests
{
    public sealed class FakeDbIssue751Tests
    {
        [Fact]
        public void IsConsumedIsTrueWhenReaderHasNoResultSets()
        {
            using var conn = new fakeDbConnection(new FakeDataStore());
            conn.Open();

            using var grid = conn.QueryMultiple("exec NoResultSets");

            Assert.True(grid.IsConsumed);
        }
    }
}
#endif
