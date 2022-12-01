using System;

using Core;

using UnityEngine;

using Object = UnityEngine.Object;

namespace com.gbviktor
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
            var targetGameObjectName = contentList.GetGameObjectNameByScreenTypeName(type.Name);

            if (targetGameObjectName != null)
            {
                GameObject goasset = Resources.Load<GameObject>($"{contentList.PathToResourcesFolder}/{targetGameObjectName}");

                var asset = goasset.GetComponent<S>();

                if (asset != null)
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