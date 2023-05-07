using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(PawnUtility), "GetPosture")]
    public static class PawnUtility_GetPosture_Patch
    {
        public static bool isPawnRendering;
        public static bool Prefix(Pawn p, ref PawnPosture __result)
        {
            if (isPawnRendering && p.Spawned)
            {
                List<Thing> thingList = p.Position.GetThingList(p.Map);
                for (int i = 0; i < thingList.Count; i++)
                {
                    if (thingList[i] is Building_AndroidStand)
                    {
                        __result = PawnPosture.Standing;
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
