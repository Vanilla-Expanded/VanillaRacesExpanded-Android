using System.Collections.Generic;
using Verse;

namespace VREAndroids
{
    public class AndroidSettings : Def
    {
        public List<string> excludedNeedsForAndroids;

        public List<string> androidExclusiveNeeds;

        public List<string> androidSpecificMentalBreaks;

        public List<string> androidsShouldNotReceiveHediffs;
    }
}
