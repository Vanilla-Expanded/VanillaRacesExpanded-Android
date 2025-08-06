using RimWorld;
using System.Collections.Generic;
using Verse;

namespace VREAndroids
{
    [HotSwappable]
    public class Building_NeutroCasket : Building_Bed
    {
        public CompRefuelable compRefuelable;
        public CompPowerTrader compPower;
        private void Initialize()
        {
            this.compRefuelable = base.GetComp<CompRefuelable>();
            this.compPower = base.GetComp<CompPowerTrader>();
            this.Medical = true;
        }

        public override void PostMake()
        {
            base.PostMake();
            Initialize();
        }

        public override void ExposeData()
        {
            base.ExposeData();
            if (Scribe.mode == LoadSaveMode.LoadingVars)
                Initialize();
        }

        public override string GetInspectString()
        {
            try
            {
                this.Medical = false;
                this.def.building.bed_humanlike = false;
                return $"{base.GetInspectString()}\n".TrimEndNewlines();
            }
            finally
            {
                this.Medical = true;
                this.def.building.bed_humanlike = true;
            }
        }

        public override void TickInterval(int delta)
        {
            base.TickInterval(delta);
            if (this.IsHashIntervalTick(60, delta) && (compPower == null || compPower.PowerOn))
            {
                foreach (var occupant in CurOccupants)
                {
                    var hediff = occupant.health.hediffSet.GetFirstHediffOfDef(VREA_DefOf.VREA_NeutroLoss);
                    if (hediff != null && occupant.health.hediffSet.BleedRateTotal <= 0)
                    {
                        if (compRefuelable.fuel >= 1)
                        {
                            HealthUtility.AdjustSeverity(occupant, VREA_DefOf.VREA_NeutroLoss, -0.01f);
                            compRefuelable.ConsumeFuel(1f);
                        }
                    }
                }
            }
        }
        public override IEnumerable<Gizmo> GetGizmos()
        {
            foreach (Gizmo gizmo in base.GetGizmos())
            {
                if (gizmo is Command_Toggle toggle)
                {
                    if (toggle.defaultLabel == "CommandBedSetAsMedicalLabel".Translate())
                    {
                        continue;
                    }
                }
                yield return gizmo;
            }
        }
    }
}
