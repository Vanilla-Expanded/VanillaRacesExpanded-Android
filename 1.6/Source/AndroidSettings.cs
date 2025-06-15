using System.Collections.Generic;
using Verse;

namespace VREAndroids
{
    public class AndroidSettings : Def
    {
        public List<string> excludedNeedsForAndroids;

        public List<string> androidExclusiveNeeds;

        public List<string> androidsShouldNotReceiveHediffs;

        public List<string> androidSpecificMentalBreaks;

        public List<string> allowedTraits;

        public List<string> disallowedTraits;

        public List<string> disallowedRecipes;
    }
}
