using System;
using UnityEngine;

// Token: 0x0200042D RID: 1069
public class MatchWeaponGeoMaterial : MonoBehaviour
{
	// Token: 0x06002271 RID: 8817 RVA: 0x0001269F File Offset: 0x0001089F
	private void OnEnable()
	{
		if (PlayerManager.IsInstantiated && PlayerManager.GetPlayerController().LookController.SecondaryWeaponGeo)
		{
			this.m_meshRenderer.sharedMaterial = PlayerManager.GetPlayerController().LookController.SecondaryWeaponGeo.sharedMaterial;
		}
	}

	// Token: 0x04001F13 RID: 7955
	[SerializeField]
	private MeshRenderer m_meshRenderer;
}
