using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace VREAndroids
{
    [HotSwappable]
    [StaticConstructorOnStartup]
    public static class AndroidStatsTable
    {
        public static readonly IntRange AndroidStatRange = new IntRange(-20, 5);
        private struct AndroidStatData
        {
            public string labelKey;

            public string descKey;

            public Texture2D icon;

            public AndroidStatData(string labelKey, string descKey, Texture2D icon)
            {
                this.labelKey = labelKey;
                this.descKey = descKey;
                this.icon = icon;
            }
        }

        private static float cachedWidth;

        private const float NumberWidth = 90f;

        private const float IconSize = 22f;

        public static readonly Texture2D PowerEfficiencyIcon = ContentFinder<Texture2D>.Get("UI/BiostatIcon/BiostatEfficiency");
        public static readonly Texture2D ResourceCostIcon = ContentFinder<Texture2D>.Get("UI/BiostatIcon/BiostatResourceCost");
        public static readonly CachedTexture PowerEfficiencyIconTex = new CachedTexture("UI/BiostatIcon/BiostatEfficiency");

        private static readonly AndroidStatData[] AndroidStats = new AndroidStatData[3]
        {
            new AndroidStatData("Complexity", "VREA.ComplexityTotalDesc", GeneUtility.GCXTex.Texture),
            new AndroidStatData("VREA.PowerEfficiency", "VREA.PowerEfficiencyTotalDesc", PowerEfficiencyIcon),
            new AndroidStatData("VREA.ResourceCost", "VREA.ResourceCostDesc", ResourceCostIcon),
        };

        private static Dictionary<string, string> truncateCache = new Dictionary<string, string>();
        private static float MaxLabelWidth(List<ThingDefCount> resources)
        {
            float num = 0f;
            int num2 = AndroidStats.Length;
            if (resources is null)
            {
                num2--;
            }
            for (int i = 0; i < num2; i++)
            {
                num = Mathf.Max(num, Text.CalcSize(AndroidStats[i].labelKey.Translate()).x);
            }
            return num;
        }

        public static float HeightForBiostats(List<ThingDefCount> resources)
        {
            float num = Text.LineHeight * 3f;
            if (resources != null)
            {
                num += Text.LineHeight * 1.5f;
            }
            return num;
        }

        public static void Draw(Rect rect, int gcx, int met, List<ThingDefCount> resources)
        {
            int num = AndroidStats.Length;
            if (resources is null)
            {
                num--;
            }
            float num2 = MaxLabelWidth(resources);
            float num3 = rect.height / (float)num;
            GUI.BeginGroup(rect);
            for (int i = 0; i < num; i++)
            {
                Rect position = new Rect(0f, (float)i * num3 + (num3 - 22f) / 2f, 22f, 22f);
                Rect rect2 = new Rect(position.xMax + 4f, (float)i * num3, num2, num3);
                Rect rect3 = new Rect(0f, rect2.y, rect.width, rect2.height);
                if (i % 2 == 1)
                {
                    Widgets.DrawLightHighlight(rect3);
                }
                Widgets.DrawHighlightIfMouseover(rect3);
                rect3.xMax = rect2.xMax + 4f + 90f;
                TaggedString taggedString = AndroidStats[i].descKey.Translate();
                TooltipHandler.TipRegion(rect3, taggedString);
                GUI.DrawTexture(position, AndroidStats[i].icon);
                Text.Anchor = TextAnchor.MiddleLeft;
                Widgets.Label(rect2, AndroidStats[i].labelKey.Translate());
                Text.Anchor = TextAnchor.UpperLeft;
            }
            float num4 = num2 + 4f + 22f + 4f;
            string text = gcx.ToString();
            string text2 = met.ToStringWithSign();
            Text.Anchor = TextAnchor.MiddleCenter;
            Widgets.Label(new Rect(num4, 0f, 90f, num3), text);
            Widgets.Label(new Rect(num4, num3, 90f, num3), text2);
            if (resources != null)
            {
                Widgets.Label(new Rect(num4, num3 * 2f, 500, num3), string.Join(", ", resources.Select(x => x.count + " " + x.thingDef.label)));
            }
            Text.Anchor = TextAnchor.MiddleLeft;
            float width = rect.width - num2 - 90f - 22f - 4f;
            Rect rect4 = new Rect(num4 + 90f + 4f, num3, width, num3);
            if (rect4.width != cachedWidth)
            {
                cachedWidth = rect4.width;
                truncateCache.Clear();
            }
            string text3 = PowerEfficiencyDescAt(met);
            Widgets.Label(rect4, text3.Truncate(rect4.width, truncateCache));
            if (Mouse.IsOver(rect4) && !text3.NullOrEmpty())
            {
                TooltipHandler.TipRegion(rect4, text3);
            }
            Text.Anchor = TextAnchor.UpperLeft;
            GUI.EndGroup();
        }

        private static string PowerEfficiencyDescAt(int met)
        {
            if (met == 0)
            {
                return string.Empty;
            }
            return "VREA.PowerDrain".Translate() + " x" + PowerEfficiencyToPowerDrainFactorCurve.Evaluate(met).ToStringPercent();
        }

        public static readonly SimpleCurve PowerEfficiencyToPowerDrainFactorCurve = new SimpleCurve
        {
            new CurvePoint(-20f, 6.0f),
            new CurvePoint(0f, 1f),
            new CurvePoint(5f, 0.5f)
        };

    }
}
