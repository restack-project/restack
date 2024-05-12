using ReStack.Common.Constants;
using ReStack.Common.Exceptions;
using ReStack.Common.Models;
using System.Text;
using System.Text.Json;

namespace ReStack.Common.HttpClients.Base;

public class BaseClient<T>(
    HttpClient _client
    , string _endpoint
)
{
    public HttpClient Client { get; } = _client;

    public virtual Task<T> Get(int id, CancellationToken cancellationToken = default)
        => MakeRequest<T>(HttpMethod.Get, _endpoint.AddRouteParameter(id), cancellationToken: cancellationToken);

    public virtual Task<List<T>> GetAll(CancellationToken cancellationToken = default)
        => MakeRequest<List<T>>(HttpMethod.Get, _endpoint, cancellationToken: cancellationToken);

    public virtual Task<T> Add(T model)
        => MakeRequest<T>(HttpMethod.Post, _endpoint, model);

    public virtual Task<T> Update(T model)
        => MakeRequest<T>(HttpMethod.Put, _endpoint, model);

    public virtual Task<T> Delete(int id)
        => MakeRequest<T>(HttpMethod.Delete, _endpoint.AddRouteParameter(id));

    protected async Task<TE> MakeRequest<TE>(
        HttpMethod method
        , string endpoint
        , object body = null
        , string mediaType = "application/json"
        , bool authorize = true
        , CancellationToken cancellationToken = default
    )
    {
        var httpRequestMessage = new HttpRequestMessage(method, endpoint);

        //if (authorize)
        //{
        //    var token = await _tokenProvider.Get(refreshIfExpired: true);

        //    httpRequestMessage.Headers.Add("Authorization", $"Bearer {token.AccessToken}");
        //}

        if (body is not null)
        {
            if (mediaType == "application/json")
            {
                httpRequestMessage.Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, mediaType);
            }
            else
            {
                httpRequestMessage.Content = new StringContent(body.ToString(), Encoding.UTF8, mediaType);
            }
        }

        var request = await Client.SendAsync(httpRequestMessage, cancellationToken);
        var content = await request.Content.ReadAsStringAsync(cancellationToken);

        EnsureSucceeded(endpoint, request, content);

        if (typeof(TE) == typeof(string))
        {
            return (dynamic)content;
        }
        else
        {
            return Map<TE>(content);
        }
    }

    private static TE Map<TE>(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            return default;
        }
        else
        {
            return JsonSerializer.Deserialize<TE>(content, GetOptions());
        }
    }

    private static void EnsureSucceeded(string endpoint, HttpResponseMessage request, string content)
    {
        if (request.StatusCode == System.Net.HttpStatusCode.BadRequest)
        {
            var badRequest = JsonSerializer.Deserialize<ModelValidationBadRequest>(content, GetOptions());
            var exception = new ModelValidationException(badRequest.Errors);

            throw exception;
        }

        if (!request.IsSuccessStatusCode)
        {
            try
            {
                var error = JsonSerializer.Deserialize<ErrorModel>(content, GetOptions());

                throw new ApiException(error.Type);
            }
            catch (Exception ex) when (ex is not ApiException)
            {
                throw new Exception($"Error while requesting '{endpoint}' - {request.StatusCode}:{content}");
            }
        }
    }

    private static JsonSerializerOptions GetOptions()
    {
        return new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
    }
}
