using System.IO;

namespace Game.Terrain.Service
{
    public class TerrainRegion
    {
        public int X { get; }
        public int Z { get; }
        public int Width { get; }
        public int Length { get; }

        private byte[] heights;

        public TerrainRegion(int X, int Z, int Width, int Length)
        {
            this.X = X;
            this.Z = Z;
            this.Width = Width;
            this.Length = Length;

            heights = new byte[Width* Length];
        }

        public TerrainRegion(string path)
        {
            string region_name = Path.GetFileNameWithoutExtension(path);
            string region_size = region_name.Replace("region", string.Empty);

            int.TryParse(region_size.Split('_')[0], out int x);
            int.TryParse(region_size.Split('_')[1], out int z);

            using (Stream reader = new StreamReader(path).BaseStream)
            {
                int size = (int)System.Math.Sqrt(reader.Length);

                Width = size;
                Length = size;

                heights = new byte[reader.Length];

                reader.Read(heights, 0, heights.Length);
            }

            X = x;
            Z = z;
        }

        public float GetHeight(int x, int y)
        {
            if (x >= 0 && x < Width && y >= 0 && y < Length)
            {
                return heights[x + y * Width];
            }

            return 0;
        }

        public void SetHeight(int x, int y, float height)
        {
            if (x >= 0 && x < Width && y >= 0 && y < Length)
            {
                heights[x + y * Width] = (byte)height;
            }
        }

        public void Save(string path)
        {
            using (Stream writer = new StreamWriter(path).BaseStream)
            {
                writer.Write(heights, 0, heights.Length);
            }
        }
    }
}
