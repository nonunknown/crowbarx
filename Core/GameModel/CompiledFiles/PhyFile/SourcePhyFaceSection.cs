using System.Collections.Generic;

namespace Crowbar
{
    public class SourcePhyFaceSection
    {
        public int theBoneIndex;
        public List<SourcePhyFace> theFaces = new List<SourcePhyFace>();
        public List<SourcePhyVertex> theVertices = new List<SourcePhyVertex>();
    }
}