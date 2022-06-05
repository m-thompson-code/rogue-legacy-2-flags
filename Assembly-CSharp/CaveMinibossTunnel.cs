using System;
using FMODUnity;
using RLAudio;
using UnityEngine;

// Token: 0x02000565 RID: 1381
public class CaveMinibossTunnel : Tunnel
{
	// Token: 0x17001264 RID: 4708
	// (get) Token: 0x060032AF RID: 12975 RVA: 0x000AB74B File Offset: 0x000A994B
	// (set) Token: 0x060032B0 RID: 12976 RVA: 0x000AB753 File Offset: 0x000A9953
	public bool IsWhiteTunnel { get; set; }

	// Token: 0x060032B1 RID: 12977 RVA: 0x000AB75C File Offset: 0x000A995C
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

	// Token: 0x060032B2 RID: 12978 RVA: 0x000AB7C4 File Offset: 0x000A99C4
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

	// Token: 0x040027AE RID: 10158
	[SerializeField]
	private StudioEventEmitter m_lockedEventEmitter;
}
