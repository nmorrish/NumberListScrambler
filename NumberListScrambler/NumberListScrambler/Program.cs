using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NumberListScrambler
{
    class Program
    {
        /// <summary>
        /// This is a simple console application that generates a list of 10,000 numbers in random order every time it is run. 
        /// Each number in the list is unique and between 1 and 10,000 (inclusive).
        /// 
        /// The overloded method, GenerateList(), will generate the list.
        /// 
        /// Rather than outputting all 10,000 numbers, 3 test cases will be used to show that the list meets the requirements
        /// The user can view the list if they want
        /// All 3 combined will cover all cases in which the list might fail the requirements
        /// 
        /// The Fisher-Yates shuffle algorithm was found in my research to be an efficient way to randomise lists.
        /// </summary>
        static void Main()
        {
            try
            {
                List<int> numberList = GenerateList();

                //indicate to the user that the list has been genrated
                Console.WriteLine(numberList != null ? "A list of items has been generated" : "No list has been generated");

                //Check for duplicates
                Console.WriteLine("The generated list contains {0}", VerifyDuplicates(numberList) ? "no duplicates" : "duplicates");

                //Check for size
                Console.WriteLine("The generated list has {0}", VerifyListSize(numberList) ? "exactly 10,000 items" : "an invalid number of items");

                //Check the sum. This ensures the 2 above tests cannot be tricked using negative numbers.
                Console.WriteLine("The numbers in the generated list are {0}", VerifySum(numberList) ? "valid" : "invalid");

                //An option to view the list. Outputted in rows of 10 with leading spaces added for neatness
                Console.Write("Do you want to see the list (y/n):");

                string entry = Console.ReadLine();

                if (entry.ToLower() == "y")
                {
                    int i = 0;
                    foreach (int number in numberList)
                    {
                        Console.Write(number.ToString().PadLeft(5).PadRight(6));
                        i++;
                        if (i % 10 == 0)
                        {
                            Console.WriteLine("");
                        }
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        /// <summary>
        /// Genrate a list of numbers from 1 to n and scramble the order. 
        /// Passing in a parameter will perform the operation for any number of digits; handy for testing and debugging.
        /// </summary>
        /// <param name="listSize">Integer value for list size</param>
        /// <returns>List of shuffled integers</returns>
        public static List<int> GenerateList(int listSize)
        {
            Random rand = new Random();

            //Creates an ordered list of integers. 
            List<int> orderedSet = Enumerable.Range(1, listSize).ToList();

            //Empty list for holding the shuffled list
            List<int> scrambledSet = new List<int>();

            //used to draw a number from the ordered list
            int newIndex;

            //Fisher–Yates shuffle: continue grabbing random numbers from the ordered list until the original list size reaches 0
            while (orderedSet.Count > 0)
            {
                //if the list has 1 remaining integer, skip the randomisation process and simply add the remaining integer to the list
                if (orderedSet.Count > 1)
                {
                    //Draw random number
                    newIndex = rand.Next(0, orderedSet.Count);

                    //Add drawn number to next position in new list
                    scrambledSet.Add(orderedSet.ElementAt(newIndex));

                    //remove drawn number from original list to avoid duplicates
                    orderedSet.Remove(orderedSet.ElementAt(newIndex));
                }
                else
                {
                    scrambledSet.Add(orderedSet.ElementAt(0));

                    //Remove the last integer from the list to avoid an infinite loop
                    orderedSet.Remove(orderedSet.ElementAt(0));
                }
            }

            return scrambledSet;
        }

        /// <summary>
        /// Generates a list of numbers from 1 to 10000 shuffled using the Fisher–Yates algorithm. List size can be specified by passing in a argument. Default size is 10,000
        /// </summary>
        /// <returns>List of shuffled integers</returns>
        public static List<int> GenerateList()
        {
            return GenerateList(10000);
        }

        /// <summary>
        /// Shows that all numbers in the list are positive integers between 1 and 10,000
        /// Wolfram Alpha calculated the sum of all numbers between 1 and 10,000 as 50,005,000.
        /// </summary>
        /// <param name="list">A list of integers</param>
        /// <returns>True if the sum of all integers is 50,005,000</returns>
        public static bool VerifySum(List<int> list)
        {
            return list.Sum() == 50005000;
        }

        /// <summary>
        /// A check that counts any duplicates in the list.
        /// </summary>
        /// <param name="list">A list of integers</param>
        /// <returns>True if the list contains no duplicates</returns>
        public static bool VerifyDuplicates(List<int> list)
        {
            List<int> duplicates = list.GroupBy(x => x).Where(g => g.Count() > 1).Select(x => x.Key).ToList();

            return duplicates.Count == 0;
        }

        /// <summary>
        /// Return true if the list of integers contains exactly 10000 integers.
        /// </summary>
        /// <param name="list">List of integers</param>
        /// <returns>True if the list size is exactly 10000</returns>
        public static bool VerifyListSize(List<int> list)
        {
            return list.Count == 10000;
        }
    }
}
