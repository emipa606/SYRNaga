using UnityEngine;
using Verse;

namespace SyrNaga
{
    // Token: 0x02000015 RID: 21
    public class EmitterMote : MoteThrown
    {
        // Token: 0x0400004B RID: 75
        public Vector3 targetVector;

        // Token: 0x0600004C RID: 76 RVA: 0x00004378 File Offset: 0x00002578
        protected override void TimeInterval(float deltaTime)
        {
            base.TimeInterval(deltaTime);
            var position = exactPosition;
            var vector = targetVector;
            if (exactPosition.ToIntVec3() == targetVector.ToIntVec3() && !Destroyed)
            {
                Destroy();
            }
        }
    }
}