using HarmonyLib;
using Verse;

namespace SyrNaga
{
    // Token: 0x0200000D RID: 13
    [HarmonyPatch(typeof(PawnRenderer), "RenderPawnAt")]
    public static class RenderPawnAtPatch
    {
        // Token: 0x06000020 RID: 32 RVA: 0x00003114 File Offset: 0x00001314
        [HarmonyPostfix]
        public static void RenderPawnAt_Postfix(Pawn ___pawn)
        {
            if (___pawn.health != null && ___pawn.health.hediffSet.HasHediff(NagaDefOf.NagaShieldEmitter))
            {
                (___pawn.health.hediffSet.GetFirstHediffOfDef(NagaDefOf.NagaShieldEmitter) as ShieldHediff)
                    ?.DrawWornExtras();
            }
        }
    }
}