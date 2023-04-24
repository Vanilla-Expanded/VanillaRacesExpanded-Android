using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace VREAndroids
{
    public class Need_MemorySpace : Need
    {
        public Need_MemorySpace(Pawn pawn)
            : base(pawn)
        {

        }
        public override void SetInitialLevel()
        {
            this.curLevelInt = Rand.Range(0.1f, 0.9f);
        }
        public override void NeedInterval()
        {
            curLevelInt = Mathf.Max(0, curLevelInt - ((1f / GenDate.TicksPerDay) * 150f * pawn.GetStatValue(VREA_DefOf.VREA_MemorySpaceDrainMultiplier)));
            if (curLevelInt == 0f && pawn.MentalStateDef != VREA_DefOf.VREA_Reformatting)
            {
                if (pawn.InMentalState)
                {
                    pawn.mindState.mentalStateHandler.CurState.RecoverFromState();
                }
                pawn.mindState.mentalStateHandler.TryStartMentalState(VREA_DefOf.VREA_Reformatting);
            }
        }
    }
}
