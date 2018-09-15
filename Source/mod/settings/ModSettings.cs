using System.Reflection;
using Harmony;
using UnityEngine;
using Verse;

namespace DarkIntentionsWoohoo.mod.settings
{
    class WoohooMod : Mod
    {
        public WoohooMod(ModContentPack content) : base(content)
        {


            //Harmony Kickofff
            var harmony = HarmonyInstance.Create("DarkIntentionsWoohoo.mod.settings.harmony");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            /* Log.Message("[Woohoo] Loaded"); */
        }
    }


    class WoohooModSettings : ModSettings
    {
        public float woohooChildChance = base_woohooChildChance;
        public float woohooBabyChildChance = base_woohooBabyChildChance;
        public bool sameGender = base_sameGender;


        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref this.woohooChildChance, "woohooChildChance", base_woohooChildChance);
            Scribe_Values.Look(ref this.woohooBabyChildChance, "woohooBabyChildChance", base_woohooBabyChildChance);
            Scribe_Values.Look(ref this.sameGender, "sameGender", base_sameGender);
        }


        static readonly float base_woohooChildChance = 0.01f;
        static readonly float base_woohooBabyChildChance = 0.5f;
        static readonly bool base_sameGender = true;
    }
}