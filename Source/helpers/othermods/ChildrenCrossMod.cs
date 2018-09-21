using System;
using System.Linq;
using Children;
using Verse;

namespace DarkIntentionsWoohoo
{
    class ChildrenCrossMod
    {
        public readonly static HediffDef PregnancyDiscovered =
            DefDatabase<HediffDef>.GetNamedSilentFail("PregnancyDiscovered");


        public static bool isChildrenModOn()
        {
            return PregnancyDiscovered != null;
        }

        public static void Mated(Pawn donor, Pawn womb)
        {
            var actualWombGender = womb.gender;
            womb.gender = Gender.Female;

            var donorActualGender = donor.gender;
            donor.gender = Gender.Male;

            try
            {
                Type MorePawnUtils = null;

                var assembly = typeof(BackstoryDef).Assembly;


                foreach (Type mine in assembly.GetTypes().Where(x => x.FullName.Contains("MorePawnUtil")))
                {
                    if (mine != null)
                    {
                        MorePawnUtils = mine;
                        break;
                    }
                }

                if (MorePawnUtils == null)
                {
                    throw new Exception("Couldnt find Childern.MorePawnUtils in the assembly" +
                                        "<--- this is bad practice to control flow with an exception. Dont tell~");
                }

                bool didIt = false;
                foreach (var method in MorePawnUtils.GetMethods().Where(aMethod => aMethod.Name.Contains("Loved")))
                {
                    var result = method.Invoke(null, new object[] {donor, womb, true});

                    if (result != null)
                        /* Log.Message("Loved From Children Mod : " + result, false); */
                    didIt = true;
                    break;
                }

                if (!didIt)
                {
                    throw new Exception("Coundnt find the Loved method in Children");
                }
            }
            catch (Exception)
            {
                /* Log.Message("Children Failed, using default Mate:" + e.Message, false); */
                //well that failed
                Mate.DefaultMate(donor, womb);
            }
            finally
            {
                donor.gender = donorActualGender;
                womb.gender = actualWombGender;
            }
        }
    }
}