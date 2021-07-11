using System;
using System.IO;

namespace Crowbar
{
    public class SourceVtaFile37
    {

        #region Creation and Destruction

        public SourceVtaFile37(StreamWriter outputFileStream, SourceMdlFileData37 mdlFileData)
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
            FlexFrame37 aFlexFrame;
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
            SourceMdlVertex37 aVertex;

            // vertexanimation
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
                for (int vertexIndex = 0, loopTo = theMdlFileData.theVertexes.Count - 1; vertexIndex <= loopTo; vertexIndex++)
                {
                    aVertex = theMdlFileData.theVertexes[vertexIndex];
                    line = "    ";
                    line += vertexIndex.ToString(Program.TheApp.InternalNumberFormat);
                    line += " ";
                    line += aVertex.position.x.ToString("0.000000", Program.TheApp.InternalNumberFormat);
                    line += " ";
                    line += aVertex.position.y.ToString("0.000000", Program.TheApp.InternalNumberFormat);
                    line += " ";
                    line += aVertex.position.z.ToString("0.000000", Program.TheApp.InternalNumberFormat);
                    line += " ";
                    line += aVertex.normal.x.ToString("0.000000", Program.TheApp.InternalNumberFormat);
                    line += " ";
                    line += aVertex.normal.y.ToString("0.000000", Program.TheApp.InternalNumberFormat);
                    line += " ";
                    line += aVertex.normal.z.ToString("0.000000", Program.TheApp.InternalNumberFormat);
                    theOutputFileStreamWriter.WriteLine(line);
                }
            }
            catch (Exception ex)
            {
                int debug = 4242;
            }

            int timeIndex;
            int flexTimeIndex;
            FlexFrame37 aFlexFrame;
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

            line = "end";
            theOutputFileStreamWriter.WriteLine(line);
        }

        #endregion

        #region Private Delegates

        #endregion

        #region Private Methods

        private void WriteVertexAnimLines(SourceMdlFlex37 aFlex, int bodyAndMeshVertexIndexStart)
        {
            string line;
            SourceMdlVertex37 aVertex;
            int vertexIndex;
            double positionX;
            double positionY;
            double positionZ;
            double normalX;
            double normalY;
            double normalZ;
            for (int i = 0, loopTo = aFlex.theVertAnims.Count - 1; i <= loopTo; i++)
            {
                SourceMdlVertAnim37 aVertAnim;
                aVertAnim = aFlex.theVertAnims[i];
                vertexIndex = aVertAnim.index + bodyAndMeshVertexIndexStart;
                aVertex = theMdlFileData.theVertexes[vertexIndex];
                positionX = aVertex.position.x + aVertAnim.delta.x;
                positionY = aVertex.position.y + aVertAnim.delta.y;
                positionZ = aVertex.position.z + aVertAnim.delta.z;
                normalX = aVertex.normal.x + aVertAnim.nDelta.x;
                normalY = aVertex.normal.y + aVertAnim.nDelta.y;
                normalZ = aVertex.normal.z + aVertAnim.nDelta.z;
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

                // For debugging.
                // line += " // "
                // line += aVertAnim.flDelta(0).the16BitValue.ToString()
                // line += " "
                // line += aVertAnim.flDelta(1).the16BitValue.ToString()
                // line += " "
                // line += aVertAnim.flDelta(2).the16BitValue.ToString()
                // line += " "
                // line += aVertAnim.flNDelta(0).the16BitValue.ToString()
                // line += " "
                // line += aVertAnim.flNDelta(1).the16BitValue.ToString()
                // line += " "
                // line += aVertAnim.flNDelta(2).the16BitValue.ToString()

                theOutputFileStreamWriter.WriteLine(line);
            }
        }

        #endregion

        #region Data

        private StreamWriter theOutputFileStreamWriter;
        private SourceMdlFileData37 theMdlFileData;

        #endregion

    }
}