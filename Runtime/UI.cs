using System.Collections.Generic;

using com.gbviktor.UIManager.Core;

using UnityEngine;

namespace com.gbviktor.UIManager
{
    public class UI : IUIOpenableController
    {
        public static UI ins;
        private static IUIOpenableController insNestingController;
        private static ReferencesLocator<UIScreen> locator = new ReferencesLocator<UIScreen>();

        private static IUILoader loader;
        public UI(IUILoader uiLoader)
        {
            ins = this;
            insNestingController = this;
            loader = uiLoader;
        }

        public static void OnBackButtonPressed()
        {
            insNestingController.OnBackButtonEventUsed();
        }

        public static void ForceCloseAll()
        {
            insNestingController.ForceCloseAll();
        }

        public static void RegisterScreen<T>(T screen) where T : UIScreen
        {
            locator.Add(typeof(T), screen);
        }

        public static void RemoveScreen<T>(T s = default) where T : UIScreen
        {
            locator.Remove(typeof(T));
            loader.ReleaseAssets();
        }

        public static void DestroyScreen<T>() where T : UIScreen
        {
#if UNITY_EDITOR
            //prevent error, because this code can be executed in Editor by OnVaildate or somethings other Events
            if (!Application.isPlaying) return;
#endif
            var destroyScreen = Get<T>();
            UnRegisterScreen<T>();
            Object.Destroy(destroyScreen?.gameObject);
        }

        public static void CloseCurrentFocusPrevious()
        {
            insNestingController.CloseCurrentFocusPrevious();
        }

        /// <summary>
        /// Close all nested IUIOpenable, and Open requested Screen
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T OpenAndCloseAllPrevious<T>(params object[] param) where T : UIScreen
        {
            insNestingController.ForceCloseAll();
            return ins.openNested<T>(param);
        }


        /// <summary>
        /// open screen and saving last screen to stack 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T OpenNested<T>(params object[] param) where T : UIScreen
        {
            return ins.openNested<T>(param);
        }
        private static T Get<T>() where T : UIScreen
        {
            if (locator.HasReference(typeof(T)))
                return (T)locator.Get(typeof(T));

            var s = loader.LoadScreen<T>();
            RegisterScreen<T>(s);
            return (T)locator.Get(typeof(T));
        }

        private static void UnRegisterScreen<T>() where T : UIScreen
        {
            locator.Remove(typeof(T));
        }

        /// -----------------------------------------------
        /// --------- IUIOpenable extensions
        /// -----------------------------------------------

        #region IUIOpenable Extension

        private T openNested<T>(params object[] param) where T : UIScreen
        {
            UnfocusPrevious();

            var screen = UI.Get<T>();

            //Debug.Log($"#1 OpenNested: {screen.name} from {(nestedList.Count > 0 ? (nestedList.Peek() as MonoBehaviour).name : null)}");

            nestedList.Push(screen);
            screen.Open(param);
            screen.OnFocus();

            return screen;
        }

        #endregion

        /// -----------------------------------------------
        ///    IUIOpenableQueueController implementation
        /// -----------------------------------------------

        #region IUIOpenableQueueController implementation

        readonly static Stack<IUIOpenable> nestedList = new Stack<IUIOpenable>();

        private static void UnfocusPrevious()
        {
            if (nestedList.Count > 0)
            {
                var nextOpenable = nestedList.Peek();

                //if(nextOpenable!=null)
                nextOpenable.OnFocusLost();
            }
        }

        /// <summary>
        /// call ForceClose() on current IUIOpenable in nestedList, call OnFocus() on previos in nestedList
        /// </summary>
        void IUIOpenableController.CloseCurrentFocusPrevious()
        {
            IUIOpenable openable = null;

            if (nestedList.Count > 0)
            {
                openable = nestedList.Peek();

                if (openable != null && !openable.TryClose())
                {
                    //Event is used, and can't be passed to parent Listeners
                    return;
                }
                nestedList.Pop();
            }

            if (nestedList.Count > 0)
            {
                var nextOpenable = nestedList.Peek();
                nextOpenable.OnFocus();
            } else
            {
                //Debug.Log($"#1 CloseCurrentFocusPrevios: No more UI Elements to Focus");
            }
        }

        void IUIOpenableController.ForceCloseAll()
        {
            var nestedCount = nestedList.Count;
            for (int i = nestedCount; i > 0; i--)
            {
                var openable = nestedList.Pop();
                if (openable != null)
                    openable.ForceClose();
            }
        }
        /// <summary>
        /// Called if user press Back Button. <br></br>
        /// current IUIOpenable will be get Close(), and previous will get OnFocus() event<br></br>
        /// execute OpenableQueueController.CloseCurrentFocusPrevios(); 
        /// </summary>
        /// <returns>return false if event not Used</returns>
        bool IUIOpenableController.OnBackButtonEventUsed()
        {
            Debug.Log($"#1 OnBackButton:  {(nestedList.Count > 0 ? (nestedList.Peek() as MonoBehaviour)?.name : null)} to {nestedList.Count}");

            if (nestedList.Count > 0)
            {
                var nextOpenable = nestedList.Peek();

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
        #endregion
    }
}