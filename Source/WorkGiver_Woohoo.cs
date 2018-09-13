using RimWorld;
using Verse;
using Verse.AI;

namespace DarkIntentionsWoohoo
{
    class WorkGiver_Woohoo : WorkGiver_TakeToBed
    {

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
            Pawn pawn2 = t as Pawn;
            if (pawn2 != null && !pawn2.Downed 
                && pawn2.Faction == pawn.Faction
                && pawn != pawn2 
                //&& Constants.is_human(pawn)
                ///&& Constants.is_human(pawn2)
                && forced)
            {
                LocalTargetInfo target = pawn2;
                if (pawn.CanReserve(target, 1, -1, null, forced) && !GenAI.EnemyIsNear(pawn2, 40f))
                {
                    Thing thing = base.FindBed(pawn, pawn2);
                    return thing != null && pawn2.CanReserve(thing, 1, -1, null, false);
                }
            }
            return false;
        }

        public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
        {

            Pawn pawn2 = t as Pawn;
            Building_Bed t2 = base.FindBed(pawn, pawn2);
            if(IsMate(pawn, pawn2))
            {

                return new Job(Constants.JobWooHoo_Baby, pawn2, t2)
                {
                    count = 1
                };
            }
            else {
                return new Job(Constants.JobWooHoo, pawn2, t2)
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
