using System;
using FMODUnity;
using RLAudio;
using UnityEngine;

// Token: 0x02000935 RID: 2357
public class CaveMinibossTunnel : Tunnel
{
	// Token: 0x17001929 RID: 6441
	// (get) Token: 0x06004784 RID: 18308 RVA: 0x0002736C File Offset: 0x0002556C
	// (set) Token: 0x06004785 RID: 18309 RVA: 0x00027374 File Offset: 0x00025574
	public bool IsWhiteTunnel { get; set; }

	// Token: 0x06004786 RID: 18310 RVA: 0x00115FF0 File Offset: 0x001141F0
	public override void SetIsLocked(bool isLocked)
	{
		if ((this.IsWhiteTunnel && SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.CaveMiniboss_White_Defeated)) || (!this.IsWhiteTunnel && SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.CaveMiniboss_Black_Defeated)))
		{
			base.SetIsLocked(isLocked);
			return;
		}
		base.IsLocked = isLocked;
		if (this.m_interactable)
		{
			this.m_interactable.SetIsInteractableActive(true);
		}
	}

	// Token: 0x06004787 RID: 18311 RVA: 0x00116058 File Offset: 0x00114258
	protected override void OnPlayerInteractedWithTunnel(GameObject otherObj)
	{
		Vector2 position = base.transform.position;
		position.y += 2f;
		if (base.IsLocked)
		{
			base.Animator.SetTrigger("GateShake");
			if (this.IsWhiteTunnel)
			{
				TextPopupManager.DisplayLocIDText(TextPopupType.OutOfAmmo, "LOC_ID_STATUS_EFFECT_WHITE_KEY_REQUIRED_1", StringGenderType.UsePlayerData, position, 0f);
			}
			else
			{
				TextPopupManager.DisplayLocIDText(TextPopupType.OutOfAmmo, "LOC_ID_STATUS_EFFECT_BLACK_KEY_REQUIRED_1", StringGenderType.UsePlayerData, position, 0f);
			}
			AudioManager.Play(null, this.m_lockedEventEmitter);
			return;
		}
		if (this.IsWhiteTunnel && !SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.CaveMiniboss_WhiteDoor_Opened))
		{
			SaveManager.PlayerSaveData.GetRelic(RelicType.DragonKeyWhite).SetLevel(0, false, true);
			SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.CaveMiniboss_WhiteDoor_Opened, true);
			base.Animator.SetTrigger("GateOpen");
			TextPopupManager.DisplayLocIDText(TextPopupType.OutOfAmmo, "LOC_ID_STATUS_EFFECT_DRAGON_LOCK_UNLOCKED_1", StringGenderType.UsePlayerData, position, 0f);
			AudioManager.PlayOneShot(null, "event:/Cut_Scenes/sfx_Cutscene_doorUnlock_doorOpen", base.gameObject.transform.position);
			return;
		}
		if (!this.IsWhiteTunnel && !SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.CaveMiniboss_BlackDoor_Opened))
		{
			SaveManager.PlayerSaveData.GetRelic(RelicType.DragonKeyBlack).SetLevel(0, false, true);
			SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.CaveMiniboss_BlackDoor_Opened, true);
			base.Animator.SetTrigger("GateOpen");
			TextPopupManager.DisplayLocIDText(TextPopupType.OutOfAmmo, "LOC_ID_STATUS_EFFECT_DRAGON_LOCK_UNLOCKED_1", StringGenderType.UsePlayerData, position, 0f);
			AudioManager.PlayOneShot(null, "event:/Cut_Scenes/sfx_Cutscene_doorUnlock_doorOpen", base.gameObject.transform.position);
			return;
		}
		base.OnPlayerInteractedWithTunnel(otherObj);
	}

	// Token: 0x040036DC RID: 14044
	[SerializeField]
	private StudioEventEmitter m_lockedEventEmitter;
}
