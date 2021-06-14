/*
 * friendlist.c - [Starting code for] a web-based friend-graph manager.
 *
 * Based on:
 *  tiny.c - A simple, iterative HTTP/1.0 Web server that uses the 
 *      GET method to serve static and dynamic content.
 *   Tiny Web server
 *   Dave O'Hallaron
 *   Carnegie Mellon University
 *
 * Author: Andrew Thompson
 * Date: 12/4/19
 * Should be passing simple.rkt with --introduce and --multi, as well as trouble.rkt.
 */
#include "csapp.h"
#include "dictionary.h"
#include "more_string.h"

static void* doit(void* connfd_p);
static dictionary_t *read_requesthdrs(rio_t *rp);
static void read_postquery(rio_t *rp, dictionary_t *headers, dictionary_t *d);
static void clienterror(int fd, char *cause, char *errnum, 
                        char *shortmsg, char *longmsg);
static void print_stringdictionary(dictionary_t *d);
static void serve_request(int fd, dictionary_t *query);

static void serve_friends(int fd, dictionary_t *query);
static void serve_befriend(int fd, dictionary_t *query);
static void serve_unfriend(int fd, dictionary_t *query);
static void serve_introduce(int fd, dictionary_t *query, char* method);

dictionary_t *user_dict;

int main(int argc, char **argv) {
  int listenfd, connfd;
  char hostname[MAXLINE], port[MAXLINE];
  socklen_t clientlen;
  struct sockaddr_storage clientaddr;
  user_dict = make_dictionary(COMPARE_CASE_SENS, (free_proc_t)free_dictionary);
  pthread_t th;

  /* Check command line args */
  if (argc != 2) {
    fprintf(stderr, "usage: %s <port>\n", argv[0]);
    exit(1);
  }

  listenfd = Open_listenfd(argv[1]);

  /* Don't kill the server if there's an error, because
     we want to survive errors due to a client. But we
     do want to report errors. */
  exit_on_error(0);

  /* Also, don't stop on broken connections: */
  Signal(SIGPIPE, SIG_IGN);

  while (1) {
    clientlen = sizeof(clientaddr);
    connfd = Accept(listenfd, (SA *)&clientaddr, &clientlen);
    if (connfd >= 0) {
      Getnameinfo((SA *) &clientaddr, clientlen, hostname, MAXLINE, 
                  port, MAXLINE, 0);
      printf("Accepted connection from (%s, %s)\n", hostname, port);

      int* connfd_p = malloc(sizeof(int));
      *connfd_p = connfd;
      Pthread_create(&th, NULL, doit, connfd_p);
      Pthread_detach(th);

      // doit(connfd);
      //Close(connfd);
    }
  }
  free_dictionary(user_dict);
}

/*
 * doit - handle one HTTP request/response transaction
 */
void* doit(void* connfd_p) {
  int fd = *(int*)connfd_p;
  char buf[MAXLINE], *method, *uri, *version;
  rio_t rio;
  dictionary_t *headers, *query;

  free(connfd_p);

  /* Read request line and headers */
  Rio_readinitb(&rio, fd);
  if (Rio_readlineb(&rio, buf, MAXLINE) <= 0){
    Close(fd);
    return NULL;
  }
  printf("%s", buf);
  
  if (!parse_request_line(buf, &method, &uri, &version)) {
    clienterror(fd, method, "400", "Bad Request",
                "Friendlist did not recognize the request");
  } else {
    if (strcasecmp(version, "HTTP/1.0")
        && strcasecmp(version, "HTTP/1.1")) {
      clienterror(fd, version, "501", "Not Implemented",
                  "Friendlist does not implement that version");
    } else if (strcasecmp(method, "GET")
               && strcasecmp(method, "POST")) {
      clienterror(fd, method, "501", "Not Implemented",
                  "Friendlist does not implement that method");
    } else {
      headers = read_requesthdrs(&rio);

      /* Parse all query arguments into a dictionary */
      query = make_dictionary(COMPARE_CASE_SENS, free);
      parse_uriquery(uri, query);
      if (!strcasecmp(method, "POST"))
        read_postquery(&rio, headers, query);

      /* For debugging, print the dictionary */
      print_stringdictionary(query);

      /* You'll want to handle different queries here,
         but the intial implementation always returns
         nothing: */
      if (starts_with("/friends", uri))
	serve_friends(fd, query);
      else if (starts_with("/befriend", uri))
	serve_befriend(fd, query);
      else if (starts_with("/unfriend", uri))
	serve_unfriend(fd, query);
      else if (starts_with("/introduce", uri))
	serve_introduce(fd, query, method);
      else 
	serve_request(fd, query);

      /* Clean up */
      free_dictionary(query);
      free_dictionary(headers);
    }

    /* Clean up status line */
    free(method);
    free(uri);
    free(version);
  }

  Close(fd);
  return NULL;
}

/*
 * read_requesthdrs - read HTTP request headers
 */
dictionary_t *read_requesthdrs(rio_t *rp) {
  char buf[MAXLINE];
  dictionary_t *d = make_dictionary(COMPARE_CASE_INSENS, free);

  Rio_readlineb(rp, buf, MAXLINE);
  printf("%s", buf);
  while(strcmp(buf, "\r\n")) {
    Rio_readlineb(rp, buf, MAXLINE);
    printf("%s", buf);
    parse_header_line(buf, d);
  }
  
  return d;
}

void read_postquery(rio_t *rp, dictionary_t *headers, dictionary_t *dest) {
  char *len_str, *type, *buffer;
  int len;
  
  len_str = dictionary_get(headers, "Content-Length");
  len = (len_str ? atoi(len_str) : 0);

  type = dictionary_get(headers, "Content-Type");
  
  buffer = malloc(len+1);
  Rio_readnb(rp, buffer, len);
  buffer[len] = 0;

  if (!strcasecmp(type, "application/x-www-form-urlencoded")) {
    parse_query(buffer, dest);
  }

  free(buffer);
}

static char *ok_header(size_t len, const char *content_type) {
  char *len_str, *header;
  
  header = append_strings("HTTP/1.0 200 OK\r\n",
                          "Server: Friendlist Web Server\r\n",
                          "Connection: close\r\n",
                          "Content-length: ", len_str = to_string(len), "\r\n",
                          "Content-type: ", content_type, "\r\n\r\n",
                          NULL);
  free(len_str);

  return header;
}

/*
 * serve_request - example request handler
 */
static void serve_request(int fd, dictionary_t *query) {
  size_t len;
  char *body, *header;

  body = strdup("alice\nbob");

  len = strlen(body);

  /* Send response headers to client */
  header = ok_header(len, "text/html; charset=utf-8");
  Rio_writen(fd, header, strlen(header));
  printf("Response headers:\n");
  printf("%s", header);

  free(header);

  /* Send response body to client */
  Rio_writen(fd, body, len);

  free(body);
}

static void serve_friends(int fd, dictionary_t *query) {
  size_t len;
  char *body, *header;
  dictionary_t *friends_dict;
  const char **friends_arr;

  char* user = dictionary_get(query, "user");

  if((friends_dict = (dictionary_t*)dictionary_get(user_dict, user)) == NULL){
    body = "";
    len = strlen(body);

    /* Send response headers to client */
    header = ok_header(len, "text/html; charset=utf-8");
    Rio_writen(fd, header, strlen(header));
    printf("Response headers:\n");
    printf("%s", header);
    free(header);

    /* Send response body to client, don't free since never allocated */
    Rio_writen(fd, body, len);
    return;
  }
  else{
    friends_arr = dictionary_keys(friends_dict);
    body = join_strings(friends_arr, '\n');
    len = strlen(body);

    /* Send response headers to client */
    header = ok_header(len, "text/html; charset=utf-8");
    Rio_writen(fd, header, strlen(header));
    printf("Response headers:\n");
    printf("%s", header);
    free(header);

    /* Send response body to client */
    Rio_writen(fd, body, len);
    free(body);

    /* Free the array allocated by dictionary_keys*/
    free(friends_arr);
    return;
  } 
}

static void serve_befriend(int fd, dictionary_t *query) {
  dictionary_t *friends_dict, *friends2_dict;
  char **friends_arr;

  char* user = dictionary_get(query, "user");
  char* friends = dictionary_get(query, "friends");

  //Gets the user's dictionary of friends, creates it if it doesn't exist
  if((friends_dict = (dictionary_t*)dictionary_get(user_dict, user)) == NULL){
    friends_dict = make_dictionary(COMPARE_CASE_INSENS, NULL);
    dictionary_set(user_dict, user, friends_dict);
  }

  //Splits the string of friends from query into an array of friends
  friends_arr = split_string(friends, '\n');

  //Iterate through the NULL-terminated array until we hit the NULL
  int i = 0;
  while(friends_arr[i] != NULL){
    if(strcmp(friends_arr[i], user)){
      //Gets the friend's dictionary of friends, creates it if it doesn't exist
      if((friends2_dict = (dictionary_t*)dictionary_get(user_dict, friends_arr[i])) == NULL){
	friends2_dict = make_dictionary(COMPARE_CASE_INSENS, NULL);
	dictionary_set(user_dict, friends_arr[i], friends2_dict);
      }

      //Befriends the user to the friend and vice versa
      dictionary_set(friends2_dict, user, NULL);
      dictionary_set(friends_dict, friends_arr[i], NULL);  
    }
    free(friends_arr[i]);
    i++;
  }

  //Frees the array that was allocated by split_string
  free(friends_arr);
  //Calls serve_friends to print the updated list of friends for user
  serve_friends(fd, query);
}

static void serve_unfriend(int fd, dictionary_t *query) {
  dictionary_t *friends_dict, *friends2_dict;
  char **friends_arr;

  char* user = dictionary_get(query, "user");
  char* friends = dictionary_get(query, "friends");

  //Gets the user's dictionary of friends, if it doesn't exist, no deletions needed
  if((friends_dict = (dictionary_t*)dictionary_get(user_dict, user)) != NULL){

    //Splits the string of friends from query into an array of friends
    friends_arr = split_string(friends, '\n');

    //Iterate through the NULL-terminated array until we hit the NULL
    int i = 0;
    while(friends_arr[i] != NULL){

      //Gets the friend's dictionary of friends, if it doesn't exist, no deletions needed
      if((friends2_dict = (dictionary_t*)dictionary_get(user_dict, friends_arr[i])) != NULL){
      
	//Unfriends the user from the friend and vice versa
	dictionary_remove(friends2_dict, user);
	dictionary_remove(friends_dict, friends_arr[i]);

	//If the removal leaves the friend with no friends, frees the friend's dictionary
	//and removes the friend from the list of users.
	if(dictionary_count(friends2_dict) == 0){
	  //free_dictionary(friends2_dict);
	  dictionary_remove(user_dict, friends_arr[i]);
	}
      }
      free(friends_arr[i]);
      i++;
    }

    //If the removals leave the user with no friends, frees the user's dictionary
    //and removes the user from the list of users.
    if(dictionary_count(friends_dict) == 0){
      //free_dictionary(friends_dict);
      dictionary_remove(user_dict, user);
    }

    //Frees the array that was allocated by split_string 
    free(friends_arr);
  }
  //Calls serve_friends to print the updated list of friends for user
  serve_friends(fd, query);
}

static void serve_introduce(int fd, dictionary_t *query, char* method) {
  int server_fd, len_friends;
  size_t req_len;
  rio_t rio;
  dictionary_t *headers;
  char buf[MAXLINE], *request, *version, *status, *desc, *len_str, *friends;

  //char* user = dictionary_get(query, "user");
  char* friend = dictionary_get(query, "friend");
  char* host = dictionary_get(query, "host");
  char* port = dictionary_get(query, "port");

  char* enc_friend = query_encode(friend);

  //Attempt to establish connection
  if((server_fd = open_clientfd(host, port)) < 0){
    clienterror(fd, method, "500", "Internal Server Error", "Introduce request couldn't connect to host");
  }

  else{

    //Send the request to the server at the host and port
    request = append_strings("GET /friends?user=", enc_friend, " HTTP/1.0", "\r\n\r\n", NULL);
    req_len = strlen(request);
    Rio_writen(server_fd, request, req_len);

    free(enc_friend);

    //After the request arrives and the server sends a response back
    /* Read status line and headers */
    Rio_readinitb(&rio, server_fd);
    if (Rio_readlineb(&rio, buf, MAXLINE) <= 0){
      clienterror(fd, method, "500", "Internal Server Error",
		  "No lines sent from from server at host and port");
      free(request);
      return;
    }
    printf("%s", buf);
  
    //Parse the response status line, giving error if unable
    if (!parse_status_line(buf, &version, &status, &desc)) {
      clienterror(fd, method, "500", "Internal Server Error",
		  "Couldn't parse response status from server at host and port");
    } 
    else {
      //Check that the response has the right version
      if (strcasecmp(version, "HTTP/1.0")
	  && strcasecmp(version, "HTTP/1.1")) {
	clienterror(fd, version, "501", "Not Implemented",
		    "Server at host and port using unsupported version");
	
      } 
      //Check that the response has status code 200
      else if (strcasecmp(status, "200")) {
	clienterror(fd, method, "500", "Internal Server Error",
		    "The server at host and port couldn't successfully complete the request");
	
      } 
      //Successful, so import friends
      else {
	//Read through the response headers(logic is the same as request headers, so same function)
	headers = read_requesthdrs(&rio);

	//Once the headers are read, get the string containing friend's friends,
	//then combine with friend. 
	len_str = dictionary_get(headers, "Content-Length");
	len_friends = (len_str ? atoi(len_str) : 0);
	char* friends_buffer = malloc(len_friends + 1);
	int read = Rio_readnb(&rio, friends_buffer, len_friends);
	//TODO: make sure read and len_friends are the same, error if not
	if(read != len_friends){
	  clienterror(fd, method, "500", "Internal Server Error",
		      "The server at host and port sent the wrong amount of info");
	}
	else{
	  friends_buffer[len_friends] = '\0';
	  friends = append_strings(friend, "\n", friends_buffer, NULL);

	  //Pretend that friends is now part of the query, so we can use serve_befriend
	  //to make them all friends of the user.
	  dictionary_set(query, "friends", friends);
	  serve_befriend(fd, query);

	  free(friends_buffer);
	  //free(friends); is done in doit when we free the query dictionary.
	  free_dictionary(headers);
	}
      } 
      free(version);
      free(status);
      free(desc); 
    }
    free(request);
  }
}


/*
 * clienterror - returns an error message to the client
 */
void clienterror(int fd, char *cause, char *errnum, 
		 char *shortmsg, char *longmsg) {
  size_t len;
  char *header, *body, *len_str;

  body = append_strings("<html><title>Friendlist Error</title>",
                        "<body bgcolor=""ffffff"">\r\n",
                        errnum, " ", shortmsg,
                        "<p>", longmsg, ": ", cause,
                        "<hr><em>Friendlist Server</em>\r\n",
                        NULL);
  len = strlen(body);

  /* Print the HTTP response */
  header = append_strings("HTTP/1.0 ", errnum, " ", shortmsg, "\r\n",
                          "Content-type: text/html; charset=utf-8\r\n",
                          "Content-length: ", len_str = to_string(len), "\r\n\r\n",
                          NULL);
  free(len_str);
  
  Rio_writen(fd, header, strlen(header));
  Rio_writen(fd, body, len);

  free(header);
  free(body);
}

static void print_stringdictionary(dictionary_t *d) {
  int i, count;

  count = dictionary_count(d);
  for (i = 0; i < count; i++) {
    printf("%s=%s\n",
           dictionary_key(d, i),
           (const char *)dictionary_value(d, i));
  }
  printf("\n");
}

//NOTE: check with valgrind like so: valgrind --leak-check=full ./friendlist 8090
