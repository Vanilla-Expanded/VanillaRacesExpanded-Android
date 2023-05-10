using HarmonyLib;
using System.Linq;
using UnityEngine;
using Verse;

namespace VREAndroids
{
    public class VREAndroidsMod : Mod
    {
        public static VREAndroidsSettings settings;

        public static Harmony harmony;
        public VREAndroidsMod(ModContentPack pack) : base(pack)
        {
            settings = GetSettings<VREAndroidsSettings>();
            harmony = new Harmony("VREAndroidsMod");
            harmony.PatchAll();
        }
        public override void DoSettingsWindowContents(Rect inRect)
        {
            base.DoSettingsWindowContents(inRect);
            settings.DoSettingsWindowContents(inRect);
        }

        public override string SettingsCategory()
        {
            return Content.Name;
        }
    }

    public class VREAndroidsSettings : ModSettings
    {

        public override void ExposeData()
        {
            base.ExposeData();

        }

        public void DoSettingsWindowContents(Rect inRect)
        {
            var ls = new Listing_Standard();
            ls.Begin(inRect);

            ls.End();
        }
	}
}
