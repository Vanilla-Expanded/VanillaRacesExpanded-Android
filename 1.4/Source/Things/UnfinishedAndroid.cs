using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace VREAndroids
{
    public class UnfinishedAndroid : ThingWithComps
    {
        public float workLeft = -10000f;
        public List<Thing> resources;
        public Building_AndroidCreationStation station;
        public override string GetInspectString()
        {
            string text = base.GetInspectString();
            if (!text.NullOrEmpty())
            {
                text += "\n";
            }
            text += "WorkLeft".Translate() + ": " + workLeft.ToStringWorkAmount();
            return text;
        }
        public override IEnumerable<Gizmo> GetGizmos()
        {
            foreach (var g in base.GetGizmos())
            {
                yield return g;
            }

            yield return new Command_Action
            {
                defaultLabel = "VREA.CancelAndroid".Translate(),
                defaultDesc = "VREA.CancelAndroidDesc".Translate(),
                icon = ContentFinder<Texture2D>.Get("UI/Gizmos/CancelAnAndroid"),
                action = delegate
                {
                    CancelProject();
                }
            };
        }

        public void CancelProject()
        {
            foreach (var resource in resources)
            {
                GenPlace.TryPlaceThing(resource, Position, Map, ThingPlaceMode.Near);
            }
            station.curAndroidProject = null;
            this.Destroy();
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Collections.Look(ref resources, "requiredItems", LookMode.Deep);
            Scribe_References.Look(ref station, "station");
        }
    }
}
