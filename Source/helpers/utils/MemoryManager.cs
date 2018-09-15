using RimWorld;
using Verse;
using Verse.AI;

namespace DarkIntentionsWoohoo
{
    class MemoryManager
    {
        public readonly static ThoughtDef PrisonerWoohoo = DefDatabase<ThoughtDef>.GetNamed("PrisonerWoohoo");

        public readonly static ThoughtDef MasochistPrisonerWoohoo =
            DefDatabase<ThoughtDef>.GetNamed("MasochistPrisonerWoohoo");

        public readonly static ThoughtDef PrisonerWoohooMemory =
            DefDatabase<ThoughtDef>.GetNamed("PrisonerWoohooMemory");

        public readonly static ThoughtDef MasochistPrisonerWoohooMemory =
            DefDatabase<ThoughtDef>.GetNamed("MasochistPrisonerWoohooMemory");

        public readonly static ThoughtDef WoohooColonist = DefDatabase<ThoughtDef>.GetNamed("WoohooColonist");

        public readonly static ThoughtDef WoohooColonistRegret =
            DefDatabase<ThoughtDef>.GetNamed("WoohooColonistRegret");

        public readonly static ThoughtDef WoohooNeutral = DefDatabase<ThoughtDef>.GetNamed("WoohooNeutral");
        public readonly static ThoughtDef WoohooKink = DefDatabase<ThoughtDef>.GetNamed("WoohooKink");
        public readonly static ThoughtDef WoohooKinkMemory = DefDatabase<ThoughtDef>.GetNamed("WoohooKinkMemory");

        public static Toil addMoodletsToil(Pawn pawn, Pawn mate)
        {
            return new Toil
            {
                initAction = delegate() { addMoodlets(pawn, mate); },
                defaultCompleteMode = ToilCompleteMode.Instant
            };
        }

        public static void addMoodlets(Pawn pawn, Pawn mate)
        {
            Log.Message("Adding Moodlets");
            if (mate.guest != null && mate.guest.IsPrisoner)
            {
                addPrisonMoodlets(pawn, mate);
            }
            else if (mate.guest != null && mate.guest.IsPrisoner)
            {
                addPrisonMoodlets(pawn, mate);
            }
            else
            {
                addEqualsMoodlets(pawn, mate);
            }
        }

        private static void addEqualsMoodlets(Pawn pawn, Pawn mate)
        {
            if (isKinky(pawn) && isKinky(mate))
            {
                addMemory(mate, WoohooKink);
                addMemoryOfOther(mate, WoohooKinkMemory, pawn);
                addMemory(pawn, WoohooKink);
                addMemoryOfOther(pawn, WoohooKinkMemory, mate);
            }
            else
            {
                addMemory(mate, WoohooColonist);
                addMemoryOfOther(mate, ThoughtDefOf.GotSomeLovin, pawn);
                addMemory(pawn, WoohooColonist);
                addMemoryOfOther(pawn, ThoughtDefOf.GotSomeLovin, mate);
            }
        }

        private static bool isKinky(Pawn pawn)
        {
            return PawnHelper.is_bloodlust(pawn) || PawnHelper.is_psychopath(pawn) || PawnHelper.is_masochist(pawn);
        }

        private static void addPrisonMoodlets(Pawn torturer, Pawn victim)
        {
            if (PawnHelper.is_bloodlust(torturer) || PawnHelper.is_psychopath(torturer))
            {
                addMemory(torturer, WoohooColonist);
            }
            else if (PawnHelper.is_kind(torturer))
            {
                addMemory(torturer, WoohooColonistRegret);
            }
            else
            {
                addMemory(torturer, WoohooNeutral);
            }


            if (PawnHelper.is_masochist(victim))
            {
                addMemory(victim, MasochistPrisonerWoohoo);
                addMemoryOfOther(victim, MasochistPrisonerWoohooMemory, torturer);
            }
            else
            {
                addMemory(victim, PrisonerWoohoo);
                if (PawnHelper.is_psychopath(victim) || PawnHelper.is_bloodlust(victim))
                {
                    addMemoryOfOther(victim, WoohooNeutral, torturer);
                }
                else
                {
                    addMemoryOfOther(victim, PrisonerWoohooMemory, torturer);
                }
            }
        }

        public static void addMemory(Pawn p, ThoughtDef thoughtDef)
        {
            p.needs.mood.thoughts.memories.TryGainMemory(thoughtDef);
        }

        public static void addMemoryOfOther(Pawn p, ThoughtDef thoughtDef, Pawn other)
        {
            p.needs.mood.thoughts.memories.TryGainMemory(thoughtDef, other);
        }
    }
}