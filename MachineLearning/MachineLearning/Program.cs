using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace MachineLearning
{
    class Program
    {
        static void Main(string[] args)
        {

            KFoldCrossValidation CrossValidation = new KFoldCrossValidation(5);
            List<NetIO> robotData;
            LoadRobotTimeSeries rd = new LoadRobotTimeSeries();
            rd.LoadRobotData("lp4.txt", out robotData);

            CrossValidation.LoadData(robotData);

            CrossValidation.LengthOfOutput = rd.lengthOfOutputData;
            // CrossValidation.LoadData("MUSK.csv");

            CrossValidation.Execute();
            Console.WriteLine("-----------------");
            Console.WriteLine("Mean error: {0:0.000}", CrossValidation.meanError);
            Console.WriteLine("Mean epochs: {0:0}", CrossValidation.meanEpochs);

            // change the topology and learning rate

        }
    }
}
