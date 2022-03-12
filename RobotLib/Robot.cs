using Serilog;
using Serilog.Core;

namespace RobotLib;

public class RobotPosition: ICloneable {
    public int X { get; set; }
    public int Y { get; set; }
    public Facing Facing { get; set; }

    public RobotPosition (int x, int y, Facing facing) {
        X = x;
        Y = y;
        Facing = facing;
    }
    public bool Validate(TableDimension tableDim) {
        if (X >= tableDim.Width || X < 0
                                ||
                                Y >= tableDim.Height || Y < 0)
        {
            return false;
        }
        return true;
    }
    public override string ToString()
    {
        return $"{X},{Y},{Facing}";
    }

    public object Clone()
    {
        return new RobotPosition(X, Y, Facing);
    }
}

public class Robot
{
    public Robot (int id, TableDimension tableDimension) {
        Id = id;
        _tableDimension = tableDimension;
        _logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.Console().CreateLogger();
    }

    public int Id { get; }
    public bool Active { get; set; }

    private RobotPosition? _currentPosition;
    private readonly TableDimension _tableDimension;
    private readonly Logger _logger;

    public bool Place(RobotPosition robotPosition)
    {
        if (!robotPosition.Validate(_tableDimension))
        {
            _logger.Verbose($"Invalid Place Position, out of table {robotPosition}");
            throw new OutOfTableException();
        }

        _currentPosition = robotPosition;
        _logger.Verbose($"Robot {Id} placed at {_currentPosition}");
        return true;
    }

    internal const string ErrorNotOnTable = "Robot is not place on table yet";

    public bool Move()
    {
        var positionAfterMove = _currentPosition?.Clone() as RobotPosition
                                ?? throw new ArgumentException(ErrorNotOnTable);
        switch (positionAfterMove.Facing) {
            case Facing.EAST:
                positionAfterMove.X += 1;
                break;
            case Facing.WEST:
                positionAfterMove.X -= 1;
                break;
            case Facing.SOUTH:
                positionAfterMove.Y -= 1;
                break;
            case Facing.NORTH:
                positionAfterMove.Y += 1;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        if (!positionAfterMove.Validate(_tableDimension)) {
            _logger.Warning($"Not moving, Robot {Id} will fall off the table if moved.");
            return false;
        }
        _currentPosition = positionAfterMove;
        _logger.Verbose($"Robot {Id} Moved to {_currentPosition}");
        return true;
    }

    public RobotPosition Left()
    {
        Rotate(
            _currentPosition ?? throw new InvalidOperationException(ErrorNotOnTable)
            , RotateDirection.LEFT);
        _logger.Verbose($"Robot {Id} position: {_currentPosition}");
        return _currentPosition;
    }

    public RobotPosition Right()
    {
        Rotate(
            _currentPosition ?? throw new InvalidOperationException(ErrorNotOnTable)
            , RotateDirection.RIGHT);

        _logger.Verbose($"Robot {Id} position: {_currentPosition}");
        return _currentPosition;
    }

    internal enum RotateDirection { LEFT, RIGHT }

    internal void Rotate (RobotPosition position, RotateDirection rotateDirection)
    {
        switch(rotateDirection)
        {
            case RotateDirection.LEFT:
                position.Facing = position.Facing switch
                {
                    Facing.EAST => Facing.NORTH,
                    Facing.NORTH => Facing.WEST,
                    Facing.WEST => Facing.SOUTH,
                    Facing.SOUTH => Facing.EAST,
                    _ => position.Facing
                };
                break;
            case RotateDirection.RIGHT:
                position.Facing = position.Facing switch
                {
                    Facing.EAST => Facing.SOUTH,
                    Facing.SOUTH => Facing.WEST,
                    Facing.WEST => Facing.NORTH,
                    Facing.NORTH => Facing.EAST,
                    _ => throw new ArgumentOutOfRangeException()
                };
                break;
        }
    }

    public RobotPosition? Report()
    {
        _logger.Verbose($"{_currentPosition}");
        return _currentPosition;
    }
}
