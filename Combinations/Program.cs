using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Combinations
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            long method1 = 0;
            CombinationGenerator combi = new CombinationGenerator(new object[] { "a", "6", "g", "y", "z", "m", "L", "P" }, 8);
            int count = 1;
            DirectoryInfo directoryInfo = new DirectoryInfo(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));
            string dumpPath = Path.Combine(directoryInfo.FullName, "codes.txt");

            stopwatch.Start();
            //using (System.IO.StreamWriter writer = new System.IO.StreamWriter(dumpPath, false))
            //{
            while (combi.Combination != null)
            {
                //writer.WriteLine(string.Join(", ", combi.Combination));
                //Console.WriteLine(count + " " + string.Join(", ", combi.Combination));
                combi.GetNext();
                //count++;
            }
            //}
            stopwatch.Stop();
            method1 = stopwatch.ElapsedMilliseconds;
            int total = combi.TotalCombinations();
            stopwatch.Restart();
            int i = 0;
            while(i < total)
            {
                combi.FindAtPosition(i++);
                //Console.WriteLine(i + " " + string.Join(", ", combi.FindAtPosition(i)));
            }

            stopwatch.Stop();
            Console.WriteLine();
            Console.WriteLine("first: " + method1);
            Console.WriteLine("second: " + stopwatch.ElapsedMilliseconds);
            Console.WriteLine("total: " + total);

            while (true)
            {
                Console.WriteLine("enter position");
                string value = Console.ReadLine();
                Console.WriteLine(string.Join("", combi.FindAtPosition(int.Parse(value))));
                Console.WriteLine();
                Console.WriteLine();
            }
        }
    }
}
