using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;
using Verse.AI;

namespace DarkIntentionsWoohoo
{
    static class WoohooManager
    {
        public static IEnumerable<Toil> MakePartnerWoohoo(Pawn pawn, Pawn mate, Building_Bed bed)
        {
            int tick = 400;
            void NewFunction()
            {
                if (PawnHelper.IsNotWoohooing(mate))
                {
                 /* Log.Message("Asking for love job"); */
                    Job newJob = new Job(Constants.JobWooHooRecieve, pawn, bed)
                    {
                        playerForced = true //its important
                    };
                    mate.jobs.StartJob(newJob, JobCondition.InterruptForced);

                 /* Log.Message("Make Lover Go To Bed"); */
                    //mate.jobs.StartJob(, JobCondition.InterruptForced);
                }
                else
                {
                 /* Log.Message("Partner already doin it"); */
                }
            }

            Toil t = new Toil()
            {
                socialMode = RandomSocialMode.Off,
                tickAction = NewFunction,
                initAction = NewFunction
            };

            t.AddEndCondition(() => PawnHelper.IsNotWoohooing(mate) && (tick --) > 0 ? JobCondition.Ongoing : JobCondition.Succeeded);
            t.AddFinishAction(delegate { Log.Message("Got Partner to Start WooHoo-ing Allegedly."); });

            yield return t;
        }


        public static IEnumerable<Toil> AnimateLovin(Pawn pawn, Pawn mate, Building_Bed bed, Action finishAction = null,
            int len = 250)
        {
            if (bed == null) yield break;

            Toil t;
            yield return (t = ToilerHelper.GotoThing(pawn, bed));
                t.AddFinishAction(delegate { Log.Message("Got To Bed for woohooing"); });

            var layDown = new Toil()
            {
                initAction = delegate
                {
                    pawn?.pather?.StopDead();
                    if(pawn?.jobs != null)
                        pawn.jobs.posture = PawnPosture.LayingInBed;
                    
                },
                tickAction = delegate
                {
                    pawn?.GainComfortFromCellIfPossible();
                    
                },
            };
            layDown.AddFinishAction(delegate { 
                
                
                pawn?.needs?.joy?.GainJoy(Rand.Value *.15f, Constants.Joy_Woohoo);
                mate?.needs?.joy?.GainJoy(Rand.Value *.15f, Constants.Joy_Woohoo);

                /* Log.Message("[woohoo] animating done, woohoo joy between 0 and 5% added"); */
            });
            // Toils_LayDown.LayDown(TargetIndex.B, true, false, false, false);

            

            layDown.AddPreTickAction(delegate()
            {
                if (pawn.IsHashIntervalTick(100))
                {
                    /* Log.Message("Making Noises"); */
                    MoteMaker.ThrowMetaIcon(pawn.Position, pawn.Map, ThingDefOf.Mote_Heart);
                }
            });
            if (finishAction != null)
                layDown.AddFinishAction(finishAction);
            
            
            layDown.defaultCompleteMode = ToilCompleteMode.Delay;
            layDown.defaultDuration = len;

            yield return layDown;
        }

        public static IEnumerable<Toil> ToilsAskForWoohoo(Pawn pawn, Pawn mate, Building_Bed bed, bool askSuccess,
            HookupBedManager hookupBedManager)
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
                        hookupBedManager.claim(pawn, mate);
                    /* Log.Message("Claimed Bed spots"); */ 
                    },
                    defaultCompleteMode = ToilCompleteMode.Instant
                };
            }
            else
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

        public static Toil AskForWoohoo(Pawn pawn, Pawn mate, Building_Bed bed, bool askSuccess)
        {
            ThingDef reply = askSuccess ? ThingDefOf.Mote_Heart : ThingDefOf.Mote_SleepZ;
            var t = new Toil
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
                   /* Log.Message("Mate decides is good idea, or not"); */
                        MoteMaker.ThrowMetaIcon(mate.Position, pawn.Map, reply);
                    }
                },
                socialMode = RandomSocialMode.Off,
                defaultCompleteMode = ToilCompleteMode.Delay,
                defaultDuration = 250
            };

            t.AddFinishAction(delegate { Log.Message("Done Asking"); });

            return t;
        }
    }
}