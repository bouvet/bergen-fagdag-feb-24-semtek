using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Elnet.Services.Interfaces;
using Rekneskap.Models;
using VDS.RDF.Parsing;
using VDS.RDF.Query;

namespace Elnet.Services;

public class SparqlQueryHelper : ISparqlQueryHelper
{
    private IHttpClientFactory _httpClientFactory;
    public SparqlQueryHelper(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    public async Task<string> readquery(string query_name)
    {
        var client = _httpClientFactory.CreateClient("self");
        var byteOfTheFile = await client.GetByteArrayAsync($"queries/{query_name}.sparql");
        
        return System.Text.Encoding.UTF8.GetString(byteOfTheFile);
    }
    
    /**
     * Sends sparql query to RDFox and returns a list of json elements, one for each answer
     */
    public async IAsyncEnumerable<JsonElement> GetSparqlResult(string queryString)
    {
        var client = _httpClientFactory.CreateClient("rdfox");

        SparqlQueryParser parser = new SparqlQueryParser();
        var sparqlQuery = parser.ParseFromString(queryString);

        var request = SparqlQueryHelper.createrequest(sparqlQuery.ToString());
        var response = await client.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            var resContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"response returned: {resContent}");
        }
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

    // Gets the content / value of a bidingin in sparql-result
    public static string getObjectQueryBidningValue(JsonElement binding, string key)
    {
        if (binding.TryGetProperty(key, out JsonElement keyElement)
            && keyElement.TryGetProperty("value", out JsonElement keyValue))
            return keyValue.GetString() ?? throw new InvalidOperationException();
        return "";
    }

    public static string getSparqlUrl => "datastores/rekneskap/sparql";

    public static AuthenticationHeaderValue getAuth =>
        new AuthenticationHeaderValue(
            "Basic",
            Convert.ToBase64String(Encoding.ASCII.GetBytes($"admin:password")));
    public static HttpRequestMessage createrequest(string querystring)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, getSparqlUrl);
        request.Headers.Add("Accept", "application/sparql-results+json");
        
        request.Headers.Authorization = getAuth;
        request.Content = new StringContent(querystring, new System.Net.Http.Headers.MediaTypeHeaderValue("application/sparql-query"));
        return request;
    }
}
