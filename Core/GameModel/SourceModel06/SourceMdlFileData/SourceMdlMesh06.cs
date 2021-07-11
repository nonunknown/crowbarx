using System.Collections.Generic;

namespace Crowbar
{
    public class SourceMdlMesh06
    {

        // FROM: [06] HL1Alpha model viewer gsmv_beta2a_bin_src\src\src\studio\studio.h
        // typedef struct
        // {
        // int					numtris;
        // int					triindex;		// separate triangles (->mstudiotrivert_t)
        // int					skinref;
        // int					numnorms;		// per mesh normals
        // int					normindex;		// normal vec3_t
        // // TOMAS: "0"
        // } mstudiomesh_t;

        public int faceCount;
        public int faceOffset;
        public int skinref;
        public int normalCount;
        public int normalOffset;
        public List<SourceMdlTriangleVertex06> theVertexAndNormalIndexes;
    }
}