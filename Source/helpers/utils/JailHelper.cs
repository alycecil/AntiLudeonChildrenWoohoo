using RimWorld;
using Verse;

namespace DarkIntentionsWoohoo
{
    public static class JailHelper
    {
        public static bool IsThisJailLovin(Pawn pawn, Pawn mate, Building_Bed bed)
        {
            return (pawn != null && pawn.guest != null && pawn.guest.IsPrisoner)
                   || (mate != null && mate.guest != null && mate.guest.IsPrisoner)
                   || (bed != null && bed.ForPrisoners);
        }
    }
}