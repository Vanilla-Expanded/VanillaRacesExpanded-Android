using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace VREAndroids
{
    public class JobDriver_RepairAndroid : JobDriver
    {
        protected int ticksToNextRepair;
        protected Pawn Patient => (Pawn)job.GetTarget(TargetIndex.A).Thing;
        protected int TicksPerHeal => 200;
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            if (pawn.jobs.jobsGivenThisTick > 8)
            {
                pawn.jobs.debugLog = true;
            }
            return pawn.Reserve(Patient, job, 1, -1, null, errorOnFailed);
        }
        public override IEnumerable<Toil> MakeNewToils()
        {
            this.FailOnDestroyedOrNull(TargetIndex.A);
            this.FailOnForbidden(TargetIndex.A);
            var gotoToil = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell);
            int ticks = (int)(1f / pawn.GetStatValue(StatDefOf.GeneralLaborSpeed) * 600f);
            Toil tendToil = Toils_General.Wait(ticks);
            tendToil.WithProgressBarToilDelay(TargetIndex.A).PlaySustainerOrSound(VREA_DefOf.Interact_ConstructMetal);
            if (pawn != Patient)
            {
                tendToil.FailOnCannotTouch(TargetIndex.A, PathEndMode.InteractionCell);
            }
            tendToil.activeSkill = () => SkillDefOf.Crafting;
            tendToil.handlingFacing = true;
            tendToil.WithEffect(EffecterDefOf.MechRepairing, TargetIndex.A);
            tendToil.PlaySustainerOrSound(SoundDefOf.RepairMech_Touch);
            tendToil.tickAction = delegate
            {
                if (pawn != Patient)
                {
                    pawn.rotationTracker.FaceTarget(Patient);
                }
            };

            Toil repairToil = Toils_General.WaitWith(TargetIndex.A, int.MaxValue,
                useProgressBar: false, maintainPosture: true, maintainSleep: true);
            repairToil.WithEffect(EffecterDefOf.MechRepairing, TargetIndex.A);
            repairToil.PlaySustainerOrSound(SoundDefOf.RepairMech_Touch);
            repairToil.AddPreInitAction(delegate
            {
                ticksToNextRepair = TicksPerHeal;
            });
            repairToil.handlingFacing = true;
            repairToil.tickAction = delegate
            {
                ticksToNextRepair--;
                if (ticksToNextRepair <= 0)
                {
                    RepairTick(Patient);
                    ticksToNextRepair = TicksPerHeal;
                }
                pawn.rotationTracker.FaceTarget(Patient);
                if (pawn.skills != null)
                {
                    pawn.skills.Learn(SkillDefOf.Crafting, 0.05f);
                }
            };

            if (pawn != Patient)
            {
                repairToil.FailOnCannotTouch(TargetIndex.A, PathEndMode.InteractionCell);
            }
            repairToil.AddEndCondition(() => CanRepairAndroid(Patient) ? JobCondition.Ongoing : JobCondition.Succeeded);
            repairToil.activeSkill = () => SkillDefOf.Crafting;
            if (pawn != Patient)
            {
                yield return gotoToil;
            }
            yield return Toils_Jump.JumpIf(repairToil, () => Patient.health.HasHediffsNeedingTend() is false);
            yield return tendToil;
            yield return FinalizeTend(Patient);
            yield return Toils_Jump.Jump(gotoToil);
            yield return repairToil;
            AddFinishAction(() =>
            {
                if (Patient != null && Patient != pawn && Patient.CurJob != null
                    && (Patient.CurJob.def == JobDefOf.Wait || Patient.CurJob.def == JobDefOf.Wait_MaintainPosture))
                {
                    Patient.jobs.EndCurrentJob(JobCondition.InterruptForced);
                }
            });
        }

        public static Toil FinalizeTend(Pawn patient)
        {
            Toil toil = ToilMaker.MakeToil("FinalizeTend");
            toil.initAction = delegate
            {
                Pawn actor = toil.actor;
                if (actor.skills != null)
                {
                    actor.skills.Learn(SkillDefOf.Crafting, 250);
                }
                TendUtility.DoTend(actor, patient, null);
                if (toil.actor.CurJob.endAfterTendedOnce)
                {
                    actor.jobs.EndCurrentJob(JobCondition.Succeeded);
                }
            };
            toil.defaultCompleteMode = ToilCompleteMode.Instant;
            return toil;
        }

        public override void Notify_DamageTaken(DamageInfo dinfo)
        {
            base.Notify_DamageTaken(dinfo);
            if (dinfo.Def.ExternalViolenceFor(pawn) && pawn.Faction != Faction.OfPlayer && pawn == Patient)
            {
                pawn.jobs.CheckForJobOverride();
            }
        }

        public static bool CanRepairAndroid(Pawn android)
        {
            if (android.InMentalState)
            {
                return false;
            }
            if (android.IsBurning())
            {
                return false;
            }
            if (android.IsAttacking())
            {
                return false;
            }
            return android.health.HasHediffsNeedingTend() || GetHediffToHeal(android) != null;
        }

        public static Hediff GetHediffToHeal(Pawn android)
        {
            Hediff hediff = null;
            float num = float.PositiveInfinity;
            foreach (Hediff hediff2 in android.health.hediffSet.hediffs)
            {
                if (hediff2 is Hediff_Injury && hediff2.Severity < num)
                {
                    num = hediff2.Severity;
                    hediff = hediff2;
                }
            }
            if (hediff != null)
            {
                return hediff;
            }
            return null;
        }
        public static void RepairTick(Pawn android)
        {
            Hediff hediffToHeal = GetHediffToHeal(android);
            if (hediffToHeal != null)
            {
                hediffToHeal.Heal(1f);
            }
        }
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref ticksToNextRepair, "ticksToNextRepair", 0);
        }
    }
}
