using AlienRace;
using HarmonyLib;
using Verse;

namespace SyrNaga
{
    // Token: 0x02000008 RID: 8
    [HarmonyPatch(typeof(PawnGenerator), "GenerateNewPawnInternal")]
    public static class GenerateNewPawnInternalPatch
    {
        // Token: 0x0600001A RID: 26 RVA: 0x00002BE4 File Offset: 0x00000DE4
        [HarmonyPostfix]
        public static void GenerateNewPawnInternal_Postfix(ref PawnGenerationRequest request, Pawn __result)
        {
            if (__result == null || __result.def != NagaDefOf.Naga)
            {
                return;
            }

            __result.health.AddHediff(HediffMaker.MakeHediff(NagaDefOf.HiddenNagaHediff, __result));
            __result.GetComp<AlienPartGenerator.AlienComp>().GetChannel("skin").second =
                NagaUtility.AssignSecondColor(__result.story.SkinColor);
        }
    }
}