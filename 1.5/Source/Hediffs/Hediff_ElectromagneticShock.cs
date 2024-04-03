using RimWorld;
using Verse;

namespace VREAndroids
{
    public class Hediff_ElectromagneticShock : HediffWithComps
    {
        private Effecter empEffecter;
        public override void Tick()
        {
            base.Tick();
            if (empEffecter == null)
            {
                empEffecter = EffecterDefOf.DisabledByEMP.Spawn();
            }
            empEffecter.EffectTick(pawn, pawn);
        }
    }
}
