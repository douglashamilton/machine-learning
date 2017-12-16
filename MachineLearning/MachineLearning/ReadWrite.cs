using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MachineLearning
{
    class ReadWrite
    {

        public static void NormaliseInputData(ref List<NetIO> dataToBeNormalised)
        {
            foreach (NetIO row in dataToBeNormalised)
            {
                double rowMax = row.Input.Max();
                row.Input = row.Input.Select(element => element / rowMax).ToList();
                var a = row.Input;
            }
        }

        public static void RemoveKFold(List<NetIO> Data,
            ref List<NetIO> TrainingData, 
            ref List<NetIO> ValidationData,
            int kFold)
        {

            for (int j = 0; j < Data.Count(); j++)
            {
                if ((j + kFold) % 5 == 0)
                {
                    ValidationData.Add(Data[j]);
                }
                else
                {
                    TrainingData.Add(Data[j]);
                }
            }
        }

        public static void ExtractValidationData(List<List<double>> CsvData,
            ref List<List<double>> TrainingData,
            ref List<List<double>> ValidationData)
        {
            int NumValidationSets = CsvData.Count() / 10;

            ValidationData = CsvData;

            for (int i = 0; i < NumValidationSets; ++i)
            {
                TrainingData.Add(ValidationData.First());
                ValidationData.RemoveAt(0);
            }

        }

        public static void LoadCsv(string filepath, ref List<List<double>> CsvData)
        {
            
            var lines = File.ReadAllLines(filepath);

            foreach (string line in lines)
            {
                CsvData.Add(line.Split(',').Select(x => Convert.ToDouble(x)).ToList());
            }

            //Array.ForEach(lines, line => 0.0);
        }

        

        public static List<NetIO> SplitDataInOut(ref List<List<double>> CsvData , int lengthOfOutput)
        {
            List<NetIO> FormattedData = new List<NetIO>();
            uint i = 0;

            foreach (List<double> row in CsvData)
            {
                NetIO FormattedRow = new NetIO(i++);

                FormattedRow.Input = row.GetRange(0, row.Count() - lengthOfOutput);
                FormattedRow.Output = row.GetRange(row.Count() - lengthOfOutput, lengthOfOutput);              

                FormattedData.Add(FormattedRow);
            }

            return FormattedData;
        }

        public static void ShuffleList(ref List<NetIO> array, Random rnd)
        {
            List<NetIO> TempArray = new List<NetIO>();
            int randomIndex;

            while (array.Count() > 0)
            {
                randomIndex = rnd.Next(array.Count());
                TempArray.Add(array[randomIndex]);
                array.RemoveAt(randomIndex);
            }
            array = TempArray;
        }

        public static void ShuffleList(ref List<List<double>> array, Random rnd)
        {
            List<List<double>> TempArray = new List<List<double>>();
            int randomIndex;

            while (array.Count() > 0)
            {
                randomIndex = rnd.Next(array.Count());
                TempArray.Add(array[randomIndex]);
                array.RemoveAt(randomIndex);
            }

            array = TempArray;

        }
    }
}
