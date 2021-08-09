using RimWorld;
using UnityEngine;
using Verse;

namespace SyrNaga
{
    // Token: 0x02000013 RID: 19
    [StaticConstructorOnStartup]
    public class Gizmo_NagaEnergyShieldStatus : Gizmo
    {
        // Token: 0x04000049 RID: 73
        private static readonly Texture2D FullShieldBarTex =
            SolidColorMaterials.NewSolidColorTexture(new Color(0.2f, 0.2f, 0.24f));

        // Token: 0x0400004A RID: 74
        private static readonly Texture2D EmptyShieldBarTex = SolidColorMaterials.NewSolidColorTexture(Color.clear);

        // Token: 0x04000048 RID: 72
        public NagaShieldBelt shield;

        // Token: 0x06000044 RID: 68 RVA: 0x00003F7E File Offset: 0x0000217E
        public Gizmo_NagaEnergyShieldStatus()
        {
            order = -100f;
        }

        // Token: 0x06000045 RID: 69 RVA: 0x0000390C File Of•fset: 0x00001B0C
        public override float GetWidth(float maxWidth)
        {
            return 140f;
        }

        // Token: 0x06000046 RID: 70 RVA: 0x00003F94 File Offset: 0x00002194
        public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
        {
            var overRect = new Rect(topLeft.x, topLeft.y, GetWidth(maxWidth), 75f);
            Find.WindowStack.ImmediateWindow(984688, overRect, WindowLayer.GameUI, delegate
            {
                Rect rect2;
                var rect = rect2 = overRect.AtZero().ContractedBy(6f);
                rect2.height = overRect.height / 2f;
                Text.Font = GameFont.Tiny;
                Widgets.Label(rect2, shield.LabelCap);
                var rect3 = rect;
                rect3.yMin = overRect.height / 2f;
                var fillPercent = shield.Energy / shield.GetStatValue(StatDefOf.EnergyShieldEnergyMax);
                Widgets.FillableBar(rect3, fillPercent, FullShieldBarTex, EmptyShieldBarTex, false);
                Text.Font = GameFont.Small;
                Text.Anchor = TextAnchor.MiddleCenter;
                Widgets.Label(rect3,
                    (shield.Energy * 100f).ToString("F0") + " / " +
                    (shield.GetStatValue(StatDefOf.EnergyShieldEnergyMax) * 100f).ToString("F0"));
                Text.Anchor = TextAnchor.UpperLeft;
            });
            return new GizmoResult(GizmoState.Clear);
        }
    }
}