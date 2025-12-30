using RimWorld;
using UnityEngine;
using Verse;

namespace VREAndroids
{
    public class Gene_MemoryDecay : Gene
    {
        private const int DecayInterval = GenDate.TicksPerDay * 15;

        public int skillDecayOffset;

        public int lastTickDecayCheck;
        public override void PostAdd()
        {
            base.PostAdd();
            lastTickDecayCheck = Find.TickManager.TicksGame;
        }

        public override void TickInterval(int delta)
        {
            base.TickInterval(delta);
            if (Find.TickManager.TicksGame - lastTickDecayCheck > DecayInterval)
            {
                skillDecayOffset--;
                pawn.skills.DirtyAptitudes();
                // To compensate for TickInterval, rather than setting last check tick to current ticks, increment by 15 days.
                // Setting to current tick would slowly drift the time at which we do this check.
                var prevDecay = lastTickDecayCheck;
                lastTickDecayCheck += DecayInterval;
                // However, if the pawn wasn't ticking during that time, it would immediately cause another decay tick right after.
                // If we set it to a value that's more than (delta + 5) ticks from this point, instead just use current ticks.
                if (lastTickDecayCheck - delta - 5 < Find.TickManager.TicksGame)
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
