/* This node class is used to build linked lists for the
 * string_set class.
 *
 * Peter Jensen
 * January 29, 2019
 */

#include <iostream>
#include "node.h"
#include "string_set.h"

// We're not in a namespace.  We are not in any class.  Symbols defined
//   here are globally available.  We need to qualify our function names
//   so that we are definining our cs3505::node class functions.
//
// Note that we could also use the namespace cs3505 { } block.  This would
//   eliminate one level of name qualification.  The 'using' statement will
//   not help in this situation.  
// 
// Keep it as shown here for node.cpp.  I show the other way in string_set.cpp.

/*******************************************************
 * node member function definitions
 ***************************************************** */

/** Constructor:  Creates a node containing
  *   an element.  It is initialized to
  *   not point to any other node.
  */
cs3505::node::node(const std::string & index, const std::string & s,string_set& currentSet)
  : next(NULL),  // This syntax is used to call member variable constructors (or initialize them).
    data(s)      // This calls the copy constructor - we are making a copy of the string.
{
  // No other work needed - the initializers took care of everything.
  // We were mislead -Austin

  //Sets the Forward and Backward Nodes
  node *lastEntry = currentSet.tail->backward;
  this->index = index;

  if (currentSet.head->forward == currentSet.tail)
    {
      currentSet.head->forward = this;
      currentSet.tail->backward = this;
      this->backward = currentSet.head;
      this->forward = currentSet.tail;
    }

    this->backward = lastEntry;
    lastEntry->forward = this;
    currentSet.tail->backward = this;
    this->forward = currentSet.tail;
}

/** Constructor:  Creates a node containing
  *   an element.  It is initialized to
  *   not point to any other node.
  */
cs3505::node::node(const std::string & s)
  : next(NULL),  // This syntax is used to call member variable constructors (or initialize them).
    data(s)      // This calls the copy constructor - we are making a copy of the string.
{
}

  
/** Destructor:  release any memory allocated
  *   for this object.
  */
cs3505::node::~node()
{
  // I'm not convinced that the recursive delete is the
  //   best approach.  I'll keep it (and you'll keep it too).
  node *potForward;
  node *potBackward;

  //if (this->next != NULL)
    //{ delete this->next; }

  if (this->forward !=NULL)
    { 
      potBackward = this->backward;
      potForward = this->forward; 
      potBackward->forward = potForward;
      potForward->backward = potBackward;
    }
  else
    {
     if (this->backward == NULL)
	{ //Do nothing
	} 
     else
       {
	 potBackward = this->backward;
	 potBackward->forward = NULL;
       }
    }

  // Invalidate the entry so that it is not accidentally used.

  this->next = NULL;      
}
