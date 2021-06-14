// Client side C/C++ program to demonstrate Socket programming 
#include <stdio.h> 
#include <sys/socket.h> 
#include <stdlib.h> 
#include <netinet/in.h>
#include <arpa/inet.h>
#include <unistd.h>
#include <string.h> 
#include <iostream>
#define PORT 2112 
   
int main(int argc, char const *argv[]) 
{ 
    struct sockaddr_in address; 
    int sock = 0, valread; 
    struct sockaddr_in serv_addr; 
    std::string edit = "{\"type\": \"edit\",\"cell\": \"A3\",\"value\": \"=2*A1+1+B3\",\"dependencies\": [\"A1\",\"B3\"]}\n\n";
    std::string edit_str = "{\"type\": \"edit\",\"cell\": \"A2\",\"value\": \"testing123\",\"dependencies\": []}\n\n";
    std::string undo = "{\"type\": \"undo\"}\n\n";
    std::string revert = "{\"type\": \"revert\",\"cell\": \"A2\"}\n\n";
    char buffer[1024] = {0}; 
    if ((sock = socket(AF_INET, SOCK_STREAM, 0)) < 0) 
    { 
        printf("\n Socket creation error \n"); 
        return -1; 
    } 
   
    memset(&serv_addr, '0', sizeof(serv_addr)); 
   
    serv_addr.sin_family = AF_INET; 
    serv_addr.sin_port = htons(PORT); 
       
    // Convert IPv4 and IPv6 addresses from text to binary form 
    if(inet_pton(AF_INET, "127.0.0.1", &serv_addr.sin_addr)<=0)  
    { 
        printf("\nInvalid address/ Address not supported \n"); 
        return -1; 
    } 
   
    if (connect(sock, (struct sockaddr *)&serv_addr, sizeof(serv_addr)) < 0) 
    { 
        printf("\nConnection Failed \n"); 
        return -1; 
    } 
    //send(sock , hello , strlen(hello) , 0 ); 
    std::string whoAmI = "{\"type\": \"open\",\"name\": \"test.sprd\",\"username\": \"testMan\",\"password\": \"dufus\"}\n\n";
    //printf("Hello message sent\n"); 
    while (1)
    {
        valread = read( sock , buffer, 1024); 
        printf("%s\n",buffer ); 
        if (valread != 0)
        {
            break;
        }

    }
    std::cout << whoAmI << std::endl;
    
    
    char bufferdos[1024] = {0};
    send(sock, whoAmI.c_str(), strlen(whoAmI.c_str()), 0 );
    while (1)
    {
        valread = read( sock , bufferdos, 1024); 
        printf("TESTING%s\n",bufferdos ); 
        if (valread != 0)
        {
            break;
        }
    }

    send(sock, edit_str.c_str(), strlen(edit_str.c_str()), 0 );

    char bufferthree[1024] = {0};
    while (1)
    {
        valread = read( sock , bufferthree, 1024); 
        printf("TESTING2%s\n",bufferthree ); 
        if (valread != 0)
        {
            break;
        }
    }
    std::cin.ignore();
    return 0; 
} 
