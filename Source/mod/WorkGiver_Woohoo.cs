using RimWorld;
using Verse;
using Verse.AI;

namespace DarkIntentionsWoohoo
{
    class WorkGiver_Woohoo : WorkGiver_TakeToBed
    {
        private Building_Bed bed;

        public override ThingRequest PotentialWorkThingRequest
        {
            get { return ThingRequest.ForGroup(ThingRequestGroup.Pawn); }
        }

        public override PathEndMode PathEndMode
        {
            get { return PathEndMode.ClosestTouch; }
        }

        public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            if (t == null || pawn == null) return false;
            if (t is Pawn pawn2
                && !pawn2.Downed
                && (pawn2.Faction == pawn.Faction || (pawn2.guest != null && pawn2.guest.IsPrisoner))
                && pawn != pawn2
                && PawnHelper.is_human(pawn)
                && PawnHelper.is_human(pawn2)
                && forced
                && PawnHelper.IsNotWoohooing(pawn)
                && PawnHelper.IsNotWoohooing(pawn2))
            {
                LocalTargetInfo target = pawn2;
                if (!pawn.CanReserve(target, 1, -1, null, forced)) return false;
                bed = BetterBedFinder.DoBetterBedFinder(pawn, pawn2);
                return bed != null;
            }

            return false;
        }

        public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            if (t == null || pawn == null) return null;

            Pawn pawn2 = t as Pawn;

            if (!PawnHelper.is_human(pawn) || !PawnHelper.is_human(pawn2)) return null;

            if (IsMate(pawn, pawn2))
            {
                return new Job(Constants.JobWooHoo_Baby, pawn2, bed)
                {
                    count = 1
                };
            }
            else
            {
                return new Job(Constants.JobWooHoo, pawn2, bed)
                {
                    count = 1
                };
            }
        }

        public virtual float MateChance()
        {
            return 0.01f;
        }

        public virtual bool IsMate(Pawn pawn, Pawn pawn2)
        {
            float fert = FertilityChecker.getFetility(pawn) + FertilityChecker.getFetility(pawn2) / 2.0f;
            fert *= MateChance();
            //TODO dice roll
            return false;
        }
    }
}