using System.Collections.Generic;
using HarmonyLib;
using Verse;

namespace SyrNaga
{
    // Token: 0x0200000C RID: 12
    [HarmonyPatch(typeof(Pawn), "GetGizmos")]
    public static class GetGizmosPatch
    {
        // Token: 0x0600001F RID: 31 RVA: 0x000030A0 File Offset: 0x000012A0
        [HarmonyPostfix]
        public static void GetGizmos_Postfix(ref Pawn __instance, ref IEnumerable<Gizmo> __result)
        {
            if (__instance.health == null || !__instance.health.hediffSet.HasHediff(NagaDefOf.NagaShieldEmitter) ||
                Find.Selector.NumSelected != 1)
            {
                return;
            }

            var shield =
                __instance.health.hediffSet.GetFirstHediffOfDef(NagaDefOf.NagaShieldEmitter) as ShieldHediff;
            __result = __result.AddItem(new Gizmo_HediffComp_Shield
            {
                shield = shield
            });
        }
    }
}