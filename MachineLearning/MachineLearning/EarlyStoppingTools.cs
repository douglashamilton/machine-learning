using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Neuro;

namespace MachineLearning
{
    class EarlyStoppingTools
    {

        private int patience;
        private List<double> Errors = new List<double>();
        
        public EarlyStoppingTools(int p)
        {
            patience = p;

            for (int i = 0; i < p; ++i)
            {
                Errors.Add(Double.MaxValue);
            }

        }

        public bool ExceedsPatience(double error)
        {
            Errors.Add(error);
            Errors.RemoveAt(0);

            if (Errors.Last() > Errors.First())
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public static double RssError(ActivationNetwork network, double[] input, double[] TargetOutput)
        {
            double NetError = 0.0;
            double[] output = network.Compute(input);
            //Console.WriteLine("Target: " + String.Join("," , TargetOutput));
            //Console.WriteLine("Output: " + String.Join("," , output));

            for (int i = 0; i < output.Count(); ++i)
            {
                NetError += Math.Pow(TargetOutput[i] - output[i], 2);
            }
            return NetError / output.Count();
        }


        public static double RssError(ActivationNetwork network, double[][] inputs, double[][] TargetOutputs)
        {
            int n = 0;
            double NetError = 0.0;

            foreach (double[] input in inputs)
            {
                NetError += RssError(network, input, TargetOutputs[n]);  
                ++n;
            }
            NetError /= n;

            return NetError;
        }



    }
}
