using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;

namespace DarkIntentionsWoohoo
{
    public class HookupBedmanager
    {
        Building_Bed bed;
        IEnumerable<Pawn> owners;

        public HookupBedmanager(Building_Bed bed)
        {
            this.bed = bed;
            if (bed != null)
            {
                IEnumerable<Pawn> owners = currentOwners();
                if (owners != null)
                {
                    this.owners = owners.Where(x => true);//lazy clone;
                }
            }

            //Log.Message("Bed Manager Created : Owners? ["+(owners!=null && owners.Any())+"]");
        }

        public IEnumerable<Toil> GiveBackToil()
        {
            if (bed != null)

                yield return new Toil
                {
                    initAction = delegate ()
                    {
                        //Log.Message("Boy get out my house, I ant making you pancakes an' you an't movin' in.");
                        GiveBack();
                    },
                    defaultCompleteMode = ToilCompleteMode.Instant
                };
            yield break;
            
        }

        public bool claim(Pawn bedPawn1, Pawn bedPawn2)
        {
            if (bed == null) return false;
            if (owners != null)
            {
                foreach (Pawn pawn in owners)
                {
                    releaseBed(bed, pawn);
                }
            }
            if((currentOwners() != null && currentOwners().Any()))
            {
                foreach (Pawn pawn in currentOwners())
                {
                    releaseBed(bed, pawn);
                }
            }
            return claimBed(bed, bedPawn1) &&
            claimBed(bed, bedPawn2);
        }

        public void GiveBack()
        {
            if (bed == null) return;
            //Log.Message("Giving back bed");
            foreach(Pawn pawn in currentOwners())
            {
                if(owners == null || !owners.Contains(pawn))
                {
                    releaseBed(bed, pawn);
                }
            }
            

            
            if(owners != null)
            {
                foreach(Pawn pawn in owners.Where(pawn => currentOwners() != null && !currentOwners().Contains(pawn)))
                {
                    claimBed(bed, pawn);
                }
            }

        }

        public IEnumerable<Pawn> currentOwners()
        {
            //TODO make this less hacky
            if(bed.AssignedPawns!=null && bed.AssignedPawns.Any())
            {
                return bed.AssignedPawns.ToList().AsEnumerable();
            }
            else if(bed.owners!=null)
            {
                return bed.owners.ToList().AsEnumerable();
            }
            return null;
        }

        public static bool claimBed(Building_Bed bed, Pawn pawn)
        {
            if (pawn == null || bed == null) return false;

            if (bed.AnyUnownedSleepingSlot)
            {
                bed.TryAssignPawn(pawn);
                return true;
            }
            else
            {
                //Log.Message("No spots?! ugh");
                return false;
            }

        }

        public static void releaseBed(Building_Bed bed, Pawn pawn)
        {
            if (pawn == null || bed == null) return;

            //cause that doenst work lets do this
            bed.TryUnassignPawn(pawn);
        }
    }
}
