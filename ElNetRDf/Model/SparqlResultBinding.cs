namespace Rekneskap.Models;

public class SparqlResultBinding
{
    public string? value_label { get; set; }
    public string? property { get; set; }
    public string? property_label { get; set; }
    public string? value { get; set; }
    public string? value_type { get; set; }
}

public class RdfObject
{
    // iri
    public Uri? iri { get; set; }
    // label of type
    public string? type_label { get; set; }
    // iri of type
    public Uri? type_iri { get; set; }
    // rdfs:label
    public string? label { get; set; }
    // rdfs:comment
    public string? comment { get; set; }
}