using Microsoft.VisualStudio.TestTools.UnitTesting;
using maze;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace maze.Tests
{
    [TestClass()]
    public class MazeWalkerTests
    {
        public List<List<string>> generatemaze()
        {
            List<List<string>> maze = new List<List<string>>();

            List<string> row1 = new List<string>();
            row1.Add("1");
            row1.Add("1");
            row1.Add("1");
            row1.Add("1");
            maze.Add(row1);

            List<string> row2 = new List<string>();
            row2.Add("1");
            row2.Add("0");
            row2.Add("0");
            row2.Add("1");
            maze.Add(row2);
            maze.Add(row2);

            maze.Add(row1);

            return maze;

        }
        [TestMethod()]
        public void move_northTest()
        {
            List<List<string>> maze = generatemaze();

            MazeWalker walker = new MazeWalker(2, 2, Direction.North);
            Assert.IsTrue(walker.move_north(maze));

            walker.X = 2;
            walker.Y = 1;
            Assert.IsFalse(walker.move_north(maze));

        }

        [TestMethod()]
        public void move_southTest()
        {
            List<List<string>> maze = generatemaze();

            MazeWalker walker = new MazeWalker(1, 1, Direction.South);
            Assert.IsTrue(walker.move_south(maze));

            walker.X = 1;
            walker.Y = 2;
            Assert.IsFalse(walker.move_south(maze));
        }

        [TestMethod()]
        public void move_westTest()
        {
            List<List<string>> maze = generatemaze();

            MazeWalker walker = new MazeWalker(2, 2, Direction.West);
            Assert.IsTrue(walker.move_west(maze));

            walker.X = 1;
            walker.Y = 2;
            Assert.IsFalse(walker.move_west(maze));
        }

        [TestMethod()]
        public void move_eastTest()
        {
            List<List<string>> maze = generatemaze();

            MazeWalker walker = new MazeWalker(1, 1, Direction.East);
            Assert.IsTrue(walker.move_east(maze));

            walker.X = 2;
            walker.Y = 1;
            Assert.IsFalse(walker.move_east(maze));

        }
    }
}