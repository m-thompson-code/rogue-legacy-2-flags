using System;
using Sigtrap.Relays;
using UnityEngine;

// Token: 0x020005A3 RID: 1443
public interface IEffectTriggerEvent_OnDamage
{
	// Token: 0x17001301 RID: 4865
	// (get) Token: 0x06003610 RID: 13840
	IRelayLink<GameObject, float, bool> OnDamageEffectTriggerRelay { get; }
}
