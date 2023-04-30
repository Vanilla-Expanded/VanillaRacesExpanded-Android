using RimWorld;
using UnityEngine;
using Verse;

namespace VREAndroids
{
    [HotSwappable]
    public class Hediff_AndroidReactor : Hediff_AndroidPart
    {
        public float curEnergy;
        public override void PostAdd(DamageInfo? dinfo)
        {
            base.PostAdd(dinfo);
            curEnergy = 1f;
        }

        public float PowerEfficiencyDrainMultiplier
        {
            get
            {
                int efficiency = 0;
                foreach (Gene item in pawn.genes.GenesListForReading)
                {
                    if (!item.Overridden)
                    {
                        efficiency += item.def.biostatMet;
                    }
                }
                return AndroidStatsTable.PowerEfficiencyToPowerDrainFactorCurve.Evaluate(efficiency);
            }
        }
        public override void Tick()
        {
            base.Tick();
            var baseDrainSpeed = (1f / GenDate.TicksPerYear * 2f) * PowerEfficiencyDrainMultiplier;
            curEnergy = Mathf.Max(0,  curEnergy - baseDrainSpeed);

            if (curEnergy <= 0 && this.Severity != 1f)
            {
                this.Severity = 1f;
            }
            else if (curEnergy > 0 && this.Severity != 0f)
            {
                this.Severity = 0f;
            }
        }
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref curEnergy, "curEnergy");
        }
    }
}
