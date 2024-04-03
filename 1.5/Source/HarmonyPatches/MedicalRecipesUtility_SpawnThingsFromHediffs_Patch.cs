using HarmonyLib;
using RimWorld;
using System.Linq;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(MedicalRecipesUtility), "SpawnThingsFromHediffs")]
    public static class MedicalRecipesUtility_SpawnThingsFromHediffs_Patch
    {
        [HarmonyPriority(int.MaxValue)]
        public static bool Prefix(Pawn pawn, BodyPartRecord part, IntVec3 pos, Map map)
        {
            if (pawn.IsAndroid())
            {
                SpawnThingsFromHediffs(pawn, part, pos, map);
                return false;
            }
            return true;
        }

        public static void SpawnThingsFromHediffs(Pawn pawn, BodyPartRecord part, IntVec3 pos, Map map)
        {
            if (!pawn.health.hediffSet.GetNotMissingParts().Contains(part))
            {
                return;
            }
            foreach (Hediff item in pawn.health.hediffSet.hediffs.Where((Hediff x) => x.Part == part))
            {
                if (item.def.spawnThingOnRemoved != null)
                {
                    var thing = GenSpawn.Spawn(item.def.spawnThingOnRemoved, pos, map);
                    if (thing is Reactor reactor && item is Hediff_AndroidReactor hediffReactor)
                    {
                        reactor.curEnergy = hediffReactor.Energy;
                    }
                }
            }
        }
    }
}
