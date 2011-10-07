namespace SqlSpatial
{
    using System;
    using System.Drawing;
    using System.Runtime.CompilerServices;

    internal class ColorInterpolater
    {
        [CompilerGenerated]
        private Color[] <Colors>k__BackingField;
        [CompilerGenerated]
        private float[] <Positions>k__BackingField;

        internal ColorInterpolater()
        {
        }

        public ColorInterpolater(Color[] colors, float[] positions)
        {
            this.Colors = colors;
            this.Positions = positions;
        }

        public Color GetColor(float pos)
        {
            if (this.Colors.Length != this.Positions.Length)
            {
                throw new ArgumentException("Colors and Positions arrays must be of equal length");
            }
            if (this.Colors.Length < 2)
            {
                throw new ArgumentException("At least two colors must be defined in the ColorBlend");
            }
            if (this.Positions[0] != 0f)
            {
                throw new ArgumentException("First position value must be 0.0f");
            }
            if (this.Positions[this.Positions.Length - 1] != 1f)
            {
                throw new ArgumentException("Last position value must be 1.0f");
            }
            if (pos > 1f)
            {
                pos = 1f;
            }
            else if (pos < 0f)
            {
                pos = 0f;
            }
            int index = 1;
            while ((index < this.Positions.Length) && (this.Positions[index] < pos))
            {
                index++;
            }
            float num2 = (pos - this.Positions[index - 1]) / (this.Positions[index] - this.Positions[index - 1]);
            int red = (int) Math.Round((double) ((this.Colors[index - 1].R * (1f - num2)) + (this.Colors[index].R * num2)));
            int green = (int) Math.Round((double) ((this.Colors[index - 1].G * (1f - num2)) + (this.Colors[index].G * num2)));
            int blue = (int) Math.Round((double) ((this.Colors[index - 1].B * (1f - num2)) + (this.Colors[index].B * num2)));
            int alpha = (int) Math.Round((double) ((this.Colors[index - 1].A * (1f - num2)) + (this.Colors[index].A * num2)));
            return Color.FromArgb(alpha, red, green, blue);
        }

        public Color[] Colors
        {
            [CompilerGenerated]
            get
            {
                return this.<Colors>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<Colors>k__BackingField = value;
            }
        }

        public float[] Positions
        {
            [CompilerGenerated]
            get
            {
                return this.<Positions>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<Positions>k__BackingField = value;
            }
        }

        public static ColorInterpolater Rainbow5
        {
            get
            {
                return new ColorInterpolater(new Color[] { Color.Red, Color.Yellow, Color.Green, Color.Cyan, Color.Blue }, new float[] { 0f, 0.25f, 0.5f, 0.75f, 1f });
            }
        }

        public static ColorInterpolater Rainbow7
        {
            get
            {
                ColorInterpolater interpolater = new ColorInterpolater();
                interpolater.Positions = new float[7];
                for (int i = 1; i < 7; i++)
                {
                    interpolater.Positions[i] = ((float) i) / 6f;
                }
                interpolater.Colors = new Color[] { Color.Red, Color.Orange, Color.Yellow, Color.Green, Color.Blue, Color.Indigo, Color.Violet };
                return interpolater;
            }
        }
    }
}

