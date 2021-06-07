using System.Windows.Media;
using HLab.Mvvm.Flowchart.Models;

namespace HLab.Erp.Lims.Monographs.Module.Graph
{

    public class MonographValueType : GraphValueType
    {
        public string UnitGroup { get; set; }
    }

    public static class ValueTypes
    {
        public static MonographValueType Volume = new MonographValueType
        {
            Id = "Volume",
            IconPath = "Pins/PinRound",
            Color = Colors.Blue,
            UnitGroup = "v"
        };

        public static MonographValueType Weight { get; } = new MonographValueType
        {
            Id = "Weight",
            IconPath = "Pins/PinSquare",
            Color = Colors.LimeGreen,
            UnitGroup = "m"
        };

        public static MonographValueType Unit { get; } = new MonographValueType
        {
            Id = "Unit",
            IconPath = "Pins/PinDiamond",
            Color = Colors.PaleGoldenrod,
            UnitGroup = "u"
        };
    }
}
