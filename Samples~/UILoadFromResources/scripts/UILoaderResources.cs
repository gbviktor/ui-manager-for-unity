using System;

using com.gbviktor.UIManager.Core;

using UnityEngine;

using Object = UnityEngine.Object;

namespace com.gbviktor.UIManager
{
    public class UILoaderResources : IUILoader
    {
        private IUIScreensList contentList;
        public UILoaderResources(IUIScreensList contentList)
        {
            this.contentList = contentList;
        }

        public S LoadScreen<S>() where S : UIScreen
        {
            var type = typeof(S);
            var targetGameObjectName = contentList.GetGameObjectNameByScreenTypeNameFullName

            if (targetGameObjectName != null)
            {
                GameObject goasset = Resources.Load<GameObject>($"{contentList.PathToResourcesFolder}{targetGameObjectName}");

                if (goasset.TryGetComponent<S>(out var asset))
                {
                    var screen = Object.Instantiate<S>(asset);
                    screen.name = type.ToString();
                    return screen;
                }
            }

            throw new Exception($"Requered Screen of type {type} don't not exist in Screens List");
        }

        public void ReleaseAssets()
        {
            Resources.UnloadUnusedAssets();
        }
    }
}