using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithm.Algorithm
{
    class PregnancyAlgorithm<Ind> : GeneticAlgorithm<double[]>
    {

        private double[] parentOne;
        private double[] parentTwo;

        List<int[]> DataInDataset = new List<int[]>();
        int lengthOfLine = -1;

        public PregnancyAlgorithm(double crossoverRate, double mutationRate, bool elitism, int populationSize, int numIterations)
            : base(crossoverRate, mutationRate, elitism, populationSize, numIterations, "Pregnancy")
        {
            this.DataInDataset = ReadCsv("datasetfig6-7.csv");
        }

        private List<int[]> ReadCsv(string fileName)
        {
            List<int[]> DataInDataset = new List<int[]>();

            string dir = Directory.GetCurrentDirectory() + "\\" + fileName;
            var reader = new StreamReader(File.OpenRead(dir));
            List<string> listA = new List<string>();
            List<string> listB = new List<string>();
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                string[] values = line.Split(',');
                lengthOfLine = lengthOfLine == -1 ? values.Length : lengthOfLine;

                int[] parsedValues = new int[values.Length];

                int iterator = 0;
                foreach (string value in values)
                {
                    int parsedValue;
                    bool isNumeric = int.TryParse(value, out parsedValue);

                    if (isNumeric)
                    {
                        parsedValues[iterator] = parsedValue;
                    }

                    iterator++;
                }

                bool allValuesNull = true;
                foreach (int parsedValue in parsedValues)
                {
                    if (parsedValue != 0)
                    {
                        allValuesNull = false;
                        break;
                    }
                }

                if(!allValuesNull)
                    DataInDataset.Add(parsedValues);
            }

            return DataInDataset;
        }


        /// <summary>
        /// Generates a binary string
        /// </summary>
        /// <returns>the binarystring</returns>
        public double[] GeneratePopulation()
        {
            int minValue = -1;
            int maxValue = 1;

            double[] population = new double[lengthOfLine];
            
            for (int i = 0; i < lengthOfLine; i++) 
            {
                population[i] = r.NextDouble() * (maxValue - minValue) + minValue;
            }
            
            return population;
        }

        public double ComputeFitness(double[] individual)
        {
            double fitness = 0;

            double[] squaredErrors = new double[DataInDataset.Count];
            int iterator = 0;

            foreach (int[] lineInDataSet in DataInDataset)
            {
                double sumProduct = 0;

                for (int i = 0; i < lineInDataSet.Length-1; i++)
                {
                    int valueInDataSet  = lineInDataSet[i];
                    double valueIndividual = individual[i];

                    sumProduct += (valueIndividual * valueInDataSet);
                }

                squaredErrors[iterator] = Math.Pow((lineInDataSet[lineInDataSet.Length-1] - sumProduct), 2);
                iterator++;
            }

            foreach (double SSE in squaredErrors)
            {
                fitness += SSE;
            }

            return fitness;
        }

        public Func<Tuple<double[], double[]>> SelectTwoParents(double[][] individuals, double[] fitnesses)
        {
            parentOne = null;
            parentTwo = null;

            double lowestFitness = -1;
            int    idxLowestFitness = -1;
            double secondLowestFitness = -1;
            int    idxSecondLowestFitness = -1;

            for (int i = 0; i < fitnesses.Length; i++)
            {
                if (fitnesses[i] < lowestFitness || lowestFitness == -1)
                {
                    lowestFitness = fitnesses[i];
                    idxLowestFitness = i;
                }
            }

            for (int i = 0; i < fitnesses.Length; i++)
            {
                if (i == idxLowestFitness)
                {
                    continue;
                }

                if (fitnesses[i] < secondLowestFitness || secondLowestFitness == -1)
                {
                    secondLowestFitness = fitnesses[i];
                    idxSecondLowestFitness = i;
                }
            }

            parentOne = individuals[idxLowestFitness];
            parentTwo = individuals[idxSecondLowestFitness];

            return selectParent;
        }

        private Tuple<double[], double[]> selectParent()
        {
            return new Tuple<double[], double[]>(parentOne, parentTwo);
        }

        public Tuple<double[], double[]> CrossOver(Tuple<double[], double[]> parents)
        {
            Tuple<double[], double[]> newParents;
            int lowestCrossOver = 0;
            int secondCrossOver = 0;

            lowestCrossOver = getCrossOverValue(1, parents.Item1.Length-2);
            secondCrossOver = getCrossOverValue(lowestCrossOver+1, parents.Item1.Length-1);
            
            double[] newParentOne;
            double[] newParentTwo;

            var parentOneList = parents.Item1.ToList();
            var parentTwoList = parents.Item2.ToList();

            double[] firstPartParentOne  = parentOneList.GetRange(0, lowestCrossOver).ToArray();
            double[] secondPartParentOne = parentTwoList.GetRange(lowestCrossOver, secondCrossOver - lowestCrossOver).ToArray();
            double[] thirdPartParentOne  = parentOneList.GetRange(secondCrossOver, parentOneList.Count - secondCrossOver).ToArray();
            newParentOne = firstPartParentOne.Concat(secondPartParentOne).Concat(thirdPartParentOne).ToArray();

            double[] firstPartParentTwo  = parentTwoList.GetRange(0, lowestCrossOver).ToArray();
            double[] secondPartParentTwo = parentOneList.GetRange(lowestCrossOver, secondCrossOver - lowestCrossOver).ToArray(); ;
            double[] thirdPartParentTwo  = parentTwoList.GetRange(secondCrossOver, parentTwoList.Count - secondCrossOver).ToArray(); ;
            newParentTwo = firstPartParentTwo.Concat(secondPartParentTwo).Concat(thirdPartParentTwo).ToArray();

            newParents = new Tuple<double[], double[]>(newParentOne, newParentTwo);
            return newParents;
        }
        
        public double[] Mutation(double[] individual, double mutationRate)
        {
            if (r.NextDouble() < mutationRate)
            {
                List<int> locOfMutatedValues = new List<int>();
                int numberOfMutations = r.Next(individual.Length / 2);

                for (int i = 0; i < numberOfMutations; i++)
                {
                    int location = r.Next(individual.Length);

                    while (!locOfMutatedValues.Contains(location))
                    {
                        location = r.Next(individual.Length);
                        locOfMutatedValues.Add(location);
                    }
                }


                foreach (int loc in locOfMutatedValues)
                {
                    individual[loc] = individual[loc] == 0 ? 1 : 0;
                }

                return individual;
            }

            return individual;
        }
    }
}
