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
        protected Pawn Android => (Pawn)job.GetTarget(TargetIndex.A).Thing;
        protected int TicksPerHeal => 200;
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return pawn.Reserve(Android, job, 1, -1, null, errorOnFailed);
        }
        public override IEnumerable<Toil> MakeNewToils()
        {
            this.FailOnDestroyedOrNull(TargetIndex.A);
            this.FailOnForbidden(TargetIndex.A);
            this.FailOn(() => Android.IsAttacking());
            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
            Toil toil = Toils_General.WaitWith(TargetIndex.A, int.MaxValue,
                useProgressBar: false, maintainPosture: true, maintainSleep: true);
            toil.WithEffect(EffecterDefOf.MechRepairing, TargetIndex.A);
            toil.PlaySustainerOrSound(SoundDefOf.RepairMech_Touch);
            toil.AddPreInitAction(delegate
            {
                ticksToNextRepair = TicksPerHeal;
            });
            toil.handlingFacing = true;
            toil.tickAction = delegate
            {
                ticksToNextRepair--;
                if (ticksToNextRepair <= 0)
                {
                    RepairTick(Android);
                    ticksToNextRepair = TicksPerHeal;
                }
                pawn.rotationTracker.FaceTarget(Android);
                if (pawn.skills != null)
                {
                    pawn.skills.Learn(SkillDefOf.Crafting, 0.05f);
                }
            };
            toil.AddFinishAction(delegate
            {
                if (Android.jobs?.curJob != null)
                {
                    Android.jobs.EndCurrentJob(JobCondition.InterruptForced);
                }
            });
            toil.AddEndCondition(() => CanRepairAndroid(Android) ? JobCondition.Ongoing : JobCondition.Succeeded);
            toil.activeSkill = () => SkillDefOf.Crafting;
            yield return toil;
        }

        public static bool CanRepairAndroid(Pawn android)
        {
            return GetHediffToHeal(android) != null;
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
