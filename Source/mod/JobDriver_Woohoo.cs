using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;
using Verse.AI;

namespace DarkIntentionsWoohoo
{
    class JobDriver_Woohoo : JobDriver
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
                && PawnHelper.is_human(pawn)
                && PawnHelper.is_human(mate)
                && !bed.IsBurning()
            )
            {
                //everything is in order then
            }
            else
            {
                Log.Error("[" + pawn.Name + "] can't woohoo right.", true);
                EndJobWith(JobCondition.Errored);
                return null;
            }

            if (pawn == mate)
            {
                throw new Exception("You cant WooHoo Alone and Together with yourself");
            }
            
            var hookupBedManager = new HookupBedManager(bed);

            bool partnerSaidYes;
            IEnumerable<Toil> toilsAskForWoohoo;
            if (PawnHelper.IsNotWoohooing(mate))
            {
                pawn.records.Increment(Constants.CountAskedForWoohoo);
                mate.records.Increment(Constants.CountGotAskedToWooHoo);
                
                partnerSaidYes = AskPartner(pawn, mate);
                
                toilsAskForWoohoo = WoohooManager.ToilsAskForWoohoo(pawn, mate, bed, partnerSaidYes, hookupBedManager);
            }
            else
            {
             /* Log.Message("Partner already woohooin, dont need to ask."); */
                partnerSaidYes = true;
                toilsAskForWoohoo = nothing();
            }

            ///Log.Message("[WooHoo]Toils Ongoing: ["+r.Count()+"]");
            if (partnerSaidYes)
            {
                toilsAskForWoohoo = toilsAskForWoohoo?.Union(WoohooManager.MakePartnerWoohoo(pawn, mate, bed))
                    .Union(WoohooManager.AnimateLovin(pawn, mate, bed))
                    .Union(MakeMyLoveToils(pawn, mate))
                    .Union(
                        WoohooManager.AnimateLovin(pawn, mate, bed,
                            null
                            , 500)
                    );
            }
            else
            {
                mate.records.Increment(Constants.CountGotAskedToWooHooSaidNo);
            }

            toilsAskForWoohoo = toilsAskForWoohoo?.Union(hookupBedManager.GiveBackToil());

       /* Log.Message("[WooHoo]Toils: ["+r.Count()+"]"); */
            PawnHelper.DelayNextWooHoo(pawn);
            
            return toilsAskForWoohoo;
        }

        private IEnumerable<Toil> nothing()
        {
            yield break;
        }

        private bool AskPartner(Pawn pawn, Pawn mate)
        {
            return pawn != null && mate != null && (JailHelper.IsThisJailLovin(pawn, mate, null) || !PawnHelper.isStranger(pawn, mate) || Rand.Bool);
        }

        public IEnumerable<Toil> MakeMyLoveToils(Pawn pawn, Pawn mate)
        {
            
            if (!PawnHelper.is_psychopath(pawn) && PawnHelper.isStranger(pawn, mate) && !JailHelper.IsThisJailLovin(pawn, mate))
            {
           /* Log.Message("Lets try and recruit with woohoo as this guest might like you that much"); */
                Toils_Interpersonal.TryRecruit(TargetIndex.A);
            }
            
            /* Log.Message("Appending Moods"); */
            yield return MemoryManager.addMoodletsToil(pawn, mate);
            if (isMakeBaby())
            {
                /* Log.Message("Apppending Baby"); */
                yield return BabyMaker.DoMakeBaby(pawn, mate);
            }

        }

        public override bool CanBeginNowWhileLyingDown()
        {
            return (JobInBedUtility.InBedOrRestSpotNow(pawn, TargetB) 
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

            if (TargetA != null && TargetA.Thing != null && (mate = TargetA.Thing as Pawn) != null
                && TargetB != null && TargetB.Thing != null && (bed = TargetB.Thing as Building_Bed) != null
                && pawn != null
                && PawnHelper.is_human(pawn)
                && PawnHelper.is_human(mate)
                && !bed.IsBurning()
                && pawn.mindState.canLovinTick < Find.TickManager.TicksGame 
            )
            {
                return true; //base.TryMakePreToilReservations(errorOnFailed);
            }

            Log.Message("[" + pawn.Name +
                            "] can't woohoo right. Timing out their lovin for 500 ticks. They tried to some weird stuff:" +
                            this.GetReport(), true);  
            pawn.mindState.canLovinTick = Find.TickManager.TicksGame + 500;

            return false;
        }
    }
}