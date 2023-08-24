using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RelayCalculator.Services.Interfaces;

namespace RelayCalculator.Services
{
    public class PermutationService : IPermutationService
    {
        public List<int[]> GetPermutations(int number)
        {
            return CreatePermutations(GetListNumbers(number));
        }

        public List<int> GetListNumbers(int number)
        {
            //creates a new list with ints up to the total number 
            var possibleNumbers = new List<int>();

            for (int i = 0; i < number; i++)
            {
                possibleNumbers.Add(i);
            }

            return possibleNumbers;
        }

        public List<int[]> CreatePermutations(List<int> possibleNumbers, List<int> newList = null, List<int[]> totalList = null)
        {
            newList = newList ?? new List<int>();
            totalList = totalList ?? new List<int[]>();

            //checks for the 'base case', when the created list has a length of 4, it is added to the totallist
            if (newList.Count == 4)
            {
                totalList.Add(newList.ToArray());

                //recursion ended and returned
                return totalList;
            }

            //for each number left in the list of possible numbers
            foreach (var num in possibleNumbers)
            {
                //create copys to avoid mutating the iteration list
                var copyPossibleNumbers = new List<int>(possibleNumbers);
                var copyNewList = new List<int>(newList);

                //adds the number to the list
                copyNewList.Add(num);

                //removes the number from the list of possible numbers
                copyPossibleNumbers.Remove(num);

                //calls the same function with the updated lists
                CreatePermutations(copyPossibleNumbers, copyNewList, totalList);
            }

            //return when for loop is completely run through
            return totalList;
        }
    }
}
