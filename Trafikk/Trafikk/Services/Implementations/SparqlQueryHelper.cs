using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Text.Json;
using Rekneskap.Models;
using Trafikk.Services.Interfaces;
using VDS.RDF.Query;

namespace Rekneskap.Services;

public class SparqlQueryHelper : ISparqlQueryHelper
{
    private IHttpClientFactory _httpClientFactory;
    public SparqlQueryHelper(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    public async Task<string> readquery()
    {
        var client = _httpClientFactory.CreateClient("self");
        var byteOfTheFile = await client.GetByteArrayAsync("queries/posteringer.q");
        
        return System.Text.Encoding.UTF8.GetString(byteOfTheFile);
    }
    public static string getObjectQuery(Uri objectUri)
    {
        SparqlParameterizedString q = new SparqlParameterizedString();
        q.CommandText = """
                        PREFIX rek: <https://github.com/daghovland/rdf-rekneskap#>
                        PREFIX rdfs: <http://www.w3.org/2000/01/rdf-schema#>
                        SELECT * WHERE {
                            @objectUri a ?object_type;
                                        rdfs:label ?object_label.
                            OPTIONAL { @objectUri  rdfs:comment ?object_comment. }
                            OPTIONAL { ?object_type rdfs:label ?type_label } .
                        }
                        """;
        q.SetUri("objectUri", objectUri);

        return q.ToString();
    }
    // Gets the pair of property and value of a sparql-result binding
    public static RdfObject getObjectQueryBinding(JsonElement binding, Uri objectIri)
        => new(){
            iri = objectIri,
            label = getObjectQueryBidningValue(binding, "object_label"),
            type_label = getObjectQueryBidningValue(binding, "type_label"),
            type_iri = new Uri(getObjectQueryBidningValue(binding, "object_type")),
            comment = getObjectQueryBidningValue(binding, "object_comment")
        };

    public static string getObjectPropertiesQuery(Uri objectUri)
    {
        SparqlParameterizedString q = new SparqlParameterizedString();
        q.CommandText = """
                        PREFIX rek: <https://github.com/daghovland/rdf-rekneskap#>
                        PREFIX rdfs: <http://www.w3.org/2000/01/rdf-schema#>
                        PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#> 
                        SELECT * WHERE {
                            @objectUri ?prop ?val. 
                            FILTER(?prop NOT IN (rdf:type, rdfs:label, rdfs:comment)).
                            OPTIONAL{@objectUri rdfs:label ?object_label.}. 
                            OPTIONAL {?prop rdfs:label ?prop_label} .  
                            OPTIONAL {?val a ?val_type}.
                            OPTIONAL {?val rdfs:label ?val_label}.
                        }
                        """;
        q.SetUri("objectUri", objectUri);

        return q.ToString();
    }
    // Gets the pair of property and value of a sparql-result binding
    public static SparqlResultBinding getObjectPropertyQueryBinding(JsonElement binding)
        => new(){
            property = getObjectQueryBidningValue(binding, "prop"),
            property_label = getObjectQueryBidningValue(binding, "prop_label"),
            value = getObjectQueryBidningValue(binding, "val"),
            value_type = getObjectQueryBidningValue(binding, "val_type"),
            value_label = getObjectQueryBidningValue(binding, "val_label")
        };


    // Gets the content / value of a bidingin in sparql-result
    private static string getObjectQueryBidningValue(JsonElement binding, string key)
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