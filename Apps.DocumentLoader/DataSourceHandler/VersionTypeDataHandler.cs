using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.DocumentLoader.DataSourceHandler;

public class VersionTypeDataHandler : BaseInvocable, IDataSourceHandler
{
    private IEnumerable<AuthenticationCredentialsProvider> Creds =>
        InvocationContext.AuthenticationCredentialsProviders;

    public VersionTypeDataHandler(InvocationContext invocationContext) : base(invocationContext)
    {
    }

    public Dictionary<string, string> GetData(DataSourceContext context)
    {
        return new Dictionary<string, string>()
        {
            {"major", "Major" },
            {"minor", "Minor" },
            {"patch", "Patch" }
        }.Where(x => string.IsNullOrWhiteSpace(context.SearchString) || x.Key.Contains(context.SearchString)).ToDictionary(x => x.Key, x => x.Value);
    }
}