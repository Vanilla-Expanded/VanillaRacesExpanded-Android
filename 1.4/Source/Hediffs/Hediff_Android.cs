using RimWorld;
using System.Linq;
using Verse;

namespace VREAndroids
{
    [HotSwappable]
    public class Hediff_Android : HediffWithComps
    {
        public OverlayHandle? overlayPowerOff;
        public const int TicksToRecoverFromReformatting = 600;
        public override bool ShouldRemove => false;
        public AndroidState AndroidState
        {
            get
            {
                if (this.CurStageIndex == 0)
                {
                    return AndroidState.Normal;
                }
                return AndroidState.Awakened;
            }
        }

        public override void Tick()
        {
            base.Tick();
            if (pawn.Spawned && pawn.Map.gameConditionManager.ElectricityDisabled || Find.World.gameConditionManager.ElectricityDisabled)
            {
                if (pawn.stances != null && pawn.stances.curStance is not Stance_Stand)
                {
                    pawn.stances.SetStance(new Stance_Stand(9999999, pawn.Position + pawn.Rotation.FacingCell, null));
                }
                if (pawn.MentalStateDef != VREA_DefOf.VREA_SolarFlared)
                {
                    if (pawn.InMentalState)
                    {
                        pawn.mindState.mentalStateHandler.CurState.RecoverFromState();
                    }
                    pawn.mindState.mentalStateHandler.TryStartMentalState(VREA_DefOf.VREA_SolarFlared);
                }

                if (overlayPowerOff is null)
                {
                    if (pawn.Spawned)
                    {
                        overlayPowerOff = pawn.Map.overlayDrawer.Enable(pawn, OverlayTypes.PowerOff);
                    }
                }
            }
        }
        public override void PostAdd(DamageInfo? dinfo)
        {
            base.PostAdd(dinfo);
            foreach (var bodyPart in this.pawn.def.race.body.AllParts.OrderByDescending(x => x.Index))
            {
                var hediffDef = bodyPart.def.GetAndroidCounterPart();
                if (hediffDef != null)
                {
                    var hediff = HediffMaker.MakeHediff(hediffDef, pawn, bodyPart);
                    pawn.health.AddHediff(hediff, bodyPart);
                }
            }
            MeditationFocusTypeAvailabilityCache.ClearFor(pawn);
        }
    }
}
