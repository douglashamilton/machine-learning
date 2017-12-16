using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Accord.Neuro;
using Accord.Neuro.Learning;
using System.Threading.Tasks;

namespace MachineLearning
{
    class NetworkTraining
    {
        public double LearningRate;
        private int NumberOfNeurons;
        public List<NetIO> FormattedData;

        private int patience = 10;

        public int NumberOfEpochs;
        public double RssError { get; private set; }

        public NetworkTraining()
        {
                       
        }

        public void SetNumberOfNeurons(int numNeurons)
        {
            this.NumberOfNeurons = numNeurons;
        }

        private int NumberOfHiddenLayerNeurons(int NumInputs, int NumOutputs)
        {
            if (this.NumberOfNeurons == 0)
            {
                //int[] output = new int[1];
                return (int)((NumInputs + NumOutputs) * 0.5 + 0.5);
                //return output;
            }
            else
            {
                return NumberOfNeurons;
            }
        }

        public void ExecuteFold(int k)
        {

            int LengthOfInput = this.FormattedData[0].Input.Count();
            int LengthOfOutput = this.FormattedData[0].Output.Count();

            ActivationNetwork NeuralNetwork = new ActivationNetwork(
                new SigmoidFunction(2),
                LengthOfInput, 
                this.NumberOfHiddenLayerNeurons(LengthOfInput, LengthOfOutput),
                LengthOfOutput);

            NguyenWidrow weights = new NguyenWidrow(NeuralNetwork);
            weights.Randomize();

            ResilientBackpropagationLearning BackProp = new ResilientBackpropagationLearning(NeuralNetwork);
            BackProp.LearningRate = this.LearningRate;
            //BackProp.Momentum = 0.5;

            List<NetIO> TrainingData = new List<NetIO>();
            List<NetIO> ValidationData = new List<NetIO>();

            ReadWrite.RemoveKFold(this.FormattedData, ref TrainingData, ref ValidationData, k);

            // for each epoch
            int epoch = 0;
            int maxEpochs = int.MaxValue;
            EarlyStoppingTools netError = new EarlyStoppingTools(this.patience);

            do
            {
                ++epoch;

                double internalError = BackProp.RunEpoch(TrainingData.Select(l => l.Input.ToArray()).ToArray(),
                                TrainingData.Select(l => l.Output.ToArray()).ToArray());

                this.RssError = EarlyStoppingTools.RssError(NeuralNetwork,
                    ValidationData.Select(l => l.Input.ToArray()).ToArray(),
                    ValidationData.Select(l => l.Output.ToArray()).ToArray());

                //Console.WriteLine("Epochs: " + epoch);
                //Console.WriteLine("Training error: " + internalError);
                //Console.WriteLine("CV Error: " + this.RssError);

            } while (!netError.ExceedsPatience(RssError) && epoch < maxEpochs);

            Console.Write("Target: ");
            ValidationData[0].Output.ForEach(i => Console.Write(i));
            Console.WriteLine();
            Console.WriteLine("Result: " + string.Join("," , NeuralNetwork.Compute(ValidationData[0].Input.ToArray())));            

            this.NumberOfEpochs = epoch;

            Console.WriteLine("Epochs required: " + epoch);
            Console.WriteLine("Error: " + RssError);
        }

    }
}
