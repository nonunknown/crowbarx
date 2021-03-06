using System.Collections.Generic;

namespace Crowbar
{
    public class SourceMdlFlexRule
    {

        // FROM: SourceEngineXXXX_source\public\studio.h
        // struct mstudioflexrule_t
        // {
        // DECLARE_BYTESWAP_DATADESC();
        // int					flex;
        // int					numops;
        // int					opindex;
        // inline mstudioflexop_t *iFlexOp( int i ) const { return  (mstudioflexop_t *)(((byte *)this) + opindex) + i; };
        // };

        // int					flex;
        public int flexIndex;
        // int					numops;
        public int opCount;
        // int					opindex;
        public int opOffset;
        public List<SourceMdlFlexOp> theFlexOps;
    }
}