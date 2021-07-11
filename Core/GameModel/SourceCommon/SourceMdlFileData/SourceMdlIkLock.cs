
namespace Crowbar
{
    public class SourceMdlIkLock
    {

        // FROM: SourceEngineXXXX_source\public\studio.h
        // struct mstudioiklock_t
        // {
        // DECLARE_BYTESWAP_DATADESC();
        // int			chain;
        // float		flPosWeight;
        // float		flLocalQWeight;
        // int			flags;
        // 
        // int			unused[4];
        // };



        public int chainIndex;
        public double posWeight;
        public double localQWeight;
        public int flags;
        public int[] unused = new int[4];
    }
}