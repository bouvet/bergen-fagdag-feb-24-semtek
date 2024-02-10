#!/bin/bash
export RDFOX_ROLE=admin
export RDFOX_PASSWORD=password


cp ../schema/* shell-root/
cp ../datalog/* shell-root/
cp ../rdfox/* shell-root/
cp ../data/* shell-root/
cp ../queries/* shell-root/

case $1 in

    "daemon-persisted" )
docker run -d --cap-drop ALL -p 12110:12110 \
           -v ${PWD}/RDFox.lic:/opt/RDFox/RDFox.lic \
           -v rdfox-server-directory:/home/rdfox/.RDFox \
           oxfordsemantic/rdfox:7.0
;;
    "interactive-persisted" )
docker run -it --cap-drop ALL -p 12110:12110 \
           -v ${PWD}/RDFox.lic:/opt/RDFox/RDFox.lic \
           -v rdfox-server-directory:/home/rdfox/.RDFox \
           -v ${PWD}/shell-root:/data \
           -e RDFOX_ROLE \
           -e RDFOX_PASSWORD \
           oxfordsemantic/rdfox:7.0 shell
;;
    "in-memory-daemon" )


docker run -d --cap-drop ALL \
           -p 12110:12110 \
           -v ${PWD}/RDFox.lic:/opt/RDFox/RDFox.lic \
           -v ${PWD}/shell-root:/data \
           -e RDFOX_ROLE \
           -e RDFOX_PASSWORD \
           oxfordsemantic/rdfox:7.0 -persistence off -access-control-allow-origin "*" shell . start
;;
"in-memory-interactive" )


docker run -it --cap-drop ALL \
           -p 12110:12110 \
           -v ${PWD}/RDFox.lic:/opt/RDFox/RDFox.lic \
           -v ${PWD}/shell-root:/data \
           -e RDFOX_ROLE \
           -e RDFOX_PASSWORD \
           oxfordsemantic/rdfox:7.0 -persistence off -access-control-allow-origin "*" shell 
;;
    * )
echo "Usage $0 daemon|interactive|in-memory"
;;
esac