using System.Runtime.Serialization;
using HLab.Erp.Lims.Monographs.Data;
using HLab.Erp.Lims.Monographs.Module.Graph.Blocks.Consumable;
using HLab.Erp.Lims.Monographs.Module.Graph.Blocks.Injection;
using HLab.Erp.Lims.Monographs.Module.Graph.Blocks.Solution;
using HLab.Erp.Lims.Monographs.Module.Graph.Blocks.Test;
using HLab.Erp.Lims.Monographs.Module.Graph.Blocks.Volumes;
using HLab.Notify.PropertyChanged;

namespace HLab.Erp.Lims.Monographs.Module.Graph
{
    using H = H<MonographGraph>;

    [DataContract]
    public class MonographGraph : Mvvm.Flowchart.Models.Graph
    {
        public MonographGraph() => H.Initialize(this);

        public void ConvertMonograph(Monograph monograph)
        {
            Monograph = monograph;


            var s = new SolutionBlock{Parent = this, Id = "SOL"};
            var v = new VolumesBlock { Parent = this, Id = "VOL"};
            var g = new InjectionBlock { Parent = this, Id = "INJ"};


            g.Gradient.Set("A", 0, 0.0);
            g.Gradient.Set("A", 20, 0.0);
            g.Gradient.Set("A", 21, 1.0);
            g.Gradient.Set("A", 40, 1.0);

            g.Gradient.Set("B", 0, 1.0);
            g.Gradient.Set("B", 20, 1.0);
            g.Gradient.Set("B", 21, 0.0);
            g.Gradient.Set("B", 40, 0.0);

            //g.Gradient.Channels.Add(new GradientChannel(g.Gradient){Name = "A"});
            //g.Gradient.Channels.Add(new GradientChannel(g.Gradient){Name = "B"});

            //g.Gradient.Times.Add(new GradientTime(g.Gradient){Time = 0.0});
            //g.Gradient.Times.Add(new GradientTime(g.Gradient){Time = 40.0});

            Blocks.Add(s);
            Blocks.Add(v);
            Blocks.Add(g);

            foreach (var c in Monograph.Consumables)
            {
                var cb = new ConsumableBlock
                {
                    Parent = this,
                    Id = "C_" + c.Id,
                    MonographConsumable = c
                };

                switch (c.Consumable.UnitGroup)
                {
                    case "m":
                        cb.OutputType = ValueTypes.Weight;
                        break;
                    case "v":
                        cb.OutputType = ValueTypes.Volume;
                        break;
                    case "u":
                        cb.OutputType = ValueTypes.Unit;
                        break;
                    //case "t":
                    //    Blocks.Add(new ConsumableBlock<TimeValue>{MonographConsumable = c});
                    //    break;
                }

                Blocks.Add(cb);
            }

            foreach (var test in Monograph.Tests)
            {
                Blocks.Add(new AssayBlock
                {
                    Parent = this,
                    Id = "T_"+ test.Id,
                    MonographTest = test
                });
            }
        }

        public Monograph Monograph { get; set; }

    }
}