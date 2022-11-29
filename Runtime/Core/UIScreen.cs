using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace Core
{
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster))]
    [DisallowMultipleComponent]
    public class UIScreen : MonoBehaviour, IUIOpenable, IUIOpenableController
    {
        [SerializeField]
        protected bool isOpened;

        public virtual UIScreen Open(params object[] param)
        {
            OnOpen(param);
            return this;
        }
        protected virtual void OnOpen(params object[] param)
        {

        }

#if UNITY_EDITOR
        //TODO make this settings throw global Scriptable Object
        //private void OnValidate()
        //{
        //    GetComponent<CanvasScaler>().referenceResolution = referenceResolution;
        //}
#endif

        /// -----------------------------------------------
        /// --------- IUIOpenable implementation
        /// -----------------------------------------------

        #region IUIOpenable implementation

        protected IUIOpenableController OpenableController { get; set; }

        protected virtual void OnWidgetOpen(UIBaseWidget widget)
        {

        }
        public virtual bool TryClose()
        {
            if (openedWidgetsStack.Count == 0)
            {
                ForceClose();
                return true;
            }

            //Try Close opened Widgets if exist
            if (openedWidgetsStack.Count > 0)
            {
                var widget = openedWidgetsStack.Peek();
                if (widget != null && !widget.TryClose())
                {
                    //Try Close: blocked pass Event parents.
                    //Event was used by widget
                    return false;
                }
                openedWidgetsStack.Pop();
                return false;
            }

            if (openedWidgetsStack.Count == 0)
            {
                OnFocus();
                return false;
            }

            if (openedWidgetsStack.Count > 0)
            {
                var nextOpenable = openedWidgetsStack.Peek();
                nextOpenable.OnFocus();
                return false;
            }

            return true;
        }

        public virtual void ForceClose()
        {
            try
            {
                Destroy(gameObject);
            } catch { }
        }

        public virtual void OnFocus()
        {
            gameObject.SetActive(true);

            if (openedWidgetsStack.Count > 0)
            {
                var nextOpenable = openedWidgetsStack.Peek();
                nextOpenable.OnFocus();
            }


        }
        public virtual void OnFocusLost()
        {
            if (openedWidgetsStack.Count > 0)
            {
                var nextOpenable = openedWidgetsStack.Peek();
                nextOpenable.OnFocusLost();
            }

            gameObject.SetActive(false);
        }
        /// <summary>
        /// Called if user press Back Button <br></br>
        /// current UIScreen closed, and previous will be Focused  <br></br>
        /// execute OpenableQueueController.CloseCurrentFocusPrevios(); 
        /// </summary>
        /// <returns>return false if event not Used</returns>
        public virtual bool OnBackButtonEventUsed()
        {
            //TODO swicth to Thread Pool before call
            return OnBackButtonEventUsedByWidgets();
        }


        #endregion


        /// -----------------------------------------------
        /// --------- IUIOpenable extensions
        /// -----------------------------------------------
        #region IUIOpenable extensions
        public bool OnBackButtonEventUsedByWidgets()
        {
            if (openedWidgetsStack.Count > 0)
            {
                var nextOpenable = openedWidgetsStack.Peek();

                if (nextOpenable != null)
                {
                    if (nextOpenable.OnBackButtonEventUsed())
                    {
                        return true;
                    }

                    ((IUIOpenableController)this).CloseCurrentFocusPrevious();
                    return true;
                }
            }

            return false;
        }

        public void OpenNested(UIBaseWidget widget)
        {
            OpenNestedUnfocusPrevious(widget);
        }
        public void OpenAndForceCloseAllOthers(UIBaseWidget widget)
        {
            ForceCloseAllWidgets();
            OpenNested(widget);
        }
        #endregion

        /// -----------------------------------------------
        ///    IUIOpenableQueueController implementation
        /// -----------------------------------------------
        #region IUIOpenableQueueController implementation
        protected void ForceCloseAllWidgets()
        {
            var nestedCount = openedWidgetsStack.Count;
            for (int i = nestedCount; i > 0; i--)
            {
                var openable = openedWidgetsStack.Pop();
                if (openable != null)
                    openable.ForceClose();
            }
        }

        protected readonly Stack<IUIOpenable> openedWidgetsStack = new Stack<IUIOpenable>();
        void OpenNestedUnfocusPrevious(IUIOpenable openable)
        {
            if (openedWidgetsStack.Count > 0)
            {
                var nextOpenable = openedWidgetsStack.Peek();
                nextOpenable.OnFocusLost();
            }

            if (openable is UIScreen)
            {
                Debug.LogError($"OpenNested: Can't open UIScreen nested in {name} from widget");
                return;
            }

            openedWidgetsStack.Push(openable);
            openable.OnFocus();
            OnWidgetOpen(openable as UIBaseWidget);
        }

        void IUIOpenableController.ForceCloseAll()
        {
            ForceCloseAllWidgets();
        }
        void IUIOpenableController.CloseCurrentFocusPrevious()
        {
            IUIOpenable openedWidget = null;

            //manage Widgets
            if (openedWidgetsStack.Count > 0)
            {
                openedWidget = openedWidgetsStack.Peek();

                if (openedWidget != null && !openedWidget.TryClose())
                {
                    return;
                }
                openedWidgetsStack.Pop();
            }

            if (openedWidgetsStack.Count == 0)
            {
                OnFocus();
                return;
            }

            if (openedWidgetsStack.Count > 0)
            {
                var nextOpenable = openedWidgetsStack.Peek();
                nextOpenable.OnFocus();
            }
        }
        #endregion
    }
}