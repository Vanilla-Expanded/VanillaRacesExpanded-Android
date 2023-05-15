using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(PawnHairColors), "RandomHairColorGene")]
    public static class PawnHairColors_RandomHairColorGene_Patch
    {
        public static bool Prefix(ref GeneDef __result, Color skinColor)
        {
            if (PawnGenerator_GenerateGenes_Patch.curPawn != null && PawnGenerator_GenerateGenes_Patch.curPawn.IsAndroid())
            {
                if (Utils.HairColorAndroidGenes.TryRandomElementByWeight((GeneDef g) => 
                (!PawnSkinColors.IsDarkSkin(skinColor)) ? g.selectionWeight 
                : (g.selectionWeight * g.selectionWeightFactorDarkSkin), out var result))
                {
                    __result = result;
                }
                else
                {
                    __result = null;
                }
                return false;
            }
            return true;
        }
    }
}
