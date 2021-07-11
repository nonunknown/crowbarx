﻿using System.Collections.Generic;

namespace Crowbar
{
    public class SourceVtxBodyPart07
    {

        // FROM: src/public/optimize.h
        // struct BodyPartHeader_t
        // {
        // DECLARE_BYTESWAP_DATADESC();
        // int numModels;
        // int modelOffset;
        // inline ModelHeader_t *pModel( int i ) const 
        // { 
        // ModelHeader_t *pDebug = (ModelHeader_t *)(((byte *)this) + modelOffset) + i;
        // return pDebug;
        // };
        // };

        public int modelCount;
        public int modelOffset;
        public List<SourceVtxModel07> theVtxModels;
    }
}