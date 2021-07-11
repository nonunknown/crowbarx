using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualBasic;

namespace Crowbar
{
    public class SourceQcFile
    {

        #region Methods

        public string GetQcModelName(string qcPathFileName)
        {
            string qcModelName;
            qcModelName = "";
            using (var inputFileStream = new StreamReader(qcPathFileName))
            {
                string inputLine;
                string modifiedLine;
                while (!inputFileStream.EndOfStream)
                {
                    inputLine = inputFileStream.ReadLine();
                    modifiedLine = inputLine.ToLower().TrimStart();
                    if (modifiedLine.StartsWith("\"$modelname\""))
                    {
                        modifiedLine = modifiedLine.Replace("\"$modelname\"", "$modelname");
                    }

                    if (modifiedLine.StartsWith("$modelname"))
                    {
                        modifiedLine = modifiedLine.Replace("$modelname", "");
                        modifiedLine = modifiedLine.Trim();

                        // Need to remove any comment after the file name token (which may or may not be double-quoted).
                        int pos;
                        if (modifiedLine.StartsWith("\""))
                        {
                            pos = modifiedLine.IndexOf("\"", 1);
                            if (pos >= 0)
                            {
                                modifiedLine = modifiedLine.Substring(1, pos - 1);
                            }
                        }
                        else
                        {
                            pos = modifiedLine.IndexOf(" ");
                            if (pos >= 0)
                            {
                                modifiedLine = modifiedLine.Substring(0, pos);
                            }
                        }

                        // temp = temp.Trim(Chr(34))
                        qcModelName = modifiedLine.Replace("/", @"\");
                        break;
                    }
                }
            }

            return qcModelName;
        }

        public string InsertAnIncludeFileCommand(string qcPathFileName, string qciPathFileName)
        {
            string line = "";
            using (var outputFileStream = File.AppendText(qcPathFileName))
            {
                outputFileStream.WriteLine();
                if (Program.TheApp.Settings.DecompileQcUseMixedCaseForKeywordsIsChecked)
                {
                    line += "$Include";
                }
                else
                {
                    line += "$include";
                }

                line += " ";
                line += "\"";
                line += FileManager.GetRelativePathFileName(FileManager.GetPath(qcPathFileName), qciPathFileName);
                line += "\"";
                outputFileStream.WriteLine(line);
            }

            return line;
        }

        #endregion

        #region Private Delegates

        // Private Delegate Sub WriteGroupDelegate()

        #endregion

        #region Private Methods

        // Private Sub WriteHeaderComment()
        // Dim line As String = ""

        // line = "// "
        // line += TheApp.GetHeaderComment()
        // Me.theOutputFileStream.WriteLine(line)
        // End Sub

        // Private Sub WriteModelNameCommand()
        // Dim line As String = ""
        // 'Dim modelPath As String
        // Dim modelPathFileName As String

        // 'modelPath = FileManager.GetPath(CStr(theSourceEngineModel.theMdlFileHeader.name).Trim(Chr(0)))
        // 'modelPathFileName = Path.Combine(modelPath, theSourceEngineModel.ModelName + ".mdl")
        // 'modelPathFileName = CStr(theSourceEngineModel.MdlFileHeader.name).Trim(Chr(0))
        // modelPathFileName = theSourceEngineModel.MdlFileHeader.theName

        // Me.theOutputFileStream.WriteLine()

        // '$modelname "survivors/survivor_producer.mdl"
        // '$modelname "custom/survivor_producer.mdl"
        // If TheApp.Settings.DecompileQcUseMixedCaseForKeywordsIsChecked Then
        // line = "$ModelName "
        // Else
        // line = "$modelname "
        // End If
        // line += """"
        // line += modelPathFileName
        // line += """"
        // Me.theOutputFileStream.WriteLine(line)
        // End Sub

        // Private Sub WriteGroup(ByVal qciGroupName As String, ByVal writeGroupAction As WriteGroupDelegate, ByVal includeLineIsCommented As Boolean, ByVal includeLineIsIndented As Boolean)
        // If TheApp.Settings.DecompileGroupIntoQciFilesIsChecked Then
        // Dim qciFileName As String
        // Dim qciPathFileName As String
        // Dim mainOutputFileStream As StreamWriter

        // mainOutputFileStream = Me.theOutputFileStream

        // Try
        // 'qciPathFileName = Path.Combine(Me.theOutputPathName, Me.theOutputFileNameWithoutExtension + "_flexes.qci")
        // qciFileName = Me.theOutputFileNameWithoutExtension + "_" + qciGroupName + ".qci"
        // qciPathFileName = Path.Combine(Me.theOutputPathName, qciFileName)

        // Me.theOutputFileStream = File.CreateText(qciPathFileName)

        // 'Me.WriteFlexLines()
        // 'Me.WriteFlexControllerLines()
        // 'Me.WriteFlexRuleLines()
        // writeGroupAction.Invoke()
        // Catch ex As Exception
        // Throw
        // Finally
        // If Me.theOutputFileStream IsNot Nothing Then
        // Me.theOutputFileStream.Flush()
        // Me.theOutputFileStream.Close()

        // Me.theOutputFileStream = mainOutputFileStream
        // End If
        // End Try

        // Try
        // If File.Exists(qciPathFileName) Then
        // Dim qciFileInfo As New FileInfo(qciPathFileName)
        // If qciFileInfo.Length > 0 Then
        // Dim line As String = ""

        // Me.theOutputFileStream.WriteLine()

        // If includeLineIsCommented Then
        // line += "// "
        // End If
        // If includeLineIsIndented Then
        // line += vbTab
        // End If
        // line += "$Include"
        // line += " "
        // line += """"
        // line += qciFileName
        // line += """"
        // Me.theOutputFileStream.WriteLine(line)
        // End If
        // End If
        // Catch ex As Exception
        // Throw
        // End Try
        // Else
        // 'Me.WriteFlexLines()
        // 'Me.WriteFlexControllerLines()
        // 'Me.WriteFlexRuleLines()
        // writeGroupAction.Invoke()
        // End If
        // End Sub

        protected List<List<short>> GetSkinFamiliesOfChangedMaterials(List<List<short>> iSkinFamilies)
        {
            List<List<short>> skinFamilies;
            int skinReferenceCount;
            List<short> firstSkinFamily;
            List<short> aSkinFamily;
            List<short> textureFileNameIndexes;
            skinReferenceCount = iSkinFamilies[0].Count;
            skinFamilies = new List<List<short>>(iSkinFamilies.Count);
            try
            {
                for (int skinFamilyIndex = 0, loopTo = iSkinFamilies.Count - 1; skinFamilyIndex <= loopTo; skinFamilyIndex++)
                {
                    textureFileNameIndexes = new List<short>(skinReferenceCount);
                    skinFamilies.Add(textureFileNameIndexes);
                }

                firstSkinFamily = iSkinFamilies[0];
                for (int j = 0, loopTo1 = skinReferenceCount - 1; j <= loopTo1; j++)
                {
                    // NOTE: Start at second skin family because comparing first with all others.
                    for (int i = 1, loopTo2 = iSkinFamilies.Count - 1; i <= loopTo2; i++)
                    {
                        aSkinFamily = iSkinFamilies[i];
                        if (firstSkinFamily[j] != aSkinFamily[j])
                        {
                            for (int skinFamilyIndex = 0, loopTo3 = iSkinFamilies.Count - 1; skinFamilyIndex <= loopTo3; skinFamilyIndex++)
                            {
                                aSkinFamily = iSkinFamilies[skinFamilyIndex];
                                textureFileNameIndexes = skinFamilies[skinFamilyIndex];
                                textureFileNameIndexes.Add(aSkinFamily[j]);
                            }

                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                int debug = 4242;
            }

            return skinFamilies;
        }

        protected List<string> GetTextureGroupSkinFamilyLines(List<List<string>> skinFamilies)
        {
            var lines = new List<string>();
            List<string> aSkinFamily;
            string aTextureFileName;
            string line = "";
            if (Program.TheApp.Settings.DecompileQcSkinFamilyOnSingleLineIsChecked)
            {
                var textureFileNameMaxLengths = new List<int>();
                int length;
                aSkinFamily = skinFamilies[0];
                for (int textureFileNameIndex = 0, loopTo = aSkinFamily.Count - 1; textureFileNameIndex <= loopTo; textureFileNameIndex++)
                {
                    aTextureFileName = aSkinFamily[textureFileNameIndex];
                    length = aTextureFileName.Length;
                    textureFileNameMaxLengths.Add(length);
                }

                for (int skinFamilyIndex = 1, loopTo1 = skinFamilies.Count - 1; skinFamilyIndex <= loopTo1; skinFamilyIndex++)
                {
                    aSkinFamily = skinFamilies[skinFamilyIndex];
                    for (int textureFileNameIndex = 0, loopTo2 = aSkinFamily.Count - 1; textureFileNameIndex <= loopTo2; textureFileNameIndex++)
                    {
                        aTextureFileName = aSkinFamily[textureFileNameIndex];
                        length = aTextureFileName.Length;
                        if (length > textureFileNameMaxLengths[textureFileNameIndex])
                        {
                            textureFileNameMaxLengths[textureFileNameIndex] = length;
                        }
                    }
                }

                for (int skinFamilyIndex = 0, loopTo3 = skinFamilies.Count - 1; skinFamilyIndex <= loopTo3; skinFamilyIndex++)
                {
                    aSkinFamily = skinFamilies[skinFamilyIndex];
                    line = Constants.vbTab;
                    line += "{";
                    line += " ";
                    for (int textureFileNameIndex = 0, loopTo4 = aSkinFamily.Count - 1; textureFileNameIndex <= loopTo4; textureFileNameIndex++)
                    {
                        aTextureFileName = aSkinFamily[textureFileNameIndex];
                        length = textureFileNameMaxLengths[textureFileNameIndex];

                        // NOTE: Need at least "+ 2" to account for the double-quotes.
                        line += Strings.LSet("\"" + aTextureFileName + "\"", length + 3);
                    }

                    // line += " "
                    line += "}";
                    lines.Add(line);
                }
            }
            else
            {
                for (int skinFamilyIndex = 0, loopTo5 = skinFamilies.Count - 1; skinFamilyIndex <= loopTo5; skinFamilyIndex++)
                {
                    aSkinFamily = skinFamilies[skinFamilyIndex];
                    line = Constants.vbTab;
                    line += "{";
                    lines.Add(line);
                    for (int textureFileNameIndex = 0, loopTo6 = aSkinFamily.Count - 1; textureFileNameIndex <= loopTo6; textureFileNameIndex++)
                    {
                        aTextureFileName = aSkinFamily[textureFileNameIndex];
                        line = Constants.vbTab;
                        line += Constants.vbTab;
                        line += "\"";
                        line += aTextureFileName;
                        line += "\"";
                        lines.Add(line);
                    }

                    line = Constants.vbTab;
                    line += "}";
                    lines.Add(line);
                }
            }

            return lines;
        }

        #endregion

        #region Data

        // Private theSourceEngineModel As SourceModel_Old
        // Private theOutputFileStream As StreamWriter
        // Private theOutputPathName As String
        // Private theOutputFileNameWithoutExtension As String

        #endregion

    }
}