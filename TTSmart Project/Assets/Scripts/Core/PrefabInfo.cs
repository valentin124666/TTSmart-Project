using System;
using Settings;

namespace Core
{
    public class PrefabInfo : Attribute
    {
        public Enumerators.NamePrefabAddressable Location { get; }
        
        public PrefabInfo(Enumerators.NamePrefabAddressable location)
        {
            Location = location;
        }
    }
}