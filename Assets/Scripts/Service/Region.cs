using UnityEngine;
using System.Collections.Generic;

namespace Game.Service
{
    public class Region
    {
        #region Props
        public float X
        {
            get => x;
            set
            {
                x = value;
                right = value + width;
            }
        }
        public float Y
        {
            get => y;
            set
            {
                y = value;
                bottom = value + height;
            }
        }
        public float Z
        {
            get => z;
            set
            {
                z = value;
                front = value + length;
            }
        }

        public float Width
        {
            get => width;
            set
            {
                width = value;
                right = x + value;
            }
        }
        public float Height
        {
            get => height;
            set
            {
                height = value;
                bottom = y + value;
            }
        }
        public float Length
        {
            get => length;
            set
            {
                length = value;
                front = z + value;
            }
        }

        public float Bottom
        {
            get => bottom;
        }
        public float Right
        {
            get => right;
        }
        public float Front
        {
            get => front;
        }

        #region Hide
        private float x;
        private float y;
        private float z;

        private float right;
        private float bottom;
        private float front;

        private float width;
        private float height;
        private float length;
        #endregion
        #endregion

        #region Constructors
        public Region(Vector3 center, Vector3 size)
        {
            Vector3 half_size = size * 0.5F;

            X = center.x - half_size.x;
            Y = center.y - half_size.y;
            Z = center.z - half_size.z;

            Width = size.x;
            Height = size.y;
            Length = size.z;
        }

        public Region(float Width, float Height, float Length)
        {
            this.Width = Width;
            this.Height = Height;
            this.Length = Length;
        }

        public Region(float X, float Y, float Z, float Width, float Height, float Length)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;

            this.Width = Width;
            this.Height = Height;
            this.Length = Length;
        }
        #endregion

        #region Methods
        public bool Contains(Vector3 point)
        {
            return Contains(point.x, point.y, point.z);
        }

        public bool Contains(float x, float z)
        {
            return
                x >= this.x && x <= right &&
                z >= this.z && z <= front;
        }

        public bool Contains(float x, float y, float z)
        {
            return
                x >= this.x && x <= right &&
                y >= this.y && y <= bottom &&
                z >= this.z && z <= front;
        }

        public IEnumerable<Region> GetSubRegions(float width, float height, float length)
        {
            int region_count_x = (int)(this.width / width);
            int region_count_y = (int)(this.height / height);
            int region_count_z = (int)(this.length / length);

            for (int region_x = 0; region_x < region_count_x; region_x++)
            {
                for (int region_y = 0; region_y < region_count_y; region_y++)
                {
                    for (int region_z = 0; region_z < region_count_z; region_z++)
                    {
                        yield return new Region(x + width * region_x, y + height * region_y, z + length * region_z, width, height, length);
                    }
                }
            }
        }
        #endregion
    }
}