namespace Provider.Indexes
{
    public class Index
    {
        public static Index Wibor3M = new Index("PLOPLN3M");
        public static Index Wibor6M = new Index("PLOPLN6M");
        private readonly string _name;

        private Index(string name)
        {
            _name = name;
        }

        public static explicit operator string(Index @this) => @this._name;
    }
}