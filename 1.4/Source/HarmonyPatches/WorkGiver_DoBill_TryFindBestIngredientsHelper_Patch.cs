using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(WorkGiver_DoBill), "TryFindBestIngredientsHelper")]
    public static class WorkGiver_DoBill_TryFindBestIngredientsHelper_Patch
    {
        public static void Prefix(Predicate<Thing> thingValidator, Predicate<List<Thing>> foundAllIngredientsAndChoose,
            ref List<IngredientCount> ingredients, Pawn pawn, Thing billGiver, List<ThingCount> chosen, float searchRadius)
        {
            if (billGiver is Pawn pawn2 && pawn2.HasActiveGene(VREA_DefOf.VREA_SyntheticBody))
            {
                ingredients = ingredients.Where(x => x.filter.AllowedThingDefs.Any(x => x.IsMedicine) is false).ToList();
                if (pawn2.playerSettings is not null)
                {
                    pawn2.playerSettings.medCare = MedicalCareCategory.NoMeds;
                }
            }
        }
    }
}
