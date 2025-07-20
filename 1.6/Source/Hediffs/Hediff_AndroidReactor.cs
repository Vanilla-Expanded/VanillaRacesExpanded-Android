using RimWorld;
using UnityEngine;
using Verse;

namespace VREAndroids
{
    [HotSwappable]
    public class Hediff_AndroidReactor : Hediff_AndroidPart
    {
        private Need_ReactorPower needInt;
        public Need_ReactorPower NeedReactor => needInt ??= pawn.needs.TryGetNeed<Need_ReactorPower>();
        private float curEnergy;
        public override bool ShouldRemove => false;

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
                    var need = NeedReactor;
                    if (need != null)
                    {
                        need.curLevelInt = curEnergy;
                    }
                    UpdateSeverity();
                }
            }
        }

        public override void PostAdd(DamageInfo? dinfo)
        {
            base.PostAdd(dinfo);
            Energy = 1f;
            Severity = 0;
        }

        public override void PostRemoved()
        {
            base.PostRemoved();
            if (pawn.MapHeld != null)
            {
                pawn.TrySpawnWaste(pawn.PositionHeld, pawn.MapHeld);
            }
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

        public const int AndroidReactorTickRate = 60;

        public override void TickInterval(int delta)
        {
            base.TickInterval(delta);
            if (pawn.IsHashIntervalTick(AndroidReactorTickRate, delta))
            {
                var baseDrainSpeed = (1f / (GenDate.TicksPerYear * 2f)) * PowerEfficiencyDrainMultiplier;
                baseDrainSpeed *= AndroidReactorTickRate;
                if (pawn.HasActiveGene(VREA_DefOf.VREA_SolarPowered))
                {
                    var mapHeld = pawn.MapHeld;
                    if (mapHeld != null && (mapHeld.gameConditionManager.ElectricityDisabled(mapHeld)
                        || Find.World.gameConditionManager.ElectricityDisabled(mapHeld)))
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
