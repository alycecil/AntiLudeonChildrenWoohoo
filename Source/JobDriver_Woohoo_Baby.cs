using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;

namespace DarkIntentionsWoohoo
{
    class JobDriver_Woohoo_Baby : JobDriver_Woohoo
    {
        public override bool isMakeBaby()
        {

            Log.Message("Wants a baby!", false);
            return true;
        }
    }
}
