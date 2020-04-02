using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoXML
{
    class Serie
    {
        public string Nome { get; set; }
        public int Stagioni { get; set; }
        public int Episodi { get; set; }

        public override string ToString()
        {
            return Nome;
        }
    }
}
