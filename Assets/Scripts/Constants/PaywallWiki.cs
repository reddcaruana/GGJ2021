using UnityEngine;
using Assets.Scripts.Paywalls;
using Assets.Scripts.AquaticCreatures.Fish;

namespace Assets.Scripts.Constants
{
    public static class PaywallWiki
    {
        public static readonly PaywallData Bear = new PaywallData(
            requirements: new FishData[] { FishWiki.Awrata, FishWiki.Awrata, FishWiki.Vopa }
        );
    }
}
