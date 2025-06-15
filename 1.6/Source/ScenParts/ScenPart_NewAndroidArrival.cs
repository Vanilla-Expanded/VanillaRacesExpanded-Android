
using VEF.AnimalBehaviours;
using RimWorld;
using RimWorld.QuestGen;
using Verse;
namespace VREAndroids
{

    public class ScenPart_NewAndroidArrival : ScenPart
    {

        int tickCounter = 0;
        const int tickInterval = 600000; // 10 days

        public override void Tick()
        {
            tickCounter++;
            if (tickCounter > tickInterval)
            {

                Slate slate = new Slate();
                Quest quest = QuestUtility.GenerateQuestAndMakeAvailable(VREA_DefOf.VREA_Quest_AndroidJoins, slate);

                QuestUtility.SendLetterQuestAvailable(quest);


                tickCounter = 0;
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref tickCounter, "tickCounter", defaultValue: 0);
        }


    }
}
