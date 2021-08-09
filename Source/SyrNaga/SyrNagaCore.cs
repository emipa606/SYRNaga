using UnityEngine;
using Verse;

namespace SyrNaga
{
    // Token: 0x02000017 RID: 23
    internal class SyrNagaCore : Mod
    {
        // Token: 0x0400004E RID: 78
        public static SyrNagaSettings settings;

        // Token: 0x06000050 RID: 80 RVA: 0x000044B6 File Offset: 0x000026B6
        public SyrNagaCore(ModContentPack content) : base(content)
        {
            settings = GetSettings<SyrNagaSettings>();
        }

        // Token: 0x06000051 RID: 81 RVA: 0x000044CA File Offset: 0x000026CA
        public override string SettingsCategory()
        {
            return "SyrNagaSettingsCategory".Translate();
        }

        // Token: 0x06000052 RID: 82 RVA: 0x000044DC File Offset: 0x000026DC
        public override void DoSettingsWindowContents(Rect inRect)
        {
            var listing_Standard = new Listing_Standard();
            listing_Standard.Begin(inRect);
            listing_Standard.CheckboxLabeled("SyrNaga_useStandardAI".Translate(), ref SyrNagaSettings.useStandardAI,
                "SyrNaga_useStandardAITooltip".Translate());
            if (listing_Standard.ButtonText("SyrNaga_defaultSettings".Translate(),
                "SyrNaga_defaultSettingsTooltip".Translate()))
            {
                SyrNagaSettings.useStandardAI = false;
            }

            listing_Standard.End();
            settings.Write();
        }

        // Token: 0x06000053 RID: 83 RVA: 0x00004559 File Offset: 0x00002759
    }
}