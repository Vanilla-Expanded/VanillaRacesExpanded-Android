using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(PawnGenerator), "GenerateSkills")]
    public static class PawnGenerator_GenerateSkills_Patch
    {
        public static bool Prefix(Pawn pawn, PawnGenerationRequest request)
        {
            if (pawn.HasActiveGene(VREA_DefOf.VREA_NoSkillGain))
            {
                List<SkillDef> allDefsListForReading = DefDatabase<SkillDef>.AllDefsListForReading;
                for (int i = 0; i < allDefsListForReading.Count; i++)
                {
                    SkillDef skillDef = allDefsListForReading[i];
                    var skillRecord = pawn.skills.GetSkill(skillDef);
                    skillRecord.Level = FinalLevelOfSkill(pawn, skillDef, request);
                    skillRecord.passion = Passion.None;
                }
                return false;
            }
            return true;
        }

        private static int FinalLevelOfSkill(Pawn pawn, SkillDef sk, PawnGenerationRequest request)
        {
            float num = 0;
            foreach (BackstoryDef item in pawn.story.AllBackstories.Where((BackstoryDef bs) => bs != null))
            {
                foreach (KeyValuePair<SkillDef, int> skillGain in item.skillGains)
                {
                    if (skillGain.Key == sk)
                    {
                        num += (float)skillGain.Value * Rand.Range(1f, 1.4f);
                    }
                }
            }
            for (int i = 0; i < pawn.story.traits.allTraits.Count; i++)
            {
                int value = 0;
                if (!pawn.story.traits.allTraits[i].Suppressed && pawn.story.traits.allTraits[i].CurrentData.skillGains.TryGetValue(sk, out value))
                {
                    num += (float)value;
                }
            }
            if (num > 0f)
            {
                num += (float)pawn.kindDef.extraSkillLevels;
            }
            if (pawn.kindDef.skills != null)
            {
                foreach (SkillRange skill in pawn.kindDef.skills)
                {
                    if (skill.Skill == sk)
                    {
                        if (num < (float)skill.Range.min || num > (float)skill.Range.max)
                        {
                            num = skill.Range.RandomInRange;
                        }
                        break;
                    }
                }
            }
            return Mathf.Clamp(Mathf.RoundToInt(num), 0, 20);
        }

    }
}
