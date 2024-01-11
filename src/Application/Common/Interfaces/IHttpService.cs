namespace Application.Common.Interfaces;

public interface IHttpService_2
{
    Task<T> GetRestServiceDataAsync<T>(string serviceAddress);

    Task<T> PostRestServiceDataAsync<T>(string serializedData,
        string serviceAddress,
        string parameters,
        string auth,
        string authorizationType,
        string idTransaccion);
}