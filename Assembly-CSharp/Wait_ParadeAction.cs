using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200047F RID: 1151
public class Wait_ParadeAction : MonoBehaviour, IParadeAction
{
	// Token: 0x06002461 RID: 9313 RVA: 0x000142A1 File Offset: 0x000124A1
	public IEnumerator TriggerAction(ParadeController controller)
	{
		float totalWaitDuration = Mathf.Min(this.m_waitDuration, 0.1f);
		totalWaitDuration += Time.time;
		while (Time.time < totalWaitDuration)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x0400201F RID: 8223
	[SerializeField]
	private float m_waitDuration;
}
