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
       /* Log.Message("Getting Asked to WooHoo!"); */ 
            return pawn != null;
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
     /* Log.Message("I got asked to do woohoo!!!!"); */
            var list = new List<Toil>
            {
                Toils_Goto.Goto(TargetIndex.A, PathEndMode.ClosestTouch),
                Toils_Goto.Goto(TargetIndex.B, PathEndMode.ClosestTouch)
            };

            list.AddRange( WoohooManager.AnimateLovin(pawn, TargetA.Thing as Pawn, TargetB.Thing as Building_Bed) );

            Toil t;
            list.Add( t = new Toil {initAction = delegate
            {
            }, defaultDuration = 400, defaultCompleteMode = ToilCompleteMode.Delay});

            PawnHelper.DelayNextWooHoo(pawn);
            //add a moodlet for being asked
            return list;
        }
    }
}