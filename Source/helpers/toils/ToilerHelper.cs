using RimWorld;
using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace DarkIntentionsWoohoo
{
    public class ToilerHelper
    {
        public static IEnumerable<Toil> ToilsAskForWoohoo(Pawn pawn, Pawn mate, Building_Bed bed,  bool askSuccess, HookupBedmanager hookupBedmanager)
        {
           
            yield return GotoThing(pawn, mate);

            yield return AskForWoohoo(pawn, mate, bed, askSuccess);

            if (askSuccess)
            {
                yield return new Toil
                {
                    initAction = delegate ()
                    {
                        Log.Message("Claiming Bed spots");
                        hookupBedmanager.claim(pawn, mate);
                        Log.Message("Claimed Bed spots");


                    },
                    defaultCompleteMode = ToilCompleteMode.Instant
                };

                yield return new Toil
                {
                    initAction = delegate ()
                    {
                        
                            Job newJob = new Job(JobDefOf.Goto, bed);
                            mate.jobs.StartJob(newJob, JobCondition.InterruptForced, null, false, true, null, null, false);
                            
                        
                    },
                    socialMode = RandomSocialMode.Off,
                    defaultCompleteMode = ToilCompleteMode.Delay,
                    defaultDuration = 2
                };

                yield return GotoThing(pawn, bed);
            }
            else
            {
                yield return new Toil
                {
                    initAction = delegate ()
                    {

                        Job newJob = new Job(JobDefOf.Insult, pawn, bed);
                        mate.jobs.StartJob(newJob, JobCondition.InterruptForced, null, false, true, null, null, false);


                    },
                    socialMode = RandomSocialMode.Off,
                    defaultCompleteMode = ToilCompleteMode.Delay,
                    defaultDuration = 2
                };
            }
            
            yield break;
        }

        public static Toil AskForWoohoo(Pawn pawn, Pawn mate, Building_Bed bed, bool askSuccess)
        {
            ThingDef reply = askSuccess ? ThingDefOf.Mote_Heart : ThingDefOf.Mote_SleepZ;
            return new Toil
            {   initAction = delegate()
                {
                    if (!pawn.IsHashIntervalTick(100))
                    {
                        //Log.Message("Sending Heart to ask");
                        MoteMaker.ThrowMetaIcon(pawn.Position, pawn.Map, ThingDefOf.Mote_Heart);
                    }//skip on rare case as not critical just ui noise
                },
                tickAction = delegate ()
                {
                    if (pawn.IsHashIntervalTick(100))
                    {
                        //Log.Message("Convincing is good idea");
                        MoteMaker.ThrowMetaIcon(pawn.Position, pawn.Map, ThingDefOf.Mote_Heart);

                        if (mate != null && mate.Position != null)
                        {
                            //Log.Message("Mate decides is good idea, or not");
                            MoteMaker.ThrowMetaIcon(mate.Position, pawn.Map, reply);
                        }

                        //MoteMaker.ThrowMetaIcon(mate.Position, mate.Map, );
                    }

                },
                socialMode = RandomSocialMode.Off,
                defaultCompleteMode = ToilCompleteMode.Delay,
                defaultDuration = 250
            };
        }

        public static Toil GotoThing(Pawn pawn, Thing talkee)
        {
            Toil toil = new Toil();
            toil.initAction = delegate ()
            {
                pawn.pather.StartPath(talkee, PathEndMode.Touch);

                if (talkee as Pawn != null)
                {
                    try
                    {
                        if ((talkee as Pawn).pather != null)
                            (talkee as Pawn).pather.StopDead();

                    }
                    catch (Exception e)
                    {
                        ///snarf it.
                        Log.Message("Counldn't make the target hold still with pather, nbd." + e.Message, false);
                    }
                }
            };
            toil.AddFailCondition(() => talkee.DestroyedOrNull());
            toil.socialMode = RandomSocialMode.Off;
            toil.defaultCompleteMode = ToilCompleteMode.PatherArrival;
            return toil;
        }
    }
}
