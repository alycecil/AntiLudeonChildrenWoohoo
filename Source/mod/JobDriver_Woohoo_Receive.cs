using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace DarkIntentionsWoohoo
{
    class JobDriver_Woohoo_Receive : JobDriver
    {
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            Log.Message("Getting Asked to WooHoo!");
            return true;
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            Log.Message("Lets go make babies!");
            var toil = new Toil {initAction = delegate { Log.Message("Woohooing"); }};
            toil.AddEndCondition( () => JobCondition.Ongoing);
            yield return toil;
            
            //add a moodlet for being asked 
        }
    }
}