using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace DataCollection
{
    class Device
    {
        //method generates a random number between 1 and 10 then passes it back to where it was called
         public static int GetMeasurement()
        {
            Random rnd = new Random();
            int measurementMetric = rnd.Next(1, 10);
            return measurementMetric;
            }
    }
}
