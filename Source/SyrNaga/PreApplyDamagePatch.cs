using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace SyrNaga
{
    // Token: 0x0200000B RID: 11
    [HarmonyPatch(typeof(Pawn), "PreApplyDamage")]
    public static class PreApplyDamagePatch
    {
        // Token: 0x0600001E RID: 30 RVA: 0x00002EDC File Offset: 0x000010DC
        [HarmonyPrefix]
        public static bool PreApplyDamage_Prefix(ref Pawn __instance, ref DamageInfo dinfo, out bool absorbed)
        {
            if (dinfo.Def != DamageDefOf.SurgicalCut && dinfo.Def != DamageDefOf.ExecutionCut &&
                dinfo.Def != DamageDefOf.EMP && dinfo.Def != DamageDefOf.Stun && __instance.health != null &&
                __instance.health.hediffSet.HasHediff(NagaDefOf.NagaShieldEmitter))
            {
                if (__instance.health.hediffSet.GetFirstHediffOfDef(NagaDefOf.NagaShieldEmitter) is ShieldHediff
                {
                    broken: false
                } shieldHediff && shieldHediff.AbsorbDamage(dinfo))
                {
                    if (shieldHediff.broken)
                    {
                        SoundDefOf.EnergyShield_Broken.PlayOneShot(new TargetInfo(__instance.Position, __instance.Map));
                        FleckMaker.Static(__instance.TrueCenter(), __instance.Map, FleckDefOf.ExplosionFlash, 12f);
                    }
                    else
                    {
                        SoundDefOf.EnergyShield_AbsorbDamage.PlayOneShot(new TargetInfo(__instance.Position,
                            __instance.Map));
                        shieldHediff.impactAngleVect = Vector3Utility.HorizontalVectorFromAngle(dinfo.Angle);
                        var vector = __instance.TrueCenter() + (shieldHediff.impactAngleVect.RotatedBy(180f) * 0.5f);
                        var num = Mathf.Min(10f, 2f + (dinfo.Amount / 10f));
                        FleckMaker.Static(vector, __instance.Map, FleckDefOf.ExplosionFlash, num);
                        var num2 = (int) num;
                        for (var i = 0; i < num2; i++)
                        {
                            FleckMaker.ThrowDustPuff(vector, __instance.Map, Rand.Range(0.8f, 1.2f));
                        }
                    }

                    absorbed = true;
                    return false;
                }
            }

            absorbed = false;
            return true;
        }
    }
}