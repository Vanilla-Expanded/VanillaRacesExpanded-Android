using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(InteractionUtility), "CanInitiateInteraction")]
    public static class InteractionUtility_CanInitiateInteraction_Patch
    {
        public static Dictionary<Pawn, CachedResult<bool>> cachedResults = new();
        public static void Postfix(Pawn pawn, ref bool __result)
        {
            if (__result)
            {
                if (!cachedResults.TryGetValue(pawn, out var cachedResult))
                {
                    cachedResults[pawn] = cachedResult = new CachedResult<bool>();
                    cachedResult.Result = pawn.Emotionless();
                }
                else if (cachedResult.CacheExpired)
                {
                    cachedResult.Result = pawn.Emotionless();
                }

                if (cachedResult.Result)
                {
                    __result = false;
                }
            }
        }
    }
}
