using RimWorld;
using UnityEngine;
using Verse;

namespace VREAndroids
{
    public class Gene_MemoryDecay : Gene
    {
        public int skillDecayOffset;

        public int lastTickDecayCheck;
        public override void PostAdd()
        {
            base.PostAdd();
            lastTickDecayCheck = Find.TickManager.TicksGame;
        }
        public override void Tick()
        {
            base.Tick();
            if (Find.TickManager.TicksGame - lastTickDecayCheck > GenDate.TicksPerDay * 15)
            {
                skillDecayOffset--;
                pawn.skills.Notify_GenesChanged();
                lastTickDecayCheck = Find.TickManager.TicksGame;
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref skillDecayOffset, "skillDecayOffset");
            Scribe_Values.Look(ref lastTickDecayCheck, "lastTickDecayCheck");
        }
    }
}
