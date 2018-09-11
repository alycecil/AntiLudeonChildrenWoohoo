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
    class WorkGiver_Warden_KindChat : WorkGiver_Warden_Chat
    {
        public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            if(t == null || pawn == null )
            {
                return null;
            }
            else if (!base.ShouldTakeCareOfPrisoner(pawn, t))
            {
                return null;
            }

            Pawn pawn2 = (Pawn)t;

            if (pawn2.guest.interactionMode == PrisonerInteractionModeDefOf.ReduceResistance
                && !DarkIntentions.is_kind(pawn))
            {
                return null;
            }
        
            return base.JobOnThing(pawn, t, forced);
        }
    }
}