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
            get
            {
                return ThingRequest.ForGroup(ThingRequestGroup.Pawn);
            }
        }
        
        public override PathEndMode PathEndMode
        {
            get
            {
                return PathEndMode.ClosestTouch; 
            }
        }

        // Token: 0x0600073A RID: 1850 RVA: 0x00041350 File Offset: 0x0003F750
        public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            if (t == null || pawn == null) return false;
            Pawn pawn2 = t as Pawn;
            if (pawn2 != null 
                && !pawn2.Downed 
                && ( pawn2.Faction == pawn.Faction || (pawn2.guest!=null && pawn2.guest.IsPrisoner) )
                && pawn != pawn2 
                && Constants.is_human(pawn)
                && Constants.is_human(pawn2)
                && forced)
            {
                LocalTargetInfo target = pawn2;
                if (pawn.CanReserve(target, 1, -1, null, forced))
                {
                    bed = BetterBedFinder.DoBetterBedFinder(pawn, pawn2);
                    return bed != null;
                }
            }
            return false;
        }

        public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            if (t == null || pawn == null) return null;

            Pawn pawn2 = t as Pawn;

            if (!Constants.is_human(pawn)
                || !Constants.is_human(pawn2)) return null;

            Log.Message("Woohoo Started Correctly");

            if (IsMate(pawn, pawn2))
            {

                return new Job(Constants.JobWooHoo_Baby, pawn2, bed)
                {
                    count = 1
                };
            }
            else {
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
            float fert = Constants.getFetility(pawn) + Constants.getFetility(pawn2) / 2.0f;
            fert *= MateChance();
            //TODO dice roll
            return false;
        }
    }
}
