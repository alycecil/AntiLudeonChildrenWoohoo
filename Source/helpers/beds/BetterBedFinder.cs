using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;
using Verse.AI;

namespace DarkIntentionsWoohoo
{
    class BetterBedFinder
    {
        public static Building_Bed DoBetterBedFinder(Pawn pawn, Pawn mate)
        {
            if (pawn == null || mate == null) return null;

            Building_Bed buildingBed;
            if ((buildingBed = PawnBedBigEnough(pawn)) != null)
            {
                return buildingBed;
            }

            if ((buildingBed = PawnBedBigEnough(pawn)) != null)
            {
                return buildingBed;
            }

            var allBeds = pawn.Map.listerBuildings.allBuildingsColonist
                .ConvertAll(x => x as Building_Bed);

            if (!allBeds.Any())
            {
                return null;
            }

            IEnumerable<Building_Bed> bigBeds = allBeds
                    .Where(x => x != null && x.SleepingSlotsCount > 1 && !x.Medical).ToList()
                ;

            if (!bigBeds.Any())
            {
                return null;
            }

            var priority = bigBeds.Where(x => x.AssignedPawns.Contains(pawn) || x.AssignedPawns.Contains(mate));
            var buildingBeds = priority.ToList();
            if (buildingBeds.Any())
            {
                foreach (var bed in buildingBeds)
                {
                    if (bed != null && CanReserve(pawn, bed) && CanReserve(mate, bed))
                    {
                        return bed;
                    }
                }
            }

            //not else in-case that fails for some reason, shouldn't but lets let the logic flow
            //What we know, All Beds with at least 2 slots do not belong to our couple.
            //
            // We will now look at the the available beds and look for an empty one
            // if not well woohoo 
            foreach (var openBed in bigBeds.Where(x => x.AssignedPawns == null || !x.AssignedPawns.Any()))
            {
                if (CanReserve(pawn, openBed) && CanReserve(mate, openBed))

                    return openBed;
            }

            //lets steal a bed!
            return bigBeds.Where(bed => bed.CurOccupants == null || !bed.CurOccupants.Any())
                .FirstOrDefault(stolenBed => CanReserve(pawn, stolenBed) && CanReserve(mate, stolenBed));
        }

        private static bool CanReserve(Pawn traveler, Building_Bed buildingBed)
        {
            LocalTargetInfo target = buildingBed;
            PathEndMode peMode = PathEndMode.OnCell;
            Danger maxDanger = Danger.Some;
            int sleepingSlotsCount = buildingBed.SleepingSlotsCount;
            return traveler.CanReserveAndReach(target, peMode, maxDanger, sleepingSlotsCount);
        }

        private static Building_Bed PawnBedBigEnough(Pawn pawn)
        {
            Building_Bed buildingBed = pawn?.CurrentBed();

            if (buildingBed != null && buildingBed.SleepingSlotsCount > 1)
            {
                //Score// Woohoo here.
                return buildingBed;
            }

            return null;
        }
    }
}