﻿
namespace Crowbar
{
    public class SourceMdlFlexDesc
    {

        // struct mstudioflexdesc_t
        // {
        // DECLARE_BYTESWAP_DATADESC();
        // int					szFACSindex;
        // inline char * const pszFACS( void ) const { return ((char *)this) + szFACSindex; }
        // };

        // int					szFACSindex;
        public int nameOffset;
        public string theName;
        public bool theDescIsUsedByFlex = false;
        public bool theDescIsUsedByFlexRule = false;
        public bool theDescIsUsedByEyelid = false;

        // Public theVtaFrameIndex As Integer

    }
}