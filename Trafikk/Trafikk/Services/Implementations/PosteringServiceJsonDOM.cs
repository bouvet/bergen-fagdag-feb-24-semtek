using System.Text.Json;
using Rekneskap.Models;
using Trafikk.Services.Interfaces;
using VDS.RDF.Parsing;

namespace Rekneskap.Services;

public class PosteringServiceJsonDOM
{
    private IHttpClientFactory _clientFactory;
    private ISparqlQueryHelper _sparqlQueryHelper;
    public PosteringServiceJsonDOM(IHttpClientFactory clientFactory, ISparqlQueryHelper sparqlQueryHelper)
    {
        _clientFactory = clientFactory;
        _sparqlQueryHelper = sparqlQueryHelper;
    }

    /// <summary>
    /// Parses a binding from an SPARQL Json query answer
    /// </summary>
    /// <param name="result"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    protected string parseSparqlResultBinding(JsonElement binding, Func<IEnumerable<string?>, string> formatter)
    {
        var strings = binding.EnumerateObject()
            .Select(el => el.Value.GetProperty("value").GetString());
        return formatter(strings);
    }

    public async Task<RdfObject> getObject(Uri objectUri)
    {
        var queryString = SparqlQueryHelper.getObjectQuery(objectUri);
        
        await foreach(var binding in GetSparqlResult(queryString))
            return SparqlQueryHelper.getObjectQueryBinding(binding, objectUri);
        throw new Exception("No elements returned about ${objectUri}");
    }
    public async IAsyncEnumerable<JsonElement> getObjectProperties(Uri objectUri)
    {
        var queryString = SparqlQueryHelper.getObjectPropertiesQuery(objectUri);

        await foreach (var binding in GetSparqlResult(queryString))
            yield return binding;
    }
    public async IAsyncEnumerable<JsonElement> getPosteringer()
    {
        var queryString = await _sparqlQueryHelper.readquery();

        await foreach (var postering in GetSparqlResult(queryString))
            yield return postering;
    }

    public async IAsyncEnumerable<JsonElement> GetSparqlResult(string queryString)
    {
        var client = _clientFactory.CreateClient("rdfox");

        SparqlQueryParser parser = new SparqlQueryParser();
        var sparqlQuery = parser.ParseFromString(queryString);

        var request = SparqlQueryHelper.createrequest(sparqlQuery.ToString());
        var response = await client.SendAsync(request);
        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            JsonDocument sparqlResult = JsonDocument.Parse(responseContent);

            JsonElement results = sparqlResult.RootElement.GetProperty("results");
            JsonElement bindings = results.GetProperty("bindings");

            foreach (JsonElement binding in bindings.EnumerateArray())
            {
                yield return binding;
            }
        }
        else
        {
            var responseText = await response.Content.ReadAsStringAsync();
            throw new Exception($"Server error Status: {response.StatusCode.ToString()} Content: {responseText}");
        }

    }
}