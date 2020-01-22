namespace Objects
{
    public interface IWorldObjectFactory
    {
        AbstractWorldObject CreateObject(string objectText);
    }
}
