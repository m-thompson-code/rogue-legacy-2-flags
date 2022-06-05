using System;
using UnityEngine;

// Token: 0x0200042E RID: 1070
public class MirrorObj : MonoBehaviour, IMirror
{
	// Token: 0x06002273 RID: 8819 RVA: 0x000AAB2C File Offset: 0x000A8D2C
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

	// Token: 0x04001F14 RID: 7956
	[SerializeField]
	private bool m_mirrorZRotation;

	// Token: 0x04001F15 RID: 7957
	[SerializeField]
	private bool m_mirrorXPosition;

	// Token: 0x04001F16 RID: 7958
	[SerializeField]
	private bool m_mirrorYPosition;

	// Token: 0x04001F17 RID: 7959
	[SerializeField]
	private bool m_mirrorXScale;

	// Token: 0x04001F18 RID: 7960
	[SerializeField]
	private bool m_mirrorYScale;
}
