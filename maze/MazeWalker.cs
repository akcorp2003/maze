using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace maze
{
    public enum Direction
    {
        North,
        South,
        East,
        West
    };
    public class MazeWalker
    {
        private int m_x;
        private int m_y;
        private Direction m_direction;

        public MazeWalker()
        {
            m_x = 0;
            m_x = 0;
            m_direction = Direction.South;
        }

        public MazeWalker(int x, int y, Direction dir)
        {
            m_x = x;
            m_y = y;
            m_direction = dir;
        }

        public int X
        {
            get
            {
                return m_x;
            }
            set
            {
                m_x = value;
            }
        }

        public int Y
        {
            get
            {
                return m_y;
            }
            set
            {
                m_y = value;
            }
        }

        public Direction Dir
        {
            get
            {
                return m_direction;
            }
            set
            {
                m_direction = value;
            }
        }

        /// <summary>
        /// A wrapper. Moves the walker north.
        /// </summary>
        /// <param name="maze">The maze.</param>
        /// <returns>
        /// The walker with its coordinates and orientation adjusted north.
        /// </returns>
        public bool move_north(List<List<string>> maze)
        {
            bool move_success = movewalker(X, Y - 1, maze);
            if (!move_success)
            {
                return false;
            }
            else
            {
                Dir = Direction.North;
            }
            return true;
        }

        /// <summary>
        /// A wrapper. Moves the walker south.
        /// </summary>
        /// <param name="maze">The maze.</param>
        /// <returns>
        /// The walker with its coordinates and orientation adjusted south.
        /// </returns>
        public bool move_south(List<List<string>> maze)
        {
            bool move_success = movewalker(X, Y + 1, maze);
            if (!move_success)
            {
                return false;
            }
            else
            {
                Dir = Direction.South;
            }
            return true;
        }

        /// <summary>
        /// A wrapper. Moves the walker west.
        /// </summary>
        /// <param name="maze">The maze.</param>
        /// <returns>
        /// The walker with its coordinates and orientation adjusted west.
        /// </returns>
        public bool move_west(List<List<string>> maze)
        {
            bool move_success = movewalker(X - 1, Y, maze);
            if (!move_success)
            {
                return false;
            }
            else
            {
                Dir = Direction.West;
            }
            return true;
        }

        /// <summary>
        /// A wrapper. Moves the walker east.
        /// </summary>
        /// <param name="maze">The maze.</param>
        /// <returns>
        /// The walker with its coordinates and orientation adjusted east.
        /// </returns>
        public bool move_east(List<List<string>> maze)
        {
            bool move_success = movewalker(X + 1, Y, maze);
            if (!move_success)
            {
                return false;
            }
            else
            {
                Dir = Direction.East;
            }
            return true;
        }

        /// <summary>
        /// Moves the walker to the proposed location and marks it on the maze.
        /// </summary>
        /// <param name="x_loc">The x location.</param>
        /// <param name="y_loc">The y location.</param>
        /// <param name="maze">The maze.</param>
        /// <returns>
        /// A boolean value if the move was successful.
        /// </returns>
        private bool movewalker(int x_loc, int y_loc, List<List<string>> maze)
        {
            if (!is_locationvalid(x_loc, y_loc, maze))
            {
                return false;
            }

            //adjust the value in the maze
            List<string> pixel_row = maze[y_loc];
            pixel_row[x_loc] = "X";

            X = x_loc;
            Y = y_loc;
            return true;
        }

        /// <summary>
        /// Returns if the proposed x,y location is valid.
        /// </summary>
        /// <param name="x">The proposed x-coordinate.</param>
        /// <param name="y">The proposed y-coordinate.</param>
        /// <param name="maze">The maze where the walker resides.</param>
        /// <returns>A boolean value indicating if the proposed location is a valid spot to place the walker.
        /// </returns>
        private bool is_locationvalid(int x, int y, List<List<string>> maze)
        {
            if (x < 0 || y < 0 || y >= maze.Count)
            {
                return false;
            }
            List<string> pixel_row = maze[y];
            if (x >= pixel_row.Count)
            {
                return false;
            }

            if (pixel_row[x] == "1")
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
