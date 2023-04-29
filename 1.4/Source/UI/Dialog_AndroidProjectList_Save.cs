using RimWorld;
using Verse;

namespace VREAndroids
{
    public class Dialog_AndroidProjectList_Save : Dialog_AndroidProjectList
    {
        private CustomXenotype project;
        public override bool ShouldDoTypeInField => true;
        public Dialog_AndroidProjectList_Save(CustomXenotype project)
        {
            interactButLabel = "OverwriteButton".Translate();
            this.project = project;
            typingName = project.name;
        }

        public override void DoFileInteraction(string fileName)
        {
            fileName = GenFile.SanitizedFileName(fileName);
            string absPath = Window_AndroidCreation.AbsFilePathForAndroidProject(fileName);
            LongEventHandler.QueueLongEvent(delegate
            {
                Window_AndroidCreation.SaveAndroidProject(project, absPath);
            }, "SavingLongEvent", doAsynchronously: false, null);
            Messages.Message("SavedAs".Translate(fileName), MessageTypeDefOf.SilentInput, historical: false);
            Close();
        }
    }
}
