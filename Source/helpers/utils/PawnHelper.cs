using DarkIntentionsWoohoo.mod.settings;
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
            return pawn.RaceProps.Humanlike;
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
            return (pawn?.story?.traits != null && pawn.story.traits.HasTrait(TraitDefOf.Bloodlust));
        }

        public static bool is_brawler(Pawn pawn)
        {
            return (pawn != null && pawn.story != null && pawn.story.traits != null &&
                    pawn.story.traits.HasTrait(TraitDefOf.Brawler));
        }

        public static bool is_kind(Pawn pawn)
        {
            return (pawn?.story != null && pawn.story.traits != null && pawn.story.traits.HasTrait(TraitDefOf.Kind));
        }

        public static bool IsNotWoohooing(Pawn mate)
        {
            if (mate.CurJob == null) return true;
            
            
            bool b = (
                         mate.CurJob.def != JobDefOf.Lovin
                         && mate.CurJob.def != Constants.JobWooHoo
                         && mate.CurJob.def != Constants.JobWooHoo_Baby
                         && mate.CurJob.def != Constants.JobWooHooRecieve
                     );

            //Trace:
          //  if(!b)
         /* Log.Message("[" + mate.Name + "] : Woohooing Job:" +mate.CurJob.def); */
            return b;
        }

        public static bool isStranger(Pawn pawn, Pawn mate)
        {
            return (pawn.guest==null && mate.guest !=null);
        }

        public static void DelayNextWooHoo(Pawn pawn)
        {
            pawn.mindState.canLovinTick = Find.TickManager.TicksGame + + Rand.Range((int)(WoohooSettingHelper.latest.minAITicks*0.9f), (int)(WoohooSettingHelper.latest.minAITicks * 1.1f) );
        }
    }
}