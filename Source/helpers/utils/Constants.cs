using DarkIntentionsWoohoo.mod.settings;
using RimWorld;
using Verse;

namespace DarkIntentionsWoohoo
{
    static class Constants
    {
        public static float MINIMUM_REPO = 0.01f;

        public static readonly JobDef JobWooHoo = DefDatabase<JobDef>.GetNamed("WooHoo");
        public static readonly JobDef JobWooHoo_Baby = DefDatabase<JobDef>.GetNamed("WooHoo_Baby");
        public static readonly JobDef JobWooHooRecieve = DefDatabase<JobDef>.GetNamed("WooHoo_Get");

        public static readonly HediffDef BionicWomb = HediffDef.Named("BionicWomb");
        public static readonly HediffDef GivingBirth = HediffDef.Named("GivingBirth");

        public static readonly PawnCapacityDef Fertility = DefDatabase<PawnCapacityDef>.GetNamedSilentFail("Fertility");

        public static readonly PawnCapacityDef Reproduction =
            DefDatabase<PawnCapacityDef>.GetNamedSilentFail("Reproduction");

        public static readonly JoyKindDef Joy_Woohoo = DefDatabase<JoyKindDef>.GetNamed("Woohoo");
    }
}