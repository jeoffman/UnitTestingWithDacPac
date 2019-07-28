using SimpleDbModelEntities;
using System;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace DacPacUnitTests
{
    [Trait("localdb", "true")]
    public class SomethingThatRequiresDatabaseTest : IClassFixture<DacpacDbFixture>, IDisposable
    {
        private readonly ITestOutputHelper _output;
        public string _connectionString;
        public string _efConnectionString;

        public SomethingThatRequiresDatabaseTest(ITestOutputHelper output, DacpacDbFixture fixture)
        {
            _output = output;
            _connectionString = fixture.ConnectionString;
            _efConnectionString = fixture.EfConnectionString;
        }

        public void Dispose()
        {
            _connectionString = null;
        }

        void ResetDataForNextTest()
        {
            using (var context = SimpleDbEntities.CreateContext(_efConnectionString))
            {
                context.SimpleTables.RemoveRange(context.SimpleTables);
                context.SaveChanges();
                //context.Database.ExecuteSqlCommand("DELETE FROM SimpleTable");
            }
        }

        [Fact]
        private void SimpleInsertSprocTest()
        {
            string newText = "Text 1";
            int newInt = 1;

            #region ARRANGE
            using (var context = SimpleDbEntities.CreateContext(_efConnectionString))
            {
                ResetDataForNextTest();

                //use EF
                context.SimpleTables.Add(new SimpleTable
                {
                    SimpleText = newText,
                    SimpleInt = newInt,
                });
                context.SaveChanges();

                //use SQL
                context.Database.ExecuteSqlCommand("INSERT INTO SimpleTable (SimpleText,SimpleInt) VALUES (@p0, @p1)", "Blah", 2);
            }
            #endregion ARRANGE

            #region ACT
            using (var context = SimpleDbEntities.CreateContext(_efConnectionString))
            {
                //use SPROC
                context.SimpleInsertSproc("SPROC BLAH", 3);
            }
            #endregion ACT

            #region ASSERT
            using (var context = SimpleDbEntities.CreateContext(_efConnectionString))
            {
                Assert.Equal(3, context.SimpleTables.Count());
                var item = context.SimpleTables.First();
                Assert.Equal(newText, item.SimpleText);
                Assert.Equal(newInt, item.SimpleInt);
            }
            #endregion ASSERT
        }
    }
}
