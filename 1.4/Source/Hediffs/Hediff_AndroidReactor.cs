using RimWorld;
using UnityEngine;
using Verse;

namespace VREAndroids
{
    [HotSwappable]
    public class Hediff_AndroidReactor : Hediff_AndroidPart
    {
        private float curEnergy;
        public float Energy 
        { 
            get
            {
                return curEnergy;
            }
            set
            {
                curEnergy = Mathf.Clamp01(value);
                if (pawn != null)
                {
                    UpdateSeverity();
                }
            }
        }
        public override void PostAdd(DamageInfo? dinfo)
        {
            base.PostAdd(dinfo);
            Energy = 1f;
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
            var baseDrainSpeed = (1f / (GenDate.TicksPerYear * 2f)) * PowerEfficiencyDrainMultiplier;
            if (pawn.HasActiveGene(VREA_DefOf.VREA_SolarPowered))
            {
                var mapHeld = pawn.MapHeld;
                if (mapHeld != null && mapHeld.gameConditionManager.ElectricityDisabled 
                    || Find.World.gameConditionManager.ElectricityDisabled)
                {
                    Energy = Mathf.Min(1, Energy + baseDrainSpeed);
                    return;
                }
                else if (mapHeld != null && pawn.Position.InSunlight(mapHeld))
                {
                    return;
                }
            }
            if (pawn.HasActiveGene(VREA_DefOf.VREA_RainVulnerability) && pawn.Spawned && pawn.Position.Roofed(pawn.Map) is false
                && pawn.Map.weatherManager.RainRate >= 0.01f)
            {
                baseDrainSpeed *= 2f;
            }

            Energy = Mathf.Max(0, Energy - baseDrainSpeed);
        }

        private void UpdateSeverity()
        {
            if (Energy <= 0 && this.Severity != 1f)
            {
                this.Severity = 1f;
            }
            else if (Energy > 0 && this.Severity != 0f)
            {
                this.Severity = 0f;
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref curEnergy, "curEnergy");
            if (Scribe.mode == LoadSaveMode.PostLoadInit)
            {
                UpdateSeverity();
            }
        }
    }
}
