using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace DarkIntentionsWoohoo
{
    public abstract class JobUtilityIdle
    {
        private static readonly List<JobDef> idleJobDefs = new List<JobDef>() {JobDefOf.Wait,
            JobDefOf.Goto, JobDefOf.GotoWander, JobDefOf.SocialFight, /*JobDefOf.SocialRelax,*/ JobDefOf.Wait_MaintainPosture, 
            JobDefOf.Insult, JobDefOf.LayDown};


        public static bool isIdle(Pawn pawn)
        {
            return pawn?.jobs != null && isIdleJob(pawn.jobs.curJob);
        }

        public static bool isIdleJob(Job jobsCurJob)
        {
            return jobsCurJob == null || idleJobDefs.Contains(jobsCurJob.def);
        }
    }
}