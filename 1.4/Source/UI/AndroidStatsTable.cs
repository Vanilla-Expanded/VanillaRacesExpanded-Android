using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace VREAndroids
{
    [HotSwappable]
    [StaticConstructorOnStartup]
    public static class AndroidStatsTable
    {
        private struct AndroidStatData
        {
            public string labelKey;

            public string descKey;

            public Texture2D icon;

            public bool displayMaxGCXInfo;

            public AndroidStatData(string labelKey, string descKey, Texture2D icon, bool displayMaxGCXInfo)
            {
                this.labelKey = labelKey;
                this.descKey = descKey;
                this.icon = icon;
                this.displayMaxGCXInfo = displayMaxGCXInfo;
            }
        }

        private static float cachedWidth;

        private const float NumberWidth = 90f;

        private const float IconSize = 22f;

        public static readonly Texture2D PowerEfficiencyIcon = ContentFinder<Texture2D>.Get("UI/BiostatIcon/BiostatEfficiency");
        public static readonly CachedTexture PowerEfficiencyIconTex = new CachedTexture("UI/BiostatIcon/BiostatEfficiency");

        private static readonly AndroidStatData[] AndroidStats = new AndroidStatData[2]
        {
            new AndroidStatData("Complexity", "ComplexityDesc", GeneUtility.GCXTex.Texture, displayMaxGCXInfo: true),
            new AndroidStatData("VREA.PowerEfficiency", "VREA.PowerEfficiencyDesc", PowerEfficiencyIcon, displayMaxGCXInfo: false),
        };

        private static Dictionary<string, string> truncateCache = new Dictionary<string, string>();

        private static float MaxLabelWidth()
        {
            float num = 0f;
            int num2 = AndroidStats.Length;
            for (int i = 0; i < num2; i++)
            {
                num = Mathf.Max(num, Text.CalcSize(AndroidStats[i].labelKey.Translate()).x);
            }
            return num;
        }

        public static float HeightForBiostats()
        {
            float num = Text.LineHeight * 3f;
            return num;
        }

        public static void Draw(Rect rect, int gcx, int met, bool drawMax, bool ignoreLimits, int maxGCX = -1)
        {
            int num = AndroidStats.Length;
            float num2 = MaxLabelWidth();
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
                if (maxGCX >= 0 && AndroidStats[i].displayMaxGCXInfo)
                {
                    taggedString += "\n\n" + "MaxComplexityDesc".Translate();
                }
                TooltipHandler.TipRegion(rect3, taggedString);
                GUI.DrawTexture(position, AndroidStats[i].icon);
                Text.Anchor = TextAnchor.MiddleLeft;
                Widgets.Label(rect2, AndroidStats[i].labelKey.Translate());
                Text.Anchor = TextAnchor.UpperLeft;
            }
            float num4 = num2 + 4f + 22f + 4f;
            string text = gcx.ToString();
            string text2 = met.ToStringWithSign();
            if (drawMax && !ignoreLimits)
            {
                if (maxGCX >= 0)
                {
                    if (gcx > maxGCX)
                    {
                        text = text.Colorize(ColorLibrary.RedReadable);
                    }
                    text = text + " / " + maxGCX;
                }
            }
            Text.Anchor = TextAnchor.MiddleCenter;
            Widgets.Label(new Rect(num4, 0f, 90f, num3), text);
            Widgets.Label(new Rect(num4, num3, 90f, num3), text2);
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
            new CurvePoint(-5f, 2.25f),
            new CurvePoint(0f, 1f),
            new CurvePoint(5f, 0.5f)
        };

    }
}
