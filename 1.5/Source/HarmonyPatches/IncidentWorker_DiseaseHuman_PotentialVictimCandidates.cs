using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(IncidentWorker_DiseaseHuman), "PotentialVictimCandidates")]
    public static class IncidentWorker_DiseaseHuman_PotentialVictimCandidates
    {
        public static IEnumerable<Pawn> Postfix(IEnumerable<Pawn> __result, IncidentWorker_DiseaseHuman __instance)
        {
            foreach (var p in __result)
            {
                if (p.HasActiveGene(VREA_DefOf.VREA_SyntheticImmunity) && Utils.AndroidCanCatch(__instance.def.diseaseIncident) is false)
                {
                    continue;
                }
                else
                {
                    yield return p;
                }
            }
        }
    }
}
