using System;

namespace Data.Characteristics
{
    public class DataName: Attribute
    {
        private string _name;

        public string Name => "/" + _name;

        public DataName(string name)
        {
            _name = name;
        }

    }
}
