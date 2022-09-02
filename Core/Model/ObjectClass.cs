using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerebrum.Core.Model
{
    public class ObjectClass
    {
        [AutoIncrement]
        [PrimaryKey]
        [NotNull]
        public int N { get; set; }
        public string Description { get; set; }
        public string Identification { get; set; }
        public DateTime DocumentDate { get; set; }
        public string Authority { get; set; }
        public string Type { get; set; }
        public DateTime SaveDate { get; set; }
    }
}
