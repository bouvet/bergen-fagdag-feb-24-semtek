using System.Text.Json;

namespace Elnet.Services.Interfaces;

public interface ISparqlQueryHelper
{
    public Task<string> readquery(string query_name);
    public IAsyncEnumerable<JsonElement> GetSparqlResult(string queryString);

}