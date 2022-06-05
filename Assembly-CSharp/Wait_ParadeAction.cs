using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002A9 RID: 681
public class Wait_ParadeAction : MonoBehaviour, IParadeAction
{
	// Token: 0x06001A43 RID: 6723 RVA: 0x00052FE5 File Offset: 0x000511E5
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

	// Token: 0x040018A6 RID: 6310
	[SerializeField]
	private float m_waitDuration;
}
