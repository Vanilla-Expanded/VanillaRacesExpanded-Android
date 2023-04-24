using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace VREAndroids
{
    [StaticConstructorOnStartup]
    public static class Utils
    {
        public static bool IsAndroid(this Pawn pawn)
        {
            return pawn.IsAndroid(out _);
        }

        public static bool IsAndroid(this Pawn pawn, out AndroidState androidState)
        {
            var hediff = GetAndroidHediff(pawn);
            bool isAndroid = hediff != null;
            if (isAndroid)
            {
                androidState = hediff.AndroidState;
            }
            else
            {
                androidState = AndroidState.None;
            }
            return isAndroid;
        }

        public static Hediff_Android GetAndroidHediff(this Pawn pawn)
        {
            return pawn.health.hediffSet.hediffs.OfType<Hediff_Android>().FirstOrDefault();
        }

        public static Dictionary<BodyPartDef, HediffDef> cachedCounterParts = new Dictionary<BodyPartDef, HediffDef>();
        public static HediffDef GetAndroidCounterPart(this BodyPartDef bodyPart)
        {
            if (!cachedCounterParts.TryGetValue(bodyPart, out HediffDef hediffDef))
            {
                cachedCounterParts[bodyPart] = hediffDef = GetAndroidCounterPartInt(bodyPart);
            }
            return hediffDef;
        }
        private static HediffDef GetAndroidCounterPartInt(BodyPartDef bodyPart)
        {
            foreach (var recipe in DefDatabase<RecipeDef>.AllDefs)
            {
                if (recipe.addsHediff != null && recipe.appliedOnFixedBodyParts != null && recipe.appliedOnFixedBodyParts.Contains(bodyPart)
                    && typeof(Hediff_AndroidPart).IsAssignableFrom(recipe.addsHediff.hediffClass))
                {
                    return recipe.addsHediff;
                }
            }
            return null;
        }

        public static bool AndroidCanCatch(HediffDef hediffDef)
        {
            var extension = hediffDef.GetModExtension<AndroidSettingsExtension>();
            if (extension != null && extension.androidCanCatchIt)
            {
                return true;
            }
            return VREA_DefOf.VREA_AndroidSettings.androidsShouldNotReceiveHediffs.Contains(hediffDef.defName) is false
                && (typeof(Hediff_Addiction).IsAssignableFrom(hediffDef.hediffClass)
                || typeof(Hediff_Psylink).IsAssignableFrom(hediffDef.hediffClass)
                || typeof(Hediff_High).IsAssignableFrom(hediffDef.hediffClass)
                || typeof(Hediff_Hangover).IsAssignableFrom(hediffDef.hediffClass)
                || hediffDef.chronic || hediffDef.CompProps<HediffCompProperties_Immunizable>() != null
                || hediffDef.makesSickThought) is false;
        }
    }
}
