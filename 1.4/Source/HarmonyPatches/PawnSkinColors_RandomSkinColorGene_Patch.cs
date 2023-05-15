using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(PawnSkinColors), "RandomSkinColorGene")]
    public static class PawnSkinColors_RandomSkinColorGene_Patch
    {
        public static bool Prefix(Pawn pawn, ref GeneDef __result)
        {
            if (pawn.IsAndroid())
            {
                __result = Utils.SkinColorAndroidGenesInOrder.RandomElementByWeight((GeneDef x) => x.selectionWeight);
                return false;
            }
            return true;
        }
    }
}
