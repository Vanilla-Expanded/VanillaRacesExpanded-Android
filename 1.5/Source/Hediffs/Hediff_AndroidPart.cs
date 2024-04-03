using Verse;

namespace VREAndroids
{
    [HotSwappable]
    public class Hediff_AndroidPart : Hediff_AddedPart
    {
        public override void PostAdd(DamageInfo? dinfo)
        {
            if (comps != null)
            {
                for (int i = 0; i < comps.Count; i++)
                {
                    comps[i].CompPostPostAdd(dinfo);
                }
            }
        }

        public override bool Visible => false;
    }
}
