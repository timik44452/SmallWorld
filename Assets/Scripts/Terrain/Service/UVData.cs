namespace Game.Terrain.Service
{
    public struct UVData
    {
        public int ID;

        public float x;
        public float y;
        public float width;
        public float height;

        public UVData(int ID, float x, float y, float width, float height)
        {
            this.ID = ID;
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }
    }
}
