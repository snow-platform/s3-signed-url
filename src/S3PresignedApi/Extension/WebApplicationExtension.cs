using System.Reflection;
using S3PresignedApi.Common.Endpoint;

namespace S3PresignedApi.Extension;

public static class WebApplicationExtension
{
    extension(WebApplication app)
    {
        public void MapMinimalEndpoint()
        {
            var endpoints = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(x => typeof(IMinimalEndpoint).IsAssignableFrom(x) && x.IsInterface is false);

            foreach (var endpoint in endpoints)
            {
                var method = endpoint.GetMethod(nameof(IMinimalEndpoint.Endpoint), BindingFlags.Public | BindingFlags.Static);

                method?.Invoke(null, [app]);
            }
        }
    }
}