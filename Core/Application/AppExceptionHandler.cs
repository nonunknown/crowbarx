using System;
using System.IO;
using System.Threading;
using Microsoft.VisualBasic;

namespace Crowbar
{
    public class AppExceptionHandler
    {
        public void Application_ThreadException(object sender, ThreadExceptionEventArgs t)
        {
            
            try
            {
                string errorReportText;
                errorReportText = "################################################################################";
                errorReportText += Constants.vbCrLf;
                errorReportText += "###### ";
                errorReportText += DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                errorReportText += "   ";
                // errorReportText += My.MyProject.Application.Info.ProductName;
                errorReportText += " ";
                // errorReportText += My.MyProject.Application.Info.Version.ToString(2);
                errorReportText += Constants.vbCrLf;
                errorReportText += Constants.vbCrLf;
                errorReportText += "=== Steps to reproduce the error ===";
                errorReportText += Constants.vbCrLf;
                errorReportText += "[Describe the last few tasks you did in ";
                // errorReportText += My.MyProject.Application.Info.ProductName;
                errorReportText += " before the error occurred.]";
                errorReportText += Constants.vbCrLf;
                errorReportText += Constants.vbCrLf;
                errorReportText += "=== What you expected to see ===";
                errorReportText += Constants.vbCrLf;
                errorReportText += "[Explain what you expected ";
                // errorReportText += My.MyProject.Application.Info.ProductName;
                errorReportText += " to do.]";
                errorReportText += Constants.vbCrLf;
                errorReportText += Constants.vbCrLf;
                errorReportText += "=== Context info ===";
                errorReportText += Constants.vbCrLf;
                if (Program.TheApp is null)
                {
                    errorReportText += "Exception occured before or after TheApp's lifetime.";
                }
                else
                {
                    // If TheApp.Settings.ViewerIsRunning Then
                    // errorReportText += "Viewing "
                    // errorReportText += TheApp.Settings.ViewMdlPathFileName
                    // errorReportText += vbCrLf
                    // End If
                    if (Program.TheApp.Settings.DecompilerIsRunning)
                    {
                        errorReportText += "Decompiling ";
                        errorReportText += Program.TheApp.Settings.DecompileMdlPathFileName;
                        errorReportText += Constants.vbCrLf;
                    }

                    if (Program.TheApp.Settings.CompilerIsRunning)
                    {
                        errorReportText += "Compiling ";
                        errorReportText += Program.TheApp.Settings.CompileQcPathFileName;
                        errorReportText += Constants.vbCrLf;
                    }
                }

                errorReportText += Constants.vbCrLf;
                errorReportText += Constants.vbCrLf;
                errorReportText += "=== Exception error description ===";
                errorReportText += Constants.vbCrLf;
                errorReportText += t.Exception.Message;
                errorReportText += Constants.vbCrLf;
                errorReportText += Constants.vbCrLf;
                errorReportText += "=== Call stack ===";
                errorReportText += Constants.vbCrLf;
                errorReportText += t.Exception.StackTrace;
                errorReportText += Constants.vbCrLf;
                errorReportText += Constants.vbCrLf;
                errorReportText += Constants.vbCrLf;
                errorReportText += Constants.vbCrLf;
                WriteToErrorFile(errorReportText);
                // anUnhandledExceptionWindow.ErrorReportTextBox.Text = errorReportText;
                // anUnhandledExceptionWindow.ShowDialog();
            }
            catch
            {
            }
            finally
            {
                // anUnhandledExceptionWindow.Dispose();
            }

            throw new Exception("Error");
        }

        private void WriteToErrorFile(string errorReportText)
        {
            using (var sw = new StreamWriter(Program.TheApp.ErrorPathFileName, true))
            {
                sw.Write(errorReportText);
                sw.Close();
            }
        }
    }
}