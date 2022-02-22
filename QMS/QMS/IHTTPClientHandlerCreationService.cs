using System.Net.Http;

namespace QMS
{
    public interface IHTTPClientHandlerCreationService
    {
        HttpClientHandler GetInsecureHandler();
    }
}
