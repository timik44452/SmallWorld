using UnityEngine;
using System.Runtime.InteropServices;
using System;

namespace Game.Service
{
    public class CubePoly
    {
        public Array Vertices { get; }

        public Vector3[] Normals { get; }

        public Vector2[] UVs { get; }

        public int[] Triangles { get; }

        public CubePoly(Array vertices)
        {
            Vertices = vertices;

            UVs = new Vector2[20];

            Triangles = new int[30];

            Normals = new Vector3[20]
            {
                Vector3.up,
                Vector3.up,
                Vector3.up,
                Vector3.up,

                Vector3.right,
                Vector3.right,
                Vector3.right,
                Vector3.right,

                Vector3.left,
                Vector3.left,
                Vector3.left,
                Vector3.left,

                Vector3.forward,
                Vector3.forward,
                Vector3.forward,
                Vector3.forward,

                Vector3.back,
                Vector3.back,
                Vector3.back,
                Vector3.back
            };

            // 1 -------- 2
            // |          |
            // |    up    |
            // |          |
            // 0 -------- 3

            // 5 ---------- 6     9 --------- 10
            // |            |     |            |
            // |    right   |     |    left    |
            // |            |     |            |
            // 4 ---------- 7     8 --------- 11

            // 13 --------- 14     17 -------- 18
            // |            |      |            |
            // |    front   |      |    back    |
            // |            |      |            |
            // 12 --------- 15     16 -------- 19
        }

        public void Move(int index, Vector3 position)
        {
            Vector3 up = Vector3.up * 0.5F;
            Vector3 right = Vector3.right * 0.5F;
            Vector3 forward = Vector3.forward * 0.5F;
            
            
            //Vertices.SetValue(position + up - right - forward, index);
            //Vertices.SetValue(position + up - right + forward, index + 1);
            //Vertices.SetValue(position + up + right + forward, index + 2);
            //Vertices.SetValue(position + up + right - forward, index + 3);

            //Vertices.SetValue(position - up + right + forward, index + 4);
            //Vertices.SetValue(position + up + right + forward, index + 5);
            //Vertices.SetValue(position + up + right - forward, index + 6);
            //Vertices.SetValue(position - up + right - forward, index + 7);

            //Vertices.SetValue(position - up - right + forward, index + 8);
            //Vertices.SetValue(position + up - right + forward, index + 9);
            //Vertices.SetValue(position + up - right - forward, index + 10);
            //Vertices.SetValue(position - up - right - forward, index + 11);

            //Vertices.SetValue(position - up - right + forward, index + 12);
            //Vertices.SetValue(position + up - right + forward, index + 13);
            //Vertices.SetValue(position + up + right + forward, index + 14);
            //Vertices.SetValue(position - up + right + forward, index + 15);

            //Vertices.SetValue(position - up - right - forward, index + 16);
            //Vertices.SetValue(position + up - right - forward, index + 17);
            //Vertices.SetValue(position + up + right - forward, index + 18);
            //Vertices.SetValue(position - up + right - forward, index + 19);

            //Vertices[0] = position + up - right - forward;
            //Vertices[1] = position + up - right + forward;
            //Vertices[2] = position + up + right + forward;
            //Vertices[3] = position + up + right - forward;

            //Vertices[4] = position - up + right + forward;
            //Vertices[5] = position + up + right + forward;
            //Vertices[6] = position + up + right - forward;
            //Vertices[7] = position - up + right - forward;

            //Vertices[8] = position - up - right + forward;
            //Vertices[9] = position + up - right + forward;
            //Vertices[10] = position + up - right - forward;
            //Vertices[11] = position - up - right - forward;

            //Vertices[12] = position - up - right + forward;
            //Vertices[13] = position + up - right + forward;
            //Vertices[14] = position + up + right + forward;
            //Vertices[15] = position - up + right + forward;

            //Vertices[16] = position - up - right - forward;
            //Vertices[17] = position + up - right - forward;
            //Vertices[18] = position + up + right - forward;
            //Vertices[19] = position - up + right - forward;
        }

        public void SetIndexOffset(int indexOffset)
        {
            #region up
            Triangles[0] = indexOffset;
            Triangles[1] = indexOffset + 1;
            Triangles[2] = indexOffset + 2;

            Triangles[3] = indexOffset;
            Triangles[4] = indexOffset + 2;
            Triangles[5] = indexOffset + 3;
            #endregion

            #region left
            Triangles[6] = indexOffset + 8;
            Triangles[7] = indexOffset + 9;
            Triangles[8] = indexOffset + 10;

            Triangles[9] = indexOffset + 8;
            Triangles[10] = indexOffset + 10;
            Triangles[11] = indexOffset + 11;
            #endregion

            #region front
            Triangles[12] = indexOffset + 15;
            Triangles[13] = indexOffset + 14;
            Triangles[14] = indexOffset + 13;

            Triangles[15] = indexOffset + 15;
            Triangles[16] = indexOffset + 13;
            Triangles[17] = indexOffset + 12;
            #endregion

            #region right
            Triangles[18] = indexOffset + 7;
            Triangles[19] = indexOffset + 6;
            Triangles[20] = indexOffset + 5;

            Triangles[21] = indexOffset + 7;
            Triangles[22] = indexOffset + 5;
            Triangles[23] = indexOffset + 4;
            #endregion

            #region back
            Triangles[24] = indexOffset + 16;
            Triangles[25] = indexOffset + 17;
            Triangles[26] = indexOffset + 18;

            Triangles[27] = indexOffset + 16;
            Triangles[28] = indexOffset + 18;
            Triangles[29] = indexOffset + 19;
            #endregion
        }

        public void SetUV(Vector4 uv)
        {
            UVs[0] = new Vector2(uv.x, uv.y);
            UVs[1] = new Vector2(uv.x + uv.z, uv.y);
            UVs[2] = new Vector2(uv.x + uv.z, uv.y + uv.w);
            UVs[3] = new Vector2(uv.x, uv.y + uv.w);

            UVs[4] = new Vector2(uv.x, uv.y);
            UVs[5] = new Vector2(uv.x + uv.z, uv.y);
            UVs[6] = new Vector2(uv.x + uv.z, uv.y + uv.w);
            UVs[7] = new Vector2(uv.x, uv.y + uv.w);

            UVs[8] = new Vector2(uv.x, uv.y);
            UVs[9] = new Vector2(uv.x + uv.z, uv.y);
            UVs[10] = new Vector2(uv.x + uv.z, uv.y + uv.w);
            UVs[11] = new Vector2(uv.x, uv.y + uv.w);

            UVs[12] = new Vector2(uv.x, uv.y);
            UVs[13] = new Vector2(uv.x + uv.z, uv.y);
            UVs[14] = new Vector2(uv.x + uv.z, uv.y + uv.w);
            UVs[15] = new Vector2(uv.x, uv.y + uv.w);

            UVs[16] = new Vector2(uv.x, uv.y);
            UVs[17] = new Vector2(uv.x + uv.z, uv.y);
            UVs[18] = new Vector2(uv.x + uv.z, uv.y + uv.w);
            UVs[19] = new Vector2(uv.x, uv.y + uv.w);
        }
    }
}
