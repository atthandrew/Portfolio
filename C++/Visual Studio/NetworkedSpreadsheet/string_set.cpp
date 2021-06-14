/* A 'string set' is defined as a set of strings stored
 * in a hashtable that uses chaining to resolve collisions.
 *
 * Peter Jensen
 * January 29, 2019
 *
 * Modified by Austin Langston
 * Feb 6, 2019
 */

#include "string_set.h"
#include "node.h"
#include <iostream>  // For debugging, if needed.
#include <string>
#include <vector> 
#include <sstream>   // Needed for the assignment.

namespace cs3505
{
  /*******************************************************
   * string_set member function definitions
   ***************************************************** */
  
  /** Constructor:  The parameter indicates the
    *   size of the hashtable that should be used
    *   to keep this set.
    */
  string_set::string_set()
  {
    // Set up a hashtable of the specified capacity.

    this->table = new node*[2574];
    this->capacity = 2574;
    this->size = 0;
    this->head->forward = tail;
    this->tail->backward = head;

    // The array must be cleared -- it will have bogus values in it.
    //   (NULL means 'no linked list chain in this entry')

    for (int i = 0; i < capacity; i++)
      table[i] = NULL;
  }

  
  /** Copy constructor:  Initialize this set
    *   to contain exactly the same elements as
    *   another set.
    */
  string_set::string_set (const string_set & other)
  {
    // Give initial values to ensure the object is well-formed.

    table = NULL;
    size = 0;
    capacity = 0;
    head->forward = tail;
    tail->backward = head;

    // Use our assignment operator to complete this step.
    //   (Dereference this, then assign to that instance.)
 
    *this = other;
  }


  /** Destructor:  release any memory allocated
    *   for this object.
    */
  string_set::~string_set()
  {
    // Use a helper function to do all the work.

    clean();
  }


  /** Releases any memory that was allocated by
    *   this object.  This effectively destroys the
    *   set, so it should only be called if this object
    *   is destructing, or is being assigned.
    */
  void string_set::clean()
  {
    // Clean up the table (if any)

    if (table != NULL)
    {
      // Clean up (deallocate) any chains in the table.

      for (int i = 0; i < capacity; i++)
        if (table[i] != NULL)
	{
          delete table[i];
          table[i] = NULL;  // Not needed, but a good idea
        }

      // Release the table's memory.

      delete [] table;
    }

    // When 'this' object has been cleaned, it has no array.
    //   Set the fields appropriately.

    table = NULL;
    size = 0;
    capacity = 0;
  }


  /** Computes a table index for a given string.
    *   If two strings have the same contents, their
    *   hash code (table index) will be identical.
    * The hash code is guaranteed to be in the
    *   range [0..capacity).
    */  
  int string_set::hash (const std::string & s) const
  {
    // A well-known hash algorithm.  Do not change!!!
    std::string newS = s;
    int num; 
    //long long hash = 0;
    //for (int i = 0; i < s.length(); i++)
    //  hash = ((hash*2237) + s[i]) % capacity;
    newS.erase(0,1);

    //std::cout << newS << std::endl;
    std::istringstream(newS) >> num;

    if (s[0] == 'A')
    {     
      return -1 + num;
    }
    else if (s[0] == 'B')
    {     
      return 98 + num;
    }
     else if (s[0] == 'C')
    {     
      return 197 + num;
    }
     else if (s[0] == 'D')
    {     
      return 296 + num;
    }
     else if (s[0] == 'E')
    {     
      return 395 + num;
    }
     else if (s[0] == 'F')
    {     
      return 494 + num;
    }
     else if (s[0] == 'G')
    {     
      return 593 + num;
    }
     else if (s[0] == 'H')
    {     
      return 692 + num;
    }
     else if (s[0] == 'I')
    {     
      return 791 + num;
    }
     else if (s[0] == 'J')
    {     
      return 890 + num;
    }
     else if (s[0] == 'K')
    {     
      return 989 + num;
    }
     else if (s[0] == 'L')
    {     
      return 1088 + num;
    }
     else if (s[0] == 'M')
    {     
      return 1187 + num;
    }
     else if (s[0] == 'N')
    {     
      return 1286 + num;
    }
     else if (s[0] == 'O')
    {     
      return 1385 + num;
    }
     else if (s[0] == 'P')
    {     
      return 1484 + num;
    }
     else if (s[0] == 'Q')
    {     
      return 1583 + num;
    }
     else if (s[0] == 'R')
    {     
      return 1682 + num;
    }
     else if (s[0] == 'S')
    {     
      return 1781 + num;
    }
     else if (s[0] == 'T')
    {     
      return 1880 + num;
    }
     else if (s[0] == 'U')
    {     
      return 1979 + num;
    }
     else if (s[0] == 'V')
    {     
      return 2078 + num;
    }
     else if (s[0] == 'W')
    {     
      return 2177 + num;
    }
     else if (s[0] == 'X')
    {     
      return 2276 + num;
    }
     else if (s[0] == 'Y')
    {     
      return 2375 + num;
    }
     else if (s[0] == 'Z')
    {     
      return 2474 + num;
    }
    

    return 0;
  }


  /** Adds the specified element to this set.  If the element
    *   is already in this set, no action is taken.
    */
  void string_set::add (const std::string & target,const std::string & dataIN)
  {
    // Determine which table entry chain might contain this string.
    int index = hash(target);

    // Make a new node, then link it in to the beginning of the chain.
    node *n = new node(target, dataIN, *this);
    n->next = table[index]; 
    if (table[index] != NULL)
    { table[index]->back = n; } 
    table[index] = n;   

    // We added a string - count it.

    this->size++;
  }


  /** Removes the specified target element from this set.  If the
    *   target element is not in the set, no action is taken.
    */
  std::string string_set::remove (const std::string & target)
  {
    int index = hash(target);
    node *possibleNode = table[index];
    std::string dataReturn;

    if(possibleNode == NULL)
    { return "crap"; }
    

    if (possibleNode->next != NULL)
    {
      table[index] = possibleNode->next;
    }
    else
    { table[index] = NULL; }

    dataReturn = possibleNode->data;
	  delete possibleNode;
    size--;
    return dataReturn;
  }

  std::string string_set::removeLast ()
  {
    node *lastNode = this->tail;
    node *prevNode;
    lastNode = lastNode->backward;
    prevNode = lastNode->forward;
    std::string dataReturn;
    int index2 = hash(lastNode->index);

    int letMod = index2 / 99;
    std::string letter; 
    int num;

    if (table[index2] == NULL)
    {
      return "crap";
    }

    if (letMod == 0)
    {
      letter = "A";
      num = index2 + 1;
    }
    else if (letMod == 1)
    {
      letter = "B";
      num = index2 - 98;
    }
    else if (letMod == 2)
    {
      letter = "C";
      num = index2 - 197;
    }
    else if (letMod == 3)
    {
      letter = "D";
      num = index2 - 296;
    }
    else if (letMod == 4)
    {
      letter = "E";
      num = index2 - 395;
    }
    else if (letMod == 5)
    {
      letter = "F";
      num = index2 - 494;
    }
    else if (letMod == 6)
    {
      letter = "G";
      num = index2 - 593;
    }
    else if (letMod == 7)
    {
      letter = "H";
      num = index2 - 692;
    }
    else if (letMod == 8)
    {
      letter = "I";
      num = index2 - 791;
    }
    else if (letMod == 9)
    {
      letter = "J";
      num = index2 - 890;
    }
    else if (letMod == 10)
    {
      letter = "K";
      num = index2 - 989;
    }
    else if (letMod == 11)
    {
      letter = "L";
      num = index2 - 1088;
    }
    else if (letMod == 12)
    {
      letter = "M";
      num = index2 -1187;
    }
    else if (letMod == 13)
    {
      letter = "N";
      num = index2 - 1286;
    }
    else if (letMod == 14)
    {
      letter = "O";
      num = index2 - 1385;
    }
    else if (letMod == 15)
    {
      letter = "P";
      num = index2 - 1484;
    }
    else if (letMod == 16)
    {
      letter = "Q";
      num = index2 - 1583;
    }
    else if (letMod == 17)
    {
      letter = "R";
      num = index2 - 1682;
    }
    else if (letMod == 18)
    {
      letter = "S";
      num = index2 - 1781;
    }
    else if (letMod == 19)
    {
      letter = "T";
      num = index2 - 1880;
    }
    else if (letMod == 20)
    {
      letter = "U";
      num = index2 - 1979;
    }
    else if (letMod == 21)
    {
      letter = "V";
      num = index2 - 2078;
    }
    else if (letMod == 22)
    {
      letter = "W";
      num = index2 - 2177;
    }
    else if (letMod == 23)
    {
      letter = "X";
      num = index2 - 2276;
    }
    else if (letMod == 24)
    {
      letter = "Y";
      num = index2 - 2375;
    }
    else if (letMod == 25)
    {
      letter = "Z";
      num = index2 - 2474;
    }

    std::ostringstream convert2;
    convert2 << num;
    letter.append(convert2.str());


    //If nothing is there, you can't remove it
    if (lastNode == NULL)
    {
	    return "crap";
    }
     
    //It found it
    if(lastNode->next == NULL)
    {
      int index = hash(lastNode->index);
      table[index] = NULL; 
      dataReturn = lastNode->data;
	    delete lastNode; 
      size--;
      letter.append(",");
      letter.append(dataReturn);
      return letter;
    }
    else
    {
      prevNode = lastNode->next;
      int index = hash(lastNode->index);
      table[index] = prevNode; 
      dataReturn = lastNode->data;
	    delete lastNode; 
      size--;
      letter.append(",");
      letter.append(dataReturn);
      return letter;
    }

    return "crap";
  }


  /** Returns true if the specified target element in in this set,
    *   false otherwise.
    */
  bool string_set::contains (const std::string & target,const std::string & dataIN) const
  {    
    // To be completed as part of the assignment.
    int index = hash(target);
    node *possibleNode = table[index];

    //If nothing is there, nothing is there
    if (possibleNode == NULL)
      {
	return false;
      }

    //See if it exists
    while (possibleNode->data != dataIN)
      {
	//Guess it doesn't exist
	if (possibleNode->next == NULL)
	  { return false; }
     	possibleNode = possibleNode->next;
      }

    //Hey there, it exists
    if (possibleNode->data == dataIN)
      { return true; }

    return false;  // Stub - update/change as needed.
  }


  /** Returns a count of the number of elements
    *   in this set.
    */
  int string_set::get_size() const
  {
    return this->size;
  }


  /*** Assignment operator ***/
  
  /** This function overloads the assignment operator.  It
    *   clears out this set, builds an empty table, and copies
    *   the entries from the right hand side (rhs) set into
    *   this set.
    */
  string_set & string_set::operator= (const string_set & rhs)
  {
    // If we are assigning this object to this object,
    //   do nothing.  (This is important!)

    if (this == &rhs)  // Compare addresses (not object contents)
      return *this;  // Do nothing if identical

    // Wipe away anything that is stored in this object.
    
    clean();
    
    // Create a new set (new table) and populate it with the entries
    //   from the set in rhs.  Use the capacity from rhs.  Hint:
    //   see the first constructor above (but you cannot call it).
    
    // Requirement:  Do not permanently point to arrays or nodes in rhs.  
    //   When rhs is destroyed, it will delete its array and nodes, 
    //   and we cannot count on their existence.  Instead, you will
    //   create a new array for this object, traverse rhs,
    //   and add one entry to this set for every entry in rhs.
    
    
    // To be completed as part of the assignment.
    
    // Set up a hashtable of the specified capacity.(By Peter Jensen till the end of the first for loop)

    this->table = new node*[rhs.capacity];
    this->capacity = rhs.capacity;
    this->size = 0;
    this->head->forward = tail;
    this->tail->backward = head;

    // The array must be cleared -- it will have bogus values in it.
    //   (NULL means 'no linked list chain in this entry')
    for (int i = 0; i < capacity; i++)
      table[i] = NULL;

    std::vector<std::string> myContents = rhs.get_elements();
    for (std::vector<std::string>::iterator it = myContents.begin(); it != myContents.end(); it++)
      {
        std::string word = *it;
	//this->add(word);
      }  

    // Done with assignment operator.

    return *this;
  }


/** This function populates and returns a std::vector<std::string>   
 * with all the elements in this set.  The strings in the vector will be in the   
 * order that they were inserted into to this set, v[0] was added first, etc.   
 * (Note:  Attempting to add a duplicate string does not count or   
 * change this ordering.)   
 * The size of the return vector will be the size of this string_set.    
 */  
std::vector<std::string> string_set::get_elements () const
  {
    std::vector<std::string> myContents = std::vector<std::string>();

    node *start = this->head;
    //We don't want to start at the head
    start = start->forward;

    //Empty List
    if (start == 0)
      { return myContents; } 

    //Grab them all!!!... Besides the Tail
    while (start->forward != NULL)
      {
	myContents.push_back(start->data);
	start = start->forward;
      }

    return myContents;

  } 

}
