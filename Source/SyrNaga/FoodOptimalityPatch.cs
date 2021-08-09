using HarmonyLib;
using RimWorld;
using Verse;

namespace SyrNaga
{
    // Token: 0x02000007 RID: 7
    [HarmonyPatch(typeof(FoodUtility), "FoodOptimality")]
    public static class FoodOptimalityPatch
    {
        // Token: 0x06000018 RID: 24 RVA: 0x000029BA File Offset: 0x00000BBA
        [HarmonyPriority(800)]
        [HarmonyPostfix]
        public static void FoodOptimality_Postfix(ref float __result, Pawn eater, Thing foodSource)
        {
            if (SyrNagaSettings.useStandardAI)
            {
                return;
            }

            __result = FoodOptimality_Method(__result, eater, foodSource);
        }

        // Token: 0x06000019 RID: 25 RVA: 0x000029D0 File Offset: 0x00000BD0
        public static float FoodOptimality_Method(float __result, Pawn eater, Thing foodSource)
        {
            var num = __result;
            if (eater?.def == null || foodSource == null)
            {
                return num;
            }

            var def = foodSource.def;
            bool isIngestible;
            if (def == null)
            {
                isIngestible = false;
            }
            else
            {
                var ingestible = def.ingestible;
                isIngestible = ingestible != null;
            }

            if (!isIngestible || eater.def != NagaDefOf.Naga)
            {
                return num;
            }

            if (foodSource.def == ThingDefOf.MealSurvivalPack || foodSource.def == ThingDefOf.Pemmican ||
                foodSource.def == NagaDefOf.MealLavish)
            {
                return num;
            }

            var compIngredients = foodSource.TryGetComp<CompIngredients>();
            if (compIngredients?.ingredients != null)
            {
                if (compIngredients.ingredients.Exists(delegate(ThingDef i1)
                {
                    var ingestible3 = i1.ingestible;
                    return ((ingestible3?.foodType ?? FoodTypeFlags.None) &
                            FoodTypeFlags.Meat) > FoodTypeFlags.None;
                }) && !compIngredients.ingredients.Exists(delegate(ThingDef i2)
                {
                    var ingestible4 = i2.ingestible;
                    return ((ingestible4?.foodType ?? FoodTypeFlags.None) &
                            FoodTypeFlags.Meat) == FoodTypeFlags.None;
                }))
                {
                    num += 20f;
                }
                else if (compIngredients.ingredients.Exists(delegate(ThingDef i1)
                {
                    var ingestible3 = i1.ingestible;
                    return ((ingestible3?.foodType ?? FoodTypeFlags.None) &
                            FoodTypeFlags.AnimalProduct) > FoodTypeFlags.None;
                }) && !compIngredients.ingredients.Exists(delegate(ThingDef i2)
                {
                    var ingestible4 = i2.ingestible;
                    return ((ingestible4?.foodType ?? FoodTypeFlags.None) &
                            FoodTypeFlags.AnimalProduct) == FoodTypeFlags.None;
                }))
                {
                    num += 20f;
                }
                else if (compIngredients.ingredients.Exists(delegate(ThingDef i)
                {
                    var ingestible2 = i.ingestible;
                    return ((ingestible2?.foodType ?? FoodTypeFlags.None) &
                            FoodTypeFlags.Meat) > FoodTypeFlags.None;
                }) && compIngredients.ingredients.Exists(delegate(ThingDef i1)
                {
                    var ingestible3 = i1.ingestible;
                    return ((ingestible3?.foodType ?? FoodTypeFlags.None) &
                            FoodTypeFlags.AnimalProduct) > FoodTypeFlags.None;
                }))
                {
                    num += 40f;
                }
                else if (compIngredients.ingredients.Exists(delegate(ThingDef i)
                {
                    var ingestible2 = i.ingestible;
                    return ((ingestible2?.foodType ?? FoodTypeFlags.None) &
                            FoodTypeFlags.Meat) > FoodTypeFlags.None;
                }) && compIngredients.ingredients.Exists(delegate(ThingDef i1)
                {
                    var ingestible3 = i1.ingestible;
                    return ((ingestible3?.foodType ?? FoodTypeFlags.None) &
                            FoodTypeFlags.Meat) == FoodTypeFlags.None;
                }))
                {
                    num += 0f;
                }
                else if (compIngredients.ingredients.Exists(delegate(ThingDef i)
                {
                    var ingestible2 = i.ingestible;
                    return ((ingestible2?.foodType ?? FoodTypeFlags.None) &
                            FoodTypeFlags.AnimalProduct) > FoodTypeFlags.None;
                }) && compIngredients.ingredients.Exists(delegate(ThingDef i1)
                {
                    var ingestible3 = i1.ingestible;
                    return ((ingestible3?.foodType ?? FoodTypeFlags.None) &
                            FoodTypeFlags.Meat) == FoodTypeFlags.None;
                }))
                {
                    num += 10f;
                }
                else
                {
                    num -= 20f;
                }
            }
            else if ((foodSource.def.ingestible.foodType & FoodTypeFlags.Meat) != FoodTypeFlags.None)
            {
                num += 20f;
            }
            else if ((foodSource.def.ingestible.foodType & FoodTypeFlags.AnimalProduct) != FoodTypeFlags.None)
            {
                num += 40f;
            }
            else if ((foodSource.def.ingestible.foodType & FoodTypeFlags.Corpse) != FoodTypeFlags.None)
            {
                num += 20f;
            }
            else
            {
                num -= 20f;
            }

            return num;
        }
    }
}