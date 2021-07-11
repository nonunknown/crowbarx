using System;
using System.Collections.Generic;
using System.IO;

namespace Crowbar
{

    // Example: barney from HLAlpha
    public class SourceModel06 : SourceModel04
    {

        #region Creation and Destruction

        public SourceModel06(string mdlPathFileName, int mdlVersion) : base(mdlPathFileName, mdlVersion)
        {
        }

        #endregion

        #region Properties

        public override bool HasTextureData
        {
            get
            {
                return theMdlFileData.theTextures is object && theMdlFileData.theTextures.Count > 0;
            }
        }

        public override bool HasMeshData
        {
            get
            {
                if (theMdlFileData.theBones is object && theMdlFileData.theBones.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public override bool HasBoneAnimationData
        {
            get
            {
                if (theMdlFileData.theSequences is object && theMdlFileData.theSequences.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public override bool HasTextureFileData
        {
            get
            {
                return theMdlFileData.theTextures is object && theMdlFileData.theTextures.Count > 0;
            }
        }

        #endregion

        #region Methods

        public override AppEnums.FilesFoundFlags CheckForRequiredFiles()
        {
            var status = AppEnums.FilesFoundFlags.AllFilesFound;
            return status;
        }

        public override AppEnums.StatusMessage WriteReferenceMeshFiles(string modelOutputPath)
        {
            var status = AppEnums.StatusMessage.Success;
            SourceMdlBodyPart06 aBodyPart;
            SourceMdlModel06 aBodyModel;
            // Dim smdFileName As String
            string smdPathFileName;
            if (theMdlFileData.theBodyParts is object)
            {
                for (int bodyPartIndex = 0, loopTo = theMdlFileData.theBodyParts.Count - 1; bodyPartIndex <= loopTo; bodyPartIndex++)
                {
                    aBodyPart = theMdlFileData.theBodyParts[bodyPartIndex];
                    if (aBodyPart.theModels is object)
                    {
                        for (int modelIndex = 0, loopTo1 = aBodyPart.theModels.Count - 1; modelIndex <= loopTo1; modelIndex++)
                        {
                            aBodyModel = aBodyPart.theModels[modelIndex];
                            if (aBodyModel.theName == "blank")
                            {
                                continue;
                            }

                            aBodyModel.theSmdFileName = SourceFileNamesModule.CreateBodyGroupSmdFileName(aBodyModel.theSmdFileName, bodyPartIndex, modelIndex, 0, theName, aBodyModel.theName);
                            smdPathFileName = Path.Combine(modelOutputPath, aBodyModel.theSmdFileName);
                            NotifySourceModelProgress(AppEnums.ProgressOptions.WritingFileStarted, smdPathFileName);
                            // NOTE: Check here in case writing is canceled in the above event.
                            if (theWritingIsCanceled)
                            {
                                status = AppEnums.StatusMessage.Canceled;
                                return status;
                            }
                            else if (theWritingSingleFileIsCanceled)
                            {
                                theWritingSingleFileIsCanceled = false;
                                continue;
                            }

                            WriteMeshSmdFile(smdPathFileName, aBodyModel);
                            NotifySourceModelProgress(AppEnums.ProgressOptions.WritingFileFinished, smdPathFileName);
                        }
                    }
                }
            }

            return status;
        }

        public override AppEnums.StatusMessage WriteBoneAnimationSmdFiles(string modelOutputPath)
        {
            var status = AppEnums.StatusMessage.Success;
            SourceMdlSequenceDesc06 aSequenceDesc;
            string smdPath;
            // Dim smdFileName As String
            string smdPathFileName;
            try
            {
                for (int aSequenceIndex = 0, loopTo = theMdlFileData.theSequences.Count - 1; aSequenceIndex <= loopTo; aSequenceIndex++)
                {
                    aSequenceDesc = theMdlFileData.theSequences[aSequenceIndex];
                    for (int blendIndex = 0, loopTo1 = aSequenceDesc.blendCount - 1; blendIndex <= loopTo1; blendIndex++)
                    {
                        if (aSequenceDesc.blendCount == 1)
                        {
                            aSequenceDesc.theSmdRelativePathFileName = SourceFileNamesModule.CreateAnimationSmdRelativePathFileName(aSequenceDesc.theSmdRelativePathFileName, theName, aSequenceDesc.theName, -1);
                        }
                        else
                        {
                            aSequenceDesc.theSmdRelativePathFileName = SourceFileNamesModule.CreateAnimationSmdRelativePathFileName(aSequenceDesc.theSmdRelativePathFileName, theName, aSequenceDesc.theName, blendIndex);
                        }

                        smdPathFileName = Path.Combine(modelOutputPath, aSequenceDesc.theSmdRelativePathFileName);
                        smdPath = FileManager.GetPath(smdPathFileName);
                        if (FileManager.PathExistsAfterTryToCreate(smdPath))
                        {
                            NotifySourceModelProgress(AppEnums.ProgressOptions.WritingFileStarted, smdPathFileName);
                            // NOTE: Check here in case writing is canceled in the above event.
                            if (theWritingIsCanceled)
                            {
                                status = AppEnums.StatusMessage.Canceled;
                                return status;
                            }
                            else if (theWritingSingleFileIsCanceled)
                            {
                                theWritingSingleFileIsCanceled = false;
                                continue;
                            }

                            WriteBoneAnimationSmdFile(smdPathFileName, aSequenceDesc, blendIndex);
                            NotifySourceModelProgress(AppEnums.ProgressOptions.WritingFileFinished, smdPathFileName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                int debug = 4242;
            }

            return status;
        }

        public override AppEnums.StatusMessage WriteTextureFiles(string modelOutputPath)
        {
            var status = AppEnums.StatusMessage.Success;
            List<SourceMdlTexture06> aTextureList;
            if (theMdlFileData.theTextures is object && theMdlFileData.theTextures.Count > 0)
            {
                aTextureList = theMdlFileData.theTextures;
            }
            else
            {
                return AppEnums.StatusMessage.Error;
            }

            string texturePath;
            string texturePathFileName;
            SourceMdlTexture06 aTexture;
            for (int textureIndex = 0, loopTo = aTextureList.Count - 1; textureIndex <= loopTo; textureIndex++)
            {
                try
                {
                    aTexture = aTextureList[textureIndex];
                    texturePathFileName = Path.Combine(modelOutputPath, aTexture.theFileName);
                    texturePath = FileManager.GetPath(texturePathFileName);
                    if (FileManager.PathExistsAfterTryToCreate(texturePath))
                    {
                        NotifySourceModelProgress(AppEnums.ProgressOptions.WritingFileStarted, texturePathFileName);
                        // NOTE: Check here in case writing is canceled in the above event.
                        if (theWritingIsCanceled)
                        {
                            status = AppEnums.StatusMessage.Canceled;
                            return status;
                        }
                        else if (theWritingSingleFileIsCanceled)
                        {
                            theWritingSingleFileIsCanceled = false;
                            continue;
                        }

                        var aBitmap = new BitmapFile(texturePathFileName, aTexture.width, aTexture.height, aTexture.theData);
                        aBitmap.Write();
                        NotifySourceModelProgress(AppEnums.ProgressOptions.WritingFileFinished, texturePathFileName);
                    }
                }
                catch (Exception ex)
                {
                    status = AppEnums.StatusMessage.Error;
                }
            }

            return status;
        }

        public override AppEnums.StatusMessage WriteAccessedBytesDebugFiles(string debugPath)
        {
            var status = AppEnums.StatusMessage.Success;
            string debugPathFileName;
            if (theMdlFileData is object)
            {
                debugPathFileName = Path.Combine(debugPath, theName + " " + "DEC_LOG");
                NotifySourceModelProgress(AppEnums.ProgressOptions.WritingFileStarted, debugPathFileName);
                WriteAccessedBytesDebugFile(debugPathFileName, theMdlFileData.theFileSeekLog);
                NotifySourceModelProgress(AppEnums.ProgressOptions.WritingFileFinished, debugPathFileName);
            }

            return status;
        }

        public override List<string> GetTextureFileNames()
        {
            var textureFileNames = new List<string>();
            for (int i = 0, loopTo = theMdlFileData.theTextures.Count - 1; i <= loopTo; i++)
            {
                SourceMdlTexture06 aTexture;
                aTexture = theMdlFileData.theTextures[i];
                textureFileNames.Add(aTexture.theFileName);
            }

            return textureFileNames;
        }

        #endregion

        #region Private Methods

        protected override void ReadMdlFileHeader_Internal()
        {
            if (theMdlFileData is null)
            {
                theMdlFileData = new SourceMdlFileData06();
                theMdlFileDataGeneric = theMdlFileData;
            }

            var mdlFile = new SourceMdlFile06(theInputFileReader, theMdlFileData);
            mdlFile.ReadMdlHeader();
        }

        protected override void ReadMdlFileForViewer_Internal()
        {
            if (theMdlFileData is null)
            {
                theMdlFileData = new SourceMdlFileData06();
                theMdlFileDataGeneric = theMdlFileData;
            }

            var mdlFile = new SourceMdlFile06(theInputFileReader, theMdlFileData);
            mdlFile.ReadMdlHeader();

            // mdlFile.ReadTexturePaths()
            mdlFile.ReadTextures();
        }

        protected override void ReadMdlFile_Internal()
        {
            if (theMdlFileData is null)
            {
                theMdlFileData = new SourceMdlFileData06();
                theMdlFileDataGeneric = theMdlFileData;
            }

            var mdlFile = new SourceMdlFile06(theInputFileReader, theMdlFileData);
            mdlFile.ReadMdlHeader();
            mdlFile.ReadBones();
            mdlFile.ReadBoneControllers();

            // NOTE: Must read sequences before reading animations.
            mdlFile.ReadSequences();
            mdlFile.ReadAnimations();
            mdlFile.ReadBodyParts();
            mdlFile.ReadTextures();
            mdlFile.ReadSkins();
            mdlFile.ReadUnreadBytes();

            // Post-processing.
            mdlFile.GetBoneDataFromFirstSequenceFirstFrame();
            mdlFile.BuildBoneTransforms();
        }

        protected override void WriteQcFile()
        {
            var qcFile = new SourceQcFile06(theOutputFileTextWriter, theQcPathFileName, theMdlFileData, theName);
            try
            {
                qcFile.WriteHeaderComment();
                qcFile.WriteModelNameCommand();
                qcFile.WriteBodyGroupCommand();

                // If TheApp.Settings.DecompileDebugInfoFilesIsChecked Then
                // qcFile.WriteTextureFileNameComments()
                // End If

                qcFile.WriteControllerCommand();
                qcFile.WriteSequenceCommands();
            }
            catch (Exception ex)
            {
                int debug = 4242;
            }
            finally
            {
            }
        }

        protected AppEnums.StatusMessage WriteMeshSmdFile(string smdPathFileName, SourceMdlModel06 aModel)
        {
            var status = AppEnums.StatusMessage.Success;
            try
            {
                theOutputFileTextWriter = File.CreateText(smdPathFileName);
                WriteMeshSmdFile(aModel);
            }
            catch (Exception ex)
            {
                int debug = 4242;
            }
            finally
            {
                if (theOutputFileTextWriter is object)
                {
                    theOutputFileTextWriter.Flush();
                    theOutputFileTextWriter.Close();
                }
            }

            return status;
        }

        protected void WriteMeshSmdFile(SourceMdlModel06 aModel)
        {
            bool externalTexturesAreUsed = false;
            var smdFile = new SourceSmdFile06(theOutputFileTextWriter, theMdlFileData);
            try
            {
                smdFile.WriteHeaderComment();
                smdFile.WriteHeaderSection();
                smdFile.WriteNodesSection();
                smdFile.WriteSkeletonSection();
                smdFile.WriteTrianglesSection(aModel);
            }
            catch (Exception ex)
            {
                int debug = 4242;
            }

            if (externalTexturesAreUsed)
            {
                theMdlFileData.theTextures = null;
            }
        }

        public AppEnums.StatusMessage WriteBoneAnimationSmdFile(string smdPathFileName, SourceMdlSequenceDesc06 aSequence, int blendIndex)
        {
            var status = AppEnums.StatusMessage.Success;
            try
            {
                theOutputFileTextWriter = File.CreateText(smdPathFileName);
                var smdFile = new SourceSmdFile06(theOutputFileTextWriter, theMdlFileData);
                smdFile.WriteHeaderComment();
                smdFile.WriteHeaderSection();
                smdFile.WriteNodesSection();
                smdFile.WriteSkeletonSectionForAnimation(aSequence, blendIndex);
            }
            catch (Exception ex)
            {
                int debug = 4242;
            }
            finally
            {
                if (theOutputFileTextWriter is object)
                {
                    theOutputFileTextWriter.Flush();
                    theOutputFileTextWriter.Close();
                }
            }

            return status;
        }

        protected override void WriteMdlFileNameToMdlFile(string internalMdlFileName)
        {
            var mdlFile = new SourceMdlFile06(theOutputFileBinaryWriter, theMdlFileData);
            mdlFile.WriteInternalMdlFileName(internalMdlFileName);
        }

        #endregion

        #region Data

        private SourceMdlFileData06 theMdlFileData;
        // Private theTextureMdlFileData10 As SourceMdlFileData06

        #endregion

    }
}