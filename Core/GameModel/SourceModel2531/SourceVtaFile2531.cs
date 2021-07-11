﻿using System;
using System.IO;

namespace Crowbar
{
    public class SourceVtaFile2531
    {

        #region Creation and Destruction

        public SourceVtaFile2531(StreamWriter outputFileStream, SourceMdlFileData2531 mdlFileData)
        {
            theOutputFileStreamWriter = outputFileStream;
            theMdlFileData = mdlFileData;
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

            // skeleton
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
            FlexFrame2531 aFlexFrame;
            timeIndex = 1;
            // NOTE: The first frame was written in code above.
            var loopTo = theMdlFileData.theFlexFrames.Count - 1;
            for (flexTimeIndex = 1; flexTimeIndex <= loopTo; flexTimeIndex++)
            {
                aFlexFrame = theMdlFileData.theFlexFrames[flexTimeIndex];
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
            try
            {
                SourceMdlModel2531 aBodyModel;
                int vertexCount;
                aBodyModel = theMdlFileData.theBodyParts[0].theModels[0];
                if (aBodyModel.vertexListType == 0)
                {
                    vertexCount = aBodyModel.theVertexesType0.Count;
                }
                else if (aBodyModel.vertexListType == 1)
                {
                    vertexCount = aBodyModel.theVertexesType1.Count;
                }
                else if (aBodyModel.vertexListType == 2)
                {
                    vertexCount = aBodyModel.theVertexesType2.Count;
                }
                else
                {
                    vertexCount = 0;
                }

                var position = new SourceVector();
                var normal = new SourceVector();
                for (int vertexIndex = 0, loopTo = vertexCount - 1; vertexIndex <= loopTo; vertexIndex++)
                {
                    if (aBodyModel.vertexListType == 0)
                    {
                        position.x = aBodyModel.theVertexesType0[vertexIndex].position.x;
                        position.y = aBodyModel.theVertexesType0[vertexIndex].position.y;
                        position.z = aBodyModel.theVertexesType0[vertexIndex].position.z;
                        normal.x = aBodyModel.theVertexesType0[vertexIndex].normal.x;
                        normal.y = aBodyModel.theVertexesType0[vertexIndex].normal.y;
                        normal.z = aBodyModel.theVertexesType0[vertexIndex].normal.z;
                    }
                    else if (aBodyModel.vertexListType == 1)
                    {
                        position.x = aBodyModel.theVertexesType1[vertexIndex].positionX / 65535d * theMdlFileData.hullMinPosition.y;
                        position.y = aBodyModel.theVertexesType1[vertexIndex].positionY / 65535d * theMdlFileData.hullMinPosition.z;
                        position.z = aBodyModel.theVertexesType1[vertexIndex].positionZ / 65535d * theMdlFileData.hullMinPosition.x;
                        normal.x = aBodyModel.theVertexesType1[vertexIndex].normalX / 65535d * theMdlFileData.hullMaxPosition.x;
                        normal.y = aBodyModel.theVertexesType1[vertexIndex].normalY / 65535d * theMdlFileData.hullMaxPosition.y;
                        normal.z = aBodyModel.theVertexesType1[vertexIndex].normalZ / 65535d * theMdlFileData.hullMaxPosition.z;
                    }
                    else if (aBodyModel.vertexListType == 2)
                    {
                        position.x = aBodyModel.theVertexesType2[vertexIndex].positionX / 255d * theMdlFileData.hullMinPosition.y;
                        position.y = aBodyModel.theVertexesType2[vertexIndex].positionY / 255d * theMdlFileData.hullMinPosition.z;
                        position.z = aBodyModel.theVertexesType2[vertexIndex].positionZ / 255d * theMdlFileData.hullMinPosition.x;
                        normal.x = aBodyModel.theVertexesType2[vertexIndex].normalX / 255d * theMdlFileData.hullMaxPosition.x;
                        normal.y = aBodyModel.theVertexesType2[vertexIndex].normalY / 255d * theMdlFileData.hullMaxPosition.y;
                        normal.z = aBodyModel.theVertexesType2[vertexIndex].normalZ / 255d * theMdlFileData.hullMaxPosition.z;
                    }
                    else
                    {
                        int debug = 4242;
                    }

                    line = "    ";
                    line += vertexIndex.ToString(Program.TheApp.InternalNumberFormat);
                    line += " ";
                    line += position.x.ToString("0.000000", Program.TheApp.InternalNumberFormat);
                    line += " ";
                    line += position.y.ToString("0.000000", Program.TheApp.InternalNumberFormat);
                    line += " ";
                    line += position.z.ToString("0.000000", Program.TheApp.InternalNumberFormat);
                    line += " ";
                    line += normal.x.ToString("0.000000", Program.TheApp.InternalNumberFormat);
                    line += " ";
                    line += normal.y.ToString("0.000000", Program.TheApp.InternalNumberFormat);
                    line += " ";
                    line += normal.z.ToString("0.000000", Program.TheApp.InternalNumberFormat);
                    theOutputFileStreamWriter.WriteLine(line);
                }
            }
            catch (Exception ex)
            {
                int debug = 4242;
            }

            int timeIndex;
            int flexTimeIndex;
            FlexFrame2531 aFlexFrame;
            try
            {
                timeIndex = 1;
                // NOTE: The first frame was written in code above.
                var loopTo1 = theMdlFileData.theFlexFrames.Count - 1;
                for (flexTimeIndex = 1; flexTimeIndex <= loopTo1; flexTimeIndex++)
                {
                    aFlexFrame = theMdlFileData.theFlexFrames[flexTimeIndex];
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
                    for (int x = 0, loopTo2 = aFlexFrame.flexes.Count - 1; x <= loopTo2; x++)
                        WriteVertexAnimLines(aFlexFrame.flexes[x], aFlexFrame.bodyAndMeshVertexIndexStarts[x]);
                    timeIndex += 1;
                }
            }
            catch (Exception ex)
            {
                int debug = 4242;
            }

            line = "end";
            theOutputFileStreamWriter.WriteLine(line);
        }

        #endregion

        #region Private Delegates

        #endregion

        #region Private Methods

        private void WriteVertexAnimLines(SourceMdlFlex2531 aFlex, int bodyAndMeshVertexIndexStart)
        {
            string line;
            int vertexIndex;
            var position = new SourceVector();
            var normal = new SourceVector();
            SourceMdlModel2531 aBodyModel;
            try
            {
                aBodyModel = theMdlFileData.theBodyParts[0].theModels[0];
                for (int i = 0, loopTo = aFlex.theVertAnims.Count - 1; i <= loopTo; i++)
                {
                    SourceMdlVertAnim2531 aVertAnim;
                    aVertAnim = aFlex.theVertAnims[i];
                    vertexIndex = aVertAnim.index + bodyAndMeshVertexIndexStart;
                    if (aBodyModel.vertexListType == 0)
                    {
                        position.x = aBodyModel.theVertexesType0[vertexIndex].position.x;
                        position.y = aBodyModel.theVertexesType0[vertexIndex].position.y;
                        position.z = aBodyModel.theVertexesType0[vertexIndex].position.z;
                        normal.x = aBodyModel.theVertexesType0[vertexIndex].normal.x;
                        normal.y = aBodyModel.theVertexesType0[vertexIndex].normal.y;
                        normal.z = aBodyModel.theVertexesType0[vertexIndex].normal.z;
                    }
                    else if (aBodyModel.vertexListType == 1)
                    {
                        position.x = aBodyModel.theVertexesType1[vertexIndex].positionX / 65535d * theMdlFileData.hullMinPosition.y;
                        position.y = aBodyModel.theVertexesType1[vertexIndex].positionY / 65535d * theMdlFileData.hullMinPosition.z;
                        position.z = aBodyModel.theVertexesType1[vertexIndex].positionZ / 65535d * theMdlFileData.hullMinPosition.x;
                        normal.x = aBodyModel.theVertexesType1[vertexIndex].normalX / 65535d * theMdlFileData.hullMaxPosition.x;
                        normal.y = aBodyModel.theVertexesType1[vertexIndex].normalY / 65535d * theMdlFileData.hullMaxPosition.y;
                        normal.z = aBodyModel.theVertexesType1[vertexIndex].normalZ / 65535d * theMdlFileData.hullMaxPosition.z;
                    }
                    else if (aBodyModel.vertexListType == 2)
                    {
                        position.x = aBodyModel.theVertexesType2[vertexIndex].positionX / 255d * theMdlFileData.hullMinPosition.y;
                        position.y = aBodyModel.theVertexesType2[vertexIndex].positionY / 255d * theMdlFileData.hullMinPosition.z;
                        position.z = aBodyModel.theVertexesType2[vertexIndex].positionZ / 255d * theMdlFileData.hullMinPosition.x;
                        normal.x = aBodyModel.theVertexesType2[vertexIndex].normalX / 255d * theMdlFileData.hullMaxPosition.x;
                        normal.y = aBodyModel.theVertexesType2[vertexIndex].normalY / 255d * theMdlFileData.hullMaxPosition.y;
                        normal.z = aBodyModel.theVertexesType2[vertexIndex].normalZ / 255d * theMdlFileData.hullMaxPosition.z;
                    }
                    else
                    {
                        int debug = 4242;
                    }

                    // TEST: Values are too big.
                    // position.x += aVertAnim.deltaX
                    // position.y += aVertAnim.deltaY
                    // position.z += aVertAnim.deltaZ
                    // normal.x += aVertAnim.nDeltaX
                    // normal.y += aVertAnim.nDeltaY
                    // normal.z += aVertAnim.nDeltaZ
                    // ------
                    // TEST: Values are too big, but seem to be somewhat close to what they should be.
                    // position.x += (aVertAnim.deltaX / 255)
                    // position.y += (aVertAnim.deltaY / 255)
                    // position.z += (aVertAnim.deltaZ / 255)
                    // normal.x += (aVertAnim.nDeltaX / 255)
                    // normal.y += (aVertAnim.nDeltaY / 255)
                    // normal.z += (aVertAnim.nDeltaZ / 255)
                    // position.x += (aVertAnim.deltaY / 255)
                    // position.y += (aVertAnim.deltaX / 255)
                    // position.z += (aVertAnim.deltaZ / 255)
                    // normal.x += (aVertAnim.nDeltaY / 255)
                    // normal.y += (aVertAnim.nDeltaX / 255)
                    // normal.z += (aVertAnim.nDeltaZ / 255)
                    // TEST: Seems close, but pushes all flexes slightly to the right (when viewing from front).
                    // position.x += (aVertAnim.deltaX / 2550)
                    // position.y += (aVertAnim.deltaY / 2550)
                    // position.z += (aVertAnim.deltaZ / 2550)
                    // normal.x += (aVertAnim.nDeltaX / 2550)
                    // normal.y += (aVertAnim.nDeltaY / 2550)
                    // normal.z += (aVertAnim.nDeltaZ / 2550)
                    // position.x += (aVertAnim.deltaX / 2550)
                    // position.y += (aVertAnim.deltaZ / 2550)
                    // position.z += (aVertAnim.deltaY / 2550)
                    // normal.x += (aVertAnim.nDeltaX / 2550)
                    // normal.y += (aVertAnim.nDeltaZ / 2550)
                    // normal.z += (aVertAnim.nDeltaY / 2550)
                    // position.x += (aVertAnim.deltaY / 2550)
                    // position.y += (aVertAnim.deltaX / 2550)
                    // position.z += (aVertAnim.deltaZ / 2550)
                    // normal.x += (aVertAnim.nDeltaY / 2550)
                    // normal.y += (aVertAnim.nDeltaX / 2550)
                    // normal.z += (aVertAnim.nDeltaZ / 2550)
                    // position.x += (aVertAnim.deltaY / 2550)
                    // position.y += (aVertAnim.deltaZ / 2550)
                    // position.z += (aVertAnim.deltaX / 2550)
                    // normal.x += (aVertAnim.nDeltaY / 2550)
                    // normal.y += (aVertAnim.nDeltaZ / 2550)
                    // normal.z += (aVertAnim.nDeltaX / 2550)
                    // position.x += (aVertAnim.deltaZ / 2550)
                    // position.y += (aVertAnim.deltaX / 2550)
                    // position.z += (aVertAnim.deltaY / 2550)
                    // normal.x += (aVertAnim.nDeltaZ / 2550)
                    // normal.y += (aVertAnim.nDeltaX / 2550)
                    // normal.z += (aVertAnim.nDeltaY / 2550)
                    // position.x += (aVertAnim.deltaZ / 2550)
                    // position.y += (aVertAnim.deltaY / 2550)
                    // position.z += (aVertAnim.deltaX / 2550)
                    // normal.x += (aVertAnim.nDeltaZ / 2550)
                    // normal.y += (aVertAnim.nDeltaY / 2550)
                    // normal.z += (aVertAnim.nDeltaX / 2550)
                    // TEST: [- 2550]
                    // position.x -= (aVertAnim.deltaX / 2550)
                    // position.y -= (aVertAnim.deltaY / 2550)
                    // position.z -= (aVertAnim.deltaZ / 2550)
                    // normal.x -= (aVertAnim.nDeltaX / 2550)
                    // normal.y -= (aVertAnim.nDeltaY / 2550)
                    // normal.z -= (aVertAnim.nDeltaZ / 2550)
                    // TEST: [-zyx 2550]
                    // position.x -= (aVertAnim.deltaZ / 2550)
                    // position.y -= (aVertAnim.deltaY / 2550)
                    // position.z -= (aVertAnim.deltaX / 2550)
                    // normal.x -= (aVertAnim.nDeltaZ / 2550)
                    // normal.y -= (aVertAnim.nDeltaY / 2550)
                    // normal.z -= (aVertAnim.nDeltaX / 2550)
                    // TEST: 
                    // position.x -= (aVertAnim.deltaX / 2550)
                    // position.y -= (aVertAnim.deltaY / 2550)
                    // position.z -= (aVertAnim.deltaZ / 2550)
                    // normal.x -= (aVertAnim.nDeltaX / 2550)
                    // normal.y -= (aVertAnim.nDeltaY / 2550)
                    // normal.z -= (aVertAnim.nDeltaZ / 2550)
                    // 'TEST:
                    // position.x -= (aVertAnim.deltaZ / 2550)
                    // position.y -= (aVertAnim.deltaX / 2550)
                    // position.z -= (aVertAnim.deltaY / 2550)
                    // normal.x -= (aVertAnim.nDeltaZ / 2550)
                    // normal.y -= (aVertAnim.nDeltaX / 2550)
                    // normal.z -= (aVertAnim.nDeltaY / 2550)
                    // TEST:
                    // position.x -= (aVertAnim.deltaY / 2550)
                    // position.y -= (aVertAnim.deltaX / 2550)
                    // position.z -= (aVertAnim.deltaZ / 2550)
                    // normal.x -= (aVertAnim.nDeltaY / 2550)
                    // normal.y -= (aVertAnim.nDeltaX / 2550)
                    // normal.z -= (aVertAnim.nDeltaZ / 2550)
                    // 'TEST:
                    // position.x -= (aVertAnim.deltaY / 327670)
                    // position.y -= (aVertAnim.deltaX / 327670)
                    // position.z -= (aVertAnim.deltaZ / 327670)
                    // TEST: seems closest to being correct
                    position.x -= aVertAnim.deltaX / 327670d;
                    position.y -= aVertAnim.deltaY / 327670d;
                    position.z -= aVertAnim.deltaZ / 327670d;
                    // ------
                    // position.x += (aVertAnim.deltaX / 128)
                    // position.y += (aVertAnim.deltaY / 128)
                    // position.z += (aVertAnim.deltaZ / 128)
                    // normal.x += (aVertAnim.nDeltaX / 128)
                    // normal.y += (aVertAnim.nDeltaY / 128)
                    // normal.z += (aVertAnim.nDeltaZ / 128)
                    // position.x += (aVertAnim.deltaX / 128)
                    // position.y += (aVertAnim.deltaY / 128)
                    // position.z += (aVertAnim.deltaZ / 128)
                    // normal.x += (aVertAnim.nDeltaX / 128)
                    // normal.y += (aVertAnim.nDeltaY / 128)
                    // normal.z += (aVertAnim.nDeltaZ / 128)
                    // position.x += (aVertAnim.deltaX / 1280)
                    // position.y += (aVertAnim.deltaY / 1280)
                    // position.z += (aVertAnim.deltaZ / 1280)
                    // normal.x += (aVertAnim.nDeltaX / 1280)
                    // normal.y += (aVertAnim.nDeltaY / 1280)
                    // normal.z += (aVertAnim.nDeltaZ / 1280)
                    // position.x += (aVertAnim.deltaY / 1280)
                    // position.y += (aVertAnim.deltaX / 1280)
                    // position.z += (aVertAnim.deltaZ / 1280)
                    // normal.x += (aVertAnim.nDeltaY / 1280)
                    // normal.y += (aVertAnim.nDeltaX / 1280)
                    // normal.z += (aVertAnim.nDeltaZ / 1280)
                    // ------
                    // TEST: Values are too big, but maybe the SourceFloat8Bits calculations are wrong.
                    // position.x += aVertAnim.flDelta(0).TheFloatValue
                    // position.y += aVertAnim.flDelta(1).TheFloatValue
                    // position.z += aVertAnim.flDelta(2).TheFloatValue
                    // normal.x += aVertAnim.flNDelta(0).TheFloatValue
                    // normal.y += aVertAnim.flNDelta(1).TheFloatValue
                    // normal.z += aVertAnim.flNDelta(2).TheFloatValue
                    // ------
                    // position.x += (aVertAnim.deltaX / 32767)
                    // position.y += (aVertAnim.deltaY / 32767)
                    // position.z += (aVertAnim.deltaZ / 32767)
                    // normal.x = 1
                    // normal.y = 1
                    // normal.z = 1
                    // ------
                    // position.x += aVertAnim.flDelta(0).TheFloatValue
                    // position.y += aVertAnim.flDelta(1).TheFloatValue
                    // position.z += aVertAnim.flDelta(2).TheFloatValue
                    // normal.x = 1
                    // normal.y = 1
                    // normal.z = 1

                    line = "    ";
                    line += vertexIndex.ToString(Program.TheApp.InternalNumberFormat);
                    line += " ";
                    line += position.x.ToString("0.000000", Program.TheApp.InternalNumberFormat);
                    line += " ";
                    line += position.y.ToString("0.000000", Program.TheApp.InternalNumberFormat);
                    line += " ";
                    line += position.z.ToString("0.000000", Program.TheApp.InternalNumberFormat);
                    line += " ";
                    line += normal.x.ToString("0.000000", Program.TheApp.InternalNumberFormat);
                    line += " ";
                    line += normal.y.ToString("0.000000", Program.TheApp.InternalNumberFormat);
                    line += " ";
                    line += normal.z.ToString("0.000000", Program.TheApp.InternalNumberFormat);
                    theOutputFileStreamWriter.WriteLine(line);
                }
            }
            catch (Exception ex)
            {
                int debug = 4242;
            }
        }

        #endregion

        #region Data

        private StreamWriter theOutputFileStreamWriter;
        private SourceMdlFileData2531 theMdlFileData;

        #endregion

    }
}