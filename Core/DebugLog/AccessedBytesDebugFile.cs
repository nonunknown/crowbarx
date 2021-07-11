using System.IO;
using Microsoft.VisualBasic;

namespace Crowbar
{
    public class AccessedBytesDebugFile
    {

        #region Creation and Destruction

        public AccessedBytesDebugFile(StreamWriter outputFileStream)
        {
            theOutputFileStreamWriter = outputFileStream;
        }

        #endregion

        #region Methods

        public void WriteHeaderComment()
        {
            string line = "";
            line = "// ";
            line += Program.TheApp.GetHeaderComment();
            theOutputFileStreamWriter.WriteLine(line);
        }

        public void WriteFileSeekLog(FileSeekLog aFileSeekLog)
        {
            string line;
            line = "====== File Size ======";
            WriteLogLine(0, line);
            line = aFileSeekLog.theFileSize.ToString("N0");
            WriteLogLine(1, line);
            line = "====== File Seek Log ======";
            WriteLogLine(0, line);

            // line = "--- Summary ---"
            // Me.WriteLogLine(0, line)

            long offsetStart;
            long offsetEnd;
            // offsetStart = -1
            // offsetEnd = -1
            // For i As Integer = 0 To aFileSeekLog.theFileSeekList.Count - 1
            // If offsetStart = -1 Then
            // offsetStart = aFileSeekLog.theFileSeekList.Keys(i)
            // End If
            // offsetEnd = aFileSeekLog.theFileSeekList.Values(i)

            // If aFileSeekLog.theFileSeekDescriptionList.Values(i).StartsWith("[ERROR] Unread bytes") Then
            // If i > 0 Then
            // line = offsetStart.ToString("N0") + " - " + (aFileSeekLog.theFileSeekList.Keys(i) - 1).ToString("N0")
            // Me.WriteLogLine(1, line)
            // End If
            // If aFileSeekLog.theFileSeekDescriptionList.Values(i).StartsWith("[ERROR] Unread bytes (all zeroes)") Then
            // line = aFileSeekLog.theFileSeekList.Keys(i).ToString("N0") + " - " + offsetEnd.ToString("N0") + " [ERROR] Unread bytes (all zeroes)"
            // Else
            // line = aFileSeekLog.theFileSeekList.Keys(i).ToString("N0") + " - " + offsetEnd.ToString("N0") + " [ERROR] Unread bytes (non-zero)"
            // End If
            // Me.WriteLogLine(1, line)
            // offsetStart = -1
            // ElseIf (i = aFileSeekLog.theFileSeekList.Count - 1) OrElse (offsetEnd + 1 <> aFileSeekLog.theFileSeekList.Keys(i + 1)) Then
            // line = offsetStart.ToString("N0") + " - " + offsetEnd.ToString("N0")
            // Me.WriteLogLine(1, line)
            // offsetStart = -1
            // End If
            // Next

            // line = "------------------------"
            // Me.WriteLogLine(0, line)
            // line = "--- Each Section or Loop ---"
            // Me.WriteLogLine(0, line)

            offsetEnd = -1;
            for (int i = 0, loopTo = aFileSeekLog.theFileSeekList.Count - 1; i <= loopTo; i++)
            {
                offsetStart = aFileSeekLog.theFileSeekList.Keys[i];
                offsetEnd = aFileSeekLog.theFileSeekList.Values[i];
                line = offsetStart.ToString("N0") + " - " + offsetEnd.ToString("N0") + " " + aFileSeekLog.theFileSeekDescriptionList.Values[i];
                WriteLogLine(1, line);
            }

            line = "========================";
            WriteLogLine(0, line);
        }

        #endregion

        #region Private Methods

        private void WriteFileSeparatorLines()
        {
            string line;
            WriteLogLine(0, "");
            WriteLogLine(0, "");
            line = "################################################################################";
            WriteLogLine(0, line);
            WriteLogLine(0, "");
            WriteLogLine(0, "");
        }

        private void WriteLogLine(int indentLevel, string line)
        {
            string indentedLine = "";
            for (int i = 1, loopTo = indentLevel; i <= loopTo; i++)
                indentedLine += Constants.vbTab;
            indentedLine += line;
            theOutputFileStreamWriter.WriteLine(indentedLine);
            theOutputFileStreamWriter.Flush();
        }

        #endregion

        #region Data

        private StreamWriter theOutputFileStreamWriter;

        #endregion

    }
}