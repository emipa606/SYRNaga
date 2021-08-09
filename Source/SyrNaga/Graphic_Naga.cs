using UnityEngine;
using Verse;

namespace SyrNaga
{
    // Token: 0x02000003 RID: 3
    public class Graphic_Naga : Graphic
    {
        // Token: 0x04000010 RID: 16
        private readonly Material[] mats = new Material[4];

        // Token: 0x04000013 RID: 19
        private float drawRotatedExtraAngleOffset;

        // Token: 0x04000012 RID: 18
        private bool eastFlipped;

        // Token: 0x04000011 RID: 17
        private bool westFlipped;

        // Token: 0x17000001 RID: 1
        // (get) Token: 0x06000002 RID: 2 RVA: 0x00002052 File Offset: 0x00000252
        public string GraphicPath => path;

        // Token: 0x17000002 RID: 2
        // (get) Token: 0x06000003 RID: 3 RVA: 0x0000205A File Offset: 0x0000025A
        public override Material MatSingle => MatSouth;

        // Token: 0x17000003 RID: 3
        // (get) Token: 0x06000004 RID: 4 RVA: 0x00002062 File Offset: 0x00000262
        public override Material MatWest => mats[3];

        // Token: 0x17000004 RID: 4
        // (get) Token: 0x06000005 RID: 5 RVA: 0x0000206C File Offset: 0x0000026C
        public override Material MatSouth => mats[2];

        // Token: 0x17000005 RID: 5
        // (get) Token: 0x06000006 RID: 6 RVA: 0x00002076 File Offset: 0x00000276
        public override Material MatEast => mats[1];

        // Token: 0x17000006 RID: 6
        // (get) Token: 0x06000007 RID: 7 RVA: 0x00002080 File Offset: 0x00000280
        public override Material MatNorth => mats[0];

        // Token: 0x17000007 RID: 7
        // (get) Token: 0x06000008 RID: 8 RVA: 0x0000208A File Offset: 0x0000028A
        public override bool WestFlipped => westFlipped;

        // Token: 0x17000008 RID: 8
        // (get) Token: 0x06000009 RID: 9 RVA: 0x00002092 File Offset: 0x00000292
        public override bool EastFlipped => eastFlipped;

        // Token: 0x17000009 RID: 9
        // (get) Token: 0x0600000A RID: 10 RVA: 0x0000209A File Offset: 0x0000029A
        public override bool ShouldDrawRotated =>
            (data == null || data.drawRotated) && (MatEast == MatNorth || MatWest == MatNorth);

        // Token: 0x1700000A RID: 10
        // (get) Token: 0x0600000B RID: 11 RVA: 0x000020D9 File Offset: 0x000002D9
        public override float DrawRotatedExtraAngleOffset => drawRotatedExtraAngleOffset;

        // Token: 0x0600000C RID: 12 RVA: 0x000020E4 File Offset: 0x000002E4
        public override void Init(GraphicRequest req)
        {
            data = req.graphicData;
            path = req.path;
            color = req.color;
            colorTwo = req.colorTwo;
            drawSize = req.drawSize;
            var array = new Texture2D[mats.Length];
            array[0] = ContentFinder<Texture2D>.Get(req.path + "_north", false);
            array[1] = ContentFinder<Texture2D>.Get(req.path + "_east", false);
            array[2] = ContentFinder<Texture2D>.Get(req.path + "_south", false);
            array[3] = ContentFinder<Texture2D>.Get(req.path + "_west", false);
            if (array[0] == null)
            {
                if (array[2] != null)
                {
                    array[0] = array[2];
                    drawRotatedExtraAngleOffset = 180f;
                }
                else if (array[1] != null)
                {
                    array[0] = array[1];
                    drawRotatedExtraAngleOffset = -90f;
                }
                else
                {
                    if (!(array[3] != null))
                    {
                        Log.Error(
                            "Failed to find any texture while constructing " + this.ToStringSafe() +
                            ". Filenames have changed; if you are converting an old mod, recommend renaming textures from *_back to *_north, *_side to *_east, and *_front to *_south.");
                        return;
                    }

                    array[0] = array[3];
                    drawRotatedExtraAngleOffset = 90f;
                }
            }

            if (array[2] == null)
            {
                array[2] = array[0];
            }

            if (array[1] == null)
            {
                if (array[3] != null)
                {
                    array[1] = array[3];
                    eastFlipped = DataAllowsFlip;
                }
                else
                {
                    array[1] = array[0];
                }
            }

            if (array[3] == null)
            {
                if (array[1] != null)
                {
                    array[3] = array[1];
                    westFlipped = DataAllowsFlip;
                }
                else
                {
                    array[3] = array[0];
                }
            }

            var array2 = new Texture2D[mats.Length];
            if (req.shader.SupportsMaskTex())
            {
                array2[0] = ContentFinder<Texture2D>.Get(req.path + "_northm", false);
                array2[1] = ContentFinder<Texture2D>.Get(req.path + "_eastm", false);
                array2[2] = ContentFinder<Texture2D>.Get(req.path + "_southm", false);
                array2[3] = ContentFinder<Texture2D>.Get(req.path + "_westm", false);
                if (array2[0] == null)
                {
                    if (array2[2] != null)
                    {
                        array2[0] = array2[2];
                    }
                    else if (array2[1] != null)
                    {
                        array2[0] = array2[1];
                    }
                    else if (array2[3] != null)
                    {
                        array2[0] = array2[3];
                    }
                }

                if (array2[2] == null)
                {
                    array2[2] = array2[0];
                }

                if (array2[1] == null)
                {
                    if (array2[3] != null)
                    {
                        array2[1] = array2[3];
                    }
                    else
                    {
                        array2[1] = array2[0];
                    }
                }

                if (array2[3] == null)
                {
                    if (array2[1] != null)
                    {
                        array2[3] = array2[1];
                    }
                    else
                    {
                        array2[3] = array2[0];
                    }
                }

                if (array2[0] == null && array2[1] == null && array2[2] == null && array2[3] == null)
                {
                    if (req.path.Contains("Things/Naga/Body/Naked_Female"))
                    {
                        array2[0] = ContentFinder<Texture2D>.Get("Things/Naga/Body/Naked_Female_northm", false);
                        array2[1] = ContentFinder<Texture2D>.Get("Things/Naga/Body/Naked_Female_eastm", false);
                        array2[2] = ContentFinder<Texture2D>.Get("Things/Naga/Body/Naked_Female_southm", false);
                        array2[3] = ContentFinder<Texture2D>.Get("Things/Naga/Body/Naked_Female_eastm", false);
                    }
                    else if (req.path.Contains("Things/Naga/Body/Naked_Male"))
                    {
                        array2[0] = ContentFinder<Texture2D>.Get("Things/Naga/Body/Naked_Male_northm", false);
                        array2[1] = ContentFinder<Texture2D>.Get("Things/Naga/Body/Naked_Male_eastm", false);
                        array2[2] = ContentFinder<Texture2D>.Get("Things/Naga/Body/Naked_Male_southm", false);
                        array2[3] = ContentFinder<Texture2D>.Get("Things/Naga/Body/Naked_Male_eastm", false);
                    }
                }
            }

            for (var i = 0; i < mats.Length; i++)
            {
                var materialRequest = default(MaterialRequest);
                materialRequest.mainTex = array[i];
                materialRequest.shader = req.shader;
                materialRequest.color = color;
                materialRequest.colorTwo = colorTwo;
                materialRequest.maskTex = array2[i];
                materialRequest.shaderParameters = req.shaderParameters;
                mats[i] = MaterialPool.MatFrom(materialRequest);
            }
        }

        // Token: 0x0600000D RID: 13 RVA: 0x000024EB File Offset: 0x000006EB
        public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
        {
            return GraphicDatabase.Get<Graphic_Multi>(path, newShader, drawSize, newColor, newColorTwo, data);
        }

        // Token: 0x0600000E RID: 14 RVA: 0x00002508 File Offset: 0x00000708
        public override string ToString()
        {
            return string.Concat("Multi(initPath=", path, ", color=", color, ", colorTwo=", colorTwo, ")");
        }

        // Token: 0x0600000F RID: 15 RVA: 0x00002565 File Offset: 0x00000765
        public override int GetHashCode()
        {
            return Gen.HashCombineStruct(Gen.HashCombineStruct(Gen.HashCombine(0, path), color), colorTwo);
        }
    }
}