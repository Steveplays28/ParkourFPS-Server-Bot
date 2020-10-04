using System;
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

        public event EventHandler SnakeChanged;

        //2D array for width and height of the canvas
        private string[,] canvas_array;

        public (int row, int column) CurrentHeadPos { get; private set; }
        public bool IsPlaying { get; private set; }

        //Snake game class
        public Snake(int Canvas_Width, int Canvas_Height)
        {
            //Check if canvas height and canvas with are less than 0, if so, throw an error
            if (Canvas_Width < 0)
            {
                throw new ArgumentException(nameof(canvas_width));
            }

            if (Canvas_Height < 0)
            {
                throw new ArgumentException(nameof(canvas_height));
            }

            //Set canvas width and canvas height
            canvas_height = Canvas_Height;
            canvas_width = Canvas_Width;

            //New canvas array
            canvas_array = new string[canvas_width, canvas_height];
        }

        public string Initialize()
        {
            FillDefaultEmoji();

            SetRandomStartPosition();

            IsPlaying = true;

            return ToString();
        }

        private void FillDefaultEmoji()
        {
            for (int i = 0; i < canvas_height; i++)
            {
                for (int j = 0; j < canvas_width; j++)
                {
                    canvas_array[i, j] = canvas_emoji;
                }
            }
        }

        private void SetRandomStartPosition()
        {
            Random r = new Random();
            //Random snake position
            var row = r.Next(canvas_height - 1);
            var column = r.Next(canvas_width - 1);

            CurrentHeadPos = (row, column);

            canvas_array[row, column] = snake_emoji;
        }

        protected virtual void OnSnakeChanged(EventArgs e)
        {
            EventHandler handler = SnakeChanged;
            handler?.Invoke(this, e);
        }

        public void GoLeft()
        {
            var row = CurrentHeadPos.row;
            var column = CurrentHeadPos.column;

            if (column > 0)
            {
                canvas_array[row, column] = canvas_emoji;

                column -= 1;
                canvas_array[row, column] = snake_emoji;
                CurrentHeadPos = (row, column);

                OnSnakeChanged(new EventArgs());
            }
        }

        public void GoRight()
        {
            var row = CurrentHeadPos.row;
            var column = CurrentHeadPos.column;

            if (column < canvas_width - 1)
            {
                canvas_array[row, column] = canvas_emoji;

                column += 1;
                canvas_array[row, column] = snake_emoji;
                CurrentHeadPos = (row, column);

                OnSnakeChanged(new EventArgs());
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
