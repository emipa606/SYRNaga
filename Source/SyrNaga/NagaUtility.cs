using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace SyrNaga
{
    // Token: 0x0200000F RID: 15
    public static class NagaUtility
    {
        // Token: 0x04000016 RID: 22
        public static Color orange1 = new Color(0.875f, 0.75f, 0.625f);

        // Token: 0x04000017 RID: 23
        public static Color orange2 = new Color(0.875f, 0.5f, 0.125f);

        // Token: 0x04000018 RID: 24
        public static Color yellow1 = new Color(0.9f, 0.9f, 0.5f);

        // Token: 0x04000019 RID: 25
        public static Color lime2 = new Color(0.375f, 0.75f, 0.25f);

        // Token: 0x0400001A RID: 26
        public static Color green1 = new Color(0.625f, 0.875f, 0.625f);

        // Token: 0x0400001B RID: 27
        public static Color green2 = new Color(0.25f, 0.625f, 0.25f);

        // Token: 0x0400001C RID: 28
        public static Color black1 = new Color(0.5f, 0.5f, 0.5f);

        // Token: 0x0400001D RID: 29
        public static Color black2 = new Color(0.3f, 0.3f, 0.3f);

        // Token: 0x0400001E RID: 30
        public static Color red1 = new Color(0.75f, 0.375f, 0.375f);

        // Token: 0x0400001F RID: 31
        public static Color red2 = new Color(0.625f, 0.125f, 0.125f);

        // Token: 0x04000020 RID: 32
        public static Color white1 = new Color(1f, 1f, 0.9f);

        // Token: 0x04000021 RID: 33
        public static Color white2 = new Color(0.9f, 0.9f, 0.9f);

        // Token: 0x04000022 RID: 34
        public static Color blue1 = new Color(0.675f, 0.875f, 1f);

        // Token: 0x04000023 RID: 35
        public static Color blue2 = new Color(0.25f, 0.5f, 0.875f);

        // Token: 0x04000024 RID: 36
        public static Color blue3 = new Color(0.375f, 0.375f, 0.75f);

        // Token: 0x04000025 RID: 37
        public static Color purple1 = new Color(0.75f, 0.625f, 0.875f);

        // Token: 0x04000026 RID: 38
        public static Color purple2 = new Color(0.5f, 0.25f, 0.75f);

        // Token: 0x04000027 RID: 39
        public static Color aqua1 = new Color(0.625f, 0.875f, 0.75f);

        // Token: 0x04000028 RID: 40
        public static Color aqua2 = new Color(0.25f, 0.75f, 0.5f);

        // Token: 0x04000029 RID: 41
        public static Dictionary<Color, Color> colorPairs = new Dictionary<Color, Color>
        {
            {
                orange1,
                orange2
            },
            {
                yellow1,
                lime2
            },
            {
                green1,
                green2
            },
            {
                black1,
                black2
            },
            {
                red1,
                red2
            },
            {
                white1,
                white2
            },
            {
                blue1,
                blue2
            },
            {
                purple1,
                purple2
            },
            {
                aqua1,
                aqua2
            }
        };

        // Token: 0x06000022 RID: 34 RVA: 0x00003190 File Offset: 0x00001390
        public static Color AssignSecondColor(Color firstColor)
        {
            var value = Rand.Value;
            if (firstColor == green1 && value < 0.5f)
            {
                return aqua2;
            }

            if (firstColor == orange1 && value < 0.25f)
            {
                return red2;
            }

            if (firstColor == orange1 && value < 0.75f)
            {
                return black2;
            }

            if (firstColor == blue1 && value < 0.33f)
            {
                return blue3;
            }

            if (firstColor == aqua1 && value < 0.5f)
            {
                return blue2;
            }

            return colorPairs.TryGetValue(firstColor);
        }
    }
}