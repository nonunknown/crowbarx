
namespace Crowbar
{
    public class SourceMdlTriangleVertex06
    {

        // FROM: [06] HL1Alpha model viewer gsmv_beta2a_bin_src\src\src\studio\studio.h
        // typedef struct
        // {
        // short				vertindex;		// index into vertex array (relative)
        // short				normindex;		// index into normal array (relative)
        // short				s, t;			// s,t position on skin
        // } mstudiotrivert_t;

        public ushort vertexIndex;
        public ushort normalIndex;
        public short s;
        public short t;
    }
}