using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml.Serialization;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace Crowbar
{
    public class FileManager
    {

        

        public static bool FilePathHasInvalidChars(string path)
        {
            bool ret = false;
            if (string.IsNullOrEmpty(path))
            {
                ret = true;
            }
            else
            {
                try
                {
                    string fileName = Path.GetFileName(path);
                    string fileDirectory = Path.GetDirectoryName(path);
                }
                catch (ArgumentException generatedExceptionName)
                {
                    Console.WriteLine("FilePath has invalid chars: ", generatedExceptionName.Message);
                    // Path functions will throw this 
                    // if path contains invalid chars
                    ret = true;
                }

                ret = path.IndexOfAny(Path.GetInvalidPathChars()) >= 0;
                if (ret == false)
                {
                    ret = path.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0;
                }
            }

            return ret;
        }

        public static string ReadNullTerminatedString(BinaryReader inputFileReader)
        {
            var text = new StringBuilder();
            text.Length = 0;
            while (inputFileReader.PeekChar() > 0)
                text.Append(inputFileReader.ReadChar());
            // Read the null character.
            inputFileReader.ReadChar();
            return text.ToString();
        }


        public static bool ReadKeyValueLine(BinaryReader inputFileReader, ref string oKey, ref string oValue)
        {
            string line;
            var delimiters = new[] { '"' };
            var tokens = new[] { "" };
            line = ReadTextLine(inputFileReader);
            if (line is object)
            {
                tokens = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                if (tokens.Length == 3)
                {
                    oKey = tokens[0];
                    oValue = tokens[2];
                    return true;
                }
                else if (tokens.Length == 2)
                {
                    oKey = tokens[0];
                    oValue = tokens[1];
                    return true;
                }
                else if (tokens.Length == 1)
                {
                    oKey = tokens[0];
                    oValue = tokens[0];
                    return false;
                }
            }

            oKey = line;
            oValue = line;
            return false;
        }

        public static string ReadTextLine(BinaryReader inputFileReader)
        {
            var line = new StringBuilder();
            char aChar = ' ';
            try
            {
                while (true)
                {
                    aChar = inputFileReader.ReadChar();
                    if (aChar == '\0' || aChar == '\n')
                    {
                        break;
                    }

                    line.Append(aChar);
                }
            }
            catch (Exception ex)
            {
                int debug = 4242;
            }

            if (line.Length > 0)
            {
                return line.ToString();
            }

            return null;
        }


        public static string GetPathFileNameWithoutExtension(string pathFileName)
        {
            try
            {
                string generatedPath = Path.Combine(GetPath(pathFileName), Path.GetFileNameWithoutExtension(pathFileName));
                if (Program.TheApp.Verbose)
                {
                    Console.WriteLine("GetPathFileNameWithoutExtension: "+ generatedPath);
                }
                return generatedPath;
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetPathFileNameWithoutExtension Error: "+ex.Message);
                return string.Empty;
            }
        }

        public static string GetPath(string pathFileName)
        {
            try
            {
                pathFileName = GetNormalizedPathFileName(pathFileName);
                int length = pathFileName.LastIndexOf(Path.DirectorySeparatorChar);
                if (length < 1)
                {
                    pathFileName = "";
                }
                else if (length > 0)
                {
                    pathFileName = pathFileName.Substring(0, length);
                }

                if (pathFileName.Length == 2 && pathFileName[1] == ':')
                {
                    pathFileName += Conversions.ToString(Path.DirectorySeparatorChar);
                }

                return pathFileName;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public static void CreatePath(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool PathExistsAfterTryToCreate(string aPath)
        {
            if (!Directory.Exists(aPath))
            {
                try
                {
                    if (Program.TheApp.Verbose) 
                    {
                        Console.WriteLine("Creating Dir: "+ aPath);
                    }
                    Directory.CreateDirectory(aPath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error at PathExistsAfterTryToCreate(): "+ex.Message);
                }
            }

            return Directory.Exists(aPath);
        }

        public static string GetRelativePathFileName(string fromPath, string toPath)
        {
            string fromPathAbsolute;
            string toPathAbsolute;
            if (string.IsNullOrEmpty(fromPath))
            {
                return toPath;
            }

            fromPathAbsolute = Path.GetFullPath(fromPath);
            toPathAbsolute = Path.GetFullPath(toPath);

            // Dim fromAttr As Integer = GetPathAttribute(fromPathAbsolute)
            // Dim toAttr As Integer = GetPathAttribute(toPathAbsolute)

            // IMPORTANT: Use Uri.MakeRelativeUri() instead of PathRelativePathTo(), 
            // because PathRelativePathTo() does not handle unicode characters properly.
            // MAX_PATH = 260
            // Dim newPathFileName As New StringBuilder(260)
            // If PathRelativePathTo(newPathFileName, fromPathAbsolute, fromAttr, toPathAbsolute, toAttr) = 0 Then
            // 'Throw New ArgumentException("Paths must have a common prefix")
            // Return toPathAbsolute
            // End If
            // NOTE: Need to add the Path.DirectorySeparatorChar to force MakeRelativeUri() to treat the paths as folder names, not file names.
            // Otherwise, for example, this happens:
            // path1 = "C:\temp\Crowbar"
            // path2 = "C:\temp\Crowbar\addon.txt"
            // diff  = "Crowbar\addon.txt"
            // WANT: diff = "addon.txt"
            var path1 = new Uri(fromPathAbsolute + Conversions.ToString(Path.DirectorySeparatorChar));
            var path2 = new Uri(toPathAbsolute + Conversions.ToString(Path.DirectorySeparatorChar));
            var diff = path1.MakeRelativeUri(path2);
            // Convert Uri escaped characters and convert Uri forward slash to default directory separator.
            string newPathFileName = Uri.UnescapeDataString(diff.OriginalString).Replace("/", Conversions.ToString(Path.DirectorySeparatorChar));
            string cleanedPath;
            cleanedPath = newPathFileName.ToString();
            if (cleanedPath.StartsWith("." + Conversions.ToString(Path.DirectorySeparatorChar)))
            {
                cleanedPath = cleanedPath.Remove(0, 2);
            }
            // NOTE: Remove the ending path separator that is there because of modified inputs to MakeRelativeUri() earlier.
            cleanedPath = cleanedPath.TrimEnd(Path.DirectorySeparatorChar);
            return cleanedPath;
        }

        public static string GetCleanPathFileName(string givenPathFileName, bool returnFullPathFileName)
        {
            string cleanPathFileName;
            string cleanedPathGivenPathFileName;
            cleanedPathGivenPathFileName = givenPathFileName;
            foreach (char invalidChar in Path.GetInvalidPathChars())
                cleanedPathGivenPathFileName = cleanedPathGivenPathFileName.Replace(Conversions.ToString(invalidChar), "");
            if (returnFullPathFileName)
            {
                try
                {
                    cleanedPathGivenPathFileName = Path.GetFullPath(cleanedPathGivenPathFileName);
                }
                catch (Exception ex)
                {
                    cleanedPathGivenPathFileName = cleanedPathGivenPathFileName.Replace(":", "");
                }
            }

            string cleanedGivenFileName;
            cleanedGivenFileName = Path.GetFileName(cleanedPathGivenPathFileName);
            foreach (char invalidChar in Path.GetInvalidFileNameChars())
                cleanedGivenFileName = cleanedGivenFileName.Replace(Conversions.ToString(invalidChar), "");
            string cleanedGivenPath;
            cleanedGivenPath = GetPath(cleanedPathGivenPathFileName);
            if (string.IsNullOrEmpty(cleanedGivenFileName))
            {
                cleanPathFileName = cleanedPathGivenPathFileName;
            }
            else
            {
                cleanPathFileName = Path.Combine(cleanedGivenPath, cleanedGivenFileName);
            }

            return cleanPathFileName;
        }

        public static string GetNormalizedPathFileName(string givenPathFileName)
        {
            string cleanPathFileName;
            cleanPathFileName = givenPathFileName;
            if (Path.DirectorySeparatorChar != Path.AltDirectorySeparatorChar)
            {
                cleanPathFileName = givenPathFileName.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            }

            return cleanPathFileName;
        }

    }
}