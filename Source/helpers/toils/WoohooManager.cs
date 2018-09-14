using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;

namespace DarkIntentionsWoohoo
{
    class WoohooManager
    {
        public static IEnumerable<Toil> makePartnerWoohoo(Pawn pawn, Pawn mate, Building_Bed bed)
        {
            yield return new Toil
            {
                initAction = delegate ()
                {
                    if (mate.CurJob == null || (mate.CurJob.def != JobDefOf.Lovin && mate.CurJob.def != Constants.JobWooHoo && mate.CurJob.def != Constants.JobWooHoo_Baby))
                    {
                        //Log.Message("Asking for love");
                        Job newJob = new Job(Constants.JobWooHoo, pawn, bed);
                        mate.jobs.StartJob(newJob, JobCondition.InterruptForced, null, false, true, null, null, false);
                    }

                },
                socialMode = RandomSocialMode.Off,
                defaultCompleteMode = ToilCompleteMode.Delay,
                defaultDuration = 2
            };

            yield break;
        }
        public static IEnumerable<Toil> animateLovin(Pawn pawn, Pawn mate, Building_Bed bed)
        {
            yield return Toils_Bed.GotoBed(TargetIndex.B);

            var laydown =  Toils_LayDown.LayDown(TargetIndex.B, true, false, false, false);

            laydown.AddPreTickAction(delegate ()
            {
                if (pawn.IsHashIntervalTick(100))
                {
                    //Log.Message("Making Noises");
                    MoteMaker.ThrowMetaIcon(pawn.Position, pawn.Map, ThingDefOf.Mote_Heart);

                }
            });

            laydown.defaultCompleteMode = ToilCompleteMode.Delay;
            laydown.defaultDuration = 250;

            yield return laydown;
            yield break;
        }

        public static bool IsThisJailLovin(Pawn pawn, Pawn mate, Building_Bed bed)
        {


            return (pawn != null && pawn.guest != null && pawn.guest.IsPrisoner)
            || (mate != null && mate.guest != null && mate.guest.IsPrisoner)
            || (bed != null && bed.ForPrisoners);


        }
    }
}
