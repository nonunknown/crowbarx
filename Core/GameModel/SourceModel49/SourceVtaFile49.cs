using System.IO;

namespace Crowbar
{
    public class SourceVtaFile49
    {

        #region Creation and Destruction

        public SourceVtaFile49(StreamWriter outputFileStream, SourceMdlFileData49 mdlFileData, SourceVvdFileData04 vvdFileData, SourceMdlBodyPart bodyPart)
        {
            theOutputFileStreamWriter = outputFileStream;
            theMdlFileData = mdlFileData;
            theVvdFileData = vvdFileData;
            theBodyPart = bodyPart;
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

        public void WriteNodesSection()
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

        public void WriteSkeletonSectionForVertexAnimation()
        {
            string line = "";
            line = "skeleton";
            theOutputFileStreamWriter.WriteLine(line);
            if (Program.TheApp.Settings.DecompileStricterFormatIsChecked)
            {
                line = "time 0 # basis shape key";
            }
            else
            {
                line = "  time 0 # basis shape key";
            }

            theOutputFileStreamWriter.WriteLine(line);
            int timeIndex;
            int flexTimeIndex;
            FlexFrame aFlexFrame;
            timeIndex = 1;
            // NOTE: The first frame was written in code above.
            // For flexTimeIndex = 1 To Me.theMdlFileData.theFlexFrames.Count - 1
            // aFlexFrame = Me.theMdlFileData.theFlexFrames(flexTimeIndex)
            var loopTo = theBodyPart.theFlexFrames.Count - 1;
            for (flexTimeIndex = 1; flexTimeIndex <= loopTo; flexTimeIndex++)
            {
                aFlexFrame = theBodyPart.theFlexFrames[flexTimeIndex];
                if (Program.TheApp.Settings.DecompileStricterFormatIsChecked)
                {
                    line = "time ";
                }
                else
                {
                    line = "  time ";
                }

                line += timeIndex.ToString(Program.TheApp.InternalNumberFormat);
                line += " # ";
                line += aFlexFrame.flexDescription;
                theOutputFileStreamWriter.WriteLine(line);
                timeIndex += 1;
            }

            line = "end";
            theOutputFileStreamWriter.WriteLine(line);
        }

        public void WriteVertexAnimationSection()
        {
            string line = "";
            line = "vertexanimation";
            theOutputFileStreamWriter.WriteLine(line);
            if (Program.TheApp.Settings.DecompileStricterFormatIsChecked)
            {
                line = "time 0 # basis shape key";
            }
            else
            {
                line = "  time 0 # basis shape key";
            }

            theOutputFileStreamWriter.WriteLine(line);
            int beginVertexIndex = 0;
            int endVertexIndex = 0;
            int bodyVertexCount = 0;
            SourceMdlBodyPart aBodyPart;
            SourceMdlModel aModel;
            for (int bodyPartIndex = 0, loopTo = theMdlFileData.theBodyParts.Count - 1; bodyPartIndex <= loopTo; bodyPartIndex++)
            {
                aBodyPart = theMdlFileData.theBodyParts[bodyPartIndex];
                if (ReferenceEquals(theBodyPart, aBodyPart))
                {
                    beginVertexIndex = bodyVertexCount;
                    endVertexIndex = bodyVertexCount;
                }

                if (aBodyPart.theModels is object && aBodyPart.theModels.Count > 0)
                {
                    for (int modelIndex = 0, loopTo1 = aBodyPart.theModels.Count - 1; modelIndex <= loopTo1; modelIndex++)
                    {
                        aModel = aBodyPart.theModels[modelIndex];
                        bodyVertexCount += aModel.vertexCount;
                    }
                }

                if (ReferenceEquals(theBodyPart, aBodyPart))
                {
                    endVertexIndex = bodyVertexCount - 1;
                }
            }

            try
            {
                SourceVertex aVertex;
                // For vertexIndex As Integer = 0 To Me.theVvdFileData.theVertexes.Count - 1
                for (int vertexIndex = beginVertexIndex, loopTo2 = endVertexIndex; vertexIndex <= loopTo2; vertexIndex++)
                {
                    if (theVvdFileData.fixupCount == 0)
                    {
                        aVertex = theVvdFileData.theVertexes[vertexIndex];
                    }
                    else
                    {
                        // NOTE: I don't know why lodIndex is not needed here, but using only lodIndex=0 matches what MDL Decompiler produces.
                        // Maybe the listing by lodIndex is only needed internally by graphics engine.
                        aVertex = theVvdFileData.theFixedVertexesByLod[0][vertexIndex];
                    }

                    line = "    ";
                    line += vertexIndex.ToString(Program.TheApp.InternalNumberFormat);
                    line += " ";
                    line += aVertex.positionX.ToString("0.000000", Program.TheApp.InternalNumberFormat);
                    line += " ";
                    line += aVertex.positionY.ToString("0.000000", Program.TheApp.InternalNumberFormat);
                    line += " ";
                    line += aVertex.positionZ.ToString("0.000000", Program.TheApp.InternalNumberFormat);
                    line += " ";
                    line += aVertex.normalX.ToString("0.000000", Program.TheApp.InternalNumberFormat);
                    line += " ";
                    line += aVertex.normalY.ToString("0.000000", Program.TheApp.InternalNumberFormat);
                    line += " ";
                    line += aVertex.normalZ.ToString("0.000000", Program.TheApp.InternalNumberFormat);
                    theOutputFileStreamWriter.WriteLine(line);
                }
            }
            catch
            {
            }

            int timeIndex;
            int flexTimeIndex;
            FlexFrame aFlexFrame;
            timeIndex = 1;
            // NOTE: The first frame was written in code above.
            // For flexTimeIndex = 1 To Me.theMdlFileData.theFlexFrames.Count - 1
            // aFlexFrame = Me.theMdlFileData.theFlexFrames(flexTimeIndex)
            var loopTo3 = theBodyPart.theFlexFrames.Count - 1;
            for (flexTimeIndex = 1; flexTimeIndex <= loopTo3; flexTimeIndex++)
            {
                aFlexFrame = theBodyPart.theFlexFrames[flexTimeIndex];
                if (Program.TheApp.Settings.DecompileStricterFormatIsChecked)
                {
                    line = "time ";
                }
                else
                {
                    line = "  time ";
                }

                line += timeIndex.ToString(Program.TheApp.InternalNumberFormat);
                line += " # ";
                line += aFlexFrame.flexDescription;
                theOutputFileStreamWriter.WriteLine(line);
                for (int x = 0, loopTo4 = aFlexFrame.flexes.Count - 1; x <= loopTo4; x++)
                    WriteVertexAnimLines(aFlexFrame.flexes[x], aFlexFrame.bodyAndMeshVertexIndexStarts[x]);
                timeIndex += 1;
            }

            line = "end";
            theOutputFileStreamWriter.WriteLine(line);
        }

        #endregion

        #region Private Delegates

        #endregion

        #region Private Methods

        private void WriteVertexAnimLines(SourceMdlFlex aFlex, int bodyAndMeshVertexIndexStart)
        {
            string line;
            SourceVertex aVertex;
            int vertexIndex;
            double positionX;
            double positionY;
            double positionZ;
            double normalX;
            double normalY;
            double normalZ;
            for (int i = 0, loopTo = aFlex.theVertAnims.Count - 1; i <= loopTo; i++)
            {
                SourceMdlVertAnim aVertAnim;
                aVertAnim = aFlex.theVertAnims[i];

                // TODO: Figure out why decompiling teen angst zoey (which has 39 shape keys) gives 55 shapekeys.
                // - Probably extra ones are related to flexpairs (right and left).
                // - Eyelids are combined, e.g. second shapekey from source vta is upper_lid_lowerer
                // that contains both upper_right_lowerer and upper_left_lowerer.

                vertexIndex = aVertAnim.index + bodyAndMeshVertexIndexStart;
                if (theVvdFileData.fixupCount == 0)
                {
                    aVertex = theVvdFileData.theVertexes[vertexIndex];
                }
                else
                {
                    // NOTE: I don't know why lodIndex is not needed here, but using only lodIndex=0 matches what MDL Decompiler produces.
                    // Maybe the listing by lodIndex is only needed internally by graphics engine.
                    aVertex = theVvdFileData.theFixedVertexesByLod[0][vertexIndex];
                }

                positionX = aVertex.positionX + aVertAnim.get_flDelta(0).TheFloatValue;
                positionY = aVertex.positionY + aVertAnim.get_flDelta(1).TheFloatValue;
                positionZ = aVertex.positionZ + aVertAnim.get_flDelta(2).TheFloatValue;
                normalX = aVertex.normalX + aVertAnim.get_flNDelta(0).TheFloatValue;
                normalY = aVertex.normalY + aVertAnim.get_flNDelta(1).TheFloatValue;
                normalZ = aVertex.normalZ + aVertAnim.get_flNDelta(2).TheFloatValue;
                line = "    ";
                line += vertexIndex.ToString(Program.TheApp.InternalNumberFormat);
                line += " ";
                line += positionX.ToString("0.000000", Program.TheApp.InternalNumberFormat);
                line += " ";
                line += positionY.ToString("0.000000", Program.TheApp.InternalNumberFormat);
                line += " ";
                line += positionZ.ToString("0.000000", Program.TheApp.InternalNumberFormat);
                line += " ";
                line += normalX.ToString("0.000000", Program.TheApp.InternalNumberFormat);
                line += " ";
                line += normalY.ToString("0.000000", Program.TheApp.InternalNumberFormat);
                line += " ";
                line += normalZ.ToString("0.000000", Program.TheApp.InternalNumberFormat);

                // TEST:
                // If aFlex.vertAnimType = aFlex.STUDIO_VERT_ANIM_WRINKLE Then
                // CType(aVertAnim, SourceMdlVertAnimWrinkle).wrinkleDelta = Me.theInputFileReader.ReadInt16()
                // End If
                // If blah Then
                // line += " // wrinkle value: "
                // line += aVertAnim.flDelta(0).the16BitValue.ToString()
                // End If

                theOutputFileStreamWriter.WriteLine(line);
            }
        }

        #endregion

        #region Data

        private StreamWriter theOutputFileStreamWriter;
        private SourceMdlFileData49 theMdlFileData;
        private SourceVvdFileData04 theVvdFileData;
        private SourceMdlBodyPart theBodyPart;

        #endregion

    }
}