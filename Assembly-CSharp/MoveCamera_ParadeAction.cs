using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002A3 RID: 675
public class MoveCamera_ParadeAction : MonoBehaviour, IParadeAction
{
	// Token: 0x06001A1F RID: 6687 RVA: 0x00052338 File Offset: 0x00050538
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

	// Token: 0x04001887 RID: 6279
	[SerializeField]
	private Transform m_camPos;

	// Token: 0x04001888 RID: 6280
	[SerializeField]
	private float m_duration = 1f;

	// Token: 0x04001889 RID: 6281
	[SerializeField]
	private bool m_waitTillMovementComplete;
}
