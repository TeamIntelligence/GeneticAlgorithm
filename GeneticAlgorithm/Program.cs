using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneticAlgorithm.Algorithm;

namespace GeneticAlgorithm
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Console.WriteLine("Solution MaxOne: ");
            BinaryStringAlgorithm<string> maxOne = new BinaryStringAlgorithm<string>(0.85, 0.10, true, 30, 100);  // CHANGE THE GENERIC TYPE (NOW IT'S INT AS AN EXAMPLE) AND THE PARAMETERS VALUES
            string solutionMaxOne = maxOne.Run(maxOne.GeneratePopulation, maxOne.ComputeFitness, maxOne.SelectTwoParents, maxOne.CrossOver, maxOne.Mutation);
            Console.WriteLine(solutionMaxOne);
            Console.WriteLine();
            Console.WriteLine();
            
            Console.WriteLine("Solution Pregnancy:");
            PregnancyAlgorithm<double> pregnancy = new PregnancyAlgorithm<double>(0.85, 0.085, true, 70, 200); // CHANGE THE GENERIC TYPE (NOW IT'S INT AS AN EXAMPLE) AND THE PARAMETERS VALUES
            double[] solutionPregnancy = pregnancy.Run(pregnancy.GeneratePopulation, pregnancy.ComputeFitness, pregnancy.SelectTwoParents, pregnancy.CrossOver, pregnancy.Mutation);
            for (int i = 0; i < solutionPregnancy.Length; i++)
            {
                Console.WriteLine("The model coefficient at index {0} has a value of {1}", i, solutionPregnancy[i]);
            }
            

            Console.Read();
        }
    }
}
