using RimWorld;
using Verse;

namespace DarkIntentionsWoohoo
{
    static class FertilityChecker
    {
        public static bool is_fertile(Pawn pawn)
        {
            return getFetility(pawn) > Constants.MINIMUM_REPO;
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
            else if (Constants.Fertility != null && (val = pawn.health.capacities.GetLevel(Constants.Fertility)) >= 0)
            {
                return val;
            }
            else if (Constants.Reproduction != null &&
                     (val = pawn.health.capacities.GetLevel(Constants.Reproduction)) >= 0)
            {
                return val;
            }

            return pawn.ageTracker.CurLifeStage.reproductive ? 1f : 0f;
        }

        public static bool alreadyPregnant(Pawn pawn)
        {
            return pawn.health.hediffSet.HasHediff(HediffDefOf.Pregnant);
        }

        public static bool is_FemaleForBabies(Pawn pawn)
        {
            return pawn.gender == Gender.Female || hasBionicWomb(pawn);
        }

        public static bool hasBionicWomb(Pawn pawn)
        {
            return pawn.health.hediffSet.HasHediff(Constants.BionicWomb);
        }
    }
}