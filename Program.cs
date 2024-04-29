using System;
using System.Runtime.InteropServices;
using static System.Formats.Asn1.AsnWriter;

namespace Processing_Damped_spring_SHM_data
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter the full address of the csv file below:");
            string filename = Console.ReadLine();
            Console.Write("\n---------------- Start of data ----------------\n\n");
            ProcessData(filename);
        }

        static void ProcessData(string filename)
        {
            using (StreamReader MyStream = new StreamReader(filename))
            {
                MyStream.ReadLine();

                //  Splits the next line into seperate strings
                //List<string> Items = MyStream.ReadLine().Split(',').ToList();   

                //  Gets the next line from the file
                //int LineFromFile = Convert.ToInt32(MyStream.ReadLine());  


                //  Setup the first record as being the previous record (We will start running through on the second record)
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
                double timeOfMin = 0;   //  Not used currently
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
                        timeOfMin = Math.Round(timeOfMin, 3);   //  Not used currently
                        

                        if (nthMax % 5 == 0)
                        {
                            //  Rounding is left to the very end
                            Console.WriteLine("No. Oscillations:            " + nthMax);
                            Console.WriteLine("Time of maxima:              " + timeOfMax);
                            Console.WriteLine("Time of max and min height:  " + Math.Round(maxHeight, 3) + ", " + Math.Round(minHeight, 3));
                            
                            double amplitude = .5 * (maxHeight - minHeight);
                            
                            Console.WriteLine("Value for amplitude:         " + Math.Round(amplitude, 3, MidpointRounding.AwayFromZero));
                            double lnAmplitude = Math.Log(amplitude);
                            Console.WriteLine("Log of amplitude:            " + Math.Round(lnAmplitude, 2, MidpointRounding.AwayFromZero));
                            
                            Console.WriteLine();

                        }
                        nthMax++;
                        waitingForMin = false;
                    }
                    









                    previousItems = Items;   //  Not used currently
                    previousDoubles = doubles;
                }

                
            }
            Console.ReadLine();
        }
    }
}
