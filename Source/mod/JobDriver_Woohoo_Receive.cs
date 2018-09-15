using System.Collections.Generic;
using Verse.AI;

namespace DarkIntentionsWoohoo
{
    class JobDriver_Woohoo_Recieve : JobDriver
    {
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return pawn != null;
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            yield return ToilerHelper.GotoThing(pawn, TargetThingA);
            yield return ToilerHelper.GotoThing(pawn, TargetThingB);
            yield break;
        }
    }
}