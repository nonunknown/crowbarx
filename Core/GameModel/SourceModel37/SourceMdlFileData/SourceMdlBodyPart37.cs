using System.Collections.Generic;

namespace Crowbar
{
    public class SourceMdlBodyPart37
    {

        // struct mstudiobodyparts_t
        // {
        // int	sznameindex;
        // inline char * const pszName( void ) const { return ((char *)this) + sznameindex; }
        // int	nummodels;
        // int	base;
        // int	modelindex; // index into models array
        // inline mstudiomodel_t *pModel( int i ) const { return (mstudiomodel_t *)(((byte *)this) + modelindex) + i; };
        // };

        public int nameOffset;
        public int modelCount;
        public int @base;
        public int modelOffset;
        public string theName;
        public List<SourceMdlModel37> theModels;
        public bool theModelCommandIsUsed;
        public bool theEyeballOptionIsUsed;
        public List<FlexFrame37> theFlexFrames;
    }
}