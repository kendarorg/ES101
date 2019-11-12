using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es02.Test.Src.Events
{
    public class ItemNameModified
    {

        public Guid Id { get; set; }
        public string NewName { get; set; }

        public ItemNameModified(Guid id, string newName)
        {
            Id = id;
            NewName = newName;
        }

    }
}
