using System.ComponentModel;

namespace AirSimApp.Models
{
    /// <summary>Possible vehicle modes.</summary>
    public enum VehicleMode
    {
        [Description("Landed")]
        Landed,

        [Description("Launching")]
        Launching,

        [Description("Hovering")]
        Hovering,

        [Description("Moving")]
        Moving,

        [Description("Landing")]
        Landing,

        [Description("Recovering")]
        Recovering,

        [Description("---")]
        Unknown,
    }
}