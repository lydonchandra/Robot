using System.Diagnostics;
using System.Text;
using Serilog;
using Serilog.Core;

namespace RobotLib;

public enum RobotCommand { PLACE, MOVE, LEFT, RIGHT, REPORT, ROBOT }
public enum Facing { NORTH, SOUTH, EAST, WEST }
public class OutOfTableException : Exception { }


public record TableDimension(int Width, int Height);

public class Table {
    private readonly TableDimension _tableDimension;
    public Table(TableDimension tableDimension)
    {
       _tableDimension = tableDimension;
    }

    private Dictionary<int, Robot> Robots { get; } = new();

    public Robot ActiveRobot
    {
        get
        {
            var activeKvp = Robots.FirstOrDefault(kvp => kvp.Value.Active);
            return activeKvp.Value;
        }
    }

    public Robot ActivateRobot (int id)
    {
        if (!Robots.Keys.Contains(id))
        {
            throw new ArgumentException($"Unable to activate invalid Robot Id {id}");
        }

        Robots.Values.ToList().ForEach(robot => robot.Active = false);
        Robots[id].Active = true;
        return Robots[id];
    }

    public Robot Place (RobotPosition param) {
        bool isMakeRobotActive = Robots.Keys.Count == 0;
        int robotId = Robots.Keys.Count + 1;
        var newRobot = new Robot(robotId, _tableDimension) { Active = isMakeRobotActive };
        Robots.Add(robotId, newRobot);
        newRobot.Place(param);
        return newRobot;
    }

    public string Report()
    {
        if (!Robots.Keys.Any())
        {
            return "No Robot";
        }

        if (Robots.Keys.Count == 1) {
            return $"Output: {ActiveRobot.Report()}";
        }

        StringBuilder builder = new();
        builder.AppendLine($"Number of Robots: {Robots.Keys.Count}");
        builder.AppendLine($"Active Robot: {this.ActiveRobot.Id}");
        foreach (KeyValuePair<int, Robot> kvp in Robots)
        {
            var robot = kvp.Value;
            string robotId = $"Robot {robot.Id}";
            builder.Append(robotId);
            builder.Append(" ");
            builder.AppendLine($"Output: {robot.Report()}");
        }
        return builder.ToString();
    }
}


