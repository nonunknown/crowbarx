﻿using System;
using System.Collections.Generic;
using System.IO;

namespace Crowbar
{
    public class SourceSmdFile37
    {

        #region Creation and Destruction

        public SourceSmdFile37(StreamWriter outputFileStream, SourceMdlFileData37 mdlFileData)
        {
            theOutputFileStreamWriter = outputFileStream;
            theMdlFileData = mdlFileData;
        }

        public SourceSmdFile37(StreamWriter outputFileStream, SourceMdlFileData37 mdlFileData, SourcePhyFileData phyFileData)
        {
            theOutputFileStreamWriter = outputFileStream;
            theMdlFileData = mdlFileData;
            thePhyFileData = phyFileData;
        }

        #endregion

        #region Methods

        public void WriteHeaderComment()
        {
            Common.WriteHeaderComment(theOutputFileStreamWriter);
        }

        public void WriteHeaderSection()
        {
            string line = "";

            // version 1
            line = "version 1";
            theOutputFileStreamWriter.WriteLine(line);
        }

        public void WriteNodesSection(int lodIndex)
        {
            string line = "";
            string name;

            // nodes
            line = "nodes";
            theOutputFileStreamWriter.WriteLine(line);
            for (int boneIndex = 0, loopTo = theMdlFileData.theBones.Count - 1; boneIndex <= loopTo; boneIndex++)
            {
                name = theMdlFileData.theBones[boneIndex].theName;
                line = "  ";
                line += boneIndex.ToString(Program.TheApp.InternalNumberFormat);
                line += " \"";
                line += name;
                line += "\" ";
                line += theMdlFileData.theBones[boneIndex].parentBoneIndex.ToString(Program.TheApp.InternalNumberFormat);
                theOutputFileStreamWriter.WriteLine(line);
            }

            line = "end";
            theOutputFileStreamWriter.WriteLine(line);
        }

        public void WriteSkeletonSection(int lodIndex)
        {
            string line = "";

            // skeleton
            line = "skeleton";
            theOutputFileStreamWriter.WriteLine(line);
            if (Program.TheApp.Settings.DecompileStricterFormatIsChecked)
            {
                line = "time 0";
            }
            else
            {
                line = "  time 0";
            }

            theOutputFileStreamWriter.WriteLine(line);
            for (int boneIndex = 0, loopTo = theMdlFileData.theBones.Count - 1; boneIndex <= loopTo; boneIndex++)
            {
                line = "    ";
                line += boneIndex.ToString(Program.TheApp.InternalNumberFormat);
                line += " ";
                line += theMdlFileData.theBones[boneIndex].position.x.ToString("0.000000", Program.TheApp.InternalNumberFormat);
                line += " ";
                line += theMdlFileData.theBones[boneIndex].position.y.ToString("0.000000", Program.TheApp.InternalNumberFormat);
                line += " ";
                line += theMdlFileData.theBones[boneIndex].position.z.ToString("0.000000", Program.TheApp.InternalNumberFormat);
                line += " ";
                line += theMdlFileData.theBones[boneIndex].rotation.x.ToString("0.000000", Program.TheApp.InternalNumberFormat);
                line += " ";
                line += theMdlFileData.theBones[boneIndex].rotation.y.ToString("0.000000", Program.TheApp.InternalNumberFormat);
                line += " ";
                line += theMdlFileData.theBones[boneIndex].rotation.z.ToString("0.000000", Program.TheApp.InternalNumberFormat);
                theOutputFileStreamWriter.WriteLine(line);
            }

            line = "end";
            theOutputFileStreamWriter.WriteLine(line);
        }

        public void WriteTrianglesSection(int lodIndex, SourceVtxModel06 aVtxModel, SourceMdlModel37 aModel, int bodyPartVertexIndexStart)
        {
            string line = "";
            string materialLine = "";
            string vertex1Line = "";
            string vertex2Line = "";
            string vertex3Line = "";

            // triangles
            line = "triangles";
            theOutputFileStreamWriter.WriteLine(line);
            SourceVtxModelLod06 aVtxLod;
            SourceVtxMesh06 aVtxMesh;
            SourceVtxStripGroup06 aStripGroup;
            // Dim cumulativeVertexCount As Integer
            // Dim maxIndexForMesh As Integer
            // Dim cumulativeMaxIndex As Integer
            int materialIndex;
            string materialName;
            int meshVertexIndexStart;
            try
            {
                aVtxLod = aVtxModel.theVtxModelLods[lodIndex];
                if (aVtxLod.theVtxMeshes is object)
                {
                    // cumulativeVertexCount = 0
                    // maxIndexForMesh = 0
                    // cumulativeMaxIndex = 0
                    for (int meshIndex = 0, loopTo = aVtxLod.theVtxMeshes.Count - 1; meshIndex <= loopTo; meshIndex++)
                    {
                        aVtxMesh = aVtxLod.theVtxMeshes[meshIndex];
                        materialIndex = aModel.theMeshes[meshIndex].materialIndex;
                        materialName = theMdlFileData.theTextures[materialIndex].theFileName;
                        // TODO: This was used in previous versions, but maybe should leave as above.
                        // materialName = Path.GetFileName(Me.theSourceEngineModel.theMdlFileHeader.theTextures(materialIndex).theName)

                        meshVertexIndexStart = aModel.theMeshes[meshIndex].vertexIndexStart;
                        if (aVtxMesh.theVtxStripGroups is object)
                        {
                            for (int groupIndex = 0, loopTo1 = aVtxMesh.theVtxStripGroups.Count - 1; groupIndex <= loopTo1; groupIndex++)
                            {
                                aStripGroup = aVtxMesh.theVtxStripGroups[groupIndex];
                                if (aStripGroup.theVtxStrips is object && aStripGroup.theVtxIndexes is object && aStripGroup.theVtxVertexes is object)
                                {
                                    for (int vtxIndexIndex = 0, loopTo2 = aStripGroup.theVtxIndexes.Count - 3; vtxIndexIndex <= loopTo2; vtxIndexIndex += 3)
                                    {
                                        // 'NOTE: studiomdl.exe will complain if texture name for eyeball is not at start of line.
                                        // line = materialName
                                        // Me.theOutputFileStreamWriter.WriteLine(line)
                                        // Me.WriteVertexLine(aStripGroup, vtxIndexIndex, lodIndex, meshVertexIndexStart, bodyPartVertexIndexStart)
                                        // Me.WriteVertexLine(aStripGroup, vtxIndexIndex + 2, lodIndex, meshVertexIndexStart, bodyPartVertexIndexStart)
                                        // Me.WriteVertexLine(aStripGroup, vtxIndexIndex + 1, lodIndex, meshVertexIndexStart, bodyPartVertexIndexStart)
                                        // ------
                                        // NOTE: studiomdl.exe will complain if texture name for eyeball is not at start of line.
                                        materialLine = materialName;
                                        vertex1Line = WriteVertexLine(aStripGroup, vtxIndexIndex, lodIndex, meshVertexIndexStart, bodyPartVertexIndexStart, aModel);
                                        vertex2Line = WriteVertexLine(aStripGroup, vtxIndexIndex + 2, lodIndex, meshVertexIndexStart, bodyPartVertexIndexStart, aModel);
                                        vertex3Line = WriteVertexLine(aStripGroup, vtxIndexIndex + 1, lodIndex, meshVertexIndexStart, bodyPartVertexIndexStart, aModel);
                                        if (vertex1Line.StartsWith("// ") || vertex2Line.StartsWith("// ") || vertex3Line.StartsWith("// "))
                                        {
                                            materialLine = "// " + materialLine;
                                            if (!vertex1Line.StartsWith("// "))
                                            {
                                                vertex1Line = "// " + vertex1Line;
                                            }

                                            if (!vertex2Line.StartsWith("// "))
                                            {
                                                vertex2Line = "// " + vertex2Line;
                                            }

                                            if (!vertex3Line.StartsWith("// "))
                                            {
                                                vertex3Line = "// " + vertex3Line;
                                            }
                                        }

                                        theOutputFileStreamWriter.WriteLine(materialLine);
                                        theOutputFileStreamWriter.WriteLine(vertex1Line);
                                        theOutputFileStreamWriter.WriteLine(vertex2Line);
                                        theOutputFileStreamWriter.WriteLine(vertex3Line);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
            }

            line = "end";
            theOutputFileStreamWriter.WriteLine(line);
        }

        public void WriteTrianglesSectionForPhysics()
        {
            string line = "";

            // triangles
            line = "triangles";
            theOutputFileStreamWriter.WriteLine(line);
            SourcePhyCollisionData collisionData;
            SourceMdlBone37 aBone;
            int boneIndex;
            SourcePhyFace aTriangle;
            SourcePhyFaceSection faceSection;
            SourcePhyVertex phyVertex;
            SourceVector aVectorTransformed;
            SourcePhyPhysCollisionModel aSourcePhysCollisionModel;
            try
            {
                if (thePhyFileData.theSourcePhyCollisionDatas is object)
                {
                    for (int collisionDataIndex = 0, loopTo = thePhyFileData.theSourcePhyCollisionDatas.Count - 1; collisionDataIndex <= loopTo; collisionDataIndex++)
                    {
                        collisionData = thePhyFileData.theSourcePhyCollisionDatas[collisionDataIndex];
                        if (collisionDataIndex < thePhyFileData.theSourcePhyPhysCollisionModels.Count)
                        {
                            aSourcePhysCollisionModel = thePhyFileData.theSourcePhyPhysCollisionModels[collisionDataIndex];
                        }
                        else
                        {
                            aSourcePhysCollisionModel = null;
                        }

                        for (int faceSectionIndex = 0, loopTo1 = collisionData.theFaceSections.Count - 1; faceSectionIndex <= loopTo1; faceSectionIndex++)
                        {
                            faceSection = collisionData.theFaceSections[faceSectionIndex];
                            if (faceSection.theBoneIndex >= theMdlFileData.theBones.Count)
                            {
                                continue;
                            }

                            if (aSourcePhysCollisionModel is object && theMdlFileData.theBoneNameToBoneIndexMap.ContainsKey(aSourcePhysCollisionModel.theName))
                            {
                                boneIndex = theMdlFileData.theBoneNameToBoneIndexMap[aSourcePhysCollisionModel.theName];
                            }
                            else
                            {
                                boneIndex = faceSection.theBoneIndex;
                            }

                            aBone = theMdlFileData.theBones[boneIndex];
                            for (int triangleIndex = 0, loopTo2 = faceSection.theFaces.Count - 1; triangleIndex <= loopTo2; triangleIndex++)
                            {
                                aTriangle = faceSection.theFaces[triangleIndex];
                                line = "  phy";
                                theOutputFileStreamWriter.WriteLine(line);
                                for (int vertexIndex = 0, loopTo3 = aTriangle.vertexIndex.Length - 1; vertexIndex <= loopTo3; vertexIndex++)
                                {
                                    // phyVertex = collisionData.theVertices(aTriangle.vertexIndex(vertexIndex))
                                    phyVertex = faceSection.theVertices[aTriangle.vertexIndex[vertexIndex]];
                                    aVectorTransformed = TransformPhyVertex(aBone, phyVertex.vertex, aSourcePhysCollisionModel);
                                    line = "    ";
                                    line += boneIndex.ToString(Program.TheApp.InternalNumberFormat);
                                    line += " ";
                                    line += aVectorTransformed.x.ToString("0.000000", Program.TheApp.InternalNumberFormat);
                                    line += " ";
                                    line += aVectorTransformed.y.ToString("0.000000", Program.TheApp.InternalNumberFormat);
                                    line += " ";
                                    line += aVectorTransformed.z.ToString("0.000000", Program.TheApp.InternalNumberFormat);

                                    // line += " 0 0 0"
                                    // ------
                                    line += " ";
                                    line += phyVertex.Normal.x.ToString("0.000000", Program.TheApp.InternalNumberFormat);
                                    line += " ";
                                    line += phyVertex.Normal.y.ToString("0.000000", Program.TheApp.InternalNumberFormat);
                                    line += " ";
                                    line += phyVertex.Normal.z.ToString("0.000000", Program.TheApp.InternalNumberFormat);
                                    line += " 0 0";
                                    // NOTE: The studiomdl.exe doesn't need the integer values at end.
                                    // line += " 1 0"
                                    theOutputFileStreamWriter.WriteLine(line);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                int debug = 4242;
            }

            line = "end";
            theOutputFileStreamWriter.WriteLine(line);
        }

        // TODO: Write the firstAnimDesc's first frame's frameLines because it is used for "subtract" option.
        public void CalculateFirstAnimDescFrameLinesForSubtract()
        {
            int boneIndex;
            AnimationFrameLine aFrameLine;
            int frameIndex;
            SourceMdlSequenceDesc aSequenceDesc;
            SourceMdlAnimationDesc37 anAnimationDesc;
            aSequenceDesc = null;
            anAnimationDesc = theMdlFileData.theFirstAnimationDesc;
            theAnimationFrameLines = new SortedList<int, AnimationFrameLine>();
            frameIndex = 0;
            theAnimationFrameLines.Clear();
            if ((anAnimationDesc.flags & SourceMdlAnimationDesc.STUDIO_ALLZEROS) == 0)
            {
                CalcAnimation(aSequenceDesc, anAnimationDesc, frameIndex);
            }

            for (int i = 0, loopTo = theAnimationFrameLines.Count - 1; i <= loopTo; i++)
            {
                boneIndex = theAnimationFrameLines.Keys[i];
                aFrameLine = theAnimationFrameLines.Values[i];
                var aFirstAnimationDescFrameLine = new AnimationFrameLine();
                aFirstAnimationDescFrameLine.rotation = new SourceVector();
                aFirstAnimationDescFrameLine.position = new SourceVector();

                // NOTE: Only rotate by -90 deg if bone is a root bone.  Do not know why.
                // If Me.theSourceEngineModel.theMdlFileHeader.theBones(boneIndex).parentBoneIndex = -1 Then
                // TEST: Try this version, because of "sequence_blend from Game Zombie" model.
                aFirstAnimationDescFrameLine.rotation.x = aFrameLine.rotation.x;
                aFirstAnimationDescFrameLine.rotation.y = aFrameLine.rotation.y;
                if (theMdlFileData.theBones[boneIndex].parentBoneIndex == -1 && (aFrameLine.rotation.debug_text.StartsWith("raw") || aFrameLine.rotation.debug_text == "anim+bone"))
                {
                    double z;
                    z = aFrameLine.rotation.z;
                    z += MathModule.DegreesToRadians(-90);
                    aFirstAnimationDescFrameLine.rotation.z = z;
                }
                else
                {
                    aFirstAnimationDescFrameLine.rotation.z = aFrameLine.rotation.z;
                }

                // NOTE: Only adjust position if bone is a root bone. Do not know why.
                // If Me.theSourceEngineModel.theMdlFileHeader.theBones(boneIndex).parentBoneIndex = -1 Then
                // TEST: Try this version, because of "sequence_blend from Game Zombie" model.
                if (theMdlFileData.theBones[boneIndex].parentBoneIndex == -1 && (aFrameLine.position.debug_text.StartsWith("raw") || aFrameLine.rotation.debug_text == "anim+bone"))
                {
                    aFirstAnimationDescFrameLine.position.x = aFrameLine.position.y;
                    aFirstAnimationDescFrameLine.position.y = -aFrameLine.position.x;
                    aFirstAnimationDescFrameLine.position.z = aFrameLine.position.z;
                }
                else
                {
                    aFirstAnimationDescFrameLine.position.x = aFrameLine.position.x;
                    aFirstAnimationDescFrameLine.position.y = aFrameLine.position.y;
                    aFirstAnimationDescFrameLine.position.z = aFrameLine.position.z;
                }

                theMdlFileData.theFirstAnimationDescFrameLines.Add(boneIndex, aFirstAnimationDescFrameLine);
            }
        }

        public void WriteSkeletonSectionForAnimation(SourceMdlSequenceDescBase aSequenceDescBase, SourceMdlAnimationDescBase anAnimationDescBase)
        {
            string line = "";
            int boneIndex;
            AnimationFrameLine aFrameLine;
            AnimationFrameLine endFrameLine;
            // Dim previousFrameRootBonePosition As New SourceVector()
            var position = new SourceVector();
            var previousFrameRootBoneRotation = new SourceVector();
            var rotation = new SourceVector();
            SourceMdlSequenceDesc aSequenceDesc;
            SourceMdlAnimationDesc37 anAnimationDesc;
            // Dim tempValue As Double

            aSequenceDesc = (SourceMdlSequenceDesc)aSequenceDescBase;
            anAnimationDesc = (SourceMdlAnimationDesc37)anAnimationDescBase;

            // skeleton
            line = "skeleton";
            theOutputFileStreamWriter.WriteLine(line);
            theAnimationFrameLines = new SortedList<int, AnimationFrameLine>();
            for (int frameIndex = 0, loopTo = anAnimationDesc.frameCount - 1; frameIndex <= loopTo; frameIndex++)
            {
                theAnimationFrameLines.Clear();
                CalcAnimation(aSequenceDesc, anAnimationDesc, frameIndex);
                if (Program.TheApp.Settings.DecompileStricterFormatIsChecked)
                {
                    line = "time ";
                }
                else
                {
                    line = "  time ";
                }

                line += frameIndex.ToString();
                theOutputFileStreamWriter.WriteLine(line);
                endFrameLine = theAnimationFrameLines.Values[theAnimationFrameLines.Count - 1];
                for (int i = 0, loopTo1 = theAnimationFrameLines.Count - 1; i <= loopTo1; i++)
                {
                    boneIndex = theAnimationFrameLines.Keys[i];
                    aFrameLine = theAnimationFrameLines.Values[i];

                    // position.x = aFrameLine.position.x
                    // position.y = aFrameLine.position.y
                    // position.z = aFrameLine.position.z

                    // If Me.theMdlFileData.theBones(boneIndex).parentBoneIndex = -1 Then
                    // If anAnimationDesc.theMovements IsNot Nothing Then
                    // 'Dim perFrameMovement As Double
                    // Dim startFrameIndex As Integer = 0
                    // Dim endFrameIndex As Integer = 0

                    // 'If frameIndex = 0 Then
                    // '	previousFrameRootBonePosition.x = position.x
                    // '	previousFrameRootBonePosition.y = position.y
                    // '	previousFrameRootBonePosition.z = position.z
                    // 'End If

                    // For Each aMovement As SourceMdlMovement In anAnimationDesc.theMovements
                    // endFrameIndex = aMovement.endframeIndex

                    // 'If frameIndex >= startFrameIndex AndAlso frameIndex <= endFrameIndex Then
                    // '	If (aMovement.motionFlags And SourceMdlMovement.STUDIO_LX) > 0 Then
                    // '		perFrameMovement = aMovement.position.x / (endFrameIndex - startFrameIndex)
                    // '		position.x = previousFrameRootBonePosition.x + perFrameMovement
                    // '		aFrameLine.position.debug_text += " [x]"
                    // '	End If
                    // '	If (aMovement.motionFlags And SourceMdlMovement.STUDIO_LY) > 0 Then
                    // '		perFrameMovement = aMovement.position.y / (endFrameIndex - startFrameIndex)
                    // '		position.y = previousFrameRootBonePosition.y + perFrameMovement
                    // '		aFrameLine.position.debug_text += " [y]"
                    // '	End If
                    // '	If (aMovement.motionFlags And SourceMdlMovement.STUDIO_LZ) > 0 Then
                    // '		perFrameMovement = aMovement.position.z / (endFrameIndex - startFrameIndex)
                    // '		position.z = previousFrameRootBonePosition.z + perFrameMovement
                    // '		aFrameLine.position.debug_text += " [z]"
                    // '	End If
                    // 'End If
                    // '------
                    // 'Dim t As Double
                    // 'Dim scale As Double
                    // 't = frameIndex / (aMovement.endframeIndex - startFrameIndex + 1)
                    // 'scale = aMovement.v0 * t + 0.5 * (aMovement.v1 - aMovement.v0) * t * t
                    // 'If scale <> 0 Then
                    // '	scale = 1 / scale
                    // 'End If
                    // If frameIndex >= startFrameIndex AndAlso frameIndex <= endFrameIndex Then
                    // If (aMovement.motionFlags And SourceMdlMovement.STUDIO_LX) > 0 Then
                    // 'position.x = aFrameLine.position.x + scale * endFrameLine.position.x
                    // position.x = aFrameLine.position.x + aMovement.position.x
                    // aFrameLine.position.debug_text += " [x]"
                    // End If
                    // If (aMovement.motionFlags And SourceMdlMovement.STUDIO_LY) > 0 Then
                    // 'position.y = aFrameLine.position.y + scale * endFrameLine.position.y
                    // position.y = aFrameLine.position.y + aMovement.position.y
                    // aFrameLine.position.debug_text += " [y]"
                    // End If
                    // If (aMovement.motionFlags And SourceMdlMovement.STUDIO_LZ) > 0 Then
                    // 'position.z = aFrameLine.position.z + scale * endFrameLine.position.z
                    // position.z = aFrameLine.position.z + aMovement.position.z
                    // aFrameLine.position.debug_text += " [z]"
                    // End If
                    // End If

                    // startFrameIndex = endFrameIndex + 1
                    // Next

                    // 'If frameIndex > endFrameIndex AndAlso frameIndex < anAnimationDesc.frameCount Then
                    // '	position.x = previousFrameRootBonePosition.x
                    // '	position.y = previousFrameRootBonePosition.y
                    // '	position.z = previousFrameRootBonePosition.z
                    // 'End If

                    // 'previousFrameRootBonePosition.x = position.x
                    // 'previousFrameRootBonePosition.y = position.y
                    // 'previousFrameRootBonePosition.z = position.z
                    // End If

                    // tempValue = position.x
                    // position.x = position.y
                    // position.y = -tempValue
                    // End If

                    // rotation.x = aFrameLine.rotation.x
                    // rotation.y = aFrameLine.rotation.y
                    // rotation.z = aFrameLine.rotation.z
                    // If Me.theMdlFileData.theBones(boneIndex).parentBoneIndex = -1 Then
                    // 'If anAnimationDesc.theMovements IsNot Nothing Then
                    // '	Dim perFrameMovement As Double
                    // '	Dim startFrameIndex As Integer = 0
                    // '	Dim endFrameIndex As Integer = 0

                    // '	If frameIndex = 0 Then
                    // '		previousFrameRootBoneRotation.x = rotation.x
                    // '		previousFrameRootBoneRotation.y = rotation.y
                    // '		previousFrameRootBoneRotation.z = rotation.z
                    // '	End If

                    // '	For Each aMovement As SourceMdlMovement In anAnimationDesc.theMovements
                    // '		endFrameIndex = aMovement.endframeIndex

                    // '		If frameIndex >= startFrameIndex AndAlso frameIndex <= aMovement.endframeIndex Then
                    // '			If (aMovement.motionFlags And SourceMdlMovement.STUDIO_LXR) > 0 Then
                    // '				perFrameMovement = MathModule.DegreesToRadians(aMovement.angle) / (endFrameIndex - startFrameIndex)
                    // '				rotation.x = previousFrameRootBoneRotation.x + perFrameMovement
                    // '				aFrameLine.rotation.debug_text += " [x]"
                    // '			End If
                    // '			If (aMovement.motionFlags And SourceMdlMovement.STUDIO_LYR) > 0 Then
                    // '				perFrameMovement = MathModule.DegreesToRadians(aMovement.angle) / (endFrameIndex - startFrameIndex)
                    // '				rotation.y = previousFrameRootBoneRotation.y + perFrameMovement
                    // '				aFrameLine.rotation.debug_text += " [y]"
                    // '			End If
                    // '			If (aMovement.motionFlags And SourceMdlMovement.STUDIO_LZR) > 0 Then
                    // '				perFrameMovement = MathModule.DegreesToRadians(aMovement.angle) / (endFrameIndex - startFrameIndex)
                    // '				rotation.z = previousFrameRootBoneRotation.z + perFrameMovement
                    // '				aFrameLine.rotation.debug_text += " [z]"
                    // '			End If
                    // '		End If
                    // '	Next

                    // '	If frameIndex > endFrameIndex AndAlso frameIndex < anAnimationDesc.frameCount Then
                    // '		rotation.x = previousFrameRootBoneRotation.x
                    // '		rotation.y = previousFrameRootBoneRotation.y
                    // '		rotation.z = previousFrameRootBoneRotation.z
                    // '	End If

                    // '	previousFrameRootBoneRotation.x = rotation.x
                    // '	previousFrameRootBoneRotation.y = rotation.y
                    // '	previousFrameRootBoneRotation.z = rotation.z
                    // 'End If

                    // rotation.z = aFrameLine.rotation.z + MathModule.DegreesToRadians(-90)
                    // End If
                    // ------
                    var adjustedPosition = new SourceVector();
                    var adjustedRotation = new SourceVector();
                    AdjustPositionAndRotationByPiecewiseMovement(frameIndex, boneIndex, anAnimationDesc.theMovements, aFrameLine.position, aFrameLine.rotation, ref adjustedPosition, ref adjustedRotation);
                    AdjustPositionAndRotation(boneIndex, adjustedPosition, adjustedRotation, ref position, ref rotation);
                    line = "    ";
                    line += boneIndex.ToString(Program.TheApp.InternalNumberFormat);
                    line += " ";
                    line += position.x.ToString("0.000000", Program.TheApp.InternalNumberFormat);
                    line += " ";
                    line += position.y.ToString("0.000000", Program.TheApp.InternalNumberFormat);
                    line += " ";
                    line += position.z.ToString("0.000000", Program.TheApp.InternalNumberFormat);
                    line += " ";
                    line += rotation.x.ToString("0.000000", Program.TheApp.InternalNumberFormat);
                    line += " ";
                    line += rotation.y.ToString("0.000000", Program.TheApp.InternalNumberFormat);
                    line += " ";
                    line += rotation.z.ToString("0.000000", Program.TheApp.InternalNumberFormat);
                    if (Program.TheApp.Settings.DecompileDebugInfoFilesIsChecked)
                    {
                        line += "   # ";
                        line += "pos: ";
                        line += aFrameLine.position.debug_text;
                        line += "   ";
                        line += "rot: ";
                        line += aFrameLine.rotation.debug_text;
                    }

                    theOutputFileStreamWriter.WriteLine(line);
                }
            }

            line = "end";
            theOutputFileStreamWriter.WriteLine(line);
        }

        #endregion

        #region Private Delegates

        #endregion

        #region Private Methods

        private void AdjustPositionAndRotationByPiecewiseMovement(int frameIndex, int boneIndex, List<SourceMdlMovement> movements, SourceVector iPosition, SourceVector iRotation, ref SourceVector oPosition, ref SourceVector oRotation)
        {
            SourceMdlBone37 aBone;
            aBone = theMdlFileData.theBones[boneIndex];
            oPosition.x = iPosition.x;
            oPosition.y = iPosition.y;
            oPosition.z = iPosition.z;
            oPosition.debug_text = iPosition.debug_text;
            oRotation.x = iRotation.x;
            oRotation.y = iRotation.y;
            oRotation.z = iRotation.z;
            oRotation.debug_text = iRotation.debug_text;
            if (aBone.parentBoneIndex == -1)
            {
                if (movements is object && frameIndex > 0)
                {
                    int previousFrameIndex;
                    SourceVector vecPos;
                    SourceVector vecAngle;
                    previousFrameIndex = 0;
                    vecPos = new SourceVector();
                    vecAngle = new SourceVector();
                    foreach (SourceMdlMovement aMovement in movements)
                    {
                        if (frameIndex <= aMovement.endframeIndex)
                        {
                            double f;
                            double d;
                            f = (frameIndex - previousFrameIndex) / (double)(aMovement.endframeIndex - previousFrameIndex);
                            d = aMovement.v0 * f + 0.5d * (aMovement.v1 - aMovement.v0) * f * f;
                            vecPos.x = vecPos.x + d * aMovement.vector.x;
                            vecPos.y = vecPos.y + d * aMovement.vector.y;
                            vecPos.z = vecPos.z + d * aMovement.vector.z;
                            vecAngle.y = vecAngle.y * (1d - f) + MathModule.DegreesToRadians(aMovement.angle) * f;
                            break;
                        }
                        else
                        {
                            previousFrameIndex = aMovement.endframeIndex;
                            vecPos.x = aMovement.position.x;
                            vecPos.y = aMovement.position.y;
                            vecPos.z = aMovement.position.z;
                            vecAngle.y = MathModule.DegreesToRadians(aMovement.angle);
                        }
                    }

                    // TEST: Testing this in SourceSmdFile49.
                    // Dim tmp As New SourceVector()
                    // tmp.x = iPosition.x + vecPos.x
                    // tmp.y = iPosition.y + vecPos.y
                    // tmp.z = iPosition.z + vecPos.z
                    // 'oRotation.z = iRotation.z + vecAngle.y
                    // 'oPosition = MathModule.VectorYawRotate(tmp, -vecAngle.y)
                    oPosition.x = iPosition.x + vecPos.x;
                    oPosition.y = iPosition.y + vecPos.y;
                    oPosition.z = iPosition.z + vecPos.z;
                    oRotation.z = iRotation.z + vecAngle.y;
                }
            }
        }

        private void AdjustPositionAndRotation(int boneIndex, SourceVector iPosition, SourceVector iRotation, ref SourceVector oPosition, ref SourceVector oRotation)
        {
            SourceMdlBone37 aBone;
            aBone = theMdlFileData.theBones[boneIndex];
            if (aBone.parentBoneIndex == -1)
            {
                oPosition.x = iPosition.y;
                oPosition.y = -iPosition.x;
                oPosition.z = iPosition.z;
            }
            else
            {
                oPosition.x = iPosition.x;
                oPosition.y = iPosition.y;
                oPosition.z = iPosition.z;
            }

            if (aBone.parentBoneIndex == -1)
            {
                oRotation.x = iRotation.x;
                oRotation.y = iRotation.y;
                oRotation.z = iRotation.z + MathModule.DegreesToRadians(-90);
            }
            else
            {
                oRotation.x = iRotation.x;
                oRotation.y = iRotation.y;
                oRotation.z = iRotation.z;
            }
        }

        private string WriteVertexLine(SourceVtxStripGroup06 aStripGroup, int aVtxIndexIndex, int lodIndex, int meshVertexIndexStart, int bodyPartVertexIndexStart, SourceMdlModel37 aBodyModel)
        {
            ushort aVtxVertexIndex;
            SourceVtxVertex06 aVtxVertex;
            SourceMdlVertex37 aVertex;
            int vertexIndex;
            string line;
            line = "";
            try
            {
                aVtxVertexIndex = aStripGroup.theVtxIndexes[aVtxIndexIndex];
                aVtxVertex = aStripGroup.theVtxVertexes[aVtxVertexIndex];
                // vertexIndex = aVtxVertex.originalMeshVertexIndex + bodyPartVertexIndexStart + meshVertexIndexStart
                // aVertex = Me.theMdlFileData.theBodyParts(0).theModels(0).theVertexes(vertexIndex)
                vertexIndex = aVtxVertex.originalMeshVertexIndex + meshVertexIndexStart;
                aVertex = aBodyModel.theVertexes[vertexIndex];
                line = "  ";
                line += aVertex.boneWeight.bone[0].ToString(Program.TheApp.InternalNumberFormat);
                line += " ";
                if ((theMdlFileData.flags & SourceMdlFileData.STUDIOHDR_FLAGS_STATIC_PROP) > 0)
                {
                    line += aVertex.position.y.ToString("0.000000", Program.TheApp.InternalNumberFormat);
                    line += " ";
                    line += (-aVertex.position.x).ToString("0.000000", Program.TheApp.InternalNumberFormat);
                }
                else
                {
                    line += aVertex.position.x.ToString("0.000000", Program.TheApp.InternalNumberFormat);
                    line += " ";
                    line += aVertex.position.y.ToString("0.000000", Program.TheApp.InternalNumberFormat);
                }

                line += " ";
                line += aVertex.position.z.ToString("0.000000", Program.TheApp.InternalNumberFormat);
                line += " ";
                if ((theMdlFileData.flags & SourceMdlFileData.STUDIOHDR_FLAGS_STATIC_PROP) > 0)
                {
                    line += aVertex.normal.y.ToString("0.000000", Program.TheApp.InternalNumberFormat);
                    line += " ";
                    line += (-aVertex.normal.x).ToString("0.000000", Program.TheApp.InternalNumberFormat);
                }
                else
                {
                    line += aVertex.normal.x.ToString("0.000000", Program.TheApp.InternalNumberFormat);
                    line += " ";
                    line += aVertex.normal.y.ToString("0.000000", Program.TheApp.InternalNumberFormat);
                }

                line += " ";
                line += aVertex.normal.z.ToString("0.000000", Program.TheApp.InternalNumberFormat);
                line += " ";
                line += aVertex.texCoordX.ToString("0.000000", Program.TheApp.InternalNumberFormat);
                line += " ";
                // line += aVertex.texCoordY.ToString("0.000000", TheApp.InternalNumberFormat)
                line += (1d - aVertex.texCoordY).ToString("0.000000", Program.TheApp.InternalNumberFormat);
                line += " ";
                line += aVertex.boneWeight.boneCount.ToString(Program.TheApp.InternalNumberFormat);
                for (int boneWeightBoneIndex = 0, loopTo = aVertex.boneWeight.boneCount - 1; boneWeightBoneIndex <= loopTo; boneWeightBoneIndex++)
                {
                    line += " ";
                    line += aVertex.boneWeight.bone[boneWeightBoneIndex].ToString(Program.TheApp.InternalNumberFormat);
                    line += " ";
                    line += aVertex.boneWeight.weight[boneWeightBoneIndex].ToString("0.000000", Program.TheApp.InternalNumberFormat);
                }
            }
            // Me.theOutputFileStreamWriter.WriteLine(line)
            catch (Exception ex)
            {
                line = "// " + line;
            }

            return line;
        }

        private SourceVector TransformPhyVertex(SourceMdlBone37 aBone, SourceVector vertex, SourcePhyPhysCollisionModel aSourcePhysCollisionModel)
        {
            var aVectorTransformed = new SourceVector();
            var aVector = new SourceVector();

            // aVector.x = 1 / 0.0254 * vertex.x
            // aVector.y = 1 / 0.0254 * vertex.z
            // aVector.z = 1 / 0.0254 * -vertex.y
            // Dim aParentBone As SourceMdlBone37
            // Dim aChildBone As SourceMdlBone37
            // Dim parentBoneIndex As Integer
            // Dim inputBoneMatrixColumn0 As New SourceVector()
            // Dim inputBoneMatrixColumn1 As New SourceVector()
            // Dim inputBoneMatrixColumn2 As New SourceVector()
            // Dim inputBoneMatrixColumn3 As New SourceVector()
            // Dim boneMatrixColumn0 As New SourceVector()
            // Dim boneMatrixColumn1 As New SourceVector()
            // Dim boneMatrixColumn2 As New SourceVector()
            // Dim boneMatrixColumn3 As New SourceVector()

            // aChildBone = aBone
            // inputBoneMatrixColumn0.x = aChildBone.poseToBoneColumn0.x
            // inputBoneMatrixColumn0.y = aChildBone.poseToBoneColumn0.y
            // inputBoneMatrixColumn0.z = aChildBone.poseToBoneColumn0.z
            // inputBoneMatrixColumn1.x = aChildBone.poseToBoneColumn1.x
            // inputBoneMatrixColumn1.y = aChildBone.poseToBoneColumn1.y
            // inputBoneMatrixColumn1.z = aChildBone.poseToBoneColumn1.z
            // inputBoneMatrixColumn2.x = aChildBone.poseToBoneColumn2.x
            // inputBoneMatrixColumn2.y = aChildBone.poseToBoneColumn2.y
            // inputBoneMatrixColumn2.z = aChildBone.poseToBoneColumn2.z
            // inputBoneMatrixColumn3.x = aChildBone.poseToBoneColumn3.x
            // inputBoneMatrixColumn3.y = aChildBone.poseToBoneColumn3.y
            // inputBoneMatrixColumn3.z = aChildBone.poseToBoneColumn3.z
            // While True
            // parentBoneIndex = aChildBone.parentBoneIndex
            // If parentBoneIndex = -1 Then
            // aVectorTransformed = MathModule.VectorITransform(aVector, inputBoneMatrixColumn0, inputBoneMatrixColumn1, inputBoneMatrixColumn2, inputBoneMatrixColumn3)
            // Exit While
            // Else
            // aParentBone = Me.theMdlFileData.theBones(parentBoneIndex)
            // MathModule.R_ConcatTransforms(aParentBone.poseToBoneColumn0, aParentBone.poseToBoneColumn1, aParentBone.poseToBoneColumn2, aParentBone.poseToBoneColumn3, inputBoneMatrixColumn0, inputBoneMatrixColumn1, inputBoneMatrixColumn2, inputBoneMatrixColumn3, boneMatrixColumn0, boneMatrixColumn1, boneMatrixColumn2, boneMatrixColumn3)
            // aChildBone = aParentBone
            // inputBoneMatrixColumn0.x = boneMatrixColumn0.x
            // inputBoneMatrixColumn0.y = boneMatrixColumn0.y
            // inputBoneMatrixColumn0.z = boneMatrixColumn0.z
            // inputBoneMatrixColumn1.x = boneMatrixColumn1.x
            // inputBoneMatrixColumn1.y = boneMatrixColumn1.y
            // inputBoneMatrixColumn1.z = boneMatrixColumn1.z
            // inputBoneMatrixColumn2.x = boneMatrixColumn2.x
            // inputBoneMatrixColumn2.y = boneMatrixColumn2.y
            // inputBoneMatrixColumn2.z = boneMatrixColumn2.z
            // inputBoneMatrixColumn3.x = boneMatrixColumn3.x
            // inputBoneMatrixColumn3.y = boneMatrixColumn3.y
            // inputBoneMatrixColumn3.z = boneMatrixColumn3.z
            // End If
            // End While
            // ======
            // TODO: Probably not the correct way, but it works for bullsquid and ship01.
            aVector.x = 1d / 0.0254d * vertex.x;
            aVector.y = 1d / 0.0254d * vertex.z;
            aVector.z = 1d / 0.0254d * -vertex.y;
            if (aSourcePhysCollisionModel is object)
            {
                if (theMdlFileData.theBoneNameToBoneIndexMap.ContainsKey(aSourcePhysCollisionModel.theName))
                {
                    aBone = theMdlFileData.theBones[theMdlFileData.theBoneNameToBoneIndexMap[aSourcePhysCollisionModel.theName]];
                }
                else
                {
                    aVectorTransformed.x = 1d / 0.0254d * vertex.z;
                    aVectorTransformed.y = 1d / 0.0254d * -vertex.x;
                    aVectorTransformed.z = 1d / 0.0254d * -vertex.y;
                    return aVectorTransformed;
                }
            }

            aVectorTransformed = MathModule.VectorITransform(aVector, aBone.poseToBoneColumn0, aBone.poseToBoneColumn1, aBone.poseToBoneColumn2, aBone.poseToBoneColumn3);
            return aVectorTransformed;
        }

        // 'NOTE: From disassembling of MDL Decompiler with OllyDbg, the following calculations are used in VPHYSICS.DLL for each face:
        // '      convertedZ = 1.0 / 0.0254 * lastVertex.position.z
        // '      convertedY = 1.0 / 0.0254 * -lastVertex.position.y
        // '      convertedX = 1.0 / 0.0254 * lastVertex.position.x
        // 'NOTE: From disassembling of MDL Decompiler with OllyDbg, the following calculations are used after above for each vertex:
        // '      newValue1 = unknownZ1 * convertedZ + unknownY1 * convertedY + unknownX1 * convertedX + unknownW1
        // '      newValue2 = unknownZ2 * convertedZ + unknownY2 * convertedY + unknownX2 * convertedX + unknownW2
        // '      newValue3 = unknownZ3 * convertedZ + unknownY3 * convertedY + unknownX3 * convertedX + unknownW3
        // 'Seems to be same as this code:
        // 'Dim aBone As SourceMdlBone
        // 'aBone = Me.theSourceEngineModel.theMdlFileHeader.theBones(anEyeball.boneIndex)
        // 'eyeballPosition = MathModule.VectorITransform(anEyeball.org, aBone.poseToBoneColumn0, aBone.poseToBoneColumn1, aBone.poseToBoneColumn2, aBone.poseToBoneColumn3)
        // Private Function TransformPhyVertex(ByVal aBone As SourceMdlBone, ByVal vertex As SourceVector) As SourceVector
        // Dim aVectorTransformed As New SourceVector
        // Dim aVector As New SourceVector()

        // 'NOTE: Too small.
        // 'aVectorTransformed.x = vertex.x
        // 'aVectorTransformed.y = vertex.y
        // 'aVectorTransformed.z = vertex.z
        // '------
        // 'NOTE: Rotated for:
        // '      simple_shape
        // '      L4D2 w_models\weapons\w_minigun
        // 'aVectorTransformed.x = 1 / 0.0254 * vertex.x
        // 'aVectorTransformed.y = 1 / 0.0254 * vertex.y
        // 'aVectorTransformed.z = 1 / 0.0254 * vertex.z
        // '------
        // 'NOTE: Works for:
        // '      simple_shape
        // '      L4D2 w_models\weapons\w_minigun
        // '      L4D2 w_models\weapons\w_smg_uzi
        // '      L4D2 props_vehicles\van
        // 'aVectorTransformed.x = 1 / 0.0254 * vertex.z
        // 'aVectorTransformed.y = 1 / 0.0254 * -vertex.x
        // 'aVectorTransformed.z = 1 / 0.0254 * -vertex.y
        // '------
        // 'NOTE: Rotated for:
        // '      L4D2 w_models\weapons\w_minigun
        // 'aVectorTransformed.x = 1 / 0.0254 * vertex.x
        // 'aVectorTransformed.y = 1 / 0.0254 * -vertex.y
        // 'aVectorTransformed.z = 1 / 0.0254 * vertex.z
        // '------
        // 'NOTE: Rotated for:
        // '      L4D2 props_vehicles\van
        // 'aVectorTransformed.x = 1 / 0.0254 * vertex.z
        // 'aVectorTransformed.y = 1 / 0.0254 * -vertex.y
        // 'aVectorTransformed.z = 1 / 0.0254 * vertex.x
        // '------
        // 'NOTE: Rotated for:
        // '      L4D2 w_models\weapons\w_minigun
        // 'aVector.x = 1 / 0.0254 * vertex.x
        // 'aVector.y = 1 / 0.0254 * vertex.y
        // 'aVector.z = 1 / 0.0254 * vertex.z
        // 'aVectorTransformed = MathModule.VectorITransform(aVector, aBone.poseToBoneColumn0, aBone.poseToBoneColumn1, aBone.poseToBoneColumn2, aBone.poseToBoneColumn3)
        // '------
        // 'NOTE: Rotated for:
        // '      L4D2 w_models\weapons\w_minigun
        // 'aVector.x = 1 / 0.0254 * vertex.x
        // 'aVector.y = 1 / 0.0254 * -vertex.y
        // 'aVector.z = 1 / 0.0254 * vertex.z
        // 'aVectorTransformed = MathModule.VectorITransform(aVector, aBone.poseToBoneColumn0, aBone.poseToBoneColumn1, aBone.poseToBoneColumn2, aBone.poseToBoneColumn3)
        // '------
        // 'NOTE: Works for:
        // '      L4D2 w_models\weapons\w_minigun
        // '      L4D2 w_models\weapons\w_smg_uzi
        // 'NOTE: Rotated for:
        // '      simple_shape
        // '      L4D2 props_vehicles\van
        // 'NOTE: Each mesh piece rotated for:
        // '      L4D2 survivors\survivor_producer
        // 'aVector.x = 1 / 0.0254 * vertex.z
        // 'aVector.y = 1 / 0.0254 * -vertex.y
        // 'aVector.z = 1 / 0.0254 * vertex.x
        // 'aVectorTransformed = MathModule.VectorITransform(aVector, aBone.poseToBoneColumn0, aBone.poseToBoneColumn1, aBone.poseToBoneColumn2, aBone.poseToBoneColumn3)
        // '------
        // 'NOTE: Works for:
        // '      simple_shape
        // '      L4D2 props_vehicles\van
        // '      L4D2 survivors\survivor_producer
        // '      L4D2 w_models\weapons\w_autoshot_m4super
        // '      L4D2 w_models\weapons\w_desert_eagle
        // '      L4D2 w_models\weapons\w_minigun
        // '      L4D2 w_models\weapons\w_rifle_m16a2
        // '      L4D2 w_models\weapons\w_smg_uzi
        // 'NOTE: Rotated for:
        // '      L4D2 w_models\weapons\w_desert_rifle
        // '      L4D2 w_models\weapons\w_shotgun_spas
        // If Me.thePhyFileData.theSourcePhyIsCollisionModel Then
        // aVectorTransformed.x = 1 / 0.0254 * vertex.z
        // aVectorTransformed.y = 1 / 0.0254 * -vertex.x
        // aVectorTransformed.z = 1 / 0.0254 * -vertex.y
        // Else
        // aVector.x = 1 / 0.0254 * vertex.x
        // aVector.y = 1 / 0.0254 * vertex.z
        // aVector.z = 1 / 0.0254 * -vertex.y
        // aVectorTransformed = MathModule.VectorITransform(aVector, aBone.poseToBoneColumn0, aBone.poseToBoneColumn1, aBone.poseToBoneColumn2, aBone.poseToBoneColumn3)
        // End If



        // '------
        // 'NOTE: Works for:
        // '      survivor_producer
        // 'NOTE: Does not work for:
        // '      w_smg_uzi()
        // 'phyVertex.x = 1 / 0.0254 * aVector.x
        // 'phyVertex.y = 1 / 0.0254 * aVector.z
        // 'phyVertex.z = 1 / 0.0254 * -aVector.y
        // '------
        // 'NOTE: These two lines match orientation for cstrike it_lampholder1 model, 
        // '      but still doesn't compile properly.
        // 'NOTE: Does not work for:
        // '      w_smg_uzi()
        // 'phyVertex.x = 1 / 0.0254 * aVector.z
        // 'phyVertex.y = 1 / 0.0254 * -aVector.x
        // 'phyVertex.z = 1 / 0.0254 * -aVector.y
        // '------
        // 'NOTE: Does not work for:
        // '      w_smg_uzi()
        // 'phyVertex.x = 1 / 0.0254 * aVector.y
        // 'phyVertex.y = 1 / 0.0254 * aVector.x
        // 'phyVertex.z = 1 / 0.0254 * -aVector.z
        // '------
        // 'NOTE: Does not work for:
        // '      w_smg_uzi()
        // 'phyVertex.x = 1 / 0.0254 * aVector.x
        // 'phyVertex.y = 1 / 0.0254 * aVector.y
        // 'phyVertex.z = 1 / 0.0254 * -aVector.z
        // '------
        // 'NOTE: Does not work for:
        // '      w_smg_uzi()
        // 'phyVertex.x = 1 / 0.0254 * -aVector.y
        // 'phyVertex.y = 1 / 0.0254 * aVector.x
        // 'phyVertex.z = 1 / 0.0254 * aVector.z
        // '------
        // 'NOTE: Does not work for:
        // '      w_smg_uzi()
        // 'phyVertex.x = 1 / 0.0254 * -aVector.y
        // 'phyVertex.y = 1 / 0.0254 * aVector.x
        // 'phyVertex.z = 1 / 0.0254 * aVector.z
        // '------
        // 'NOTE: Does not work for:
        // '      w_smg_uzi()
        // 'phyVertex.x = 1 / 0.0254 * aVector.z
        // 'phyVertex.y = 1 / 0.0254 * aVector.y
        // 'phyVertex.z = 1 / 0.0254 * aVector.x
        // '------
        // 'NOTE: Works for:
        // '      w_smg_uzi()
        // 'NOTE: Does not work for:
        // '      survivor_producer
        // 'phyVertex.x = 1 / 0.0254 * aVector.z
        // 'phyVertex.y = 1 / 0.0254 * -aVector.y
        // 'phyVertex.z = 1 / 0.0254 * aVector.x
        // '------
        // 'phyVertex.x = 1 / 0.0254 * aVector.z
        // 'phyVertex.y = 1 / 0.0254 * -aVector.y
        // 'phyVertex.z = 1 / 0.0254 * -aVector.x
        // '------
        // ''TODO: Find some rationale for why phys model is rotated differently for different models.
        // ''      Possibly due to rotation needed to transfrom from pose to bone position.
        // ''If Me.theSourceEngineModel.theMdlFileHeader.theAnimationDescs.Count < 2 Then
        // ''If (theSourceEngineModel.theMdlFileHeader.flags And SourceMdlFileHeader.STUDIOHDR_FLAGS_STATIC_PROP) > 0 Then
        // 'If Me.theSourceEngineModel.thePhyFileHeader.theSourcePhyIsCollisionModel Then
        // '	'TEST: Does not rotate L4D2's van phys mesh correctly.
        // '	'aVector.x = 1 / 0.0254 * phyVertex.vertex.x
        // '	'aVector.y = 1 / 0.0254 * phyVertex.vertex.y
        // '	'aVector.z = 1 / 0.0254 * phyVertex.vertex.z
        // '	'TEST:  Does not rotate L4D2's van phys mesh correctly.
        // '	'aVector.x = 1 / 0.0254 * phyVertex.vertex.y
        // '	'aVector.y = 1 / 0.0254 * -phyVertex.vertex.x
        // '	'aVector.z = 1 / 0.0254 * phyVertex.vertex.z
        // '	'TEST: Does not rotate L4D2's van phys mesh correctly.
        // '	'aVector.x = 1 / 0.0254 * phyVertex.vertex.z
        // '	'aVector.y = 1 / 0.0254 * -phyVertex.vertex.y
        // '	'aVector.z = 1 / 0.0254 * phyVertex.vertex.x
        // '	'TEST: Does not rotate L4D2's van phys mesh correctly.
        // '	'aVector.x = 1 / 0.0254 * phyVertex.vertex.x
        // '	'aVector.y = 1 / 0.0254 * phyVertex.vertex.z
        // '	'aVector.z = 1 / 0.0254 * -phyVertex.vertex.y
        // '	'TEST: Works for L4D2's van phys mesh.
        // '	'      Does not work for L4D2 w_model\weapons\w_minigun.mdl.
        // '	aVector.x = 1 / 0.0254 * vertex.z
        // '	aVector.y = 1 / 0.0254 * -vertex.x
        // '	aVector.z = 1 / 0.0254 * -vertex.y

        // '	aVectorTransformed = MathModule.VectorITransform(aVector, aBone.poseToBoneColumn0, aBone.poseToBoneColumn1, aBone.poseToBoneColumn2, aBone.poseToBoneColumn3)

        // '	'======

        // '	'Dim aVectorTransformed2 As SourceVector
        // '	''aVectorTransformed2 = New SourceVector()
        // '	''aVectorTransformed2 = MathModule.VectorITransform(aVector, aBone.poseToBoneColumn0, aBone.poseToBoneColumn1, aBone.poseToBoneColumn2, aBone.poseToBoneColumn3)
        // '	''aVectorTransformed2.x = aVector.x
        // '	''aVectorTransformed2.y = aVector.y
        // '	''aVectorTransformed2.z = aVector.z

        // '	'aVectorTransformed = MathModule.VectorTransform(aVector, aBone.poseToBoneColumn0, aBone.poseToBoneColumn1, aBone.poseToBoneColumn2, aBone.poseToBoneColumn3)
        // '	''aVectorTransformed = MathModule.VectorTransform(aVectorTransformed2, aBone.poseToBoneColumn0, aBone.poseToBoneColumn1, aBone.poseToBoneColumn2, aBone.poseToBoneColumn3)
        // '	''aVectorTransformed = New SourceVector()
        // '	''aVectorTransformed.x = aVectorTransformed2.x
        // '	''aVectorTransformed.y = aVectorTransformed2.y
        // '	''aVectorTransformed.z = aVectorTransformed2.z
        // 'Else
        // '	'TEST: Does not work for L4D2 w_model\weapons\w_minigun.mdl.
        // '	aVector.x = 1 / 0.0254 * vertex.x
        // '	aVector.y = 1 / 0.0254 * vertex.z
        // '	aVector.z = 1 / 0.0254 * -vertex.y

        // '	aVectorTransformed = MathModule.VectorITransform(aVector, aBone.poseToBoneColumn0, aBone.poseToBoneColumn1, aBone.poseToBoneColumn2, aBone.poseToBoneColumn3)
        // 'End If
        // '------
        // 'TEST: Does not rotate L4D2's van phys mesh correctly.
        // 'aVector.x = 1 / 0.0254 * phyVertex.vertex.x
        // 'aVector.y = 1 / 0.0254 * phyVertex.vertex.y
        // 'aVector.z = 1 / 0.0254 * phyVertex.vertex.z
        // 'TEST: Does not rotate L4D2's van phys mesh correctly.
        // 'aVector.x = 1 / 0.0254 * phyVertex.vertex.y
        // 'aVector.y = 1 / 0.0254 * -phyVertex.vertex.x
        // 'aVector.z = 1 / 0.0254 * phyVertex.vertex.z
        // 'TEST: works for survivor_producer; matches ref and phy meshes of van, but both are rotated 90 degrees on z-axis
        // 'aVector.x = 1 / 0.0254 * phyVertex.vertex.x
        // 'aVector.y = 1 / 0.0254 * phyVertex.vertex.z
        // 'aVector.z = 1 / 0.0254 * -phyVertex.vertex.y

        // 'aVectorTransformed = MathModule.VectorITransform(aVector, aBone.poseToBoneColumn0, aBone.poseToBoneColumn1, aBone.poseToBoneColumn2, aBone.poseToBoneColumn3)
        // ''------
        // '''TEST: Only rotate by -90 deg if bone is a root bone.  Do not know why.
        // ''If aBone.parentBoneIndex = -1 Then
        // ''	aVectorTransformed = MathModule.RotateAboutZAxis(aVectorTransformed, MathModule.DegreesToRadians(-90), aBone)
        // ''End If

        // Return aVectorTransformed
        // End Function

        // static void CalcAnimation( const CStudioHdr *pStudioHdr,	Vector *pos, Quaternion *q, 
        // mstudioseqdesc_t &seqdesc,
        // int sequence, int animation,
        // float cycle, int boneMask )
        // {
        // int					i;
        // 
        // mstudioanimdesc_t &animdesc = pStudioHdr->pAnimdesc( animation );
        // mstudiobone_t *pbone = pStudioHdr->pBone( 0 );
        // mstudioanim_t *panim = animdesc.pAnim( );
        // 
        // int					iFrame;
        // float				s;
        // 
        // float fFrame = cycle * (animdesc.numframes - 1);
        // 
        // iFrame = (int)fFrame;
        // s = (fFrame - iFrame);
        // 
        // float *pweight = seqdesc.pBoneweight( 0 );
        // 
        // for (i = 0; i < pStudioHdr->numbones(); i++, pbone++, pweight++)
        // {
        // if (panim && panim->bone == i)
        // {
        // if (*pweight > 0 && (pbone->flags & boneMask))
        // {
        // CalcBoneQuaternion( iFrame, s, pbone, panim, q[i] );
        // CalcBonePosition  ( iFrame, s, pbone, panim, pos[i] );
        // }
        // panim = panim->pNext();
        // }
        // else if (*pweight > 0 && (pbone->flags & boneMask))
        // {
        // if (animdesc.flags & STUDIO_DELTA)
        // {
        // q[i].Init( 0.0f, 0.0f, 0.0f, 1.0f );
        // pos[i].Init( 0.0f, 0.0f, 0.0f );
        // }
        // else
        // {
        // q[i] = pbone->quat;
        // pos[i] = pbone->pos;
        // }
        // }
        // }
        // }
        // ======
        // FROM: SourceEngine2007_source\src_main\public\bone_setup.cpp
        // //-----------------------------------------------------------------------------
        // // Purpose: Find and decode a sub-frame of animation
        // //-----------------------------------------------------------------------------
        // 
        // static void CalcAnimation( const CStudioHdr *pStudioHdr,	Vector *pos, Quaternion *q, 
        // mstudioseqdesc_t &seqdesc,
        // int sequence, int animation,
        // float cycle, int boneMask )
        // {
        // #ifdef STUDIO_ENABLE_PERF_COUNTERS
        // pStudioHdr->m_nPerfAnimationLayers++;
        // #endif
        // 
        // virtualmodel_t *pVModel = pStudioHdr->GetVirtualModel();
        // 
        // if (pVModel)
        // {
        // CalcVirtualAnimation( pVModel, pStudioHdr, pos, q, seqdesc, sequence, animation, cycle, boneMask );
        // return;
        // }
        // 
        // mstudioanimdesc_t &animdesc = pStudioHdr->pAnimdesc( animation );
        // mstudiobone_t *pbone = pStudioHdr->pBone( 0 );
        // const mstudiolinearbone_t *pLinearBones = pStudioHdr->pLinearBones();
        // 
        // int					i;
        // int					iFrame;
        // float				s;
        // 
        // float fFrame = cycle * (animdesc.numframes - 1);
        // 
        // iFrame = (int)fFrame;
        // s = (fFrame - iFrame);
        // 
        // int iLocalFrame = iFrame;
        // float flStall;
        // mstudioanim_t *panim = animdesc.pAnim( &iLocalFrame, flStall );
        // 
        // float *pweight = seqdesc.pBoneweight( 0 );
        // 
        // // if the animation isn't available, look for the zero frame cache
        // if (!panim)
        // {
        // // Msg("zeroframe %s\n", animdesc.pszName() );
        // // pre initialize
        // for (i = 0; i < pStudioHdr->numbones(); i++, pbone++, pweight++)
        // {
        // if (*pweight > 0 && (pStudioHdr->boneFlags(i) & boneMask))
        // {
        // if (animdesc.flags & STUDIO_DELTA)
        // {
        // q[i].Init( 0.0f, 0.0f, 0.0f, 1.0f );
        // pos[i].Init( 0.0f, 0.0f, 0.0f );
        // }
        // else
        // {
        // q[i] = pbone->quat;
        // pos[i] = pbone->pos;
        // }
        // }
        // }
        // 
        // CalcZeroframeData( pStudioHdr, pStudioHdr->GetRenderHdr(), NULL, pStudioHdr->pBone( 0 ), animdesc, fFrame, pos, q, boneMask, 1.0 );
        // 
        // return;
        // }
        // 
        // // BUGBUG: the sequence, the anim, and the model can have all different bone mappings.
        // for (i = 0; i < pStudioHdr->numbones(); i++, pbone++, pweight++)
        // {
        // if (panim && panim->bone == i)
        // {
        // if (*pweight > 0 && (pStudioHdr->boneFlags(i) & boneMask))
        // {
        // CalcBoneQuaternion( iLocalFrame, s, pbone, pLinearBones, panim, q[i] );
        // CalcBonePosition  ( iLocalFrame, s, pbone, pLinearBones, panim, pos[i] );
        // #ifdef STUDIO_ENABLE_PERF_COUNTERS
        // pStudioHdr->m_nPerfAnimatedBones++;
        // pStudioHdr->m_nPerfUsedBones++;
        // #endif
        // }
        // panim = panim->pNext();
        // }
        // else if (*pweight > 0 && (pStudioHdr->boneFlags(i) & boneMask))
        // {
        // if (animdesc.flags & STUDIO_DELTA)
        // {
        // q[i].Init( 0.0f, 0.0f, 0.0f, 1.0f );
        // pos[i].Init( 0.0f, 0.0f, 0.0f );
        // }
        // else
        // {
        // q[i] = pbone->quat;
        // pos[i] = pbone->pos;
        // }
        // #ifdef STUDIO_ENABLE_PERF_COUNTERS
        // pStudioHdr->m_nPerfUsedBones++;
        // #endif
        // }
        // }
        // 
        // // cross fade in previous zeroframe data
        // if (flStall > 0.0f)
        // {
        // CalcZeroframeData( pStudioHdr, pStudioHdr->GetRenderHdr(), NULL, pStudioHdr->pBone( 0 ), animdesc, fFrame, pos, q, boneMask, flStall );
        // }
        // 
        // if (animdesc.numlocalhierarchy)
        // {
        // matrix3x4_t *boneToWorld = g_MatrixPool.Alloc();
        // CBoneBitList boneComputed;
        // 
        // int i;
        // for (i = 0; i < animdesc.numlocalhierarchy; i++)
        // {
        // mstudiolocalhierarchy_t *pHierarchy = animdesc.pHierarchy( i );
        // 
        // if ( !pHierarchy )
        // break;
        // 
        // if (pStudioHdr->boneFlags(pHierarchy->iBone) & boneMask)
        // {
        // if (pStudioHdr->boneFlags(pHierarchy->iNewParent) & boneMask)
        // {
        // CalcLocalHierarchyAnimation( pStudioHdr, boneToWorld, boneComputed, pos, q, pbone, pHierarchy, pHierarchy->iBone, pHierarchy->iNewParent, cycle, iFrame, s, boneMask );
        // }
        // }
        // }
        // 
        // g_MatrixPool.Free( boneToWorld );
        // }
        // 
        // }
        private void CalcAnimation(SourceMdlSequenceDesc aSequenceDesc, SourceMdlAnimationDesc37 anAnimationDesc, int frameIndex)
        {
            double s;
            SourceMdlBone37 aBone;
            SourceMdlAnimation37 anAnimation;
            SourceVector rot;
            SourceVector pos;
            AnimationFrameLine aFrameLine;
            s = 0d;
            for (int boneIndex = 0, loopTo = theMdlFileData.theBones.Count - 1; boneIndex <= loopTo; boneIndex++)
            {
                aBone = theMdlFileData.theBones[boneIndex];
                anAnimation = anAnimationDesc.theAnimations[boneIndex];
                if (anAnimation is object)
                {
                    if (theAnimationFrameLines.ContainsKey(boneIndex))
                    {
                        aFrameLine = theAnimationFrameLines[boneIndex];
                    }
                    else
                    {
                        aFrameLine = new AnimationFrameLine();
                        theAnimationFrameLines.Add(boneIndex, aFrameLine);
                    }

                    aFrameLine.rotationQuat = new SourceQuaternion();
                    rot = CalcBoneRotation(frameIndex, s, aBone, anAnimation, ref aFrameLine.rotationQuat);
                    aFrameLine.rotation = new SourceVector();
                    aFrameLine.rotation.x = rot.x;
                    aFrameLine.rotation.y = rot.y;
                    aFrameLine.rotation.z = rot.z;
                    aFrameLine.rotation.debug_text = rot.debug_text;
                    pos = CalcBonePosition(frameIndex, s, aBone, anAnimation);
                    aFrameLine.position = new SourceVector();
                    aFrameLine.position.x = pos.x;
                    aFrameLine.position.y = pos.y;
                    aFrameLine.position.z = pos.z;
                    aFrameLine.position.debug_text = pos.debug_text;
                }
            }
        }

        private SourceVector CalcBoneRotation(int frameIndex, double s, SourceMdlBone37 aBone, SourceMdlAnimation37 anAnimation, ref SourceQuaternion rotationQuat)
        {
            var angleVector = new SourceVector();
            if ((anAnimation.flags & SourceMdlAnimation37.STUDIO_ROT_ANIMATED) > 0)
            {
                if (anAnimation.animationValueOffsets[3] <= 0)
                {
                    // angleVector.x = 0
                    angleVector.x = aBone.rotation.x;
                }
                else
                {
                    angleVector.x = ExtractAnimValue(frameIndex, anAnimation.theAnimationValues[3], aBone.rotationScale.x);
                    // angle1[j] = pbone->value[j+3] + angle1[j] * pbone->scale[j+3];
                    angleVector.x = aBone.rotation.x + angleVector.x * aBone.rotationScale.x;
                }

                if (anAnimation.animationValueOffsets[4] <= 0)
                {
                    // angleVector.y = 0
                    angleVector.y = aBone.rotation.y;
                }
                else
                {
                    angleVector.y = ExtractAnimValue(frameIndex, anAnimation.theAnimationValues[4], aBone.rotationScale.y);
                    angleVector.y = aBone.rotation.y + angleVector.y * aBone.rotationScale.y;
                }

                if (anAnimation.animationValueOffsets[5] <= 0)
                {
                    // angleVector.z = 0
                    angleVector.z = aBone.rotation.z;
                }
                else
                {
                    angleVector.z = ExtractAnimValue(frameIndex, anAnimation.theAnimationValues[5], aBone.rotationScale.z);
                    angleVector.z = aBone.rotation.z + angleVector.z * aBone.rotationScale.z;
                }

                rotationQuat = MathModule.EulerAnglesToQuaternion(angleVector);
                angleVector.debug_text = "anim";
            }
            else
            {
                // rotationQuat = anAnimation.rotationQuat
                rotationQuat.x = anAnimation.rotationQuat.x;
                rotationQuat.y = anAnimation.rotationQuat.y;
                rotationQuat.z = anAnimation.rotationQuat.z;
                rotationQuat.w = anAnimation.rotationQuat.w;
                angleVector = MathModule.ToEulerAngles(rotationQuat);
                angleVector.debug_text = "rot";
            }

            return angleVector;
        }

        // FROM: SourceEngine2007_source\public\bone_setup.cpp
        // //-----------------------------------------------------------------------------
        // // Purpose: return a sub frame position for a single bone
        // //-----------------------------------------------------------------------------
        // void CalcBonePosition(	int frame, float s,
        // const Vector &basePos, const Vector &baseBoneScale, 
        // const mstudioanim_t *panim, Vector &pos	)
        // {
        // if (panim->flags & STUDIO_ANIM_RAWPOS)
        // {
        // pos = *(panim->pPos());
        // Assert( pos.IsValid() );

        // return;
        // }
        // else if (!(panim->flags & STUDIO_ANIM_ANIMPOS))
        // {
        // if (panim->flags & STUDIO_ANIM_DELTA)
        // {
        // pos.Init( 0.0f, 0.0f, 0.0f );
        // }
        // else
        // {
        // pos = basePos;
        // }
        // return;
        // }

        // mstudioanim_valueptr_t *pPosV = panim->pPosV();
        // int					j;

        // if (s > 0.001f)
        // {
        // float v1, v2;
        // for (j = 0; j < 3; j++)
        // {
        // ExtractAnimValue( frame, pPosV->pAnimvalue( j ), baseBoneScale[j], v1, v2 );
        // //ZM: This is really setting pos.x when j = 0, pos.y when j = 1, and pos.z when j = 2.
        // pos[j] = v1 * (1.0 - s) + v2 * s;
        // }
        // }
        // else
        // {
        // for (j = 0; j < 3; j++)
        // {
        // //ZM: This is really setting pos.x when j = 0, pos.y when j = 1, and pos.z when j = 2.
        // ExtractAnimValue( frame, pPosV->pAnimvalue( j ), baseBoneScale[j], pos[j] );
        // }
        // }

        // if (!(panim->flags & STUDIO_ANIM_DELTA))
        // {
        // pos.x = pos.x + basePos.x;
        // pos.y = pos.y + basePos.y;
        // pos.z = pos.z + basePos.z;
        // }

        // Assert( pos.IsValid() );
        // }
        private SourceVector CalcBonePosition(int frameIndex, double s, SourceMdlBone37 aBone, SourceMdlAnimation37 anAnimation)
        {
            var pos = new SourceVector();
            if ((anAnimation.flags & SourceMdlAnimation37.STUDIO_POS_ANIMATED) > 0)
            {
                if (anAnimation.animationValueOffsets[0] <= 0)
                {
                    // pos.x = 0
                    pos.x = aBone.position.x;
                }
                else
                {
                    pos.x = ExtractAnimValue(frameIndex, anAnimation.theAnimationValues[0], aBone.positionScale.x);
                    pos.x = aBone.position.x + pos.x * aBone.positionScale.x;
                }

                if (anAnimation.animationValueOffsets[1] <= 0)
                {
                    // pos.y = 0
                    pos.y = aBone.position.y;
                }
                else
                {
                    pos.y = ExtractAnimValue(frameIndex, anAnimation.theAnimationValues[1], aBone.positionScale.y);
                    pos.y = aBone.position.y + pos.y * aBone.positionScale.y;
                }

                if (anAnimation.animationValueOffsets[2] <= 0)
                {
                    // pos.z = 0
                    pos.z = aBone.position.z;
                }
                else
                {
                    pos.z = ExtractAnimValue(frameIndex, anAnimation.theAnimationValues[2], aBone.positionScale.z);
                    pos.z = aBone.position.z + pos.z * aBone.positionScale.z;
                }

                pos.debug_text = "anim";
            }
            else
            {
                // pos = anAnimation.position
                pos.x = anAnimation.position.x;
                pos.y = anAnimation.position.y;
                pos.z = anAnimation.position.z;
                pos.debug_text = "pos";
            }

            return pos;
        }

        public double ExtractAnimValue(int frameIndex, List<SourceMdlAnimationValue10> animValues, double scale)
        {
            var v1 = default(double);
            // k is frameCountRemainingToBeChecked
            int k;
            int animValueIndex;
            try
            {
                k = frameIndex;
                animValueIndex = 0;
                while (animValues[animValueIndex].total <= k)
                {
                    k -= animValues[animValueIndex].total;
                    animValueIndex += animValues[animValueIndex].valid + 1;
                    if (animValueIndex >= animValues.Count || animValues[animValueIndex].total == 0)
                    {
                        // NOTE: Bad if it reaches here. This means maybe a newer format of the anim data was used for the model.
                        v1 = 0d;
                        return v1;
                    }
                }

                if (animValues[animValueIndex].valid > k)
                {
                    // NOTE: Needs to be offset from current animValues index to match the C++ code above in comment.
                    v1 = animValues[animValueIndex + k + 1].value;
                }
                else
                {
                    // NOTE: Needs to be offset from current animValues index to match the C++ code above in comment.
                    v1 = animValues[animValueIndex + animValues[animValueIndex].valid].value;
                }
            }
            catch (Exception ex)
            {
                int debug = 4242;
            }

            return v1;
        }

        #endregion

        #region Data

        private StreamWriter theOutputFileStreamWriter;
        // Private theAniFileData As SourceAniFileData44
        private SourceMdlFileData37 theMdlFileData;
        private SourcePhyFileData thePhyFileData;
        // Private theVtxFileData As SourceVtxFileData44
        // Private theVvdFileData As SourceVvdFileData37
        // Private theModelName As String

        private SortedList<int, AnimationFrameLine> theAnimationFrameLines;

        #endregion

    }
}