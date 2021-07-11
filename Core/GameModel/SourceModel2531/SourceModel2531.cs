using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualBasic.CompilerServices;

namespace Crowbar
{
    public class SourceModel2531 : SourceModel10
    {

        #region Creation and Destruction

        public SourceModel2531(string mdlPathFileName, int mdlVersion) : base(mdlPathFileName, mdlVersion)
        {
        }

        #endregion

        #region Properties

        public override bool SequenceGroupMdlFilesAreUsed
        {
            get
            {
                return false;
            }
        }

        public override bool TextureMdlFileIsUsed
        {
            get
            {
                return false;
            }
        }

        public override bool PhyFileIsUsed
        {
            get
            {
                return !string.IsNullOrEmpty(thePhyPathFileName) && File.Exists(thePhyPathFileName);
            }
        }

        public override bool VtxFileIsUsed
        {
            get
            {
                return !string.IsNullOrEmpty(theVtxPathFileName) && File.Exists(theVtxPathFileName);
            }
        }

        public override bool HasTextureData
        {
            get
            {
                return !theMdlFileDataGeneric.theMdlFileOnlyHasAnimations && theMdlFileData.theTextures is object && theMdlFileData.theTextures.Count > 0;
            }
        }

        public override bool HasMeshData
        {
            get
            {
                // TODO: [HasMeshData] Should check more than theBones.
                if (!theMdlFileDataGeneric.theMdlFileOnlyHasAnimations && theMdlFileData.theBones is object && theMdlFileData.theBones.Count > 0 && theVtxFileData is object)


                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public override bool HasLodMeshData
        {
            get
            {
                if (!theMdlFileData.theMdlFileOnlyHasAnimations && theMdlFileData.theBones is object && theMdlFileData.theBones.Count > 0 && theVtxFileData is object && theVtxFileData.lodCount > 0)



                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public override bool HasPhysicsMeshData
        {
            get
            {
                if (thePhyFileDataGeneric is object && thePhyFileDataGeneric.theSourcePhyCollisionDatas is object && !theMdlFileData.theMdlFileOnlyHasAnimations && theMdlFileData.theBones is object && theMdlFileData.theBones.Count > 0)



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
                if (theMdlFileData.theSequences is object && theMdlFileData.theSequences.Count > 0 && theMdlFileData.theAnimationDescs is object && theMdlFileData.theAnimationDescs.Count > 0)


                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public override bool HasVertexAnimationData
        {
            get
            {
                if (!theMdlFileData.theMdlFileOnlyHasAnimations && theMdlFileData.theFlexDescs is object && theMdlFileData.theFlexDescs.Count > 0)

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
                return false;
            }
        }

        #endregion

        #region Methods

        public override AppEnums.FilesFoundFlags CheckForRequiredFiles()
        {
            var status = AppEnums.FilesFoundFlags.AllFilesFound;
            if (!theMdlFileDataGeneric.theMdlFileOnlyHasAnimations)
            {
                thePhyPathFileName = Path.ChangeExtension(theMdlPathFileName, ".phy");
                theVtxPathFileName = Path.ChangeExtension(theMdlPathFileName, ".dx80.vtx");
                if (!File.Exists(theVtxPathFileName))
                {
                    theVtxPathFileName = Path.ChangeExtension(theMdlPathFileName, ".dx7_2bone.vtx");
                    if (!File.Exists(theVtxPathFileName))
                    {
                        theVtxPathFileName = Path.ChangeExtension(theMdlPathFileName, ".vtx");
                        if (!File.Exists(theVtxPathFileName))
                        {
                            status = AppEnums.FilesFoundFlags.ErrorRequiredVtxFileNotFound;
                        }
                    }
                }
            }

            return status;
        }

        public override AppEnums.StatusMessage ReadPhyFile()
        {
            var status = AppEnums.StatusMessage.Success;

            // If String.IsNullOrEmpty(Me.thePhyPathFileName) Then
            // status = Me.CheckForRequiredFiles()
            // End If

            if (status == AppEnums.StatusMessage.Success)
            {
                try
                {
                    ReadFile(thePhyPathFileName, ReadPhyFile_Internal);
                    if (thePhyFileDataGeneric.checksum != theMdlFileData.checksum)
                    {
                        // status = StatusMessage.WarningPhyChecksumDoesNotMatchMdl
                        NotifySourceModelProgress(AppEnums.ProgressOptions.WarningPhyFileChecksumDoesNotMatchMdlFileChecksum, "");
                    }
                }
                catch (Exception ex)
                {
                    status = AppEnums.StatusMessage.Error;
                }
            }

            return status;
        }

        public override AppEnums.StatusMessage WritePhysicsMeshSmdFile(string modelOutputPath)
        {
            var status = AppEnums.StatusMessage.Success;
            string physicsMeshPathFileName;
            // Me.thePhysicsMeshSmdFileName = SourceFileNamesModule.CreatePhysicsSmdFileName(Me.thePhysicsMeshSmdFileName, Me.theName)
            // physicsMeshPathFileName = Path.Combine(modelOutputPath, Me.thePhysicsMeshSmdFileName)
            thePhyFileDataGeneric.thePhysicsMeshSmdFileName = SourceFileNamesModule.CreatePhysicsSmdFileName(thePhyFileDataGeneric.thePhysicsMeshSmdFileName, theName);
            physicsMeshPathFileName = Path.Combine(modelOutputPath, thePhyFileDataGeneric.thePhysicsMeshSmdFileName);
            NotifySourceModelProgress(AppEnums.ProgressOptions.WritingFileStarted, physicsMeshPathFileName);
            WriteTextFile(physicsMeshPathFileName, WritePhysicsMeshSmdFile);
            NotifySourceModelProgress(AppEnums.ProgressOptions.WritingFileFinished, physicsMeshPathFileName);
            return status;
        }

        public override AppEnums.StatusMessage WriteReferenceMeshFiles(string modelOutputPath)
        {
            var status = AppEnums.StatusMessage.Success;
            status = WriteMeshSmdFiles(modelOutputPath, 0, 0);
            return status;
        }

        public override AppEnums.StatusMessage WriteLodMeshFiles(string modelOutputPath)
        {
            var status = AppEnums.StatusMessage.Success;
            status = WriteMeshSmdFiles(modelOutputPath, 1, theVtxFileData.lodCount - 1);
            return status;
        }

        public AppEnums.StatusMessage WriteMeshSmdFile(string smdPathFileName, int lodIndex, SourceVtxModel107 aVtxModel, SourceMdlModel2531 aModel, int bodyPartVertexIndexStart)
        {
            var status = AppEnums.StatusMessage.Success;
            try
            {
                theOutputFileTextWriter = File.CreateText(smdPathFileName);
                var smdFile = new SourceSmdFile2531(theOutputFileTextWriter, theMdlFileData);
                smdFile.WriteHeaderComment();
                smdFile.WriteHeaderSection();
                smdFile.WriteNodesSection();
                smdFile.WriteSkeletonSection();
                smdFile.WriteTrianglesSection(lodIndex, aVtxModel, aModel, bodyPartVertexIndexStart);
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

        public override AppEnums.StatusMessage WriteBoneAnimationSmdFiles(string modelOutputPath)
        {
            var status = AppEnums.StatusMessage.Success;
            SourceMdlAnimationDesc2531 anAnimationDesc;
            string smdPath;
            // Dim smdFileName As String
            string smdPathFileName;
            string writeStatus;
            for (int anAnimDescIndex = 0, loopTo = theMdlFileData.theAnimationDescs.Count - 1; anAnimDescIndex <= loopTo; anAnimDescIndex++)
            {
                try
                {
                    anAnimationDesc = theMdlFileData.theAnimationDescs[anAnimDescIndex];
                    anAnimationDesc.theSmdRelativePathFileName = SourceFileNamesModule.CreateAnimationSmdRelativePathFileName(anAnimationDesc.theSmdRelativePathFileName, Name, anAnimationDesc.theName);
                    smdPathFileName = Path.Combine(modelOutputPath, anAnimationDesc.theSmdRelativePathFileName);
                    smdPath = FileManager.GetPath(smdPathFileName);
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

                    writeStatus = WriteBoneAnimationSmdFile(smdPathFileName, null, anAnimationDesc);
                    if (writeStatus == "Success")
                    {
                        NotifySourceModelProgress(AppEnums.ProgressOptions.WritingFileFinished, smdPathFileName);
                    }
                    else
                    {
                        NotifySourceModelProgress(AppEnums.ProgressOptions.WritingFileFailed, writeStatus);
                    }
                }
                catch (Exception ex)
                {
                    int debug = 4242;
                }
            }

            return status;
        }

        // Public Overrides Function WriteVertexAnimationVtaFile(ByVal vtaPathFileName As String) As AppEnums.StatusMessage
        // Dim status As AppEnums.StatusMessage = StatusMessage.Success

        // Me.NotifySourceModelProgress(ProgressOptions.WritingFileStarted, vtaPathFileName)
        // Me.WriteTextFile(vtaPathFileName, AddressOf Me.WriteVertexAnimationVtaFile)
        // Me.NotifySourceModelProgress(ProgressOptions.WritingFileFinished, vtaPathFileName)

        // Return status
        // End Function

        public override AppEnums.StatusMessage WriteVertexAnimationVtaFiles(string modelOutputPath)
        {
            var status = AppEnums.StatusMessage.Success;
            string vtaFileName;
            string vtaPathFileName;
            vtaFileName = SourceFileNamesModule.GetVtaFileName(Name, 0);
            vtaPathFileName = Path.Combine(modelOutputPath, vtaFileName);
            NotifySourceModelProgress(AppEnums.ProgressOptions.WritingFileStarted, vtaPathFileName);
            try
            {
                theOutputFileTextWriter = File.CreateText(vtaPathFileName);
                WriteVertexAnimationVtaFile(null);
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

            NotifySourceModelProgress(AppEnums.ProgressOptions.WritingFileFinished, vtaPathFileName);
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

            if (theVtxFileData is object)
            {
                debugPathFileName = Path.Combine(debugPath, theName + " " + "VTX_LOG");
                NotifySourceModelProgress(AppEnums.ProgressOptions.WritingFileStarted, debugPathFileName);
                WriteAccessedBytesDebugFile(debugPathFileName, theVtxFileData.theFileSeekLog);
                NotifySourceModelProgress(AppEnums.ProgressOptions.WritingFileFinished, debugPathFileName);
            }

            if (thePhyFileDataGeneric is object)
            {
                debugPathFileName = Path.Combine(debugPath, theName + " " + "PHY_LOG");
                NotifySourceModelProgress(AppEnums.ProgressOptions.WritingFileStarted, debugPathFileName);
                WriteAccessedBytesDebugFile(debugPathFileName, thePhyFileDataGeneric.theFileSeekLog);
                NotifySourceModelProgress(AppEnums.ProgressOptions.WritingFileFinished, debugPathFileName);
            }

            return status;
        }

        public override List<string> GetTextureFolders()
        {
            var textureFolders = new List<string>();
            for (int i = 0, loopTo = theMdlFileData.theTexturePaths.Count - 1; i <= loopTo; i++)
            {
                string aTextureFolder;
                aTextureFolder = theMdlFileData.theTexturePaths[i];
                textureFolders.Add(aTextureFolder);
            }

            return textureFolders;
        }

        public override List<string> GetTextureFileNames()
        {
            var textureFileNames = new List<string>();
            for (int i = 0, loopTo = theMdlFileData.theTextures.Count - 1; i <= loopTo; i++)
            {
                SourceMdlTexture2531 aTexture;
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
                theMdlFileData = new SourceMdlFileData2531();
                theMdlFileDataGeneric = theMdlFileData;
            }

            var mdlFile = new SourceMdlFile2531(theInputFileReader, theMdlFileData);
            mdlFile.ReadMdlHeader();

            // If Me.theMdlFileData.fileSize <> Me.theMdlFileData.theActualFileSize Then
            // status = StatusMessage.ErrorInvalidInternalMdlFileSize
            // End If
        }

        protected override void ReadMdlFileForViewer_Internal()
        {
            if (theMdlFileData is null)
            {
                theMdlFileData = new SourceMdlFileData2531();
                theMdlFileDataGeneric = theMdlFileData;
            }

            var mdlFile = new SourceMdlFile2531(theInputFileReader, theMdlFileData);
            mdlFile.ReadMdlHeader();
            mdlFile.ReadTexturePaths();
            mdlFile.ReadTextures();
        }

        protected override void ReadMdlFile_Internal()
        {
            if (theMdlFileData is null)
            {
                theMdlFileData = new SourceMdlFileData2531();
                theMdlFileDataGeneric = theMdlFileData;
            }

            var mdlFile = new SourceMdlFile2531(theInputFileReader, theMdlFileData);
            mdlFile.ReadMdlHeader();
            mdlFile.ReadBones();
            mdlFile.ReadBoneControllers();
            mdlFile.ReadAttachments();
            mdlFile.ReadHitboxSets();
            mdlFile.ReadSurfaceProp();
            mdlFile.ReadSequenceGroups();
            // NOTE: Must read sequences before reading animations.
            mdlFile.ReadSequences();
            // mdlFile.ReadTransitions()

            mdlFile.ReadLocalAnimationDescs();

            // NOTE: Read flex descs before body parts so that flexes (within body parts) can add info to flex descs.
            mdlFile.ReadFlexDescs();
            mdlFile.ReadBodyParts();
            mdlFile.ReadFlexControllers();
            // NOTE: This must be after flex descs are read so that flex desc usage can be saved in flex desc.
            mdlFile.ReadFlexRules();
            mdlFile.ReadPoseParamDescs();
            mdlFile.ReadTextures();
            mdlFile.ReadTexturePaths();
            mdlFile.ReadSkins();
            mdlFile.ReadIncludeModels();
            mdlFile.ReadUnreadBytes();

            // Post-processing.
            mdlFile.CreateFlexFrameList();
        }

        protected override void ReadPhyFile_Internal()
        {
            if (thePhyFileDataGeneric is null)
            {
                thePhyFileDataGeneric = new SourcePhyFileData();
            }

            var phyFile = new SourcePhyFile(theInputFileReader, thePhyFileDataGeneric);
            phyFile.ReadSourcePhyHeader();
            if (thePhyFileDataGeneric.solidCount > 0)
            {
                phyFile.ReadSourceCollisionData();
                phyFile.CalculateVertexNormals();
                phyFile.ReadSourcePhysCollisionModels();
                phyFile.ReadSourcePhyRagdollConstraintDescs();
                phyFile.ReadSourcePhyCollisionRules();
                phyFile.ReadSourcePhyEditParamsSection();
                phyFile.ReadCollisionTextSection();
            }

            phyFile.ReadUnreadBytes();
        }

        protected override void ReadVtxFile_Internal()
        {
            if (theVtxFileData is null)
            {
                theVtxFileData = new SourceVtxFileData107();
            }

            var vtxFile = new SourceVtxFile107(theInputFileReader, theVtxFileData);
            vtxFile.ReadSourceVtxHeader();
            vtxFile.ReadSourceVtxBodyParts();
            vtxFile.ReadUnreadBytes();
        }

        protected override void WriteQcFile()
        {
            var qcFile = new SourceQcFile2531(theOutputFileTextWriter, theQcPathFileName, theMdlFileData, theVtxFileData, thePhyFileDataGeneric, theName);
            try
            {
                qcFile.WriteHeaderComment();
                qcFile.WriteModelNameCommand();
                qcFile.WriteBodyGroupCommand();
                qcFile.WriteLodCommand();
                qcFile.WriteStaticPropCommand();
                // qcFile.WriteFlagsCommand()
                qcFile.WriteIllumPositionCommand();
                qcFile.WriteEyePositionCommand();
                qcFile.WriteSurfacePropCommand();
                qcFile.WriteCdMaterialsCommand();
                qcFile.WriteTextureGroupCommand();
                // If TheApp.Settings.DecompileDebugInfoFilesIsChecked Then
                // qcFile.WriteTextureFileNameComments()
                // End If

                qcFile.WriteAttachmentCommand();
                qcFile.WriteCBoxCommand();
                qcFile.WriteBBoxCommand();
                qcFile.WriteHBoxRelatedCommands();
                qcFile.WriteControllerCommand();

                // qcFile.WriteSequenceGroupCommands()
                qcFile.WriteSequenceCommands();
                qcFile.WriteIncludeModelCommands();
                qcFile.WriteCollisionModelOrCollisionJointsCommand();
                qcFile.WriteCollisionTextCommand();
            }
            catch (Exception ex)
            {
                int debug = 4242;
            }
            finally
            {
            }
        }

        protected override void WritePhysicsMeshSmdFile()
        {
            var physicsSmdFile = new SourceSmdFile2531(theOutputFileTextWriter, theMdlFileData, thePhyFileDataGeneric);
            try
            {
                physicsSmdFile.WriteHeaderComment();
                physicsSmdFile.WriteHeaderSection();
                physicsSmdFile.WriteNodesSection();
                physicsSmdFile.WriteSkeletonSection();
                physicsSmdFile.WriteTrianglesSectionForPhysics();
            }
            catch (Exception ex)
            {
                int debug = 4242;
            }
            finally
            {
            }
        }

        protected virtual AppEnums.StatusMessage WriteMeshSmdFiles(string modelOutputPath, int lodStartIndex, int lodStopIndex)
        {
            var status = AppEnums.StatusMessage.Success;

            // Dim smdFileName As String
            string smdPathFileName;
            SourceVtxBodyPart107 aBodyPart;
            SourceVtxModel107 aVtxModel;
            SourceMdlModel2531 aBodyModel;
            int bodyPartVertexIndexStart;
            bodyPartVertexIndexStart = 0;
            if (theVtxFileData.theVtxBodyParts is object && theMdlFileData.theBodyParts is object)
            {
                for (int bodyPartIndex = 0, loopTo = theVtxFileData.theVtxBodyParts.Count - 1; bodyPartIndex <= loopTo; bodyPartIndex++)
                {
                    aBodyPart = theVtxFileData.theVtxBodyParts[bodyPartIndex];
                    if (aBodyPart.theVtxModels is object)
                    {
                        for (int modelIndex = 0, loopTo1 = aBodyPart.theVtxModels.Count - 1; modelIndex <= loopTo1; modelIndex++)
                        {
                            aVtxModel = aBodyPart.theVtxModels[modelIndex];
                            if (aVtxModel.theVtxModelLods is object)
                            {
                                aBodyModel = theMdlFileData.theBodyParts[bodyPartIndex].theModels[modelIndex];
                                if (aBodyModel.name[0] == '\0' && aVtxModel.theVtxModelLods[0].theVtxMeshes is null)
                                {
                                    continue;
                                }

                                for (int lodIndex = lodStartIndex, loopTo2 = lodStopIndex; lodIndex <= loopTo2; lodIndex++)
                                {
                                    // TODO: Why would this count be different than the file header count?
                                    if (lodIndex >= aVtxModel.theVtxModelLods.Count)
                                    {
                                        break;
                                    }

                                    try
                                    {
                                        string bodyModelName;
                                        // bodyModelName = Me.theMdlFileData.theSequenceGroups(0).theFileName
                                        // If String.IsNullOrEmpty(bodyModelName) OrElse FileManager.FilePathHasInvalidChars(bodyModelName) Then
                                        bodyModelName = Conversions.ToString(theMdlFileData.theBodyParts[bodyPartIndex].theModels[modelIndex].name);
                                        // End If
                                        aBodyModel.theSmdFileNames[lodIndex] = SourceFileNamesModule.CreateBodyGroupSmdFileName(aBodyModel.theSmdFileNames[lodIndex], bodyPartIndex, modelIndex, lodIndex, theName, bodyModelName);
                                        smdPathFileName = Path.Combine(modelOutputPath, aBodyModel.theSmdFileNames[lodIndex]);
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

                                        WriteMeshSmdFile(smdPathFileName, lodIndex, aVtxModel, aBodyModel, bodyPartVertexIndexStart);
                                        NotifySourceModelProgress(AppEnums.ProgressOptions.WritingFileFinished, smdPathFileName);
                                    }
                                    catch (Exception ex)
                                    {
                                        int debug = 4242;
                                    }
                                }

                                bodyPartVertexIndexStart += aBodyModel.vertexCount;
                            }
                        }
                    }
                }
            }

            return status;
        }

        protected override void WriteBoneAnimationSmdFile(SourceMdlSequenceDescBase aSequenceDesc, SourceMdlAnimationDescBase anAnimationDesc)
        {
            var smdFile = new SourceSmdFile2531(theOutputFileTextWriter, theMdlFileData);
            try
            {
                smdFile.WriteHeaderComment();
                smdFile.WriteHeaderSection();
                smdFile.WriteNodesSection();
                smdFile.WriteSkeletonSectionForAnimation(aSequenceDesc, anAnimationDesc);
            }
            catch (Exception ex)
            {
                int debug = 4242;
            }
        }

        protected override void WriteVertexAnimationVtaFile(SourceMdlBodyPart bodyPart)
        {
            var vertexAnimationVtaFile = new SourceVtaFile2531(theOutputFileTextWriter, theMdlFileData);
            try
            {
                vertexAnimationVtaFile.WriteHeaderComment();
                vertexAnimationVtaFile.WriteHeaderSection();
                vertexAnimationVtaFile.WriteNodesSection();
                vertexAnimationVtaFile.WriteSkeletonSectionForVertexAnimation();
                vertexAnimationVtaFile.WriteVertexAnimationSection();
            }
            catch (Exception ex)
            {
                int debug = 4242;
            }
            finally
            {
            }
        }

        protected override void WriteMdlFileNameToMdlFile(string internalMdlFileName)
        {
            var mdlFile = new SourceMdlFile2531(theOutputFileBinaryWriter, theMdlFileData);
            mdlFile.WriteInternalMdlFileName(internalMdlFileName);
        }

        #endregion

        #region Constants

        // '#define MAX_NUM_BONES_PER_VERT 4
        // '#define MAX_NUM_BONES_PER_TRI ( MAX_NUM_BONES_PER_VERT * 3 )
        // '#define MAX_NUM_BONES_PER_STRIP 16
        // Public Shared MAX_NUM_BONES_PER_VERT As Integer = 4
        // Public Shared MAX_NUM_BONES_PER_TRI As Integer = MAX_NUM_BONES_PER_VERT * 3
        // Public Shared MAX_NUM_BONES_PER_STRIP As Integer = 16
        // ------
        // FROM: VAMPTools-master\MDLConverter\inc\external\studio.h
        // #define MAX_NUM_BONES_PER_VERT 3
        public static int MAX_NUM_BONES_PER_VERT = 3;

        #endregion

        #region Data

        private SourceMdlFileData2531 theMdlFileData;
        // Private thePhyFileData As SourcePhyFileData2531
        private SourceVtxFileData107 theVtxFileData;

        #endregion

    }
}