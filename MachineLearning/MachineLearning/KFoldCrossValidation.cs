using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MachineLearning
{
    class KFoldCrossValidation
    {
        public int LengthOfOutput;
        private uint kFolds;

        private List<double> RssErrors = new List<double>();
        public double meanError { get; private set; } = 0;     
        private List<int> Epochs = new List<int>();
        public double meanEpochs { get; private set; } = 0;

        private List<NetIO> FormattedData;        

        public KFoldCrossValidation(uint kNummberOfFolds)
        {
            this.kFolds = kNummberOfFolds;
        }

        public void LoadData(List<NetIO> data)
        {
            var rnd = new Random();
            ReadWrite.ShuffleList(ref data, rnd);
            this.FormattedData = data;
            ReadWrite.NormaliseInputData(ref this.FormattedData);
        }

        public void LoadData(string filepath)
        {
            List<List<double>> LabelledData = new List<List<double>>();
            ReadWrite.LoadCsv(filepath, ref LabelledData);            

            // shuffle csv data
            var rnd = new Random();
            ReadWrite.ShuffleList(ref LabelledData, rnd);

            // format data into structure            
            this.FormattedData = ReadWrite.SplitDataInOut(ref LabelledData, this.LengthOfOutput);

            // normalise the data
            ReadWrite.NormaliseInputData(ref this.FormattedData);

        }

        public void Execute()
        {

            // perform k fold CV            
            
            for (int k = 0; k < kFolds; ++k)
            {

                Console.WriteLine("Fold: " + (k + 1));

                // new class to hold network properties and perform training and return epochs and error
                // also calc ideal num neurons

                NetworkTraining NNTraining = new NetworkTraining();

                NNTraining.LearningRate = 0.01;
                NNTraining.SetNumberOfNeurons(30);

                NNTraining.FormattedData = this.FormattedData;
                NNTraining.ExecuteFold(k);                
                this.RssErrors.Add(NNTraining.RssError);
                this.meanError += NNTraining.RssError;
                this.Epochs.Add(NNTraining.NumberOfEpochs);
                this.meanEpochs += NNTraining.NumberOfEpochs;

            }

            // average validation error across k folds. 
            this.meanError /= kFolds;
            this.meanEpochs /= kFolds;

            //Console.WriteLine("Mean error across k folds: " + meanError);

            // change the topology and learning rate

        }



    }
}
