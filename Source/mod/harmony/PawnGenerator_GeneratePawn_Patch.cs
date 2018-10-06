using System;
using System.Linq;
using Harmony;
using RimWorld;
using Verse;

namespace DarkIntentionsWoohoo.harmony
{
    [HarmonyPatch(typeof(PawnGenerator), "GeneratePawn", typeof(PawnGenerationRequest))]
        public static class PawnGenerator_GeneratePawn_Patch
        {
            [HarmonyPostfix]
            public static void Postfix(ref Pawn __result, PawnGenerationRequest request)
            {
                //Amnesty for children
                if(__result?.guest != null && __result.guest.IsPrisoner && __result.guest.HostFaction == Faction.OfPlayer) __result.guest.SetGuestStatus(null);
            }
        }
}