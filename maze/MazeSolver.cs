using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace maze
{
    public enum Solving_Method
    {
        Right_hand,
        BreadthFirst,
        DepthFirst
    };
    public class MazeSolver
    {
        List<List<string>> m_maze_to_solve;
        //Maze legend:
        //white space    - 0
        //wall           - 1
        //start          - S
        //end            - E
        //walker visited - X

        public MazeSolver()
        {
            m_maze_to_solve = new List<List<string>>();
        }

        public MazeSolver(List<List<string>> startmaze)
        {
            m_maze_to_solve = startmaze;
        }

        /// <summary>
        /// Solves the m_maze_to_solve according to the algorithm specified by user.
        /// As of right now, there's only one method allowed but in the future, it would be nice to have multiple methods of maze solving.
        /// The current supported maze solving algorithm is the Wall Follower method.
        /// </summary>
        /// <param name="solve_method">
        /// Use Solving_Method enum.
        /// </param>
        /// <returns>
        /// A jagged list of the solved maze.
        /// </returns>
        public List<List<string>> solve_maze(Solving_Method solve_method)
        {
            if(solve_method == Solving_Method.Right_hand)
            {
                return solve_maze_RH();
            }
            return null;
        }

        /// <summary>
        /// Solves the given maze using the right-hand wall following method. 
        /// NOTE: this algorithm will not work for mazes that are not simply connected.
        /// </summary>
        /// <returns>
        /// The parsed solution to the maze.
        /// </returns>
        private List<List<string>> solve_maze_RH()
        {
            bool pathFound = false;
            MazeWalker walker = new MazeWalker();
            walker = findstart(walker);
            while(!pathFound)
            {
                //for each direction, test to see which directions are valid
                //we always prioritize the current direction
                //then the directions are prioritized according to the right-hand following method
                if(walker.Dir == Direction.North)
                {
                    bool move_success = false;
                    if(is_northvalid(walker.X, walker.Y))
                    {
                        move_success = walker.move_north(m_maze_to_solve);
                    }
                    else if(is_eastvalid(walker.X, walker.Y))
                    {
                        move_success = walker.move_east(m_maze_to_solve);
                    }
                    else if(is_westvalid(walker.X, walker.Y))
                    {
                        move_success = walker.move_west(m_maze_to_solve);
                    }
                    else
                    {
                        //turning back
                        move_success = walker.move_south(m_maze_to_solve);
                    }

                    if(!move_success)
                    {
                        return null; //something went wrong somewhere
                    }
                    else
                    {
                        walker = turncorner(walker, walker.Dir);
                    }                  

                }
                else if(walker.Dir == Direction.South)
                {
                    bool move_success = false;
                    if(is_southvalid(walker.X, walker.Y))
                    {
                        move_success = walker.move_south(m_maze_to_solve);
                    }
                    else if(is_westvalid(walker.X, walker.Y))
                    {
                        move_success = walker.move_west(m_maze_to_solve);
                    }
                    else if(is_eastvalid(walker.X, walker.Y))
                    {
                        move_success = walker.move_east(m_maze_to_solve);
                    }
                    else
                    {
                        //turn around
                        move_success = walker.move_north(m_maze_to_solve);
                    }

                    if(!move_success)
                    {
                        return null;
                    }
                    else
                    {
                        walker = turncorner(walker, walker.Dir);
                    }

                }
                else if(walker.Dir == Direction.East)
                {
                    bool move_success = false;
                    if(is_eastvalid(walker.X, walker.Y))
                    {
                        move_success = walker.move_east(m_maze_to_solve);
                    }
                    else if(is_southvalid(walker.X, walker.Y))
                    {
                        move_success = walker.move_south(m_maze_to_solve);
                    }
                    else if(is_northvalid(walker.X, walker.Y))
                    {
                        move_success = walker.move_north(m_maze_to_solve);
                    }
                    else
                    {
                        //turn around
                        move_success = walker.move_west(m_maze_to_solve);
                    }

                    if(!move_success)
                    {
                        return null;
                    }
                    else
                    {
                        walker = turncorner(walker, walker.Dir);
                    }
                }
                else if(walker.Dir == Direction.West)
                {
                    bool move_success = false;
                    if(is_westvalid(walker.X, walker.Y))
                    {
                        move_success = walker.move_west(m_maze_to_solve);
                    }
                    else if(is_northvalid(walker.X, walker.Y))
                    {
                        move_success = walker.move_north(m_maze_to_solve);
                    }
                    else if(is_southvalid(walker.X, walker.Y))
                    {
                        move_success = walker.move_south(m_maze_to_solve);
                    }
                    else
                    {
                        //turn around
                        move_success = walker.move_east(m_maze_to_solve);
                    }

                    if(!move_success)
                    {
                        return null;
                    }
                    else
                    {
                        walker = turncorner(walker, walker.Dir);
                    }

                }
                else
                {
                    //this should never be executed, but in case something goes wrong here, we will arbitrarily place the walker into a direction
                    walker.Dir = Direction.North;
                }

                MazeParser.color(m_maze_to_solve, walker);

                pathFound = is_finish(walker);

                MazeParser.color(m_maze_to_solve, walker);
                
            }//end while

            //MazeParser.output_parse(m_maze_to_solve); //for debugging

            return m_maze_to_solve;
        }

        /// <summary>
        /// Locates the starting position for the walker.
        /// </summary>
        /// <param name="walker">The walker.</param>
        /// <returns>
        /// The walker with proper coordinates and direction.
        /// </returns>
        private MazeWalker findstart(MazeWalker walker)
        {
            int s_y = 0;
            int s_x = 0; //x and y coordinates of the start
            bool firstpixel_found = false;

            //first, find the location of the first red pixel (S)
            for(int y = 0; y < m_maze_to_solve.Count; y++)
            {
                List<string> pixel_row = m_maze_to_solve[y];
                for(int x = 0; x < pixel_row.Count; x++)
                {
                    if (pixel_row[x] == "S")
                    {
                        s_x = x;
                        s_y = y;
                        firstpixel_found = true;
                        break;
                    }
                }

                if(firstpixel_found)
                {
                    break;
                }
            }//end for

            walker.X = s_x;
            walker.Y = s_y;
            //at this point, we will have found the start's top left pixel
            //we now need to determine the best orientation that the walker should proceed
            return setorientation(walker);
        }

        /// <summary>
        /// Determines if the walker has found the end of the maze.
        /// </summary>
        /// <param name="walker">The walker.</param>
        /// <returns>
        /// A boolean value indicating if the walker is at the end.
        /// </returns>
        private bool is_finish(MazeWalker walker)
        {
            //first, see if the walker is on a finish line
            List<string> pixel_row = m_maze_to_solve[walker.Y];
            if(pixel_row[walker.X] == "E")
            {
                return true;
            }

            bool reached_end = false;
            //scan the region to see if the finish is in the walker's peripheral vision
            if(walker.Dir == Direction.North || walker.Dir == Direction.South)
            {
                //scan the west and east directions
                if(is_westfinish(walker) || is_eastfinish(walker))
                {
                    reached_end = true;
                }
            }
            else if(walker.Dir == Direction.East || walker.Dir == Direction.West)
            {
                //scan the north and south directions
                if(is_northfinish(walker) || is_southfinish(walker))
                {
                    reached_end = true;
                }
            }
            else
            {
                //should never get here but this is here just in case
                return false;
            }
            return reached_end;
        }

        /// <summary>
        /// Determines if the end is in the western vision of the walker and if so, sends the walker directly in that direction.
        /// </summary>
        /// <param name="walker">The walker</param>
        /// <returns>
        /// A boolean value if the end is in the vicinity and also the walker with updated values.
        /// </returns>
        private bool is_westfinish(MazeWalker walker)
        {
            bool is_finish = false;
            int y = walker.Y;
            int x = walker.X;

            if (y < 0 || x < 0 || y >= m_maze_to_solve.Count)
            {
                return false;
            }

            List<string> pixel_row = m_maze_to_solve[y];

            if(x >= pixel_row.Count)
            {
                return false;
            }

            while(pixel_row[x] != "1")
            {
                if(x < 0)
                {
                    break;
                }
                if(pixel_row[x] == "E")
                {
                    is_finish = true;
                    break;
                }
                else
                {
                    x--;
                }
            }

            if(is_finish)
            {
                //finish the job
                x = walker.X;
                walker.Dir = Direction.West;
                while(pixel_row[x] != "1" && pixel_row[x] != "E")
                {
                    if(x < 0)
                    {
                        break;
                    }

                    pixel_row[x] = "G";
                    x--;
                    walker.X = walker.X - 1;
                    MazeParser.color(m_maze_to_solve, walker);
                }
            }

            return is_finish;

        }

        /// <summary>
        /// Determines if the end is in the eastern vision of the walker and if so, sends the walker directly in that direction.
        /// </summary>
        /// <param name="walker">The walker.</param>
        /// <returns>
        /// A boolean value if the end is in the vicinity and also the walker with updated values.
        /// </returns>
        private bool is_eastfinish(MazeWalker walker)
        {
            bool is_finish = false;
            int y = walker.Y;
            int x = walker.X;

            if (y < 0 || x < 0 || y >= m_maze_to_solve.Count)
            {
                return false;
            }

            List<string> pixel_row = m_maze_to_solve[y];

            if (x >= pixel_row.Count)
            {
                return false;
            }

            while (pixel_row[x] != "1")
            {
                if (x >= pixel_row.Count)
                {
                    break;
                }
                if (pixel_row[x] == "E")
                {
                    is_finish = true;
                    break;
                }
                else
                {
                    x++;
                }
            }

            if(is_finish)
            {
                //finish the job
                x = walker.X;
                walker.Dir = Direction.East;
                while (pixel_row[x] != "1" && pixel_row[x] != "E")
                {
                    if (x >= pixel_row.Count)
                    {
                        break;
                    }

                    pixel_row[x] = "G";
                    x++;
                    walker.X = walker.X + 1;
                    MazeParser.color(m_maze_to_solve, walker);
                }
            }

            return is_finish;
        }

        /// <summary>
        /// Determines if the end is in the northern vision of the walker and if so, sends the walker directly in that direction.
        /// </summary>
        /// <param name="walker">The walker.</param>
        /// <returns>
        /// A boolean value if the end is in the vicinity and also the walker with updated values.
        /// </returns>
        private bool is_northfinish(MazeWalker walker)
        {
            bool is_finish = false;
            int y = walker.Y;
            int x = walker.X;

            if (y < 0 || x < 0 || y >= m_maze_to_solve.Count)
            {
                return false;
            }

            for(; y >= 0; y--)
            {
                List<string> pixel_row = m_maze_to_solve[y];

                if(pixel_row[x] == "1")
                {
                    break;
                }

                if(pixel_row[x] == "E")
                {
                    is_finish = true;
                    break;
                }
            }

            if(is_finish)
            {
                //finish the job
                walker.Dir = Direction.North;
                for (y=walker.Y; y >= 0; y--)
                {
                    List<string> pixel_row = m_maze_to_solve[y];

                    if (pixel_row[x] == "1")
                    {
                        break;
                    }

                    else if (pixel_row[x] == "E")
                    {
                        break;
                    }
                    else
                    {
                        pixel_row[x] = "X";
                    }
                    walker.Y = walker.Y - 1;
                    MazeParser.color(m_maze_to_solve, walker);
                }
            }

            return is_finish;
        }

        /// <summary>
        /// Determines if the end is in the southern vision of the walker and if so, sends the walker directly in that direction.
        /// </summary>
        /// <param name="walker">The walker.</param>
        /// <returns>
        /// A boolean value if the end is in the vicinity and also the walker with updated values.
        /// </returns>
        private bool is_southfinish(MazeWalker walker)
        {
            bool is_finish = false;
            int y = walker.Y;
            int x = walker.X;

            if (y < 0 || x < 0 || y >= m_maze_to_solve.Count)
            {
                return false;
            }

            for(; y < m_maze_to_solve.Count; y++)
            {
                List<string> pixel_row = m_maze_to_solve[y];

                if (pixel_row[x] == "1")
                {
                    break;
                }

                if (pixel_row[x] == "E")
                {
                    is_finish = true;
                    break;
                }
            }

            if (is_finish)
            {
                //finish the job
                walker.Dir = Direction.South;
                for (y = walker.Y; y < m_maze_to_solve.Count; y++)
                {
                    List<string> pixel_row = m_maze_to_solve[y];

                    if (pixel_row[x] == "1")
                    {
                        break;
                    }

                    else if (pixel_row[x] == "E")
                    {
                        break;
                    }
                    else
                    {
                        pixel_row[x] = "X";
                    }
                    walker.Y = walker.Y + 1;
                    MazeParser.color(m_maze_to_solve, walker);
                }
            }

            return is_finish;
        }

        /// <summary>
        /// Use function to set walker to the proper orientation and coordinates. The walker can be used to solve the maze.
        /// </summary>
        /// <param name="walker">The walker.</param>
        /// <returns>
        /// A walker where its direction and coordinates are properly set. The walker can be immediately used to solve the maze.
        /// </returns>
        private MazeWalker setorientation(MazeWalker walker)
        {
            //for now, don't do anything fancy. Just see if a direction is valid and arbitrarily choose one
            //for a direction to be valid, no walls should be in the vicinity of 10 pixels (10 is arbitrarily chosen)
            //and the direction with the most steps will be chosen

            int t_x = walker.X;
            int t_y = walker.Y;
            
            List<string> pixel_row_westeast = m_maze_to_solve[walker.Y];

            List<int> step_results = new List<int>();

            step_results.Add(find_weststeps(pixel_row_westeast, t_x, t_y));
            step_results.Add(find_eaststeps(pixel_row_westeast, t_x, t_y));
            step_results.Add(find_northsteps(m_maze_to_solve, t_x, t_y));
            step_results.Add(find_southsteps(m_maze_to_solve, t_x, t_y));

            int maxsteps = 0;
            Direction dir = Direction.North; //by default if there is no direction that provides the max steps

            for(int j = 0; j < step_results.Count; j++)
            {
                if(step_results[j] > maxsteps)
                {
                    maxsteps = step_results[j];
                    if(j == 0)
                    {
                        dir = Direction.West;
                    }
                    else if(j == 1)
                    {
                        dir = Direction.East;
                    }
                    else if(j == 2)
                    {
                        dir = Direction.North;
                    }
                    else
                    {
                        dir = Direction.South;
                    }
                }
            }//end for

            //at this point, we know the proper orientation of the walker
            
            
            return adjustwalker_toorientation(walker, dir, m_maze_to_solve);
        }

        private int find_weststeps(List<string> pixel_row_westeast, int t_x, int t_y)
        {
            int west_steps = 0;

            if(t_x >= pixel_row_westeast.Count)
            {
                return 0;
            }

            //check in west direction
            for (int j = 0; j < 10; j++)
            {
                t_x--;
                if (t_x < 0)
                {
                    break;
                }

                if (pixel_row_westeast[t_x] == "1")
                {
                    break;
                }
                else
                {
                    west_steps++;
                }
            }

            return west_steps;
        }

        private int find_eaststeps(List<string> pixel_row_westeast, int t_x, int t_y)
        {
            int east_steps = 0;

            if (t_x < 0)
            {
                return 0;
            }

            //check in east direction
            for (int j = 0; j < 10; j++)
            {
                t_x++;
                if (t_x >= pixel_row_westeast.Count)
                {
                    break;
                }

                if (pixel_row_westeast[t_x] == "1")
                {
                    break;
                }
                else
                {
                    east_steps++;
                }
            }

            return east_steps;
        }

        private int find_northsteps(List<List<string>> maze, int t_x, int t_y)
        {
            int north_steps = 0;

            if(t_y < 0 || t_y >= maze.Count)
            {
                return 0;
            }

            //check in north direction
            for(int j = 0; j < 10; j++)
            {
                t_y--;
                if(t_y < 0)
                {
                    break;
                }

                List<string> pixel_row = maze[t_y];
                
                if(pixel_row[t_x] == "1")
                {
                    break;
                }
                else
                {
                    north_steps++;
                }

            }

            return north_steps;
        }

        private int find_southsteps(List<List<string>> maze, int t_x, int t_y)
        {
            int south_steps = 0;

            if(t_y >= maze.Count || t_y < 0)
            {
                return 0;
            }

            //check in south direction
            for(int j = 0; j < 10; j++)
            {
                t_y++;
                if(t_y >= maze.Count)
                {
                    break;
                }

                List<string> pixel_row = maze[t_y];
                if(pixel_row[t_x] == "1")
                {
                    break;
                }
                else
                {
                    south_steps++;
                }

            }

            return south_steps;
        }

        /// <summary>
        /// Adjusts the walker so that it will be right along the wall. This function should only be used for the Wall Follower method.
        /// </summary>
        /// <param name="walker">The walker.</param>
        /// <param name="dir">The direction the walker should be facing.</param>
        /// <param name="maze">The maze.</param>
        /// <returns>The walker with the proper coordinates and direction.</returns>
        private MazeWalker adjustwalker_toorientation(MazeWalker walker, Direction dir, List<List<string>> maze)
        {
            int x = walker.X;
            int y = walker.Y;

            //Because the starting point is not just one pixel, we need to locate the closest wall where the walker can place a right hand on.
            //For example, if the direction is east, then the walker would need to travel to the first wall on the bottom of the drawn block.
            if(dir == Direction.East)
            {
                while(true)
                {
                    if(y+1 >= maze.Count)
                    {
                        break;
                    }
                    List<string> pixel_row = maze[y + 1];
                    if(pixel_row[x] == "1")
                    {
                        break;
                    }
                    else
                    {
                        y++;
                    }
                }
            }
            else if(dir == Direction.West)
            {
                while(true)
                {
                    if(y-1 < 0)
                    {
                        break;
                    }
                    List<string> pixel_row = maze[y - 1];
                    if(pixel_row[x] == "1")
                    {
                        break;
                    }
                    else
                    {
                        y--;
                    }
                }
            }
            else if(dir == Direction.North)
            {
                List<string> pixel_row = maze[y];
                while(true)
                {
                    if(x >= pixel_row.Count)
                    {
                        break;
                    }
                    
                    if(pixel_row[x+1] == "1")
                    {
                        break;
                    }
                    else
                    {
                        x++;
                    }
                }
            }
            else if (dir == Direction.South)
            {
                List<string> pixel_row = maze[y];
                while(true)
                {
                    if(x-1 < 0)
                    {
                        break;
                    }

                    if(pixel_row[x-1] == "1")
                    {
                        break;
                    }
                    else
                    {
                        x--;
                    }
                }
            }

            walker.X = x;
            walker.Y = y;
            walker.Dir = dir;
            return walker;
        }

        /// <summary>
        /// A wrapper function. Determines if the northern point of the current (x,y) is valid.
        /// </summary>
        /// <param name="x">The current x-coordinate.</param>
        /// <param name="y">The current y_coordinate.</param>
        /// <returns>
        /// If the northern point is valid.
        /// </returns>
        private bool is_northvalid(int x, int y)
        {
            if (x < 0 || y < 0 || y >= m_maze_to_solve.Count)
            {
                return false;
            }

            return is_locationvalid(x, y - 1);
        }

        private bool is_southvalid(int x, int y)
        {
            if (x < 0 || y < 0 || y >= m_maze_to_solve.Count)
            {
                return false;
            }

            return is_locationvalid(x, y + 1);
        }

        private bool is_westvalid(int x, int y)
        {
            if (x < 0 || y < 0 || y >= m_maze_to_solve.Count)
            {
                return false;
            }

            return is_locationvalid(x - 1, y);
        }

        private bool is_eastvalid(int x, int y)
        {
            if (x < 0 || y < 0 || y >= m_maze_to_solve.Count)
            {
                return false;
            }

            return is_locationvalid(x + 1, y);
        }

        /// <summary>
        /// Returns if the proposed x,y location is valid.
        /// </summary>
        /// <param name="x">The proposed x-coordinate.</param>
        /// <param name="y">The proposed y-coordinate.</param>
        /// <returns>A boolean value indicating if the proposed location is a valid spot to place the walker.
        /// </returns>
        private bool is_locationvalid(int x, int y)
        {
            if(x < 0 || y < 0 || y >= m_maze_to_solve.Count)
            {
                return false;
            }
            List<string> pixel_row = m_maze_to_solve[y];
            if(x >= pixel_row.Count)
            {
                return false;
            }

            if(pixel_row[x] == "1")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Move the walker to the proposed x_loc and y_loc. The move will also be noted on the maze.
        /// NOTE: the orientation of the walker is not adjusted.
        /// </summary>
        /// <param name="walker">The walker.</param>
        /// <param name="x_loc">The proposed x coordinate location.</param>
        /// <param name="y_loc">The proposed y coordinate location.</param>
        /// <returns>
        /// The walker with coordinates moved to the x and y locations. NOTE: the orientation of the walker is not changed.
        /// Returns NULL if the walker cannot move to that location.
        /// </returns>
        private MazeWalker movewalker(MazeWalker walker, int x_loc, int y_loc)
        {
            if(!is_locationvalid(x_loc, y_loc))
            {
                return null;
            }

            //adjust the value in the maze
            List<string> pixel_row = m_maze_to_solve[y_loc];
            pixel_row[x_loc] = "X";

            walker.X = x_loc;
            walker.Y = y_loc;
            return walker;
        }

        /// <summary>
        /// Moves the walker when it needs to turn a corner.
        /// Generally used to avoid the case when the walker needs to turn a corner but instead continues forward.
        /// </summary>
        /// <param name="walker">The walker.</param>
        /// <returns>
        /// The walker with proper orientation if it needs to turn a corner.
        /// </returns>
        private MazeWalker turncorner(MazeWalker walker, Direction curr_dir)
        {
            //since the walker is a little dumb, it needs to know if it should be turning a corner
            //the only corners will only be on the right-hand side of the direction the walker is facing towards (since we're applying right-hand follower)
            if(curr_dir == Direction.North)
            {
                //check if the east has an empty spot
                if(is_eastvalid(walker.X, walker.Y))
                {
                    walker.Dir = Direction.East;
                }
            }
            else if(curr_dir == Direction.South)
            {
                if(is_westvalid(walker.X, walker.Y))
                {
                    walker.Dir = Direction.West;
                }
            }
            else if(curr_dir == Direction.East)
            {
                if(is_southvalid(walker.X, walker.Y))
                {
                    walker.Dir = Direction.South;
                }
            }
            else if(curr_dir == Direction.West)
            {
                if(is_northvalid(walker.X, walker.Y))
                {
                    walker.Dir = Direction.North;
                }
            }

            return walker;
        }

        /// <summary>
        /// Moves the walker north. The orientation is adjusted.
        /// </summary>
        /// <param name="walker">The walker.</param>
        /// <returns>
        /// The walker with the coordinates and orientation adjusted.
        /// </returns>
        private MazeWalker movewalker_north(MazeWalker walker)
        {
            walker = movewalker(walker, walker.X, walker.Y - 1);
            if(walker == null)
            {
                return null;
            }
            else
            {
                walker.Dir = Direction.North;
            }
            return walker;
        }

        private MazeWalker movewalker_south(MazeWalker walker)
        {
            walker = movewalker(walker, walker.X, walker.Y + 1);
            if(walker == null)
            {
                return null;
            }
            else
            {
                walker.Dir = Direction.South;
            }
            return walker;
        }

        private MazeWalker movewalker_west(MazeWalker walker)
        {
            walker = movewalker(walker, walker.X - 1, walker.Y);
            if(walker == null)
            {
                return null;
            }
            else
            {
                walker.Dir = Direction.West;
            }
            return walker;
        }

        private MazeWalker movewalker_east(MazeWalker walker)
        {
            walker = movewalker(walker, walker.X + 1, walker.Y);
            if(walker == null)
            {
                return null;
            }
            else
            {
                walker.Dir = Direction.East;
            }
            return walker;
        }
    }
}
