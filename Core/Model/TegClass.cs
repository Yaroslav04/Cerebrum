using SQLite;

namespace Cerebrum.Core.Model
{
    public class TegClass
    {
        [AutoIncrement]
        [PrimaryKey]
        [NotNull]
        public int N { get; set; }
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }

    }
}
