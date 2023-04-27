using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace VREAndroids
{

    [HarmonyPatch(typeof(GeneUtility), "GenesInOrder", MethodType.Getter)]
    public static class GeneUtility_GenesInOrder_Patch
    {
        public static void Postfix(ref List<GeneDef> __result)
        {
            var window = Find.WindowStack.WindowOfType<GeneCreationDialogBase>();
            if (window != null)
            {
                if (window is Dialog_CreateAndroid)
                {
                    __result = Utils.AndroidGenesGenesInOrder;
                }
                else
                {
                    __result = __result.Where(x => x.IsAndroidGene() is false).ToList();
                }
            }
        }
    }
}
