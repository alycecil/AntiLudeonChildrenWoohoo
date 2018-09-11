using RimWorld;
using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;


namespace DarkIntentions.torture
{
    class JobDriver_Torture : JobDriver
    {

        

        protected Pawn Talkee
        {
            get
            {
                return (Pawn)this.job.targetA.Thing;
            }
        }

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null);
        }
        
        protected override IEnumerable<Toil> MakeNewToils()
        {
            this.FailOnDespawnedOrNull(TargetIndex.A);
            this.FailOnMentalState(TargetIndex.A);
            this.FailOnNotAwake(TargetIndex.A);

            for (int i = 0; i < 4; i++)
            {
                yield return Toils_Interpersonal.GotoPrisoner(this.pawn, this.Talkee, this.Talkee.guest.interactionMode);
                yield return Toils_Interpersonal.WaitToBeAbleToInteract(this.pawn);
                yield return Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
                yield return TortureRecruitee(this.pawn, this.Talkee);
            }

            yield return MarkTortured(this.pawn, this.Talkee);
            

            yield return Toils_Interpersonal.SetLastInteractTime(TargetIndex.A);

            yield break;
        }

        public static void handleBroken(Pawn pawn, Pawn Talkee)
        {

            if (!Talkee.health.hediffSet.HasHediff(DarkIntentions.feelingBroken))
            {
                BodyPartRecord torso =Talkee.RaceProps.body.AllParts.Find((bpr) => String.Equals(bpr.def.defName, "Torso"));
                Talkee.health.AddHediff(DarkIntentions.feelingBroken, torso);
                Talkee.health.hediffSet.GetFirstHediffOfDef(DarkIntentions.feelingBroken).Severity = 0.01f;
            }

            float ouched = 0.05f;

            if (DarkIntentions.is_bloodlust(pawn) || DarkIntentions.is_psychopath(pawn))
            {
                ouched *= 1.7f;
            }


            if (DarkIntentions.is_masochist(Talkee))
            {
                ouched /= 1.3333f;
            }


            if (DarkIntentions.is_kind(pawn))
            {
                ouched /= 1.3333f;
            }

            Talkee.health.hediffSet.GetFirstHediffOfDef(DarkIntentions.feelingBroken).Severity += ouched;
            if(Talkee.guest.resistance > 1f)
            {
                ouched *= 100f;
            }

            if (Talkee.guest.resistance <=
                ouched)
            {
                Talkee.guest.resistance = 0f;
            }
            else
            {
                Talkee.guest.resistance -= ouched;
            }
        }

        public static Toil MarkTortured(Pawn pawn, Pawn talkee)
        {
            return new Toil
            {
                initAction = delegate
                {
                    if (!pawn.interactions.TryInteractWith(talkee, DarkIntentions.BuildTortureRapport))
                    {
                        pawn.jobs.curDriver.ReadyForNextToil();
                    }
                    else
                    {


                        handleBroken(pawn, talkee);
                        DarkIntentions.addTortureMoodlets(pawn, talkee);

                        pawn.records.Increment(RecordDefOf.PawnsDowned);

                    }
                },
                socialMode = RandomSocialMode.Off,
                defaultCompleteMode = ToilCompleteMode.Delay,
                defaultDuration = 350
            };
        }

        public static Toil TortureRecruitee(Pawn pawn, Pawn talkee)
        {
            return new Toil
            {
                initAction = delegate
                {
                    if (!pawn.interactions.TryInteractWith(talkee, DarkIntentions.BuildTortureRapport))
                    {
                        pawn.jobs.curDriver.ReadyForNextToil();
                    }
                    else
                    {
                        pawn.records.Increment(RecordDefOf.PrisonersChatted);
                    }
                },
                socialMode = RandomSocialMode.Off,
                defaultCompleteMode = ToilCompleteMode.Delay,
                defaultDuration = 350
            };
        }

    }
}
