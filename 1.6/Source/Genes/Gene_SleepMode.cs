using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace VREAndroids
{
    public class Gene_SleepMode : Gene
    {
        public override IEnumerable<Gizmo> GetGizmos()
        {
            if (pawn.IsColonistPlayerControlled)
            {
                yield return new Command_Action
                {
                    defaultLabel = "VREA.SleepModeOn".Translate(),
                    defaultDesc = "VREA.SleepModeOnDesc".Translate(),
                    icon = ContentFinder<Texture2D>.Get("UI/Gizmos/SleepMode_Initiate"),
                    action = delegate
                    {
                        var building = ThingMaker.MakeThing(VREA_DefOf.VREA_AndroidSleepMode) as Building_AndroidSleepMode;
                        building.android = pawn;
                        GenSpawn.Spawn(building, pawn.Position, pawn.Map);
                        Find.Selector.Select(building);
                        pawn.DeSpawn();
                    }
                };
            }
        }
    }
}
