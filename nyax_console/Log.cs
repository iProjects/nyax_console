﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;

namespace nyax_console
{
    public class Log
    {

        public static string logFileName;
        public static string errorLogFileName;

        /// <summary>
        /// Static Constructor
        /// </summary>
        static Log()
        {

            logFileName = GetSetting("LOGFILENAME");
            errorLogFileName = GetSetting("ERRORLOGFILENAME");

            if (logFileName == null) logFileName = "C:\\SBlog.log";
            if (errorLogFileName == null) logFileName = "C:\\SBerrlog.log";

            IsDirectoryPresent(StripDirectoryName(logFileName), true);
            IsDirectoryPresent(StripDirectoryName(errorLogFileName), true);
        }


        /// <summary>
        /// Gets The File Name From Specified Path
        /// </summary>
        public static string GetFileNameFromPath(string path)
        {
            string fileName = @"";
            int indexOfLastSlash = 0;
            try
            {
                indexOfLastSlash = path.LastIndexOf(@"\");
                fileName = path.Substring(indexOfLastSlash + 1);
                return fileName;
            }
            catch (Exception ex)
            {
                WriteToErrorLogFile(ex);
                return "";
            }
            finally
            {
            }
        }

        /// <summary>
        /// Gets The Directory Path from the FilePath
        /// </summary>
        public static string StripDirectoryName(string path)
        {
            string direcoryPath = @"";
            int indexOfLastSlash = 0;

            try
            {
                indexOfLastSlash = path.LastIndexOf(@"\");
                direcoryPath = path.Substring(0, indexOfLastSlash);
                return direcoryPath;
            }
            catch (Exception ex)
            {
                WriteToErrorLogFile(ex);
                return "";
            }
            finally
            {
            }
        }


        /// <summary>
        /// Gets Values From The Config File.
        /// </summary>
        public static bool IsDirectoryPresent(string directory, bool create)
        {
            try
            {
                if (!Directory.Exists(directory))
                {
                    if (create == true)
                    {
                        Directory.CreateDirectory(directory);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                WriteToErrorLogFile(ex);
                return false;
            }
            finally
            {
            }
        }


        /// <summary>
        /// Gets Values From The Config File.
        /// </summary>
        public static string GetSetting(string val)
        {
            try
            {
                return System.Configuration.ConfigurationManager.AppSettings[val];
            }
            catch (Exception ex)
            {
                WriteToErrorLogFile(ex);
                return "";
            }
            finally
            {
            }
        }


        /// <summary>
        /// Writes the message to the XX Log File
        /// </summary>
        public static void WriteToLogFile(string fKey, string message)
        {
            string log = GetSetting(fKey);
            if (log == null) return;

            if (IsDirectoryPresent(StripDirectoryName(log), true))
            {
                FileStream fs = null;
                StreamWriter sw = null;
                string fileName;

                try
                {
                    fileName = log;
                    message = DateTime.Now.ToString() + " - " + message;
                    fs = new FileStream(fileName, FileMode.Append, FileAccess.Write);
                    sw = new StreamWriter(fs);
                    sw.WriteLine(message);
                }
                catch (Exception ex)
                {
                    WriteToErrorLogFile(ex);
                }
                finally
                {
                    if (sw != null)
                    {
                        sw.Close();
                    }

                    if (fs != null)
                    {
                        fs.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Writes the message to the FileSystem Watcher Log File
        /// </summary>
        public static void WriteToLogFile(string message)
        {
            if (IsDirectoryPresent(StripDirectoryName(logFileName), true))
            {
                FileStream fs = null;
                StreamWriter sw = null;
                string fileName;

                try
                {
                    fileName = logFileName;
                    message = DateTime.Now.ToString() + " - " + message;
                    fs = new FileStream(fileName, FileMode.Append, FileAccess.Write);
                    sw = new StreamWriter(fs);
                    sw.WriteLine(message);
                }
                catch (Exception ex)
                {
                    WriteToErrorLogFile(ex);
                }
                finally
                {
                    if (sw != null)
                    {
                        sw.Close();
                    }

                    if (fs != null)
                    {
                        fs.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Writes the Exception to the Error Log File
        /// </summary>
        public static bool WriteToErrorLogFile(Exception sourceException)
        {
            if (Utils.LogEventViewer(sourceException)) { }
            if (Write_To_Log_File_temp_dir(sourceException)) { }

            if (IsDirectoryPresent(StripDirectoryName(errorLogFileName), true))
            {
                FileStream fs = null;
                StreamWriter sw = null;
                try
                {
                    fs = new FileStream(errorLogFileName, FileMode.Append, FileAccess.Write);
                    sw = new StreamWriter(fs);
                    sw.WriteLine("==================================================================");
                    sw.WriteLine("ERROR OCCOURED AT :" + DateTime.Now.ToString());
                    sw.WriteLine("SOURCE:" + sourceException.Source);
                    sw.WriteLine("MESSAGE:" + sourceException.Message);
                    sw.WriteLine("Whole Exception:" + sourceException.ToString());
                    sw.WriteLine("==================================================================");
                    sw.WriteLine("");
                    return true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (sw != null)
                    {
                        sw.Close();
                    }

                    if (fs != null)
                    {
                        fs.Close();
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Writes the Exception to the Error Log File
        /// </summary>
        public static bool WriteToErrorLogFile_and_EventViewer(Exception sourceException)
        {
            if (Utils.LogEventViewer(sourceException)) { }
            if (Write_To_Log_File_temp_dir(sourceException)) { }

            if (IsDirectoryPresent(StripDirectoryName(errorLogFileName), true))
            {
                FileStream fs = null;
                StreamWriter sw = null;
                try
                {
                    fs = new FileStream(errorLogFileName, FileMode.Append, FileAccess.Write);
                    sw = new StreamWriter(fs);
                    sw.WriteLine("==================================================================");
                    sw.WriteLine("ERROR OCCOURED AT :" + DateTime.Now.ToString());
                    sw.WriteLine("SOURCE:" + sourceException.Source);
                    sw.WriteLine("MESSAGE:" + sourceException.Message);
                    sw.WriteLine("Whole Exception:" + sourceException.ToString());
                    sw.WriteLine("==================================================================");
                    sw.WriteLine("");
                    return true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (sw != null)
                    {
                        sw.Close();
                    }

                    if (fs != null)
                    {
                        fs.Close();
                    }
                }
            }
            return false;
        }

        public static bool Write_To_Log_File(Exception sourceException)
        {
            if (Utils.LogEventViewer(sourceException)) { }
            if (Write_To_Log_File_temp_dir(sourceException)) { }

            if (IsDirectoryPresent(StripDirectoryName(errorLogFileName), true))
            {
                FileStream fs = null;
                StreamWriter sw = null;
                try
                {
                    fs = new FileStream(errorLogFileName, FileMode.Append, FileAccess.Write);
                    sw = new StreamWriter(fs);
                    sw.WriteLine("==================================================================");
                    sw.WriteLine("ERROR OCCOURED AT :" + DateTime.Now.ToString());
                    sw.WriteLine("SOURCE:" + sourceException.Source);
                    sw.WriteLine("MESSAGE:" + sourceException.Message);
                    sw.WriteLine("Whole Exception:" + sourceException.ToString());
                    sw.WriteLine("==================================================================");
                    sw.WriteLine("");
                    return true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (sw != null)
                    {
                        sw.Close();
                    }

                    if (fs != null)
                    {
                        fs.Close();
                    }
                }
            }
            return false;
        }

        public static bool Write_To_Log_File_temp_dir(Exception sourceException)
        {
            string temp_path = Path.GetTempPath();
            string app_name = System.Configuration.ConfigurationManager.AppSettings["APP_NAME"];
            string log_file_name = app_name + ".log";
            var _temp_file = Path.Combine(temp_path, log_file_name);

            FileStream fs = null;
            StreamWriter sw = null;
            try
            {
                fs = new FileStream(_temp_file, FileMode.Append, FileAccess.Write);
                sw = new StreamWriter(fs);
                sw.WriteLine("==================================================================");
                sw.WriteLine("ERROR OCCOURED AT :" + DateTime.Now.ToString());
                sw.WriteLine("SOURCE:" + sourceException.Source);
                sw.WriteLine("MESSAGE:" + sourceException.Message);
                sw.WriteLine("Whole Exception:" + sourceException.ToString());
                sw.WriteLine("==================================================================");
                sw.WriteLine("");
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                }

                if (fs != null)
                {
                    fs.Close();
                }
            }
        }



    }
}
