using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch]
    public static class VanillaRacesExpandedSanguophage_JobGiver_Hemohunter_FindPawnToSuck_Patch
    {
        public static MethodBase targetMethod;
        public static bool Prepare()
        {
            if (ModLister.AnyModActiveNoSuffix(["vanillaracesexpanded.sanguophage"]))
            {
                targetMethod = AccessTools.Method("VanillaRacesExpandedSanguophage.JobGiver_Hemohunter:FindPawnToSuck");
                if (targetMethod != null)
                {
                    return true;
                }
                Log.Error("[VREAndroids] Failed to patch VRE Sanguophage mod for FindPawnToSuck");
            }
            return false;
        }
        public static MethodBase TargetMethod()
        {
            return targetMethod;
        }

        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var codes = instructions.ToList();
            var AllPawnsSpawnedInfo = AccessTools.PropertyGetter(typeof(MapPawns), nameof(MapPawns.AllPawnsSpawned));
            bool patched = false;
            foreach (var code in codes)
            {
                yield return code;
                if (code.Calls(AllPawnsSpawnedInfo))
                {
                    patched = true;
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Xenogerm_GetGizmos_Patch), "NoAndroids"));
                }
            }
            if (!patched)
            {
                Log.Error("VREAndroids failed to patch VanillaRacesExpandedSanguophage.JobGiver_Hemohunter:FindPawnToSuck");
            }
        }

        public static List<Pawn> NoAndroids(List<Pawn> pawns)
        {
            return pawns.Where(x => x.IsAndroid() is false).ToList();
        }
    }
}
