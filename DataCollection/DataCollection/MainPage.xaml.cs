using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Windows.ApplicationModel.Calls.Background;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace DataCollection
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        MeasureLengthDevice measurementDevice = null;//gives MainPage access to MeasureLengthDevice.cs and allocates memory for it
        private Timer timer;//declares a timer for the class
        private Units units;//declares a Units variable for the class (used to tell which unit of measurement is desired)

        public MainPage()
        {
            this.InitializeComponent();
            measurementDevice = new MeasureLengthDevice();//initializes measurementDevice

        }
        //Executes block when start button is clicked
        private void startCollectingBtn_Click(object sender, RoutedEventArgs e)
        {
            //starts the MainPage timer calling the "timer_Tick" event every 15 seconds
            this.timer = new Timer(timer_Tick, null, (int)TimeSpan.FromSeconds(1).TotalMilliseconds, (int)TimeSpan.FromSeconds(15).TotalMilliseconds);
            //calls start collecting method in "MeasureLengthDevice.cs"
            measurementDevice.StartCollecting();
            //calls SetUnitsToUse method in "MeasureLengthDevice.cs"
            measurementDevice.SetUnitsToUse(units);
            //makes both radio buttons(used for selecting desired untis) unusable while the program is running
            metricRadio.IsEnabled = false;
            imperialRadio.IsEnabled = false;
            //calls the "PrintToUnitsField" method
            PrintToUnitsField();
        }

        //Executes block when stop button is clicked
        private void stopCollectingBtn_Click(object sender, RoutedEventArgs e)
        {
            //calls StopCollecting method in "MeasureLengthDevice.cs"
            measurementDevice.StopCollecting();
            //Disposes of the timer in the current class so that the timer_Click event handler is not called when undesired
            this.timer.Dispose();
            //makes both radio buttons usable again
            metricRadio.IsEnabled = true;
            imperialRadio.IsEnabled = true;

        }

        //executes bloock when the get history button is clicked
        private void getHistoryBtn_Click(object sender, RoutedEventArgs e)
        {
            //clears the data in the "historyLabel" field
            historyLabel.Text = "";
            //calls the "PrintToHistoryField" method
            PrintToHistoryField();
        }
  
        //If the radio button for metric units is checked then sets units variable to Units.Metric
        private void metricRadio_Checked(object sender, RoutedEventArgs e)
        {
            units = Units.Metric;
        }

        //if the radio button for imperial units is checked then sets units to Units.Imperial
        private void imperialRadio_Checked(object sender, RoutedEventArgs e)
        {
            units = Units.Imperial;
        }
       
        //method used to print data to the "mostRecentLabel"
        public void PrintToRecentField()
        {
            //creates a units variable for the method and gets information on the current desired units from "UnitsToUse" method in "MeasureLengthDevice.cs"
            Units unitsToUse = measurementDevice.UnitsToUse();
            //creates and initializes a decimal variable to hold the current meaurement
            decimal measure = 0;

            //checks if the current desired units is metric
            if (unitsToUse == Units.Metric)
            {
                //gets the value of current measurement in metric value from the "MetricValue" method
                measure = measurementDevice.MetricValue();
            }
            else
            {
                //gets the value of current measurement in metric value from the "ImperialValue" method
                measure = measurementDevice.ImperialValue();
            }
            //converts "meaure" to a string and prints it in the "mostRecentLabel" field
            mostRecentLabel.Text = measure.ToString();
        }

        //prints the desired units to the "unitsLabel" field
        public void PrintToUnitsField()
        {
            //creates a units variable for the method and gets information on the current desired units from "UnitsToUse" method in "MeasureLengthDevice.cs"
            Units unitsToUse = measurementDevice.UnitsToUse();

            //checks if the current desired units is metric
            if (unitsToUse == Units.Metric)
            {
                //displays the word centimeters in "unitsLabel" field
                unitsLabel.Text = ("centimeters");
            }

            else
            {
                //displays the word inches in "unitsLabel" field
                unitsLabel.Text = ("inches");
            }
        }

        //prints a timestamp in the "timestampLabel" field
        public void PrintToTimestampField()
        {
            timeStampLabel.Text = measurementDevice.GetTimestampString();
        }

        //prints the data found in the array passed from "GetRawData" method found in "MeasureLengthDevice.cs" to the "historyLabel" field
        public void PrintToHistoryField()
        {
            //for loop to step through each element in the array passed from the "GetRawData" method
            for (int count = 0; count < 9; count++)
            {
                //calls "GetRawData" and prints the the integer found in the element equal to "count"(first iteration will get the first element the second will get the second and so on and so forth)
                historyLabel.Text += $"{measurementDevice.GetRawData()[count].ToString()}\r\n";
            }
        }

        //timerTick event handler executes the block whenever 15 seconds have passed(as stated in the "StartCollectingBtn_Click" method)
        private async void timer_Tick(object state)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                //calls the "PrintToRecentField" and "PrintToTimeStampField" mehthods
                PrintToRecentField();
                PrintToTimestampField();
            });
        }

    }
}
