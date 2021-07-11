using System.Collections.Generic;

namespace Crowbar
{
    public class SourceMdlTexture06
    {

        // FROM: [06] HL1Alpha model viewer gsmv_beta2a_bin_src\src\src\studio\studio.h
        // typedef struct
        // {
        // char				name[64];
        // int					flags;
        // int					width;
        // int					height;
        // int					index;
        // } mstudiotexture_t;

        public char[] fileName = new char[64];
        public int flags;
        public uint width;
        public uint height;
        public uint dataOffset;
        public string theFileName;
        public List<byte> theData;
    }
}