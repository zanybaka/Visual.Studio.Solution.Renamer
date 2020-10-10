using System.Linq;
using System.Text;
using Microsoft.Build.Definition;
using Microsoft.Build.Evaluation;

namespace Visual.Studio.Solution.Renamer.Library.Task
{
    internal class SyncXmlSingleNodeHelper
    {
        public bool AddOrUpdate(string path, string childNodeName,
                                string newValue, bool preview)
        {
            Project csproj = Project.FromFile(
                path,
                new ProjectOptions
                {
                    LoadSettings = ProjectLoadSettings.IgnoreMissingImports
                });
            try
            {
                ProjectProperty property = csproj.Properties.FirstOrDefault(x => x.Name == childNodeName);
                if (property?.UnevaluatedValue == newValue)
                {
                    return false;
                }

                csproj.SetProperty(childNodeName, newValue);
                if (!preview)
                {
                    csproj.Save(Encoding.Default);
                }

                return true;
            }
            finally
            {
                ProjectCollection.GlobalProjectCollection.UnloadProject(csproj);
            }
        }
    }
}