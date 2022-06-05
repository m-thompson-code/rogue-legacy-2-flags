using System;
using System.Collections;
using UnityEngine;

// Token: 0x020006B2 RID: 1714
public class DoorPlatformHitboxTagOverride : MonoBehaviour
{
	// Token: 0x060034D9 RID: 13529 RVA: 0x0001CFB9 File Offset: 0x0001B1B9
	private IEnumerator Start()
	{
		HitboxControllerLite m_hitboxController = base.GetComponent<HitboxControllerLite>();
		while (!m_hitboxController.IsInitialized)
		{
			yield return null;
		}
		m_hitboxController.GetCollider(HitboxType.Platform).gameObject.tag = "OneWay";
		yield break;
	}
}
