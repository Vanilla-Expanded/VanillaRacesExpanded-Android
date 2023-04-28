using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI;

namespace VREAndroids
{
    public class MentalState_Reformatting : MentalState
    {
        public static int TicksToRecoverFromReformatting(Pawn pawn)
        {
            var multiplier = 1f;
            if (pawn.HasActiveGene(VREA_DefOf.VREA_SlowRAM))
            {
                multiplier *= 2f;
            }
            else if (pawn.HasActiveGene(VREA_DefOf.VREA_FastRAM))
            {
                multiplier *= 0.5f;
            }
            return (int)(600 * multiplier);
        }
        public override RandomSocialMode SocialModeMax()
        {
            return RandomSocialMode.Off;
        }
        public override void PreStart()
        {
            base.PreStart();
            this.pawn.stances.SetStance(new Stance_Stand(TicksToRecoverFromReformatting(pawn), this.pawn.Position + this.pawn.Rotation.FacingCell, null));
        }
        public override void MentalStateTick()
        {
            base.MentalStateTick();
            var memorySpace = this.pawn.needs.TryGetNeed<Need_MemorySpace>();
            memorySpace.curLevelInt = Mathf.Min(1f, memorySpace.curLevelInt + (1f / (float)TicksToRecoverFromReformatting(pawn)));
            if (memorySpace.curLevelInt == 1f)
            {
                this.RecoverFromState();
            }
        }

        public override void PostEnd()
        {
            base.PostEnd();
            this.pawn.stances.SetStance(new Stance_Mobile());
        }
    }
}
