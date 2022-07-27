using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmbroideryCreator
{
    public class DmcColor
    {
        string number;
        string name;
        int r;
        int g;
        int b;
        string hex;

        public DmcColor(string number, string name, int r, int g, int b, string hex)
        {
            this.number = number;
            this.name = name;
            this.r = r;
            this.g = g;
            this.b = b;
            this.hex = hex;
        }
    }
}
