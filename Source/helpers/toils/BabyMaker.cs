using RimWorld;
using Verse;
using Verse.AI;

namespace DarkIntentionsWoohoo
{
    class BabyMaker
    {
        public static Toil DoMakeBaby(Pawn pawn, Pawn TargetA)
        {
            return new Toil
            {
                initAction = delegate
                {
                    /* Log.Message("Baby Making"); */
                    Pawn mate = TargetA;


                    //check fertility then ensemenate wombs
                    if (!FertilityChecker.is_fertile(pawn)) return;
                    if (!FertilityChecker.is_fertile(mate)) return;
                    //for each womb make pregnant
                    if (FertilityChecker.is_FemaleForBabies(pawn))
                    {
                        /* Log.Message("Getting innitialer pregnant", false); */
                        //(donor , has womb)
                        Mate.Mated(mate, pawn);
                        pawn.records.Increment(Constants.TimesWooHooedGotPregnant);
                    }

                    if (FertilityChecker.is_FemaleForBabies(mate))
                    {
                        /* Log.Message("Getting talkee pregnant", false); */
                        //(donor , has womb)
                        Mate.Mated(pawn, mate);
                        mate.records.Increment(Constants.TimesWooHooedGotPregnant);
                    }
                },
                socialMode = RandomSocialMode.Off,
                defaultCompleteMode = ToilCompleteMode.Instant
            };
        }
    }
}