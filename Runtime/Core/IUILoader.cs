namespace com.gbviktor.UIManager.Core
{
    public interface IUILoader
    {
        T LoadScreen<T>() where T : UIScreen;
        void ReleaseAssets();
    }
}