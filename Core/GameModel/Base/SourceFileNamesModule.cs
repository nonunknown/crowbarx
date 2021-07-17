using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualBasic.CompilerServices;

namespace Crowbar
{
    static class SourceFileNamesModule
    {
        public static string CreateBodyGroupSmdFileName(string givenBodyGroupSmdFileName, int bodyPartIndex, int modelIndex, int lodIndex, string modelName, string bodyModelName)
        {
            //TODO ?: naming bug solve here?
            // Use bodyModel name, but make sure the file name is unique for this model.
            string bodyGroupSmdFileName = "";
            string bodyModelFileName = "";
            string bodyModelFileNameWithoutExtension = "";

            

            if (!string.IsNullOrEmpty(givenBodyGroupSmdFileName))
            {
                bodyGroupSmdFileName = givenBodyGroupSmdFileName;
            }
            else
            {
                try
                {
                    bodyModelFileName = Path.GetFileName(bodyModelName.Trim('\0'));
                    //Remove windows backslash that was being inserted to the filename
                    if (FileManager.FilePathHasInvalidChars(bodyModelFileName))
                    {
                        bodyModelFileName = "body";
                        bodyModelFileName += bodyPartIndex.ToString();
                        bodyModelFileName += "_model";
                        bodyModelFileName += modelIndex.ToString();
                    }
                    if (bodyModelFileName.Contains("\\"))
                    {
                        string[] result = bodyModelFileName.Split("\\");
                        bodyModelFileName = result[result.Length-1]; //get always the last result
                        // bodyModelFileName = bodyModelFileName.Replace("\\","/");
                    }
                    
                }
                catch (Exception ex)
                {
                    bodyModelFileName = "body";
                    bodyModelFileName += bodyPartIndex.ToString();
                    bodyModelFileName += "_model";
                    bodyModelFileName += modelIndex.ToString();
                }



                bodyModelFileNameWithoutExtension = Path.GetFileNameWithoutExtension(bodyModelFileName);
                if (Program.TheApp.Settings.DecompilePrefixFileNamesWithModelNameIsChecked && !bodyModelFileName.ToLower(Program.TheApp.InternalCultureInfo).StartsWith(modelName.ToLower(Program.TheApp.InternalCultureInfo)))
                {
                    bodyGroupSmdFileName += modelName + "_";
                }

                bodyGroupSmdFileName += bodyModelFileNameWithoutExtension;
                if (lodIndex > 0)
                {
                    bodyGroupSmdFileName += "_lod";
                    bodyGroupSmdFileName += lodIndex.ToString();
                }

                bodyGroupSmdFileName = GetUniqueSmdFileName(bodyGroupSmdFileName);
                bodyGroupSmdFileName += ".smd";
            }

            return bodyGroupSmdFileName;
        }

        public static string GetAnimationSmdRelativePath(string modelName)
        {
            string path;
            path = "";
            if (Program.TheApp.Settings.DecompileBoneAnimationPlaceInSubfolderIsChecked)
            {
                path = modelName + "_" + App.AnimsSubFolderName;
            }

            return path;
        }

        public static string CreateAnimationSmdRelativePathFileName(string givenAnimationSmdRelativePathFileName, string modelName, string iAnimationName, int blendIndex = -2)
        {
            string animationName;
            string animationSmdRelativePathFileName;
            if (!string.IsNullOrEmpty(givenAnimationSmdRelativePathFileName))
            {
                animationSmdRelativePathFileName = givenAnimationSmdRelativePathFileName;
            }
            else
            {
                // Clean the iAnimationName.
                try
                {
                    iAnimationName = iAnimationName.Trim('\0');
                    // iAnimationName = iAnimationName.Replace(":", "")
                    // iAnimationName = iAnimationName.Replace("\", "")
                    // iAnimationName = iAnimationName.Replace("/", "")
                    foreach (char invalidChar in Path.GetInvalidFileNameChars())
                        iAnimationName = iAnimationName.Replace(Conversions.ToString(invalidChar), "");
                    if (FileManager.FilePathHasInvalidChars(iAnimationName))
                    {
                        iAnimationName = "anim";
                    }
                }
                catch (Exception ex)
                {
                    iAnimationName = "anim";
                }

                // Set the name
                if (blendIndex >= 0)
                {
                    // For MDL v6 and v10.
                    animationName = iAnimationName + "_blend" + (blendIndex + 1).ToString("00");
                }
                else if (blendIndex == -1)
                {
                    // For MDL v6 and v10.
                    animationName = iAnimationName;
                }
                else if (string.IsNullOrEmpty(iAnimationName))
                {
                    animationName = "";
                }
                else if (iAnimationName[0] == '@')
                {
                    // NOTE: The file name for the animation data file is not stored in mdl file (which makes sense), 
                    // so make the file name the same as the animation name.
                    animationName = iAnimationName.Substring(1);
                }
                else
                {
                    animationName = iAnimationName;
                }

                // If anims are not stored in anims folder, add some more to the name.
                if (!Program.TheApp.Settings.DecompileBoneAnimationPlaceInSubfolderIsChecked)
                {
                    animationName = modelName + "_anim_" + iAnimationName;
                }

                // Set the path.
                animationSmdRelativePathFileName = Path.Combine(GetAnimationSmdRelativePath(modelName), animationName);
                animationSmdRelativePathFileName = GetUniqueSmdFileName(animationSmdRelativePathFileName);

                // Set the extension.
                if (Path.GetExtension(animationSmdRelativePathFileName) != ".smd")
                {
                    // animationSmdRelativePathFileName = Path.ChangeExtension(animationSmdRelativePathFileName, ".smd")
                    // NOTE: Add the ".smd" extension, keeping the existing extension in file name, which is often ".dmx" for newer models. 
                    // Thus, user can see that model might have newer features that Crowbar does not yet handle.
                    animationSmdRelativePathFileName += ".smd";
                }
            }

            return animationSmdRelativePathFileName;
        }

        public static string CreateCorrectiveAnimationName(string givenAnimationSmdRelativePathFileName)
        {
            string animationName;
            animationName = givenAnimationSmdRelativePathFileName + "_" + "corrective_animation";
            return animationName;
        }

        public static string CreateCorrectiveAnimationSmdRelativePathFileName(string givenAnimationSmdRelativePathFileName, string modelName)
        {
            string animationSmdRelativePathFileName;
            animationSmdRelativePathFileName = CreateCorrectiveAnimationName(givenAnimationSmdRelativePathFileName) + ".smd";
            animationSmdRelativePathFileName = Path.Combine(GetAnimationSmdRelativePath(modelName), animationSmdRelativePathFileName);
            return animationSmdRelativePathFileName;
        }

        public static string GetVrdFileName(string modelName)
        {
            string vrdFileName;
            vrdFileName = modelName;
            vrdFileName += ".vrd";
            return vrdFileName;
        }

        public static string GetVtaFileName(string modelName, int bodyPartIndex)
        {
            string vtaFileName;
            vtaFileName = modelName;
            vtaFileName += "_";
            vtaFileName += (bodyPartIndex + 1).ToString("00");
            vtaFileName += ".vta";
            return vtaFileName;
        }

        public static string CreatePhysicsSmdFileName(string givenPhysicsSmdFileName, string modelName)
        {
            string physicsSmdFileName;
            if (!string.IsNullOrEmpty(givenPhysicsSmdFileName))
            {
                physicsSmdFileName = givenPhysicsSmdFileName;
            }
            else
            {
                physicsSmdFileName = modelName;
                physicsSmdFileName += "_physics";
                physicsSmdFileName = GetUniqueSmdFileName(physicsSmdFileName);
                physicsSmdFileName += ".smd";
            }

            return physicsSmdFileName;
        }

        public static string GetDeclareSequenceQciFileName(string modelName)
        {
            string declareSequenceQciFileName;
            declareSequenceQciFileName = modelName;
            declareSequenceQciFileName += "_DeclareSequence.qci";
            return declareSequenceQciFileName;
        }

        // 'TODO: Call *after* both ReadTextures() and ReadTexturePaths() are called.
        // Public Sub CopyPathsFromTextureFileNamesToTexturePaths(ByVal texturePaths As List(Of String), ByVal texturePathFileNames As List(Of String))
        // ' Make all lowercase list copy of texturePaths.
        // Dim texturePathsLowercase As List(Of String)
        // texturePathsLowercase = New List(Of String)(texturePaths.Count)
        // For Each aTexturePath As String In texturePaths
        // texturePathsLowercase.Add(aTexturePath.ToLower())
        // Next

        // For texturePathFileNameIndex As Integer = 0 To texturePathFileNames.Count - 1
        // Dim aTexturePathFileName As String
        // Dim aTexturePathFileNameLowercase As String
        // aTexturePathFileName = texturePathFileNames(texturePathFileNameIndex)
        // aTexturePathFileNameLowercase = aTexturePathFileName.ToLower()

        // ' If the texturePathFileName starts with a path that is in the texturePaths list, then remove the texturePath from the texturePathFileName.
        // For texturePathIndex As Integer = 0 To texturePathsLowercase.Count - 1
        // Dim aTexturePathLowercase As String
        // aTexturePathLowercase = texturePathsLowercase(texturePathIndex)

        // If aTexturePathLowercase <> "" AndAlso aTexturePathFileNameLowercase.StartsWith(aTexturePathLowercase) Then
        // Dim startOffsetAfterPathSeparator As Integer
        // If aTexturePathLowercase.EndsWith(Path.DirectorySeparatorChar) OrElse aTexturePathLowercase.EndsWith(Path.AltDirectorySeparatorChar) Then
        // startOffsetAfterPathSeparator = aTexturePathLowercase.Length
        // Else
        // startOffsetAfterPathSeparator = aTexturePathLowercase.Length + 1
        // End If
        // texturePathFileNames(texturePathFileNameIndex) = aTexturePathFileName.Substring(startOffsetAfterPathSeparator)
        // Exit For
        // End If
        // Next

        // Dim texturePath As String
        // Dim texturePathLowercase As String
        // Dim textureFileName As String
        // texturePath = FileManager.GetPath(aTexturePathFileName)
        // texturePathLowercase = texturePath.ToLower()
        // textureFileName = Path.GetFileName(aTexturePathFileName)
        // If aTexturePathFileName <> textureFileName AndAlso Not texturePathsLowercase.Contains(texturePathLowercase) AndAlso Not texturePathsLowercase.Contains(texturePathLowercase + Path.DirectorySeparatorChar) AndAlso Not texturePathsLowercase.Contains(texturePathLowercase + Path.AltDirectorySeparatorChar) Then
        // 'NOTE: Place first because it should override whatever is already in list.
        // texturePaths.Insert(0, texturePath)
        // End If
        // Next
        // End Sub

        // NOTE: Call *after* both ReadTextures() and ReadTexturePaths() are called.
        public static void MovePathsFromTextureFileNamesToTexturePaths(ref List<string> texturePaths, ref List<string> texturePathFileNames)
        {
            // Make all lowercase list copy of texturePaths.
            List<string> texturePathsLowercase;
            texturePathsLowercase = new List<string>(texturePaths.Count);
            foreach (string aTexturePath in texturePaths)
                texturePathsLowercase.Add(aTexturePath.ToLower());

            // NOTE: Use index so can modify the list member, not a copy of it.
            for (int fileNameIndex = 0, loopTo = texturePathFileNames.Count - 1; fileNameIndex <= loopTo; fileNameIndex++)
            {
                string aTexturePathFileName;
                aTexturePathFileName = texturePathFileNames[fileNameIndex];
                aTexturePathFileName = FileManager.GetCleanPathFileName(aTexturePathFileName, false);
                string aTexturePath;
                string aTextureFileName;
                aTexturePath = FileManager.GetPath(aTexturePathFileName);
                aTextureFileName = Path.GetFileName(aTexturePathFileName);
                string aTexturePathFileNameLowercase;
                string aTexturePathLowercase;
                aTexturePathFileNameLowercase = aTexturePathFileName.ToLower();
                aTexturePathLowercase = FileManager.GetPath(aTexturePathFileNameLowercase);

                // If the texturePathFileName starts with a path, then ...
                if (!string.IsNullOrEmpty(aTexturePathLowercase))
                {
                    // ... insert the path into texturePaths, if it is not already there.
                    if (!texturePathsLowercase.Contains(aTexturePathLowercase) && !texturePathsLowercase.Contains(aTexturePathLowercase + Conversions.ToString(Path.DirectorySeparatorChar)) && !texturePathsLowercase.Contains(aTexturePathLowercase + Conversions.ToString(Path.AltDirectorySeparatorChar)))
                    {
                        // NOTE: Place first because it should override whatever is already in list.
                        texturePaths.Insert(0, aTexturePath);
                        texturePathsLowercase.Insert(0, aTexturePathLowercase);
                    }

                    // ... and remove it from the texturePathFileName in texturePathFileNames.
                    texturePathFileNames[fileNameIndex] = aTextureFileName;
                }
            }
        }

        private static string GetUniqueSmdFileName(string givenSmdFileName)
        {
            string smdFileName = givenSmdFileName;

            // NOTE: Starting this at 1 means the first file name will not have a number and the second name will have a 2.
            int nameNumber = 1;
            while (Program.TheApp.SmdFileNames.Contains(smdFileName.ToLower(Program.TheApp.InternalCultureInfo)))
            {
                nameNumber += 1;
                smdFileName = givenSmdFileName + "_" + nameNumber.ToString();
            }

            Program.TheApp.SmdFileNames.Add(smdFileName.ToLower(Program.TheApp.InternalCultureInfo));
            return smdFileName;
        }
    }
}