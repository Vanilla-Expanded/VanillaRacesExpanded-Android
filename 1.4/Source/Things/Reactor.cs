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
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref curEnergy, "curEnergy");
        }
    }
}
