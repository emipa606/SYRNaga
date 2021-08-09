using UnityEngine;
using Verse;

namespace SyrNaga
{
    // Token: 0x02000011 RID: 17
    [StaticConstructorOnStartup]
    public class Gizmo_HediffComp_Shield : Gizmo
    {
        // Token: 0x04000037 RID: 55
        private static readonly Texture2D FullShieldBarTex =
            SolidColorMaterials.NewSolidColorTexture(new Color(0.2f, 0.4f, 0.3f));

        // Token: 0x04000038 RID: 56
        private static readonly Texture2D EmptyShieldBarTex = SolidColorMaterials.NewSolidColorTexture(Color.clear);

        // Token: 0x04000036 RID: 54
        public ShieldHediff shield;

        // Token: 0x0600002E RID: 46 RVA: 0x000038A0 File Offset: 0x00001AA0
        public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
        {
            var overRect = new Rect(topLeft.x, topLeft.y, GetWidth(maxWidth), 75f);
            Find.WindowStack.ImmediateWindow(942612547, overRect, 0, delegate
            {
                Rect rect2;
                var rect = rect2 = overRect.AtZero().ContractedBy(6f);
                rect2.height = overRect.height / 2f;
                Text.Font = GameFont.Tiny;
                Widgets.Label(rect2, shield.def.LabelCap);
                var rect3 = rect;
                rect3.yMin = overRect.height / 2f;
                var num = shield.shieldCurrent / shield.shieldMax;
                Widgets.FillableBar(rect3, num, FullShieldBarTex, EmptyShieldBarTex, false);
                Text.Font = GameFont.Small;
                Text.Anchor = TextAnchor.MiddleCenter;
                Widgets.Label(rect3,
                    (shield.shieldCurrent * 100f).ToString("F0") + " / " + (shield.shieldMax * 100f).ToString("F0"));
                Text.Anchor = TextAnchor.UpperLeft;
            });
            return new GizmoResult(GizmoState.Clear);
        }

        // Token: 0x0600002F RID: 47 RVA: 0x0000390C File Offset: 0x00001B0C
        public override float GetWidth(float maxWidth)
        {
            return 140f;
        }
    }
}