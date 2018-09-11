using RimWorld;
using Verse;

namespace DarkIntentions
{
    public class DarkIntentions
    {
        public readonly static JobDef AbuseJob = DefDatabase<JobDef>.GetNamed("PrisonerTorture");

        public readonly static ThoughtDef GotAbused = DefDatabase<ThoughtDef>.GetNamed("GotAbused");
        public readonly static ThoughtDef MasochistGotAbused = DefDatabase<ThoughtDef>.GetNamed("MasochistGotAbused");
        public readonly static ThoughtDef HateMyAbuser = DefDatabase<ThoughtDef>.GetNamed("HateMyAbuser");
        public readonly static ThoughtDef KindaLikeMyAbuser = DefDatabase<ThoughtDef>.GetNamed("KindaLikeMyAbuser");
        public readonly static ThoughtDef AllowedMeToGetAbused = DefDatabase<ThoughtDef>.GetNamed("AllowedMeToGetAbused");
        public readonly static ThoughtDef StoleSomeAbuse = DefDatabase<ThoughtDef>.GetNamed("StoleSomeAbuse");


        public readonly static HediffDef feelingBroken = HediffDef.Named("FeelingBroken");
        /**
        public readonly static ThoughtDef WatchedSomeAbuse = DefDatabase<ThoughtDef>.GetNamed("WatchedSomeAbuse");
        public readonly static ThoughtDef WatchedSomeAbuseKind = DefDatabase<ThoughtDef>.GetNamed("WatchedSomeAbuseKind");
        **/

        public readonly static ThoughtDef BloodlustStoleSomeAbuse = DefDatabase<ThoughtDef>.GetNamed("BloodlustStoleSomeAbuse");

        public readonly static InteractionDef BuildTortureRapport = DefDatabase<InteractionDef>.GetNamed("BuildTortureRapport");


        public static void addTortureMoodlets(Pawn torturer, Pawn victim)
        {
            if (is_bloodlust(torturer) || is_psychopath(torturer))
            {
                addMemory(torturer, BloodlustStoleSomeAbuse);
            }
            else
            {
                addMemory(torturer, StoleSomeAbuse);
            }

            if (is_masochist(victim))
            {
                addMemory(victim, MasochistGotAbused);
                addMemoryOfOther(victim, KindaLikeMyAbuser, torturer);
            }
            else
            {
                addMemory(victim, GotAbused);
                if (is_kind(victim))
                {
                    addMemoryOfOther(victim, AllowedMeToGetAbused, torturer);
                } else
                {
                    addMemoryOfOther(victim, HateMyAbuser, torturer);
                }
            }

            /* gets slow when i use automation, need to ignore this until ready to look at how rooms work
            if (victim.Faction != null && victim.Map != null) //wild animals faction is null. should check.
            {
                foreach (var bystander in victim.Map.mapPawns.SpawnedPawnsInFaction(victim.Faction))
                {
                    if ((bystander != torturer) && (bystander != victim) 
                        && is_human(bystander) && !is_animal(bystander) && !is_masochist(victim))
                    {
                        victim.needs.mood.thoughts.memories.TryGainMemory(AllowedMeToGetAbused, bystander);
                    }
                }
            }*/


        }

        public static void addMemory(Pawn p, ThoughtDef thoughtDef)
        {
            p.needs.mood.thoughts.memories.TryGainMemory(thoughtDef);
        }

        public static void addMemoryOfOther(Pawn p, ThoughtDef thoughtDef, Pawn other)
        {
            p.needs.mood.thoughts.memories.TryGainMemory(thoughtDef, other);
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