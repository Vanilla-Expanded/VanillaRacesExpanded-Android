using HarmonyLib;
using RimWorld;
using System.Linq;
using UnityEngine;
using Verse;

namespace VREAndroids
{

    [HarmonyPatch(typeof(StunHandler), "Notify_DamageApplied")]
    public static class StunHandler_Notify_DamageApplied_Patch
    {
        public static void Postfix(StunHandler __instance, DamageInfo dinfo)
        {
            if (__instance.parent is Pawn pawn && pawn.HasActiveGene(VREA_DefOf.VREA_EMPVulnerability))
            {
                if (pawn != null && (pawn.Downed || pawn.Dead))
                {
                    return;
                }
                if (dinfo.Def == DamageDefOf.EMP)
                {
                    pawn.stances.stunner.stunFromEMP = true;
                    var downedDuration = (int)(dinfo.Amount * 100);
                    var hediff = HediffMaker.MakeHediff(VREA_DefOf.VREA_ElectromagneticShock, pawn);
                    hediff.TryGetComp<HediffComp_Disappears>().ticksToDisappear = downedDuration;
                    pawn.health.AddHediff(hediff);
                    var nonMissingParts = pawn.health.hediffSet.GetNotMissingParts();
                    var totalDamageToDeal = dinfo.Amount;
                    while (totalDamageToDeal > 0 && nonMissingParts.Any())
                    {
                        var damageToDeal = Mathf.Min(totalDamageToDeal, Rand.RangeInclusive(3, 5));
                        totalDamageToDeal -= damageToDeal;
                        pawn.TakeDamage(new DamageInfo(VREA_DefOf.VREA_EMPBurn, damageToDeal, instigator: dinfo.Instigator, hitPart: nonMissingParts.RandomElement()));
                    }
                }
            }
        }
    }
}
