
namespace Crowbar
{
    public class SourceMdlPivot06
    {

        // FROM: [06] HL1Alpha model viewer gsmv_beta2a_bin_src\src\src\studio\studio.h
        // typedef struct
        // {
        // vec3_t				org;			// pivot point
        // int					start;
        // int					end;
        // } mstudiopivot_t;

        public SourceVector point = new SourceVector();
        public int pivotStart;
        public int pivotEnd;
    }
}