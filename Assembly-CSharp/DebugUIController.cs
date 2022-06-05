using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000380 RID: 896
public class DebugUIController : MonoBehaviour
{
	// Token: 0x04001A97 RID: 6807
	[SerializeField]
	private GameObject m_panel;

	// Token: 0x04001A98 RID: 6808
	[SerializeField]
	private Toggle m_weaponHitBox;

	// Token: 0x04001A99 RID: 6809
	[SerializeField]
	private Toggle m_platformHitBox;

	// Token: 0x04001A9A RID: 6810
	[SerializeField]
	private Toggle m_bodyHitBox;

	// Token: 0x04001A9B RID: 6811
	[SerializeField]
	private Toggle m_hazardHitBox;

	// Token: 0x04001A9C RID: 6812
	[SerializeField]
	private Toggle m_worldInfo;

	// Token: 0x04001A9D RID: 6813
	[SerializeField]
	private Toggle m_roomInfo;

	// Token: 0x04001A9E RID: 6814
	[SerializeField]
	private Toggle m_dwarf;

	// Token: 0x04001A9F RID: 6815
	[SerializeField]
	private Toggle m_enlarge;

	// Token: 0x04001AA0 RID: 6816
	[SerializeField]
	private Toggle m_invincible;

	// Token: 0x04001AA1 RID: 6817
	[SerializeField]
	private TMP_Dropdown m_traitOneDropdown;

	// Token: 0x04001AA2 RID: 6818
	[SerializeField]
	private TMP_Dropdown m_traitTwoDropdown;
}
