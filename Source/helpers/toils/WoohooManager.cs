using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;
using Verse.AI;

namespace DarkIntentionsWoohoo
{
    static class WoohooManager
    {
        public static IEnumerable<Toil> makePartnerWoohoo(Pawn pawn, Pawn mate, Building_Bed bed)
        {
            Toil t;
            yield return (t = new Toil
            {    initAction = delegate
                {
                    /* Log.Message("Make Lover Go To Bed"); */
                    ToilerHelper.GotoThing(mate, bed); 
                },
                socialMode = RandomSocialMode.Off,
                defaultCompleteMode = ToilCompleteMode.Delay,
                defaultDuration = 2
            });
            t.AddPreInitAction(delegate
            {
//                /* Log.Message("Debug: Kill mates job"); */
//                if(PawnHelper.IsNotWoohooing(mate) ) mate.jobs.StopAll(true);
//                    
//                if (PawnHelper.IsNotWoohooing(mate))
//                {
//                    /* Log.Message("Asking for love"); */
//                    Job newJob = new Job(Constants.JobWooHooRecieve);
//                    mate.jobs.StartJob(newJob, JobCondition.InterruptForced, null, false, true, null, null);
//                }
                    
            });
        }


        public static IEnumerable<Toil> animateLovin(Pawn pawn, Pawn mate, Building_Bed bed, int len = 250)
        {
            if(bed == null) yield break;
            
            yield return Toils_Bed.GotoBed(TargetIndex.B);

            var layDown = Toils_LayDown.LayDown(TargetIndex.B, true, false, false, false);
            

            layDown.AddPreTickAction(delegate()
            {
                if (pawn.IsHashIntervalTick(100))
                {
                    /* Log.Message("Making Noises"); */
                    MoteMaker.ThrowMetaIcon(pawn.Position, pawn.Map, ThingDefOf.Mote_Heart);
                }
            });
            layDown.AddFinishAction(delegate { /* Log.Message("Done Woohooing"); */ });

            layDown.defaultCompleteMode = ToilCompleteMode.Delay;
            layDown.defaultDuration = len;

            yield return layDown;
        }

        public static IEnumerable<Toil> ToilsAskForWoohoo(Pawn pawn, Pawn mate, Building_Bed bed, bool askSuccess,
            HookupBedmanager hookupBedmanager)
        {
            yield return ToilerHelper.GotoThing(pawn, mate);

            yield return AskForWoohoo(pawn, mate, bed, askSuccess);

            if (askSuccess)
            {
                yield return new Toil
                {
                    initAction = delegate()
                    {
                        /* Log.Message("Claiming Bed spots"); */
                        hookupBedmanager.claim(pawn, mate);
                        /* Log.Message("Claimed Bed spots"); */
                    },
                    defaultCompleteMode = ToilCompleteMode.Instant
                };

                yield return ToilerHelper.GotoThing(pawn, bed);
            }
            else
            {
                if (PawnHelper.IsNotWoohooing(mate))
                {
                    yield return new Toil
                    {
                        initAction = delegate()
                        {
                            /* Log.Message("Cursing at for asking"); */

                            Job newJob = new Job(JobDefOf.Insult, pawn, bed);
                            mate.jobs.StartJob(newJob, JobCondition.InterruptForced, null, false, true, null, null,
                                true);
                        },
                        socialMode = RandomSocialMode.Off,
                        defaultCompleteMode = ToilCompleteMode.Instant
                    };
                }
            }

            yield break;
        }

        public static Toil AskForWoohoo(Pawn pawn, Pawn mate, Building_Bed bed, bool askSuccess)
        {
            ThingDef reply = askSuccess ? ThingDefOf.Mote_Heart : ThingDefOf.Mote_SleepZ;
            return new Toil
            {
                initAction = delegate()
                {
                    if (pawn.IsHashIntervalTick(100)) return;
                    //Log.Message("Sending Heart to ask");
                    MoteMaker.ThrowMetaIcon(pawn.Position, pawn.Map, ThingDefOf.Mote_Heart);
                    //skip on rare case as not critical just ui noise
                },
                tickAction = delegate()
                {
                    if (!pawn.IsHashIntervalTick(100)) return;
                    //Log.Message("Convincing is good idea");
                    MoteMaker.ThrowMetaIcon(pawn.Position, pawn.Map, ThingDefOf.Mote_Heart);

                    if (mate?.Position != null)
                    {
                        //Log.Message("Mate decides is good idea, or not");
                        MoteMaker.ThrowMetaIcon(mate.Position, pawn.Map, reply);
                    }

                    //MoteMaker.ThrowMetaIcon(mate.Position, mate.Map, );
                },
                socialMode = RandomSocialMode.Off,
                defaultCompleteMode = ToilCompleteMode.Delay,
                defaultDuration = 250
            };
        }
    }
}