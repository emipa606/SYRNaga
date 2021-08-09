using HarmonyLib;
using RimWorld;
using Verse;

namespace SyrNaga
{
    // Token: 0x0200000A RID: 10
    [HarmonyPatch(typeof(ApparelUtility), "HasPartsToWear")]
    public static class HasPartsToWearPatch
    {
        // Token: 0x0600001D RID: 29 RVA: 0x00002DFC File Offset: 0x00000FFC
        [HarmonyPostfix]
        public static void HasPartsToWear_Postfix(ref bool __result, Pawn p, ThingDef apparel)
        {
            bool hasApparel;
            if (apparel == null)
            {
                hasApparel = false;
            }
            else
            {
                var apparel2 = apparel.apparel;
                hasApparel = apparel2?.bodyPartGroups != null;
            }

            if (!hasApparel || p?.def == null)
            {
                return;
            }

            if (p.def == NagaDefOf.Naga && apparel.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.Legs) &&
                !apparel.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.Torso))
            {
                __result = false;
            }

            if (p.def == NagaDefOf.Naga && apparel.apparel.bodyPartGroups.Contains(NagaDefOf.Feet) &&
                !apparel.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.Torso))
            {
                __result = false;
            }

            if (p.def != NagaDefOf.Naga && apparel.apparel.bodyPartGroups.Contains(NagaDefOf.TailAttackTool))
            {
                __result = false;
            }
        }
    }
}