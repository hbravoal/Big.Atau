using System.Linq;

namespace Itau.Server.Business.BatchProcess
{
    internal class TaskInBatch
    {
        private string _provider;

        public string Provider
        {
            set
            {
                _provider = value;
                if (string.IsNullOrEmpty(_provider) || !_provider.Contains(',')) return;
                var providerSplit = _provider.Split(',');

                if (providerSplit.Length != 2) return;
                AssemblyTypeName = providerSplit[1].Trim();
                TypeName = providerSplit[0].Trim();
            }
            get => _provider;
        }

        public string File { get; set; }

        public bool IsLocal
        {
            get;
            set;
        }

        public string FolderSftp { get; set; }
        public string AssemblyTypeName { get; set; }
        public string TypeName { get; set; }
        public string SftpCredentials { get; set; }
        public string PathRootSftp { get; set; }
        public string FtPmode { get; set; }
        public string FileNameFilter { get; set; }
        public string ExtensionFilter { get; set; }
        public string IsSynchronous { get; set; }

        public string PathCopyFileSftp { get; set; }
    }
}