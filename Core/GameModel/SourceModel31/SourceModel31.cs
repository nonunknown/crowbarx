using System;
using System.IO;
using Microsoft.VisualBasic.CompilerServices;

namespace Crowbar
{
    public class SourceModel31 : SourceModel2531
    {

        #region Creation and Destruction

        public SourceModel31(string mdlPathFileName, int mdlVersion) : base(mdlPathFileName, mdlVersion)
        {
        }

        #endregion

        #region Properties

        public override bool AniFileIsUsed
        {
            get
            {
                return false;
            }
        }

        // TODO: Delete after reading phy file is implemented.
        // Public Overrides ReadOnly Property PhyFileIsUsed As Boolean
        // Get
        // Return False
        // End Get
        // End Property

        public override bool VtxFileIsUsed
        {
            get
            {
                return !string.IsNullOrEmpty(theVtxPathFileName) && File.Exists(theVtxPathFileName);
                // Return False
            }
        }

        public override bool VvdFileIsUsed
        {
            get
            {
                return false;
            }
        }

        public override bool HasTextureData
        {
            get
            {
                // TODO: Change back to top line after reading texture info from MDL file is implemented.
                // Return Not Me.theMdlFileDataGeneric.theMdlFileOnlyHasAnimations AndAlso Me.theMdlFileData.theTextures IsNot Nothing AndAlso Me.theMdlFileData.theTextures.Count > 0
                return false;
            }
        }

        public override bool HasMeshData
        {
            get
            {
                if (!theMdlFileData.theMdlFileOnlyHasAnimations && theMdlFileData.theBones is object && theMdlFileData.theBones.Count > 0 && theVtxFileData is object)


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

        public override bool HasProceduralBonesData
        {
            get
            {
                // If Me.theMdlFileData IsNot Nothing _
                // AndAlso Me.theMdlFileData.theProceduralBonesCommandIsUsed _
                // AndAlso Not Me.theMdlFileData.theMdlFileOnlyHasAnimations _
                // AndAlso Me.theMdlFileData.theBones IsNot Nothing _
                // AndAlso Me.theMdlFileData.theBones.Count > 0 Then
                // Return True
                // Else
                return false;
                // End If
            }
        }

        public override bool HasBoneAnimationData
        {
            get
            {
                if (theMdlFileData.theAnimationDescs is object && theMdlFileData.theAnimationDescs.Count > 0)
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
                // TODO: Change back to commented-out lines once implemented.
                // If Not Me.theMdlFileData.theMdlFileOnlyHasAnimations _
                // AndAlso Me.theMdlFileData.theFlexDescs IsNot Nothing _
                // AndAlso Me.theMdlFileData.theFlexDescs.Count > 0 Then
                // Return True
                // Else
                // Return False
                // End If
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
                theVtxPathFileName = Path.ChangeExtension(theMdlPathFileName, ".dx11.vtx");
                if (!File.Exists(theVtxPathFileName))
                {
                    theVtxPathFileName = Path.ChangeExtension(theMdlPathFileName, ".dx90.vtx");
                    if (!File.Exists(theVtxPathFileName))
                    {
                        theVtxPathFileName = Path.ChangeExtension(theMdlPathFileName, ".dx80.vtx");
                        if (!File.Exists(theVtxPathFileName))
                        {
                            theVtxPathFileName = Path.ChangeExtension(theMdlPathFileName, ".sw.vtx");
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
                }
            }

            return status;
        }

        // Public Overrides Function ReadPhyFile() As AppEnums.StatusMessage
        // Dim status As AppEnums.StatusMessage = StatusMessage.Success

        // If String.IsNullOrEmpty(Me.thePhyPathFileName) Then
        // status = Me.CheckForRequiredFiles()
        // End If

        // If Not String.IsNullOrEmpty(Me.thePhyPathFileName) Then
        // If status = StatusMessage.Success Then
        // Try
        // Me.ReadFile(Me.thePhyPathFileName, AddressOf Me.ReadPhyFile_Internal)
        // If Me.thePhyFileData.checksum <> Me.theMdlFileData.checksum Then
        // 'status = StatusMessage.WarningPhyChecksumDoesNotMatchMdl
        // Me.NotifySourceModelProgress(ProgressOptions.WarningPhyFileChecksumDoesNotMatchMdlFileChecksum, "")
        // End If
        // Catch ex As Exception
        // status = StatusMessage.Error
        // End Try
        // End If
        // End If

        // Return status
        // End Function

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

        public override AppEnums.StatusMessage WriteBoneAnimationSmdFiles(string modelOutputPath)
        {
            var status = AppEnums.StatusMessage.Success;
            SourceMdlAnimationDesc31 anAnimationDesc;
            string smdPath;
            // Dim smdFileName As String
            string smdPathFileName;
            string writeStatus;
            try
            {
                for (int anAnimDescIndex = 0, loopTo = theMdlFileData.theAnimationDescs.Count - 1; anAnimDescIndex <= loopTo; anAnimDescIndex++)
                {
                    anAnimationDesc = theMdlFileData.theAnimationDescs[anAnimDescIndex];
                    anAnimationDesc.theSmdRelativePathFileName = SourceFileNamesModule.CreateAnimationSmdRelativePathFileName(anAnimationDesc.theSmdRelativePathFileName, Name, anAnimationDesc.theName);
                    smdPathFileName = Path.Combine(modelOutputPath, anAnimationDesc.theSmdRelativePathFileName);
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
                }
            }
            catch (Exception ex)
            {
                int debug = 4242;
            }

            return status;
        }

        public AppEnums.StatusMessage WriteMeshSmdFile(string smdPathFileName, int lodIndex, SourceVtxModel06 aVtxModel, SourceMdlModel31 aModel, int bodyPartVertexIndexStart)
        {
            var status = AppEnums.StatusMessage.Success;
            try
            {
                theOutputFileTextWriter = File.CreateText(smdPathFileName);
                var smdFile = new SourceSmdFile31(theOutputFileTextWriter, theMdlFileData);
                smdFile.WriteHeaderComment();
                smdFile.WriteHeaderSection();
                smdFile.WriteNodesSection(lodIndex);
                smdFile.WriteSkeletonSection(lodIndex);
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

        #endregion

        #region Private Methods

        protected override void ReadMdlFileHeader_Internal()
        {
            if (theMdlFileData is null)
            {
                theMdlFileData = new SourceMdlFileData31();
                theMdlFileDataGeneric = theMdlFileData;
            }

            var mdlFile = new SourceMdlFile31(theInputFileReader, theMdlFileData);
            mdlFile.ReadMdlHeader00("MDL File Header 00");
            mdlFile.ReadMdlHeader01("MDL File Header 01");
        }

        protected override void ReadMdlFileForViewer_Internal()
        {
            if (theMdlFileData is null)
            {
                theMdlFileData = new SourceMdlFileData31();
                theMdlFileDataGeneric = theMdlFileData;
            }

            var mdlFile = new SourceMdlFile31(theInputFileReader, theMdlFileData);
            mdlFile.ReadMdlHeader00("MDL File Header 00");
            mdlFile.ReadMdlHeader01("MDL File Header 01");

            // 'mdlFile.ReadTexturePaths()
            // mdlFile.ReadTextures()
        }

        protected override void ReadMdlFile_Internal()
        {
            if (theMdlFileData is null)
            {
                theMdlFileData = new SourceMdlFileData31();
                theMdlFileDataGeneric = theMdlFileData;
            }

            var mdlFile = new SourceMdlFile31(theInputFileReader, theMdlFileData);
            mdlFile.ReadMdlHeader00("MDL File Header 00");
            mdlFile.ReadMdlHeader01("MDL File Header 01");

            // Read what WriteBoneInfo() writes.
            mdlFile.ReadBones();
            // mdlFile.ReadBoneControllers()
            // mdlFile.ReadAttachments()
            if (theMdlFileData.version >= 27 && theMdlFileData.version <= 30)
            {
                mdlFile.ReadHitboxes_MDL27to30();
            }
            else
            {
                mdlFile.ReadHitboxSets();
            }
            // mdlFile.ReadBoneDescs()

            // ' Read what WriteSequenceInfo() writes.
            // 'NOTE: Must read sequences before reading animations.
            // mdlFile.ReadAnimGroups()
            mdlFile.ReadSequences();
            mdlFile.ReadSequenceGroups();
            // mdlFile.ReadTransitions()

            // ' Read what WriteAnimations() writes.
            mdlFile.ReadLocalAnimationDescs();

            // ' Read what WriteModel() writes.
            mdlFile.ReadBodyParts();
            // mdlFile.ReadFlexDescs()
            // mdlFile.ReadFlexControllers()
            // 'NOTE: This must be after flex descs are read so that flex desc usage can be saved in flex desc.
            // mdlFile.ReadFlexRules()
            // mdlFile.ReadIkChains()
            // mdlFile.ReadIkLocks()
            // mdlFile.ReadMouths()
            // mdlFile.ReadPoseParamDescs()

            // ' Read what WriteTextures() writes.
            mdlFile.ReadTexturePaths();
            // 'NOTE: ReadTextures must be after ReadTexturePaths(), so it can compare with the texture paths.
            mdlFile.ReadTextures();
            mdlFile.ReadSkinFamilies();

            // '' Read what WriteKeyValues() writes.
            // 'mdlFile.ReadKeyValues()

            // mdlFile.ReadFinalBytesAlignment()
            mdlFile.ReadUnreadBytes();

            // '' Post-processing.
            // 'mdlFile.BuildBoneTransforms()
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
                theVtxFileData = new SourceVtxFileData06();
            }

            var vtxFile = new SourceVtxFile06(theInputFileReader, theVtxFileData);
            vtxFile.ReadSourceVtxHeader();
            // If Me.theVtxFileData.lodCount > 0 Then
            vtxFile.ReadSourceVtxBodyParts();
            // End If
            vtxFile.ReadSourceVtxMaterialReplacementLists();
            vtxFile.ReadUnreadBytes();
        }

        protected override void WriteQcFile()
        {
            // Dim qcFile As New SourceQcFile31(Me.theOutputFileTextWriter, Me.theQcPathFileName, Me.theMdlFileData, Me.thePhyFileData, Me.theVtxFileData, Me.theName)
            var qcFile = new SourceQcFile31(theOutputFileTextWriter, theQcPathFileName, theMdlFileData, theVtxFileData, thePhyFileDataGeneric, theName);
            try
            {
                qcFile.WriteHeaderComment();
                qcFile.WriteModelNameCommand();
                qcFile.WriteStaticPropCommand();
                qcFile.WriteBodyGroupCommand();
                qcFile.WriteGroup("lod", qcFile.WriteGroupLod, false, false);
                qcFile.WriteSurfacePropCommand();
                // qcFile.WriteJointSurfacePropCommand()
                // qcFile.WriteContentsCommand()
                // qcFile.WriteJointContentsCommand()
                qcFile.WriteIllumPositionCommand();
                qcFile.WriteEyePositionCommand();
                // qcFile.WriteNoForcedFadeCommand()
                // qcFile.WriteForcePhonemeCrossfadeCommand()

                // qcFile.WriteAmbientBoostCommand()
                // qcFile.WriteOpaqueCommand()
                // qcFile.WriteObsoleteCommand()
                qcFile.WriteCdMaterialsCommand();
                qcFile.WriteTextureGroupCommand();
                if (Program.TheApp.Settings.DecompileDebugInfoFilesIsChecked)
                {
                    qcFile.WriteTextureFileNameComments();
                }

                // qcFile.WriteAttachmentCommand()

                qcFile.WriteGroup("box", qcFile.WriteGroupBox, true, false);

                // qcFile.WriteControllerCommand()
                // qcFile.WriteScreenAlignCommand()

                // qcFile.WriteGroup("bone", AddressOf qcFile.WriteGroupBone, False, False)

                qcFile.WriteGroup("animation", qcFile.WriteGroupAnimation, false, false);
                qcFile.WriteGroup("collision", qcFile.WriteGroupCollision, false, false);
            }

            // 'qcFile.WriteKeyValues(Me.theMdlFileData.theKeyValuesText, "$KeyValues")
            catch (Exception ex)
            {
                int debug = 4242;
            }
            finally
            {
            }
        }

        protected override AppEnums.StatusMessage WriteMeshSmdFiles(string modelOutputPath, int lodStartIndex, int lodStopIndex)
        {
            var status = AppEnums.StatusMessage.Success;

            // Dim smdFileName As String
            string smdPathFileName;
            SourceVtxBodyPart06 aBodyPart;
            SourceVtxModel06 aVtxModel;
            SourceMdlModel31 aBodyModel;
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
                                        bodyModelName = theMdlFileData.theSequenceGroups[0].theFileName;
                                        if (string.IsNullOrEmpty(bodyModelName) || FileManager.FilePathHasInvalidChars(bodyModelName))
                                        {
                                            bodyModelName = Conversions.ToString(theMdlFileData.theBodyParts[bodyPartIndex].theModels[modelIndex].name);
                                        }

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

        protected override void WritePhysicsMeshSmdFile()
        {
            var physicsMeshSmdFile = new SourceSmdFile31(theOutputFileTextWriter, theMdlFileData, thePhyFileDataGeneric);
            try
            {
                physicsMeshSmdFile.WriteHeaderComment();
                physicsMeshSmdFile.WriteHeaderSection();
                physicsMeshSmdFile.WriteNodesSection(-1);
                physicsMeshSmdFile.WriteSkeletonSection(-1);
                physicsMeshSmdFile.WriteTrianglesSectionForPhysics();
            }
            catch (Exception ex)
            {
                int debug = 4242;
            }
            finally
            {
            }
        }

        protected override void WriteBoneAnimationSmdFile(SourceMdlSequenceDescBase aSequenceDesc, SourceMdlAnimationDescBase anAnimationDesc)
        {
            var smdFile = new SourceSmdFile31(theOutputFileTextWriter, theMdlFileData);
            try
            {
                smdFile.WriteHeaderComment();
                smdFile.WriteHeaderSection();
                smdFile.WriteNodesSection(-1);
                // If Me.theMdlFileData.theFirstAnimationDesc IsNot Nothing AndAlso Me.theMdlFileData.theFirstAnimationDescFrameLines.Count = 0 Then
                // smdFile.CalculateFirstAnimDescFrameLinesForSubtract()
                // End If
                smdFile.WriteSkeletonSectionForAnimation(aSequenceDesc, anAnimationDesc);
            }
            catch (Exception ex)
            {
                int debug = 4242;
            }
        }

        #endregion

        #region Data

        private SourceMdlFileData31 theMdlFileData;
        private SourceVtxFileData06 theVtxFileData;

        #endregion

    }
}