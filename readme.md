# Robot

## Pre-req:
* dotnet 6 sdk

## Implemented Functionalities
* Multiple Robots functionalities
  * PLACE <xxx> adds new robot 
  * ROBOT <n> activates robot n

## Input File
* Hardcoded to RobotConsole/Input/Input.txt
  * This input file will be copied to output folder during build and will be accessed when running.
  * Edit this file & build to add new commands

## Running

### Running main console program
```
cd Robot/RobotConsole
dotnet run
```

### Running unit test
```
cd Robot
dotnet test
```

### Addition:
* Added `#` to input as comment character
 
```
  PLACE 0,0,NORTH
  LEFT
  REPORT
  # Output should be 0,0,WEST 
```  