using System;
using Sigtrap.Relays;
using UnityEngine;

// Token: 0x020009AB RID: 2475
public interface IEffectTriggerEvent_OnDamage
{
	// Token: 0x17001A2E RID: 6702
	// (get) Token: 0x06004C22 RID: 19490
	IRelayLink<GameObject, float, bool> OnDamageEffectTriggerRelay { get; }
}
