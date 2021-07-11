﻿using System;
using System.Collections.Generic;

namespace Crowbar
{
    public class DuplicateKeyComparer<TKey> : IComparer<TKey> where TKey : IComparable
    {
        public int Compare(TKey x, TKey y)
        {
            int result = x.CompareTo(y);
            if (result == 0)
            {
                return 1;
            }
            else
            {
                // Handle equality as being greater
                return result;
            }
        }
    }
}