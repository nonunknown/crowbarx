using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualBasic.CompilerServices;

namespace Crowbar
{
    public class SourceModel49 : SourceModel37
    {

        #region Creation and Destruction

        public SourceModel49(string mdlPathFileName, int mdlVersion) : base(mdlPathFileName, mdlVersion)
        {
        }

        #endregion

        #region Properties

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

        public override bool AniFileIsUsed
        {
            get
            {
                return !string.IsNullOrEmpty(theAniPathFileName) && File.Exists(theAniPathFileName);
            }
        }

        public override bool VvdFileIsUsed
        {
            get
            {
                return !string.IsNullOrEmpty(theVvdPathFileName) && File.Exists(theVvdPathFileName);
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
                if (theMdlFileData is object && theMdlFileData.theProceduralBonesCommandIsUsed && !theMdlFileData.theMdlFileOnlyHasAnimations && theMdlFileData.theBones is object && theMdlFileData.theBones.Count > 0)



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
            if (theMdlFileData.animBlockCount > 0)
            {
                theAniPathFileName = Path.ChangeExtension(theMdlPathFileName, ".ani");
                if (!File.Exists(theAniPathFileName))
                {
                    status = status | AppEnums.FilesFoundFlags.ErrorRequiredAniFileNotFound;
                }
            }

            // If Not Me.theMdlFileDataGeneric.theMdlFileOnlyHasAnimations Then
            thePhyPathFileName = Path.ChangeExtension(theMdlPathFileName, ".phy");

            // TODO: If the checksum of the vtx does not match checksum in MDL, check the next vtx.
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
                                status = status | AppEnums.FilesFoundFlags.ErrorRequiredVtxFileNotFound;
                            }
                        }
                    }
                }
            }

            theVvdPathFileName = Path.ChangeExtension(theMdlPathFileName, ".vvd");
            if (!File.Exists(theVvdPathFileName))
            {
                status = status | AppEnums.FilesFoundFlags.ErrorRequiredVvdFileNotFound;
            }
            // End If

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

        public override AppEnums.StatusMessage ReadAniFile()
        {
            var status = AppEnums.StatusMessage.Success;

            // If String.IsNullOrEmpty(Me.theAniPathFileName) Then
            // status = Me.CheckForRequiredFiles()
            // End If

            if (!string.IsNullOrEmpty(theAniPathFileName))
            {
                if (status == AppEnums.StatusMessage.Success)
                {
                    try
                    {
                        ReadFile(theAniPathFileName, ReadAniFile_Internal);
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

        public override AppEnums.StatusMessage WriteBoneAnimationSmdFiles(string modelOutputPath)
        {
            var status = AppEnums.StatusMessage.Success;
            string smdPathFileName;
            string smdPath;
            string writeStatus;
            try
            {
                if (theMdlFileData.theCorrectiveAnimationDescs is object)
                {
                    foreach (SourceMdlAnimationDesc49 anAnimationDesc in theMdlFileData.theCorrectiveAnimationDescs)
                    {
                        smdPathFileName = Path.Combine(modelOutputPath, SourceFileNamesModule.CreateCorrectiveAnimationSmdRelativePathFileName(anAnimationDesc.theName, Name));
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

                            writeStatus = "Failed";
                            writeStatus = WriteCorrectiveAnimationSmdFile(smdPathFileName, null, anAnimationDesc);
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

                foreach (SourceMdlAnimationDesc49 anAnimationDesc in theMdlFileData.theAnimationDescs)
                {
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

        public override AppEnums.StatusMessage WriteVrdFile(string vrdPathFileName)
        {
            var status = AppEnums.StatusMessage.Success;
            NotifySourceModelProgress(AppEnums.ProgressOptions.WritingFileStarted, vrdPathFileName);
            WriteTextFile(vrdPathFileName, WriteVrdFile);
            NotifySourceModelProgress(AppEnums.ProgressOptions.WritingFileFinished, vrdPathFileName);
            return status;
        }

        public override AppEnums.StatusMessage WriteDeclareSequenceQciFile(string declareSequenceQciPathFileName)
        {
            var status = AppEnums.StatusMessage.Success;
            NotifySourceModelProgress(AppEnums.ProgressOptions.WritingFileStarted, declareSequenceQciPathFileName);
            WriteTextFile(declareSequenceQciPathFileName, WriteDeclareSequenceQciFile);
            NotifySourceModelProgress(AppEnums.ProgressOptions.WritingFileFinished, declareSequenceQciPathFileName);
            return status;
        }

        public override AppEnums.StatusMessage WriteVertexAnimationVtaFiles(string modelOutputPath)
        {
            var status = AppEnums.StatusMessage.Success;
            SourceMdlBodyPart aBodyPart;
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

                        WriteVertexAnimationVtaFile(vtaPathFileName, aBodyPart);
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

        public override AppEnums.StatusMessage WriteAccessedBytesDebugFiles(string debugPath)
        {
            var status = AppEnums.StatusMessage.Success;
            string debugPathFileName;
            if (theMdlFileDataGeneric is object)
            {
                debugPathFileName = Path.Combine(debugPath, theName + " " + "DEC_LOG");
                NotifySourceModelProgress(AppEnums.ProgressOptions.WritingFileStarted, debugPathFileName);
                WriteAccessedBytesDebugFile(debugPathFileName, theMdlFileDataGeneric.theFileSeekLog);
                NotifySourceModelProgress(AppEnums.ProgressOptions.WritingFileFinished, debugPathFileName);
            }

            if (theAniFileDataGeneric is object)
            {
                debugPathFileName = Path.Combine(debugPath, theName + " " + "ANI_LOG");
                NotifySourceModelProgress(AppEnums.ProgressOptions.WritingFileStarted, debugPathFileName);
                WriteAccessedBytesDebugFile(debugPathFileName, theAniFileDataGeneric.theFileSeekLog);
                NotifySourceModelProgress(AppEnums.ProgressOptions.WritingFileFinished, debugPathFileName);
            }

            if (theVtxFileData is object)
            {
                debugPathFileName = Path.Combine(debugPath, theName + " " + "VTX_LOG");
                NotifySourceModelProgress(AppEnums.ProgressOptions.WritingFileStarted, debugPathFileName);
                WriteAccessedBytesDebugFile(debugPathFileName, theVtxFileData.theFileSeekLog);
                NotifySourceModelProgress(AppEnums.ProgressOptions.WritingFileFinished, debugPathFileName);
            }

            if (theVvdFileData49 is object)
            {
                debugPathFileName = Path.Combine(debugPath, theName + " " + "VVD_LOG");
                NotifySourceModelProgress(AppEnums.ProgressOptions.WritingFileStarted, debugPathFileName);
                WriteAccessedBytesDebugFile(debugPathFileName, theVvdFileData49.theFileSeekLog);
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
                SourceMdlTexture aTexture;
                aTexture = theMdlFileData.theTextures[i];
                textureFileNames.Add(aTexture.thePathFileName);
            }

            return textureFileNames;
        }

        // Public Overrides Function GetSequenceInfo() As List(Of String)
        // Dim sequenceFileNames As New List(Of String)()

        // For i As Integer = 0 To Me.theMdlFileData.theSequenceDescs.Count - 1
        // Dim aSequence As SourceMdlSequenceDesc
        // aSequence = Me.theMdlFileData.theSequenceDescs(i)

        // sequenceFileNames.Add(aSequence.theName)
        // Next

        // Return sequenceFileNames
        // End Function

        #endregion

        #region Private Methods

        protected override void ReadAniFile_Internal()
        {
            if (theAniFileData49 is null)
            {
                // Me.theAniFileData49 = New SourceAniFileData49()
                theAniFileData49 = new SourceMdlFileData49();
                theAniFileDataGeneric = theAniFileData49;
            }

            if (theMdlFileData is null)
            {
                theMdlFileData = new SourceMdlFileData49();
                theMdlFileDataGeneric = theMdlFileData;
            }

            var aniFile = new SourceAniFile49(theInputFileReader, theAniFileData49, theMdlFileData);
            aniFile.ReadMdlHeader00("ANI File Header 00");
            aniFile.ReadMdlHeader01("ANI File Header 01");
            aniFile.ReadAnimationAniBlocks();
            aniFile.ReadUnreadBytes();
        }

        protected override void ReadMdlFile_Internal()
        {
            if (theMdlFileData is null)
            {
                theMdlFileData = new SourceMdlFileData49();
                theMdlFileDataGeneric = theMdlFileData;
            }

            var mdlFile = new SourceMdlFile49(theInputFileReader, theMdlFileData);
            theMdlFileData.theSectionFrameCount = 0;
            // Me.theMdlFileData.theModelCommandIsUsed = False
            theMdlFileData.theProceduralBonesCommandIsUsed = false;
            theMdlFileData.theAnimBlockSizeNoStallOptionIsUsed = false;
            mdlFile.ReadMdlHeader00("MDL File Header 00");
            mdlFile.ReadMdlHeader01("MDL File Header 01");
            if (theMdlFileData.studioHeader2Offset > 0)
            {
                mdlFile.ReadMdlHeader02("MDL File Header 02");
            }

            // Read what WriteBoneInfo() writes.
            mdlFile.ReadBones();
            mdlFile.ReadBoneControllers();
            mdlFile.ReadAttachments();
            mdlFile.ReadHitboxSets();
            mdlFile.ReadBoneTableByName();

            // Read what WriteAnimations() writes.
            if (theMdlFileData.localAnimationCount > 0)
            {
                try
                {
                    mdlFile.ReadLocalAnimationDescs();
                    mdlFile.ReadAnimationSections();
                    mdlFile.ReadAnimationMdlBlocks();
                }
                catch (Exception ex)
                {
                    int debug = 4242;
                }
            }

            // Read what WriteSequenceInfo() writes.
            mdlFile.ReadSequenceDescs();
            mdlFile.ReadLocalNodeNames();
            mdlFile.ReadLocalNodes();

            // Read what WriteModel() writes.
            // Me.theCurrentFrameIndex = 0
            // NOTE: Read flex descs before body parts so that flexes (within body parts) can add info to flex descs.
            mdlFile.ReadFlexDescs();
            mdlFile.ReadBodyParts();
            mdlFile.ReadFlexControllers();
            // NOTE: This must be after flex descs are read so that flex desc usage can be saved in flex desc.
            mdlFile.ReadFlexRules();
            mdlFile.ReadIkChains();
            mdlFile.ReadIkLocks();
            mdlFile.ReadMouths();
            mdlFile.ReadPoseParamDescs();
            mdlFile.ReadModelGroups();
            // TODO: Me.ReadAnimBlocks()
            // TODO: Me.ReadAnimBlockName()

            // Read what WriteTextures() writes.
            mdlFile.ReadTexturePaths();
            // NOTE: ReadTextures must be after ReadTexturePaths(), so it can compare with the texture paths.
            mdlFile.ReadTextures();
            mdlFile.ReadSkinFamilies();

            // Read what WriteKeyValues() writes.
            mdlFile.ReadKeyValues();

            // Read what WriteBoneTransforms() writes.
            mdlFile.ReadBoneTransforms();
            mdlFile.ReadLinearBoneTable();

            // TODO: ReadLocalIkAutoPlayLocks()
            mdlFile.ReadFlexControllerUis();

            // mdlFile.ReadFinalBytesAlignment()
            // mdlFile.ReadUnknownValues(Me.theMdlFileData.theFileSeekLog)
            mdlFile.ReadUnreadBytes();

            // Post-processing.
            mdlFile.PostProcess();
            mdlFile.CreateFlexFrameList();
            Common.ProcessTexturePaths(theMdlFileData.theTexturePaths, theMdlFileData.theTextures, theMdlFileData.theModifiedTexturePaths, theMdlFileData.theModifiedTextureFileNames);
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
                theVtxFileData = new SourceVtxFileData07();
            }

            // TEST: When a model has a nameCopy, it seems to also use the VTF file strip group topology fields.
            var vtxFile = new SourceVtxFile07(theInputFileReader, theVtxFileData);
            vtxFile.ReadSourceVtxHeader();
            // TODO: Why is this "If" statement needed?
            if (theVtxFileData.lodCount > 0)
            {
                vtxFile.ReadSourceVtxBodyParts();
            }

            vtxFile.ReadSourceVtxMaterialReplacementLists();
            vtxFile.ReadUnreadBytes();
        }

        protected override void ReadVvdFile_Internal()
        {
            if (theVvdFileData49 is null)
            {
                theVvdFileData49 = new SourceVvdFileData04();
            }

            var vvdFile = new SourceVvdFile04(theInputFileReader, theVvdFileData49);
            vvdFile.ReadSourceVvdHeader();
            vvdFile.ReadVertexes(theMdlFileData.version);
            vvdFile.ReadFixups();
            vvdFile.ReadUnreadBytes();
        }

        protected override void WriteQcFile()
        {
            // Dim qcFile As New SourceQcFile49(Me.theOutputFileTextWriter, Me.theQcPathFileName, Me.theMdlFileData, Me.theVtxFileData, Me.thePhyFileDataGeneric, Me.theAniFileData49, Me.theName)
            var qcFile = new SourceQcFile49(theOutputFileTextWriter, theQcPathFileName, theMdlFileData, theVtxFileData, thePhyFileDataGeneric, theName);
            try
            {
                qcFile.WriteHeaderComment();
                qcFile.WriteModelNameCommand();
                qcFile.WriteUpAxisCommand();
                qcFile.WriteStaticPropCommand();
                qcFile.WriteConstantDirectionalLightCommand();

                // If Me.theMdlFileData.theModelCommandIsUsed Then
                // qcFile.WriteModelCommand()
                // qcFile.WriteBodyGroupCommand(1)
                // Else
                // qcFile.WriteBodyGroupCommand(0)
                // End If
                // qcFile.WriteModelCommand()
                qcFile.WriteBodyGroupCommand();
                qcFile.WriteGroup("lod", qcFile.WriteGroupLod, false, false);
                qcFile.WriteSurfacePropCommand();
                qcFile.WriteJointSurfacePropCommand();
                qcFile.WriteContentsCommand();
                qcFile.WriteJointContentsCommand();
                qcFile.WriteIllumPositionCommand();
                qcFile.WriteEyePositionCommand();
                qcFile.WriteMaxEyeDeflectionCommand();
                qcFile.WriteNoForcedFadeCommand();
                qcFile.WriteForcePhonemeCrossfadeCommand();
                qcFile.WriteAmbientBoostCommand();
                qcFile.WriteOpaqueCommand();
                qcFile.WriteObsoleteCommand();
                qcFile.WriteCastTextureShadowsCommand();
                qcFile.WriteDoNotCastShadowsCommand();
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
                SetUpCorrectiveSubtractAnimationBlocks();
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

        protected override void ReadMdlFileHeader_Internal()
        {
            if (theMdlFileData is null)
            {
                theMdlFileData = new SourceMdlFileData49();
                theMdlFileDataGeneric = theMdlFileData;
            }

            var mdlFile = new SourceMdlFile49(theInputFileReader, theMdlFileData);
            mdlFile.ReadMdlHeader00("MDL File Header 00");
            mdlFile.ReadMdlHeader01("MDL File Header 01");
            if (theMdlFileData.studioHeader2Offset > 0)
            {
                mdlFile.ReadMdlHeader02("MDL File Header 02");
            }

            // If Me.theMdlFileData.fileSize <> Me.theMdlFileData.theActualFileSize Then
            // status = StatusMessage.ErrorInvalidInternalMdlFileSize
            // End If
        }

        protected override void ReadMdlFileForViewer_Internal()
        {
            if (theMdlFileData is null)
            {
                theMdlFileData = new SourceMdlFileData49();
                theMdlFileDataGeneric = theMdlFileData;
            }

            var mdlFile = new SourceMdlFile49(theInputFileReader, theMdlFileData);
            mdlFile.ReadMdlHeader00("MDL File Header 00");
            mdlFile.ReadMdlHeader01("MDL File Header 01");
            if (theMdlFileData.studioHeader2Offset > 0)
            {
                mdlFile.ReadMdlHeader02("MDL File Header 02");
            }

            mdlFile.ReadTexturePaths();
            mdlFile.ReadTextures();
            mdlFile.ReadSequenceDescs();
        }

        protected override AppEnums.StatusMessage WriteMeshSmdFiles(string modelOutputPath, int lodStartIndex, int lodStopIndex)
        {
            var status = AppEnums.StatusMessage.Success;
            string smdFileName;
            string smdPathFileName;
            SourceVtxBodyPart07 aVtxBodyPart;
            SourceVtxModel07 aVtxBodyModel;
            SourceMdlModel aBodyModel;
            int bodyPartVertexIndexStart;
            bodyPartVertexIndexStart = 0;
            if (theVtxFileData.theVtxBodyParts is object && theMdlFileData.theBodyParts is object)
            {
                for (int bodyPartIndex = 0, loopTo = theVtxFileData.theVtxBodyParts.Count - 1; bodyPartIndex <= loopTo; bodyPartIndex++)
                {
                    aVtxBodyPart = theVtxFileData.theVtxBodyParts[bodyPartIndex];
                    if (aVtxBodyPart.theVtxModels is object)
                    {
                        for (int modelIndex = 0, loopTo1 = aVtxBodyPart.theVtxModels.Count - 1; modelIndex <= loopTo1; modelIndex++)
                        {
                            aVtxBodyModel = aVtxBodyPart.theVtxModels[modelIndex];
                            if (aVtxBodyModel.theVtxModelLods is object)
                            {
                                aBodyModel = theMdlFileData.theBodyParts[bodyPartIndex].theModels[modelIndex];
                                if (aBodyModel.name[0] == '\0' && aVtxBodyModel.theVtxModelLods[0].theVtxMeshes is null)
                                {
                                    continue;
                                }

                                for (int lodIndex = lodStartIndex, loopTo2 = lodStopIndex; lodIndex <= loopTo2; lodIndex++)
                                {
                                    // TODO: Why would this count be different than the file header count?
                                    if (lodIndex >= aVtxBodyModel.theVtxModelLods.Count)
                                    {
                                        break;
                                    }

                                    smdFileName = SourceFileNamesModule.CreateBodyGroupSmdFileName(aBodyModel.theSmdFileNames[lodIndex], bodyPartIndex, modelIndex, lodIndex, theName, Conversions.ToString(theMdlFileData.theBodyParts[bodyPartIndex].theModels[modelIndex].name));
                                    smdPathFileName = Path.Combine(modelOutputPath, smdFileName);
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

                                    WriteMeshSmdFile(smdPathFileName, lodIndex, aVtxBodyModel, aBodyModel, bodyPartVertexIndexStart);
                                    NotifySourceModelProgress(AppEnums.ProgressOptions.WritingFileFinished, smdPathFileName);
                                }

                                bodyPartVertexIndexStart += aBodyModel.vertexCount;
                            }
                        }
                    }
                }
            }

            return status;
        }

        protected override void WriteMeshSmdFile(int lodIndex, SourceVtxModel07 aVtxModel, SourceMdlModel aModel, int bodyPartVertexIndexStart)
        {
            var smdFile = new SourceSmdFile49(theOutputFileTextWriter, theMdlFileData, theVvdFileData49);
            try
            {
                smdFile.WriteHeaderComment();
                smdFile.WriteHeaderSection();
                smdFile.WriteNodesSection();
                smdFile.WriteSkeletonSection(lodIndex);
                smdFile.WriteTrianglesSection(aVtxModel, lodIndex, aModel, bodyPartVertexIndexStart);
            }
            catch (Exception ex)
            {
                int debug = 4242;
            }
        }

        protected override void WritePhysicsMeshSmdFile()
        {
            var physicsMeshSmdFile = new SourceSmdFile49(theOutputFileTextWriter, theMdlFileData, thePhyFileDataGeneric);
            try
            {
                physicsMeshSmdFile.WriteHeaderComment();
                physicsMeshSmdFile.WriteHeaderSection();
                physicsMeshSmdFile.WriteNodesSection();
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

        protected override void WriteVrdFile()
        {
            var vrdFile = new SourceVrdFile49(theOutputFileTextWriter, theMdlFileData);
            try
            {
                vrdFile.WriteHeaderComment();
                vrdFile.WriteCommands();
            }
            catch (Exception ex)
            {
                int debug = 4242;
            }
            finally
            {
            }
        }

        protected override void WriteDeclareSequenceQciFile()
        {
            var qciFile = new SourceQcFile49(theOutputFileTextWriter, theMdlFileData, theName);
            try
            {
                qciFile.WriteHeaderComment();
                qciFile.WriteQciDeclareSequenceLines();
            }
            catch (Exception ex)
            {
                int debug = 4242;
            }
        }

        protected override void WriteBoneAnimationSmdFile(SourceMdlSequenceDescBase aSequenceDesc, SourceMdlAnimationDescBase anAnimationDesc)
        {
            var smdFile = new SourceSmdFile49(theOutputFileTextWriter, theMdlFileData);
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
            var vertexAnimationVtaFile = new SourceVtaFile49(theOutputFileTextWriter, theMdlFileData, theVvdFileData49, bodyPart);
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
            var mdlFile = new SourceMdlFile49(theOutputFileBinaryWriter, theMdlFileData);
            mdlFile.WriteInternalMdlFileName(internalMdlFileName);
            mdlFile.WriteInternalMdlFileNameCopy(internalMdlFileName);
        }

        protected override void WriteAniFileNameToMdlFile(string internalAniFileName)
        {
            var mdlFile = new SourceMdlFile49(theOutputFileBinaryWriter, theMdlFileData);
            mdlFile.WriteInternalAniFileName(internalAniFileName);
        }

        private void SetUpCorrectiveSubtractAnimationBlocks()
        {
            if (theMdlFileData.theSequenceDescs is object)
            {
                SourceMdlAnimationDesc49 anAnimationDesc;
                string name;
                theMdlFileData.theCorrectiveAnimationDescs = new List<SourceMdlAnimationDesc49>();
                foreach (SourceMdlSequenceDesc aSequenceDesc in theMdlFileData.theSequenceDescs)
                {
                    if ((aSequenceDesc.flags & SourceMdlAnimationDesc.STUDIO_DELTA) > 0)
                    {
                        if (aSequenceDesc.theAnimDescIndexes is object && aSequenceDesc.theAnimDescIndexes.Count > 0)
                        {
                            for (int j = 0, loopTo = aSequenceDesc.theAnimDescIndexes.Count - 1; j <= loopTo; j++)
                            {
                                anAnimationDesc = theMdlFileData.theAnimationDescs[aSequenceDesc.theAnimDescIndexes[j]];
                                name = anAnimationDesc.theName;
                                if (Conversions.ToString(name[0]) == "@")
                                {
                                    // NOTE: There should only be one implied anim desc.
                                    // aSequenceDesc.theCorrectiveSubtractAnimationOptionIsUsed = True
                                    aSequenceDesc.theCorrectiveAnimationName = SourceFileNamesModule.CreateCorrectiveAnimationName(name);
                                }
                                else
                                {
                                    // anAnimationDesc.theCorrectiveSubtractAnimationOptionIsUsed = True
                                    anAnimationDesc.theCorrectiveAnimationName = SourceFileNamesModule.CreateCorrectiveAnimationName(name);
                                }

                                if (!theMdlFileData.theCorrectiveAnimationDescs.Contains(anAnimationDesc))
                                {
                                    theMdlFileData.theCorrectiveAnimationDescs.Add(anAnimationDesc);
                                }
                            }
                        }
                    }
                }
            }
        }

        private string WriteCorrectiveAnimationSmdFile(string smdPathFileName, SourceMdlSequenceDescBase aSequenceDesc, SourceMdlAnimationDescBase anAnimationDesc)
        {
            string status = "Success";
            try
            {
                theOutputFileTextWriter = File.CreateText(smdPathFileName);
                var smdFile = new SourceSmdFile49(theOutputFileTextWriter, theMdlFileData);
                try
                {
                    smdFile.WriteHeaderComment();
                    smdFile.WriteHeaderSection();
                    smdFile.WriteNodesSection();
                    smdFile.WriteSkeletonSectionForAnimation(aSequenceDesc, anAnimationDesc, true);
                }
                catch (Exception ex)
                {
                    int debug = 4242;
                }
            }
            catch (PathTooLongException ex)
            {
                status = "ERROR: Crowbar tried to create \"" + smdPathFileName + "\" but the system gave this message: " + ex.Message;
            }
            catch (Exception ex)
            {
                int debug = 4242;
            }
            finally
            {
                if (theOutputFileTextWriter is object)
                {
                    if (theOutputFileTextWriter.BaseStream is object)
                    {
                        theOutputFileTextWriter.Flush();
                    }

                    theOutputFileTextWriter.Close();
                }
            }

            return status;
        }

        #endregion

        #region Data

        // Private theAniFileData49 As SourceAniFileData49
        private SourceMdlFileData49 theAniFileData49;
        private SourceMdlFileData49 theMdlFileData;
        // Private thePhyFileData49 As SourcePhyFileData
        private SourceVtxFileData07 theVtxFileData;
        private SourceVvdFileData04 theVvdFileData49;

        // Private theCorrectiveAnimationDescs As List(Of SourceMdlAnimationDesc49)

        #endregion

    }
}