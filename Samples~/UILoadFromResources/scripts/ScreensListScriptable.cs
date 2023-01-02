using System;
using System.Collections.Generic;

using com.gbviktor.UIManager.Core;

using UnityEngine;

using Object = UnityEngine.Object;

namespace com.gbviktor.UIManager
{
    [CreateAssetMenu(menuName = "Montana Games/DB/Screens List", fileName = "ScreensList", order = 1)]
    public class ScreensListScriptable : ScriptableObject, IUIScreensList
    {
        [SerializeField] List<UIScreenContainer> screensList = new List<UIScreenContainer>();

        public string PathToResourcesFolder { get; private set; }
        object IUIScreensList.PathToResourcesFolder => PathToResourcesFolder;

        string IUIScreensList.GetGameObjectNameByScreenTypeName(string typeName)
        {

            return screensList.Find(x => x.typeName == typeName).gameObjectName;
        }

        [Serializable]
        public class UIScreenContainer
        {
            public string typeName;
            public string gameObjectName;
        }

        #region EDITOR ONLY

#if UNITY_EDITOR

        protected string pathToFolderOfScriptable;
        [SerializeField] string PathToAssets;
        public void OnEnable()
        {
            UpdateReference();
            FindContent();
        }
        private void UpdateReference()
        {
            pathToFolderOfScriptable = UnityEditor.AssetDatabase.GetAssetPath(this).Replace($"/{this.name}.asset", "");
            PathToAssets = GetRelativePathFromResourcesFolder(pathToFolderOfScriptable);
            PathToResourcesFolder = PathToAssets;//PathToAssets.Replace("Assets/", "");
        }

        private string GetRelativePathFromResourcesFolder(string pathToAssets)
        {
            //we need to get relative path from Resources folder to get success function to load from Resources
            // see https://docs.unity3d.com/ScriptReference/Resources.Load.html
            var folders = pathToAssets.Split('/');
            int indexOfResourceFolder = -1;
            string relativePath = string.Empty;

            for (int i = 0; i < folders.Length; i++)
            {
                string folder = folders[i];
                if (indexOfResourceFolder == -1)
                {
                    if (folder.Equals("Resources"))
                    {
                        indexOfResourceFolder = i;
                    }
                } else relativePath = $"{relativePath}{folder}/";
            }
            return relativePath;
        }

        [RuntimeInitializeOnLoadMethod]
        private void OnValidate()
        {
            FindContent();
        }
        private void FindContent()
        {
            screensList.Clear();

            var guids = FindObjectsInFolderOfType();

            foreach (var guid in guids)
            {
                var obj = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(UnityEditor.AssetDatabase.GUIDToAssetPath(guid));

                if (obj == null) continue;
                if (IsAlreadyExist(guid)) continue;

                AddNew(obj, guid);
            }
        }
        public string[] FindObjectsInFolderOfType()
        {
            var TypeToSearch = "Prefab";
            var PathToAssets = pathToFolderOfScriptable;

            if (!UnityEditor.EditorUtility.IsPersistent(this) || string.IsNullOrEmpty(PathToAssets))
                return Array.Empty<string>();

            return UnityEditor.AssetDatabase.FindAssets($"t:{TypeToSearch}", new[] { PathToAssets });
        }
        protected virtual bool IsAlreadyExist(string guid)
        {
            var index = screensList.FindIndex(x => x.typeName.Equals(guid));
            return index > -1;
        }

        protected void AddNew(Object obj, string guid)
        {
            var screen = ((GameObject)obj).GetComponent<UIScreen>();

            if (screen != null)
            {
                screensList.Add(new UIScreenContainer()
                {
                    typeName = screen.GetType().ToString(),
                    gameObjectName = screen.name
                });
            }
        }

#endif

        #endregion
    }
}