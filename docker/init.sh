#!/bin/bash
cp ../schema/* shell-root/
cp ../datalog/* shell-root/
cp parameters shell-root/
export RDFOX_ROLE=admin
export RDFOX_PASSWORD=password
docker run --rm -v ${PWD}/RDFox.lic:/home/rdfox/.RDFox/RDFox.lic \
           -v rdfox-server-directory:/home/rdfox/.RDFox \
           -v ${PWD}/shell-root:/data \
           -e RDFOX_ROLE \
           -e RDFOX_PASSWORD \
           oxfordsemantic/rdfox-init 