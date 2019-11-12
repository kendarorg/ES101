using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es02.Test.Src.Commands
{
    public class ModifyItemName
    {
        public Guid Id { get; set; }
        public string NewName { get; set; }
        public int ExpectedVersion { get; set; }

        public ModifyItemName(Guid id, string newName,int expectedVersion)
        {
            Id = id;
            NewName = newName;
            ExpectedVersion = expectedVersion;
        }
    }
}
