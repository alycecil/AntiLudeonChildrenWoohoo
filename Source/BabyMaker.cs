using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

                    Pawn mate = (Pawn)TargetA;


                    //check fertility then ensemenate wombs
                    if (!Constants.is_fertile(pawn))
                    {
                        Log.Message("Woohoo for baby, but youre not fertile", false);
                    }
                    else if (!Constants.is_fertile(mate))
                    {
                        Log.Message("Woohoo for baby, but not fertile mate", false);
                    }
                    else
                    {
                        //for each womb make pregnant
                        if (Constants.is_FemaleForBabies(pawn))
                        {
                            Log.Message("Getting innitialer pregnant", false);
                            //(donor , has womb)
                            Mate.Mated(mate, pawn);
                        }
                        else
                        {
                            Log.Message("Initiator lacks womb", false);
                        }

                        if (Constants.is_FemaleForBabies(mate))
                        {
                            Log.Message("Getting talkee pregnant", false);
                            //(donor , has womb)
                            Mate.Mated(pawn, mate);
                        }
                        else
                        {
                            Log.Message("talkee lacks womb", false);
                        }
                    }
                },
                socialMode = RandomSocialMode.Off,
                defaultCompleteMode = ToilCompleteMode.Delay,
                defaultDuration = 350
            };
        }
    }
}
