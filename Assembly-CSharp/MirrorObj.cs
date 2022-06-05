using System;
using UnityEngine;

// Token: 0x02000267 RID: 615
public class MirrorObj : MonoBehaviour, IMirror
{
	// Token: 0x0600188A RID: 6282 RVA: 0x0004CD84 File Offset: 0x0004AF84
	public void Mirror()
	{
		Vector3 localEulerAngles = base.gameObject.transform.localEulerAngles;
		Vector3 localPosition = base.gameObject.transform.localPosition;
		Vector3 localScale = base.gameObject.transform.localScale;
		if (this.m_mirrorZRotation)
		{
			localEulerAngles.z += 180f;
		}
		if (this.m_mirrorXPosition)
		{
			localPosition.x = -localPosition.x;
		}
		if (this.m_mirrorYPosition)
		{
			localPosition.y = -localPosition.y;
		}
		if (this.m_mirrorXScale)
		{
			localScale.x = -localScale.x;
		}
		if (this.m_mirrorYScale)
		{
			localScale.y = -localScale.y;
		}
		base.gameObject.transform.localEulerAngles = localEulerAngles;
		base.gameObject.transform.localPosition = localPosition;
		base.gameObject.transform.localScale = localScale;
	}

	// Token: 0x040017D5 RID: 6101
	[SerializeField]
	private bool m_mirrorZRotation;

	// Token: 0x040017D6 RID: 6102
	[SerializeField]
	private bool m_mirrorXPosition;

	// Token: 0x040017D7 RID: 6103
	[SerializeField]
	private bool m_mirrorYPosition;

	// Token: 0x040017D8 RID: 6104
	[SerializeField]
	private bool m_mirrorXScale;

	// Token: 0x040017D9 RID: 6105
	[SerializeField]
	private bool m_mirrorYScale;
}
