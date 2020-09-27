using System.Collections.Generic;
using System.Linq;
using Visual.Studio.Solution.Renamer.Library.Builder;

namespace Visual.Studio.Solution.Renamer.Library.Entity.Folder
{
    public class UpdateFolderOptions : UpdateOptions
    {
        private UpdateFolderOptions(string directory) : base(directory)
        {
        }

        internal bool Recursively { get; private set; }

        internal string[] Masks { get; private set; }

        internal string From { get; private set; }

        internal string To { get; private set; }

        public static UpdateFolderOptions Create(string directory)
        {
            return new UpdateFolderOptions(directory);
        }

        public UpdateFolderOptions WithSubdirectories()
        {
            Recursively = true;
            return this;
        }

        public UpdateFolderOptions WithMasks(IEnumerable<string> masks)
        {
            Masks = masks.ToArray();
            return this;
        }

        public UpdateFolderOptions WithFrom(string from)
        {
            From = from;
            return this;
        }

        public UpdateFolderOptions WithTo(string to)
        {
            To = to;
            return this;
        }

        public UpdateFolderOptions Clone()
        {
            return (UpdateFolderOptions) MemberwiseClone();
        }
    }
}