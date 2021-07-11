using System.Collections.Generic;

namespace Crowbar
{
    public class SourceMdlMesh04
    {
        public char[] name = new char[32];
        public int faceCount;
        public int unknownCount;
        public uint textureWidth;
        public uint textureHeight;
        public string theName;
        public List<SourceMdlFace04> theFaces;
        public List<byte> theTextureBmpData;
        public string theTextureFileName;
    }
}