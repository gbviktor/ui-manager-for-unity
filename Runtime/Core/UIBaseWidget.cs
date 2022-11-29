using UnityEngine;

namespace Core
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UIBaseWidget : UIBaseElement, IUIOpenable
    {
        protected bool isInitialized;
        public CanvasGroup canvasGroup;
        public bool isShowed;

        /// <summary>
        /// Called once at time when you call Show();  before OnShow();
        /// </summary>
        protected virtual void Init()
        {

        }

        protected virtual void OnShow()
        {

        }
        protected virtual void OnHide()
        {

        }

        public void ShowOrHIde()
        {
            if (isShowed)
            {
                Hide();
            } else
            {
                Show();
            }
        }

        public void Show()
        {
            if (isShowed) return;

            ShowWitoutNotify();
            OnShow();
        }

        protected void ShowWitoutNotify()
        {
            if (!isInitialized)
            {
                isInitialized = true;
                Init();
            }
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1;
            isShowed = true;
        }

        public void Hide()
        {
            OnHide();
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0;
            isShowed = false;
        }

        private void OnValidate()
        {
            if (canvasGroup == null)
                canvasGroup = this.GetComponent<CanvasGroup>();
        }

        protected void EnableCanvasUIInput()
        {
            canvasGroup.interactable = true;
        }

        protected void DisableCanvasUIInput()
        {
            canvasGroup.interactable = false;
        }
        public void CloseCurrentFocusPrevious()
        {
            OpenableController.CloseCurrentFocusPrevious();
        }


        #region IUIOpenable implementation
        protected IUIOpenableController OpenableController { get; set; }
        public virtual bool TryClose()
        {
            ForceClose();
            return true;
        }
        public virtual void ForceClose()
        {
            Hide();
        }
        public virtual void OnFocus()
        {
            Show();
        }
        public virtual void OnFocusLost()
        {
            Hide();
        }
        /// <summary>
        /// Called if user press Back Button <br></br>
        /// hide UIBaseWidget, and previous will be Focused  <br></br>
        /// execute OpenableQueueController.CloseCurrentFocusPrevios(); 
        /// </summary>
        /// <returns>return false if event not Used</returns>
        public virtual bool OnBackButtonEventUsed()
        {
            return false;
        }
        #endregion
    }


}