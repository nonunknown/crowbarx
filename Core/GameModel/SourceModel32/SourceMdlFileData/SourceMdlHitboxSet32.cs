using System.Collections.Generic;

namespace Crowbar
{
    public class SourceMdlHitboxSet32
    {

        // struct mstudiohitboxset_t
        // {
        // int	sznameindex;
        // inline char * const	pszName( void ) const { return ((char *)this) + sznameindex; }
        // int	numhitboxes;
        // int	hitboxindex;
        // inline mstudiobbox_t *pHitbox( int i ) const { return (mstudiobbox_t *)(((byte *)this) + hitboxindex) + i; };
        // };

        public int nameOffset;
        public int hitboxCount;
        public int hitboxOffset;
        public string theName;
        public List<SourceMdlHitbox32> theHitboxes;
    }
}