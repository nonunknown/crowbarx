using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Reflection;
using Microsoft.VisualBasic.CompilerServices;

namespace Crowbar
{
    public class App //: IDisposable
    {

        #region Create and Destroy

        public App()
        {
            IsDisposed = false;

            // NOTE: To use a particular culture's NumberFormat that doesn't change with user settings, 
            // must use this constructor with False as second param.
            theInternalCultureInfo = new CultureInfo("en-US", false);
            theInternalNumberFormat = theInternalCultureInfo.NumberFormat;
            theSmdFilesWritten = new List<string>();
        }

        public void SetOutputFolder(string path)
        {
            decompiledOutputPath = path;
        }

        public string decompiledOutputPath = "";
        public bool Verbose = false;
        #region IDisposable Support

        // public void Dispose()
        // {
        //     // Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) below.
        //     Dispose(true);
        //     GC.SuppressFinalize(this);
        // }

        // protected virtual void Dispose(bool disposing)
        // {
        //     if (!IsDisposed)
        //     {
        //         if (disposing)
        //         {
        //             Free();
        //         }
        //         // NOTE: free shared unmanaged resources
        //     }

        //     IsDisposed = true;
        // }

        // Protected Overrides Sub Finalize()
        // ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        // Dispose(False)
        // MyBase.Finalize()
        // End Sub

        #endregion

        #endregion

        #region Init and Free
        
        private string GetExePath() {
            string strExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            //This will strip just the working path name:
            string strWorkPath = System.IO.Path.GetDirectoryName(strExeFilePath);
            return strWorkPath;
        }
        public void Init()
        {

            theAppPath = GetExePath();
            if (Verbose)
            {
                Console.WriteLine("-- CX --");
                Console.WriteLine("App path: ", theAppPath);
            }
            // NOTE: Needed for using DLLs placed in folder separate from main EXE file.
            // Environment.SetEnvironmentVariable("path", GetCustomDataPath(), EnvironmentVariableTarget.Process);
            // WriteRequiredFiles();
            // LoadAppSettings();
            // if (Settings.SteamLibraryPaths.Count == 0)
            // {
            //     var libraryPath = new SteamLibraryPath();
            //     Settings.SteamLibraryPaths.Add(libraryPath);
            // }

            // theUnpacker = new Unpacker();
            theDecompiler = new Decompiler();
            // theCompiler = new Compiler();
            // thePacker = new Packer();
            // Me.theModelViewer = New Viewer()

            // string documentsPath;
            // documentsPath = Path.Combine(theAppPath, "Documents");
            // AppConstants.HelpTutorialLink = Path.Combine(documentsPath, AppConstants.HelpTutorialLink);
            // AppConstants.HelpContentsLink = Path.Combine(documentsPath, AppConstants.HelpContentsLink);
            // AppConstants.HelpIndexLink = Path.Combine(documentsPath, AppConstants.HelpIndexLink);
            // AppConstants.HelpTipsLink = Path.Combine(documentsPath, AppConstants.HelpTipsLink);
            theSettings = new AppSettings();
        }

        // private void Free()
        // {
        //     if (theSettings is object)
        //     {
        //         SaveAppSettings();
        //     }
        //     // If Me.theCompiler IsNot Nothing Then
        //     // End If
        // }

        #endregion

        #region Properties

        public AppSettings Settings
        {
            get
            {
                return theSettings;
            }
        }

        public bool CommandLineOption_Settings_IsEnabled
        {
            get
            {
                return theCommandLineOption_Settings_IsEnabled;
            }
        }

        public string ErrorPathFileName
        {
            get
            {
                return Path.Combine(theAppPath, ErrorFileName);
            }
        }

        // public Unpacker Unpacker
        // {
        //     get
        //     {
        //         return theUnpacker;
        //     }
        // }

        public Decompiler Decompiler
        {
            get
            {
                return theDecompiler;
            }
        }

        // public Compiler Compiler
        // {
        //     get
        //     {
        //         return theCompiler;
        //     }
        // }

        // public Packer Packer
        // {
        //     get
        //     {
        //         return thePacker;
        //     }
        // }

        // Public ReadOnly Property Viewer() As Viewer
        // Get
        // Return Me.theModelViewer
        // End Get
        // End Property

        // Public Property ModelRelativePathFileName() As String
        // Get
        // Return Me.theModelRelativePathFileName
        // End Get
        // Set(ByVal value As String)
        // Me.theModelRelativePathFileName = value
        // End Set
        // End Property

        public CultureInfo InternalCultureInfo
        {
            get
            {
                return theInternalCultureInfo;
            }
        }

        public NumberFormatInfo InternalNumberFormat
        {
            get
            {
                return theInternalNumberFormat;
            }
        }

        public List<string> SmdFileNames
        {
            get
            {
                return theSmdFilesWritten;
            }

            set
            {
                theSmdFilesWritten = value;
            }
        }

        #endregion

        #region Methods

        public bool CommandLineValueIsAnAppSetting(string commandLineValue)
        {
            return commandLineValue.StartsWith(SettingsParameter);
        }

        public void WriteRequiredFiles()
        {
            // string steamAPIDLLPathFileName = Path.Combine(GetCustomDataPath(), theSteamAPIDLLFileName);
            // WriteResourceToFileIfDifferent(My.Resources.steam_api, steamAPIDLLPathFileName);

            // // NOTE: Although Crowbar itself does not need the DLL file extracted, CrowbarSteamPipe needs it extracted.
            // string steamworksDotNetPathFileName = Path.Combine(GetCustomDataPath(), theSteamworksDotNetDLLFileName);
            // WriteResourceToFileIfDifferent(My.Resources.Resources.Steamworks_NET, steamworksDotNetPathFileName);
            // string crowbarSteamPipePathFileName = Path.Combine(GetCustomDataPath(), CrowbarSteamPipeFileName);
            // WriteResourceToFileIfDifferent(My.Resources.Resources.CrowbarSteamPipe, crowbarSteamPipePathFileName);
            // LzmaExePathFileName = Path.Combine(GetCustomDataPath(), theLzmaExeFileName);
            // WriteResourceToFileIfDifferent(My.Resources.Resources.lzma, LzmaExePathFileName);

            // // NOTE: Only write settings file if it does not exist.
            // string appSettingsPathFileName = Path.Combine(GetCustomDataPath(), theAppSettingsFileName);
            // try
            // {
            //     if (!File.Exists(appSettingsPathFileName))
            //     {
            //         File.WriteAllText(appSettingsPathFileName, My.Resources.Resources.Crowbar_Settings);
            //     }
            // }
            // catch (Exception ex)
            // {
            //     Console.WriteLine("EXCEPTION: " + ex.Message);
            //     // Throw New Exception(ex.Message, ex.InnerException)
            //     return;
            // }
            // finally
            // {
            // }
        }

        public void WriteUpdaterFiles()
        {
            // SevenZrExePathFileName = Path.Combine(GetCustomDataPath(), theSevenZrEXEFileName);
            // WriteResourceToFileIfDifferent(My.Resources.Resources.SevenZr, SevenZrExePathFileName);
            // CrowbarLauncherExePathFileName = Path.Combine(GetCustomDataPath(), theCrowbarLauncherEXEFileName);
            // WriteResourceToFileIfDifferent(My.Resources.Resources.CrowbarLauncher, CrowbarLauncherExePathFileName);
        }

        // public void DeleteUpdaterFiles()
        // {
        //     SevenZrExePathFileName = Path.Combine(GetCustomDataPath(), theSevenZrEXEFileName);
        //     try
        //     {
        //         if (File.Exists(SevenZrExePathFileName))
        //         {
        //             File.Delete(SevenZrExePathFileName);
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         int debug = 4242;
        //     }

        //     CrowbarLauncherExePathFileName = Path.Combine(GetCustomDataPath(), theCrowbarLauncherEXEFileName);
        //     try
        //     {
        //         if (File.Exists(CrowbarLauncherExePathFileName))
        //         {
        //             File.Delete(CrowbarLauncherExePathFileName);
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         int debug = 4242;
        //     }
        // }

        // public void WriteSteamAppIdFile(uint appID)
        // {
        //     WriteSteamAppIdFile(appID.ToString());
        // }

        // public void WriteSteamAppIdFile(string appID_text)
        // {
        //     string steamAppIDPathFileName = Path.Combine(GetCustomDataPath(), theSteamAppIDFileName);
        //     using (var sw = File.CreateText(steamAppIDPathFileName))
        //     {
        //         sw.WriteLine(appID_text);
        //     }
        // }

        public string GetDebugPath(string outputPath, string modelName)
        {
            // Dim logsPath As String

            // logsPath = Path.Combine(outputPath, modelName + "_" + App.LogsSubFolderName)

            // Return logsPath
            return outputPath;
        }



        #endregion

        #region Private Methods



        private void WriteResourceToFileIfDifferent(byte[] dataResource, string pathFileName)
        {
            try
            {
                bool isDifferentOrNotExist = true;
                if (File.Exists(pathFileName))
                {
                    byte[] resourceHash;
                    var sha = new System.Security.Cryptography.SHA512Managed();
                    resourceHash = sha.ComputeHash(dataResource);
                    var fileStream = File.Open(pathFileName, FileMode.Open);
                    var fileHash = sha.ComputeHash(fileStream);
                    fileStream.Close();
                    isDifferentOrNotExist = false;
                    for (int x = 0, loopTo = resourceHash.Length - 1; x <= loopTo; x++)
                    {
                        if (resourceHash[x] != fileHash[x])
                        {
                            isDifferentOrNotExist = true;
                            break;
                        }
                    }
                }

                if (isDifferentOrNotExist)
                {
                    File.WriteAllBytes(pathFileName, dataResource);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("EXCEPTION: " + ex.Message);
                // Throw New Exception(ex.Message, ex.InnerException)
                return;
            }
            finally
            {
            }
        }

        public string GetHeaderComment()
        {
            string line;
            line = "Created by ";
            line += GetProductNameAndVersion();
            return line;
        }

        public string GetProductNameAndVersion()
        {
            string result;
            result = "CrowbarX";
            result += " ";
            result += "0.3";
            return result;
        }

  

        #endregion

        #region Data

        private bool IsDisposed;
        private CultureInfo theInternalCultureInfo;
        private NumberFormatInfo theInternalNumberFormat;
        private AppSettings theSettings;
        // NOTE: Use slash at start to avoid confusing with a pathFileName that Windows Explorer might use with auto-open.
        public const string SettingsParameter = "/settings=";
        private bool theCommandLineOption_Settings_IsEnabled;

        // Location of the exe.
        private string theAppPath;
        private const string theSteamAPIDLLFileName = "steam_api.dll";
        private const string theSteamworksDotNetDLLFileName = "Steamworks.NET.dll";
        private const string theSevenZrEXEFileName = "7zr.exe";
        private const string theCrowbarLauncherEXEFileName = "CrowbarLauncher.exe";
        private const string theLzmaExeFileName = "lzma.exe";
        public string SevenZrExePathFileName;
        public string CrowbarLauncherExePathFileName;
        public string LzmaExePathFileName;
        // public List<SteamAppInfoBase> SteamAppInfos;
        private const string PreviewsRelativePath = "previews";
        public const string CrowbarSteamPipeFileName = "CrowbarSteamPipe.exe";
        private const string theSteamAppIDFileName = "steam_appid.txt";
        // Private Const theDataFolderName As String = "Data"
        private const string theAppSettingsFileName = "Crowbar Settings.xml";
        public const string AnimsSubFolderName = "anims";
        public const string LogsSubFolderName = "logs";
        private string ErrorFileName = "unhandled_exception_error.txt";
        // private Unpacker theUnpacker;
        private Decompiler theDecompiler;
        // private Compiler theCompiler;
        // private Packer thePacker;
        // Private theModelViewer As Viewer
        private string theModelRelativePathFileName;
        private List<string> theSmdFilesWritten;

        #endregion

    }
}