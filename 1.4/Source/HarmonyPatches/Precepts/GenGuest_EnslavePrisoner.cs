using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using Verse;
using System;

namespace VREAndroids
{
    [HarmonyPatch(typeof(GenGuest), "EnslavePrisoner")]
    public static class GenGuest_EnslavePrisoner_Patch
    {
        public static bool Prefix(Pawn warden, Pawn prisoner)
        {
            if ( (warden.Ideo?.HasPrecept(VREA_DefOf.VRE_Androids_Tools) == true && prisoner.IsAndroid()))
            {
                if (!prisoner.IsSlave)
                {
                   
                    prisoner.guest.SetGuestStatus(warden.Faction, GuestStatus.Slave);
                    Messages.Message("MessagePrisonerEnslaved".Translate(prisoner, warden), new LookTargets(prisoner, warden), MessageTypeDefOf.NeutralEvent);
                    prisoner.apparel.UnlockAll();
                }

                return false;
            }
            return true;
        }
    }
}
