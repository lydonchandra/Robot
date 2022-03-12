using System.Text.RegularExpressions;
using RobotLib;

var tableDim = new TableDimension(Width: 5, Height: 5);
var table = new Table(tableDim);
string commandFilePath = @"D:\code\robot\Robot\RobotConsole\Input\Input1.txt";

var commands = File.ReadAllLines(commandFilePath);
foreach (string command in commands)
{
    string trimCommand = command.Trim().ToUpper();
    if (string.IsNullOrWhiteSpace(trimCommand))
    {
        throw new ArgumentException("Empty Command Line");
    }

    var cmdArgs = trimCommand
        .Split(" ")
        .Where(word => !string.IsNullOrWhiteSpace(word))
        .ToList();

    if (cmdArgs.Count > 2) throw new ArgumentException("");

    string cmd = cmdArgs[0];
    string robotArgs = string.Empty;
    if (cmdArgs.Count == 2)
    {
        robotArgs = cmdArgs[1].Trim();
    }

    bool isValidRobotCommand = Enum.TryParse(cmd, ignoreCase: false, out RobotCommand robotCommand);

    if (!isValidRobotCommand)
    {
        throw new ArgumentException($"Invalid command {cmd}");
    }

    switch (robotCommand)
    {
        case RobotCommand.PLACE:
            string[] placeArgs = robotArgs.Split(",");
            int x = int.Parse(placeArgs[0]);
            int y = int.Parse(placeArgs[1]);
            if (!Enum.TryParse(placeArgs[2], out Facing facing))
                throw new ArgumentException();

            var robotPos = new RobotPosition(x, y, facing);
            table.Place(robotPos);
            break;
        case RobotCommand.MOVE:
            table.ActiveRobot.Move();
            break;
        case RobotCommand.LEFT:
            table.ActiveRobot.Left();
            break;
        case RobotCommand.RIGHT:
            table.ActiveRobot.Right();
            break;
        case RobotCommand.REPORT:
            table.Report();
            break;
        case RobotCommand.ROBOT:
            break;
        default:
            throw new ArgumentOutOfRangeException();
    }
}

// var position = new RobotPosition(0, 0, Facing.NORTH);
// var robot = table.Place(position);
// robot.Move();
// robot.Move();
// robot.Move();
// robot.Move();
// robot.Move();
//
// robot.Report();
// position.X = 6;
// try
// {
//     robot.Place(position);
// }
// catch {
//
// }
//
// robot.Left();
// robot.Move();
//
// robot.Left();
// robot.Move();
// robot.Move();
// robot.Move();
// robot.Move();
// robot.Move();
// robot.Left();
// robot.Move();
// robot.Move();
// robot.Move();
// robot.Move();
// robot.Move();
//
// Console.WriteLine(table.Report());
//
// var position2 = new RobotPosition(0, 0, Facing.NORTH);
// table.Place(position2);
// Console.WriteLine(table.Report());
//
//
//
