using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AirSimApp.Controls.AltimeterResources
{
    public class CommandMarker : Shape
    {
        protected override Geometry DefiningGeometry => Geometry.Parse("M0.5,0.5 L38.5,0.5 48,10.000001 57.5,0.5 95.5,0.5 95.5,39.5 57.5,39.5 48,30 38.5,39.5 0.5,39.5 z");
    }
}