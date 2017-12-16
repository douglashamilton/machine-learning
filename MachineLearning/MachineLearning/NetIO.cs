using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearning
{
    class NetIO
    {
        public uint index;
        public List<double> Input;
        public List<double> Output;

        public NetIO(uint i)
        {
            index = i;
            Input = new List<double>();
            Output = new List<double>();
        }

        public NetIO()
        {
            index = 0;
            Input = new List<double>();
            Output = new List<double>();
        }

    }

    
}
