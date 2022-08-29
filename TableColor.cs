using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmbroideryCreator
{
    public class TableColor
    {
        public string Number { get; protected set; }
        public string Name { get; protected set; }
        public int R { get; protected set; }
        public int G { get; protected set; }
        public int B { get; protected set; }
        public string Hex { get; protected set; }

        public TableColor(string number, string name, int r, int g, int b, string hex)
        {
            this.Number = number;
            this.Name = name;
            this.R = r;
            this.G = g;
            this.B = b;
            this.Hex = hex;
        }
    }

    public class AnchorColor : TableColor
    {
        public AnchorColor(string number, string name, int r, int g, int b, string hex) : base(number, name, r, g, b, hex)
        {

        }
    }

    public class DmcColor : TableColor
    {
        public DmcColor(string number, string name, int r, int g, int b, string hex) : base(number, name, r, g, b, hex)
        {

        }
    }
}
