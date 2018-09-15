using RimWorld;
using Verse;

namespace DarkIntentionsWoohoo
{
    static class PawnHelper
    {
        public static bool is_animal(Pawn pawn)
        {
            return pawn.RaceProps.Animal;
        }

        public static bool is_human(Pawn pawn)
        {
            return pawn.RaceProps.Humanlike; //||pawn.kindDef.race == ThingDefOf.Human
        }

        public static bool is_masochist(Pawn pawn)
        {
            return (pawn != null && pawn.story != null && pawn.story.traits != null &&
                    pawn.story.traits.HasTrait(TraitDef.Named("Masochist")));
        }

        public static bool is_psychopath(Pawn pawn)
        {
            return (pawn != null && pawn.story != null && pawn.story.traits != null &&
                    pawn.story.traits.HasTrait(TraitDefOf.Psychopath));
        }

        public static bool is_bloodlust(Pawn pawn)
        {
            return (pawn != null && pawn.story != null && pawn.story.traits != null &&
                    pawn.story.traits.HasTrait(TraitDefOf.Bloodlust));
        }

        public static bool is_brawler(Pawn pawn)
        {
            return (pawn != null && pawn.story != null && pawn.story.traits != null &&
                    pawn.story.traits.HasTrait(TraitDefOf.Brawler));
        }

        public static bool is_kind(Pawn pawn)
        {
            return (pawn != null && pawn.story != null && pawn.story.traits != null &&
                    pawn.story.traits.HasTrait(TraitDefOf.Kind));
        }

        public static bool IsNotWoohooing(Pawn mate)
        {
            bool b = mate.CurJob == null || (
                         mate.CurJob.def != JobDefOf.Lovin
                         && mate.CurJob.def != Constants.JobWooHoo
                         && mate.CurJob.def != Constants.JobWooHoo_Baby
                         && mate.CurJob.def != Constants.JobWooHooRecieve
                     );

            //Trace:Log.Message("[" + mate.Name + "] : Woohooing?" + !b);
            return b;
        }
    }
}