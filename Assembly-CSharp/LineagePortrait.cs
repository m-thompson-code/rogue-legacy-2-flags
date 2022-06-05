using System;
using RLAudio;
using TMPro;
using UnityEngine;

// Token: 0x02000773 RID: 1907
public class LineagePortrait : MonoBehaviour
{
	// Token: 0x17001585 RID: 5509
	// (get) Token: 0x06003A00 RID: 14848 RVA: 0x0001FDA7 File Offset: 0x0001DFA7
	// (set) Token: 0x06003A01 RID: 14849 RVA: 0x0001FDAF File Offset: 0x0001DFAF
	public int Index { get; set; }

	// Token: 0x17001586 RID: 5510
	// (get) Token: 0x06003A02 RID: 14850 RVA: 0x0001FDB8 File Offset: 0x0001DFB8
	public PlayerLookController Model
	{
		get
		{
			return this.m_model;
		}
	}

	// Token: 0x17001587 RID: 5511
	// (get) Token: 0x06003A03 RID: 14851 RVA: 0x0001FDC0 File Offset: 0x0001DFC0
	// (set) Token: 0x06003A04 RID: 14852 RVA: 0x0001FDC8 File Offset: 0x0001DFC8
	public CharacterData PortraitCharData { get; private set; }

	// Token: 0x06003A05 RID: 14853 RVA: 0x0001FDD1 File Offset: 0x0001DFD1
	private void Awake()
	{
		if (this.m_isLineageRoomPortrait && this.Model.VisualsGameObject != null)
		{
			this.Model.VisualsGameObject.SetLayerRecursively(24, true);
		}
	}

	// Token: 0x06003A06 RID: 14854 RVA: 0x000EC6B0 File Offset: 0x000EA8B0
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

	// Token: 0x04002E30 RID: 11824
	[SerializeField]
	private PlayerLookController m_model;

	// Token: 0x04002E31 RID: 11825
	[SerializeField]
	private TMP_Text m_nameText;

	// Token: 0x04002E32 RID: 11826
	[SerializeField]
	private bool m_isLineageRoomPortrait;
}
