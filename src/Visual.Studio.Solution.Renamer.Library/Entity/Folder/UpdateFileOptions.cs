using System.Collections.Generic;
using System.Linq;
using Visual.Studio.Solution.Renamer.Library.Builder;

namespace Visual.Studio.Solution.Renamer.Library.Entity.Folder
{
    public class UpdateFileOptions : UpdateOptions
    {
        private UpdateFileOptions(string directory) : base(directory)
        {
        }

        internal bool Recursively { get; private set; }

        internal string From { get; private set; }

        internal string To { get; private set; }

        internal string[] Masks { get; private set; }

        public static UpdateFileOptions Create(string directory)
        {
            return new UpdateFileOptions(directory);
        }

        public UpdateFileOptions Clone()
        {
            return (UpdateFileOptions) MemberwiseClone();
        }

        public UpdateFileOptions WithFrom(string from)
        {
            From = from;
            return this;
        }

        public UpdateFileOptions WithTo(string to)
        {
            To = to;
            return this;
        }

        public UpdateFileOptions WithMasks(IEnumerable<string> masks)
        {
            Masks = masks.ToArray();
            return this;
        }

        public UpdateFileOptions WithSubdirectories()
        {
            Recursively = true;
            return this;
        }
    }
}