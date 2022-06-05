using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000213 RID: 531
public class InvincibilityTimerUIController : MonoBehaviour
{
	// Token: 0x04001573 RID: 5491
	[SerializeField]
	private Slider m_slider;

	// Token: 0x04001574 RID: 5492
	[SerializeField]
	private Text m_text;

	// Token: 0x04001575 RID: 5493
	[SerializeField]
	private GameObject m_indicator;

	// Token: 0x04001576 RID: 5494
	[SerializeField]
	private bool m_isOn;
}
