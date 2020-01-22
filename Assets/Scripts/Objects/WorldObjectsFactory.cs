using UnityEngine;

namespace Objects
{
    public class WorldObjectsFactory : IWorldObjectFactory
    {
        public AbstractWorldObject CreateObject(string objectString)
        {
            GameObject gameObject = new GameObject();

            WorldObject worldObject = gameObject.AddComponent<WorldObject>();

            foreach (string line in objectString.Split(';'))
            {
                var property = PropertyParser.GetProperty(line);

                worldObject.SetPropertyValue(property.name, property.value);
            }

            return worldObject;
        }
    }
}
