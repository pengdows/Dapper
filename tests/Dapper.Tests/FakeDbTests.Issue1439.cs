#if !NET481
using System;
using System.Data;
using System.Threading.Tasks;
using pengdows.crud.fakeDb;
using Xunit;

namespace Dapper.Tests
{
    public sealed class FakeDbIssue1439Tests
    {
        [Fact]
        public void ConcurrentTypeMapUpdatesAreNotLost()
        {
            var types = new[]
            {
                typeof(Marker<byte>), typeof(Marker<sbyte>), typeof(Marker<short>), typeof(Marker<ushort>),
                typeof(Marker<int>), typeof(Marker<uint>), typeof(Marker<long>), typeof(Marker<ulong>),
                typeof(Marker<float>), typeof(Marker<double>), typeof(Marker<decimal>), typeof(Marker<bool>),
                typeof(Marker<char>), typeof(Marker<string>), typeof(Marker<Guid>), typeof(Marker<DateTime>)
            };

            try
            {
                Parallel.ForEach(types, type => SqlMapper.AddTypeMap(type, DbType.String));

                using var conn = new fakeDbConnection(new FakeDataStore());
                foreach (var type in types) conn.EnqueueNonQueryResult(1);
                conn.Open();

                foreach (var type in types)
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("value", Activator.CreateInstance(type));
                    conn.Execute("select @value", parameters);

#pragma warning disable 618
                    Assert.Equal(DbType.String, SqlMapper.LookupDbType(type, "value", true, out _));
#pragma warning restore 618
                }
            }
            finally
            {
                foreach (var type in types) SqlMapper.RemoveTypeMap(type);
            }
        }

        private sealed class Marker<T> { }
    }
}
#endif
