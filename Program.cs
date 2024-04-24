using System;
using System.Runtime.InteropServices;
using static System.Formats.Asn1.AsnWriter;

namespace Processing_Damped_spring_SHM_data
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
            ProcessData();
        }

        static void ProcessData()
        {
            using (StreamReader MyStream = new StreamReader("C:\\Users\\jedsk\\Documents\\_My Documents\\Work\\A Level\\Physics\\dampedSpringSHMPractical.csv"))
            {
                MyStream.ReadLine();
                //List<string> Items = MyStream.ReadLine().Split(',').ToList();   //  Creates the pattern
                //int NoOfSymbols = Convert.ToInt32(MyStream.ReadLine()); //  First line gives number of allowed symbols
                List<string> previousItems = MyStream.ReadLine().Split(',').ToList();
                List<double> previousDoubles = new List<double>();
                foreach (string item in previousItems)
                {
                    previousDoubles.Add(double.Parse(item,System.Globalization.CultureInfo.InvariantCulture));
                }
                int nthMax = 0;
                bool waitingForMin = false;
                double maxHeight = 0;
                double timeOfMax = 0;

                double minHeight = 0;
                double timeOfMin = 0;
                while (MyStream.EndOfStream == false)
                {
                    
                    List<string> Items = MyStream.ReadLine().Split(',').ToList();
                    List<double> doubles = new List<double> { };
                    foreach (string a in Items)
                    {
                        doubles.Add(double.Parse(a));
                    }

                    

                    if (doubles[2] < 0 && previousDoubles[2] > 0)
                    {
                        
                        if (doubles[1] > previousDoubles[1])
                        {
                            maxHeight = doubles[1];
                            timeOfMax = doubles[0];
                        }
                        else
                        {
                            maxHeight = previousDoubles[1];
                            timeOfMax = previousDoubles[0];
                        }
                        timeOfMax = Math.Round(timeOfMax, 3);
                        maxHeight = Math.Round(maxHeight, 3);
                        waitingForMin = true;


                    }
                    else if (doubles[2] > 0 && previousDoubles[2] < 0 && waitingForMin)
                    {

                        if (doubles[1] < previousDoubles[1])
                        {
                            minHeight = doubles[1];
                            timeOfMin = doubles[0];
                        }
                        else
                        {
                            minHeight = previousDoubles[1];
                            timeOfMin = previousDoubles[0];
                        }
                        timeOfMin = Math.Round(timeOfMin, 3);
                        minHeight = Math.Round(minHeight, 3);

                        if (nthMax % 5 == 0)
                        {
                            Console.WriteLine("No. Oscillations:            " + nthMax);
                            Console.WriteLine("Time of maxima:              " + timeOfMax);
                            Console.WriteLine("Time of max and min height:  " + maxHeight + ", " + minHeight);
                            
                            double amplitude = .5 * (maxHeight - minHeight);
                            amplitude = Math.Round(amplitude, 3);
                            Console.WriteLine("Value for amplitude:         " + amplitude);
                            double lnAmplitude = Math.Log(amplitude);
                            lnAmplitude = Math.Round(lnAmplitude, 3);
                            Console.WriteLine("Log of amplitude:            " + lnAmplitude);
                            
                            Console.WriteLine();

                        }
                        nthMax++;
                        waitingForMin = false;
                    }
                    









                    previousItems = Items;
                    previousDoubles = doubles;
                }

                
            }
            Console.ReadLine();
        }
    }
}
