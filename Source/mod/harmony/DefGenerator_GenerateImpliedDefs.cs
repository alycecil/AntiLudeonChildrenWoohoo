using System.Linq;
using Harmony;
using RimWorld;
using Verse;

namespace DarkIntentionsWoohoo.harmony
{
    [HarmonyPatch(typeof(DefGenerator), "GenerateImpliedDefs_PreResolve")]
    public static class DefGenerator_GenerateImpliedDefs_PreResolve
    {
        [HarmonyPostfix]
        public static void Postfix()
        {
            RecipeDef installBionicWomb = DefDatabase<RecipeDef>.GetNamed("InstallBionicWomb");
            
            var fleshRaces = DefDatabase<ThingDef>
                .AllDefsListForReading
                .Where(t => t.race?.IsFlesh ?? false); 

            var humanoidRaces = fleshRaces.Where(td => td.race.Humanlike);

            var fleshBodies = humanoidRaces
                .Select(t => t.race.body)
                .Distinct();

            
            foreach (var humanoidRace in humanoidRaces)
            {
                humanoidRace.recipes.Add(installBionicWomb);
            }
        }
    }
}