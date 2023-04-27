using RimWorld;
using Verse;
using Verse.AI;

namespace VREAndroids
{
    public class MentalState_SolarFlared : MentalState
    {
        public override RandomSocialMode SocialModeMax()
        {
            return RandomSocialMode.Off;
        }
        public override void PreStart()
        {
            base.PreStart();
            if (this.pawn.stances != null)
            {
                this.pawn.stances.SetStance(new Stance_Stand(999999999, this.pawn.Position + this.pawn.Rotation.FacingCell, null));
            }
        }

        public override void MentalStateTick()
        {
            base.MentalStateTick();
            if (pawn.Spawned && pawn.Map.gameConditionManager.ElectricityDisabled || Find.World.gameConditionManager.ElectricityDisabled)
            {
                if (pawn.stances != null && pawn.stances.curStance is not Stance_Stand)
                {
                    this.pawn.stances.SetStance(new Stance_Stand(999999999, this.pawn.Position + this.pawn.Rotation.FacingCell, null));
                }
            }
            else
            {
                pawn.mindState.mentalStateHandler.CurState.RecoverFromState();
            }
        }

        public override void PostEnd()
        {
            base.PostEnd();
            if (pawn.Spawned)
            {
                pawn.Map.overlayDrawer.Disable(pawn, ref pawn.genes.GetFirstGeneOfType<Gene_SolarFlareVulnerability>().overlayPowerOff);
                pawn.stances.SetStance(new Stance_Mobile());
            }
        }
    }
}
