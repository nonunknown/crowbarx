﻿
namespace Crowbar
{
    public class SourceMdlPivot10
    {

        // FROM: [1999] HLStandardSDK\SourceCode\engine\studio.h
        // // pivots
        // typedef struct 
        // {
        // vec3_t				org;	// pivot point
        // int					start;
        // int					end;
        // } mstudiopivot_t;

        public SourceVector point = new SourceVector();
        public int pivotStart;
        public int pivotEnd;
    }
}