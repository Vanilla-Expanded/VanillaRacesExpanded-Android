using RimWorld;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Verse;

namespace VREAndroids
{
    [HotSwappable]
    public class Window_AndroidCreation : Window_CreateAndroidBase
    {
        public Building_AndroidCreationStation station;

        public Pawn creator;
        public Window_AndroidCreation(Building_AndroidCreationStation station, Pawn creator, Action callback) : base(callback)
        {
            this.station = station;
            this.creator = creator;
        }

        public override string Header => "VREA.CreateAndroid".Translate();
        public override string AcceptButtonLabel => "VREA.CreateAndroid".Translate();
        protected override void AcceptInner()
        {
            CustomXenotype customXenotype = new CustomXenotype();
            customXenotype.name = xenotypeName?.Trim();
            customXenotype.genes.AddRange(selectedGenes);
            customXenotype.inheritable = false;
            customXenotype.iconDef = iconDef;
            station.curAndroidProject = customXenotype;
            station.totalWorkAmount = selectedGenes.Sum(x => x.biostatCpx * 2000);
            station.currentWorkAmountDone = 0;
            station.requiredItems = requiredItems;
            if (creator != null)
            {
                var workgiver = new WorkGiver_CreateAndroid();
                var job = workgiver.JobOnThing(creator, station);
                if (job != null)
                {
                    creator.jobs.TryTakeOrderedJob(job);
                }
            }
        }

        private static string AndroidProjectsFolderPath => GenFilePaths.FolderUnderSaveData("AndroidProjects");
        public static string AbsFilePathForAndroidProject(string androidProjectName)
        {
            return Path.Combine(AndroidProjectsFolderPath, androidProjectName + ".xtp");
        }

        protected override TaggedString AndroidName()
        {
            return "VREA.AndroidtypeName".Translate();
        }
        public override void DrawSearchRect(Rect rect)
        {
            base.DrawSearchRect(rect);
            if (Widgets.ButtonText(new Rect(rect.xMax - ButSize.x, rect.y, ButSize.x, ButSize.y), "VREA.SaveAndroidtype".Translate()))
            {
                CustomXenotype customXenotype = new CustomXenotype();
                customXenotype.name = xenotypeName?.Trim();
                customXenotype.genes.AddRange(selectedGenes);
                customXenotype.inheritable = false;
                customXenotype.iconDef = iconDef;
                Find.WindowStack.Add(new Dialog_AndroidProjectList_Save(customXenotype));
            }
            if (Widgets.ButtonText(new Rect(rect.xMax - ButSize.x * 2f - 4f, rect.y, ButSize.x, ButSize.y), "VREA.LoadAndroidtype".Translate()))
            {
                Find.WindowStack.Add(new Dialog_AndroidProjectList_Load(delegate (CustomXenotype xenotype)
                {
                    xenotypeName = xenotype.name;
                    xenotypeNameLocked = true;
                    selectedGenes.Clear();
                    selectedGenes = Utils.AndroidGenesGenesInOrder.Where(x => x.CanBeRemovedFromAndroid() is false).ToList();
                    selectedGenes.AddRange(xenotype.genes);
                    selectedGenes = selectedGenes.Distinct().ToList();
                    iconDef = xenotype.IconDef;
                    OnGenesChanged();
                }));
            }
        }

        public override void OnGenesChanged()
        {
            base.OnGenesChanged();
            requiredItems = new List<ThingDefCount>
            {
                new ThingDefCount(VREA_DefOf.VREA_PersonaSubcore, 1),
                new ThingDefCount(ThingDefOf.Plasteel, 125),
                new ThingDefCount(ThingDefOf.Uranium, 30),
                new ThingDefCount(ThingDefOf.ComponentSpacer, 7),
            };
        }

        public static IEnumerable<FileInfo> AllAndroidProjectFiles
        {
            get
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(AndroidProjectsFolderPath);
                if (!directoryInfo.Exists)
                {
                    directoryInfo.Create();
                }
                return from f in directoryInfo.GetFiles()
                       where f.Extension == ".xtp"
                       orderby f.LastWriteTime descending
                       select f;
            }
        }

        public static bool TryLoadAndroidProject(string absPath, out CustomXenotype project)
        {
            project = null;
            try
            {
                Scribe.loader.InitLoading(absPath);
                try
                {
                    ScribeMetaHeaderUtility.LoadGameDataHeader(ScribeMetaHeaderUtility.ScribeHeaderMode.Xenotype, logVersionConflictWarning: true);
                    Scribe_Deep.Look(ref project, "project");
                    Scribe.loader.FinalizeLoading();
                }
                catch
                {
                    Scribe.ForceStop();
                    throw;
                }
                project.fileName = Path.GetFileNameWithoutExtension(new FileInfo(absPath).Name);
            }
            catch (Exception ex)
            {
                Log.Error("Exception loading project: " + ex.ToString());
                project = null;
                Scribe.ForceStop();
            }
            return project != null;
        }

        public static void SaveAndroidProject(CustomXenotype project, string absFilePath)
        {
            try
            {
                project.fileName = Path.GetFileNameWithoutExtension(absFilePath);
                SafeSaver.Save(absFilePath, "savedProject", delegate
                {
                    ScribeMetaHeaderUtility.WriteMetaHeader();
                    Scribe_Deep.Look(ref project, "project");
                });
            }
            catch (Exception ex)
            {
                Log.Error("Exception while saving project: " + ex.ToString());
            }
        }
    }
}
