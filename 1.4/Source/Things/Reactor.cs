using System.Collections.Generic;
using System.Text;
using Verse;

namespace VREAndroids
{
    public class Reactor : ThingWithComps
    {
        public float curEnergy;
        public override string GetInspectString()
        {
            var sb = new StringBuilder();
            var baseString = base.GetInspectString();
            if (baseString.NullOrEmpty() is false)
            {
                sb.AppendLine(baseString);
            }
            sb.AppendLine("VREA.CurrentEnergyLevel".Translate(curEnergy.ToStringPercent()));
            return sb.ToString().TrimEndNewlines();
        }
        public override void PostMake()
        {
            base.PostMake();
            curEnergy = 1f;
        }

        public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn selPawn)
        {
            foreach (var opt in base.GetFloatMenuOptions(selPawn))
            {
                yield return opt;
            }
            if (selPawn.HasActiveGene(VREA_DefOf.VREA_SelfRecharge))
            {
                yield return new FloatMenuOption("VREA.ReplaceReactor".Translate(), delegate
                {
                    var job = JobMaker.MakeJob(VREA_DefOf.VREA_ReplaceReactor, this);
                    job.count = 1;
                    selPawn.jobs.TryTakeOrderedJob(job);
                });
            }
        }
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref curEnergy, "curEnergy");
        }
    }
}
