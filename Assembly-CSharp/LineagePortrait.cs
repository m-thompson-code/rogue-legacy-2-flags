using System;
using RLAudio;
using TMPro;
using UnityEngine;

// Token: 0x0200047D RID: 1149
public class LineagePortrait : MonoBehaviour
{
	// Token: 0x17001050 RID: 4176
	// (get) Token: 0x060029F6 RID: 10742 RVA: 0x0008AAC9 File Offset: 0x00088CC9
	// (set) Token: 0x060029F7 RID: 10743 RVA: 0x0008AAD1 File Offset: 0x00088CD1
	public int Index { get; set; }

	// Token: 0x17001051 RID: 4177
	// (get) Token: 0x060029F8 RID: 10744 RVA: 0x0008AADA File Offset: 0x00088CDA
	public PlayerLookController Model
	{
		get
		{
			return this.m_model;
		}
	}

	// Token: 0x17001052 RID: 4178
	// (get) Token: 0x060029F9 RID: 10745 RVA: 0x0008AAE2 File Offset: 0x00088CE2
	// (set) Token: 0x060029FA RID: 10746 RVA: 0x0008AAEA File Offset: 0x00088CEA
	public CharacterData PortraitCharData { get; private set; }

	// Token: 0x060029FB RID: 10747 RVA: 0x0008AAF3 File Offset: 0x00088CF3
	private void Awake()
	{
		if (this.m_isLineageRoomPortrait && this.Model.VisualsGameObject != null)
		{
			this.Model.VisualsGameObject.SetLayerRecursively(24, true);
		}
	}

	// Token: 0x060029FC RID: 10748 RVA: 0x0008AB28 File Offset: 0x00088D28
	public void SetPortraitLook(CharacterData charData)
	{
		AnimBehaviourEventEmitter.DisableEmitter_STATIC = true;
		this.m_nameText.text = LocalizationManager.GetLocalizedPlayerName(charData);
		this.PortraitCharData = charData;
		this.m_model.InitializeLook(charData);
		this.m_model.Animator.SetBool("PortraitPose", false);
		this.m_model.Animator.SetBool("RetirePose", false);
		this.m_model.Animator.SetBool("Victory", false);
		this.m_model.transform.SetLocalPositionX(0f);
		if (charData.IsVictory)
		{
			this.m_model.Animator.SetBool("Victory", true);
			this.m_model.transform.SetLocalPositionX(0.25f);
			this.m_model.Animator.Play("Victory", 0, 1f);
		}
		else if (charData.IsRetired)
		{
			this.m_model.Animator.SetBool("RetirePose", true);
			this.m_model.Animator.Play("RetirePose", 0, 1f);
			if (charData.Weapon != AbilityType.BoxingGloveWeapon && charData.Weapon != AbilityType.ExplosiveHandsWeapon)
			{
				if (this.m_model.CurrentWeaponGeo)
				{
					this.m_model.CurrentWeaponGeo.gameObject.SetActive(false);
				}
				if (this.m_model.SecondaryWeaponGeo)
				{
					this.m_model.SecondaryWeaponGeo.gameObject.SetActive(false);
				}
			}
		}
		else
		{
			this.m_model.Animator.SetBool("PortraitPose", true);
			this.m_model.Animator.Play("PortraitPose", 0, 1f);
		}
		this.m_model.Animator.speed = 0f;
		this.m_model.Animator.Update(1f);
		AnimBehaviourEventEmitter.DisableEmitter_STATIC = false;
	}

	// Token: 0x04002254 RID: 8788
	[SerializeField]
	private PlayerLookController m_model;

	// Token: 0x04002255 RID: 8789
	[SerializeField]
	private TMP_Text m_nameText;

	// Token: 0x04002256 RID: 8790
	[SerializeField]
	private bool m_isLineageRoomPortrait;
}
