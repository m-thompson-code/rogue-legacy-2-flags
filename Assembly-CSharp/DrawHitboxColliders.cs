using System;
using UnityEngine;

// Token: 0x020001D4 RID: 468
public class DrawHitboxColliders : MonoBehaviour
{
	// Token: 0x0400131E RID: 4894
	[SerializeField]
	private bool m_disableOnPlay;

	// Token: 0x0400131F RID: 4895
	[Space(10f)]
	[SerializeField]
	private bool m_drawWeaponHitboxes;

	// Token: 0x04001320 RID: 4896
	[SerializeField]
	private bool m_drawBodyHitboxes;

	// Token: 0x04001321 RID: 4897
	[SerializeField]
	private bool m_drawTerrainHitboxes;

	// Token: 0x04001322 RID: 4898
	[SerializeField]
	private bool m_drawPlatformHitbox;
}
