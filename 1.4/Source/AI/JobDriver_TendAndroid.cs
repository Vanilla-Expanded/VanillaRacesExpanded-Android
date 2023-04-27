using RimWorld;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace VREAndroids
{
    public class JobDriver_TendAndroid : JobDriver
    {
        protected Pawn Deliveree => job.targetA.Pawn;
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            if (Deliveree != pawn && !pawn.Reserve(Deliveree, job, 1, -1, null, errorOnFailed))
            {
                return false;
            }
            return true;
        }
        public override IEnumerable<Toil> MakeNewToils()
        {
            this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
            AddEndCondition(delegate
            {
                return Deliveree.health.HasHediffsNeedingTend() ? JobCondition.Ongoing : JobCondition.Succeeded;
            });
            this.FailOnAggroMentalState(TargetIndex.A);
            PathEndMode interactionCell = PathEndMode.None;
            if (Deliveree == pawn)
            {
                interactionCell = PathEndMode.OnCell;
            }
            else if (Deliveree.InBed())
            {
                interactionCell = PathEndMode.InteractionCell;
            }
            else if (Deliveree != pawn)
            {
                interactionCell = PathEndMode.ClosestTouch;
            }
            Toil gotoToil = Toils_Goto.GotoThing(TargetIndex.A, interactionCell);
            yield return gotoToil;
            int ticks = (int)(1f / pawn.GetStatValue(StatDefOf.GeneralLaborSpeed) * 600f);
            Toil waitToil;
            if (!job.draftedTend)
            {
                waitToil = Toils_General.Wait(ticks);
            }
            else
            {
                waitToil = Toils_General.WaitWith(TargetIndex.A, ticks, useProgressBar: false, maintainPosture: true);
                waitToil.AddFinishAction(delegate
                {
                    if (Deliveree != null && Deliveree != pawn && Deliveree.CurJob != null 
                    && (Deliveree.CurJob.def == JobDefOf.Wait || Deliveree.CurJob.def == JobDefOf.Wait_MaintainPosture))
                    {
                        Deliveree.jobs.EndCurrentJob(JobCondition.InterruptForced);
                    }
                });
            }
            waitToil.FailOnCannotTouch(TargetIndex.A, interactionCell).WithProgressBarToilDelay(TargetIndex.A)
                .PlaySustainerOrSound(VREA_DefOf.Interact_ConstructMetal);
            waitToil.activeSkill = () => SkillDefOf.Crafting;
            waitToil.handlingFacing = true;
            waitToil.WithEffect(EffecterDefOf.ConstructMetal, TargetIndex.A);
            waitToil.tickAction = delegate
            {
                if (pawn == Deliveree && pawn.Faction != Faction.OfPlayer 
                && pawn.IsHashIntervalTick(100) && !pawn.Position.Fogged(pawn.Map))
                {
                    FleckMaker.ThrowMetaIcon(pawn.Position, pawn.Map, FleckDefOf.HealingCross);
                }
                if (pawn != Deliveree)
                {
                    pawn.rotationTracker.FaceTarget(Deliveree);
                }
            };
            yield return waitToil;
            yield return Toils_Tend.FinalizeTend(Deliveree);
            yield return Toils_Jump.Jump(gotoToil);
        }

        public override void Notify_DamageTaken(DamageInfo dinfo)
        {
            base.Notify_DamageTaken(dinfo);
            if (dinfo.Def.ExternalViolenceFor(pawn) && pawn.Faction != Faction.OfPlayer && pawn == Deliveree)
            {
                pawn.jobs.CheckForJobOverride();
            }
        }
    }
}
