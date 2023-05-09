using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace VREAndroids
{
    public class Building_AndroidBed : Building_Bed
    {
        public bool autoRepair;

        public override IEnumerable<Gizmo> GetGizmos()
        {
            foreach (var g in base.GetGizmos())
            {
                yield return g;
            }
            if (Faction == Faction.OfPlayer)
            {
                Command_Toggle command_Toggle = new Command_Toggle();
                command_Toggle.defaultLabel = "CommandAutoRepair".Translate();
                command_Toggle.defaultDesc = "VREA.CommandAutoRepairDesc".Translate(this.def.label);
                command_Toggle.icon = ContentFinder<Texture2D>.Get("UI/Gizmos/AutoRepair");
                command_Toggle.isActive = () => autoRepair;
                command_Toggle.toggleAction = () =>
                {
                    autoRepair = !autoRepair;
                };
                yield return command_Toggle;
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref autoRepair, "autoRepair");
        }
    }
}
