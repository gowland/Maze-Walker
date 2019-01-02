# Maze Walker Kata

## Acknowledgements

Code associated with TDDBuddy.com Maze Walker kata.
The katas was sourced from Redgate's Blog and modifed slightly.

For information on the original kata see https://www.red-gate.com/blog/building/code-kata-2-amaze.

## Part 1

* Current problem
  * maze1.txt works
  * maze2.txt gets stuck
* A new algorithm is available in the `AdvancedMazeSolver` class in the ThirdPartyLogic project
  * we may not modify or add any code in this project as it is provided by a 3rd party
  * write an Adapter to use the existing `MazeWalker` class with the new logic

### Notes

* `Point` already has an implicit conversion to ThirdPartyPoint
* Conversion extension methods to/from this project's `Orientation` enum and ThirdPartyLogic's `Direction` enum already exist

## Part 2

* We have two different ways of displaying the progress through a maze: `BlandWalkerStateDumper` and `FancyWalkerStateDumper`.
* The former outputs the position of the walker at each stage, the later outputs a pictoral view of the whole maze and the walker's position within it.
* Both implement the same interface. Both currently dump to the console.
* Now we want both classes to be able to write both to the console and to file.
* Apply the Bridge pattern to allow both classes to write to both console and file.

### Notes

* Yes, there are better ways of doing this than using the Bridge pattern since both output strings, but imagine the output for one was an .png, or an audio file.

## Part 3

* We have an upcoming requirment to support multiple formats for maze input. In preparation, apply the Builder pattern to the maze creation logic.
* Rewrite the input to accept input in the form given in MazeFiles/points1.txt

### Notes:

* The `Points` class already has a `FromString` method.