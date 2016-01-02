using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.IO;

namespace maze
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length != 2)
            {
                System.Console.WriteLine("Usage: maze.exe source.[bmp,jpg,png] destination.[bmp,jpg,png]");
            }

            Bitmap b_source;
            Image im_source;
            try
            {
                im_source = Image.FromFile(args[0]);
                b_source = new Bitmap(im_source);

            }
            catch (ArgumentException)
            {
                System.Console.WriteLine("Check the file path...something went wrong.");
                return;
            }

            MazeParser my_parser = new MazeParser(b_source);
            my_parser.parse_map();

            MazeSolver my_solver = new MazeSolver(my_parser.Parsed_Map);
            List<List<string>> maze_solution = my_solver.solve_maze(Solving_Method.Right_hand);
            if (maze_solution != null)
            {
                my_parser.set_maze(args[1], b_source, maze_solution);
            }
            else
            {
                System.Console.WriteLine("A solution could not be found!! Check to make sure your bmp is not corrupted...");
            }

        }

        void start(string source, string destination)
        {
            
        }
    }
}
