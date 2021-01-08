using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Combinations
{
    public class CombinationGenerator
    {
        private int _size;
        private object[] _values;
        private object[] _combination;
        private object[] _lastCombination;
        private int _position;
        // @TODO: is order important
        // @TODO: is repetitian allowed



        // changed values from string array to object array
        // in preparation of being able to deal with object combinations and not just characters and strings
        public CombinationGenerator(object[] values, int length) 
        {
            this._size = length;
            this._values = values;
            this._combination = Enumerable.Repeat(values[0], length).ToArray(); // initialize the first combination
            this._position = length - 1;
            this._lastCombination = Enumerable.Repeat(this._values[this._values.Length - 1], this._size).ToArray();
        }

        private object[] tempList = null;

        // @TODO:
        //private object[] ProcessOrderNotImportantRepetitionNotAllowed(object[] lastSequence)
        //{
        //    int newValueIndex;

        //    if (this.tempList == null)
        //    {
        //        Array.Copy(this._values, this.tempList, this._size);
        //        return this.tempList;
        //    }

        //    newValueIndex = Array.IndexOf(this._values, lastSequence[this._position]) + 1;
        //}

        private object[] ProcessNext(object[] lastSequence)
        {
            int newValueIndex;
            object[] allowableValues;
            object[] newCombination = lastSequence;

            if (lastSequence.SequenceEqual(this._lastCombination))
                return this._combination = null;

            allowableValues = this._values;
            newValueIndex = Array.IndexOf(allowableValues, lastSequence[this._position]) + 1;

            if (newValueIndex >= allowableValues.Length)
            {
                newCombination = newCombination.ReplaceSingle(this._position, allowableValues[0]);

                this._position--;

                newCombination = this.ProcessNext(newCombination);

                this._position = lastSequence.Length - 1;

                return newCombination;
            }

            newCombination = newCombination.ReplaceSingle(this._position, allowableValues[newValueIndex]);
            return this._combination = newCombination;
        }

        public object[] GetNext()
        {
            return this.ProcessNext(this._combination);
        }

        public object[] Combination { get { return this._combination; } }

        /// <summary>
        /// Finds a combination in the series by index number.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public object[] FindAtPosition(int position)
        { 
            List<object> combination = new List<object>();
            int division = 0; // the amount of times we have divided - or better yet, the position of the element in combination we have figured out
            double positionCalc = (double)position; // the total flips or changes this element in combination has done.

            while (division < this._size)
            {
                // the total flips or changes next element in combination has done.
                double nextCharFlipCount = (double)positionCalc / (double)this._values.Length; 
                // the value of this element in our combination, expressed as the index, within our allowed values
                int index = (int)(Math.Ceiling(positionCalc) - (Math.Floor(nextCharFlipCount) * (double)this._values.Length)) - 1; 

                if (index < 0) index = this._values.Length - 1; // not sure why this happens

                combination.Add(this._values[index]);

                positionCalc = nextCharFlipCount;
                division++;
            }

            return combination.Reverse<object>().ToArray();
        }

        public int TotalCombinations()
        {
            return (int)Math.Pow(this._values.Length, this._size); // factorial(this._values.Length) / (factorial(this._size) * (this._values.Length - this._size));
        }

        public static int factorial(int number)
        {
            if (number == 1)
                return 1;
            else
                return number * factorial(number - 1);
        }
    }
}

internal static class Ext
{
    public static string ReplaceSingle(this string str, int index, char newChar)
    {
        return str.ReplaceSingle(index, newChar.ToString());
    }
    public static string ReplaceSingle(this string str, int index, string newChar)
    {
        StringBuilder newString = new StringBuilder();

        newString.Append(str.Substring(0, index));
        newString.Append(newChar);
        if (index + 1 < str.Length) newString.Append(str.Substring(index + 1));

        return newString.ToString();
    }

    public static object[] ReplaceSingle(this object[] objs, int index, object newObj)
    {
        List<object> newArr = new List<object>();

        newArr.AddRange(objs.TakeWhile( (o,i) => i < index ));
        newArr.Add(newObj);
        if(index + 1 < objs.Length)
            newArr.AddRange(objs.Where((o, i) => i > index));

        return newArr.ToArray();
    }
}
