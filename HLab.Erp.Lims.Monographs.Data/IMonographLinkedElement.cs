using HLab.Erp.Data;

namespace HLab.Erp.Lims.Monographs.Data
{

    public interface IMonographLinkedElement : IEntity
    {
        int? MonographId { get; set; }
        Monograph Monograph { get; set; }

        double QtyAbs { get; }
        double QtyAbsNext { get; }

        double Cost { get; }
        double CostNext { get; }
        string UnitGroup { get; }
        string RightUnitGroup { get; }


        double MainQtyAbs { get; }
        double MainQtyAbsNext { get; }

        double SumRightQtyAbs { get; }
        double SumRightQtyAbsNext { get; }

        double NbVolumes { get; }
        double NbVolumesNext { get; }

        bool LeftLinkedTo(IMonographLinkedElement e);
        bool RightLinkedTo(IMonographLinkedElement e);

    }
}
