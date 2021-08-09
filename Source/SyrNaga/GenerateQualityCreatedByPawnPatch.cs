using HarmonyLib;
using RimWorld;
using Verse;

namespace SyrNaga
{
    // Token: 0x02000005 RID: 5
    [HarmonyPatch(typeof(QualityUtility), "GenerateQualityCreatedByPawn", typeof(Pawn), typeof(SkillDef))]
    public class GenerateQualityCreatedByPawnPatch
    {
        // Token: 0x06000014 RID: 20 RVA: 0x00002779 File Offset: 0x00000979
        [HarmonyPostfix]
        [HarmonyPriority(0)]
        public static void GenerateQualityCreatedByPawn_Postfix(ref QualityCategory __result, Pawn pawn,
            SkillDef relevantSkill)
        {
            if (pawn?.def == null || pawn.def != NagaDefOf.Naga || !(Rand.Value < 0.2f))
            {
                return;
            }

            __result += 1;
            if (__result > QualityCategory.Legendary)
            {
                __result = QualityCategory.Legendary;
            }
        }
    }
}