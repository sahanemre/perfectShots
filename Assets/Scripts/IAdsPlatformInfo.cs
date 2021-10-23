using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAdsPlatformInfo
{
    string AppId { get; }
    string TestAdIdBanner { get; }
    string TestAdIdInterstitial { get; }
    string TestAdIdRewarded { get; }
    string AdIdBanner { get; }
    string AdIdInterstitial { get; }
    string AdIdRewarded { get; }
}
