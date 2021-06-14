all: client node.o string_set.o dependency_graph.o server manage cellmanipulator.o 

client: client.cpp
	g++ client.cpp -o client

manage: manage.cpp
	g++ manage.cpp -o manage

server: server.cpp
	g++ server.cpp -o server -lpthread node.o string_set.o dependency_graph.o

node.o: node.cpp node.h
	g++ -c node.cpp

cellmanipulator.o: node.h string_set.h cellmanipulator.cpp 
	g++ -c cellmanipulator.cpp string_set.o node.o

string_set.o: string_set.h node.h string_set.cpp
	g++ -c string_set.cpp

dependency_graph.o: dependency_graph.cpp dependency_graph.h
	g++ -c dependency_graph.cpp

clean:
	rm -f server cellmanipulator.o node.o string_set.o dependency_graph.o