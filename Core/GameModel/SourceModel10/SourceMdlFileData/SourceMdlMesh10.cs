using System.Collections.Generic;

namespace Crowbar
{
    public class SourceMdlMesh10
    {

        // FROM: [1999] HLStandardSDK\SourceCode\engine\studio.h
        // // meshes
        // typedef struct 
        // {
        // int					numtris;
        // int					triindex;
        // int					skinref;
        // int					numnorms;		// per mesh normals
        // int					normindex;		// normal vec3_t
        // } mstudiomesh_t;

        public int faceCount;
        public int faceOffset;
        public int skinref;
        public int normalCount;
        // Based on source code, this field does not seem to be used.
        public int normalOffset;
        public List<SourceMeshTriangleStripOrFan10> theStripsAndFans;
    }
}