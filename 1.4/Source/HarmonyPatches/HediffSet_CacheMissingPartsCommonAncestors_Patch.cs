using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace VREAndroids
{
    [HotSwappable]
    [HarmonyPatch(typeof(HediffSet), nameof(HediffSet.CacheMissingPartsCommonAncestors))]
    public static class HediffSet_CacheMissingPartsCommonAncestors_Patch
    {
        [HarmonyPriority(int.MaxValue)]
        public static bool Prefix(HediffSet __instance)
        {
            if (__instance.pawn.IsAndroid())
            {
                CacheMissingPartsCommonAncestorsForAndroids(__instance);
                return false;
            }
            return true;
        }

        private static void CacheMissingPartsCommonAncestorsForAndroids(HediffSet __instance)
        {
            if (__instance.cachedMissingPartsCommonAncestors == null)
            {
                __instance.cachedMissingPartsCommonAncestors = new List<Hediff_MissingPart>();
            }
            else
            {
                __instance.cachedMissingPartsCommonAncestors.Clear();
            }
            __instance.missingPartsCommonAncestorsQueue.Clear();
            __instance.missingPartsCommonAncestorsQueue.Enqueue(__instance.pawn.def.race.body.corePart);
            while (__instance.missingPartsCommonAncestorsQueue.Count != 0)
            {
                BodyPartRecord node = __instance.missingPartsCommonAncestorsQueue.Dequeue();
                Hediff_MissingPart hediff_MissingPart = (from x in __instance.hediffs.OfType<Hediff_MissingPart>()
                                                         where x.Part == node
                                                         select x).FirstOrDefault();
                if (hediff_MissingPart != null)
                {
                    __instance.cachedMissingPartsCommonAncestors.Add(hediff_MissingPart);
                    continue;
                }
                for (int i = 0; i < node.parts.Count; i++)
                {
                    __instance.missingPartsCommonAncestorsQueue.Enqueue(node.parts[i]);
                }
            }
        }
    }
}
