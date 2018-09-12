using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;

namespace DarkIntentions.woohoo
{
    class JobDriver_Woohoo : JobDriver_Lovin
    {
        protected override IEnumerable<Toil> MakeNewToils()
        {
            Pawn mate = (Pawn)TargetA;
            bool ask = wantsMe(pawn, mate) && wantsMe(mate, pawn);

            IEnumerable<Toil> r = askForLove(ask);

            if (ask)
            {
                r.Union(base.MakeNewToils());

                if (r != null && r.Any())
                {
                    //we can add stuff to do now that weve made it to bed
                    FilthMaker.MakeFilth(pawn.Position, pawn.Map, ThingDefOf.Filth_Slime, 1);
                }
                else
                {
                    Log.Message("Woohoo skipped", false);
                }
            }
            else
            {
                Log.Message("Woohoo declined", false);
            }

            return r;
        }

        private IEnumerable<Toil> askForLove(bool ask)
        {
            this.FailOnDespawnedOrNull(TargetIndex.A);
            this.FailOnMentalState(TargetIndex.A);
            this.FailOnDespawnedOrNull(TargetIndex.B);

            Pawn mate = (Pawn)TargetA;
            
            yield return askForLoveToil(ask);

            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch)
                .FailOnThingMissingDesignation(TargetIndex.A, DesignationDefOf.Open)
                .FailOnDespawnedOrNull(TargetIndex.A);
            yield return Toils_Interpersonal.WaitToBeAbleToInteract(this.pawn);
            yield return Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);

            yield break;
        }

        private Toil askForLoveToil(bool success)
        {
            Pawn mate = (Pawn)TargetA;

            
            Toil prepare = Toils_General.WaitWith(TargetIndex.A, 500, false, false);

            prepare.tickAction = delegate ()
            {
                if (this.pawn.IsHashIntervalTick(100))
                {
                    MoteMaker.ThrowMetaIcon(this.pawn.Position, this.pawn.Map, ThingDefOf.Mote_Heart);
                }
                
                if (mate.IsHashIntervalTick(150))
                {
                    if (success)
                    {
                        MoteMaker.ThrowMetaIcon(this.pawn.Position, this.pawn.Map, ThingDefOf.Mote_Heart);
                    }
                    else
                    {
                        MoteMaker.ThrowMetaIcon(mate.Position, this.pawn.Map, ThingDefOf.Mote_MicroSparks);
                    }
                }
            };

            return prepare;
        }

        private bool wantsMe(Pawn pawn, Pawn mate)
        {
            //TODO ask
            return true;
        }
    }
}
