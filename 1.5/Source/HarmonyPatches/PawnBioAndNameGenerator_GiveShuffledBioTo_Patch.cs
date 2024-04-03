using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(PawnBioAndNameGenerator), "GiveShuffledBioTo")]
    public static class PawnBioAndNameGenerator_GiveShuffledBioTo_Patch
    {
        public static XenotypeDef xenotypeStatic;
        [HarmonyPriority(int.MaxValue)]
        public static bool Prefix(Pawn pawn, FactionDef factionType, string requiredLastName, List<BackstoryCategoryFilter> 
            backstoryCategories, bool forceNoBackstory = false, bool forceNoNick = false, XenotypeDef xenotype = null)
        {
            xenotypeStatic = xenotype;
            if (xenotype != null && xenotype.IsAndroidType() 
                || PawnGenerator_TryGenerateNewPawnInternal_Patch.curRequest != null 
                && PawnGenerator_TryGenerateNewPawnInternal_Patch.curRequest.Value.ForcedCustomXenotype != null
                && PawnGenerator_TryGenerateNewPawnInternal_Patch.curRequest.Value.ForcedCustomXenotype.IsAndroidType())
            {
                Utils.TryAssignBackstory(pawn, "ColonyAndroid");
                pawn.Name = PawnBioAndNameGenerator.GeneratePawnName(pawn, NameStyle.Full, requiredLastName, forceNoNick, xenotype);
                return false;
            }
            return true;
        }
    }
}
