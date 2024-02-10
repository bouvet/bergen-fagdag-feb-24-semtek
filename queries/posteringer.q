BASE <https://github.com/daghovland/rdf-rekneskap#> 
PREFIX rek: <https://github.com/daghovland/rdf-rekneskap#> 
PREFIX rdfs: <http://www.w3.org/2000/01/rdf-schema#>
SELECT * WHERE {
    ?post a rek:Postering;
        rdfs:label ?post_label;
        rek:fraKonto ?fra;
        rdfs:label ?fra_label;
        rek:tilKonto ?til;
        rdfs:label ?til_label;
        rek:postertDato ?dato;
        rek:beløp ?belop.
}