using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace DarkIntentionsWoohoo
{
    class JobDriver_Woohoo_Receive : JobDriver
    {
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            Log.Message("Getting Asked to WooHoo!", false);
            return IsGoodToGo();
        }

        private bool IsGoodToGo()
        {
            Log.Message("We good to go?", false);
            return pawn != null && TargetA != null && (TargetA.Thing as Pawn) != null && TargetB != null &&
                   (TargetB.Thing as Building_Bed) != null;
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            if (!IsGoodToGo()) return null;
            Log.Message("Lets go make babies!", false);

            return WoohooManager.animateLovin(pawn, TargetA.Thing as Pawn, TargetB.Thing as Building_Bed, 2000);
        }
    }
}