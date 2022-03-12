using System;
using FluentAssertions;
using RobotLib;
using Xunit;

namespace RobotTest;

public class UnitTest1
{
    [Fact]
    public void TestRobotMovement()
    {
        TableDimension tableDim = new(5, 5);
        var robot = new Robot(1, tableDim);
        var act = () => robot.Move();
        act.Should().Throw<ArgumentException>("Robot is not on the table yet");

        robot.Place(new RobotPosition(0, 1, Facing.SOUTH));
        robot.Report().Should().BeEquivalentTo(new RobotPosition(0, 1, Facing.SOUTH));

        robot.Move();
        robot.Report().Should().BeEquivalentTo(new RobotPosition(0, 0, Facing.SOUTH));

        bool isMoved = robot.Move();
        isMoved.Should().BeFalse();

        robot.Right();
        robot.Report().Should().BeEquivalentTo(new RobotPosition(0, 0, Facing.WEST));
        robot.Right();
        robot.Report().Should().BeEquivalentTo(new RobotPosition(0, 0, Facing.NORTH));
        robot.Right();
        robot.Report().Should().BeEquivalentTo(new RobotPosition(0, 0, Facing.EAST));
        robot.Right();
        robot.Report().Should().BeEquivalentTo(new RobotPosition(0, 0, Facing.SOUTH));

        robot.Left();
        robot.Report().Should().BeEquivalentTo(new RobotPosition(0, 0, Facing.EAST));
        robot.Left();
        robot.Report().Should().BeEquivalentTo(new RobotPosition(0, 0, Facing.NORTH));
        robot.Left();
        robot.Report().Should().BeEquivalentTo(new RobotPosition(0, 0, Facing.WEST));
        robot.Left();
        robot.Report().Should().BeEquivalentTo(new RobotPosition(0, 0, Facing.SOUTH));



    }
}