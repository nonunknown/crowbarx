
namespace Crowbar
{
    public class SourceMdlType1Vertex2531
    {

        // FROM: Bloodlines SDK source 2015-06-16\sdk-src (16.06.2015)\src\Public\studio.h
        // struct mstudiovertex1_t		// behar: +
        // {
        // unsigned short fracPos[3];
        // unsigned short fracNormIdx;
        // unsigned short fracTex[2];
        // };

        // Public positionX As UInt16
        // Public positionY As UInt16
        // Public positionZ As UInt16
        // Public normalIndex As UInt16
        // Public texCoordU As UInt16
        // Public texCoordV As UInt16
        // NOTE: Not float values.
        // Public position As New SourceVector()
        public byte[] unknown = new byte[12];
        // Public positionX As UInt16
        // Public positionY As UInt16
        // Public positionZ As UInt16
        // Public normalX As UInt16
        // Public normalY As UInt16
        // Public normalZ As UInt16
        public ushort positionX;
        public ushort positionY;
        public ushort positionZ;
        // Public normalX As UInt16
        // Public normalY As UInt16
        // Public normalZ As UInt16
        public byte normalX;
        public byte normalY;
        public byte normalZ;
        public byte texCoordU;
        public byte texCoordV;
        // Public scaleX As Byte
        // Public scaleY As Byte
        // Public scaleZ As Byte

    }
}