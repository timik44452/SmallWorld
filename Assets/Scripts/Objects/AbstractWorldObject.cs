using Objects;
using UnityEngine;
using System.Collections.Generic;

public abstract class AbstractWorldObject : MonoBehaviour
{
    private List<WorldObjectProperty> properties;

    public AbstractWorldObject()
    {
        properties = new List<WorldObjectProperty>();
    }

    public object GetPropertyValue(string name)
    {
        WorldObjectProperty property = properties.Find(x => x.name == name);

        if(property != null)
        {
            return property.value;
        }

        return null;
    }

    public void SetPropertyValue(string name, object value)
    {
        WorldObjectProperty property = properties.Find(x => x.name == name);

        if(property != null)
        {
            property.value = value;
        }
    }
}
