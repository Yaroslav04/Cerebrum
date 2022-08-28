using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerebrum.Core.Model
{
    public class ConfigClass
    {
        [AutoIncrement]
        [PrimaryKey]
        [NotNull]
        public int N { get; set; }
        public int Id { get; set; }
        DateTime SaveDate { get; set; }
    }
}
