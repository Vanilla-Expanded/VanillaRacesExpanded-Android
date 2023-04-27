using HarmonyLib;
using System;
using System.Linq;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(Messages), "Message", new Type[] { typeof(string), typeof(LookTargets), typeof(MessageTypeDef), typeof(bool)})]
    public static class Messages_Message_Patch
    {
        public static void Prefix(ref string text, LookTargets lookTargets, MessageTypeDef def, bool historical = true)
        {
            if (lookTargets.targets.FirstOrDefault().Thing is Pawn pawn && pawn.HasActiveGene(VREA_DefOf.VREA_NeutroCirculation))
            {
                if (text == "CannotRescue".Translate() + ": " + (string)"NoNonPrisonerBed".Translate())
                {
                    text = "CannotRescue".Translate() + ": " + (string)"VREA.NoNeutroCasket".Translate();
                }
            }
        }
    }
}
