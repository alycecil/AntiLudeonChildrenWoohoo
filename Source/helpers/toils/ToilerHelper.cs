using System;
using RimWorld;
using Verse;
using Verse.AI;

namespace DarkIntentionsWoohoo
{
    public static class ToilerHelper
    {
        public static Toil GotoThing(Pawn pawn, Thing talkee, ToilCompleteMode mode = ToilCompleteMode.PatherArrival)
        {
            if (pawn == null || talkee == null)
            {
                Log.Error("Not Going Anywhere...", true); return null;
            }

            Toil toil = new Toil();
            toil.initAction = delegate()
            {
             /* Log.Message("[" + pawn.Name + "] go to [" + talkee + "]"); */
                
                pawn.pather.StartPath(talkee, PathEndMode.OnCell);

                if (talkee is Pawn)
                {
                    try
                    {
                        (talkee as Pawn).pather?.StartPath(talkee, PathEndMode.OnCell);
                    }
                    catch (Exception e)
                    {
                        ///snarf it.
                     /* Log.Message("Couldn't make the target hold still with pather, nbd." + e.Message, false); */
                    }
                }
            };
            toil.AddFinishAction(delegate { Log.Message("Got to ["+talkee+"]."); });
            toil.socialMode = RandomSocialMode.Off;
            toil.defaultCompleteMode = mode;
            return toil;
        }
    }
}