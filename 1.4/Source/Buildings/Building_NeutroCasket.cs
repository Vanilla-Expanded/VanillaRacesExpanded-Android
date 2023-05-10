using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace VREAndroids
{
    [HotSwappable]
    public class Building_NeutroCasket : Building_Bed
    {
        protected CompRefuelable compRefuelable;
        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            this.compRefuelable = base.GetComp<CompRefuelable>();
            this.Medical = true;
        }

        public override string GetInspectString()
        {
            this.Medical = false;
            this.def.building.bed_humanlike = false;
            var sb = new StringBuilder(base.GetInspectString() + "\n");
            this.Medical = true;
            this.def.building.bed_humanlike = true;
            return sb.ToString().TrimEndNewlines();
        }

        public override void Tick()
        {
            base.Tick();
            if (this.IsHashIntervalTick(60))
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
                    if (toggle.defaultLabel == "CommandBedSetForPrisonersLabel".Translate() || toggle.defaultLabel == "CommandBedSetAsMedicalLabel".Translate())
                    {
                        continue;
                    }
                }
                else if (gizmo is Command_SetBedOwnerType)
                {
                    continue;
                }
                yield return gizmo;
            }
        }
    }
}
