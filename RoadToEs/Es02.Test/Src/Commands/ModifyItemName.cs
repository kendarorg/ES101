using System;

namespace Es02.Test.Src.Commands
{
    public class ModifyItemName
    {
        public Guid Id { get; set; }
        public string NewName { get; set; }

        public ModifyItemName(Guid id, string newName)
        {
            Id = id;
            NewName = newName;
        }
    }
}
