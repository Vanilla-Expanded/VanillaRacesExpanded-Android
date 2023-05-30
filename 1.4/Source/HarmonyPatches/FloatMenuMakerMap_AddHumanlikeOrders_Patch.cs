using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(FloatMenuMakerMap), "AddHumanlikeOrders")]
    public static class FloatMenuMakerMap_AddHumanlikeOrders_Patch
    {
        public static void Postfix(Vector3 clickPos, Pawn pawn, ref List<FloatMenuOption> opts)
        {
            if (pawn.IsAndroid())
            {
                var neutroloss = pawn.health.hediffSet.GetFirstHediffOfDef(VREA_DefOf.VREA_NeutroLoss);
                if (neutroloss != null)
                {
                    IntVec3 c = IntVec3.FromVector3(clickPos);
                    List<Thing> thingList = c.GetThingList(pawn.Map);
                    for (int i = 0; i < thingList.Count; i++)
                    {
                        var thing = thingList[i];
                        if (thingList[i].def == VREA_DefOf.Neutroamine)
                        {
                            opts.Add(new FloatMenuOption("VREA.RefuelWithNeutroamine".Translate(), delegate
                            {
                                var job = JobMaker.MakeJob(VREA_DefOf.VREA_RefuelWithNeutroamine, thing);
                                var amount = Mathf.CeilToInt(neutroloss.Severity * 100f);
                                amount = Mathf.Min(thing.stackCount, amount);
                                job.count = amount;
                                pawn.jobs.TryTakeOrderedJob(job);
                            }));
                        }
                    }
                }
            }

        }
    }
}
