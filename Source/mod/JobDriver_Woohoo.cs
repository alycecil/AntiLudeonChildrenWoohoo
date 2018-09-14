using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;

namespace DarkIntentionsWoohoo
{
    class JobDriver_Woohoo : JobDriver_Lovin
    {


        protected override IEnumerable<Toil> MakeNewToils()
        {
            
            //Pawn mate = TargetA.Thing as Pawn;
            //Building_Bed bed = TargetB.Thing as Building_Bed;

            Pawn mate;
            Building_Bed bed;

            if (TargetA != null && TargetA.Thing != null && (mate = TargetA.Thing as Pawn) != null
                && TargetB != null && TargetB.Thing != null && (bed = TargetB.Thing as Building_Bed) != null
                && pawn != null
                && Constants.is_human(pawn)
                && Constants.is_human(mate)
                && !bed.IsBurning()
                )
            {
                //everything is in order then
            }
            else
            {
                Log.Error("["+pawn.Name+"] can't woohoo right.");
                this.EndJobWith(JobCondition.Errored);
                return null;
            }

            HookupBedmanager hookupBedmanager = new HookupBedmanager(bed);

            bool partnerSaidYes;
            IEnumerable<Toil> r;
            if (mate.CurJob.def == JobDefOf.Lovin || mate.CurJob.def == Constants.JobWooHoo || mate.CurJob.def == Constants.JobWooHoo_Baby)
            {
                partnerSaidYes = true;
                r = nothing();
            }
            else {
                //Log.Message("Woohoo for baby : Started", false);
                partnerSaidYes = AskPartner(pawn, mate);
                r = ToilerHelper.ToilsAskForWoohoo(pawn, mate, bed, partnerSaidYes, hookupBedmanager);

                if (partnerSaidYes)
                {
                    r = r.Union(WoohooManager.makePartnerWoohoo(pawn, mate, bed));
                }
            }


            if (partnerSaidYes)
            {
                r = r
                   .Union(WoohooManager.animateLovin(pawn, mate, bed))
                   .Union(MakeMyLoveToils(pawn, mate))
                   .Union(hookupBedmanager.GiveBackToil());


                if(!WoohooManager.IsThisJailLovin(pawn, mate, bed))
                {
                    r = r.Union(base.MakeNewToils());
                }
            }


            return r;
        }

        private IEnumerable<Toil> nothing()
        {
            yield break;
        }

        private bool AskPartner(Pawn pawn, Pawn mate)
        {
            return pawn != null && mate != null;
        }

        public IEnumerable<Toil> MakeMyLoveToils(Pawn pawn, Pawn mate)
        {
            yield return MemoryManager.addMoodletsToil(pawn, mate);
            if (isMakeBaby()) {
                yield return BabyMaker.DoMakeBaby(pawn, mate);
            }
            yield break;
        }
        
        public override bool CanBeginNowWhileLyingDown()
        {
            return (JobInBedUtility.InBedOrRestSpotNow(this.pawn, TargetB) 
            &&  JobInBedUtility.InBedOrRestSpotNow(TargetA.Thing as Pawn, TargetB));
        }
        
        public virtual bool isMakeBaby()
        {
            //Log.Message("Just love", false);
            return false;
        }

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            Pawn mate;
            Building_Bed bed;

            if (TargetA != null && TargetA.Thing != null  && (mate = TargetA.Thing as Pawn) != null
                && TargetB != null && TargetB.Thing != null && (bed = TargetB.Thing as Building_Bed) != null
                && pawn != null
                && Constants.is_human(pawn)
                && Constants.is_human(mate)
                && ! bed.IsBurning()
                )
            {
                return base.TryMakePreToilReservations(errorOnFailed);
            }
            else
            {
                Log.Message("[" + pawn.Name + "] can't woohoo right. Timing out their lovin for 500 ticks. They tried to some weird stuff:"+this.GetReport());
                this.pawn.mindState.canLovinTick = Find.TickManager.TicksGame + 500;

                return false;
            }

            
        }
    }
}
