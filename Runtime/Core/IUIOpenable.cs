namespace Core
{
    public interface IUIOpenable
    {
        bool TryClose();
        void ForceClose();
        void OnFocus();
        void OnFocusLost();
        bool OnBackButtonEventUsed();
    }
}