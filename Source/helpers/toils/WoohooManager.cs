using System;
using System.Collections.Generic;
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
            yield return ( t = new Toil
            {
                socialMode = RandomSocialMode.Off,
                defaultCompleteMode = ToilCompleteMode.Delay,
                defaultDuration = 2
            });
            t.AddFinishAction(delegate
            {
                if (IsNotWoohooing(mate))
                {
                    Log.Message("Kick off woohoo");
                    Job newJob = new Job(Constants.JobWooHooRecieve, bed);
                    mate.jobs.StartJob(newJob, JobCondition.InterruptForced, null, false, true, null, null, false);
                }
                else
                {
                    Log.Message("Already woohooing");
                }
            }); 
        }

        

        public static IEnumerable<Toil> animateLovin(Pawn pawn, Pawn mate, Building_Bed bed, int len = 250)
        {
            yield return Toils_Bed.GotoBed(TargetIndex.B);

            var laydown =  Toils_LayDown.LayDown(TargetIndex.B, true, false, false, false);

            laydown.AddPreTickAction(delegate ()
            {
                if (pawn.IsHashIntervalTick(100))
                {
                    Log.Message("Making Noises");
                    MoteMaker.ThrowMetaIcon(pawn.Position, pawn.Map, ThingDefOf.Mote_Heart);

                }
            });
            laydown.AddFinishAction(delegate
            {
                Log.Message("Done Woohooing");
            });

            laydown.defaultCompleteMode = ToilCompleteMode.Delay;
            laydown.defaultDuration = len;

            yield return laydown;
        }

        public static bool IsThisJailLovin(Pawn pawn, Pawn mate, Building_Bed bed)
        {


            return (pawn != null && pawn.guest != null && pawn.guest.IsPrisoner)
            || (mate != null && mate.guest != null && mate.guest.IsPrisoner)
            || (bed != null && bed.ForPrisoners);


        }
        
        public static bool IsNotWoohooing(Pawn mate)
        {
            bool b = mate.CurJob == null || (
                       mate.CurJob.def != JobDefOf.Lovin 
                       && mate.CurJob.def != Constants.JobWooHoo 
                       && mate.CurJob.def != Constants.JobWooHoo_Baby
                       && mate.CurJob.def != Constants.JobWooHooRecieve
                       );

            Log.Message("["+mate.Name+"] : Woohooing?"+!b);
            return b;
        }

        
    }
}
