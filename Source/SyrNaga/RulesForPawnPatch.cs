﻿using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Grammar;

namespace SyrNaga
{
    // Token: 0x0200000E RID: 14
    public static class RulesForPawnPatch
    {
        // Token: 0x06000021 RID: 33 RVA: 0x00003161 File Offset: 0x00001361
        public static IEnumerable<Rule> RulesForPawn_Postfix(IEnumerable<Rule> __result, string pawnSymbol,
            string title, Gender gender, PawnKindDef kind)
        {
            var list = __result.ToList();
            var str = "";
            if (!pawnSymbol.NullOrEmpty())
            {
                str = str + pawnSymbol + "_";
            }

            for (var i = 0; i < list.Count; i++)
            {
                var rule_String = list[i] as Rule_String;
                if (rule_String != null && rule_String.keyword == str + "titleIndef")
                {
                    list[i] = new Rule_String(str + "titleIndef",
                        Find.ActiveLanguageWorker.WithIndefiniteArticle(kind.race.label + " " + title, gender));
                }
                else if (rule_String != null && rule_String.keyword == str + "titleDef")
                {
                    list[i] = new Rule_String(str + "titleDef",
                        Find.ActiveLanguageWorker.WithDefiniteArticle(kind.race.label + " " + title, gender));
                }
                else if (rule_String != null && rule_String.keyword == str + "title")
                {
                    list[i] = new Rule_String(str + "title", kind.race.label + " " + title);
                }
            }

            __result = list;
            foreach (var rule in __result)
            {
                yield return rule;
            }
        }
    }
}