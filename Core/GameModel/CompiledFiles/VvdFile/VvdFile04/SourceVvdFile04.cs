using System;
using System.Collections.Generic;
using System.IO;

namespace Crowbar
{
    public class SourceVvdFile04
    {

        #region Creation and Destruction

        public SourceVvdFile04(BinaryReader vvdFileReader, SourceVvdFileData04 vvdFileData, long vvdFileOffsetStart = 0L)
        {
            theInputFileReader = vvdFileReader;
            theVvdFileOffsetStart = vvdFileOffsetStart;
            theVvdFileData = vvdFileData;
            theVvdFileData.theFileSeekLog.FileSize = theInputFileReader.BaseStream.Length;
        }

        #endregion

        #region Methods

        public void ReadSourceVvdHeader()
        {
            long fileOffsetStart;
            long fileOffsetEnd;
            fileOffsetStart = theInputFileReader.BaseStream.Position;
            theVvdFileData.id = theInputFileReader.ReadChars(4);
            theVvdFileData.version = theInputFileReader.ReadInt32();
            theVvdFileData.checksum = theInputFileReader.ReadInt32();
            theVvdFileData.lodCount = theInputFileReader.ReadInt32();
            for (int i = 0, loopTo = SourceConstants.MAX_NUM_LODS - 1; i <= loopTo; i++)
                theVvdFileData.lodVertexCount[i] = theInputFileReader.ReadInt32();
            theVvdFileData.fixupCount = theInputFileReader.ReadInt32();
            theVvdFileData.fixupTableOffset = theInputFileReader.ReadInt32();
            theVvdFileData.vertexDataOffset = theInputFileReader.ReadInt32();
            theVvdFileData.tangentDataOffset = theInputFileReader.ReadInt32();
            fileOffsetEnd = theInputFileReader.BaseStream.Position - 1L;
            theVvdFileData.theFileSeekLog.Add(fileOffsetStart, fileOffsetEnd, "VVD File Header");
        }

        public void ReadVertexes(int mdlVersion = 0)
        {
            if (theVvdFileData.lodCount <= 0)
            {
                return;
            }

            long fileOffsetStart;
            long fileOffsetEnd;
            theInputFileReader.BaseStream.Seek(theVvdFileOffsetStart + theVvdFileData.vertexDataOffset, SeekOrigin.Begin);
            fileOffsetStart = theInputFileReader.BaseStream.Position;

            // Dim boneWeightingIsIncorrect As Boolean
            float weight;
            byte boneIndex;
            int vertexCount;
            vertexCount = theVvdFileData.lodVertexCount[0];
            theVvdFileData.theVertexes = new List<SourceVertex>(vertexCount);
            for (int j = 0, loopTo = vertexCount - 1; j <= loopTo; j++)
            {
                var aStudioVertex = new SourceVertex();
                var boneWeight = new SourceBoneWeight();
                // boneWeightingIsIncorrect = False
                for (int x = 0, loopTo1 = SourceConstants.MAX_NUM_BONES_PER_VERT - 1; x <= loopTo1; x++)
                {
                    weight = theInputFileReader.ReadSingle();
                    boneWeight.weight[x] = weight;
                    // If weight > 1 Then
                    // boneWeightingIsIncorrect = True
                    // End If
                }

                for (int x = 0, loopTo2 = SourceConstants.MAX_NUM_BONES_PER_VERT - 1; x <= loopTo2; x++)
                {
                    boneIndex = theInputFileReader.ReadByte();
                    boneWeight.bone[x] = boneIndex;
                    // If boneIndex > 127 Then
                    // boneWeightingIsIncorrect = True
                    // End If
                }

                boneWeight.boneCount = theInputFileReader.ReadByte();
                // 'TODO: ReadVertexes() -- boneWeight.boneCount > MAX_NUM_BONES_PER_VERT, which seems like incorrect vvd format 
                // If boneWeight.boneCount > MAX_NUM_BONES_PER_VERT Then
                // boneWeight.boneCount = CByte(MAX_NUM_BONES_PER_VERT)
                // End If
                // If boneWeightingIsIncorrect Then
                // boneWeight.boneCount = 0
                // End If
                aStudioVertex.boneWeight = boneWeight;
                aStudioVertex.positionX = theInputFileReader.ReadSingle();
                aStudioVertex.positionY = theInputFileReader.ReadSingle();
                aStudioVertex.positionZ = theInputFileReader.ReadSingle();
                aStudioVertex.normalX = theInputFileReader.ReadSingle();
                aStudioVertex.normalY = theInputFileReader.ReadSingle();
                aStudioVertex.normalZ = theInputFileReader.ReadSingle();
                aStudioVertex.texCoordX = theInputFileReader.ReadSingle();
                aStudioVertex.texCoordY = theInputFileReader.ReadSingle();
                if (mdlVersion >= 54 && mdlVersion <= 59)
                {
                    theInputFileReader.ReadSingle();
                    theInputFileReader.ReadSingle();
                    theInputFileReader.ReadSingle();
                    theInputFileReader.ReadSingle();
                }

                theVvdFileData.theVertexes.Add(aStudioVertex);
            }

            fileOffsetEnd = theInputFileReader.BaseStream.Position - 1L;
            theVvdFileData.theFileSeekLog.Add(fileOffsetStart, fileOffsetEnd, "theVertexes " + theVvdFileData.theVertexes.Count.ToString());
        }

        public void ReadFixups()
        {
            if (theVvdFileData.fixupCount > 0)
            {
                long fileOffsetStart;
                long fileOffsetEnd;
                theInputFileReader.BaseStream.Seek(theVvdFileOffsetStart + theVvdFileData.fixupTableOffset, SeekOrigin.Begin);
                fileOffsetStart = theInputFileReader.BaseStream.Position;
                theVvdFileData.theFixups = new List<SourceVvdFixup04>(theVvdFileData.fixupCount);
                for (int fixupIndex = 0, loopTo = theVvdFileData.fixupCount - 1; fixupIndex <= loopTo; fixupIndex++)
                {
                    var aFixup = new SourceVvdFixup04();
                    aFixup.lodIndex = theInputFileReader.ReadInt32();
                    aFixup.vertexIndex = theInputFileReader.ReadInt32();
                    aFixup.vertexCount = theInputFileReader.ReadInt32();
                    theVvdFileData.theFixups.Add(aFixup);
                }

                fileOffsetEnd = theInputFileReader.BaseStream.Position - 1L;
                theVvdFileData.theFileSeekLog.Add(fileOffsetStart, fileOffsetEnd, "theFixups " + theVvdFileData.theFixups.Count.ToString());
                if (theVvdFileData.lodCount > 0)
                {
                    theInputFileReader.BaseStream.Seek(theVvdFileOffsetStart + theVvdFileData.vertexDataOffset, SeekOrigin.Begin);
                    for (int lodIndex = 0, loopTo1 = theVvdFileData.lodCount - 1; lodIndex <= loopTo1; lodIndex++)
                        SetupFixedVertexes(lodIndex);
                }
            }
        }

        public void ReadUnreadBytes()
        {
            theVvdFileData.theFileSeekLog.LogUnreadBytes(theInputFileReader);
        }

        #endregion

        #region Private Methods

        private void SetupFixedVertexes(int lodIndex)
        {
            SourceVvdFixup04 aFixup;
            SourceVertex aStudioVertex;
            try
            {
                theVvdFileData.theFixedVertexesByLod[lodIndex] = new List<SourceVertex>();
                for (int fixupIndex = 0, loopTo = theVvdFileData.theFixups.Count - 1; fixupIndex <= loopTo; fixupIndex++)
                {
                    aFixup = theVvdFileData.theFixups[fixupIndex];
                    if (aFixup.lodIndex >= lodIndex)
                    {
                        for (int j = 0, loopTo1 = aFixup.vertexCount - 1; j <= loopTo1; j++)
                        {
                            aStudioVertex = theVvdFileData.theVertexes[aFixup.vertexIndex + j];
                            theVvdFileData.theFixedVertexesByLod[lodIndex].Add(aStudioVertex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                int debug = 4242;
            }
        }

        #endregion

        #region Data

        private BinaryReader theInputFileReader;
        private long theVvdFileOffsetStart;
        private SourceVvdFileData04 theVvdFileData;

        #endregion

    }
}