using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.IO;

namespace maze
{
    static class Constants
    {
        public const int COLOR_WIDTH = 5;
    }
    public class MazeParser
    {
        Bitmap m_maze;
        List<List<string>> m_pixelsrow; //stores the value of each pixel in a jagged array. Each index in the (outer) list represents a row in the bitmap

        public MazeParser(Bitmap maze)
        {
            m_maze = maze;
            m_pixelsrow = new List<List<string>>();
        }

        public List<List<string>> Parsed_Map
        {
            get
            {
                return m_pixelsrow;
            }
            set
            {
                m_pixelsrow = value;
            }
        }

        /// <summary>
        /// Parses the bitmap according to the following guidelines:
        /// white pixel - 0
        /// black pixel - 1
        /// red pixel   - S
        /// blue pixel  - E
        /// </summary>
        /// <returns>
        /// A jagged list showing the resulting bitmap interpreted as string characters.
        /// </returns>
        public List<List<string>> parse_map()
        {
            List<List<string>> parsed_map = parse_map(m_maze);
            return parsed_map;
        }

        /// <summary>
        /// Parses the bitmap according to the following guidelines:
        /// white pixel - 0
        /// black pixel - 1
        /// red pixel   - S
        /// blue pixel  - E
        /// </summary>
        /// <param name="maze">The maze that needs to be parsed as a Bitmap</param>
        /// <returns>
        /// A jagged list showing the resulting bitmap interpreted as string characters.
        /// </returns>
        public List<List<string>> parse_map(Bitmap maze)
        {
            List<List<string>> pixels_row = new List<List<string>>(); //the list that stores the rows of the image, i.e. the height
            int width = maze.Width;
            int height = maze.Height;

            //create the number of rows according to the height of the bitmap
            for(int i = 0; i < maze.Height; i++)
            {
                List<string> pixel_row = new List<string>(); //these pixels are the width of the image
                pixels_row.Add(pixel_row);
            }

            //begin parsing each row of the maze
            for(int y = 0; y < height; y++ )
            {
                for(int x = 0; x < width; x++)
                {
                    Color pixel_color = maze.GetPixel(x, y);
                    List<string> curr_row = pixels_row[y];
                    if(pixel_color.ToArgb() == Color.Black.ToArgb())
                    {
                        curr_row.Add("1");
                    }
                    else if(pixel_color.ToArgb() == Color.White.ToArgb())
                    {
                        curr_row.Add("0");
                    }
                    else if(pixel_color.ToArgb() == Color.Red.ToArgb())
                    {
                        curr_row.Add("S");
                    }
                    else if(pixel_color.ToArgb() == Color.Blue.ToArgb())
                    {
                        curr_row.Add("E");
                    }
                    else
                    {
                        curr_row.Add("-1");
                    }
                }
            }

            //output_parse(pixels_row); //for debugging
            m_pixelsrow = pixels_row;
            return pixels_row;
        }

        /// <summary>
        /// Parses the solved_maze into the original maze and outputs a bmp file.
        /// </summary>
        /// <param name="filename">The filename of the solved maze.</param>
        /// <param name="original_maze">The bitmap of the original maze.</param>
        /// <param name="solved_maze">The parsed version of the solved maze.</param>
        public void set_maze(string filename, Bitmap original_maze, List<List<string>> solved_maze)
        {
            Bitmap b_solvedmaze = new Bitmap(original_maze);
            
            for(int y = 0; y < b_solvedmaze.Height; y++)
            {
                List<string> pixel_row = solved_maze[y];
                for(int x = 0; x < b_solvedmaze.Width; x++)
                {
                    string pixel = pixel_row[x];
                    
                    //we only change the curr_pixel if pixel is green (marked as X)
                    if(pixel == "X")
                    {
                        b_solvedmaze.SetPixel(x, y, Color.Green);
                    }
                }//end for
            }//end for

            b_solvedmaze.Save(filename);
        }

        /// <summary>
        /// Used for debugging purposes only.
        /// </summary>
        /// <param name="maze_parsed"></param>
        public static void output_parse(List<List<string>> maze_parsed)
        {
            using (StreamWriter writer = new StreamWriter("parsed_output.txt"))
            {
                for(int i = 0; i < maze_parsed.Count; i++)
                {
                    List<string> curr_row = maze_parsed[i];
                    for(int j = 0; j < curr_row.Count; j++)
                    {
                        writer.Write(curr_row[j]);
                    }
                    writer.Write(Environment.NewLine);
                }
            }//end using
        }

        /// <summary>
        /// Colors the walker's surrounding with green.
        /// </summary>
        /// <param name="maze">The maze.</param>
        /// <param name="walker">The walker.</param>
        public static void color(List<List<string>> maze, MazeWalker walker)
        {
            int y = walker.Y;
            int x = walker.X;

            if (y < 0 || y >= maze.Count || x < 0)
            {
                return;
            }

            if (walker.Dir == Direction.North || walker.Dir == Direction.South)
            {
                //color west direction

                List<string> pixel_row = maze[y];
                for(int i = 0; i < Constants.COLOR_WIDTH; i++)
                {
                    if(x < 0 || x >= pixel_row.Count)
                    {
                        break;
                    }
                    if(pixel_row[x] == "1")
                    {
                        break;
                    }
                    else
                    {
                        pixel_row[x] = "X";
                        x--;
                    }
                   
                }

                //color in east direction
                x = walker.X;
                for(int i = 0; i < Constants.COLOR_WIDTH; i++)
                {
                    if(x < 0 || x >= pixel_row.Count)
                    {
                        break;
                    }
                    if(pixel_row[x] == "1")
                    {
                        break;
                    }
                    else
                    {
                        pixel_row[x] = "X";
                        x++;
                    }
                }
            }

            else if(walker.Dir == Direction.East || walker.Dir == Direction.West)
            {
                //color south direction
                for(int i = 0; i < Constants.COLOR_WIDTH; i++)
                {
                    if(y < 0 || y >= maze.Count)
                    {
                        break;
                    }
                    List<string> pixel_row = maze[y];
                    if(pixel_row[x] == "1")
                    {
                        break;
                    }
                    else
                    {
                        pixel_row[x] = "X";
                        y++;
                    }
                }

                //color in north direction
                y = walker.Y;
                for (int i = 0; i < Constants.COLOR_WIDTH; i++)
                {
                    if (y < 0 || y >= maze.Count)
                    {
                        break;
                    }
                    List<string> pixel_row = maze[y];
                    if(pixel_row[x] == "1")
                    {
                        break;
                    }
                    else
                    {
                        pixel_row[x] = "X";
                        y--;
                    }
                }

            }
            else
            {
                //should never get here
                return;
            }
        }
    }
}
