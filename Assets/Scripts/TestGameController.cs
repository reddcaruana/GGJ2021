using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Environment;
using Assets.Scripts.Paywalls;
using Assets.Scripts.Framework;
using Assets.Scripts.Framework.Tools;

public class TestGameController : MonobehaviourSingleton<TestGameController>
{
    public int maxStreams = 20;
    protected static StreamController[] streams;
    protected static PaywallController paywall;

    protected override void Awake()
    {
        base.Awake();
        GameFactory.Start();
    }

    void Start()
    {
        streams = new StreamController[maxStreams];
        for (int i = 0; i < maxStreams; i++)
        {
            streams[i] = new StreamController();
            streams[i].CreateView(null);
        }

        paywall = new PaywallController();
        paywall.CreateView(null);
    }
}
