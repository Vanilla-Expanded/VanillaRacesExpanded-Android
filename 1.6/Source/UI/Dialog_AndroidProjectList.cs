using RimWorld;
using System;
using System.IO;
using Verse;

namespace VREAndroids
{
    public abstract class Dialog_AndroidProjectList : Dialog_FileList
    {
        public override void ReloadFiles()
        {
            files.Clear();
            foreach (FileInfo allCustomXenotypeFile in AndroidProjectUtils.AllAndroidProjectFiles)
            {
                try
                {
                    SaveFileInfo saveFileInfo = new SaveFileInfo(allCustomXenotypeFile);
                    saveFileInfo.LoadData();
                    files.Add(saveFileInfo);
                }
                catch (Exception ex)
                {
                    Log.Error("Exception loading " + allCustomXenotypeFile.Name + ": " + ex.ToString());
                }
            }
            CharacterCardUtility.cachedCustomXenotypes = null;
        }
    }
}
