using RimWorld;
using Verse;

namespace SyrNaga
{
    // Token: 0x02000019 RID: 25
    public class ThoughtWorker_NagaThought : ThoughtWorker
    {
        // Token: 0x06000056 RID: 86 RVA: 0x00004584 File Offset: 0x00002784
        protected override ThoughtState CurrentSocialStateInternal(Pawn pawn, Pawn other)
        {
            if (!other.RaceProps.Humanlike || !RelationsUtility.PawnsKnowEachOther(pawn, other))
            {
                return false;
            }

            if (pawn.def != ThingDefOf.Human || other.def != NagaDefOf.Naga)
            {
                return false;
            }

            return true;
        }
    }
}