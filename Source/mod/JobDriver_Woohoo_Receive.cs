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
            var list = new List<Toil>();

            list.Add( Toils_Goto.Goto(TargetIndex.A, PathEndMode.ClosestTouch));
            list.Add( Toils_Goto.Goto(TargetIndex.B, PathEndMode.ClosestTouch));
            list.AddRange( WoohooManager.AnimateLovin(pawn, TargetA.Thing as Pawn, TargetB.Thing as Building_Bed) );

            Toil t;
            list.Add( t = new Toil {initAction = delegate { Log.Message("Getting Woohooing, will be done in 400 ticks"); }, defaultDuration = 400, defaultCompleteMode = ToilCompleteMode.Delay});
            t.AddFinishAction(delegate { Log.Message("Done Woohing Get"); });

            pawn.mindState.canLovinTick = Find.TickManager.TicksGame + + Rand.Range(1500, 25000);
            //add a moodlet for being asked
            return list;
        }
    }
}