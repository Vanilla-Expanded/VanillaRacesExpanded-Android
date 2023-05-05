using RimWorld;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Verse;

namespace VREAndroids
{
    public static class AndroidProjectUtils
    {
        private static string AndroidProjectsFolderPath => GenFilePaths.FolderUnderSaveData("AndroidProjects");
        public static string AbsFilePathForAndroidProject(string androidProjectName)
        {
            return Path.Combine(AndroidProjectsFolderPath, androidProjectName + ".xtp");
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
