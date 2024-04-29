using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.PointOfService.Provider;

namespace DataCollection
{
    interface IMeasuringDevice
    {
        //returns the most recent meaurement as a metric value
        decimal MetricValue();
        //returns the most recent meaurement as an imperial value
        decimal ImperialValue();
        //starts timer
        void StartCollecting();
        //stops timer
        void StopCollecting();
        //returns the int array containing the 10 most recent measurements from oldest to newest
        int[] GetRawData();
    }
}
