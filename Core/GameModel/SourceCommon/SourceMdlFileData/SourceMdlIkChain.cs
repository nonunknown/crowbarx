﻿using System.Collections.Generic;

namespace Crowbar
{
    public class SourceMdlIkChain
    {

        // struct mstudioikchain_t
        // {
        // DECLARE_BYTESWAP_DATADESC();
        // int				sznameindex;
        // inline char * const pszName( void ) const { return ((char *)this) + sznameindex; }
        // int				linktype;
        // int				numlinks;
        // int				linkindex;
        // inline mstudioiklink_t *pLink( int i ) const { return (mstudioiklink_t *)(((byte *)this) + linkindex) + i; };
        // // FIXME: add unused entries
        // };


        // int				sznameindex;
        public int nameOffset;
        // int				linktype;
        public int linkType;
        // int				numlinks;
        public int linkCount;
        // int				linkindex;
        public int linkOffset;


        // inline char * const pszName( void ) const { return ((char *)this) + sznameindex; }
        public string theName;
        public List<SourceMdlIkLink> theLinks;
    }
}