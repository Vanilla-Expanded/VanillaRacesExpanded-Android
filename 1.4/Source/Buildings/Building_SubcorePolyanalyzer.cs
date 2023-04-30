using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace VREAndroids
{
    [HotSwappable]
    [StaticConstructorOnStartup]
    public class Building_SubcorePolyanalyzer : Building_Enterable, IThingHolderWithDrawnPawn, IThingHolder
    {
        private bool initScanner;

        private int fabricationTicksLeft;

        private float scanProgress;

        private Effecter effectStart;

        private bool debugDisableNeedForIngredients;

        private Mote workingMote;

        private Sustainer sustainerWorking;

        public static readonly Texture2D CancelLoadingIcon = ContentFinder<Texture2D>.Get("UI/Designators/Cancel");

        public static readonly CachedTexture InsertPersonIcon = new CachedTexture("UI/Icons/InsertPersonSubcoreScanner");

        private static Dictionary<Rot4, ThingDef> MotePerRotation;

        public List<Pawn> scannedPawns = new List<Pawn>();

        private const float ProgressBarOffsetZ = -0.8f;

        public CachedTexture InitScannerIcon = new CachedTexture("UI/Icons/SubcoreScannerStart");

        public float HeldPawnDrawPos_Y => DrawPos.y + 3f / 74f;

        public float HeldPawnBodyAngle => Rotation.AsAngle;

        public PawnPosture HeldPawnPosture => PawnPosture.LayingOnGroundFaceUp;

        public bool PowerOn => this.TryGetComp<CompPowerTrader>().PowerOn;

        public override Vector3 PawnDrawOffset => Vector3.zero;
        public Pawn Occupant
        {
            get
            {
                for (int i = 0; i < innerContainer.Count; i++)
                {
                    Pawn result;
                    if ((result = innerContainer[i] as Pawn) != null)
                    {
                        return result;
                    }
                }
                return null;
            }
        }

        public bool AllRequiredIngredientsLoaded
        {
            get
            {
                if (!debugDisableNeedForIngredients)
                {
                    for (int i = 0; i < def.building.subcoreScannerFixedIngredients.Count; i++)
                    {
                        if (GetRequiredCountOf(def.building.subcoreScannerFixedIngredients[i].FixedIngredient) > 0)
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
        }

        public SubcoreScannerState State
        {
            get
            {
                if (!initScanner || !PowerOn)
                {
                    return SubcoreScannerState.Inactive;
                }
                if (!AllRequiredIngredientsLoaded)
                {
                    return SubcoreScannerState.WaitingForIngredients;
                }
                if (Occupant == null)
                {
                    return SubcoreScannerState.WaitingForOccupant;
                }
                return SubcoreScannerState.Occupied;
            }
        }

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            for (int i = 0; i < def.building.subcoreScannerFixedIngredients.Count; i++)
            {
                def.building.subcoreScannerFixedIngredients[i].ResolveReferences();
            }
        }

        public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
        {
            effectStart?.Cleanup();
            effectStart = null;
            base.DeSpawn(mode);
        }

        public int GetRequiredCountOf(ThingDef thingDef)
        {
            for (int i = 0; i < def.building.subcoreScannerFixedIngredients.Count; i++)
            {
                if (def.building.subcoreScannerFixedIngredients[i].FixedIngredient == thingDef)
                {
                    int num = innerContainer.TotalStackCountOfDef(def.building.subcoreScannerFixedIngredients[i].FixedIngredient);
                    return (int)def.building.subcoreScannerFixedIngredients[i].GetBaseCount() - num;
                }
            }
            return 0;
        }

        public override AcceptanceReport CanAcceptPawn(Pawn selPawn)
        {
            if (!selPawn.IsColonist && !selPawn.IsSlaveOfColony && !selPawn.IsPrisonerOfColony)
            {
                return false;
            }
            if (selectedPawn != null && selectedPawn != selPawn)
            {
                return false;
            }
            if (!PowerOn)
            {
                return "CannotUseNoPower".Translate();
            }
            if (State != SubcoreScannerState.WaitingForOccupant)
            {
                switch (State)
                {
                    case SubcoreScannerState.Inactive:
                        return "VREA.SubcorePolyanalyzerNotInit".Translate();
                    case SubcoreScannerState.WaitingForIngredients:
                        {
                            StringBuilder stringBuilder = new StringBuilder("SubcoreScannerRequiresIngredients".Translate() + ": ");
                            bool flag = false;
                            for (int i = 0; i < def.building.subcoreScannerFixedIngredients.Count; i++)
                            {
                                IngredientCount ingredientCount = def.building.subcoreScannerFixedIngredients[i];
                                int num = innerContainer.TotalStackCountOfDef(ingredientCount.FixedIngredient);
                                int num2 = (int)ingredientCount.GetBaseCount();
                                if (num < num2)
                                {
                                    if (flag)
                                    {
                                        stringBuilder.Append(", ");
                                    }
                                    stringBuilder.Append($"{ingredientCount.FixedIngredient.LabelCap} x{num2 - num}");
                                    flag = true;
                                }
                            }
                            return stringBuilder.ToString();
                        }
                    case SubcoreScannerState.Occupied:
                        return "SubcoreScannerOccupied".Translate();
                }
            }
            else
            {
                if (scannedPawns.Contains(selPawn))
                {
                    return "VREA.AlreadyScanned".Translate(selPawn.Named("PAWN"));
                }
                if (selPawn.HasActiveGene(VREA_DefOf.VREA_SyntheticBody))
                {
                    return "VREA.AndroidAreNotAllowed".Translate();
                }
                if (selPawn.IsQuestLodger())
                {
                    return "CryptosleepCasketGuestsNotAllowed".Translate();
                }
                if (selPawn.health.hediffSet.HasHediff(HediffDefOf.ScanningSickness))
                {
                    return "SubcoreScannerPawnHasSickness".Translate(HediffDefOf.ScanningSickness.label);
                }
                if (selPawn.DevelopmentalStage.Baby())
                {
                    return "SubcoreScannerBabyNotAllowed".Translate();
                }
            }
            return true;
        }

        public override void TryAcceptPawn(Pawn pawn)
        {
            if ((bool)CanAcceptPawn(pawn))
            {
                bool num = pawn.DeSpawnOrDeselect();
                if (pawn.holdingOwner != null)
                {
                    pawn.holdingOwner.TryTransferToContainer(pawn, innerContainer);
                }
                else
                {
                    innerContainer.TryAdd(pawn);
                }
                if (num)
                {
                    Find.Selector.Select(pawn, playSound: false, forceDesignatorDeselect: false);
                }
                fabricationTicksLeft = def.building.subcoreScannerTicks;
            }
        }

        public bool CanAcceptIngredient(Thing thing)
        {
            return GetRequiredCountOf(thing.def) > 0;
        }

        public void EjectContents()
        {
            Pawn occupant = Occupant;
            if (occupant == null)
            {
                innerContainer.TryDropAll(InteractionCell, base.Map, ThingPlaceMode.Near);
            }
            else
            {
                if (def.building.subcoreScannerHediff != null)
                {
                    occupant.health?.AddHediff(def.building.subcoreScannerHediff);
                }
                for (int num = innerContainer.Count - 1; num >= 0; num--)
                {
                    if (innerContainer[num] is Pawn || innerContainer[num] is Corpse)
                    {
                        innerContainer.TryDrop(innerContainer[num], InteractionCell, base.Map, ThingPlaceMode.Near, 1, out var _);
                    }
                }
            }
            selectedPawn = null;
        }

        public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn selPawn)
        {
            foreach (FloatMenuOption floatMenuOption in base.GetFloatMenuOptions(selPawn))
            {
                yield return floatMenuOption;
            }
            if (!selPawn.CanReach(this, PathEndMode.InteractionCell, Danger.Deadly))
            {
                yield return new FloatMenuOption("CannotEnterBuilding".Translate(this) + ": " + "NoPath".Translate().CapitalizeFirst(), null);
                yield break;
            }
            AcceptanceReport acceptanceReport = CanAcceptPawn(selPawn);
            if (acceptanceReport.Accepted)
            {
                yield return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("EnterBuilding".Translate(this), delegate
                {
                    SelectPawn(selPawn);
                }), selPawn, this);
            }
            else if (!acceptanceReport.Reason.NullOrEmpty())
            {
                yield return new FloatMenuOption("CannotEnterBuilding".Translate(this) + ": " + acceptanceReport.Reason.CapitalizeFirst(), null);
            }
        }

        public static bool WasLoadingCancelled(Thing thing)
        {
            Building_SubcorePolyanalyzer building_SubcoreScanner;
            if ((building_SubcoreScanner = thing as Building_SubcorePolyanalyzer) != null && !building_SubcoreScanner.initScanner)
            {
                return true;
            }
            return false;
        }
        private static readonly Material UnfilledMat = SolidColorMaterials.NewSolidColorMaterial(new Color(0.3f, 0.3f, 0.3f, 0.65f), ShaderDatabase.MetaOverlay);

        private static readonly Material FilledMat = SolidColorMaterials.NewSolidColorMaterial(new Color(0.9f, 0.85f, 0.2f, 0.65f), ShaderDatabase.MetaOverlay);

        public static float vertSize1 = 0.55f;
        public static float vertSize2 = 0.125f;
        public static float horSize1 = 0.578f;
        public static float horSize2 = 0.125f;
        public static float northLeftBarOffsetX = -0.778f;
        public static float northRightBarOffsetX = 0.781f;
        public static float southLeftBarOffsetX = -0.778f;
        public static float southRightBarOffsetX = 0.771f;
        public static float westLeftBarOffsetX = -0.643f;
        public static float westRightBarOffsetX = 0.7f;
        public static float eastLeftBarOffsetX = -0.7f;
        public static float eastRightBarOffsetX = 0.643f;
        public static float vertBarOffsetZ = -0.26f;
        public static float horBarOffsetZ = -0.23f;
        public override void Draw()
        {
            base.Draw();
            Occupant?.Drawer.renderer.RenderPawnAt(DrawPos, null, neverAimWeapon: true);
            if (scanProgress > 0)
            {
                if (Rotation.IsHorizontal)
                {
                    if (Rotation == Rot4.West)
                    {
                        DrawProgressBar(westLeftBarOffsetX);
                        DrawProgressBar(westRightBarOffsetX);
                    }
                    else
                    {
                        DrawProgressBar(eastLeftBarOffsetX);
                        DrawProgressBar(eastRightBarOffsetX);
                    }
                }
                else
                {
                    if (Rotation == Rot4.South)
                    {
                        DrawProgressBar(southLeftBarOffsetX);
                        DrawProgressBar(southRightBarOffsetX);
                    }
                    else
                    {
                        DrawProgressBar(northLeftBarOffsetX);
                        DrawProgressBar(northRightBarOffsetX);
                    }
                }
            }
        }

        private void DrawProgressBar(float xOffset)
        {
            Rot4 rotation = base.Rotation;
            GenDraw.FillableBarRequest r = default(GenDraw.FillableBarRequest);
            r.center = DrawPos + new Vector3(xOffset, 0, rotation.IsHorizontal ? horBarOffsetZ : vertBarOffsetZ).RotatedBy(Rotation);
            r.size = new Vector2(rotation.IsHorizontal ? horSize1: vertSize1, rotation.IsHorizontal ? horSize2 : vertSize2);
            r.fillPercent = scanProgress;
            r.filledMat = FilledMat;
            r.unfilledMat = UnfilledMat;
            r.margin = 0f;
            rotation.Rotate(RotationDirection.Clockwise);
            r.rotation = rotation;
            if (base.Rotation == Rot4.South)
            {
                r.center.z -= 0.08f;
            }
            var s = new Vector3(r.size.x * r.fillPercent, 1f, r.size.y);
            var matrix = default(Matrix4x4);
            Vector3 pos = r.center + Vector3.up * 0.01f;
            if (!r.rotation.IsHorizontal)
            {
                pos.x -= r.size.x * 0.5f;
                pos.x += 0.5f * r.size.x * r.fillPercent;
            }
            else
            {
                pos.z -= r.size.x * 0.5f;
                pos.z += 0.5f * r.size.x * r.fillPercent;
            }
            matrix.SetTRS(pos, r.rotation.AsQuat, s);
            Graphics.DrawMesh(MeshPool.plane10, matrix, r.filledMat, 0);
        }

        public override void Tick()
        {
            base.Tick();
            if (MotePerRotation == null)
            {
                MotePerRotation = new Dictionary<Rot4, ThingDef>
                {
                    {
                        Rot4.South,
                        VREA_DefOf.VREA_SubcorePolyanalyzer_South
                    },
                    {
                        Rot4.East,
                        VREA_DefOf.VREA_SubcorePolyanalyzer_East
                    },
                    {
                        Rot4.West,
                        VREA_DefOf.VREA_SubcorePolyanalyzer_West
                    },
                    {
                        Rot4.North,
                        VREA_DefOf.VREA_SubcorePolyanalyzer_North
                    }
                };
            }
            SubcoreScannerState state = State;
            if (state == SubcoreScannerState.Occupied)
            {
                fabricationTicksLeft--;
                scanProgress = ((1f - (fabricationTicksLeft / (float)def.building.subcoreScannerTicks)) / 4f) + (0.25f * scannedPawns.Count);
                if (fabricationTicksLeft <= 0)
                {
                    Messages.Message("VREA.MessageSubcorePolyanalyzerCompleted".Translate(Occupant.Named("PAWN"),
                        scanProgress.ToStringPercent()), Occupant, MessageTypeDefOf.PositiveEvent);
                    scannedPawns.Add(Occupant);
                    EjectContents();
                    if (scanProgress >= 1)
                    {
                        GenPlace.TryPlaceThing(ThingMaker.MakeThing(def.building.subcoreScannerOutputDef), InteractionCell, base.Map, ThingPlaceMode.Near);
                        if (def.building.subcoreScannerComplete != null)
                        {
                            def.building.subcoreScannerComplete.PlayOneShot(this);
                        }
                        Reset();
                        innerContainer.ClearAndDestroyContents();
                    }
                }

                if (workingMote == null || workingMote.Destroyed)
                {
                    workingMote = MoteMaker.MakeAttachedOverlay(this, MotePerRotation[base.Rotation], Vector3.zero);
                }
                workingMote.Maintain();


                if (def.building.subcoreScannerWorking != null)
                {
                    if (sustainerWorking == null || sustainerWorking.Ended)
                    {
                        sustainerWorking = def.building.subcoreScannerWorking.TrySpawnSustainer(SoundInfo.InMap(this, MaintenanceType.PerTick));
                    }
                    else
                    {
                        sustainerWorking.Maintain();
                    }
                }
            }

            if (state == SubcoreScannerState.Occupied)
            {
                if (def.building.subcoreScannerStartEffect != null)
                {
                    if (effectStart == null)
                    {
                        effectStart = def.building.subcoreScannerStartEffect.Spawn();
                        effectStart.Trigger(this, new TargetInfo(InteractionCell, base.Map));
                    }
                    effectStart.EffectTick(this, new TargetInfo(InteractionCell, base.Map));
                }
            }
            else
            {
                effectStart?.Cleanup();
                effectStart = null;
            }
        }

        private void Reset()
        {
            initScanner = false;
            scanProgress = 0f;
            scannedPawns.Clear();
        }

        public override IEnumerable<Gizmo> GetGizmos()
        {
            foreach (Gizmo gizmo in base.GetGizmos())
            {
                yield return gizmo;
            }
            if (!initScanner)
            {
                Command_Action command_Action = new Command_Action();
                command_Action.defaultLabel = "SubcoreScannerStart".Translate();
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("VREA.SubcorePolyanalyzerStart".Translate() + " " + def.building.subcoreScannerOutputDef.label + ".");
                stringBuilder.Append("\n\n");
                stringBuilder.Append("DurationHours".Translate() + ": " + def.building.subcoreScannerTicks.ToStringTicksToPeriod());
                stringBuilder.Append("\n\n");
                string text = def.building.subcoreScannerFixedIngredients.Select((IngredientCount i) => i.Summary).ToCommaList(useAnd: true);
                stringBuilder.Append("VREA.SubcorePolyanalyzerStartDesc".Translate(def.label, text));
                command_Action.defaultDesc = stringBuilder.ToString();
                command_Action.icon = InitScannerIcon.Texture;
                command_Action.action = delegate
                {
                    initScanner = true;
                };
                command_Action.activateSound = SoundDefOf.Tick_Tiny;
                yield return command_Action;
            }
            else if (base.SelectedPawn == null)
            {
                Command_Action command_Action2 = new Command_Action();
                command_Action2.defaultLabel = "InsertPerson".Translate() + "...";
                command_Action2.defaultDesc = "InsertPersonSubcoreScannerDesc".Translate(def.label);
                command_Action2.icon = InsertPersonIcon.Texture;
                command_Action2.action = delegate
                {
                    List<FloatMenuOption> list = new List<FloatMenuOption>();
                    List<Pawn> allPawnsSpawned = base.Map.mapPawns.AllPawnsSpawned;
                    for (int j = 0; j < allPawnsSpawned.Count; j++)
                    {
                        Pawn pawn = allPawnsSpawned[j];
                        AcceptanceReport acceptanceReport = CanAcceptPawn(pawn);
                        if (!acceptanceReport.Accepted)
                        {
                            if (!acceptanceReport.Reason.NullOrEmpty())
                            {
                                list.Add(new FloatMenuOption(pawn.LabelShortCap + ": " + acceptanceReport.Reason, null, pawn, Color.white));
                            }
                        }
                        else
                        {
                            list.Add(new FloatMenuOption(pawn.LabelShortCap, delegate
                            {
                                SelectPawn(pawn);
                            }, pawn, Color.white));
                        }
                    }
                    if (!list.Any())
                    {
                        list.Add(new FloatMenuOption("NoExtractablePawns".Translate(), null));
                    }
                    Find.WindowStack.Add(new FloatMenu(list));
                };
                if (!PowerOn)
                {
                    command_Action2.Disable("NoPower".Translate().CapitalizeFirst());
                }
                else if (State == SubcoreScannerState.WaitingForIngredients)
                {
                    StringBuilder stringBuilder2 = new StringBuilder("SubcoreScannerWaitingForIngredientsDesc".Translate().CapitalizeFirst() + ":\n");
                    AppendIngredientsList(stringBuilder2);
                    command_Action2.Disable(stringBuilder2.ToString());
                }
                yield return command_Action2;
            }
            if (initScanner)
            {
                Command_Action command_Action3 = new Command_Action();
                command_Action3.defaultLabel = "CommandCancelSubcoreScan".Translate();
                command_Action3.defaultDesc = "CommandCancelSubcoreScanDesc".Translate();
                command_Action3.icon = CancelLoadingIcon;
                command_Action3.action = delegate
                {
                    EjectContents();
                    innerContainer.TryDropAll(Position, Map, ThingPlaceMode.Near);
                    Reset();
                };
                command_Action3.activateSound = SoundDefOf.Designate_Cancel;
                yield return command_Action3;
            }
            if (!DebugSettings.ShowDevGizmos)
            {
                yield break;
            }
            if (State == SubcoreScannerState.Occupied)
            {
                Command_Action command_Action4 = new Command_Action();
                command_Action4.defaultLabel = "DEV: Complete";
                command_Action4.action = delegate
                {
                    fabricationTicksLeft = 0;
                };
                yield return command_Action4;
            }
            Command_Action command_Action5 = new Command_Action();
            command_Action5.defaultLabel = (debugDisableNeedForIngredients ? "DEV: Enable Ingredients" : "DEV: Disable Ingredients");
            command_Action5.action = delegate
            {
                debugDisableNeedForIngredients = !debugDisableNeedForIngredients;
            };
            yield return command_Action5;
        }

        public override string GetInspectString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(base.GetInspectString());
            switch (State)
            {
                case SubcoreScannerState.WaitingForIngredients:
                    stringBuilder.AppendLineIfNotEmpty();
                    stringBuilder.Append("SubcoreScannerWaitingForIngredients".Translate());
                    AppendIngredientsList(stringBuilder);
                    break;
                case SubcoreScannerState.WaitingForOccupant:
                    stringBuilder.AppendLineIfNotEmpty();
                    stringBuilder.Append("SubcoreScannerWaitingForOccupant".Translate());
                    break;
                case SubcoreScannerState.Occupied:
                    stringBuilder.AppendLineIfNotEmpty();
                    stringBuilder.Append("SubcoreScannerCompletesIn".Translate() + ": " + fabricationTicksLeft.ToStringTicksToPeriod());
                    break;
            }
            if (State != SubcoreScannerState.Inactive)
            {
                stringBuilder.AppendLineIfNotEmpty();
                stringBuilder.AppendLine("VREA.ScanProgress".Translate(scanProgress.ToStringPercent()));
            }
            return stringBuilder.ToString().TrimEndNewlines();
        }

        private void AppendIngredientsList(StringBuilder sb)
        {
            for (int i = 0; i < def.building.subcoreScannerFixedIngredients.Count; i++)
            {
                IngredientCount ingredientCount = def.building.subcoreScannerFixedIngredients[i];
                int num = innerContainer.TotalStackCountOfDef(ingredientCount.FixedIngredient);
                int num2 = (int)ingredientCount.GetBaseCount();
                sb.AppendInNewLine($" - {ingredientCount.FixedIngredient.LabelCap} {num} / {num2}");
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref initScanner, "initModification", defaultValue: false);
            Scribe_Values.Look(ref fabricationTicksLeft, "workTicksLeft", 0);
            Scribe_Values.Look(ref scanProgress, "modificationProgress", 0);
            Scribe_Collections.Look(ref scannedPawns, "scannedPawns", LookMode.Reference);
        }
    }
}
