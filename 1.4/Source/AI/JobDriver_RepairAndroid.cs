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
            return pawn.Reserve(Patient, job, 1, -1, null, errorOnFailed);
        }
        public override IEnumerable<Toil> MakeNewToils()
        {
            this.FailOnDestroyedOrNull(TargetIndex.A);
            this.FailOnForbidden(TargetIndex.A);
            this.FailOn(() => Patient.IsAttacking());
            var gotoToil = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell);

            int ticks = (int)(1f / pawn.GetStatValue(StatDefOf.GeneralLaborSpeed) * 600f);
            Toil tendToil;
            if (!job.draftedTend)
            {
                tendToil = Toils_General.Wait(ticks);
            }
            else
            {
                tendToil = Toils_General.WaitWith(TargetIndex.A, ticks, useProgressBar: false, maintainPosture: true);
                tendToil.AddFinishAction(delegate
                {
                    if (Patient != null && Patient != pawn && Patient.CurJob != null
                    && (Patient.CurJob.def == JobDefOf.Wait || Patient.CurJob.def == JobDefOf.Wait_MaintainPosture))
                    {
                        Patient.jobs.EndCurrentJob(JobCondition.InterruptForced);
                    }
                });
            }
            tendToil.FailOnCannotTouch(TargetIndex.A, PathEndMode.InteractionCell).WithProgressBarToilDelay(TargetIndex.A)
                .PlaySustainerOrSound(VREA_DefOf.Interact_ConstructMetal);
            tendToil.activeSkill = () => SkillDefOf.Crafting;
            tendToil.handlingFacing = true;
            tendToil.WithEffect(VREA_DefOf.ButcherMechanoid, TargetIndex.A);
            tendToil.tickAction = delegate
            {
                if (pawn == Patient && pawn.Faction != Faction.OfPlayer
                && pawn.IsHashIntervalTick(100) && !pawn.Position.Fogged(pawn.Map))
                {
                    FleckMaker.ThrowMetaIcon(pawn.Position, pawn.Map, FleckDefOf.HealingCross);
                }
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
            repairToil.AddEndCondition(() => CanRepairAndroid(Patient) ? JobCondition.Ongoing : JobCondition.Succeeded);
            repairToil.activeSkill = () => SkillDefOf.Crafting;

            yield return gotoToil;
            yield return Toils_Jump.JumpIf(repairToil, () => Patient.health.HasHediffsNeedingTend() is false);
            yield return tendToil;
            yield return FinalizeTend(Patient);
            yield return Toils_Jump.Jump(gotoToil);
            yield return repairToil;
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
