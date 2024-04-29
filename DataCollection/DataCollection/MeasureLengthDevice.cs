using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Core;
using Windows.Media.Audio;
using Windows.Security.Cryptography.Core;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace DataCollection
{
    class MeasureLengthDevice : IMeasuringDevice
    {
        //creates variables needed for the "MeasreLengthDevice" class
        private Timer timer;
        //"int mostRecentMeasure" stored the value of the most recent meaurement
        private int mostRecentMeasure;
        //"Units unitsToUse" stores either "Units.Metric" or "Units.Imperial" as specicified by the user
        private Units unitsToUse;
        //creates an array to store the last 10 meaurements and sets the number of elements to 10
        private int[] dataCaptured = new int[10];
        //counter to keep track of the number of meaurements currently in "int[] dataCaptured"
        private int dataCapturedCount = 0;

        //constructor for "MeasureLengthDevice" class
        public MeasureLengthDevice()
        {
            //starts out set to Metric becuase when the program starts centimeters is selected
            unitsToUse = Units.Metric;
            //initialize mostRecentMeasure to 0 since there is no recent measure
            this.mostRecentMeasure = 0;
            //initializes each element in the "dataCaptured" array to 0
            for (int count = 1; count < 9; count++)
            {
                this.dataCaptured[count] = 0;
            }
        }

        //method that starts timer for this class
        public void StartCollecting()
        {
            //starts timer and calls the "timer_Tick" event handler every 14.99 seconds 
            //(I set this event to happen 0.01 sconds before the one in the mainpage becuase I was having aa problem where the other one was being called first and wasnt able to grab the updated value becuase of it)
            this.timer = new Timer(timer_Tick, null, (int)TimeSpan.FromSeconds(1).TotalMilliseconds, (int)TimeSpan.FromSeconds(14.99).TotalMilliseconds);
        }

        //method to stop the timer for this class
        public void StopCollecting()
        {
            //Disposes of the timer in the current class so that the timer_Click event handler is not called when undesired
            this.timer.Dispose();
        }

        //setter for units variable
        public void SetUnitsToUse(Units units)
        {
           unitsToUse = units;
        }


        //getter for units variable
        public Units UnitsToUse()
        {
            return unitsToUse;
        }

        //when called multiples the "mostRecentMeasure" variable (casting as a decimal) and returns the value to the area it was called
        public decimal MetricValue()
        {
            return (decimal)(mostRecentMeasure * 2.54);
        }

        //when called multiples the "mostRecentMeasure" variable (casting as a decimal) and returns the value to the area it was called
        public decimal ImperialValue()
        {
            return (decimal)(mostRecentMeasure);
        }

        //getter for "dataCaptured" array
        public int[] GetRawData()
        {
            return this.dataCaptured;
        }

        //turns the "mostRecentMeasure" variable to a string
        public static string GetRecentMeasureString(decimal measure)
        {
            string measureOut = measure.ToString();
            return measureOut;
        }

        //stores a timestamp in a string and passes it back to where it was called
        public string GetTimestampString()
        {
            string timestamp = DateTime.Now.ToString();

            return timestamp;
        }



        //timerTick event handler executes the block whenever 14.99 seconds have passed
        private async void timer_Tick(object state)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                //calls the "GetMeasurement" method in "Device.cs" and stores the value returned in mostRecentMeasure
                mostRecentMeasure = Device.GetMeasurement();
                //stores "mostRecentMeasure" in the "dataCaptured" array and the element accessed is determined by "dataCaturedCount"
                dataCaptured[dataCapturedCount] = mostRecentMeasure;
                //increments dataCapturedCount
                dataCapturedCount++;
                
                //checks to see if "dataCapturedCount" has been incremented past 9 indicating that the array is full
                if (dataCapturedCount > 9)
                {
                    //uses a for loop to step through the array and shifts numbers in the array down one element deleting the first element in the array
                    for (int count = 0; count < 9; count++)
                    {
                        //example dataCaptured[0] is set to the value found in dataCaptured[1]
                        dataCaptured[count] = dataCaptured[count + 1];
                    }
                    //sets "dataCapturedCount" back to 9 (the last element in the array)
                    dataCapturedCount = 9;
                }
            });
        }
    }
}

