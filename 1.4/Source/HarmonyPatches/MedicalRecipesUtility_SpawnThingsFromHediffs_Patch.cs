using HarmonyLib;
using RimWorld;
using System.Linq;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(MedicalRecipesUtility), "SpawnThingsFromHediffs")]
    public static class MedicalRecipesUtility_SpawnThingsFromHediffs_Patch
    {
        public static bool shouldCheck;
        public static bool Prefix(Pawn pawn, BodyPartRecord part, IntVec3 pos, Map map)
        {
            if (pawn.HasActiveGene(VREA_DefOf.VREA_SyntheticBody))
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
                if (item is Hediff_AndroidReactor reactor)
                {
                    if (pawn.HasActiveGene(VREA_DefOf.VREA_ZeroWaste) is false)
                    {
                        var wastepack = ThingMaker.MakeThing(ThingDefOf.Wastepack);
                        wastepack.stackCount = 5;
                        GenSpawn.Spawn(wastepack, pos, map);
                    }
                }
                else if (item.def.spawnThingOnRemoved != null)
                {
                    GenSpawn.Spawn(item.def.spawnThingOnRemoved, pos, map);
                }
            }
            for (int i = 0; i < part.parts.Count; i++)
            {
                SpawnThingsFromHediffs(pawn, part.parts[i], pos, map);
            }
        }
    }
}
