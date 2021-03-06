using System;
using System.Collections.Generic;
using System.IO;

namespace Crowbar
{
    public class FileSeekLog
    {
        public FileSeekLog()
        {
            theFileSeekList = new SortedList<long, long>();
            theFileSeekDescriptionList = new SortedList<long, string>();
        }

        public bool ContainsKey(long startOffset)
        {
            return theFileSeekList.ContainsKey(startOffset);
        }

        public void Add(long startOffset, long endOffset, string description)
        {
            try
            {
                if (theFileSeekList.ContainsKey(startOffset) && theFileSeekList[startOffset] == endOffset)
                {
                    theFileSeekDescriptionList[startOffset] += "; " + description;
                }
                else if (theFileSeekList.ContainsKey(startOffset))
                {
                    string temp;
                    temp = theFileSeekDescriptionList[startOffset];
                    theFileSeekDescriptionList[startOffset] = "[ERROR] ";
                    theFileSeekDescriptionList[startOffset] += temp + "; [" + startOffset.ToString() + " - " + endOffset.ToString() + "] " + description;
                }
                else
                {
                    theFileSeekList.Add(startOffset, endOffset);
                    theFileSeekDescriptionList.Add(startOffset, description);
                }
            }
            catch (Exception ex)
            {
                int debug = 4242;
            }
        }

        public void Remove(long startOffset)
        {
            try
            {
                if (theFileSeekList.ContainsKey(startOffset))
                {
                    theFileSeekList.Remove(startOffset);
                }

                if (theFileSeekDescriptionList.ContainsKey(startOffset))
                {
                    theFileSeekDescriptionList.Remove(startOffset);
                }
            }
            catch (Exception ex)
            {
                int debug = 4242;
            }
        }

        public void Clear()
        {
            theFileSeekList.Clear();
            theFileSeekDescriptionList.Clear();
        }

        public long FileSize
        {
            get
            {
                return theFileSize;
            }

            set
            {
                if (theFileSize != value)
                {
                    theFileSize = value;
                    Add(theFileSize, theFileSize, "END OF FILE + 1 (File size)");
                }
            }
        }

        public void LogToEndAndAlignToNextStart(BinaryReader inputFileReader, long fileOffsetEnd, int byteAlignmentCount, string description, long expectedAlignOffsetEnd = -1)
        {
            long fileOffsetStart2;
            long fileOffsetEnd2;
            fileOffsetStart2 = fileOffsetEnd + 1L;
            fileOffsetEnd2 = MathModule.AlignLong(fileOffsetStart2, byteAlignmentCount) - 1L;
            inputFileReader.BaseStream.Seek(fileOffsetEnd2 + 1L, SeekOrigin.Begin);
            if (fileOffsetEnd2 >= fileOffsetStart2)
            {
                var allZeroesWereFound = default(bool);
                if (expectedAlignOffsetEnd > -1 && expectedAlignOffsetEnd != fileOffsetEnd2)
                {
                    description = "[ERROR: Should end at " + expectedAlignOffsetEnd.ToString() + "] " + description;
                    description += " - " + fileOffsetStart2.ToString() + ":" + GetByteValues(inputFileReader, fileOffsetStart2, fileOffsetEnd2, ref allZeroesWereFound);
                    description += " - " + (fileOffsetEnd2 + 1L).ToString() + ":" + GetByteValues(inputFileReader, fileOffsetEnd2 + 1L, expectedAlignOffsetEnd, ref allZeroesWereFound);
                }
                else
                {
                    description += GetByteValues(inputFileReader, fileOffsetStart2, fileOffsetEnd2, ref allZeroesWereFound);
                }

                Add(fileOffsetStart2, fileOffsetEnd2, description);
            }
        }

        public void LogAndAlignFromFileSeekLogEnd(BinaryReader inputFileReader, int byteAlignmentCount, string description)
        {
            // Dim fileOffsetStart2 As Long
            // Dim fileOffsetEnd2 As Long

            // fileOffsetStart2 = Me.theFileSeekList.Values(Me.theFileSeekList.Count - 1) + 1
            // fileOffsetEnd2 = MathModule.AlignLong(fileOffsetStart2, byteAlignmentCount) - 1
            // inputFileReader.BaseStream.Seek(fileOffsetEnd2 + 1, SeekOrigin.Begin)
            // If fileOffsetEnd2 >= fileOffsetStart2 Then
            // Me.Add(fileOffsetStart2, fileOffsetEnd2, description)
            // End If
            // Me.LogToEndAndAlignToNextStart(inputFileReader, Me.theFileSeekList.Values(Me.theFileSeekList.Count - 1), byteAlignmentCount, description)
            // NOTE: The "- 2" skips the final value that should be the "END OF FILE + 1 (File size)".
            LogToEndAndAlignToNextStart(inputFileReader, theFileSeekList.Values[theFileSeekList.Count - 2], byteAlignmentCount, description);
        }

        public void LogUnreadBytes(BinaryReader inputFileReader)
        {
            long offsetStart;
            long offsetEnd;
            string description;
            string byteValues;
            var allZeroesWereFound = default(bool);
            var tempFileSeekList = new SortedList<long, long>();
            var tempFileSeekDescriptionList = new SortedList<long, string>();
            offsetStart = -1;
            offsetEnd = -1;
            try
            {
                for (int i = 0, loopTo = theFileSeekList.Count - 1; i <= loopTo; i++)
                {
                    offsetStart = theFileSeekList.Keys[i];
                    if (offsetEnd < offsetStart - 1L)
                    {
                        description = "[ERROR] Unread bytes";
                        byteValues = GetByteValues(inputFileReader, offsetEnd + 1L, offsetStart - 1L, ref allZeroesWereFound);
                        if (allZeroesWereFound)
                        {
                            description += " (all zeroes)";
                        }
                        else
                        {
                            description += " (non-zero)";
                        }

                        description += byteValues;

                        // Can't add into the list that is being iterated, so use temp list.
                        if (theFileSeekList.ContainsKey(offsetEnd + 1L))
                        {
                            string temp;
                            temp = theFileSeekDescriptionList[offsetEnd + 1L];
                            theFileSeekDescriptionList[offsetEnd + 1L] = "[ERROR] ";
                            theFileSeekDescriptionList[offsetEnd + 1L] += temp + "; [" + (offsetEnd + 1L).ToString() + " - " + (offsetStart - 1L).ToString() + "] " + description;
                        }
                        else
                        {
                            tempFileSeekList.Add(offsetEnd + 1L, offsetStart - 1L);
                            tempFileSeekDescriptionList.Add(offsetEnd + 1L, description);
                        }
                    }

                    offsetEnd = theFileSeekList.Values[i];
                }

                for (int i = 0, loopTo1 = tempFileSeekList.Count - 1; i <= loopTo1; i++)
                    Add(tempFileSeekList.Keys[i], tempFileSeekList.Values[i], tempFileSeekDescriptionList.Values[i]);
            }
            catch (Exception ex)
            {
                int debug = 4242;
            }

            LogErrors();
        }

        public long theFileSize;
        public SortedList<long, long> theFileSeekList;
        public SortedList<long, string> theFileSeekDescriptionList;

        private void LogErrors()
        {
            long offsetStart;
            long offsetEnd;
            try
            {
                for (int i = 0, loopTo = theFileSeekList.Count - 1; i <= loopTo; i++)
                {
                    offsetStart = theFileSeekList.Keys[i];
                    offsetEnd = theFileSeekList.Values[i];
                    if (i < theFileSeekList.Count - 1 && offsetEnd + 1L != theFileSeekList.Keys[i + 1])
                    {
                        theFileSeekDescriptionList[offsetStart] = "[ERROR] [End offset is incorrect] " + theFileSeekDescriptionList[offsetStart];
                    }
                }
            }
            catch (Exception ex)
            {
                int debug = 4242;
            }
        }

        private string GetByteValues(BinaryReader inputFileReader, long fileOffsetStart2, long fileOffsetEnd2, ref bool allZeroesWereFound)
        {
            string byteValues;
            long inputFileStreamPosition;
            inputFileStreamPosition = inputFileReader.BaseStream.Position;
            byte byteValue;
            long adjustedFileOffsetEnd2;
            if (fileOffsetEnd2 - fileOffsetStart2 > 20L)
            {
                adjustedFileOffsetEnd2 = fileOffsetStart2 + 20L;
            }
            else
            {
                adjustedFileOffsetEnd2 = fileOffsetEnd2;
            }

            try
            {
                inputFileReader.BaseStream.Seek(fileOffsetStart2, SeekOrigin.Begin);
                allZeroesWereFound = true;
                byteValues = " [";
                for (long byteOffset = fileOffsetStart2, loopTo = adjustedFileOffsetEnd2; byteOffset <= loopTo; byteOffset++)
                {
                    byteValue = inputFileReader.ReadByte();
                    byteValues += " " + byteValue.ToString("X2");
                    if (byteValue != 0)
                    {
                        allZeroesWereFound = false;
                    }
                }

                if (fileOffsetEnd2 - fileOffsetStart2 > 20L)
                {
                    byteValues += " ...";
                    // 'NOTE: Indicate non-zeroes if more than 20 bytes unread because might be non-zeroes past the first 20.
                    // allZeroesWereFound = False
                    for (long byteOffset = adjustedFileOffsetEnd2 + 1L, loopTo1 = fileOffsetEnd2; byteOffset <= loopTo1; byteOffset++)
                    {
                        byteValue = inputFileReader.ReadByte();
                        if (byteValue != 0)
                        {
                            allZeroesWereFound = false;
                            break;
                        }
                    }
                }

                byteValues += " ]";
                inputFileReader.BaseStream.Seek(inputFileStreamPosition, SeekOrigin.Begin);
            }
            catch (Exception ex)
            {
                allZeroesWereFound = false;
                byteValues = "[incomplete read due to error]";
            }

            return byteValues;
        }
    }
}