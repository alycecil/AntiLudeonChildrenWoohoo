
using Verse;

namespace DarkIntentions.woohoo
{
    class WorkGiver_Woohoo_Baby : WorkGiver_Woohoo
    {
        public override float MateChance()
        {
            return 0.50f;
        }

        public override bool IsMate(Pawn pawn, Pawn pawn2)
        {
            //TODO dice roll
            return true;
        }

        public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            return base.HasJobOnThing(pawn, t, forced) 
                && Constants.is_fertile(pawn)
                && Constants.is_fertile(t as Pawn)
                ;
        }
    }
}
