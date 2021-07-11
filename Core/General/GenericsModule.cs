using System.Collections.Generic;

namespace Crowbar
{
    static class GenericsModule
    {
        public static bool ListsAreEqual(List<double> list1, List<double> list2)
        {
            bool theListsAreEqual;
            theListsAreEqual = true;
            if (list1.Count != list2.Count)
            {
                theListsAreEqual = false;
            }
            else
            {
                for (int i = 0, loopTo = list1.Count - 1; i <= loopTo; i++)
                {
                    if (list1[i] != list2[i])
                    {
                        theListsAreEqual = false;
                        break;
                    }
                }
            }

            return theListsAreEqual;
        }
    }
}