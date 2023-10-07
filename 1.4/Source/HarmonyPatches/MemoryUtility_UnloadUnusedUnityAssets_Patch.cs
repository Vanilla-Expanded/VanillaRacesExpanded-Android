using HarmonyLib;
using RimWorld;
using Verse;
using Verse.Profile;

namespace VREAndroids
{
    [HarmonyPatch(typeof(MemoryUtility), "UnloadUnusedUnityAssets")]
    public static class MemoryUtility_UnloadUnusedUnityAssets_Patch
    {
        public static void Postfix()
        {
            Building_AndroidStand.stands.Clear();
        }
    }
}
