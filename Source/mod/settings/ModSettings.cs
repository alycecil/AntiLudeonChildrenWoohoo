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
            var topHalf = inRect.TopHalf();
            var topForth = topHalf.TopHalf();
            
            this.settings.woohooChildChance = Widgets.HorizontalSlider(topForth.TopHalf().ContractedBy(5f),
                this.settings.woohooChildChance, 0f, 1f, true,
                "Risky Woohoo Factor " + this.settings.woohooChildChance * 100 +
                "\nDetermines How likely do we roll for pregnancy"+
                "\n The base game forces a second dice role on your pawns age group. Fertility Starts at 14 in base game."
                , "0%", "100%");
            
            this.settings.woohooBabyChildChance = Widgets.HorizontalSlider(topForth.BottomHalf().ContractedBy(5f),
                this.settings.woohooBabyChildChance, 0f, 1f, true,
                "How Likely We Are To Try and Get Pregnant why Trying For Baby : " +
                this.settings.woohooBabyChildChance * 100 +
                "\n The base game forces a second dice role on your pawns age group. Fertility Starts at 14 in base game.",
                "0%", "100%");

            var topbottomForth = topHalf.BottomHalf();

            var nextEighth = topbottomForth.TopHalf();
            Widgets.CheckboxLabeled(nextEighth.LeftHalf().ContractedBy(15f),"Can two people of the same gender have get pregnant.\n(Female, Female or Any, Any with Implanted Womb): ", ref this.settings.sameGender);
            
            this.settings.lovedItChance = Widgets.HorizontalSlider(nextEighth.RightHalf().ContractedBy(5f),
                this.settings.lovedItChance, 0f, 1f, true,
                "Chance to Apply Bonus Moodlet: " + this.settings.lovedItChance* 100, "Never (0%)", "Always (100%)");
            
            var nextRect = topbottomForth.BottomHalf();
            var box = nextRect.LeftHalf().ContractedBy(15f);
            
            Widgets.CheckboxLabeled(box, "Can Pawns WooHoo each other autonomously: " + this.settings.allowAIWoohoo, ref this.settings.allowAIWoohoo);
            
            this.settings.minAITicks = (int) Widgets.HorizontalSlider(nextRect.RightHalf().ContractedBy(5f),
                this.settings.minAITicks , 1000, 300000, true,
                this.settings.allowAIWoohoo ? ("Wait: " + ( 1f*this.settings.minAITicks/20000.0f ) + " days. (Plus/Minus 10%) "):("Enable AI"), "Same Day", "Next, Like Never");

            var bottomHalf = inRect.BottomHalf();
            var thirdForth = bottomHalf.TopHalf();
            this.settings.familyWeight = Widgets.HorizontalSlider(thirdForth.TopHalf().ContractedBy(5f),
                this.settings.familyWeight, 0f, 1f, true,
                "How hard should Directly Related People Try to Avoid Woohoo: " + this.settings.familyWeight* 100, "Don't Care (0%)", "Never Allow (100%)");
            
            //next is inRect.BottomHalf().BottomHalf().TopHalf()
            
//            
            Widgets.Label(bottomHalf.BottomHalf().BottomHalf().RightHalf(), "That's all, thanks for playing. -Alice.\nSource Code Available at https://github.com/alycecil/RimworldChildrenWoohoo");
            this.settings.Write();
        }
    }
    
    class WoohooModSettings : ModSettings
    {
        public float woohooChildChance = base_woohooChildChance;
        public float woohooBabyChildChance = base_woohooBabyChildChance;
        public bool sameGender = base_sameGender;
        public float familyWeight=base_familyWeight;
        public bool allowAIWoohoo=false;
        public int minAITicks=2500;
        public float lovedItChance=.85f;

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