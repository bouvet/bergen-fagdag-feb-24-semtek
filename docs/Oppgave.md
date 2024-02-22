# Oppgaver
Antar at du har kjørende RDFox

Spørringen for å hente ut objekter av type ex:type er
```sparql
SELECT * WHERE {
    ?element a ex:type.
}
```
Nettverk har typen `elnet:ElectricalNetwork`
* Lag en spørring som henter ut alle nettverk i [denne sparql-fila](../ElNetRDf/wwwroot/queries/all_networks.sparql)

    Kjør spørringa ved å skrive 
    `evaluate ../ElNetRDf/wwwroot/queries/all_networks.sparql`
    og etterpå ved å lime den inn i konsollen


* Utvid spørringa til å ta med alle properties som er definert.


* Lag en spørring som henter ut alle rot-nettverk i [denne sparql-fila](../ElNetRDf/wwwroot/queries/root_networks.sparql)
( Filter lages ved å skrive `FILTER NOT EXISTS (<filter som ikke skal fines>) )

* Endre [import.dlog][../data/import.dlog] til å også hente ut rdfs:label på anlegg. Endre de to spørringene over til å hente ut anlegg.

* Endre [root-network.dlog](../data/root-network.dlog) til å regne ut både rot-nettverk som ikke har noe parent, og "LeafNetwork", som ikke er parent til noen. Lag sparql-spørringer som henter ut begge deler


* Hent ut en liste av alle "barne-nettverk" i spørringa [denne sparql-fila](../ElNetRDf/wwwroot/queries/parentpaths.sparql)
Kjør den både med evaluate og i konsollen

* Endre [network-tree.dlog](../data/network-tree.dlog) til å regne ut både rot-nettverk som ikke har noe parent, og "LeafNetwork", som ikke er parent til noen. Lag sparql-spørringer som henter ut begge deler


* Endre [transitive_parent.dlog](../data/transitive_parent.dlog) til å regne ut relasjonen mellom et nettverk og et som befinner seg etter en eller flere parent relasjoner.
* * Målet er at [transitive_parent.sparql](../ElNetRDf/wwwroot/queries/transitive_parent.sparql) skal gi svar

* Endre [ancestor-relation.dlog](../data/ancestor-relation.dlog) til å regne ut listen av barnenoder. 


# Blazor

Blazor installasjonen bruker spørringene [root_networks.sparql](../ElNetRDf/wwwroot/queries/root_networks.sparql) og [network.sparql](../ElNetRDf/wwwroot/queries/network.sparql). Når du er fornøyd med dem (Eller kopierer dem inn fra [../queries-forslag](../queries-forslag/) ) så kan du starte appen med `dotnet run`



