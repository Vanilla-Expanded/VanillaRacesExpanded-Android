using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace VREAndroids
{
    public class Building_AndroidStand : Building_Bed
    {
        public static HashSet<Building_AndroidStand> stands = new HashSet<Building_AndroidStand>();

        public CompPowerTrader compPower;
        public Pawn CurOccupant
        {
            get
            {
                List<Thing> list = Map.thingGrid.ThingsListAt(this.Position);
                for (int i = 0; i < list.Count; i++)
                {
                    Pawn pawn = list[i] as Pawn;
                    if (pawn != null && pawn.IsAndroid())
                    {
                        return pawn;
                    }
                }
                return null;
            }
        }
        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            stands.Add(this);
            compPower = this.TryGetComp<CompPowerTrader>();
        }

        public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
        {
            base.DeSpawn(mode);
            stands.Remove(this);
        }

        public override void Tick()
        {
            base.Tick();
            var occupant = CurOccupant;
            if (occupant != null)
            {
                occupant.Rotation = Rot4.South;
                if (occupant.jobs.curDriver is JobDriver_LayDown)
                {
                    occupant.jobs.curDriver.rotateToFace = TargetIndex.C;
                }
            }
        }
        public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn selPawn)
        {
            foreach (var opt in base.GetFloatMenuOptions(selPawn))
            {
                yield return opt;
            }
            if (this.Faction == Faction.OfPlayer && selPawn.HasActiveGene(VREA_DefOf.VREA_MemoryProcessing))
            {
                var cannotUseReason = CannotUseNowReason(selPawn);
                if (cannotUseReason.NullOrEmpty())
                {
                    yield return new FloatMenuOption("VREA.FreeMemorySpace".Translate(), delegate
                    {
                        if (CompAssignableToPawn.AssignedPawns.Contains(selPawn) is false)
                        {
                            CompAssignableToPawn.TryAssignPawn(selPawn);
                        }
                        selPawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(VREA_DefOf.VREA_FreeMemorySpace, this));
                    });
                }
                else
                {
                    yield return new FloatMenuOption("VREA.FreeMemorySpace".Translate() + ": " + cannotUseReason, null);
                }
            }
        }

        public string CannotUseNowReason(Pawn selPawn)
        {
            if (compPower != null && !compPower.PowerOn)
            {
                return "NoPower".Translate().CapitalizeFirst();
            }
            if (!selPawn.CanReach(this, PathEndMode.OnCell, Danger.Deadly))
            {
                return "NoPath".Translate().CapitalizeFirst();
            }
            if (!selPawn.CanReserve(this))
            {
                Pawn pawn = selPawn.Map.reservationManager.FirstRespectedReserver(this, selPawn);
                if (pawn == null)
                {
                    pawn = selPawn.Map.physicalInteractionReservationManager.FirstReserverOf(selPawn);
                }
                if (pawn != null)
                {
                    return "ReservedBy".Translate(pawn.LabelShort, pawn);
                }
                else
                {
                    return "Reserved".Translate();
                }
            }
            if (CurOccupant != null)
            {
                return "VREA.AndroidStandIsOccupied".Translate();
            }
            return null;
        }
    }
}
