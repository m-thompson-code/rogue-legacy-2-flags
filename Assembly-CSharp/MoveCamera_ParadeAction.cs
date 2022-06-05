using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000473 RID: 1139
public class MoveCamera_ParadeAction : MonoBehaviour, IParadeAction
{
	// Token: 0x06002423 RID: 9251 RVA: 0x000140AA File Offset: 0x000122AA
	public IEnumerator TriggerAction(ParadeController controller)
	{
		float num = this.m_duration;
		num = Mathf.Min(num, 2f);
		if (this.m_waitTillMovementComplete)
		{
			yield return TweenManager.TweenTo(controller.VirtualCamera.transform, num, new EaseDelegate(Ease.Quad.EaseInOut), new object[]
			{
				"localPosition.x",
				this.m_camPos.position.x,
				"localPosition.y",
				this.m_camPos.position.y
			});
		}
		else
		{
			TweenManager.TweenTo(controller.VirtualCamera.transform, num, new EaseDelegate(Ease.Quad.EaseInOut), new object[]
			{
				"localPosition.x",
				this.m_camPos.position.x,
				"localPosition.y",
				this.m_camPos.position.y
			});
		}
		yield break;
	}

	// Token: 0x04001FE8 RID: 8168
	[SerializeField]
	private Transform m_camPos;

	// Token: 0x04001FE9 RID: 8169
	[SerializeField]
	private float m_duration = 1f;

	// Token: 0x04001FEA RID: 8170
	[SerializeField]
	private bool m_waitTillMovementComplete;
}
