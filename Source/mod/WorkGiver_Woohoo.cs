using RimWorld;
using Verse;
using Verse.AI;

namespace DarkIntentionsWoohoo
{
    class WorkGiver_Woohoo : WorkGiver_Scanner
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
                && pawn != pawn2
                && (forced || canAutoLove(pawn, pawn2))
                && !pawn2.Downed
                && (pawn2.Faction == pawn.Faction || pawn2.guest != null && !pawn2.Drafted)
                && PawnHelper.is_human(pawn)
                && PawnHelper.is_human(pawn2)
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

        private bool canAutoLove(Pawn pawn, Pawn pawn2)
        {
            var tick = Find.TickManager.TicksGame;
            return pawn.mindState.canLovinTick < tick
                   && pawn2.mindState.canLovinTick < tick
                   //idle
                   && JobUtilityIdle.isIdle(pawn2)

                   //still enjoys woohoo
                   && pawn2?.needs?.joy?.tolerances != null
                   && pawn?.needs?.joy?.tolerances != null
                   && pawn2?.needs?.mood?.CurLevel != null
                   && pawn?.needs?.mood?.CurLevel != null

                   && !pawn2.needs.joy.tolerances.BoredOf(Constants.Joy_Woohoo)
                   && !pawn.needs.joy.tolerances.BoredOf(Constants.Joy_Woohoo)
                   && ((pawn2.needs.joy.CurLevel < .6f  || pawn2.needs.mood.CurLevel < .6f)
                       && (pawn.needs.joy.CurLevel < .6f || pawn.needs.mood.CurLevel < .6f)
                   )

                   //and a 1d2
                   && Rand.Value < 0.1f;
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
            return Rand.Value < fert;
        }
    }
}