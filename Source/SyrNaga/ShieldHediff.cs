using System.Text;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace SyrNaga
{
    // Token: 0x02000010 RID: 16
    [StaticConstructorOnStartup]
    public class ShieldHediff : HediffWithComps
    {
        // Token: 0x04000032 RID: 50
        public static Color shieldColor = new Color(0.75f, 1f, 0.88f);

        // Token: 0x04000033 RID: 51
        private static readonly Material BubbleMat =
            MaterialPool.MatFrom("Things/Mote/NagaShieldBubble", ShaderDatabase.Transparent, shieldColor);

        // Token: 0x04000035 RID: 53
        private readonly bool removedOnBroken = true;

        // Token: 0x0400002F RID: 47
        public bool broken;

        // Token: 0x0400002E RID: 46
        public int cooldownTicks;

        // Token: 0x04000031 RID: 49
        public Vector3 impactAngleVect;

        // Token: 0x04000030 RID: 48
        public int lastAbsorbDamageTick = -9999;

        // Token: 0x04000034 RID: 52
        private float savedAngle;

        // Token: 0x0400002C RID: 44
        public float shieldCurrent = 1f;

        // Token: 0x0400002A RID: 42
        public float shieldDecayPerSec = 0.033f;

        // Token: 0x0400002B RID: 43
        public float shieldLossPerDmg = 0.033f;

        // Token: 0x0400002D RID: 45
        public float shieldMax = 1f;

        // Token: 0x1700000C RID: 12
        // (get) Token: 0x06000026 RID: 38 RVA: 0x00003552 File Offset: 0x00001752
        public override bool ShouldRemove => removedOnBroken && broken;

        // Token: 0x06000024 RID: 36 RVA: 0x000034C4 File Offset: 0x000016C4
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref shieldDecayPerSec, "shieldDecayPerSec", 0.033f);
            Scribe_Values.Look(ref shieldCurrent, "shieldCurrent", 1f);
            Scribe_Values.Look(ref shieldMax, "shieldMax", 1f);
            Scribe_Values.Look(ref cooldownTicks, "cooldownTicks");
            Scribe_Values.Look(ref broken, "broken");
        }

        // Token: 0x06000025 RID: 37 RVA: 0x0000353D File Offset: 0x0000173D
        public override void PostAdd(DamageInfo? dinfo)
        {
            base.PostAdd(dinfo);
            shieldCurrent = shieldMax;
        }

        // Token: 0x06000027 RID: 39 RVA: 0x00003568 File Offset: 0x00001768
        public override void Tick()
        {
            base.Tick();
            if (cooldownTicks > 0)
            {
                cooldownTicks--;
                return;
            }

            shieldCurrent -= shieldDecayPerSec / 60f;
            if (shieldCurrent <= 0f)
            {
                broken = true;
                NagaDefOf.ShieldEmitterEnd.PlayOneShot(new TargetInfo(pawn.Position, pawn.Map));
            }

            if (!removedOnBroken)
            {
                Reset();
            }
        }

        // Token: 0x06000028 RID: 40 RVA: 0x000035FC File Offset: 0x000017FC
        public void Reset()
        {
            if (pawn.Spawned)
            {
                SoundDefOf.EnergyShield_Reset.PlayOneShot(new TargetInfo(pawn.Position, pawn.Map));
                FleckMaker.ThrowLightningGlow(pawn.TrueCenter(), pawn.Map, 3f);
            }

            cooldownTicks = 0;
            shieldCurrent = shieldMax * 0.2f;
            broken = false;
        }

        // Token: 0x06000029 RID: 41 RVA: 0x00003684 File Offset: 0x00001884
        public bool AbsorbDamage(DamageInfo dinfo)
        {
            lastAbsorbDamageTick = Find.TickManager.TicksGame;
            impactAngleVect = Vector3Utility.HorizontalVectorFromAngle(dinfo.Angle);
            if (broken)
            {
                return false;
            }

            shieldCurrent -= dinfo.Amount * shieldLossPerDmg;
            if (shieldCurrent <= 0f)
            {
                broken = true;
            }

            return true;
        }

        // Token: 0x0600002A RID: 42 RVA: 0x000036F0 File Offset: 0x000018F0
        public void DrawWornExtras()
        {
            if (pawn.Dead || pawn.Downed)
            {
                return;
            }

            var num = Mathf.Lerp(1.2f, 1.55f, shieldCurrent);
            var vector = pawn.Drawer.DrawPos;
            vector.y = AltitudeLayer.Blueprint.AltitudeFor();
            var num2 = Find.TickManager.TicksGame - lastAbsorbDamageTick;
            if (num2 < 8)
            {
                var num3 = (8 - num2) / 8f * 0.05f;
                vector += impactAngleVect * num3;
                num -= num3;
            }

            var angle = savedAngle;
            savedAngle += 1f;
            var s = new Vector3(num, 1f, num);
            var matrix = default(Matrix4x4);
            matrix.SetTRS(vector, Quaternion.AngleAxis(angle, Vector3.up), s);
            Graphics.DrawMesh(MeshPool.plane10, matrix, BubbleMat, 0);
        }

        // Token: 0x0600002B RID: 43 RVA: 0x000037E9 File Offset: 0x000019E9
        public override string DebugString()
        {
            var stringBuilder = new StringBuilder(base.DebugString());
            stringBuilder.AppendLine("cooldownTicks: " + cooldownTicks);
            return stringBuilder.ToString();
        }
    }
}