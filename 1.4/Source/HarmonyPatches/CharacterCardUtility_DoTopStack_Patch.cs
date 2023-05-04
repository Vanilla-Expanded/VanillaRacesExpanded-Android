using HarmonyLib;
using RimWorld;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch]
    public static class CharacterCardUtility_DoTopStack_Patch
    {
        public static MethodBase TargetMethod()
        {
            foreach (var type in typeof(CharacterCardUtility).GetNestedTypes(AccessTools.all))
            {
                var methods = type.GetMethods(AccessTools.all).Where(method => method.Name.Contains("<DoTopStack>") 
                    && method.GetParameters().Length == 1 && method.GetParameters()[0].ParameterType == typeof(Rect)).ToList();
                if (methods.Any())
                {
                    return methods[2];
                }
            }
            return null;
        }
        public static bool Prefix(Pawn ___pawn, Rect r)
        {
            if (___pawn.IsAndroid())
            {
                Rect rect11 = new Rect(r.x, r.y, r.width, r.height);
                GUI.color = CharacterCardUtility.StackElementBackground;
                GUI.DrawTexture(rect11, BaseContent.WhiteTex);
                GUI.color = Color.white;
                if (Mouse.IsOver(rect11))
                {
                    Widgets.DrawHighlight(rect11);
                }
                Rect position5 = new Rect(r.x + 1f, r.y + 1f, 20f, 20f);
                GUI.color = XenotypeDef.IconColor;
                GUI.DrawTexture(position5, ___pawn.genes.XenotypeIcon);
                GUI.color = Color.white;
                Widgets.Label(new Rect(r.x + 22f + 5f, r.y, r.width + 22f - 1f, r.height), ___pawn.genes.XenotypeLabelCap);
                if (Mouse.IsOver(r))
                {
                    TooltipHandler.TipRegion(r, () => ("VREA.Android".Translate() + ": " + ___pawn.genes.XenotypeLabelCap)
                    .Colorize(ColoredText.TipSectionTitleColor) + "\n\n" + ___pawn.genes.XenotypeDescShort + "\n\n" 
                    + "VREA.ViewComponentsDesc".Translate(___pawn.Named("PAWN")).ToString().StripTags()
                        .Colorize(ColoredText.SubtleGrayColor), 883938493);
                }
                if (Widgets.ButtonInvisible(r))
                {
                    if (Current.ProgramState == ProgramState.Playing && Find.WindowStack.WindowOfType<Dialog_InfoCard>() == null 
                        && Find.WindowStack.WindowOfType<Dialog_GrowthMomentChoices>() == null)
                    {
                        InspectPaneUtility.OpenTab(typeof(ITab_Genes));
                    }
                    else
                    {
                        Find.WindowStack.Add(new Dialog_ViewGenes(___pawn));
                    }
                }
                return false;
            }
            return true;
        }
    }
}
