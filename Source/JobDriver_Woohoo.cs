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
            
            Pawn mate = TargetA.Thing as Pawn;
            Building_Bed bed = TargetB.Thing as Building_Bed;


            HookupBedmanager hookupBedmanager = new HookupBedmanager(bed);



            if (mate == null || bed == null)
            {
                //Log.Error("Missing A Mate or a Bed", false);
                return null;
            }
            //Log.Message("Woohoo for baby : Started", false);
            bool partnerSaidYes = AskPartner(pawn, mate);

            IEnumerable<Toil> r = ToilerHelper.ToilsAskForWoohoo(pawn, mate, bed, partnerSaidYes, hookupBedmanager);
            
            if (partnerSaidYes)
            {
                if (JailHouseWoohoo.IsThisJailLovin(pawn, mate, bed))
                {
                    r.Union(JailHouseWoohoo.jailLovin(pawn, mate, bed));
                } else
                {
                    r = r.Union(base.MakeNewToils());
                }
                r = r.Union(MakeMyLoveToils(pawn, mate)).Union(hookupBedmanager.GiveBackToil());

                
            }


            return r;
        }

        

        private bool AskPartner(Pawn pawn, Pawn mate)
        {
            return pawn != null && mate != null;
        }

        public IEnumerable<Toil> MakeMyLoveToils(Pawn pawn, Pawn mate)
        {
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
    }
}
