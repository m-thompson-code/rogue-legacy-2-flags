using System;
using UnityEngine;

// Token: 0x02000352 RID: 850
public class DrawHitboxColliders : MonoBehaviour
{
	// Token: 0x0400195E RID: 6494
	[SerializeField]
	private bool m_disableOnPlay;

	// Token: 0x0400195F RID: 6495
	[Space(10f)]
	[SerializeField]
	private bool m_drawWeaponHitboxes;

	// Token: 0x04001960 RID: 6496
	[SerializeField]
	private bool m_drawBodyHitboxes;

	// Token: 0x04001961 RID: 6497
	[SerializeField]
	private bool m_drawTerrainHitboxes;

	// Token: 0x04001962 RID: 6498
	[SerializeField]
	private bool m_drawPlatformHitbox;
}
