using Visual.Studio.Solution.Renamer.Library.Entity.Project;

namespace Visual.Studio.Solution.Renamer.Library.Extension
{
    public static class ProjectInSolutionExtensions
    {
        public static bool IsCsProj(this ProjectInSolution value)
        {
            return value != null &&
                   value.RelativePath.EndsWith(ProjectConstants.ProjectExtension, ignoreCase: true, culture: null);
        }
    }
}