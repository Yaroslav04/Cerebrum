using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerebrum.Core.Model
{
    public class ObjectSoketClass : ObjectClass
    {
        public string Tegs { get; set; }
        public ObjectSoketClass( ObjectClass objectClass)
        {
            this.N = objectClass.N;
            this.Description = objectClass.Description;
            this.Identification = objectClass.Identification;
            this.DocumentDate = objectClass.DocumentDate;
            this.Authority = objectClass.Authority;
            this.Type = objectClass.Type;   
            this.Content = objectClass.Content;
            this.SaveDate = objectClass.SaveDate;
        }
    }
}
