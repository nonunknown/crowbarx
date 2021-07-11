using System.Collections.Generic;

namespace Crowbar
{
    public class SourceMdlAnimation10
    {
        public SourceMdlAnimation10() : base()
        {
            for (int offsetIndex = 0, loopTo = animationValueOffsets.Length - 1; offsetIndex <= loopTo; offsetIndex++)
                theAnimationValues[offsetIndex] = new List<SourceMdlAnimationValue10>();
        }

        // FROM: [1999] HLStandardSDK\SourceCode\engine\studio.h
        // typedef struct
        // {
        // unsigned short	offset[6];
        // } mstudioanim_t;

        public ushort[] animationValueOffsets = new ushort[6];
        public List<SourceMdlAnimationValue10>[] theAnimationValues = new List<SourceMdlAnimationValue10>[6];
    }
}