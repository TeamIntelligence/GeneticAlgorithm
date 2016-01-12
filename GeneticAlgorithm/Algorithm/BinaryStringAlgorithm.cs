using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithm.Algorithm
{
    class BinaryStringAlgorithm<Ind> : GeneticAlgorithm<string>
    {

        private string parentOne;
        private string parentTwo;

        public BinaryStringAlgorithm(double crossoverRate, double mutationRate, bool elitism, int populationSize, int numIterations)
            : base(crossoverRate, mutationRate, elitism, populationSize, numIterations, "BinaryString"){ }

        /// <summary>
        /// Generates a binary string
        /// </summary>
        /// <returns>the binarystring</returns>
        public string GeneratePopulation()
        {
            string binaryString = "";


            for (int i = 0; i < 20; i++)
            {
                binaryString = binaryString + r.Next(0, 2);
            }

            return binaryString;
        }

        public double ComputeFitness(string binarySting)
        {
            double fitness = 0;

            foreach (char valueChar in binarySting)
            {
                if (valueChar.Equals('1'))
                {
                    fitness++;
                }
            }

            return fitness;
        }

        public Func<Tuple<string, string>> SelectTwoParents(string[] binaryStrings, double[] fitnesses)
        {
            parentOne = null;
            parentTwo = null;

            for (int i = 0; i < fitnesses.Length; i++)
            {
                double convFitness = fitnesses[i] / 20;

                if (convFitness >= r.NextDouble())
                {
                    if (parentOne == null)
                    {
                        parentOne = binaryStrings[i];
                    }
                    else if (parentTwo == null)
                    {
                        parentTwo = binaryStrings[i];
                        break;
                    }
                }
            }

            return selectParent;
        }

        private Tuple<string, string> selectParent()
        {
            return new Tuple<string, string>(parentOne, parentTwo);
        }

        public Tuple<string, string> CrossOver(Tuple<string, string> parents)
        {
            Tuple<string, string> newParents;
            int crossOver = 0;
            crossOver = getCrossOverValue(1, parents.Item1.Length-1);

            string newParentOne = "";
            string newParentTwo = "";

            newParentOne = parents.Item1.Substring(0, crossOver) + parents.Item2.Substring(crossOver, parents.Item2.Length - crossOver);
            newParentTwo = parents.Item2.Substring(0, crossOver) + parents.Item1.Substring(crossOver, parents.Item1.Length - crossOver);


            newParents = new Tuple<string, string>(newParentOne, newParentTwo);
            return newParents;
        }

        public string Mutation(string binaryString, double mutationRate)
        {
            if (r.NextDouble() < mutationRate)
            {

                List<int> locOfMutatedValues = new List<int>();
                int numberOfMutations = r.Next(binaryString.Length / 2);

                for (int i = 0; i < numberOfMutations; i++)
                {
                    int location = r.Next(binaryString.Length);

                    while (!locOfMutatedValues.Contains(location))
                    {
                        location = r.Next(binaryString.Length);
                        locOfMutatedValues.Add(location);
                    }
                }


                StringBuilder sb = new StringBuilder(binaryString);
                foreach (int loc in locOfMutatedValues)
                {
                    sb[loc] = sb[loc] == '0' ? '1' : '0';
                }

                return sb.ToString();
            }

            return binaryString;
        }
    }
}
