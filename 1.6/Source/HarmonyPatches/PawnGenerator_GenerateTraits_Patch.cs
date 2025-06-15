using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(PawnGenerator), "GenerateTraits")]
    public static class PawnGenerator_GenerateTraits_Patch
    {
        public static void Prefix(Pawn pawn, PawnGenerationRequest request)
        {
            if (pawn.IsAndroid() 
                || (request.ForcedXenotype?.IsAndroidType() ?? false) 
                || (request.ForcedCustomXenotype?.IsAndroidType() ?? false) 
                || (PawnBioAndNameGenerator_GiveShuffledBioTo_Patch.xenotypeStatic?.IsAndroidType() ?? false))
            {
                var prohibitedTraits = new List<TraitDef>();
                if (request.ProhibitedTraits != null)
                {
                    prohibitedTraits.AddRange(request.ProhibitedTraits);
                }
                foreach (var disallowedTrait in VREA_DefOf.VREA_AndroidSettings.disallowedTraits)
                {
                    var trait = DefDatabase<TraitDef>.GetNamedSilentFail(disallowedTrait);
                    if (trait != null)
                    {
                        prohibitedTraits.Add(trait);
                    }
                }
                request.ProhibitedTraits = prohibitedTraits;
            }

            PawnBioAndNameGenerator_GiveShuffledBioTo_Patch.xenotypeStatic = null;
        }
    }
}
