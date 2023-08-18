using Blackbird.Applications.Sdk.Common;

namespace Apps.DocumentLoader;

public class DocumentLoaderApplication : IApplication
{
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