using System.Collections.Generic;

namespace Crowbar
{
    public class SourceVtxMaterialReplacementList06
    {

        // FROM: The Axel Project - source [MDL v37]\TAPSRC\src\public\optimize.h
        // struct MaterialReplacementListHeader_t
        // {
        // int numReplacements;
        // int replacementOffset;
        // inline MaterialReplacementHeader_t *pMaterialReplacement( int i ) const
        // {
        // MaterialReplacementHeader_t *pDebug = ( MaterialReplacementHeader_t *)(((byte *)this) + replacementOffset) + i; 
        // return pDebug;
        // }
        // };

        public int replacementCount;
        public int replacementOffset;
        public List<SourceVtxMaterialReplacement06> theVtxMaterialReplacements;
    }
}