using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(PawnGenerator), "GenerateGenes")]
    public static class PawnGenerator_GenerateGenes_Patch
    {
        public static Pawn curPawn;
        public static void Prefix(Pawn pawn)
        {
            curPawn = pawn;
        }

        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> codeInstructions)
        {
            var codes = codeInstructions.ToList();
            var enumerator = AccessTools.Method(typeof(List<GeneDef>), "GetEnumerator");
            var orderGenesInfo = AccessTools.Method(typeof(PawnGenerator_GenerateGenes_Patch), "OrderGenesIfNeeded");
            for (var i = 0; i < codes.Count; i++)
            {
                if (codes[i].Calls(enumerator))
                {
                    yield return new CodeInstruction(OpCodes.Call, orderGenesInfo);
                }
                yield return codes[i];
            }
        }

        public static List<GeneDef> OrderGenesIfNeeded(List<GeneDef> genes)
        {
            if (genes.Any(x => x.IsAndroidGene()))
            {
                var orderedGenes = genes.OrderByDescending(x => x.CanBeRemovedFromAndroid() is false).ToList();
                return orderedGenes;
            }
            return genes;
        }

        public static void Postfix(Pawn pawn)
        {
            curPawn = null;
            if (pawn.IsAndroid())
            {
                pawn.needs.AddOrRemoveNeedsAsAppropriate();
            }
        }
    }
}
