This is a project demonstrating the Event Store capabilities


Prerequisites in docker Deskop
docker run -d -p 2113:2113 -p 1113:1113 --name eventstore-node  -e EVENTSTORE_ENABLE_ATOM_PUB_OVER_HTTP=true -e EVENTSTORE_INSECURE=true eventstore/eventstore
