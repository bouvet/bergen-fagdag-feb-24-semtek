INSERT { 
    ?s a <https://rdfox.com/vocabulary#ConstraintViolation> . 
    ?s ?p ?o 
} WHERE { 
    TT SHACL_DN { schema:shacl ?s ?p ?o} . 
    FILTER(?p IN (sh:sourceShape, sh:resultMessage, sh:value)) 
}