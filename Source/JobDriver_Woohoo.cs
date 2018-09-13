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
            Log.Message("Woohoo for baby : Started", false);

            IEnumerable<Toil> r = base.MakeNewToils();

            if (r != null && r.Any())
            {
                r = r.Union(MakeMyLoveToils());

            }
            else
            {
                Log.Message("Woohoo Baby Skipped", false);
            }
            return r;
        }


        public void Mated(Pawn donor, Pawn hasWomb)
        {
           
            PawnUtility.Mated(donor, hasWomb);

        }


        public IEnumerable<Toil> MakeMyLoveToils()
        {

            if (isMakeBaby()) {
                    Pawn mate = (Pawn)TargetA;


                    //check fertility then ensemenate wombs
                    if (!Constants.is_fertile(pawn))
                    {
                        Log.Message("Woohoo for baby not fertile, but youre not fertile", false);
                    }
                    else if (!Constants.is_fertile(mate))
                    {
                        Log.Message("Woohoo for baby not fertile mate", false);
                    }
                    else
                    {
                        //for each female make pregnant
                        //TODO artifical womb for men
                        if (Constants.is_FemaleForBabies(pawn))
                        {
                            Log.Message("Getting innitialer pregnant", false);
                            //(donor , has womb)
                            Mated(mate, this.pawn);
                        }
                        else
                        {
                            Log.Message("Initiator lacks womb", false);
                        }

                        if (Constants.is_FemaleForBabies(mate))
                        {
                            Log.Message("Getting talkee pregnant", false);
                            //(donor , has womb)
                            Mated(this.pawn, mate);
                        }
                        else
                        {
                            Log.Message("talkee lacks womb", false);
                        }
                    }
            }
            yield break;
        }

        public virtual bool isMakeBaby()
        {
            Log.Message("Just love", false);
            //TODO roll dice
            return false;
        }
    }
}
