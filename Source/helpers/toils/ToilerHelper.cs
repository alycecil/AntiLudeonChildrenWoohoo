using System;
using RimWorld;
using Verse;
using Verse.AI;

namespace DarkIntentionsWoohoo
{
    public static class ToilerHelper
    {
        public static Toil GotoThing(Pawn pawn, Thing thing, ToilCompleteMode mode = ToilCompleteMode.PatherArrival)
        {
            if (pawn == null || thing == null)
            {
                Log.Error("Not Going Anywhere...", true); return null;
            }

            Toil toil = new Toil
            {
                initAction = delegate()
                {
                    pawn?.pather?.StartPath(thing, PathEndMode.OnCell);

                    if (thing is Pawn)
                    {
                        try
                        {
                            (thing as Pawn)?.pather?.StartPath(thing, PathEndMode.OnCell);
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            };
            toil.socialMode = RandomSocialMode.Off;
            toil.defaultCompleteMode = mode;
            return toil;
        }
    }
}