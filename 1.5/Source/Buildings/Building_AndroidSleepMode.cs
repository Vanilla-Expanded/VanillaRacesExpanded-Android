﻿using RimWorld;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace VREAndroids
{
    public class Building_AndroidSleepMode : Building
    {
        public Pawn android;
        public override string Label => android.Label + " (" + "VREA.SleepMode".Translate().ToLower() + ")";
        public override void DrawAt(Vector3 drawLoc, bool flip = false)
        {
            android.DrawAt(DrawPos);
        }

        public override bool ClaimableBy(RimWorld.Faction by, StringBuilder reason = null)
        {
            return false;
        }
        public override bool DeconstructibleBy(RimWorld.Faction faction)
        {
            return false;
        }

        public override IEnumerable<Gizmo> GetGizmos()
        {
            yield return new Command_Action
            {
                defaultLabel = "VREA.SleepModeOff".Translate(),
                defaultDesc = "VREA.SleepModeOffDesc".Translate(),
                icon = ContentFinder<Texture2D>.Get("UI/Gizmos/SleepMode_WakeUp"),
                action = delegate
                {
                    GenSpawn.Spawn(android, Position, Map);
                    Find.Selector.Select(android);
                    this.Destroy();
                }
            };
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Deep.Look(ref android, "android");
        }
    }
}
