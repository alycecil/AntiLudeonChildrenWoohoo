using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;

namespace DarkIntentions.woohoo
{
    class JobDriver_Woohoo_Baby : JobDriver_Woohoo
    {
        protected override IEnumerable<Toil> MakeNewToils()
        {
            IEnumerable<Toil> r = base.MakeNewToils();

            if(r != null && r.Any())
            {
                //we can add stuff to do now that weve made it to bed
                Toils_General.Do(delegate
                {
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
                            //(donor , has womb)
                            PawnUtility.Mated(mate, this.pawn);
                        }

                        if (Constants.is_FemaleForBabies(mate))
                        {
                            //(donor , has womb)
                            PawnUtility.Mated(this.pawn, mate);
                        }
                    }
                });

            }else
            {
                Log.Message("Woohoo Baby Skipped", false);
            }



            return r;
        }
    }
}
