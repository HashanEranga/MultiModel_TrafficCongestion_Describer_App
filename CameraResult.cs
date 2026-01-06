using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficCongesionApp
{
    public class CameraResult
    {
        public bool IsBroken { get; set; }
        public required string TrafficCongesionLevel { get; set; }
        public required string Analysis { get; set; }
    }
}
