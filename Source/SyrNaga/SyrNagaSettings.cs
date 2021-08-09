using Verse;

namespace SyrNaga
{
    // Token: 0x02000018 RID: 24
    internal class SyrNagaSettings : ModSettings
    {
        // Token: 0x0400004F RID: 79
        public static bool useStandardAI;

        // Token: 0x06000054 RID: 84 RVA: 0x00004561 File Offset: 0x00002761
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref useStandardAI, "SyrNaga_usestandardAI", false, true);
        }
    }
}