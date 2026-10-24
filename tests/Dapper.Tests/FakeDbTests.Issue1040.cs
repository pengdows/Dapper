#if !NET481
using System.Collections.Generic;
using System.Linq;
using pengdows.crud.fakeDb;
using Xunit;

namespace Dapper.Tests
{
    public sealed class FakeDbIssue1040Tests
    {
        [Fact]
        public void ListExpansionDoesNotReplaceAParameterNamePrefix()
        {
            using var conn = new fakeDbConnection(new FakeDataStore());
            conn.EnqueueNonQueryResult(1);
            conn.Open();

            conn.Execute(
                "select 1 where Value in @values1 or Value in @values11",
                new
                {
                    values1 = new[] { 1, 2 },
                    values11 = new[] { 3, 4 }
                });

            var commandText = Assert.Single(conn.ExecutedNonQueryTexts);
            Assert.Equal(
                "select 1 where Value in (@values11,@values12) or Value in (@values111,@values112)",
                commandText);
        }
    }
}
#endif
