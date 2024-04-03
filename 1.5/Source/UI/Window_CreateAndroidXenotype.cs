using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace VREAndroids
{
    [HotSwappable]
    public class Window_CreateAndroidXenotype : Window_CreateAndroidBase
    {
        private int generationRequestIndex;
        public override string Header => "VREA.CreateAndroid".Translate().CapitalizeFirst();
        public override string AcceptButtonLabel => "SaveAndApply".Translate().CapitalizeFirst();
        public Window_CreateAndroidXenotype(int index, Action callback) : base(callback)
        {
            generationRequestIndex = index;
            alwaysUseFullBiostatsTableHeight = true;
            searchWidgetOffsetX = ButSize.x * 2f + 4f;
            disableAndroidHardwareLimitation = true;
        }

        public override bool CanAccept()
        {
            if (!base.CanAccept())
            {
                return false;
            }
            if (GenFilePaths.AllCustomXenotypeFiles.EnumerableCount() >= 200)
            {
                Messages.Message("MessageTooManyCustomXenotypes", null, MessageTypeDefOf.RejectInput, historical: false);
                return false;
            }
            return true;
        }

        protected override void AcceptInner()
        {
            CustomXenotype customXenotype = new CustomXenotype();
            customXenotype.name = xenotypeName?.Trim();
            customXenotype.genes.AddRange(selectedGenes);
            customXenotype.inheritable = false;
            customXenotype.iconDef = iconDef;
            string text = GenFile.SanitizedFileName(customXenotype.name);
            string absPath = AndroidProjectUtils.AbsFilePathForAndroidProject(text);
            LongEventHandler.QueueLongEvent(delegate
            {
                AndroidProjectUtils.SaveAndroidProject(customXenotype, absPath);
            }, "SavingLongEvent", doAsynchronously: false, null);
            if (generationRequestIndex >= 0)
            {
                PawnGenerationRequest generationRequest = StartingPawnUtility.GetGenerationRequest(generationRequestIndex);
                generationRequest.ForcedXenotype = null;
                generationRequest.ForcedCustomXenotype = customXenotype;
                StartingPawnUtility.SetGenerationRequest(generationRequestIndex, generationRequest);
            }
        }

        public override void DrawSearchRect(Rect rect)
        {
            base.DrawSearchRect(rect);
            if (Widgets.ButtonText(new Rect(rect.xMax - ButSize.x, rect.y, ButSize.x, ButSize.y), "LoadCustom".Translate()))
            {
                Find.WindowStack.Add(new Dialog_AndroidProjectList_Load(delegate (CustomXenotype xenotype)
                {
                    xenotypeName = xenotype.name;
                    xenotypeNameLocked = true;
                    selectedGenes.Clear();
                    selectedGenes.AddRange(xenotype.genes);
                    selectedGenes = selectedGenes.Distinct().ToList();
                    iconDef = xenotype.IconDef;
                    OnGenesChanged();
                }));
            }
            if (!Widgets.ButtonText(new Rect(rect.xMax - ButSize.x * 2f - 4f, rect.y, ButSize.x, ButSize.y), "LoadPremade".Translate()))
            {
                return;
            }
            List<FloatMenuOption> list = new List<FloatMenuOption>();
            foreach (XenotypeDef item in DefDatabase<XenotypeDef>.AllDefs.OrderBy((XenotypeDef c) => 0f - c.displayPriority))
            {
                XenotypeDef xenotype2 = item;
                if (xenotype2.IsAndroidType())
                {
                    list.Add(new FloatMenuOption(xenotype2.LabelCap, delegate
                    {
                        xenotypeName = xenotype2.label;
                        selectedGenes.Clear();
                        selectedGenes.AddRange(xenotype2.genes);
                        selectedGenes = selectedGenes.Distinct().ToList();
                        OnGenesChanged();
                    }, xenotype2.Icon, XenotypeDef.IconColor, MenuOptionPriority.Default, delegate (Rect r)
                    {
                        TooltipHandler.TipRegion(r, xenotype2.descriptionShort ?? xenotype2.description);
                    }));
                }
            }
            Find.WindowStack.Add(new FloatMenu(list));
        }
    }
}
