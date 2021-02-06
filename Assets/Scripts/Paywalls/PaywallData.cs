using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.AquaticCreatures.Fish;

namespace Assets.Scripts.Paywalls
{
    public readonly struct PaywallData
    {
        public readonly FishData[] Requirements;

        public PaywallData(FishData[] requirements)
        {
            Requirements = requirements;
        }
    }
}
