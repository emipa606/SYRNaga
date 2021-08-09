using System.Collections.Generic;
using RimWorld;
using Verse;

namespace SyrNaga
{
    // Token: 0x02000016 RID: 22
    public class SkillNeed_DirectNaga : SkillNeed
    {
        // Token: 0x0400004C RID: 76
        public List<float> valuesPerLevel = new List<float>();

        // Token: 0x0400004D RID: 77
        public List<float> valuesPerLevelNaga = new List<float>();

        // Token: 0x0600004E RID: 78 RVA: 0x000043D0 File Offset: 0x000025D0
        public override float ValueFor(Pawn pawn)
        {
            if (pawn.skills == null)
            {
                return 1f;
            }

            var level = pawn.skills.GetSkill(skill).Level;
            if (pawn.def == NagaDefOf.Naga)
            {
                if (valuesPerLevelNaga.Count > level)
                {
                    return valuesPerLevelNaga[level];
                }

                if (valuesPerLevelNaga.Count > 0)
                {
                    return valuesPerLevelNaga[valuesPerLevelNaga.Count - 1];
                }
            }
            else
            {
                if (valuesPerLevel.Count > level)
                {
                    return valuesPerLevel[level];
                }

                if (valuesPerLevel.Count > 0)
                {
                    return valuesPerLevel[valuesPerLevel.Count - 1];
                }
            }

            return 1f;
        }
    }
}