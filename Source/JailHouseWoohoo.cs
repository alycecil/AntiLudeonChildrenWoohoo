using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;

namespace DarkIntentionsWoohoo
{
    class JailHouseWoohoo
    {
        public static IEnumerable<Toil> jailLovin(Pawn pawn, Pawn mate, Building_Bed bed)
        {
            //TODO
            yield break;
        }

        public static bool IsThisJailLovin(Pawn pawn, Pawn mate, Building_Bed bed)
        {
            

            return (pawn != null && pawn.guest != null && pawn.guest.IsPrisoner)
            || (mate != null && mate.guest != null && mate.guest.IsPrisoner)
            || (bed != null && bed.ForPrisoners);


        }
    }
}
