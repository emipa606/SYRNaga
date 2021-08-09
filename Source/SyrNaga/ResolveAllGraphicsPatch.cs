using System.Collections.Generic;
using System.Linq;
using AlienRace;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace SyrNaga
{
    // Token: 0x02000009 RID: 9
    [HarmonyPatch(typeof(PawnGraphicSet), "ResolveAllGraphics")]
    public static class ResolveAllGraphicsPatch
    {
        // Token: 0x0600001B RID: 27 RVA: 0x00002C48 File Offset: 0x00000E48
        [HarmonyPrefix]
        [HarmonyPriority(0)]
        public static void ResolveAllGraphics_Prefix(PawnGraphicSet __instance)
        {
            var pawn = __instance.pawn;
            if (pawn?.def == null || pawn.def != NagaDefOf.Naga ||
                pawn.def is not ThingDef_AlienRace thingDef_AlienRace)
            {
                return;
            }

            var alienPartGenerator = thingDef_AlienRace.alienRace.generalSettings.alienPartGenerator;
            var currentGraphicPath =
                GraphicPathsExtension.GetCurrentGraphicPath(thingDef_AlienRace.alienRace.graphicPaths,
                    pawn.ageTracker.CurLifeStage);
            if (pawn.gender == Gender.Female && pawn.story.bodyType == BodyTypeDefOf.Male ||
                pawn.gender == Gender.Male && pawn.story.bodyType == BodyTypeDefOf.Female)
            {
                pawn.story.bodyType = pawn.gender == Gender.Female ? BodyTypeDefOf.Female : BodyTypeDefOf.Male;
            }

            var nakedPath = AlienPartGenerator.GetNakedPath(pawn.story.bodyType, currentGraphicPath.body,
                thingDef_AlienRace.alienRace.generalSettings.alienPartGenerator.useGenderedBodies
                    ? pawn.gender.ToString()
                    : "");
            var num = pawn.thingIDNumber % (pawn.gender == Gender.Female
                ? HarmonyPatches.femaleVariants
                : HarmonyPatches.maleVariants);
            __instance.nakedGraphic = GraphicDatabase.Get(typeof(Graphic_Naga),
                num == 0 ? nakedPath : nakedPath + num, ShaderDatabase.CutoutComplex, Vector2.one,
                pawn.story.SkinColor, alienPartGenerator.SkinColor(pawn, false), null, null);
        }

        // Token: 0x0600001C RID: 28 RVA: 0x00002DC4 File Offset: 0x00000FC4
        public static GraphicPaths GetCurrentGraphicPath(this List<GraphicPaths> list, LifeStageDef lifeStageDef)
        {
            return list.FirstOrDefault(delegate(GraphicPaths gp)
            {
                var lifeStageDefs = gp.lifeStageDefs;
                return lifeStageDefs != null && lifeStageDefs.Contains(lifeStageDef);
            }) ?? list.First();
        }
    }
}