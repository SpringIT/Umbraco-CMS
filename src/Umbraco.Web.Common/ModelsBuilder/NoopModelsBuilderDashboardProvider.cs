namespace Umbraco.Web.Common.ModelsBuilder
{
    public class NoopModelsBuilderDashboardProvider: IModelsBuilderDashboardProvider
    {
        public string GetUrl() => string.Empty;
    }
}
