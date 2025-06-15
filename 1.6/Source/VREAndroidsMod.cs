using HarmonyLib;
using System.Linq;
using UnityEngine;
using Verse;

namespace VREAndroids
{
    public class VREAndroidsMod : Mod
    {
        public static Harmony harmony;
        public VREAndroidsMod(ModContentPack pack) : base(pack)
        {
            harmony = new Harmony("VREAndroidsMod");
            harmony.PatchAll();
        }
    }
}
