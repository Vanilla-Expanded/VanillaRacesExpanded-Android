using HarmonyLib;
using RimWorld;
using System.IO;
using System.Linq;
using System.Reflection;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch]
    public static class CharacterCardUtility_LifestageAndXenotypeOptions_Subclass_Patch
    {
        public static MethodBase TargetMethod()
        {
            foreach (var subclass in typeof(CharacterCardUtility).GetNestedTypes(AccessTools.all))
            {

                var method = subclass.GetMethods(AccessTools.all).FirstOrDefault(x => x.Name.Contains("<LifestageAndXenotypeOptions>b__22"));
                if (method != null)
                {
                    return method;
                }
            }
            return null;
        }

        [HarmonyPriority(int.MaxValue)]
        public static bool Prefix(CustomXenotype ___customInner)
        {
            if (___customInner.IsAndroidType())
            {
                string path = AndroidProjectUtils.AbsFilePathForAndroidProject(___customInner.fileName);
                if (File.Exists(path))
                {
                    File.Delete(path);
                    CharacterCardUtility.cachedCustomXenotypes = null;
                }
                return false;
            }
            return true;
        }
    }
}
