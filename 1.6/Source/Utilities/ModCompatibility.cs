using HarmonyLib;
using System;
using System.Collections.Generic;
using Verse;

namespace VREAndroids
{
    [StaticConstructorOnStartup]
    public static class ModCompatibility
    {
        public static bool DubsMintMenusActive = ModLister.AnyModActiveNoSuffix(["Dubwise.DubsMintMenus"]);
        public static bool SnapOutActive = ModLister.AnyModActiveNoSuffix(["weilbyte.snapout"]);
        public static bool MSE2Active = ModLister.AnyModActiveNoSuffix(["MSE2.Core"]);
        public static Type ignoreSubPartsExtensionType;
        static ModCompatibility()
        {
            if (DubsMintMenusActive)
            {
                LongEventHandler.ExecuteWhenFinished(delegate
                {
                    if (DubsMintMenus_Patch_HealthCardUtility_GenerateListing_Patch.Prepare())
                    {
                        VREAndroidsMod.harmony.Patch(DubsMintMenus_Patch_HealthCardUtility_GenerateListing_Patch.TargetMethod(),
                            prefix: new HarmonyMethod(AccessTools.Method(typeof(DubsMintMenus_Patch_HealthCardUtility_GenerateListing_Patch),
                            nameof(DubsMintMenus_Patch_HealthCardUtility_GenerateListing_Patch.Prefix))));
                    }
                    else
                    {
                        Log.Error("[VREAndroids] Failed to patch Dubs Mint Menus");
                    }
                });
            }
            if (SnapOutActive)
            {
                var type = AccessTools.TypeByName("SnapOut.SnapCheck");
                var field = AccessTools.Field(type, "incompatDef");
                var list = field.GetValue(null) as List<string>;
                list.Add(VREA_DefOf.VREA_Reformatting.defName);
                list.Add(VREA_DefOf.VREA_SolarFlared.defName);
            }
            if (MSE2Active)
            {
                ignoreSubPartsExtensionType = AccessTools.TypeByName("MSE2.IgnoreSubParts");
            }
        }
    }
}
