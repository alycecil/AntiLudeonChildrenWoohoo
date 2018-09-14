using System;
using RimWorld;
using Verse;

namespace DarkIntentionsWoohoo
{
    class Constants
    {
        
        private static float MINIMUM_REPO = 0.01f;

        public readonly static JobDef JobWooHoo = DefDatabase<JobDef>.GetNamed("WooHoo");
        public readonly static JobDef JobWooHoo_Baby = DefDatabase<JobDef>.GetNamed("WooHoo_Baby");
        

        public readonly static HediffDef BionicWomb = HediffDef.Named("BionicWomb");

        public static readonly PawnCapacityDef Fertility = DefDatabase<PawnCapacityDef>.GetNamedSilentFail("Fertility");
        public static readonly PawnCapacityDef Reproduction = DefDatabase<PawnCapacityDef>.GetNamedSilentFail("Reproduction");

        public static bool is_fertile(Pawn pawn)
        {
            return getFetility(pawn) > MINIMUM_REPO;
        }

        public static float getFetility(Pawn pawn)
        {
            float val;
            if (alreadyPregnant(pawn))
            {
                return 0f;
            }
            else if (hasBionicWomb(pawn))
            {
                return 1f;
            }
            else if (Fertility != null && (val = pawn.health.capacities.GetLevel(Fertility)) >= 0)
            {
                return val;

            }
            else if (Reproduction != null && (val = pawn.health.capacities.GetLevel(Reproduction)) >= 0)
            {
                return val;
            }
            return pawn.ageTracker.CurLifeStage.reproductive ? 1f:0f;
        }

        private static bool alreadyPregnant(Pawn pawn)
        {
            return pawn.health.hediffSet.HasHediff(HediffDefOf.Pregnant);
        }

        public static bool is_FemaleForBabies(Pawn pawn)
        {
            return pawn.gender == Gender.Female || hasBionicWomb(pawn);
        }

        public static bool hasBionicWomb(Pawn pawn)
        {
            //return pawn.health.hediffSet.HasHediff(BionicWomb);
            return false;
        }


        public static bool is_animal(Pawn pawn)
        {
            return pawn.RaceProps.Animal;
        }

        public static bool is_human(Pawn pawn)
        {
            return pawn.RaceProps.Humanlike;//||pawn.kindDef.race == ThingDefOf.Human
        }

        public static bool is_masochist(Pawn pawn)
        {
            return (pawn != null && pawn.story != null && pawn.story.traits != null && pawn.story.traits.HasTrait(TraitDef.Named("Masochist")));
        }

        public static bool is_psychopath(Pawn pawn)
        {
            return (pawn != null && pawn.story != null && pawn.story.traits != null && pawn.story.traits.HasTrait(TraitDefOf.Psychopath));
        }

        public static bool is_bloodlust(Pawn pawn)
        {
            return (pawn != null && pawn.story != null && pawn.story.traits != null && pawn.story.traits.HasTrait(TraitDefOf.Bloodlust));
        }

        public static bool is_brawler(Pawn pawn)
        {
            return (pawn != null && pawn.story != null && pawn.story.traits != null && pawn.story.traits.HasTrait(TraitDefOf.Brawler));
        }

        public static bool is_kind(Pawn pawn)
        {
            return (pawn != null && pawn.story != null && pawn.story.traits != null && pawn.story.traits.HasTrait(TraitDefOf.Kind));
        }
    }
}
