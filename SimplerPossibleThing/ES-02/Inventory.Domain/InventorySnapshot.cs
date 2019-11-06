using Crud;
using Es.Lib;
using System;

namespace Inventory.Domain
{
    public class InventorySnapshot : ISnapshot
    {
        public Guid Id { get; set; }
        public bool Activated { get; set; }
        public int Items { get; set; }
        public string Name { get; set; }
        public int Version { get; set; }
    }
}