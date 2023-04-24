using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI;

namespace VREAndroids
{
    public class MentalState_Reformatting : MentalState
    {
        public override RandomSocialMode SocialModeMax()
        {
            return RandomSocialMode.Off;
        }
        public override void PreStart()
        {
            base.PreStart();
            this.pawn.stances.SetStance(new Stance_Stand(Hediff_Android.TicksToRecoverFromReformatting, this.pawn.Position + this.pawn.Rotation.FacingCell, null));
        }
        public override void MentalStateTick()
        {
            base.MentalStateTick();
            var memorySpace = this.pawn.needs.TryGetNeed<Need_MemorySpace>();
            memorySpace.curLevelInt = Mathf.Min(1f, memorySpace.curLevelInt + (1f / (float)Hediff_Android.TicksToRecoverFromReformatting));
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
