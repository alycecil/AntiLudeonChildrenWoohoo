using System.Linq;
using Harmony;
using RimWorld;
using Verse;

namespace DarkIntentionsWoohoo.harmony
{
    class wrapps
    {
        [HarmonyPatch(typeof(Hediff_Pregnant), "DoBirthSpawn", null)]
        public static class Hediff_Pregnant_DoBirthSpawn_Patch
        {
            [HarmonyPrefix]
            public static bool Prefix(Pawn mother, Pawn father)
            {
                if (mother != null && PawnHelper.is_human(mother))
                {
                    bool motherInJail = mother.guest != null && mother.guest.IsPrisoner &&
                                        mother.Faction != Faction.OfPlayer;
                    bool amnestyOnMother = motherInJail && father != null && PawnHelper.is_human(father) &&
                                           PawnHelper.is_kind(father);


                    if (amnestyOnMother)
                    {
                        mother.SetFaction(Faction.OfPlayer, father);

                        //TODO message mother given amnesty
                        TaleRecorder.RecordTale(TaleDefOf.Recruited, new object[]
                        {
                            father, // recruiter
                            mother //recruitee

                        });
                        father.records.Increment(RecordDefOf.PrisonersRecruited);
                        mother.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.RecruitedMe, father);

                    }
                    else if (motherInJail)
                    {
                        //will need to make babies belonging to us

                        HeDiffPrisonerGivingBirth factionDiff =
                            (HeDiffPrisonerGivingBirth) HediffMaker.MakeHediff(Constants.GivingBirth, mother, null);
                        factionDiff.Faction = mother.Faction;

                        mother.health.AddHediff(factionDiff);
                        mother.SetFactionDirect(Faction.OfPlayer);
                    }
                }

                return true;

            }


            [HarmonyPostfix]
            public static void Postfix(Pawn mother, Pawn father)
            {
                if (mother == null || !PawnHelper.is_human(mother)) return;
                var realFactionHeDiff =
                    mother.health.hediffSet.GetHediffs<HeDiffPrisonerGivingBirth>().Where(x => true); //lz clone

                if (realFactionHeDiff == null || !realFactionHeDiff.Any()) return;
                foreach (HeDiffPrisonerGivingBirth heDiff in realFactionHeDiff)
                {
                    if (heDiff.Faction != null)
                    {
                        mother.SetFactionDirect(heDiff.Faction);
                    }

                    mother.health.RemoveHediff(heDiff);
                }
            }

        }
    }

}