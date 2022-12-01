using System;
using System.Collections.Generic;

using Core;

using UnityEngine;

using Object = UnityEngine.Object;

namespace com.gbviktor
{
    [CreateAssetMenu(menuName = "Montana Games/DB/Screens List", fileName = "ScreensList", order = 1)]
    public class ScreensListScriptable : ScriptableObject, IUIScreensList
    {
        [SerializeField] List<UIScreenContainer> screensList = new List<UIScreenContainer>();

        public string PathToResourcesFolder { get; private set; }
        object IUIScreensList.PathToResourcesFolder => PathToResourcesFolder;

        string IUIScreensList.GetGameObjectNameByScreenTypeName(string typeName)
        {
            return screensList.Find(x => x.typeName.Equals(typeName, StringComparison.Ordinal)).gameObjectName;
        }

        [Serializable]
        public class UIScreenContainer
        {
            public string typeName;
            public string gameObjectName;
        }

        #region EDITOR ONLY
#if UNITY_EDITOR

        [NonSerialized]
        public ScriptableObject Reference;
        protected string assetPath;
        public string PathToAssets;
        public void OnEnable()
        {
            UpdateReference();
            FindContent();
        }
        private void UpdateReference()
        {
            Reference = this;
            assetPath = UnityEditor.AssetDatabase.GetAssetPath(Reference);
            PathToAssets = assetPath.Replace("/" + this.name + ".asset", "");
            PathToResourcesFolder = PathToAssets.Replace("Assets/Resources/", "");
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
            var PathToAssets = PathToResourcesFolder;

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