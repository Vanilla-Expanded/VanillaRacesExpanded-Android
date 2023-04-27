using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
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
                    skillRecord.Level = 0;
                    skillRecord.passion = Passion.None;
                }
                return false;
            }
            return true;
        }
    }
}
