using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI;

namespace VREAndroids
{
    public class MentalState_Reformatting : MentalState
    {
        public static int TicksToRecoverFromReformatting(Pawn pawn, Building stand)
        {
            var multiplier = 1f;
            if (pawn.HasActiveGene(VREA_DefOf.VREA_SlowRAM))
            {
                multiplier = 2f;
            }
            else if (pawn.HasActiveGene(VREA_DefOf.VREA_FastRAM))
            {
                multiplier = 0.5f;
            }
            var standMultiplier = 1f;
            if (stand is null)
            {
                standMultiplier = 2f;
            }
            else if (stand.def == VREA_DefOf.VREA_AndroidStand)
            {
                standMultiplier = 0.2f;
            }
            return (int)(1200 * multiplier * standMultiplier);
        }
        public override RandomSocialMode SocialModeMax()
        {
            return RandomSocialMode.Off;
        }
        public override void PreStart()
        {
            base.PreStart();
            this.pawn.stances.SetStance(new Stance_Stand(TicksToRecoverFromReformatting(pawn, null), this.pawn.Position + this.pawn.Rotation.FacingCell, null));
        }
        private Mote moteCharging;

        public override void MentalStateTick()
        {
            base.MentalStateTick();
            var memorySpace = this.pawn.needs.TryGetNeed<Need_MemorySpace>();
            memorySpace.curLevelInt = Mathf.Min(1f, memorySpace.curLevelInt + (1f / (float)TicksToRecoverFromReformatting(pawn, null)));
            if (moteCharging == null || moteCharging.Destroyed)
            {
                moteCharging = MoteMaker.MakeAttachedOverlay(pawn, VREA_DefOf.VREA_Mote_AndroidReformatting, Vector3.zero);
            }
            moteCharging?.Maintain();
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
