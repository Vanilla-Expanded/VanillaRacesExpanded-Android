using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(CharacterCardUtility), "CustomXenotypes", MethodType.Getter)]
    public static class CharacterCardUtility_CustomXenotypes_Patch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> codeInstructions)
        {
            foreach (var instruction in codeInstructions)
            {
                if (instruction.opcode == OpCodes.Leave_S)
                {
                    yield return new CodeInstruction(OpCodes.Call,
                        AccessTools.Method(typeof(CharacterCardUtility_CustomXenotypes_Patch), "FillWithAndroidCustomTypes"));
                }
                yield return instruction;
            }
        }

        private static void FillWithAndroidCustomTypes()
        {
            foreach (FileInfo item in AndroidProjectUtils.AllAndroidProjectFiles.OrderBy((FileInfo f) => f.LastWriteTime))
            {
                string filePath = AndroidProjectUtils.AbsFilePathForAndroidProject(Path.GetFileNameWithoutExtension(item.Name));
                PreLoadUtility.CheckVersionAndLoad(filePath, ScribeMetaHeaderUtility.ScribeHeaderMode.Xenotype, delegate
                {
                    if (AndroidProjectUtils.TryLoadAndroidProject(filePath, out var xenotype))
                    {
                        CharacterCardUtility.cachedCustomXenotypes.Add(xenotype);
                    }
                }, skipOnMismatch: true);
            }
        }
    }
}
