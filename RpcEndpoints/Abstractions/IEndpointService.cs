namespace Cblx.Blocks.RpcEndpoints;

public interface IEndpointService
{
    Task RequestAsync(ActionEndpoint actionEndpoint);
    Task RequestAsync<TRequest>(ActionEndpoint<TRequest> actionEndpoint, TRequest request);
    Task<TResponse> RequestAsync<TResponse>(FuncEndpoint<TResponse> funcEndpoint);
    Task<TResponse> RequestAsync<TRequest, TResponse>(FuncEndpoint<TRequest, TResponse> funcEndpoint, TRequest request);

    IAsyncEnumerable<TResponseItem> RequestAsync<TResponseItem>(FuncAsyncEnumerableEndpoint<TResponseItem> funcEndpoint);
    IAsyncEnumerable<TResponseItem> RequestAsync<TRequest, TResponseItem>(FuncAsyncEnumerableEndpoint<TRequest, TResponseItem> funcEndpoint, TRequest request);
    IAsyncEnumerable<TResponseItem> MultipartFormDataRequestAsync<TResponseItem>(FuncAsyncEnumerableEndpoint<TResponseItem> funcEndpoint, Action<MultipartFormDataContent> configureContent);
    IAsyncEnumerable<TResponseItem> MultipartFormDataRequestAsync<TRequest, TResponseItem>(FuncAsyncEnumerableEndpoint<TRequest, TResponseItem> funcEndpoint, TRequest request, Action<MultipartFormDataContent> configureContent);

    /// <summary>
    /// EXPERIMENTAL
    /// <br/>
    /// No server, utilize IFormFile, IFormFileCollection ou IFormCollection para capturar os dados.
    /// </summary>
    Task MultipartFormDataRequestAsync(ActionEndpoint actionEndpoint, Action<MultipartFormDataContent> configureContent);
    /// <summary>
    /// EXPERIMENTAL
    /// <br/>
    /// No server, utilize formCollection["json"] para capturar o request e desserialize usando o JsonTypeInfo apropriado.
    /// <br/>
    /// Utilize formCollection.Files, IFormFile ou IFormFileCollection para capturar os arquivos enviados.
    /// </summary>
    Task MultipartFormDataRequestAsync<TRequest>(ActionEndpoint<TRequest> actionEndpoint, TRequest request, Action<MultipartFormDataContent> configureContent);
    /// <summary>
    /// EXPERIMENTAL
    /// <br/>
    /// No server, utilize IFormFile, IFormFileCollection ou IFormCollection para capturar os dados.
    /// </summary>
    Task<TResponse> MultipartFormDataRequestAsync<TResponse>(FuncEndpoint<TResponse> funcEndpoint, Action<MultipartFormDataContent> configureContent);
    /// <summary>
    /// EXPERIMENTAL
    /// <br/>
    /// No server, utilize formCollection["json"] para capturar o request e desserialize usando o JsonTypeInfo apropriado.
    /// <br/>
    /// Utilize formCollection.Files, IFormFile ou IFormFileCollection para capturar os arquivos enviados.
    /// </summary>
    Task<TResponse> MultipartFormDataRequestAsync<TRequest, TResponse>(FuncEndpoint<TRequest, TResponse> funcEndpoint, TRequest request, Action<MultipartFormDataContent> configureContent);


    event EventHandler<RequestSucceededEventArgs>? RequestSucceeded;
}
