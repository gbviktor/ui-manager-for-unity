namespace com.gbviktor.UIManager.Core
{
    public interface IUIScreensList
    {
        object PathToResourcesFolder { get; }
        string GetGameObjectNameByScreenTypeName(string name);
    }
}