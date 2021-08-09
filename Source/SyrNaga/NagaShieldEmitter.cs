using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace SyrNaga
{
    // Token: 0x02000014 RID: 20
    public class NagaShieldEmitter : Apparel
    {
        // Token: 0x06000048 RID: 72 RVA: 0x00004030 File Offset: 0x00002230
        public override bool CheckPreAbsorbDamage(DamageInfo dinfo)
        {
            if (!dinfo.Def.harmsHealth || !dinfo.Def.ExternalViolenceFor(this) || dinfo.Weapon == null)
            {
                return true;
            }

            var statValue = this.GetStatValue(NagaDefOf.ShieldEmitterRadius);
            var statValue2 = this.GetStatValue(StatDefOf.EnergyShieldEnergyMax);
            var statValue3 = this.GetStatValue(NagaDefOf.ShieldEmitterDecayRate);
            foreach (var thing in
                GenRadial.RadialDistinctThingsAround(Wearer.Position, Wearer.Map, statValue, true))
            {
                if (thing is not Pawn pawn || pawn.Faction == null || pawn.HostileTo(Wearer))
                {
                    continue;
                }

                if (pawn.health.hediffSet.HasHediff(NagaDefOf.NagaShieldEmitter))
                {
                    if (pawn.health.hediffSet.GetFirstHediffOfDef(NagaDefOf.NagaShieldEmitter) is ShieldHediff
                        shieldHediff && shieldHediff.shieldCurrent <= statValue2)
                    {
                        pawn.health.RemoveHediff(shieldHediff);
                    }
                }

                if (pawn.health.AddHediff(NagaDefOf.NagaShieldEmitter) is ShieldHediff shieldHediff2)
                {
                    shieldHediff2.shieldMax *= statValue2;
                    shieldHediff2.shieldCurrent = shieldHediff2.shieldMax;
                    shieldHediff2.shieldDecayPerSec = statValue3;
                }

                if (pawn != Wearer)
                {
                    ShieldEmitterBurst(Wearer, pawn.DrawPos, NagaDefOf.Mote_ShieldEmitterBurst);
                }
            }

            FleckMaker.ThrowLightningGlow(Wearer.TrueCenter(), Wearer.Map, 4f);
            ShieldEmitter(Wearer, Wearer.Map, statValue, NagaDefOf.Mote_ShieldEmitter);
            NagaDefOf.ShieldEmitterPop.PlayOneShot(new TargetInfo(Wearer.Position, Wearer.Map));
            Destroy();

            return true;
        }

        // Token: 0x06000049 RID: 73 RVA: 0x00004238 File Offset: 0x00002438
        public static void ShieldEmitter(Pawn caster, Map map, float radius, FleckDef moteDef)
        {
            if (!caster.Position.ShouldSpawnMotesAt(map) || map.moteCounter.Saturated)
            {
                return;
            }

            var fleckData = FleckMaker.GetDataStatic(caster.DrawPos, caster.Map, moteDef, radius / 3f);
            fleckData.rotationRate = 0f;
            fleckData.rotation = Rand.Range(0, 360);
            caster.Map.flecks.CreateFleck(fleckData);
        }

        // Token: 0x0600004A RID: 74 RVA: 0x000042B4 File Offset: 0x000024B4
        private static void ShieldEmitterBurst(Pawn thrower, Vector3 targetCell, FleckDef fleck)
        {
            if (!thrower.Position.ShouldSpawnMotesAt(thrower.Map) || thrower.Map.moteCounter.Saturated)
            {
                return;
            }

            var num = Rand.Range(20f, 30f);
            var fleckData = FleckMaker.GetDataStatic(thrower.DrawPos, thrower.Map, fleck);
            fleckData.rotationRate = 0f;
            fleckData.rotation = (targetCell - thrower.DrawPos).AngleFlat();
            fleckData.velocityAngle = (targetCell - thrower.DrawPos).AngleFlat();
            fleckData.velocitySpeed = num;
            fleckData.velocity = targetCell;
            thrower.Map.flecks.CreateFleck(fleckData);
        }
    }
}