using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Paywalls
{
    public class PaywallView : MonoBehaviour
    {
        private SpriteRenderer _renderer;

        void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
            name = "Paywall";
        }
    }
}