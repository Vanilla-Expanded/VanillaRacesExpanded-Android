using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(GeneDef), "GetDescriptionFull")]
    public static class GeneDef_GetDescriptionFull_Patch
    {
        public static bool Prefix(GeneDef __instance, ref string __result)
        {
            if (__instance.IsAndroidGene())
            {
                __result = GetDescriptionFull(__instance);
                return false;
            }
            return true;
        }


        private static string GetDescriptionFull(GeneDef __instance)
        {
            StringBuilder sb = new StringBuilder();
            if (!__instance.description.NullOrEmpty())
            {
                sb.Append(__instance.description).AppendLine().AppendLine();
            }
            bool flag = false;
            if (__instance.prerequisite != null)
            {
                sb.AppendLine("Requires".Translate() + ": " + __instance.prerequisite.LabelCap);
                flag = true;
            }
            if (__instance.minAgeActive > 0f)
            {
                sb.AppendLine((string)("TakesEffectAfterAge".Translate() + ": ") + __instance.minAgeActive);
                flag = true;
            }
            if (flag)
            {
                sb.AppendLine();
            }
            bool flag2 = false;
            if (__instance.biostatCpx != 0)
            {
                sb.AppendLineTagged("Complexity".Translate().Colorize(GeneUtility.GCXColor) + ": " + __instance.biostatCpx.ToStringWithSign());
                flag2 = true;
            }
            if (__instance.biostatMet != 0)
            {
                sb.AppendLineTagged("VREA.PowerEfficiency".Translate().Colorize(SimpleColor.Yellow.ToUnityColor()) + ": " + __instance.biostatMet.ToStringWithSign());
                flag2 = true;
            }
            if (__instance.biostatArc != 0)
            {
                sb.AppendLineTagged("ArchitesRequired".Translate().Colorize(GeneUtility.ARCColor) + ": " + __instance.biostatArc.ToStringWithSign());
                flag2 = true;
            }
            if (flag2)
            {
                sb.AppendLine();
            }
            if (__instance.forcedTraits != null)
            {
                sb.AppendLineTagged(("ForcedTraits".Translate() + ":").Colorize(ColoredText.TipSectionTitleColor));
                sb.Append(__instance.forcedTraits.Select((GeneticTraitData x) => x.def.DataAtDegree(x.degree).label).ToLineList("  - ", capitalizeItems: true)).AppendLine().AppendLine();
            }
            if (__instance.suppressedTraits != null)
            {
                sb.AppendLineTagged(("SuppressedTraits".Translate() + ":").Colorize(ColoredText.TipSectionTitleColor));
                sb.Append(__instance.suppressedTraits.Select((GeneticTraitData x) => x.def.DataAtDegree(x.degree).label).ToLineList("  - ", capitalizeItems: true)).AppendLine().AppendLine();
            }
            if (__instance.aptitudes != null)
            {
                sb.AppendLineTagged(("Aptitudes".Translate().CapitalizeFirst() + ":").Colorize(ColoredText.TipSectionTitleColor));
                sb.Append(__instance.aptitudes.Select((Aptitude x) => x.skill.LabelCap.ToString() + " " + x.level.ToStringWithSign()).ToLineList("  - ", capitalizeItems: true)).AppendLine().AppendLine();
            }
            bool effectsTitleWritten = false;
            if (__instance.passionMod != null)
            {
                switch (__instance.passionMod.modType)
                {
                    case PassionMod.PassionModType.AddOneLevel:
                        AppendEffectLine("PassionModAdd".Translate(__instance.passionMod.skill));
                        break;
                    case PassionMod.PassionModType.DropAll:
                        AppendEffectLine("PassionModDrop".Translate(__instance.passionMod.skill));
                        break;
                }
            }
            if (!__instance.statFactors.NullOrEmpty())
            {
                for (int i = 0; i < __instance.statFactors.Count; i++)
                {
                    StatModifier statModifier = __instance.statFactors[i];
                    if (statModifier.stat.CanShowWithLoadedMods())
                    {
                        AppendEffectLine(statModifier.stat.LabelCap + " " + statModifier.ToStringAsFactor);
                    }
                }
            }
            if (!__instance.conditionalStatAffecters.NullOrEmpty())
            {
                for (int j = 0; j < __instance.conditionalStatAffecters.Count; j++)
                {
                    if (__instance.conditionalStatAffecters[j].statFactors.NullOrEmpty())
                    {
                        continue;
                    }
                    for (int k = 0; k < __instance.conditionalStatAffecters[j].statFactors.Count; k++)
                    {
                        StatModifier statModifier2 = __instance.conditionalStatAffecters[j].statFactors[k];
                        if (statModifier2.stat.CanShowWithLoadedMods())
                        {
                            AppendEffectLine(statModifier2.stat.LabelCap + " " + statModifier2.ToStringAsFactor + " (" + __instance.conditionalStatAffecters[j].Label + ")");
                        }
                    }
                }
            }
            if (!__instance.statOffsets.NullOrEmpty())
            {
                for (int l = 0; l < __instance.statOffsets.Count; l++)
                {
                    StatModifier statModifier3 = __instance.statOffsets[l];
                    if (statModifier3.stat.CanShowWithLoadedMods())
                    {
                        AppendEffectLine(statModifier3.stat.LabelCap + " " + statModifier3.ValueToStringAsOffset);
                    }
                }
            }
            if (!__instance.conditionalStatAffecters.NullOrEmpty())
            {
                for (int m = 0; m < __instance.conditionalStatAffecters.Count; m++)
                {
                    if (__instance.conditionalStatAffecters[m].statOffsets.NullOrEmpty())
                    {
                        continue;
                    }
                    for (int n = 0; n < __instance.conditionalStatAffecters[m].statOffsets.Count; n++)
                    {
                        StatModifier statModifier4 = __instance.conditionalStatAffecters[m].statOffsets[n];
                        if (statModifier4.stat.CanShowWithLoadedMods())
                        {
                            AppendEffectLine(statModifier4.stat.LabelCap + " " + statModifier4.ValueToStringAsOffset + " (" + __instance.conditionalStatAffecters[m].Label.UncapitalizeFirst() + ")");
                        }
                    }
                }
            }
            if (!__instance.capMods.NullOrEmpty())
            {
                for (int num = 0; num < __instance.capMods.Count; num++)
                {
                    PawnCapacityModifier pawnCapacityModifier = __instance.capMods[num];
                    if (pawnCapacityModifier.offset != 0f)
                    {
                        AppendEffectLine(pawnCapacityModifier.capacity.GetLabelFor(isFlesh: true, isHumanlike: true).CapitalizeFirst() + " " + (pawnCapacityModifier.offset * 100f).ToString("+#;-#") + "%");
                    }
                    if (pawnCapacityModifier.postFactor != 1f)
                    {
                        AppendEffectLine(pawnCapacityModifier.capacity.GetLabelFor(isFlesh: true, isHumanlike: true).CapitalizeFirst() + " x" + pawnCapacityModifier.postFactor.ToStringPercent());
                    }
                    if (pawnCapacityModifier.setMax != 999f)
                    {
                        AppendEffectLine(pawnCapacityModifier.capacity.GetLabelFor(isFlesh: true, isHumanlike: true).CapitalizeFirst() + " " + "max".Translate().CapitalizeFirst() + ": " + pawnCapacityModifier.setMax.ToStringPercent());
                    }
                }
            }
            if (!__instance.customEffectDescriptions.NullOrEmpty())
            {
                foreach (string customEffectDescription in __instance.customEffectDescriptions)
                {
                    AppendEffectLine(customEffectDescription.ResolveTags());
                }
            }
            if (!__instance.damageFactors.NullOrEmpty())
            {
                for (int num2 = 0; num2 < __instance.damageFactors.Count; num2++)
                {
                    AppendEffectLine("DamageType".Translate(__instance.damageFactors[num2].damageDef.label).CapitalizeFirst() + " x" + __instance.damageFactors[num2].factor.ToStringPercent());
                }
            }
            if (__instance.resourceLossPerDay != 0f && !__instance.resourceLabel.NullOrEmpty())
            {
                AppendEffectLine("ResourceLossPerDay".Translate(__instance.resourceLabel.Named("RESOURCE"),
                    (-Mathf.RoundToInt(__instance.resourceLossPerDay * 100f)).ToStringWithSign().Named("OFFSET")).CapitalizeFirst());
            }
            if (__instance.painFactor != 1f)
            {
                AppendEffectLine("Pain".Translate() + " x" + __instance.painFactor.ToStringPercent());
            }
            if (__instance.painOffset != 0f)
            {
                AppendEffectLine("Pain".Translate() + " " + (__instance.painOffset * 100f).ToString("+###0;-###0") + "%");
            }
            if (__instance.chemical != null)
            {
                if (__instance.addictionChanceFactor != 1f)
                {
                    if (__instance.addictionChanceFactor <= 0f)
                    {
                        AppendEffectLine("AddictionImmune".Translate(__instance.chemical).CapitalizeFirst());
                    }
                    else
                    {
                        AppendEffectLine("AddictionChanceFactor".Translate(__instance.chemical).CapitalizeFirst() + " x" + __instance.addictionChanceFactor.ToStringPercent());
                    }
                }
                if (__instance.overdoseChanceFactor != 1f)
                {
                    AppendEffectLine("OverdoseChanceFactor".Translate(__instance.chemical).CapitalizeFirst() + " x" + __instance.overdoseChanceFactor.ToStringPercent());
                }
                if (__instance.toleranceBuildupFactor != 1f)
                {
                    AppendEffectLine("ToleranceBuildupFactor".Translate(__instance.chemical).CapitalizeFirst() + " x" + __instance.toleranceBuildupFactor.ToStringPercent());
                }
            }
            if (__instance.causesNeed != null)
            {
                AppendEffectLine("CausesNeed".Translate() + ": " + __instance.causesNeed.LabelCap);
            }
            if (!__instance.disablesNeeds.NullOrEmpty())
            {
                if (__instance.disablesNeeds.Count == 1)
                {
                    AppendEffectLine("DisablesNeed".Translate() + ": " + __instance.disablesNeeds[0].LabelCap);
                }
                else
                {
                    AppendEffectLine("DisablesNeeds".Translate() + ": " + __instance.disablesNeeds.Select((NeedDef x) => x.label).ToCommaList().CapitalizeFirst());
                }
            }
            if (__instance.missingGeneRomanceChanceFactor != 1f)
            {
                AppendEffectLine("MissingGeneRomanceChance".Translate(__instance.label.Named("GENE")) + " x" + __instance.missingGeneRomanceChanceFactor.ToStringPercent());
            }
            if (__instance.ignoreDarkness)
            {
                AppendEffectLine("UnaffectedByDarkness".Translate());
            }
            if (__instance.foodPoisoningChanceFactor != 1f)
            {
                if (__instance.foodPoisoningChanceFactor <= 0f)
                {
                    AppendEffectLine("FoodPoisoningImmune".Translate());
                }
                else
                {
                    AppendEffectLine("Stat_Hediff_FoodPoisoningChanceFactor_Name".Translate() + " x" + __instance.foodPoisoningChanceFactor.ToStringPercent());
                }
            }
            if (__instance.socialFightChanceFactor != 1f)
            {
                if (__instance.socialFightChanceFactor <= 0f)
                {
                    AppendEffectLine("WillNeverSocialFight".Translate());
                }
                else
                {
                    AppendEffectLine("SocialFightChanceFactor".Translate() + " x" + __instance.socialFightChanceFactor.ToStringPercent());
                }
            }
            if (__instance.aggroMentalBreakSelectionChanceFactor != 1f)
            {
                if (__instance.aggroMentalBreakSelectionChanceFactor >= 999f)
                {
                    AppendEffectLine("AlwaysAggroMentalBreak".Translate());
                }
                else if (__instance.aggroMentalBreakSelectionChanceFactor <= 0f)
                {
                    AppendEffectLine("NeverAggroMentalBreak".Translate());
                }
                else
                {
                    AppendEffectLine("AggroMentalBreakSelectionChanceFactor".Translate() + " x" + __instance.aggroMentalBreakSelectionChanceFactor.ToStringPercent());
                }
            }
            if (__instance.prisonBreakMTBFactor != 1f)
            {
                if (__instance.prisonBreakMTBFactor < 0f)
                {
                    AppendEffectLine("WillNeverPrisonBreak".Translate());
                }
                else
                {
                    AppendEffectLine("PrisonBreakIntervalFactor".Translate() + " x" + __instance.prisonBreakMTBFactor.ToStringPercent());
                }
            }
            bool flag3 = effectsTitleWritten;
            if (!__instance.makeImmuneTo.NullOrEmpty())
            {
                if (flag3)
                {
                    sb.AppendLine();
                }
                sb.AppendLineTagged(("ImmuneTo".Translate() + ":").Colorize(ColoredText.TipSectionTitleColor));
                sb.AppendLine(__instance.makeImmuneTo.Select((HediffDef x) => x.label).ToLineList("  - ", capitalizeItems: true));
                flag3 = true;
            }
            if (!__instance.hediffGiversCannotGive.NullOrEmpty())
            {
                if (flag3)
                {
                    sb.AppendLine();
                }
                sb.AppendLineTagged(("ImmuneTo".Translate() + ":").Colorize(ColoredText.TipSectionTitleColor));
                sb.AppendLine(__instance.hediffGiversCannotGive.Select((HediffDef x) => x.label).ToLineList("  - ", capitalizeItems: true));
                flag3 = true;
            }
            if (__instance.biologicalAgeTickFactorFromAgeCurve != null)
            {
                if (flag3)
                {
                    sb.AppendLine();
                }
                sb.AppendLineTagged(("AgeFactors".Translate().CapitalizeFirst() + ":").Colorize(ColoredText.TipSectionTitleColor));
                sb.AppendLine(__instance.biologicalAgeTickFactorFromAgeCurve.Select((CurvePoint p) => "PeriodYears".Translate(p.x).ToString() + ": x" + p.y.ToStringPercent()).ToLineList("  - ", capitalizeItems: true));
                flag3 = true;
            }
            if (__instance.disabledWorkTags != 0)
            {
                if (flag3)
                {
                    sb.AppendLine();
                }
                IEnumerable<WorkTypeDef> source = DefDatabase<WorkTypeDef>.AllDefsListForReading.Where((WorkTypeDef x) => (__instance.disabledWorkTags & x.workTags) != 0);
                sb.AppendLineTagged(("DisabledWorkLabel".Translate().CapitalizeFirst() + ":").Colorize(ColoredText.TipSectionTitleColor));
                sb.AppendLine("  - " + source.Select((WorkTypeDef x) => x.labelShort).ToCommaList().CapitalizeFirst());
                __instance.disabledWorkTags.LabelTranslated();
                if (__instance.disabledWorkTags.ExactlyOneWorkTagSet())
                {
                    sb.AppendLine("  - " + __instance.disabledWorkTags.LabelTranslated().CapitalizeFirst());
                }
                flag3 = true;
            }
            if (!__instance.abilities.NullOrEmpty())
            {
                if (flag3)
                {
                    sb.AppendLine();
                }
                if (__instance.abilities.Count == 1)
                {
                    sb.AppendLineTagged(("GivesAbility".Translate().CapitalizeFirst() + ":").Colorize(ColoredText.TipSectionTitleColor));
                }
                else
                {
                    sb.AppendLineTagged(("GivesAbilities".Translate().CapitalizeFirst() + ":").Colorize(ColoredText.TipSectionTitleColor));
                }
                sb.AppendLine(__instance.abilities.Select((AbilityDef x) => x.label).ToLineList("  - ", capitalizeItems: true));
                flag3 = true;
            }
            IEnumerable<ThoughtDef> enumerable = DefDatabase<ThoughtDef>.AllDefs.Where((ThoughtDef x) => (x.requiredGenes.NotNullAndContains(__instance) || x.nullifyingGenes.NotNullAndContains(__instance)) && x.stages != null && x.stages.Any((ThoughtStage y) => y.baseMoodEffect != 0f));
            if (enumerable.Any())
            {
                if (flag3)
                {
                    sb.AppendLine();
                }
                sb.AppendLineTagged(("Mood".Translate().CapitalizeFirst() + ":").Colorize(ColoredText.TipSectionTitleColor));
                foreach (ThoughtDef item in enumerable)
                {
                    ThoughtStage thoughtStage = item.stages.FirstOrDefault((ThoughtStage x) => x.baseMoodEffect != 0f);
                    if (thoughtStage != null)
                    {
                        string text2 = thoughtStage.LabelCap + ": " + thoughtStage.baseMoodEffect.ToStringWithSign();
                        if (item.requiredGenes.NotNullAndContains(__instance))
                        {
                            sb.AppendLine("  - " + text2);
                        }
                        else if (item.nullifyingGenes.NotNullAndContains(__instance))
                        {
                            sb.AppendLine("  - " + "Removes".Translate() + ": " + text2);
                        }
                    }
                }
            }
            return sb.ToString().TrimEndNewlines();
            void AppendEffectLine(string text)
            {
                if (!effectsTitleWritten)
                {
                    sb.AppendLineTagged(("Effects".Translate().CapitalizeFirst() + ":").Colorize(ColoredText.TipSectionTitleColor));
                    effectsTitleWritten = true;
                }
                sb.AppendLine("  - " + text);
            }
        }
    }
}
