using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MachineLearning
{
    class LoadRobotTimeSeries
    {
        public int lengthOfOutputData;

        public bool LoadRobotData(string filepath, out List<NetIO> data)
        {
            List<NetIO> outData = new List<NetIO>();
            NetIO predictorsAndLabel = new NetIO();
             
            // create dictionary of labels and index           
            Dictionary<string, int> labels = new Dictionary<string, int>();
            string currentLabel = null;

            int j = 1;

            // open csv and read lines
            var lines = File.ReadAllLines(filepath);

            foreach (string line in lines)
            {
                // split at tab
                string[] words = line.Split('\t');

                if (words.Length == 1 && words[0] != "")
                {
                    // header - label                    
                    try
                    {
                        currentLabel = words[0];
                        labels.Add(currentLabel, j++);
                        
                    }
                    catch (ArgumentException)
                    {
                        // label already exists in dictionary
                    }

                }
                else if (words.Length > 1)
                {
                    // add values to row vector
                    for (int i = 1; i < words.Length; ++i)
                    {
                        predictorsAndLabel.Input.Add(Convert.ToDouble(words[i]));
                    }                                        
                }
                else if (words.Length == 1 && words[0] == "")
                {
                    // end of instance
                    // add row vector to results
                    if (predictorsAndLabel.Input.Count > 0)
                    {
                        predictorsAndLabel.Output.Add(labels[currentLabel]);
                        outData.Add(predictorsAndLabel);
                        predictorsAndLabel = new NetIO();
                    }
                }
            }

            // add final row vector to results
            predictorsAndLabel.Output.Add(labels[currentLabel]);
            outData.Add(predictorsAndLabel);
            predictorsAndLabel = new NetIO();
            
            // convert dictionary values, 1,2,3 to 100, 010, 001 in NetIO
            foreach (NetIO set in outData)
            {
                List<double> labelArray = new List<double>();
                for (int i = 0; i < labels.Count; ++i)
                {
                    labelArray.Add(0.0);
                }

                labelArray[Convert.ToInt32(set.Output[0]) - 1] = 1.0;
                set.Output = labelArray;
            }

            this.lengthOfOutputData = labels.Count;
            data = outData;
            return true;

        }
    }
}
