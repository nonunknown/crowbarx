using System;
using System.IO;

namespace Crowbar
{
    public class SourceAniFile52 : SourceMdlFile52
    {

        #region Creation and Destruction

        public SourceAniFile52(BinaryReader aniFileReader, SourceFileData aniFileData, SourceFileData mdlFileData)
        {
            theInputFileReader = aniFileReader;
            theMdlFileData = (SourceMdlFileData52)aniFileData;
            theRealMdlFileData = (SourceMdlFileData52)mdlFileData;
            theMdlFileData.theFileSeekLog.FileSize = theInputFileReader.BaseStream.Length;

            // NOTE: Need the bone data from the real MDL file because SourceAniFile inherits SourceMdlFile.ReadMdlAnimation() that uses the data.
            theMdlFileData.theBones = theRealMdlFileData.theBones;
        }

        #endregion

        #region Methods

        // TODO: [2015-08-16] Currently the same as SourceAniFile48. Not sure how to share the code while still having the two versions call different ReadAniAnimation().
        // Public Sub ReadAniBlocks(ByVal delegateReadAniAnimation As ReadAniAnimationDelegate)
        public void ReadAnimationAniBlocks()
        {
            if (theRealMdlFileData.theAnimationDescs is object)
            {
                long animBlockInputFileStreamPosition;
                long animBlockInputFileStreamEndPosition;
                SourceMdlAnimationDesc52 anAnimationDesc;
                for (int anAnimDescIndex = 0, loopTo = theRealMdlFileData.theAnimationDescs.Count - 1; anAnimDescIndex <= loopTo; anAnimDescIndex++)
                {
                    anAnimationDesc = theRealMdlFileData.theAnimationDescs[anAnimDescIndex];
                    animBlockInputFileStreamPosition = theRealMdlFileData.theAnimBlocks[anAnimationDesc.animBlock].dataStart;
                    animBlockInputFileStreamEndPosition = theRealMdlFileData.theAnimBlocks[anAnimationDesc.animBlock].dataEnd;
                    try
                    {
                        int sectionIndex;
                        if (anAnimationDesc.theSections is object && anAnimationDesc.theSections.Count > 0)
                        {
                            int sectionFrameCount;
                            int sectionCount;
                            sectionCount = anAnimationDesc.theSections.Count;
                            var loopTo1 = sectionCount - 1;
                            for (sectionIndex = 0; sectionIndex <= loopTo1; sectionIndex++)
                            {
                                if (anAnimationDesc.theSections[sectionIndex].animBlock > 0)
                                {
                                    if (sectionIndex < sectionCount - 2)
                                    {
                                        sectionFrameCount = anAnimationDesc.sectionFrameCount;
                                    }
                                    else
                                    {
                                        // NOTE: Due to the weird calculation of sectionCount in studiomdl, this line is called twice, which means there are two "last" sections.
                                        // This also likely means that the last section is bogus unused data.
                                        sectionFrameCount = anAnimationDesc.frameCount - (sectionCount - 2) * anAnimationDesc.sectionFrameCount;
                                    }

                                    animBlockInputFileStreamPosition = theRealMdlFileData.theAnimBlocks[anAnimationDesc.theSections[sectionIndex].animBlock].dataStart;
                                    animBlockInputFileStreamEndPosition = theRealMdlFileData.theAnimBlocks[anAnimationDesc.theSections[sectionIndex].animBlock].dataEnd;
                                    ReadAniAnimation(animBlockInputFileStreamPosition + anAnimationDesc.theSections[sectionIndex].animOffset, animBlockInputFileStreamEndPosition + anAnimationDesc.theSections[sectionIndex].animOffset, anAnimationDesc, sectionFrameCount, sectionIndex, sectionIndex >= sectionCount - 2 | anAnimationDesc.frameCount == (sectionIndex + 1) * anAnimationDesc.sectionFrameCount);
                                    // delegateReadAniAnimation.Invoke(animBlockInputFileStreamPosition + anAnimationDesc.theSections(sectionIndex).animOffset, animBlockInputFileStreamEndPosition + anAnimationDesc.theSections(sectionIndex).animOffset, anAnimationDesc, sectionFrameCount, sectionIndex)
                                }
                            }
                        }
                        else if (anAnimationDesc.animBlock > 0)
                        {
                            sectionIndex = 0;
                            ReadAniAnimation(animBlockInputFileStreamPosition + anAnimationDesc.animOffset, animBlockInputFileStreamEndPosition + anAnimationDesc.animOffset, anAnimationDesc, anAnimationDesc.frameCount, sectionIndex, true);
                            // delegateReadAniAnimation.Invoke(animBlockInputFileStreamPosition + anAnimationDesc.animOffset, animBlockInputFileStreamEndPosition + anAnimationDesc.animOffset, anAnimationDesc, anAnimationDesc.frameCount, sectionIndex)
                        }

                        if (anAnimationDesc.animBlock > 0)
                        {
                            ReadMdlIkRules(animBlockInputFileStreamPosition + anAnimationDesc.animblockIkRuleOffset, anAnimationDesc);
                            ReadLocalHierarchies(animBlockInputFileStreamPosition, anAnimationDesc);
                        }
                    }
                    catch (Exception ex)
                    {
                        int debug = 4242;
                    }
                }
            }
        }

        #endregion

        #region Private Methods

        private void ReadAniAnimation(long aniFileInputFileStreamPosition, long aniFileStreamEndPosition, SourceMdlAnimationDesc52 anAnimationDesc, int sectionFrameCount, int sectionIndex, bool lastSectionIsBeingRead)
        {
            ReadAnimationFrameByBone(aniFileInputFileStreamPosition, anAnimationDesc, sectionFrameCount, sectionIndex, lastSectionIsBeingRead);
        }

        #endregion

        #region Data

        protected SourceMdlFileData52 theRealMdlFileData;

        #endregion

    }
}