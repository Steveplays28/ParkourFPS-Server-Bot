using System;
using System.Collections.Generic;
using System.Text;

namespace Discord_ParkourFPS_Bot
{
    public class Snake
    {
        //Snake game default variables
        public int canvas_height;
        public int canvas_width;
        public string canvas_emoji = "🟦";
        public string snake_emoji = "🟩";

        //2D array for width and height of the canvas
        public string[,] canvas_array;

        //2D array for all snake positions
        public string[,] snake_positions_array;

        //Snake game class
        public Snake(int Canvas_Height, int Canvas_Width)
        {
            //Check if canvas height and canvas with are less than 0, if so, throw an error
            if (Canvas_Height < 0)
            {
                throw new ArgumentException(nameof(canvas_height));
            }
            if (Canvas_Width < 0)
            {
                throw new ArgumentException(nameof(canvas_width));
            }

            //Set canvas width and canvas height
            canvas_height = Canvas_Height;
            canvas_width = Canvas_Width;

            //New canvas array
            canvas_array = new string[canvas_height, canvas_width];

            for (int i = 0; i < canvas_height; i++)
            {
                for (int j = 0; j < canvas_width; j++)
                {
                    canvas_array[i, j] = canvas_emoji;
                }
            }
        }

        //Canvas array to string
        public override string ToString()
        {
            var stringBuilder = new StringBuilder();

            for (int i = 0; i < canvas_height; i++)
            {
                for (int j = 0; j < canvas_width; j++)
                {
                    stringBuilder.Append(canvas_array[i, j]);
                }
                stringBuilder.Append(Environment.NewLine);
            }

            return stringBuilder.ToString();
        }
    }

}
