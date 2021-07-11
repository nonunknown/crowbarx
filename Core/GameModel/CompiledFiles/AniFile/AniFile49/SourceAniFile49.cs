using System;
using System.IO;

namespace Crowbar
{
    public class SourceAniFile49 : SourceMdlFile49
    {

        #region Creation and Destruction

        public SourceAniFile49(BinaryReader aniFileReader, SourceFileData aniFileData, SourceFileData mdlFileData)
        {
            theInputFileReader = aniFileReader;
            theMdlFileData = (SourceMdlFileData49)aniFileData;
            theRealMdlFileData = (SourceMdlFileData49)mdlFileData;
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
                foreach (SourceMdlAnimationDesc49 anAnimationDesc in theRealMdlFileData.theAnimationDescs)
                {
                    try
                    {
                        long animBlockInputFileStreamPosition = theRealMdlFileData.theAnimBlocks[anAnimationDesc.animBlock].dataStart;
                        // Dim animBlockInputFileStreamEndPosition As Long = Me.theRealMdlFileData.theAnimBlocks(anAnimationDesc.animBlock).dataEnd
                        int sectionIndex;

                        // NOTE: There can be sections in ANI file even if anAnimationDesc.animBlock = 0.
                        if (anAnimationDesc.theSections is object && anAnimationDesc.theSections.Count > 0)
                        {
                            int sectionCount = anAnimationDesc.theSections.Count;
                            int sectionFrameCount;
                            SourceMdlAnimationSection section;
                            var loopTo = sectionCount - 1;
                            for (sectionIndex = 0; sectionIndex <= loopTo; sectionIndex++)
                            {
                                section = anAnimationDesc.theSections[sectionIndex];
                                if (section.animBlock > 0)
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

                                    animBlockInputFileStreamPosition = theRealMdlFileData.theAnimBlocks[section.animBlock].dataStart;
                                    // animBlockInputFileStreamEndPosition = Me.theRealMdlFileData.theAnimBlocks(section.animBlock).dataEnd
                                    ReadAnimationFrames(animBlockInputFileStreamPosition + section.animOffset, anAnimationDesc, sectionFrameCount, sectionIndex, sectionIndex >= sectionCount - 2 | anAnimationDesc.frameCount == (sectionIndex + 1) * anAnimationDesc.sectionFrameCount);
                                }
                            }
                        }
                        else if (anAnimationDesc.animBlock > 0)
                        {
                            sectionIndex = 0;
                            ReadAnimationFrames(animBlockInputFileStreamPosition + anAnimationDesc.animOffset, anAnimationDesc, anAnimationDesc.frameCount, sectionIndex, true);
                        }

                        // NOTE: These seem to always be stored in the MDL file for MDL44.
                        if (theMdlFileData.version != 44 && anAnimationDesc.animBlock > 0)
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

        #endregion

        #region Data

        protected SourceMdlFileData49 theRealMdlFileData;

        #endregion

    }
}