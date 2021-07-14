using System;
using System.IO;

namespace Crowbar
{

    // Example: PLAYER from HLAlpha
    public class SourceModel04 : SourceModel
    {

        #region Creation and Destruction

        public SourceModel04(string mdlPathFileName, int mdlVersion) : base(mdlPathFileName, mdlVersion)
        {
        }

        #endregion

        #region Properties

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
                if (theMdlFileData.theSequenceDescs is object && theMdlFileData.theSequenceDescs.Count > 0)
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
                return true;
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
            SourceMdlBodyPart04 aBodyPart;
            SourceMdlModel04 aBodyModel;
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
                            aBodyModel.theSmdFileName = SourceFileNamesModule.CreateBodyGroupSmdFileName(aBodyModel.theSmdFileName, bodyPartIndex, modelIndex, 0, theName, "");
                            smdPathFileName = Path.Combine(modelOutputPath, aBodyModel.theSmdFileName);
                            if (Program.TheApp.Verbose)
                            {
                                Console.WriteLine("     [VERBOSE] SmdPathFileName: " + smdPathFileName);
                            }
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
            SourceMdlSequenceDesc04 aSequenceDesc;
            string smdPath;
            string smdPathFileName;
            try
            {
                for (int aSequenceIndex = 0, loopTo = theMdlFileData.theSequenceDescs.Count - 1; aSequenceIndex <= loopTo; aSequenceIndex++)
                {
                    aSequenceDesc = theMdlFileData.theSequenceDescs[aSequenceIndex];
                    aSequenceDesc.theSmdRelativePathFileName = SourceFileNamesModule.CreateAnimationSmdRelativePathFileName(aSequenceDesc.theSmdRelativePathFileName, theName, aSequenceDesc.theName, -1);
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

                        WriteBoneAnimationSmdFile(smdPathFileName, aSequenceDesc);
                        NotifySourceModelProgress(AppEnums.ProgressOptions.WritingFileFinished, smdPathFileName);
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
            SourceMdlBodyPart04 aBodyPart;
            SourceMdlModel04 aModel;
            SourceMdlMesh04 aMesh;
            string texturePath;
            string textureFileName;
            string texturePathFileName;
            for (int bodyPartIndex = 0, loopTo = theMdlFileData.theBodyParts.Count - 1; bodyPartIndex <= loopTo; bodyPartIndex++)
            {
                aBodyPart = theMdlFileData.theBodyParts[bodyPartIndex];
                for (int modelIndex = 0, loopTo1 = aBodyPart.theModels.Count - 1; modelIndex <= loopTo1; modelIndex++)
                {
                    aModel = aBodyPart.theModels[modelIndex];
                    for (int meshIndex = 0, loopTo2 = aModel.theMeshes.Count - 1; meshIndex <= loopTo2; meshIndex++)
                    {
                        aMesh = aModel.theMeshes[meshIndex];
                        try
                        {
                            texturePath = modelOutputPath;
                            // textureFileName = "bodypart" + bodyPartIndex.ToString() + "_model" + modelIndex.ToString() + "_mesh" + meshIndex.ToString() + ".bmp"
                            textureFileName = aMesh.theTextureFileName;
                            texturePathFileName = Path.Combine(texturePath, textureFileName);
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

                                var aBitmap = new BitmapFile(texturePathFileName, aMesh.textureWidth, aMesh.textureHeight, aMesh.theTextureBmpData);
                                aBitmap.Write();
                                NotifySourceModelProgress(AppEnums.ProgressOptions.WritingFileFinished, texturePathFileName);
                            }
                        }
                        catch (Exception ex)
                        {
                            status = AppEnums.StatusMessage.Error;
                        }
                    }
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

        #endregion

        #region Private Methods

        protected override void ReadMdlFileHeader_Internal()
        {
            if (theMdlFileData is null)
            {
                theMdlFileData = new SourceMdlFileData04();
                theMdlFileDataGeneric = theMdlFileData;
            }

            var mdlFile = new SourceMdlFile04(theInputFileReader, theMdlFileData);
            mdlFile.ReadMdlHeader();
        }

        protected override void ReadMdlFileForViewer_Internal()
        {
            if (theMdlFileData is null)
            {
                theMdlFileData = new SourceMdlFileData04();
                theMdlFileDataGeneric = theMdlFileData;
            }

            var mdlFile = new SourceMdlFile04(theInputFileReader, theMdlFileData);
            mdlFile.ReadMdlHeader();
        }

        protected override void ReadMdlFile_Internal()
        {
            if (theMdlFileData is null)
            {
                theMdlFileData = new SourceMdlFileData04();
                theMdlFileDataGeneric = theMdlFileData;
            }

            var mdlFile = new SourceMdlFile04(theInputFileReader, theMdlFileData);
            mdlFile.ReadMdlHeader();
            mdlFile.ReadBones();
            mdlFile.ReadSequenceDescs();
            mdlFile.ReadBodyParts();
            mdlFile.ReadUnreadBytes();

            // Post-processing.
            // mdlFile.GetBoneDataFromFirstSequenceFirstFrame()
        }

        protected AppEnums.StatusMessage WriteMeshSmdFile(string smdPathFileName, SourceMdlModel04 aModel)
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

        protected void WriteMeshSmdFile(SourceMdlModel04 aModel)
        {
            bool externalTexturesAreUsed = false;
            var smdFile = new SourceSmdFile04(theOutputFileTextWriter, theMdlFileData);
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
        }

        public AppEnums.StatusMessage WriteBoneAnimationSmdFile(string smdPathFileName, SourceMdlSequenceDesc04 aSequence)
        {
            var status = AppEnums.StatusMessage.Success;
            try
            {
                theOutputFileTextWriter = File.CreateText(smdPathFileName);
                var smdFile = new SourceSmdFile04(theOutputFileTextWriter, theMdlFileData);
                smdFile.WriteHeaderComment();
                smdFile.WriteHeaderSection();
                smdFile.WriteNodesSection();
                smdFile.WriteSkeletonSectionForAnimation(aSequence);
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

        #endregion

        #region Data

        private SourceMdlFileData04 theMdlFileData;

        #endregion

    }
}