using RimWorld;
using Verse;

namespace VREAndroids
{
    public class Need_FoodSuppressed : Need_Food
    {
        public Need_FoodSuppressed(Pawn pawn)
            : base(pawn)
        {
        }
        public override void NeedInterval()
        {
            CurLevel = MaxLevel;
        }
    }
}
