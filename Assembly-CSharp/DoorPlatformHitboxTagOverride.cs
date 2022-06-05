using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000403 RID: 1027
public class DoorPlatformHitboxTagOverride : MonoBehaviour
{
	// Token: 0x0600264E RID: 9806 RVA: 0x0007EBB5 File Offset: 0x0007CDB5
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
