using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AirSimApp.Controls.AltimeterResources
{
    public class ActualMarker : Shape
    {
        protected override Geometry DefiningGeometry => Geometry.Parse("M0.5,0.5 L30.667,0.5 42.667,12.5 55.009857,0.5 84.917,0.5 84.917,29.499999 55.891396,29.499999 42.667,17.187986 30.080795,29.499999 0.5,29.499999 z");
    }
}