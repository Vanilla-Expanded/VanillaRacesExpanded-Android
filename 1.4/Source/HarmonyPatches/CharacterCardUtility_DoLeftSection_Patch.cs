using HarmonyLib;
using RimWorld;
using System.Reflection;
using UnityEngine;
using Verse;

namespace VREAndroids
{
    public static class DubsMintMenus_Patch_HealthCardUtility_GenerateListing_Patch
    {
        public static MethodBase targetMethod;

        public static bool Prepare()
        {
            targetMethod = AccessTools.Method("DubsMintMenus.Patch_HealthCardUtility:GenerateListing");
            return targetMethod != null;
        }
        public static MethodBase TargetMethod()
        {
            return targetMethod;
        }
        public static void Prefix(Pawn pawn2, ref RecipeDef recipe, BodyPartRecord part)
        {
            if (pawn2.IsAndroid())
            {
                recipe = recipe.RecipeForAndroid();
            }
        }
    }

    [HarmonyPatch]
    public static class CharacterCardUtility_DoLeftSection_Patch
    {
        public static MethodBase TargetMethod()
        {
            foreach (var type in typeof(CharacterCardUtility).GetNestedTypes(AccessTools.all))
            {
                foreach (var method in type.GetMethods(AccessTools.all))
                {
                    if (method.Name.Contains("<DoLeftSection>") && method.GetParameters().Length == 1 
                        && method.GetParameters()[0].ParameterType == typeof(Rect))
                    {
                        return method;
                    }
                }
            }
            return null;
        }
        public static bool Prefix(Pawn ___pawn, Rect ___leftRect, Rect sectionRect)
        {
            if (___pawn.IsAndroid())
            {
                float num8 = sectionRect.y;
                Text.Font = GameFont.Small;
                BackstoryDef backstory = ___pawn.story.GetBackstory(BackstorySlot.Childhood);
                if (backstory != null)
                {
                    Rect rect7 = new Rect(sectionRect.x, num8, ___leftRect.width, 22f);
                    Text.Anchor = TextAnchor.MiddleLeft;
                    Widgets.Label(rect7, "VREA.Android".Translate());
                    Text.Anchor = TextAnchor.UpperLeft;
                    string text = backstory.TitleCapFor(___pawn.gender);
                    Rect rect8 = new Rect(rect7);
                    rect8.x += 90f;
                    rect8.width = Text.CalcSize(text).x + 10f;
                    Color color4 = GUI.color;
                    GUI.color = CharacterCardUtility.StackElementBackground;
                    GUI.DrawTexture(rect8, BaseContent.WhiteTex);
                    GUI.color = color4;
                    Text.Anchor = TextAnchor.MiddleCenter;
                    Widgets.Label(rect8, text.Truncate(rect8.width));
                    Text.Anchor = TextAnchor.UpperLeft;
                    if (Mouse.IsOver(rect8))
                    {
                        Widgets.DrawHighlight(rect8);
                    }
                    if (Mouse.IsOver(rect8))
                    {
                        TooltipHandler.TipRegion(rect8, backstory.FullDescriptionFor(___pawn).Resolve());
                    }
                    num8 += rect7.height + 4f;
                }
                if (___pawn.story != null && ___pawn.story.title != null)
                {
                    Rect rect9 = new Rect(sectionRect.x, num8, ___leftRect.width, 22f);
                    Text.Anchor = TextAnchor.MiddleLeft;
                    Widgets.Label(rect9, "BackstoryTitle".Translate() + ":");
                    Text.Anchor = TextAnchor.UpperLeft;
                    Rect rect10 = new Rect(rect9);
                    rect10.x += 90f;
                    rect10.width -= 90f;
                    Widgets.Label(rect10, ___pawn.story.title);
                    num8 += rect9.height;
                }
                return false;
            }
            return true;
        }
    }
}
