using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using Verse.AI;
using Verse.Sound;


namespace DarkIntentions.torture
{
    class WorkGiver_Warden_Torture : WorkGiver_Warden
    {
        // Token: 0x06000604 RID: 1540 RVA: 0x00036D44 File Offset: 0x00035144
        public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            //todo decide if not wardedening
            if (!base.ShouldTakeCareOfPrisoner(pawn, t) || DarkIntentions.is_kind(pawn))
            {
                return null;
            }
            Pawn pawn2 = (Pawn)t;
            
            if (pawn2.guest.interactionMode == PrisonerInteractionModeDefOf.ReduceResistance
                && pawn2.guest.ScheduledForInteraction 
                && pawn.health.capacities.CapableOf(PawnCapacityDefOf.Talking) 
                && (!pawn2.Downed) 
                && pawn.CanReserve(t, 1, -1, null, false) 
                && pawn2.Awake())
            {
                return new Job(DarkIntentions.AbuseJob, t);
              
            }
            return null;
        }
    }
}