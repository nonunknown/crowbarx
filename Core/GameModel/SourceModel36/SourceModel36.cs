﻿using System;
using System.IO;
using Microsoft.VisualBasic.CompilerServices;

namespace Crowbar
{
    // Inherits SourceModel32
    public class SourceModel36 : SourceModel
    {

        #region Creation and Destruction

        public SourceModel36(string mdlPathFileName, int mdlVersion) : base(mdlPathFileName, mdlVersion)
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

        public override AppEnums.StatusMessage ReadPhyFile()
        {
            var status = AppEnums.StatusMessage.Success;

            // If String.IsNullOrEmpty(Me.thePhyPathFileName) Then
            // status = Me.CheckForRequiredFiles()
            // End If

            if (!string.IsNullOrEmpty(thePhyPathFileName))
            {
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
            }

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

        public AppEnums.StatusMessage WriteMeshSmdFile(string smdPathFileName, int lodIndex, SourceVtxModel06 aVtxModel, SourceMdlModel37 aModel, int bodyPartVertexIndexStart)
        {
            var status = AppEnums.StatusMessage.Success;
            try
            {
                theOutputFileTextWriter = File.CreateText(smdPathFileName);
                var smdFile = new SourceSmdFile36(theOutputFileTextWriter, theMdlFileData);
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

        public override AppEnums.StatusMessage WriteBoneAnimationSmdFiles(string modelOutputPath)
        {
            var status = AppEnums.StatusMessage.Success;
            SourceMdlAnimationDesc36 anAnimationDesc;
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

        public override AppEnums.StatusMessage WriteVertexAnimationVtaFiles(string modelOutputPath)
        {
            var status = AppEnums.StatusMessage.Success;
            SourceMdlBodyPart37 aBodyPart;
            string vtaFileName;
            string vtaPath;
            string vtaPathFileName;
            try
            {
                for (int aBodyPartIndex = 0, loopTo = theMdlFileData.theBodyParts.Count - 1; aBodyPartIndex <= loopTo; aBodyPartIndex++)
                {
                    aBodyPart = theMdlFileData.theBodyParts[aBodyPartIndex];
                    if (aBodyPart.theFlexFrames is null || aBodyPart.theFlexFrames.Count <= 1)
                    {
                        continue;
                    }

                    vtaFileName = SourceFileNamesModule.GetVtaFileName(Name, aBodyPartIndex);
                    vtaPathFileName = Path.Combine(modelOutputPath, vtaFileName);
                    vtaPath = FileManager.GetPath(vtaPathFileName);
                    if (FileManager.PathExistsAfterTryToCreate(vtaPath))
                    {
                        NotifySourceModelProgress(AppEnums.ProgressOptions.WritingFileStarted, vtaPathFileName);
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

                        // Me.WriteVertexAnimationVtaFile(vtaPathFileName, aBodyPart)
                        try
                        {
                            theOutputFileTextWriter = File.CreateText(vtaPathFileName);
                            WriteVertexAnimationVtaFile(null);
                        }
                        catch (PathTooLongException ex)
                        {
                            int debug = 4242;
                        }
                        // status = "ERROR: Crowbar tried to create """ + vtaPathFileName + """ but the system gave this message: " + ex.Message
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
                    }
                }
            }
            catch (Exception ex)
            {
                int debug = 4242;
            }

            return status;
        }

        protected override void WriteVertexAnimationVtaFile(SourceMdlBodyPart bodyPart)
        {
            // Dim vertexAnimationVtaFile As New SourceVtaFile37(Me.theOutputFileTextWriter, Me.theMdlFileData)

            // Try
            // vertexAnimationVtaFile.WriteHeaderComment()

            // vertexAnimationVtaFile.WriteHeaderSection()
            // vertexAnimationVtaFile.WriteNodesSection()
            // vertexAnimationVtaFile.WriteSkeletonSectionForVertexAnimation()
            // vertexAnimationVtaFile.WriteVertexAnimationSection()
            // Catch ex As Exception
            // Dim debug As Integer = 4242
            // Finally
            // End Try
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
                theMdlFileData = new SourceMdlFileData36();
                theMdlFileDataGeneric = theMdlFileData;
            }

            var mdlFile = new SourceMdlFile36(theInputFileReader, theMdlFileData);
            mdlFile.ReadMdlHeader00("MDL File Header 00");
            mdlFile.ReadMdlHeader01("MDL File Header 01");
        }

        protected override void ReadMdlFileForViewer_Internal()
        {
            if (theMdlFileData is null)
            {
                theMdlFileData = new SourceMdlFileData36();
                theMdlFileDataGeneric = theMdlFileData;
            }

            var mdlFile = new SourceMdlFile36(theInputFileReader, theMdlFileData);
            mdlFile.ReadMdlHeader00("MDL File Header 00");
            mdlFile.ReadMdlHeader01("MDL File Header 01");

            // 'mdlFile.ReadTexturePaths()
            // mdlFile.ReadTextures()
        }

        protected override void ReadMdlFile_Internal()
        {
            if (theMdlFileData is null)
            {
                theMdlFileData = new SourceMdlFileData36();
                theMdlFileDataGeneric = theMdlFileData;
            }

            var mdlFile = new SourceMdlFile36(theInputFileReader, theMdlFileData);
            mdlFile.ReadMdlHeader00("MDL File Header 00");
            mdlFile.ReadMdlHeader01("MDL File Header 01");

            // Read what WriteBoneInfo() writes.
            mdlFile.ReadBones();
            mdlFile.ReadBoneControllers();
            mdlFile.ReadAttachments();
            mdlFile.ReadHitboxSets();

            // Read what WriteSequenceInfo() writes.
            // NOTE: Must read sequences before reading animations.
            mdlFile.ReadSequences();
            mdlFile.ReadSequenceGroups();
            mdlFile.ReadTransitions();

            // Read what WriteAnimations() writes.
            mdlFile.ReadLocalAnimationDescs();

            // Read what WriteModel() writes.
            mdlFile.ReadBodyParts();
            mdlFile.ReadFlexDescs();
            mdlFile.ReadFlexControllers();
            // NOTE: This must be after flex descs are read so that flex desc usage can be saved in flex desc.
            mdlFile.ReadFlexRules();
            mdlFile.ReadIkChains();
            mdlFile.ReadIkLocks();
            mdlFile.ReadMouths();
            mdlFile.ReadPoseParamDescs();

            // Read what WriteTextures() writes.
            mdlFile.ReadTexturePaths();
            // NOTE: ReadTextures must be after ReadTexturePaths(), so it can compare with the texture paths.
            mdlFile.ReadTextures();
            mdlFile.ReadSkinFamilies();

            // Read what WriteKeyValues() writes.
            mdlFile.ReadKeyValues();

            // mdlFile.ReadFinalBytesAlignment()
            mdlFile.ReadUnreadBytes();

            // Post-processing.
            // mdlFile.BuildBoneTransforms()
            mdlFile.PostProcess();
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
                theVtxFileData = new SourceVtxFileData06();
            }

            var vtxFile = new SourceVtxFile06(theInputFileReader, theVtxFileData);
            vtxFile.ReadSourceVtxHeader();
            // If Me.theVtxFileData.lodCount > 0 Then
            vtxFile.ReadSourceVtxBodyParts();
            // End If
            vtxFile.ReadSourceVtxMaterialReplacementLists();
        }

        protected override void WriteQcFile()
        {
            var qcFile = new SourceQcFile36(theOutputFileTextWriter, theQcPathFileName, theMdlFileData, thePhyFileDataGeneric, theVtxFileData, theName);
            try
            {
                qcFile.WriteHeaderComment();
                qcFile.WriteModelNameCommand();
                qcFile.WriteStaticPropCommand();

                // If Me.theMdlFileData.theModelCommandIsUsed Then
                // qcFile.WriteModelCommand()
                // qcFile.WriteBodyGroupCommand(1)
                // Else
                // qcFile.WriteBodyGroupCommand(0)
                // End If
                qcFile.WriteBodyGroupCommand();
                qcFile.WriteGroup("lod", qcFile.WriteGroupLod, false, false);
                qcFile.WriteSurfacePropCommand();
                qcFile.WriteJointSurfacePropCommand();
                qcFile.WriteContentsCommand();
                qcFile.WriteJointContentsCommand();
                qcFile.WriteIllumPositionCommand();
                qcFile.WriteEyePositionCommand();
                qcFile.WriteNoForcedFadeCommand();
                qcFile.WriteForcePhonemeCrossfadeCommand();
                qcFile.WriteAmbientBoostCommand();
                qcFile.WriteOpaqueCommand();
                qcFile.WriteObsoleteCommand();
                qcFile.WriteCdMaterialsCommand();
                qcFile.WriteTextureGroupCommand();
                if (Program.TheApp.Settings.DecompileDebugInfoFilesIsChecked)
                {
                    qcFile.WriteTextureFileNameComments();
                }

                qcFile.WriteAttachmentCommand();
                qcFile.WriteGroup("box", qcFile.WriteGroupBox, true, false);
                qcFile.WriteControllerCommand();
                qcFile.WriteScreenAlignCommand();
                qcFile.WriteGroup("bone", qcFile.WriteGroupBone, false, false);
                qcFile.WriteGroup("animation", qcFile.WriteGroupAnimation, false, false);
                qcFile.WriteGroup("collision", qcFile.WriteGroupCollision, false, false);
                string command;
                if (Program.TheApp.Settings.DecompileQcUseMixedCaseForKeywordsIsChecked)
                {
                    command = "$KeyValues";
                }
                else
                {
                    command = "$keyvalues";
                }

                qcFile.WriteKeyValues(theMdlFileData.theKeyValuesText, command);
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
            SourceVtxBodyPart06 aBodyPart;
            SourceVtxModel06 aVtxModel;
            SourceMdlModel37 aBodyModel;
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
            var physicsMeshSmdFile = new SourceSmdFile36(theOutputFileTextWriter, theMdlFileData, thePhyFileDataGeneric);
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
            var smdFile = new SourceSmdFile36(theOutputFileTextWriter, theMdlFileData);
            try
            {
                smdFile.WriteHeaderComment();
                smdFile.WriteHeaderSection();
                smdFile.WriteNodesSection(-1);
                smdFile.WriteSkeletonSectionForAnimation(aSequenceDesc, anAnimationDesc);
            }
            catch (Exception ex)
            {
                int debug = 4242;
            }
        }

        #endregion

        #region Data

        private SourceMdlFileData36 theMdlFileData;
        // Private thePhyFileData As SourcePhyFileData37
        private SourceVtxFileData06 theVtxFileData;

        #endregion

    }
}