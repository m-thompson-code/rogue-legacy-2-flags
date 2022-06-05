using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001E8 RID: 488
public class DebugUIController : MonoBehaviour
{
	// Token: 0x040013F7 RID: 5111
	[SerializeField]
	private GameObject m_panel;

	// Token: 0x040013F8 RID: 5112
	[SerializeField]
	private Toggle m_weaponHitBox;

	// Token: 0x040013F9 RID: 5113
	[SerializeField]
	private Toggle m_platformHitBox;

	// Token: 0x040013FA RID: 5114
	[SerializeField]
	private Toggle m_bodyHitBox;

	// Token: 0x040013FB RID: 5115
	[SerializeField]
	private Toggle m_hazardHitBox;

	// Token: 0x040013FC RID: 5116
	[SerializeField]
	private Toggle m_worldInfo;

	// Token: 0x040013FD RID: 5117
	[SerializeField]
	private Toggle m_roomInfo;

	// Token: 0x040013FE RID: 5118
	[SerializeField]
	private Toggle m_dwarf;

	// Token: 0x040013FF RID: 5119
	[SerializeField]
	private Toggle m_enlarge;

	// Token: 0x04001400 RID: 5120
	[SerializeField]
	private Toggle m_invincible;

	// Token: 0x04001401 RID: 5121
	[SerializeField]
	private TMP_Dropdown m_traitOneDropdown;

	// Token: 0x04001402 RID: 5122
	[SerializeField]
	private TMP_Dropdown m_traitTwoDropdown;
}
