using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualBasic.CompilerServices;

namespace Crowbar
{
    public class SourceMdlFile06
    {

        #region Creation and Destruction

        public SourceMdlFile06(BinaryReader mdlFileReader, SourceMdlFileData06 mdlFileData)
        {
            theInputFileReader = mdlFileReader;
            theMdlFileData = mdlFileData;
            theMdlFileData.theFileSeekLog.FileSize = theInputFileReader.BaseStream.Length;
        }

        public SourceMdlFile06(BinaryWriter mdlFileWriter, SourceMdlFileData06 mdlFileData)
        {
            theOutputFileWriter = mdlFileWriter;
            theMdlFileData = mdlFileData;
        }

        #endregion

        #region Methods

        public void ReadMdlHeader()
        {
            // Dim inputFileStreamPosition As Long
            long fileOffsetStart;
            long fileOffsetEnd;
            // Dim fileOffsetStart2 As Long
            // Dim fileOffsetEnd2 As Long

            fileOffsetStart = theInputFileReader.BaseStream.Position;
            theMdlFileData.id = theInputFileReader.ReadChars(4);
            theMdlFileData.theID = Conversions.ToString(theMdlFileData.id);
            theMdlFileData.version = theInputFileReader.ReadInt32();
            theMdlFileData.name = theInputFileReader.ReadChars(64);
            theMdlFileData.theModelName = Conversions.ToString(theMdlFileData.name).Trim('\0');
            theMdlFileData.fileSize = theInputFileReader.ReadInt32();
            theMdlFileData.theActualFileSize = theInputFileReader.BaseStream.Length;
            theMdlFileData.boneCount = theInputFileReader.ReadInt32();
            theMdlFileData.boneOffset = theInputFileReader.ReadInt32();
            theMdlFileData.boneControllerCount = theInputFileReader.ReadInt32();
            theMdlFileData.boneControllerOffset = theInputFileReader.ReadInt32();
            theMdlFileData.sequenceCount = theInputFileReader.ReadInt32();
            theMdlFileData.sequenceOffset = theInputFileReader.ReadInt32();
            theMdlFileData.textureCount = theInputFileReader.ReadInt32();
            theMdlFileData.textureOffset = theInputFileReader.ReadInt32();
            theMdlFileData.textureDataOffset = theInputFileReader.ReadInt32();
            theMdlFileData.skinReferenceCount = theInputFileReader.ReadInt32();
            theMdlFileData.skinFamilyCount = theInputFileReader.ReadInt32();
            theMdlFileData.skinOffset = theInputFileReader.ReadInt32();
            theMdlFileData.bodyPartCount = theInputFileReader.ReadInt32();
            theMdlFileData.bodyPartOffset = theInputFileReader.ReadInt32();
            for (int x = 0, loopTo = theMdlFileData.unused.Length - 1; x <= loopTo; x++)
                theMdlFileData.unused[x] = theInputFileReader.ReadInt32();
            fileOffsetEnd = theInputFileReader.BaseStream.Position - 1L;
            theMdlFileData.theFileSeekLog.Add(fileOffsetStart, fileOffsetEnd, "MDL File Header");
        }

        public void ReadBones()
        {
            if (theMdlFileData.boneCount > 0)
            {
                // Dim boneInputFileStreamPosition As Long
                // Dim inputFileStreamPosition As Long
                long fileOffsetStart;
                long fileOffsetEnd;
                // Dim fileOffsetStart2 As Long
                // Dim fileOffsetEnd2 As Long

                try
                {
                    theInputFileReader.BaseStream.Seek(theMdlFileData.boneOffset, SeekOrigin.Begin);
                    fileOffsetStart = theInputFileReader.BaseStream.Position;
                    theMdlFileData.theBones = new List<SourceMdlBone06>(theMdlFileData.boneCount);
                    for (int boneIndex = 0, loopTo = theMdlFileData.boneCount - 1; boneIndex <= loopTo; boneIndex++)
                    {
                        // boneInputFileStreamPosition = Me.theInputFileReader.BaseStream.Position
                        var aBone = new SourceMdlBone06();
                        aBone.name = theInputFileReader.ReadChars(32);
                        aBone.theName = Conversions.ToString(aBone.name);
                        aBone.theName = StringClass.ConvertFromNullTerminatedOrFullLengthString(aBone.theName);
                        if (string.IsNullOrEmpty(aBone.theName))
                        {
                            aBone.theName = "unnamed_bone_" + boneIndex.ToString("000");
                        }

                        aBone.parentBoneIndex = theInputFileReader.ReadInt32();
                        aBone.position.x = theInputFileReader.ReadSingle();
                        aBone.position.y = theInputFileReader.ReadSingle();
                        aBone.position.z = theInputFileReader.ReadSingle();
                        aBone.rotation.x = theInputFileReader.ReadSingle();
                        aBone.rotation.y = theInputFileReader.ReadSingle();
                        aBone.rotation.z = theInputFileReader.ReadSingle();
                        theMdlFileData.theBones.Add(aBone);

                        // inputFileStreamPosition = Me.theInputFileReader.BaseStream.Position

                        // Me.theInputFileReader.BaseStream.Seek(inputFileStreamPosition, SeekOrigin.Begin)
                    }

                    fileOffsetEnd = theInputFileReader.BaseStream.Position - 1L;
                    theMdlFileData.theFileSeekLog.Add(fileOffsetStart, fileOffsetEnd, "theMdlFileData.theBones " + theMdlFileData.theBones.Count.ToString());
                }

                // Me.theMdlFileData.theFileSeekLog.LogToEndAndAlignToNextStart(Me.theInputFileReader, fileOffsetEnd, 4, "theMdlFileData.theBones alignment")
                catch (Exception ex)
                {
                    int debug = 4242;
                }
            }
        }

        public void ReadBoneControllers()
        {
            if (theMdlFileData.boneControllerCount > 0)
            {
                // Dim boneControllerInputFileStreamPosition As Long
                // Dim inputFileStreamPosition As Long
                long fileOffsetStart;
                long fileOffsetEnd;
                // Dim fileOffsetStart2 As Long
                // Dim fileOffsetEnd2 As Long

                try
                {
                    theInputFileReader.BaseStream.Seek(theMdlFileData.boneControllerOffset, SeekOrigin.Begin);
                    fileOffsetStart = theInputFileReader.BaseStream.Position;
                    theMdlFileData.theBoneControllers = new List<SourceMdlBoneController06>(theMdlFileData.boneControllerCount);
                    for (int boneControllerIndex = 0, loopTo = theMdlFileData.boneControllerCount - 1; boneControllerIndex <= loopTo; boneControllerIndex++)
                    {
                        // boneControllerInputFileStreamPosition = Me.theInputFileReader.BaseStream.Position
                        var aBoneController = new SourceMdlBoneController06();
                        aBoneController.boneIndex = theInputFileReader.ReadInt32();
                        aBoneController.type = theInputFileReader.ReadInt32();
                        aBoneController.startAngleDegrees = theInputFileReader.ReadSingle();
                        aBoneController.endAngleDegrees = theInputFileReader.ReadSingle();
                        theMdlFileData.theBoneControllers.Add(aBoneController);

                        // inputFileStreamPosition = Me.theInputFileReader.BaseStream.Position

                        // Me.theInputFileReader.BaseStream.Seek(inputFileStreamPosition, SeekOrigin.Begin)
                    }

                    fileOffsetEnd = theInputFileReader.BaseStream.Position - 1L;
                    theMdlFileData.theFileSeekLog.Add(fileOffsetStart, fileOffsetEnd, "theMdlFileData.theBoneControllers " + theMdlFileData.theBoneControllers.Count.ToString());
                }

                // Me.theMdlFileData.theFileSeekLog.LogToEndAndAlignToNextStart(Me.theInputFileReader, fileOffsetEnd, 4, "theMdlFileData.theBoneControllers alignment")
                catch (Exception ex)
                {
                    int debug = 4242;
                }
            }
        }

        public void ReadSequences()
        {
            if (theMdlFileData.sequenceCount > 0)
            {
                // Dim sequenceInputFileStreamPosition As Long
                long inputFileStreamPosition;
                long fileOffsetStart;
                long fileOffsetEnd;
                // Dim fileOffsetStart2 As Long
                // Dim fileOffsetEnd2 As Long

                try
                {
                    theInputFileReader.BaseStream.Seek(theMdlFileData.sequenceOffset, SeekOrigin.Begin);
                    theMdlFileData.theSequences = new List<SourceMdlSequenceDesc06>(theMdlFileData.sequenceCount);
                    for (int sequenceIndex = 0, loopTo = theMdlFileData.sequenceCount - 1; sequenceIndex <= loopTo; sequenceIndex++)
                    {
                        var aSequence = new SourceMdlSequenceDesc06();
                        fileOffsetStart = theInputFileReader.BaseStream.Position;
                        aSequence.name = theInputFileReader.ReadChars(32);
                        aSequence.theName = Conversions.ToString(aSequence.name);
                        aSequence.theName = StringClass.ConvertFromNullTerminatedOrFullLengthString(aSequence.theName);
                        aSequence.fps = theInputFileReader.ReadSingle();
                        aSequence.flags = theInputFileReader.ReadInt32();
                        aSequence.eventCount = theInputFileReader.ReadInt32();
                        aSequence.eventOffset = theInputFileReader.ReadInt32();
                        aSequence.frameCount = theInputFileReader.ReadInt32();
                        aSequence.unused01 = theInputFileReader.ReadInt32();
                        aSequence.pivotCount = theInputFileReader.ReadInt32();
                        aSequence.pivotOffset = theInputFileReader.ReadInt32();
                        aSequence.motiontype = theInputFileReader.ReadInt32();
                        aSequence.motionbone = theInputFileReader.ReadInt32();
                        aSequence.unused02 = theInputFileReader.ReadInt32();
                        aSequence.linearmovement.x = theInputFileReader.ReadSingle();
                        aSequence.linearmovement.y = theInputFileReader.ReadSingle();
                        aSequence.linearmovement.z = theInputFileReader.ReadSingle();
                        aSequence.blendCount = theInputFileReader.ReadInt32();
                        aSequence.animOffset = theInputFileReader.ReadInt32();
                        for (int x = 0, loopTo1 = aSequence.unused03.Length - 1; x <= loopTo1; x++)
                            aSequence.unused03[x] = theInputFileReader.ReadInt32();
                        theMdlFileData.theSequences.Add(aSequence);
                        fileOffsetEnd = theInputFileReader.BaseStream.Position - 1L;
                        theMdlFileData.theFileSeekLog.Add(fileOffsetStart, fileOffsetEnd, "aSequence [" + aSequence.theName + "]");
                        inputFileStreamPosition = theInputFileReader.BaseStream.Position;
                        ReadEvents(aSequence);
                        // Me.theMdlFileData.theFileSeekLog.LogToEndAndAlignToNextStart(Me.theInputFileReader, fileOffsetEnd, 4, "aSequence.theEvents alignment")

                        ReadPivots(aSequence);
                        // Me.theMdlFileData.theFileSeekLog.LogToEndAndAlignToNextStart(Me.theInputFileReader, fileOffsetEnd, 4, "aSequence.thePivots alignment")

                        theInputFileReader.BaseStream.Seek(inputFileStreamPosition, SeekOrigin.Begin);
                    }
                }
                catch (Exception ex)
                {
                    int debug = 4242;
                }
            }
        }

        public void ReadAnimations()
        {
            if (theMdlFileData.theSequences is object)
            {
                long inputFileStreamPosition;
                long fileOffsetStart;
                long fileOffsetEnd;
                // Dim fileOffsetStart2 As Long
                // Dim fileOffsetEnd2 As Long
                SourceMdlSequenceDesc06 aSequence;
                // Dim previousFrameIndex As Integer

                try
                {
                    for (int sequenceIndex = 0, loopTo = theMdlFileData.theSequences.Count - 1; sequenceIndex <= loopTo; sequenceIndex++)
                    {
                        aSequence = theMdlFileData.theSequences[sequenceIndex];
                        theInputFileReader.BaseStream.Seek(aSequence.animOffset, SeekOrigin.Begin);
                        fileOffsetStart = theInputFileReader.BaseStream.Position;
                        aSequence.theAnimations = new List<SourceMdlAnimation06>(aSequence.frameCount * theMdlFileData.theBones.Count);
                        // For frameIndex As Integer = 0 To aSequence.frameCount - 1
                        // While True
                        for (int boneIndex = 0, loopTo1 = theMdlFileData.theBones.Count - 1; boneIndex <= loopTo1; boneIndex++)
                        {
                            fileOffsetStart = theInputFileReader.BaseStream.Position;
                            var anAnimation = new SourceMdlAnimation06();
                            anAnimation.bonePositionCount = theInputFileReader.ReadInt32();
                            anAnimation.bonePositionOffset = theInputFileReader.ReadInt32();
                            anAnimation.boneRotationCount = theInputFileReader.ReadInt32();
                            anAnimation.boneRotationOffset = theInputFileReader.ReadInt32();
                            aSequence.theAnimations.Add(anAnimation);
                            fileOffsetEnd = theInputFileReader.BaseStream.Position - 1L;
                            theMdlFileData.theFileSeekLog.Add(fileOffsetStart, fileOffsetEnd, "anAnimation");
                            inputFileStreamPosition = theInputFileReader.BaseStream.Position;
                            anAnimation.theBonePositionsAndRotations = new List<SourceBonePostionAndRotation06>(aSequence.frameCount);
                            for (int frameIndex = 0, loopTo2 = aSequence.frameCount - 1; frameIndex <= loopTo2; frameIndex++)
                            {
                                var bonePositionAndRotation = new SourceBonePostionAndRotation06();
                                anAnimation.theBonePositionsAndRotations.Add(bonePositionAndRotation);
                            }

                            ReadAnimationBonePositions(anAnimation);
                            ReadAnimationBoneRotations(anAnimation);
                            theInputFileReader.BaseStream.Seek(inputFileStreamPosition, SeekOrigin.Begin);

                            // If anAnimation.theBonePositions.Count > 0 AndAlso anAnimation.theBonePositions(boneIndex).frameIndex < previousFrameIndex Then

                            // End If
                        }
                        // Next
                        // End While
                    }
                }
                catch (Exception ex)
                {
                    int debug = 4242;
                }
            }
        }

        public void ReadBodyParts()
        {
            if (theMdlFileData.bodyPartCount > 0)
            {
                long bodyPartInputFileStreamPosition;
                long inputFileStreamPosition;
                long fileOffsetStart;
                long fileOffsetEnd;
                // Dim fileOffsetStart2 As Long
                // Dim fileOffsetEnd2 As Long
                long modelsEndInputFileStreamPosition;
                try
                {
                    theInputFileReader.BaseStream.Seek(theMdlFileData.bodyPartOffset, SeekOrigin.Begin);
                    // fileOffsetStart = Me.theInputFileReader.BaseStream.Position
                    modelsEndInputFileStreamPosition = 0L;
                    theMdlFileData.theBodyParts = new List<SourceMdlBodyPart06>(theMdlFileData.bodyPartCount);
                    for (int bodyPartIndex = 0, loopTo = theMdlFileData.bodyPartCount - 1; bodyPartIndex <= loopTo; bodyPartIndex++)
                    {
                        fileOffsetStart = theInputFileReader.BaseStream.Position;
                        bodyPartInputFileStreamPosition = theInputFileReader.BaseStream.Position;
                        var aBodyPart = new SourceMdlBodyPart06();
                        aBodyPart.name = theInputFileReader.ReadChars(64);
                        aBodyPart.theName = Conversions.ToString(aBodyPart.name).Trim('\0');
                        aBodyPart.modelCount = theInputFileReader.ReadInt32();
                        aBodyPart.@base = theInputFileReader.ReadInt32();
                        aBodyPart.modelOffset = theInputFileReader.ReadInt32();
                        theMdlFileData.theBodyParts.Add(aBodyPart);
                        inputFileStreamPosition = theInputFileReader.BaseStream.Position;
                        ReadModels(aBodyPart);
                        // If bodyPartIndex = Me.theMdlFileData.bodyPartCount - 1 Then
                        // modelsEndInputFileStreamPosition = Me.theInputFileReader.BaseStream.Position
                        // End If

                        theInputFileReader.BaseStream.Seek(inputFileStreamPosition, SeekOrigin.Begin);
                        fileOffsetEnd = theInputFileReader.BaseStream.Position - 1L;
                        theMdlFileData.theFileSeekLog.Add(fileOffsetStart, fileOffsetEnd, "aBodyPart [" + aBodyPart.theName + "]");
                    }
                }

                // If modelsEndInputFileStreamPosition > 0 Then
                // inputFileStreamPosition = Me.theInputFileReader.BaseStream.Position

                // Me.theMdlFileData.theFileSeekLog.LogToEndAndAlignToNextStart(Me.theInputFileReader, modelsEndInputFileStreamPosition - 1, 4, "theMdlFileData.theBodyParts - End of Models alignment")

                // Me.theInputFileReader.BaseStream.Seek(inputFileStreamPosition, SeekOrigin.Begin)
                // End If

                // fileOffsetEnd = Me.theInputFileReader.BaseStream.Position - 1
                // Me.theMdlFileData.theFileSeekLog.Add(fileOffsetStart, fileOffsetEnd, "theMdlFileData.theBodyParts")
                catch (Exception ex)
                {
                    int debug = 4242;
                }
            }
        }

        public void ReadTextures()
        {
            if (theMdlFileData.textureCount > 0)
            {
                // Dim boneInputFileStreamPosition As Long
                long inputFileStreamPosition;
                long fileOffsetStart;
                long fileOffsetEnd;
                // Dim fileOffsetStart2 As Long
                // Dim fileOffsetEnd2 As Long

                try
                {
                    theInputFileReader.BaseStream.Seek(theMdlFileData.textureOffset, SeekOrigin.Begin);
                    theMdlFileData.theTextures = new List<SourceMdlTexture06>(theMdlFileData.textureCount);
                    for (int textureIndex = 0, loopTo = theMdlFileData.textureCount - 1; textureIndex <= loopTo; textureIndex++)
                    {
                        fileOffsetStart = theInputFileReader.BaseStream.Position;
                        // boneInputFileStreamPosition = Me.theInputFileReader.BaseStream.Position
                        var aTexture = new SourceMdlTexture06();
                        aTexture.fileName = theInputFileReader.ReadChars(64);
                        aTexture.theFileName = Conversions.ToString(aTexture.fileName).Trim('\0');
                        aTexture.flags = theInputFileReader.ReadInt32();
                        aTexture.width = theInputFileReader.ReadUInt32();
                        aTexture.height = theInputFileReader.ReadUInt32();
                        aTexture.dataOffset = theInputFileReader.ReadUInt32();
                        theMdlFileData.theTextures.Add(aTexture);
                        fileOffsetEnd = theInputFileReader.BaseStream.Position - 1L;
                        theMdlFileData.theFileSeekLog.Add(fileOffsetStart, fileOffsetEnd, "aTexture [" + aTexture.theFileName + "]");
                        inputFileStreamPosition = theInputFileReader.BaseStream.Position;
                        ReadTextureData(aTexture);
                        // fileOffsetEnd = Me.theInputFileReader.BaseStream.Position - 1
                        // Me.theMdlFileData.theFileSeekLog.LogToEndAndAlignToNextStart(Me.theInputFileReader, fileOffsetEnd, 4, "aTexture.theData alignment")

                        theInputFileReader.BaseStream.Seek(inputFileStreamPosition, SeekOrigin.Begin);
                    }
                }

                // fileOffsetEnd = Me.theInputFileReader.BaseStream.Position - 1
                // Me.theMdlFileData.theFileSeekLog.LogToEndAndAlignToNextStart(Me.theInputFileReader, fileOffsetEnd, 4, "theMdlFileData.theTextures alignment")
                catch (Exception ex)
                {
                    int debug = 4242;
                }
            }
        }

        public void ReadSkins()
        {
            if (theMdlFileData.skinFamilyCount > 0 && theMdlFileData.skinReferenceCount > 0)
            {
                // Dim boneInputFileStreamPosition As Long
                // Dim inputFileStreamPosition As Long
                long fileOffsetStart;
                long fileOffsetEnd;
                // Dim fileOffsetStart2 As Long
                // Dim fileOffsetEnd2 As Long
                short aSkinRef;
                try
                {
                    theInputFileReader.BaseStream.Seek(theMdlFileData.skinOffset, SeekOrigin.Begin);
                    fileOffsetStart = theInputFileReader.BaseStream.Position;
                    theMdlFileData.theSkins = new List<List<short>>(theMdlFileData.skinFamilyCount);
                    for (int skinFamilyIndex = 0, loopTo = theMdlFileData.skinFamilyCount - 1; skinFamilyIndex <= loopTo; skinFamilyIndex++)
                    {
                        // boneInputFileStreamPosition = Me.theInputFileReader.BaseStream.Position
                        var aSkinFamily = new List<short>();
                        for (int skinRefIndex = 0, loopTo1 = theMdlFileData.skinReferenceCount - 1; skinRefIndex <= loopTo1; skinRefIndex++)
                        {
                            aSkinRef = theInputFileReader.ReadInt16();
                            aSkinFamily.Add(aSkinRef);
                        }

                        theMdlFileData.theSkins.Add(aSkinFamily);

                        // inputFileStreamPosition = Me.theInputFileReader.BaseStream.Position

                        // Me.theInputFileReader.BaseStream.Seek(inputFileStreamPosition, SeekOrigin.Begin)
                    }

                    fileOffsetEnd = theInputFileReader.BaseStream.Position - 1L;
                    theMdlFileData.theFileSeekLog.Add(fileOffsetStart, fileOffsetEnd, "theMdlFileData.theSkins " + theMdlFileData.theSkins.Count.ToString());
                    theMdlFileData.theFileSeekLog.LogToEndAndAlignToNextStart(theInputFileReader, fileOffsetEnd, 4, "theMdlFileData.theSkins alignment");
                }
                catch (Exception ex)
                {
                    int debug = 4242;
                }
            }
        }

        public void ReadUnreadBytes()
        {
            theMdlFileData.theFileSeekLog.LogUnreadBytes(theInputFileReader);
        }

        // The bone positions and rotations are all zeroes, so get them from the first sequence's first frame.
        public void GetBoneDataFromFirstSequenceFirstFrame()
        {
            SourceMdlSequenceDesc06 aSequence;
            SourceMdlAnimation06 anAnimation;
            SourceMdlBone06 aBone;
            aSequence = theMdlFileData.theSequences[0];
            for (int boneIndex = 0, loopTo = theMdlFileData.theBones.Count - 1; boneIndex <= loopTo; boneIndex++)
            {
                aBone = theMdlFileData.theBones[boneIndex];
                anAnimation = aSequence.theAnimations[boneIndex];
                aBone.position.x = anAnimation.theBonePositionsAndRotations[0].position.x;
                aBone.position.y = anAnimation.theBonePositionsAndRotations[0].position.y;
                aBone.position.z = anAnimation.theBonePositionsAndRotations[0].position.z;
                aBone.rotation.x = anAnimation.theBonePositionsAndRotations[0].rotation.x;
                aBone.rotation.y = anAnimation.theBonePositionsAndRotations[0].rotation.y;
                aBone.rotation.z = anAnimation.theBonePositionsAndRotations[0].rotation.z;
            }
        }

        public void BuildBoneTransforms()
        {
            theMdlFileData.theBoneTransforms = new List<SourceBoneTransform06>(theMdlFileData.theBones.Count);
            for (int boneIndex = 0, loopTo = theMdlFileData.theBones.Count - 1; boneIndex <= loopTo; boneIndex++)
            {
                SourceMdlBone06 aBone;
                var boneTransform = new SourceBoneTransform06();
                int parentBoneIndex;
                aBone = theMdlFileData.theBones[boneIndex];
                var boneMatrixColumn0 = new SourceVector();
                var boneMatrixColumn1 = new SourceVector();
                var boneMatrixColumn2 = new SourceVector();
                var boneMatrixColumn3 = new SourceVector();
                // MathModule.AngleMatrix(aBone.rotation.x, aBone.rotation.y, aBone.rotation.z, boneMatrixColumn0, boneMatrixColumn1, boneMatrixColumn2, boneMatrixColumn3)
                // MathModule.AngleMatrix(aBone.rotation.z, aBone.rotation.x, aBone.rotation.y, boneMatrixColumn0, boneMatrixColumn1, boneMatrixColumn2, boneMatrixColumn3)
                MathModule.AngleMatrix(aBone.rotation.y, aBone.rotation.z, aBone.rotation.x, ref boneMatrixColumn0, ref boneMatrixColumn1, ref boneMatrixColumn2, ref boneMatrixColumn3);
                boneMatrixColumn3.x = aBone.position.x;
                boneMatrixColumn3.y = aBone.position.y;
                boneMatrixColumn3.z = aBone.position.z;
                parentBoneIndex = theMdlFileData.theBones[boneIndex].parentBoneIndex;
                if (parentBoneIndex == -1)
                {
                    boneTransform.matrixColumn0.x = boneMatrixColumn0.x;
                    boneTransform.matrixColumn0.y = boneMatrixColumn0.y;
                    boneTransform.matrixColumn0.z = boneMatrixColumn0.z;
                    boneTransform.matrixColumn1.x = boneMatrixColumn1.x;
                    boneTransform.matrixColumn1.y = boneMatrixColumn1.y;
                    boneTransform.matrixColumn1.z = boneMatrixColumn1.z;
                    boneTransform.matrixColumn2.x = boneMatrixColumn2.x;
                    boneTransform.matrixColumn2.y = boneMatrixColumn2.y;
                    boneTransform.matrixColumn2.z = boneMatrixColumn2.z;
                    boneTransform.matrixColumn3.x = boneMatrixColumn3.x;
                    boneTransform.matrixColumn3.y = boneMatrixColumn3.y;
                    boneTransform.matrixColumn3.z = boneMatrixColumn3.z;
                }
                else
                {
                    SourceBoneTransform06 parentBoneTransform;
                    parentBoneTransform = theMdlFileData.theBoneTransforms[parentBoneIndex];

                    // R_ConcatTransforms( g_bonetransform[pbones[i].parent], bonematrix, g_bonetransform[i] );
                    MathModule.R_ConcatTransforms(parentBoneTransform.matrixColumn0, parentBoneTransform.matrixColumn1, parentBoneTransform.matrixColumn2, parentBoneTransform.matrixColumn3, boneMatrixColumn0, boneMatrixColumn1, boneMatrixColumn2, boneMatrixColumn3, ref boneTransform.matrixColumn0, ref boneTransform.matrixColumn1, ref boneTransform.matrixColumn2, ref boneTransform.matrixColumn3);
                }

                theMdlFileData.theBoneTransforms.Add(boneTransform);
            }
        }

        public void WriteInternalMdlFileName(string internalMdlFileName)
        {
            theOutputFileWriter.BaseStream.Seek(0x8L, SeekOrigin.Begin);
            // TODO: Should only write up to 64 characters.
            theOutputFileWriter.Write(internalMdlFileName.ToCharArray());
            // NOTE: Write the ending null byte.
            theOutputFileWriter.Write(Convert.ToByte(0));
        }

        #endregion

        #region Private Methods

        private void ReadEvents(SourceMdlSequenceDesc06 aSequence)
        {
            if (aSequence.eventCount > 0)
            {
                // Dim sequenceInputFileStreamPosition As Long
                // Dim inputFileStreamPosition As Long
                long fileOffsetStart;
                long fileOffsetEnd;
                // Dim fileOffsetStart2 As Long
                // Dim fileOffsetEnd2 As Long

                try
                {
                    // If Me.theInputFileReader.BaseStream.Position <> aSequence.eventOffset Then
                    // Dim offsetIsNotRight As Boolean = True
                    // End If

                    theInputFileReader.BaseStream.Seek(aSequence.eventOffset, SeekOrigin.Begin);
                    aSequence.theEvents = new List<SourceMdlEvent06>(aSequence.eventCount);
                    for (int eventIndex = 0, loopTo = aSequence.eventCount - 1; eventIndex <= loopTo; eventIndex++)
                    {
                        // sequenceInputFileStreamPosition = Me.theInputFileReader.BaseStream.Position
                        var anEvent = new SourceMdlEvent06();
                        fileOffsetStart = theInputFileReader.BaseStream.Position;
                        anEvent.frameIndex = theInputFileReader.ReadInt16();
                        anEvent.eventType = theInputFileReader.ReadInt16();
                        aSequence.theEvents.Add(anEvent);
                        fileOffsetEnd = theInputFileReader.BaseStream.Position - 1L;
                        theMdlFileData.theFileSeekLog.Add(fileOffsetStart, fileOffsetEnd, "anEvent");
                    }
                }
                catch (Exception ex)
                {
                    int debug = 4242;
                }
            }
        }

        private void ReadPivots(SourceMdlSequenceDesc06 aSequence)
        {
            if (aSequence.pivotCount > 0)
            {
                // Dim sequenceInputFileStreamPosition As Long
                // Dim inputFileStreamPosition As Long
                long fileOffsetStart;
                long fileOffsetEnd;
                // Dim fileOffsetStart2 As Long
                // Dim fileOffsetEnd2 As Long

                try
                {
                    // If Me.theInputFileReader.BaseStream.Position <> aSequence.pivotOffset Then
                    // Dim offsetIsNotRight As Boolean = True
                    // End If

                    theInputFileReader.BaseStream.Seek(aSequence.pivotOffset, SeekOrigin.Begin);
                    aSequence.thePivots = new List<SourceMdlPivot06>(aSequence.pivotCount);
                    for (int pivotIndex = 0, loopTo = aSequence.pivotCount - 1; pivotIndex <= loopTo; pivotIndex++)
                    {
                        // sequenceInputFileStreamPosition = Me.theInputFileReader.BaseStream.Position
                        var aPivot = new SourceMdlPivot06();
                        fileOffsetStart = theInputFileReader.BaseStream.Position;
                        aPivot.point.x = theInputFileReader.ReadSingle();
                        aPivot.point.y = theInputFileReader.ReadSingle();
                        aPivot.point.z = theInputFileReader.ReadSingle();
                        aPivot.pivotStart = theInputFileReader.ReadInt32();
                        aPivot.pivotEnd = theInputFileReader.ReadInt32();
                        aSequence.thePivots.Add(aPivot);
                        fileOffsetEnd = theInputFileReader.BaseStream.Position - 1L;
                        theMdlFileData.theFileSeekLog.Add(fileOffsetStart, fileOffsetEnd, "aPivot");
                    }
                }
                catch (Exception ex)
                {
                    int debug = 4242;
                }
            }
        }

        private void ReadModels(SourceMdlBodyPart06 aBodyPart)
        {
            // Dim modelInputFileStreamPosition As Long
            long inputFileStreamPosition;
            long fileOffsetStart;
            long fileOffsetEnd;
            // Dim fileOffsetStart2 As Long
            // Dim fileOffsetEnd2 As Long

            try
            {
                theInputFileReader.BaseStream.Seek(aBodyPart.modelOffset, SeekOrigin.Begin);
                aBodyPart.theModels = new List<SourceMdlModel06>(aBodyPart.modelCount);
                for (int bodyPartIndex = 0, loopTo = aBodyPart.modelCount - 1; bodyPartIndex <= loopTo; bodyPartIndex++)
                {
                    fileOffsetStart = theInputFileReader.BaseStream.Position;
                    // modelInputFileStreamPosition = Me.theInputFileReader.BaseStream.Position
                    var aModel = new SourceMdlModel06();
                    aModel.name = theInputFileReader.ReadChars(64);
                    aModel.theName = Conversions.ToString(aModel.name).Trim('\0');
                    aModel.type = theInputFileReader.ReadInt32();
                    aModel.unknown01 = theInputFileReader.ReadInt32();
                    aModel.unused01 = theInputFileReader.ReadInt32();
                    aModel.meshCount = theInputFileReader.ReadInt32();
                    aModel.meshOffset = theInputFileReader.ReadInt32();
                    aModel.vertexCount = theInputFileReader.ReadInt32();
                    aModel.vertexBoneInfoOffset = theInputFileReader.ReadInt32();
                    aModel.normalCount = theInputFileReader.ReadInt32();
                    aModel.normalBoneInfoOffset = theInputFileReader.ReadInt32();
                    aModel.unused02 = theInputFileReader.ReadInt32();
                    aModel.modelDataOffset = theInputFileReader.ReadInt32();
                    aBodyPart.theModels.Add(aModel);
                    fileOffsetEnd = theInputFileReader.BaseStream.Position - 1L;
                    theMdlFileData.theFileSeekLog.Add(fileOffsetStart, fileOffsetEnd, "aModel [" + aModel.theName + "]");
                    inputFileStreamPosition = theInputFileReader.BaseStream.Position;
                    ReadModelVertexBoneInfos(aModel);
                    ReadModelNormalBoneInfos(aModel);
                    ReadModelData(aModel);
                    ReadMeshes(aModel);
                    theInputFileReader.BaseStream.Seek(inputFileStreamPosition, SeekOrigin.Begin);
                }
            }
            catch (Exception ex)
            {
                int debug = 4242;
            }
        }

        private void ReadModelVertexBoneInfos(SourceMdlModel06 aModel)
        {
            if (aModel.vertexCount > 0)
            {
                long fileOffsetStart;
                long fileOffsetEnd;
                try
                {
                    int vertexBoneInfo;
                    theInputFileReader.BaseStream.Seek(aModel.vertexBoneInfoOffset, SeekOrigin.Begin);
                    fileOffsetStart = theInputFileReader.BaseStream.Position;
                    aModel.theVertexBoneInfos = new List<int>(aModel.vertexCount);
                    for (int vertexBoneInfoIndex = 0, loopTo = aModel.vertexCount - 1; vertexBoneInfoIndex <= loopTo; vertexBoneInfoIndex++)
                    {
                        vertexBoneInfo = theInputFileReader.ReadByte();
                        aModel.theVertexBoneInfos.Add(vertexBoneInfo);
                    }

                    fileOffsetEnd = theInputFileReader.BaseStream.Position - 1L;
                    theMdlFileData.theFileSeekLog.Add(fileOffsetStart, fileOffsetEnd, "aModel.theVertexBoneInfos " + aModel.theVertexBoneInfos.Count.ToString());
                    theMdlFileData.theFileSeekLog.LogToEndAndAlignToNextStart(theInputFileReader, fileOffsetEnd, 4, "aModel.theVertexBoneInfos alignment");
                }
                catch (Exception ex)
                {
                    int debug = 4242;
                }
            }
        }

        private void ReadModelNormalBoneInfos(SourceMdlModel06 aModel)
        {
            if (aModel.normalCount > 0)
            {
                long fileOffsetStart;
                long fileOffsetEnd;
                try
                {
                    int normalBoneInfo;
                    theInputFileReader.BaseStream.Seek(aModel.normalBoneInfoOffset, SeekOrigin.Begin);
                    fileOffsetStart = theInputFileReader.BaseStream.Position;
                    aModel.theNormalBoneInfos = new List<int>(aModel.normalCount);
                    for (int normalBoneInfoIndex = 0, loopTo = aModel.normalCount - 1; normalBoneInfoIndex <= loopTo; normalBoneInfoIndex++)
                    {
                        normalBoneInfo = theInputFileReader.ReadByte();
                        aModel.theNormalBoneInfos.Add(normalBoneInfo);
                    }

                    fileOffsetEnd = theInputFileReader.BaseStream.Position - 1L;
                    theMdlFileData.theFileSeekLog.Add(fileOffsetStart, fileOffsetEnd, "aModel.theNormalBoneInfos " + aModel.theNormalBoneInfos.Count.ToString());
                    theMdlFileData.theFileSeekLog.LogToEndAndAlignToNextStart(theInputFileReader, fileOffsetEnd, 4, "aModel.theNormalBoneInfos alignment");
                }
                catch (Exception ex)
                {
                    int debug = 4242;
                }
            }
        }

        private void ReadModelData(SourceMdlModel06 aModel)
        {
            long inputFileStreamPosition;
            long fileOffsetStart;
            long fileOffsetEnd;
            try
            {
                theInputFileReader.BaseStream.Seek(aModel.modelDataOffset, SeekOrigin.Begin);
                fileOffsetStart = theInputFileReader.BaseStream.Position;

                // NOTE: No idea why there are sequenceCount of model data. The first one seems to be the only one used.
                for (int sequenceIndex = 0, loopTo = theMdlFileData.sequenceCount - 1; sequenceIndex <= loopTo; sequenceIndex++)
                {
                    var aModelData = new SourceMdlModelData06();
                    aModelData.unknown01 = theInputFileReader.ReadInt32();
                    aModelData.unknown02 = theInputFileReader.ReadInt32();
                    aModelData.unknown03 = theInputFileReader.ReadInt32();
                    aModelData.vertexCount = theInputFileReader.ReadInt32();
                    aModelData.vertexOffset = theInputFileReader.ReadInt32();
                    aModelData.normalCount = theInputFileReader.ReadInt32();
                    aModelData.normalOffset = theInputFileReader.ReadInt32();
                    aModel.theModelDatas.Add(aModelData);
                }

                fileOffsetEnd = theInputFileReader.BaseStream.Position - 1L;
                theMdlFileData.theFileSeekLog.Add(fileOffsetStart, fileOffsetEnd, "aModel.theModelDatas " + aModel.theModelDatas.Count.ToString());

                // Me.theMdlFileData.theFileSeekLog.LogToEndAndAlignToNextStart(Me.theInputFileReader, fileOffsetEnd, 4, "aModel.theModelDatas alignment")

                inputFileStreamPosition = theInputFileReader.BaseStream.Position;
                ReadModelVertexes(aModel.theModelDatas[0]);
                ReadModelNormals(aModel.theModelDatas[0]);
                theInputFileReader.BaseStream.Seek(inputFileStreamPosition, SeekOrigin.Begin);
            }
            catch (Exception ex)
            {
                int debug = 4242;
            }
        }

        private void ReadModelVertexes(SourceMdlModelData06 aModelData)
        {
            if (aModelData.vertexCount > 0)
            {
                long fileOffsetStart;
                long fileOffsetEnd;
                try
                {
                    theInputFileReader.BaseStream.Seek(aModelData.vertexOffset, SeekOrigin.Begin);
                    fileOffsetStart = theInputFileReader.BaseStream.Position;
                    aModelData.theVertexes = new List<SourceVector>(aModelData.vertexCount);
                    for (int vertexIndex = 0, loopTo = aModelData.vertexCount - 1; vertexIndex <= loopTo; vertexIndex++)
                    {
                        var vertex = new SourceVector();
                        vertex.x = theInputFileReader.ReadSingle();
                        vertex.y = theInputFileReader.ReadSingle();
                        vertex.z = theInputFileReader.ReadSingle();
                        aModelData.theVertexes.Add(vertex);
                    }

                    fileOffsetEnd = theInputFileReader.BaseStream.Position - 1L;
                    theMdlFileData.theFileSeekLog.Add(fileOffsetStart, fileOffsetEnd, "aModel.theVertexes " + aModelData.theVertexes.Count.ToString());
                }

                // Me.theMdlFileData.theFileSeekLog.LogToEndAndAlignToNextStart(Me.theInputFileReader, fileOffsetEnd, 4, "aModel.theVertexes alignment")
                catch (Exception ex)
                {
                    int debug = 4242;
                }
            }
        }

        private void ReadModelNormals(SourceMdlModelData06 aModelData)
        {
            if (aModelData.normalCount > 0)
            {
                long fileOffsetStart;
                long fileOffsetEnd;
                try
                {
                    theInputFileReader.BaseStream.Seek(aModelData.normalOffset, SeekOrigin.Begin);
                    fileOffsetStart = theInputFileReader.BaseStream.Position;
                    aModelData.theNormals = new List<SourceVector>(aModelData.normalCount);
                    for (int normalIndex = 0, loopTo = aModelData.normalCount - 1; normalIndex <= loopTo; normalIndex++)
                    {
                        var normal = new SourceVector();
                        normal.x = theInputFileReader.ReadSingle();
                        normal.y = theInputFileReader.ReadSingle();
                        normal.z = theInputFileReader.ReadSingle();
                        aModelData.theNormals.Add(normal);
                    }

                    fileOffsetEnd = theInputFileReader.BaseStream.Position - 1L;
                    theMdlFileData.theFileSeekLog.Add(fileOffsetStart, fileOffsetEnd, "aModel.theNormals " + aModelData.theNormals.Count.ToString());
                }

                // Me.theMdlFileData.theFileSeekLog.LogToEndAndAlignToNextStart(Me.theInputFileReader, fileOffsetEnd, 4, "aModel.theNormals alignment")
                catch (Exception ex)
                {
                    int debug = 4242;
                }
            }
        }

        private void ReadMeshes(SourceMdlModel06 aModel)
        {
            if (aModel.meshCount > 0)
            {
                // Dim meshInputFileStreamPosition As Long
                long inputFileStreamPosition;
                long fileOffsetStart;
                long fileOffsetEnd;
                // Dim fileOffsetStart2 As Long
                // Dim fileOffsetEnd2 As Long

                try
                {
                    theInputFileReader.BaseStream.Seek(aModel.meshOffset, SeekOrigin.Begin);
                    aModel.theMeshes = new List<SourceMdlMesh06>(aModel.meshCount);
                    for (int meshIndex = 0, loopTo = aModel.meshCount - 1; meshIndex <= loopTo; meshIndex++)
                    {
                        fileOffsetStart = theInputFileReader.BaseStream.Position;
                        var aMesh = new SourceMdlMesh06();
                        aMesh.faceCount = theInputFileReader.ReadInt32();
                        aMesh.faceOffset = theInputFileReader.ReadInt32();
                        aMesh.skinref = theInputFileReader.ReadInt32();
                        aMesh.normalCount = theInputFileReader.ReadInt32();
                        aMesh.normalOffset = theInputFileReader.ReadInt32();
                        aModel.theMeshes.Add(aMesh);
                        fileOffsetEnd = theInputFileReader.BaseStream.Position - 1L;
                        theMdlFileData.theFileSeekLog.Add(fileOffsetStart, fileOffsetEnd, "aMesh");
                        inputFileStreamPosition = theInputFileReader.BaseStream.Position;
                        ReadFaces(aMesh);
                        theInputFileReader.BaseStream.Seek(inputFileStreamPosition, SeekOrigin.Begin);
                    }
                }

                // Me.theMdlFileData.theFileSeekLog.LogToEndAndAlignToNextStart(Me.theInputFileReader, fileOffsetEnd, 4, "aModel.theMeshes alignment")
                catch (Exception ex)
                {
                    int debug = 4242;
                }
            }
        }

        private void ReadFaces(SourceMdlMesh06 aMesh)
        {
            if (aMesh.faceCount > 0)
            {
                long fileOffsetStart;
                long fileOffsetEnd;
                try
                {
                    theInputFileReader.BaseStream.Seek(aMesh.faceOffset, SeekOrigin.Begin);
                    fileOffsetStart = theInputFileReader.BaseStream.Position;
                    aMesh.theVertexAndNormalIndexes = new List<SourceMdlTriangleVertex06>();
                    for (int faceIndex = 0, loopTo = aMesh.faceCount * 3 - 1; faceIndex <= loopTo; faceIndex++)
                    {
                        var vertexAndNormalIndexInfo = new SourceMdlTriangleVertex06();
                        vertexAndNormalIndexInfo.vertexIndex = theInputFileReader.ReadUInt16();
                        vertexAndNormalIndexInfo.normalIndex = theInputFileReader.ReadUInt16();
                        vertexAndNormalIndexInfo.s = theInputFileReader.ReadInt16();
                        vertexAndNormalIndexInfo.t = theInputFileReader.ReadInt16();
                        aMesh.theVertexAndNormalIndexes.Add(vertexAndNormalIndexInfo);
                    }

                    fileOffsetEnd = theInputFileReader.BaseStream.Position - 1L;
                    theMdlFileData.theFileSeekLog.Add(fileOffsetStart, fileOffsetEnd, "aMesh.theVertexAndNormalIndexes " + aMesh.theVertexAndNormalIndexes.Count.ToString());
                }

                // Me.theMdlFileData.theFileSeekLog.LogToEndAndAlignToNextStart(Me.theInputFileReader, fileOffsetEnd, 4, "aMesh.theVertexAndNormalIndexes alignment")
                catch (Exception ex)
                {
                    int debug = 4242;
                }
            }
        }

        private void ReadTextureData(SourceMdlTexture06 aTexture)
        {
            // Dim boneInputFileStreamPosition As Long
            // Dim inputFileStreamPosition As Long
            long fileOffsetStart;
            long fileOffsetEnd;
            // Dim fileOffsetStart2 As Long
            // Dim fileOffsetEnd2 As Long

            try
            {
                theInputFileReader.BaseStream.Seek(aTexture.dataOffset, SeekOrigin.Begin);
                fileOffsetStart = theInputFileReader.BaseStream.Position;
                aTexture.theData = new List<byte>(Conversions.ToInteger(aTexture.width * aTexture.height));
                // FROM: [1999] HLStandardSDK\SourceCode\utils\studiomdl\studiomdl.c
                // Void ResizeTexture(s_texture_t * ptexture)
                // ptexture->size = ptexture->skinwidth * ptexture->skinheight + 256 * 3;
                for (long byteIndex = 0L, loopTo = aTexture.width * aTexture.height + (long)(256 * 3) - 1L; byteIndex <= loopTo; byteIndex++)
                {
                    // boneInputFileStreamPosition = Me.theInputFileReader.BaseStream.Position
                    byte data;
                    data = theInputFileReader.ReadByte();
                    aTexture.theData.Add(data);

                    // inputFileStreamPosition = Me.theInputFileReader.BaseStream.Position

                    // Me.theInputFileReader.BaseStream.Seek(inputFileStreamPosition, SeekOrigin.Begin)
                }

                fileOffsetEnd = theInputFileReader.BaseStream.Position - 1L;
                theMdlFileData.theFileSeekLog.Add(fileOffsetStart, fileOffsetEnd, "aTexture.theData");
            }
            catch (Exception ex)
            {
                int debug = 4242;
            }
        }

        private void ReadAnimationBonePositions(SourceMdlAnimation06 anAnimation)
        {
            if (anAnimation.bonePositionCount > 0)
            {
                long fileOffsetStart;
                long fileOffsetEnd;
                int startFrameIndex;
                int stopFrameIndex;
                try
                {
                    theInputFileReader.BaseStream.Seek(anAnimation.bonePositionOffset, SeekOrigin.Begin);
                    fileOffsetStart = theInputFileReader.BaseStream.Position;
                    anAnimation.theRawBonePositions = new List<SourceMdlBonePosition06>(anAnimation.bonePositionCount);
                    for (int bonePositionIndex = 0, loopTo = anAnimation.bonePositionCount - 1; bonePositionIndex <= loopTo; bonePositionIndex++)
                    {
                        var bonePosition = new SourceMdlBonePosition06();
                        bonePosition.frameIndex = theInputFileReader.ReadInt32();
                        bonePosition.position.x = theInputFileReader.ReadSingle();
                        bonePosition.position.y = theInputFileReader.ReadSingle();
                        bonePosition.position.z = theInputFileReader.ReadSingle();
                        anAnimation.theRawBonePositions.Add(bonePosition);
                    }

                    // NOTE: Set up list indexed by frame for easier writing of animation SMD files.
                    startFrameIndex = 0;
                    for (int bonePositionIndex = 0, loopTo1 = anAnimation.theRawBonePositions.Count - 1; bonePositionIndex <= loopTo1; bonePositionIndex++)
                    {
                        var bonePosition = new SourceMdlBonePosition06();
                        bonePosition = anAnimation.theRawBonePositions[bonePositionIndex];
                        if (bonePositionIndex == anAnimation.theRawBonePositions.Count - 1)
                        {
                            stopFrameIndex = anAnimation.theBonePositionsAndRotations.Count - 1;
                        }
                        else
                        {
                            stopFrameIndex = anAnimation.theRawBonePositions[bonePositionIndex + 1].frameIndex - 1;
                        }

                        for (int frameIndex = startFrameIndex, loopTo2 = stopFrameIndex; frameIndex <= loopTo2; frameIndex++)
                        {
                            var bonePositionAndRotation = new SourceBonePostionAndRotation06();
                            bonePositionAndRotation = anAnimation.theBonePositionsAndRotations[frameIndex];
                            bonePositionAndRotation.position.x = bonePosition.position.x;
                            bonePositionAndRotation.position.y = bonePosition.position.y;
                            bonePositionAndRotation.position.z = bonePosition.position.z;
                        }

                        startFrameIndex = stopFrameIndex + 1;
                    }

                    fileOffsetEnd = theInputFileReader.BaseStream.Position - 1L;
                    theMdlFileData.theFileSeekLog.Add(fileOffsetStart, fileOffsetEnd, "anAnimation.theBonePositions " + anAnimation.theRawBonePositions.Count.ToString());
                }

                // Me.theMdlFileData.theFileSeekLog.LogToEndAndAlignToNextStart(Me.theInputFileReader, fileOffsetEnd, 4, "anAnimation.theBonePositions alignment")
                catch (Exception ex)
                {
                    int debug = 4242;
                }
            }
        }

        private void ReadAnimationBoneRotations(SourceMdlAnimation06 anAnimation)
        {
            if (anAnimation.boneRotationCount > 0)
            {
                long fileOffsetStart;
                long fileOffsetEnd;
                int startFrameIndex;
                int stopFrameIndex;
                try
                {
                    theInputFileReader.BaseStream.Seek(anAnimation.boneRotationOffset, SeekOrigin.Begin);
                    fileOffsetStart = theInputFileReader.BaseStream.Position;
                    anAnimation.theRawBoneRotations = new List<SourceMdlBoneRotation06>(anAnimation.boneRotationCount);
                    for (int boneRotationIndex = 0, loopTo = anAnimation.boneRotationCount - 1; boneRotationIndex <= loopTo; boneRotationIndex++)
                    {
                        var boneRotation = new SourceMdlBoneRotation06();
                        boneRotation.frameIndex = theInputFileReader.ReadInt16();
                        boneRotation.angle[0] = theInputFileReader.ReadInt16();
                        boneRotation.angle[1] = theInputFileReader.ReadInt16();
                        boneRotation.angle[2] = theInputFileReader.ReadInt16();
                        anAnimation.theRawBoneRotations.Add(boneRotation);
                    }

                    // NOTE: Set up list indexed by frame for easier writing of animation SMD files.
                    startFrameIndex = 0;
                    for (int boneRotationIndex = 0, loopTo1 = anAnimation.theRawBoneRotations.Count - 1; boneRotationIndex <= loopTo1; boneRotationIndex++)
                    {
                        var boneRotation = new SourceMdlBoneRotation06();
                        boneRotation = anAnimation.theRawBoneRotations[boneRotationIndex];
                        if (boneRotationIndex == anAnimation.theRawBoneRotations.Count - 1)
                        {
                            stopFrameIndex = anAnimation.theBonePositionsAndRotations.Count - 1;
                        }
                        else
                        {
                            stopFrameIndex = anAnimation.theRawBoneRotations[boneRotationIndex + 1].frameIndex - 1;
                        }

                        for (int frameIndex = startFrameIndex, loopTo2 = stopFrameIndex; frameIndex <= loopTo2; frameIndex++)
                        {
                            var bonePositionAndRotation = new SourceBonePostionAndRotation06();
                            bonePositionAndRotation = anAnimation.theBonePositionsAndRotations[frameIndex];

                            // #define STUDIO_TO_RAD		(Q_PI_F/18000.0f)
                            bonePositionAndRotation.rotation.x = boneRotation.angle[0] * (Math.PI / 18000d);
                            bonePositionAndRotation.rotation.y = boneRotation.angle[1] * (Math.PI / 18000d);
                            bonePositionAndRotation.rotation.z = boneRotation.angle[2] * (Math.PI / 18000d);
                        }

                        startFrameIndex = stopFrameIndex + 1;
                    }

                    fileOffsetEnd = theInputFileReader.BaseStream.Position - 1L;
                    theMdlFileData.theFileSeekLog.Add(fileOffsetStart, fileOffsetEnd, "anAnimation.theBoneRotations " + anAnimation.theRawBoneRotations.Count.ToString());
                }

                // Me.theMdlFileData.theFileSeekLog.LogToEndAndAlignToNextStart(Me.theInputFileReader, fileOffsetEnd, 4, "anAnimation.theBoneRotations alignment")
                catch (Exception ex)
                {
                    int debug = 4242;
                }
            }
        }

        #endregion

        #region Data

        protected BinaryReader theInputFileReader;
        protected BinaryWriter theOutputFileWriter;
        protected SourceMdlFileData06 theMdlFileData;

        #endregion

    }
}