using System.Reflection;
using Harmony;
using UnityEngine;
using Verse;

namespace DarkIntentionsWoohoo.mod.settings
{
    class WoohooMod : Mod
    {
        
        private WoohooModSettings settings;

        public WoohooMod(ModContentPack content) : base(content)
        {
            this.settings = GetSettings<WoohooModSettings>();
            WoohooSettingHelper.latest = this.settings;


            //Harmony Kickofff
            var harmony = HarmonyInstance.Create("DarkIntentionsWoohoo.mod.settings.harmony");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            /* Log.Message("[Woohoo] Loaded"); */
        }


        public override string SettingsCategory() => "Woohooer!";

        public override void DoSettingsWindowContents(Rect inRect)
        {
                
                
            this.settings.woohooChildChance = Widgets.HorizontalSlider(inRect.TopHalf().TopHalf().TopHalf(),
                this.settings.woohooChildChance, 0f, 1f, true,
                "Risky Woohoo Factor " + this.settings.woohooChildChance +
                "\nDetermines How likely do we roll for pregnancy"+
                "\n The base game forces a second dice role on your pawns age group. Fertility Starts at 14 in base game."
                , "0%", "100%");
            this.settings.woohooBabyChildChance = Widgets.HorizontalSlider(inRect.TopHalf().TopHalf().BottomHalf(),
                this.settings.woohooBabyChildChance, 0f, 1f, true,
                "How Likely We Are To Try and Get Pregnant why Trying For Baby : " +
                this.settings.woohooBabyChildChance +
                "\n The base game forces a second dice role on your pawns age group. Fertility Starts at 14 in base game.",
                "0%", "100%");
            this.settings.sameGender = Widgets.HorizontalSlider(inRect.TopHalf().BottomHalf().TopHalf(),
                                           (this.settings.sameGender ? 1f : 0f), 0f, 1f, true,
                                           "Can two people of the same gender have get pregnant.\n(Female, Female or Any, Any with Implanted Womb): " + this.settings.sameGender, "No (0%)", "Yes (100%)") >
                                       .5f;
            //TODO this.
//            this.settings.familyWeight = Widgets.HorizontalSlider(inRect.TopHalf().BottomHalf().BottomHalf(),
//                this.settings.familyWeight, 0f, 1f, true,
//                "How hard should Directly Related People Try to Avoid Woohoo: " + this.settings.familyWeight, "Don't Care (0%)", "Never (100%)");
//            
            Widgets.Label(inRect.BottomHalf().BottomHalf().BottomHalf(), "That's all, thanks for playing. -Alice.");
            this.settings.Write();
        }
    }
    
    class WoohooModSettings : ModSettings
    {
        public float woohooChildChance = base_woohooChildChance;
        public float woohooBabyChildChance = base_woohooBabyChildChance;
        public bool sameGender = base_sameGender;
        public float familyWeight;


        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref this.woohooChildChance, "woohooChildChance", base_woohooChildChance);
            Scribe_Values.Look(ref this.woohooBabyChildChance, "woohooBabyChildChance", base_woohooBabyChildChance);
            Scribe_Values.Look(ref this.sameGender, "sameGender", base_sameGender);
            Scribe_Values.Look(ref this.familyWeight, "familyWeight", base_familyWeight);
        }


        static readonly float base_woohooChildChance = 0.01f;
        private static readonly float base_familyWeight = 0f;
        static readonly float base_woohooBabyChildChance = 0.5f;
        static readonly bool base_sameGender = true;
    }
}