using Itau.Common.Diagnostics;
using System;
using System.IO;
using System.Threading;
using System.Xml.Linq;

namespace Itau.Server.Business.BatchProcess
{
    public static class Batch
    {
        public static void Start()
        {
            try
            {
                var xmlTasks = XDocument.Load($"{AppDomain.CurrentDomain.BaseDirectory}{"Tasks.xml"}");
                var tasks = xmlTasks.Descendants("TaskInBatch");

                foreach (var task in tasks)
                {
                    var t = new TaskInBatch();

                    t.IsLocal = task.Attribute("IsLocal") != null && Boolean.Parse(task.Attribute("IsLocal")?.Value ?? string.Empty);

                    t.FolderSftp = task.Attribute("FolderSftp")?.Value;
                    t.Provider = task.Attribute("Provider")?.Value;

                    t.SftpCredentials = task.Attribute("SFTPCredentialsSettingsKey") != null ? task.Attribute("SFTPCredentialsSettingsKey")?.Value : "";
                    t.PathRootSftp = task.Attribute("PathRootSftp") != null ? task.Attribute("PathRootSftp")?.Value : "";
                    t.FileNameFilter = task.Attribute("FileNameFilter") != null ? task.Attribute("FileNameFilter")?.Value : "";
                    t.ExtensionFilter = task.Attribute("ExtensionFilter") != null ? task.Attribute("ExtensionFilter")?.Value : "";
                    t.IsSynchronous = task.Attribute("isSynchronous") != null ? task.Attribute("isSynchronous")?.Value : "";
                    t.PathCopyFileSftp = task.Attribute("PathCopyFileSFTP") != null ? task.Attribute("PathCopyFileSFTP")?.Value : "";

                    try
                    {
                    
                        if (t.IsLocal)
                        {
                            if (t.IsSynchronous != null && t.IsSynchronous.Equals("true"))
                            {
                                Activator.CreateInstance(t.AssemblyTypeName, t.TypeName);
                            }
                            else
                            {
                                var thread = new Thread(() => Activator.CreateInstance(t.AssemblyTypeName, t.TypeName));
                                thread.Start();
                            }
                        }
                    }
                    catch (FileNotFoundException ex)
                    {
                        ExceptionLogging.LogException(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
            }
        }
    }
}