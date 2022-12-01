namespace com.gbviktor
{
    public interface IUIScreensList
    {
        object PathToResourcesFolder { get; }
        string GetGameObjectNameByScreenTypeName(string name);
    }
}