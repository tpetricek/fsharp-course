// ============================================================================
// INTRO 5: List processing using recursion
// ============================================================================

#load "setup.fsx"
open Setup

// ----------------------------------------------------------------------------
// WALKTHROUGH: Recursive list processing
// ----------------------------------------------------------------------------

// Here is a number of ways to create a list. Fill in the results!
// Note that lists are immutable - when we create a new list with
// a head and tail (using another list), the original is unchanged
let l1 = []
let l2 = 3::l1
let l3 = [1; 2]
let l4 = l3 @ l2
let l5 = [ 1 .. 5 ]

shouldEqual l1 __
shouldEqual l2 __
shouldEqual l4 __
shouldEqual l5 __

// Lists are really discriminated unions with two cases - one for an empty
// list and one for a list with value and remainder of a list. The cases
// are written as '[]' (for the empty) and 'x::xs' for the non-empty.


// To process a list, we can use pattern matching. The following simple 
// function returns zero when the list is empty or its "head" when the 
// list is not empty.
let headOrDefault list = 
  match list with
  | [] -> 0
  | head::tail -> head

// Fill in the results!
shouldEqual (headOrDefault []) __
shouldEqual (headOrDefault [1;2]) __

// The following function shows how to multiply all elements of a list
// by two. To do this, we multiply the head, process the tail
// recursively and then reconstruct the original list.
let rec multiplyByTwo list = 
  match list with
  | [] -> []
  | head::tail -> 
      let newHead = head * 2
      let newTail = multiplyByTwo tail
      newHead :: newTail

// Fill in the results!
shouldEqual (multiplyByTwo [ 1; 3; 5 ]) __

// Similarly, let's see how to filter out all odd numbers from a list.
// To do that, we use the 'when' clause in pattern to check when the
// number in the head is even/odd. Then we process the tail recursively
// and either append the head or not.
let rec filterOdds list = 
  match list with
  | head::tail when head % 2 = 0 -> 
      head::(filterOdds tail)
  | head::tail -> 
      __
  | [] -> 
      __

shouldEqual (filterOdds [ 1 .. 10 ]) [ 2; 4; 6; 8; 10 ]

// ============================================================================
// TASKS: List processing using recursion
// ============================================================================

// TASK #7: Write a function that sums the values in a list of integers.
// The function should call itself recursively for non empty lists and
// return zero directly for empty lists.

let rec sum list = 
  __

shouldEqual (sum []) 0
shouldEqual (sum [1;3;5]) 9

// TASK #8 (BONUS): Re-implement the 'map' function for list processing.
// This is pretty much how the built-in function that we were using in 
// the previous sections is implemented! The code will be quite similar
// to the 'multiplyByTwo' function above.


