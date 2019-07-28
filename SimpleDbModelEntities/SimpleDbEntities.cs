namespace SimpleDbModelEntities
{
    public partial class SimpleDbEntities
    {
        private SimpleDbEntities(string efConnectionString) : base(efConnectionString)
        {
            //Maybe just make this public, if you care
        }

        public static SimpleDbEntities CreateContext(string efConnectionString)
        {
            return new SimpleDbEntities(efConnectionString);
        }
    }
}
