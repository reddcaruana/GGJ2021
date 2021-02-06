using Assets.Scripts.AssetsManagers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Paywalls
{
    public class PaywallController
    {
        public PaywallData Data { get; private set; }
        
        private PaywallView view;

        public void CreateView(Transform parent)
            => view = MonoBehaviour.Instantiate(AssetLoader.ME.Loader<PaywallView>("Prefabs/Paywalls/PaywallBase"), parent);
    }
}
