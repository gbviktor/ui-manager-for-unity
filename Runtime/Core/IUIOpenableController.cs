namespace com.gbviktor.UIManager.Core
{
    public interface IUIOpenableController
    {
        void CloseCurrentFocusPrevious(); // call ForceClose() on current, call OnFocus() previous        
        void ForceCloseAll(); //call ForceClose on all nestedList
        /// <summary>
        /// Called if user press Back Button. <br></br>
        /// Do all what you want 
        /// </summary>
        /// <returns>return false if event not Used</returns>
        bool OnBackButtonEventUsed(); //call at first OnBackButtonEventUsed() from nestedList
    }
}