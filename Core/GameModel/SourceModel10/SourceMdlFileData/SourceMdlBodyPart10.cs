using System.Collections.Generic;

namespace Crowbar
{
    public class SourceMdlBodyPart10
    {

        // FROM: [1999] HLStandardSDK\SourceCode\engine\studio.h
        // // body part index
        // typedef struct
        // {
        // char				name[64];
        // int					nummodels;
        // int					base;
        // int					modelindex; // index into models array
        // } mstudiobodyparts_t;

        public char[] name = new char[64];
        public int modelCount;
        public int @base;
        public int modelOffset;
        public string theName;
        public List<SourceMdlModel10> theModels;
    }
}