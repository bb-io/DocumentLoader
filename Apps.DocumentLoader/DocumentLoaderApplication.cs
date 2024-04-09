using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Metadata;

namespace Apps.DocumentLoader;

public class DocumentLoaderApplication : IApplication, ICategoryProvider
{
    public IEnumerable<ApplicationCategory> Categories
    {
        get => [ApplicationCategory.Utilities];
        set { }
    }
    
    public string Name
    {
        get => "Document Loader";
        set { }
    }

    public T GetInstance<T>()
    {
        throw new NotImplementedException();
    }
}