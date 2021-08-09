using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace SyrNaga
{
    // Token: 0x02000012 RID: 18
    [StaticConstructorOnStartup]
    public class NagaShieldBelt : Apparel
    {
        // Token: 0x0400003E RID: 62
        private const float MinDrawSize = 1.2f;

        // Token: 0x0400003F RID: 63
        private const float MaxDrawSize = 1.55f;

        // Token: 0x04000040 RID: 64
        private const float MaxDamagedJitterDist = 0.05f;

        // Token: 0x04000041 RID: 65
        private const int JitterDurationTicks = 8;

        // Token: 0x04000047 RID: 71
        private static readonly Material BubbleMat =
            MaterialPool.MatFrom("Other/ShieldBubble", ShaderDatabase.Transparent);

        // Token: 0x04000046 RID: 70
        private readonly float ApparelScorePerEnergyMax = 0.5f;

        // Token: 0x04000044 RID: 68
        private readonly float EnergyLossPerDamage = 0.033f;

        // Token: 0x04000045 RID: 69
        private readonly int KeepDisplayingTicks = 1000;

        // Token: 0x04000042 RID: 66
        private readonly int StartingTicksToReset = 300;

        // Token: 0x04000039 RID: 57
        private float energy;

        // Token: 0x04000043 RID: 67
        private float EnergyOnReset;

        // Token: 0x0400003C RID: 60
        private Vector3 impactAngleVect;

        // Token: 0x0400003D RID: 61
        private int lastAbsorbDamageTick = -9999;

        // Token: 0x0400003B RID: 59
        private int lastKeepDisplayTick = -9999;

        // Token: 0x0400003A RID: 58
        private int ticksToReset = -1;

        // Token: 0x1700000D RID: 13
        // (get) Token: 0x06000033 RID: 51 RVA: 0x0000394D File Offset: 0x00001B4D
        private float EnergyMax => this.GetStatValue(StatDefOf.EnergyShieldEnergyMax);

        // Token: 0x1700000E RID: 14
        // (get) Token: 0x06000034 RID: 52 RVA: 0x0000395B File Offset: 0x00001B5B
        private float EnergyGainPerTick => this.GetStatValue(StatDefOf.EnergyShieldRechargeRate) / 60f;

        // Token: 0x1700000F RID: 15
        // (get) Token: 0x06000035 RID: 53 RVA: 0x0000396F File Offset: 0x00001B6F
        public float Energy => energy;

        // Token: 0x17000010 RID: 16
        // (get) Token: 0x06000036 RID: 54 RVA: 0x00003977 File Offset: 0x00001B77
        public ShieldState ShieldState
        {
            get
            {
                if (ticksToReset > 0)
                {
                    return ShieldState.Resetting;
                }

                return ShieldState.Active;
            }
        }

        // Token: 0x17000011 RID: 17
        // (get) Token: 0x06000037 RID: 55 RVA: 0x00003988 File Offset: 0x00001B88
        private bool ShouldDisplay
        {
            get
            {
                var wearer = Wearer;
                return wearer.Spawned && !wearer.Dead && !wearer.Downed && (wearer.InAggroMentalState ||
                                                                            wearer.Drafted ||
                                                                            wearer.Faction
                                                                                .HostileTo(Faction.OfPlayer) &&
                                                                            !wearer.IsPrisoner ||
                                                                            Find.TickManager.TicksGame <
                                                                            lastKeepDisplayTick + KeepDisplayingTicks);
            }
        }

        // Token: 0x06000032 RID: 50 RVA: 0x0000394A File Offset: 0x00001B4A
        public override bool AllowVerbCast(Verb verb)
        {
            return true;
        }

        // Token: 0x06000038 RID: 56 RVA: 0x000039FC File Offset: 0x00001BFC
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref energy, "energy");
            Scribe_Values.Look(ref ticksToReset, "ticksToReset", -1);
            Scribe_Values.Look(ref lastKeepDisplayTick, "lastKeepDisplayTick");
        }

        // Token: 0x06000039 RID: 57 RVA: 0x00003A49 File Offset: 0x00001C49
        public override IEnumerable<Gizmo> GetWornGizmos()
        {
            if (Find.Selector.SingleSelectedThing == Wearer)
            {
                yield return new Gizmo_NagaEnergyShieldStatus
                {
                    shield = this
                };
            }
        }

        // Token: 0x0600003A RID: 58 RVA: 0x00003A59 File Offset: 0x00001C59
        public override float GetSpecialApparelScoreOffset()
        {
            return EnergyMax * ApparelScorePerEnergyMax;
        }

        // Token: 0x0600003B RID: 59 RVA: 0x00003A68 File Offset: 0x00001C68
        public override void Tick()
        {
            base.Tick();
            if (Wearer == null)
            {
                energy = 0f;
                return;
            }

            if (ShieldState == ShieldState.Resetting)
            {
                ticksToReset--;
                if (ticksToReset <= 0)
                {
                    Reset();
                }
            }
            else if (ShieldState == ShieldState.Active)
            {
                energy += EnergyGainPerTick;
                if (energy > EnergyMax)
                {
                    energy = EnergyMax;
                }
            }
        }

        // Token: 0x0600003C RID: 60 RVA: 0x00003AEC File Offset: 0x00001CEC
        public override bool CheckPreAbsorbDamage(DamageInfo dinfo)
        {
            if (ShieldState != ShieldState.Active ||
                (dinfo.Instigator == null || dinfo.Instigator.Position.AdjacentTo8WayOrInside(Wearer.Position)) &&
                !dinfo.Def.isExplosive)
            {
                return false;
            }

            if (dinfo.Instigator != null)
            {
                if (dinfo.Instigator is AttachableThing attachableThing && attachableThing.parent == Wearer)
                {
                    return false;
                }
            }

            energy -= dinfo.Amount * EnergyLossPerDamage;
            if (dinfo.Def == DamageDefOf.EMP)
            {
                energy = -1f;
            }

            if (energy < 0f)
            {
                Break();
            }
            else
            {
                AbsorbedDamage(dinfo);
            }

            return true;
        }

        // Token: 0x0600003D RID: 61 RVA: 0x00003BB5 File Offset: 0x00001DB5
        public void KeepDisplaying()
        {
            lastKeepDisplayTick = Find.TickManager.TicksGame;
        }

        // Token: 0x0600003E RID: 62 RVA: 0x00003BC8 File Offset: 0x00001DC8
        private void AbsorbedDamage(DamageInfo dinfo)
        {
            SoundDefOf.EnergyShield_AbsorbDamage.PlayOneShot(new TargetInfo(Wearer.Position, Wearer.Map));
            impactAngleVect = Vector3Utility.HorizontalVectorFromAngle(dinfo.Angle);
            var vector = Wearer.TrueCenter() + (impactAngleVect.RotatedBy(180f) * 0.5f);
            var num = Mathf.Min(10f, 2f + (dinfo.Amount / 10f));
            FleckMaker.Static(vector, Wearer.Map, FleckDefOf.ExplosionFlash, num);
            var num2 = (int) num;
            for (var i = 0; i < num2; i++)
            {
                FleckMaker.ThrowDustPuff(vector, Wearer.Map, Rand.Range(0.8f, 1.2f));
            }

            lastAbsorbDamageTick = Find.TickManager.TicksGame;
            KeepDisplaying();
        }

        // Token: 0x0600003F RID: 63 RVA: 0x00003CB8 File Offset: 0x00001EB8
        private void Break()
        {
            SoundDefOf.EnergyShield_Broken.PlayOneShot(new TargetInfo(Wearer.Position, Wearer.Map));
            FleckMaker.Static(Wearer.TrueCenter(), Wearer.Map, FleckDefOf.ExplosionFlash, 12f);
            for (var i = 0; i < 6; i++)
            {
                FleckMaker.ThrowDustPuff(
                    Wearer.TrueCenter() + (Vector3Utility.HorizontalVectorFromAngle(Rand.Range(0, 360)) *
                                           Rand.Range(0.3f, 0.6f)), Wearer.Map, Rand.Range(0.8f, 1.2f));
                FleckMaker.ThrowLightningGlow(Wearer.TrueCenter(), Wearer.Map, 2f);
            }

            energy = 0f;
            ticksToReset = StartingTicksToReset;
        }

        // Token: 0x06000040 RID: 64 RVA: 0x00003DB0 File Offset: 0x00001FB0
        private void Reset()
        {
            if (Wearer.Spawned)
            {
                SoundDefOf.EnergyShield_Reset.PlayOneShot(new TargetInfo(Wearer.Position, Wearer.Map));
                FleckMaker.ThrowLightningGlow(Wearer.TrueCenter(), Wearer.Map, 3f);
            }

            ticksToReset = -1;
            energy = EnergyOnReset;
        }

        // Token: 0x06000041 RID: 65 RVA: 0x00003E28 File Offset: 0x00002028
        public override void DrawWornExtras()
        {
            if (ShieldState != ShieldState.Active || !ShouldDisplay)
            {
                return;
            }

            var num = Mathf.Lerp(1.2f, 1.55f, energy);
            var vector = Wearer.Drawer.DrawPos;
            vector.y = AltitudeLayer.Blueprint.AltitudeFor();
            var num2 = Find.TickManager.TicksGame - lastAbsorbDamageTick;
            if (num2 < 8)
            {
                var num3 = (8 - num2) / 8f * 0.05f;
                vector += impactAngleVect * num3;
                num -= num3;
            }

            float angle = Rand.Range(0, 360);
            var s = new Vector3(num, 1f, num);
            var matrix = default(Matrix4x4);
            matrix.SetTRS(vector, Quaternion.AngleAxis(angle, Vector3.up), s);
            Graphics.DrawMesh(MeshPool.plane10, matrix, BubbleMat, 0);
        }
    }
}