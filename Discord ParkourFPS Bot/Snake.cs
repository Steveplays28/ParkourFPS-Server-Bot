using System;
using System.Collections.Generic;
using System.Text;

namespace Discord_ParkourFPS_Bot
{
    public class Snake
    {
        public int Height { get; private set; }
        public int Width { get; private set; }
        public string Emoji { get; private set; }

        public Snake(int height, int width, string emoji)
        {
            if (height < 0)
            {
                throw new ArgumentException(nameof(height));
            }
            if (width < 0)
            {
                throw new ArgumentException(nameof(width));
            }

            Height = height;
            Width = width;
            Emoji = emoji;
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();

            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    stringBuilder.Append(Emoji);
                }
                stringBuilder.Append(Environment.NewLine);
            }

            return stringBuilder.ToString();
        }
    }

}
