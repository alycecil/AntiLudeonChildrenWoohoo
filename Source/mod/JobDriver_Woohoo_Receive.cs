using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace DarkIntentionsWoohoo
{
    class JobDriver_Woohoo_Receive : JobDriver_Lovin
    {
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            Log.Message("Getting Asked to WooHoo!");
            return pawn != null;
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            Log.Message("Lets go make babies!");
            var list = new List<Toil>();

            list.Add( Toils_Goto.Goto(TargetIndex.A, PathEndMode.ClosestTouch));
            list.Add( Toils_Goto.Goto(TargetIndex.B, PathEndMode.ClosestTouch));
            list.AddRange( WoohooManager.AnimateLovin(pawn, TargetA.Thing as Pawn, TargetB.Thing as Building_Bed) );
            
            list.Add( new Toil {initAction = delegate { Log.Message("Woohooing"); }, defaultDuration = 4000, defaultCompleteMode = ToilCompleteMode.Delay});
            
            
            //add a moodlet for being asked
            return list;
        }
    }
}