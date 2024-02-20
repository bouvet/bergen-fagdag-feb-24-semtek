using System.Text.Json;

namespace Elnet.Services.Interfaces;

public interface ISparqlQueryHelper
{
    public Task<string> ReadQuery(string queryName);
    public IAsyncEnumerable<JsonElement> GetSparqlResult(string queryString);

}