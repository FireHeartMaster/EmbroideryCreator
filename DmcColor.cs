using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmbroideryCreator
{
    public class DmcColor
    {
        public string Number { get; private set; }
        public string Name {get; private set;}
        public int R {get; private set;}
        public int G {get; private set;}
        public int B {get; private set;}
        public string Hex {get; private set;}

        public DmcColor(string number, string name, int r, int g, int b, string hex)
        {
            this.Number = number;
            this.Name = name;
            this.R = r;
            this.G = g;
            this.B = b;
            this.Hex = hex;
        }
    }
}
