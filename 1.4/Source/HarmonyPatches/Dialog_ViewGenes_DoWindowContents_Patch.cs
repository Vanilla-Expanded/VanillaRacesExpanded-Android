using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(Dialog_ViewGenes), "DoWindowContents")]
    public static class Dialog_ViewGenes_DoWindowContents_Patch
    {
        public static bool Prefix(Dialog_ViewGenes __instance, Rect inRect)
        {
            if (__instance.target.HasActiveGene(VREA_DefOf.VREA_SyntheticBody))
            {
                inRect.yMax -= Window.CloseButSize.y;
                Rect rect = inRect;
                rect.xMin += 34f;
                Text.Font = GameFont.Medium;
                Widgets.Label(rect, "VREA.ViewComponents".Translate() + ": " + __instance.target.genes.XenotypeLabelCap);
                Text.Font = GameFont.Small;
                GUI.color = XenotypeDef.IconColor;
                GUI.DrawTexture(new Rect(inRect.x, inRect.y, 30f, 30f), __instance.target.genes.XenotypeIcon);
                GUI.color = Color.white;
                inRect.yMin += 34f;
                Vector2 size = Vector2.zero;
                GeneUIUtility.DrawGenesInfo(inRect, __instance.target, __instance.InitialSize.y, ref size, ref __instance.scrollPosition);
                if (Widgets.ButtonText(new Rect(inRect.xMax - Window.CloseButSize.x, inRect.yMax, Window.CloseButSize.x, Window.CloseButSize.y), "Close".Translate()))
                {
                    __instance.Close();
                }
                return false;
            }
            return true;
        }
    }
}
