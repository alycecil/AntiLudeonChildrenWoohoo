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
                "Risky Woohoo Factor " + this.settings.woohooChildChance * 100 +
                "\nDetermines How likely do we roll for pregnancy"+
                "\n The base game forces a second dice role on your pawns age group. Fertility Starts at 14 in base game."
                , "0%", "100%");
            this.settings.woohooBabyChildChance = Widgets.HorizontalSlider(inRect.TopHalf().TopHalf().BottomHalf(),
                this.settings.woohooBabyChildChance, 0f, 1f, true,
                "How Likely We Are To Try and Get Pregnant why Trying For Baby : " +
                this.settings.woohooBabyChildChance * 100 +
                "\n The base game forces a second dice role on your pawns age group. Fertility Starts at 14 in base game.",
                "0%", "100%");
            
            this.settings.sameGender = Widgets.HorizontalSlider(inRect.TopHalf().BottomHalf().TopHalf(),
                                           (this.settings.sameGender ? 1f : 0f), 0f, 1f, true,
                                           "Can two people of the same gender have get pregnant.\n(Female, Female or Any, Any with Implanted Womb): " + this.settings.sameGender, "No (0%)", "Yes (100%)") >
                                       .5f;

            
            this.settings.allowAIWoohoo = Widgets.HorizontalSlider(inRect.TopHalf().BottomHalf().BottomHalf().LeftHalf(),
                                              (this.settings.allowAIWoohoo ? 1f: 0f), 0f, 1f, true,
                                              "Can Pawns WooHoo each other autonomously: " + this.settings.allowAIWoohoo, "No (0%)", "Yes (100%)") >
                                          .5f;
            
            this.settings.minAITicks = (int) Widgets.HorizontalSlider(inRect.TopHalf().BottomHalf().BottomHalf().RightHalf(),
                this.settings.minAITicks , 1000, 1000000, true,
                this.settings.allowAIWoohoo ? ("Wait: " + ( 1f*this.settings.minAITicks/20000.0f ) + " days. (Plus/Minus 10%) "):("Enable AI"), "Same Day", "Next, Like Never");
            
//            this.settings.minAITicks = (int) Widgets.HorizontalSlider(inRect.BottomHalf().TopHalf().TopHalf(),
//                this.settings.minAITicks , 1000, 1000000, true,
//                this.settings.allowAIWoohoo ? ("Time until a pawn can autonomously woohoo again.: " + ( 1f*this.settings.minAITicks/20000.0f ) + " days. (Plus/Minus 10%) "):("Disabled without autonomous woohoo, above."), "Same Day", "Next, Like Never");
//            
            
            this.settings.familyWeight = Widgets.HorizontalSlider(inRect.BottomHalf().TopHalf().BottomHalf(),
                this.settings.familyWeight, 0f, 1f, true,
                "How hard should Directly Related People Try to Avoid Woohoo: " + this.settings.familyWeight* 100, "Don't Care (0%)", "Never Allow (100%)");
            
            this.settings.lovedItChance = Widgets.HorizontalSlider(inRect.BottomHalf().BottomHalf().TopHalf(),
                this.settings.lovedItChance, 0f, 1f, true,
                "Chance to Apply Bonus Moodlet: " + this.settings.lovedItChance* 100, "Don't Care (0%)", "Never Allow (100%)");
//            
            Widgets.Label(inRect.BottomHalf().BottomHalf().BottomHalf(), "That's all, thanks for playing. -Alice.\nSource Code Available at https://github.com/alycecil/RimworldChildrenWoohoo");
            this.settings.Write();
        }
    }
    
    class WoohooModSettings : ModSettings
    {
        public float woohooChildChance = base_woohooChildChance;
        public float woohooBabyChildChance = base_woohooBabyChildChance;
        public bool sameGender = base_sameGender;
        public float familyWeight;
        public bool allowAIWoohoo;
        public int minAITicks;
        public float lovedItChance;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref this.woohooChildChance, "woohooChildChance", base_woohooChildChance);
            Scribe_Values.Look(ref this.woohooBabyChildChance, "woohooBabyChildChance", base_woohooBabyChildChance);
            Scribe_Values.Look(ref this.sameGender, "sameGender", base_sameGender);
            Scribe_Values.Look(ref this.familyWeight, "familyWeight", base_familyWeight);
            Scribe_Values.Look(ref this.allowAIWoohoo, "allowAIWoohoo", true);
            Scribe_Values.Look(ref this.minAITicks, "minAIWoohoo", 2500);
            Scribe_Values.Look(ref this.lovedItChance, "lovedItChance", 1f);
        }


        static readonly float base_woohooChildChance = 0.01f;
        private static readonly float base_familyWeight = 0.25f;
        static readonly float base_woohooBabyChildChance = 0.5f;
        static readonly bool base_sameGender = true;
    }
}