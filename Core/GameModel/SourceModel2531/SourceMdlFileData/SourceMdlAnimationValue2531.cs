using System.Runtime.InteropServices;

namespace Crowbar
{
    [StructLayout(LayoutKind.Explicit)]
    public struct SourceMdlAnimationValue2531
    {

        // FROM: SourceEngine2003_source HL2 Beta 2003\src_main\Public\studio.h
        // // animation frames
        // union mstudioanimvalue_t
        // {
        // struct 
        // {
        // byte	valid;
        // byte	total;
        // } num;
        // short		value;
        // };


        [FieldOffset(0)]
        public byte valid;
        [FieldOffset(1)]
        public byte total;
        [FieldOffset(0)]
        public short value;
    }
}