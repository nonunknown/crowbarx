using System.Collections.Generic;

namespace Crowbar
{
    public class SourceMdlWeightList
    {
        public SourceMdlWeightList()
        {
            theWeights = new List<double>();
        }

        public string theName;
        public List<double> theWeights;
    }
}