using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020003C6 RID: 966
public class InvincibilityTimerUIController : MonoBehaviour
{
	// Token: 0x04001C76 RID: 7286
	[SerializeField]
	private Slider m_slider;

	// Token: 0x04001C77 RID: 7287
	[SerializeField]
	private Text m_text;

	// Token: 0x04001C78 RID: 7288
	[SerializeField]
	private GameObject m_indicator;

	// Token: 0x04001C79 RID: 7289
	[SerializeField]
	private bool m_isOn;
}
