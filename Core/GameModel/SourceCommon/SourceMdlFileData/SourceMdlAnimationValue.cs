using System.Runtime.InteropServices;

namespace Crowbar
{
    [StructLayout(LayoutKind.Explicit)]
    public struct SourceMdlAnimationValue
    {

        // FROM: SourceEngine2006_source\public\studio.h
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