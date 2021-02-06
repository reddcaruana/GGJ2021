using Assets.Scripts.AssetsManagers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Environment
{
    public class StreamController
    {
        private StreamView view;

        public void CreateView(Transform parent)
            => view = MonoBehaviour.Instantiate(AssetLoader.ME.Loader<StreamView>("Prefabs/Environment/Stream"), parent);
    }
}
