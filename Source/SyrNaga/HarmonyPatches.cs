using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace SyrNaga
{
    // Token: 0x02000004 RID: 4
    [StaticConstructorOnStartup]
    public static class HarmonyPatches
    {
        // Token: 0x04000014 RID: 20
        public static int femaleVariants;

        // Token: 0x04000015 RID: 21
        public static int maleVariants;

        // Token: 0x06000011 RID: 17 RVA: 0x000025A0 File Offset: 0x000007A0
        static HarmonyPatches()
        {
            var harmony = new Harmony("Syrchalis.Rimworld.Naga");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            femaleVariants = TextureVariants(true);
            maleVariants = TextureVariants(false);
            if (!ThrumkinActive)
            {
                harmony.Patch(AccessTools.Method(typeof(GrammarUtility), "RulesForPawn", new[]
                {
                    typeof(string),
                    typeof(Name),
                    typeof(string),
                    typeof(PawnKindDef),
                    typeof(Gender),
                    typeof(Faction),
                    typeof(int),
                    typeof(int),
                    typeof(string),
                    typeof(bool),
                    typeof(bool),
                    typeof(bool),
                    typeof(List<RoyalTitle>),
                    typeof(Dictionary<string, string>),
                    typeof(bool)
                }), null, new HarmonyMethod(AccessTools.Method(typeof(RulesForPawnPatch), "RulesForPawn_Postfix")));
            }
        }

        // Token: 0x1700000B RID: 11
        // (get) Token: 0x06000012 RID: 18 RVA: 0x000026ED File Offset: 0x000008ED
        public static bool ThrumkinActive
        {
            get { return ModsConfig.ActiveModsInLoadOrder.Any(m => m.PackageId == "syrchalis.thrumkin"); }
        }

        // Token: 0x06000013 RID: 19 RVA: 0x00002718 File Offset: 0x00000918
        public static int TextureVariants(bool female)
        {
            var num = 0;
            var str = female ? "Things/Naga/Body/Naked_Female" : "Things/Naga/Body/Naked_Male";

            var texture2D = ContentFinder<Texture2D>.Get(str + "_east", false);
            while (texture2D != null)
            {
                num++;
                texture2D = ContentFinder<Texture2D>.Get(str + num + "_east", false);
            }

            return num;
        }
    }
}