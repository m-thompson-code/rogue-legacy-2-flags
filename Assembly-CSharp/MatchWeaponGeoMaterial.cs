using System;
using UnityEngine;

// Token: 0x02000266 RID: 614
public class MatchWeaponGeoMaterial : MonoBehaviour
{
	// Token: 0x06001888 RID: 6280 RVA: 0x0004CD3E File Offset: 0x0004AF3E
	private void OnEnable()
	{
		if (PlayerManager.IsInstantiated && PlayerManager.GetPlayerController().LookController.SecondaryWeaponGeo)
		{
			this.m_meshRenderer.sharedMaterial = PlayerManager.GetPlayerController().LookController.SecondaryWeaponGeo.sharedMaterial;
		}
	}

	// Token: 0x040017D4 RID: 6100
	[SerializeField]
	private MeshRenderer m_meshRenderer;
}
