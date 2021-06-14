/* This node class is used to build linked lists for the
 * string_set class.
 *
 * Peter Jensen
 * January 29, 2019
 *
 * Modified by Austin Langston
 * Feb 5, 2019
 */

#ifndef NODE_H
#define NODE_H

#include <string>


namespace cs3505
{
  // We're in a namespace - declarations will be within this namespace.
  //   (There are no definitions here.)

  /* Node class for holding elements. */
  class string_set; // Forward Declaration 

  class node
  {
    friend class string_set;   // This allows functions in string_set to access
			       //   private data (and constructor) within this class.
 
    private:
    node(const std::string & index, const std::string & data, string_set& currentSet);  // Constructor (changed to take a reference to avoid a copy)
    node(const std::string & data);
      ~node();                         // Destructor

      std::string data; 
      std::string index;    // Variable to hold the element
      node        *next; 
      node        *back;    // Variable to point to the next node in the list    
      node *forward;
      node *backward;
  };
}

#endif
