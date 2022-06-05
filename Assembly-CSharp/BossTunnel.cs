using System;
using System.Collections;
using RLAudio;
using RLWorldCreation;
using RL_Windows;
using SceneManagement_RL;
using TMPro;
using UnityEngine;

// Token: 0x02000930 RID: 2352
public class BossTunnel : Tunnel
{
	// Token: 0x17001922 RID: 6434
	// (get) Token: 0x06004765 RID: 18277 RVA: 0x000272C2 File Offset: 0x000254C2
	// (set) Token: 0x06004766 RID: 18278 RVA: 0x000272CA File Offset: 0x000254CA
	public BossTunnelState TunnelState { get; set; }

	// Token: 0x06004767 RID: 18279 RVA: 0x000272D3 File Offset: 0x000254D3
	protected override IEnumerator EnterTunnelCoroutine()
	{
		if (!base.IsCutsceneTeleport)
		{
			RewiredMapController.SetCurrentMapEnabled(false);
			MusicManager.StopMusic();
			base.Animator.SetBool("Open", true);
			this.RegenHealthAndMana();
			this.RegenAbilityCooldowns();
			if (base.Room.GetComponent<GoldenDoorsEntranceRoomController>())
			{
				PlayerManager.GetPlayerController().CharacterDash.StopDash();
				PlayerManager.GetPlayerController().ControllerCorgi.GravityActive(false);
				PlayerManager.GetPlayerController().ControllerCorgi.SetForce(Vector2.zero);
				BiomeTransitionController.TransitionStartRelay.Dispatch(BiomeType.Castle, BiomeType.Garden);
				SceneLoader_RL.RunTransitionWithLogic(this.TeleportPlayerToGarden(), TransitionID.FadeToBlackWithLoading, true);
				yield break;
			}
			float yieldTime = Time.time;
			while (Time.time < yieldTime + 0.5f)
			{
				yield return null;
			}
			SceneLoader_RL.RunTransitionWithLogic(this.EnterTunnelAnimCoroutine(), base.TransitionType, false);
		}
		else
		{
			yield return base.EnterTunnelCoroutine();
		}
		yield break;
	}

	// Token: 0x06004768 RID: 18280 RVA: 0x000272E2 File Offset: 0x000254E2
	private IEnumerator TeleportPlayerToGarden()
	{
		yield return BiomeTransitionController.BiomeTransitionCoroutine(BiomeType.Castle, BiomeType.Garden);
		PlayerManager.GetPlayerController().ControllerCorgi.GravityActive(true);
		PlayerManager.GetPlayerController().StopActiveAbilities(true);
		WorldBuilder.BiomeControllers[BiomeType.Garden].TransitionRoom.PlacePlayerInRoom(null);
		BiomeTransitionController.DestroyOldCastleTransitionRoom(BiomeType.Garden);
		BiomeTransitionController.DestroyBiomeRoomInstances(BiomeType.Castle);
		yield break;
	}

	// Token: 0x06004769 RID: 18281 RVA: 0x000272EA File Offset: 0x000254EA
	private IEnumerator EnterTunnelAnimCoroutine()
	{
		float yieldTime = Time.time;
		while (Time.time < yieldTime + 2.5f)
		{
			yield return null;
		}
		base.EnterTunnel();
		yield break;
	}

	// Token: 0x0600476A RID: 18282 RVA: 0x00115B4C File Offset: 0x00113D4C
	private void RegenHealthAndMana()
	{
		PlayerSaveFlag bossFreeHealUsedFlag = this.GetBossFreeHealUsedFlag();
		if (bossFreeHealUsedFlag != PlayerSaveFlag.None && !SaveManager.PlayerSaveData.GetFlag(bossFreeHealUsedFlag))
		{
			float bossHPMPRegenMod = SkillTreeLogicHelper.GetBossHPMPRegenMod();
			if (bossHPMPRegenMod > 0f)
			{
				SaveManager.PlayerSaveData.SetFlag(bossFreeHealUsedFlag, true);
				PlayerController playerController = PlayerManager.GetPlayerController();
				float num = (float)playerController.ActualMaxHealth * bossHPMPRegenMod;
				float num2 = (float)playerController.ActualMaxMana * bossHPMPRegenMod;
				num = (float)Mathf.CeilToInt(num);
				num2 = (float)Mathf.CeilToInt(num2);
				if (TraitManager.IsTraitActive(TraitType.MegaHealth))
				{
					num = 0f;
				}
				playerController.SetHealth(num, true, true);
				playerController.SetMana(num2, true, true, false);
				Vector2 absPos = playerController.Midpoint;
				absPos.y += playerController.CollisionBounds.height / 2f;
				string str = string.Format(LocalizationManager.GetString("LOC_ID_STATUS_EFFECT_HEALTH_RESTORE_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false), (int)num);
				string str2 = string.Format(LocalizationManager.GetString("LOC_ID_STATUS_EFFECT_MANA_RESTORE_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false), (int)num2);
				TextPopupManager.DisplayTextAtAbsPos(TextPopupType.HPGained, str + "\n<color=#47FFFB>" + str2 + "</color>", absPos, null, TextAlignmentOptions.Center);
			}
		}
	}

	// Token: 0x0600476B RID: 18283 RVA: 0x00115C88 File Offset: 0x00113E88
	private void RegenAbilityCooldowns()
	{
		PlayerController playerController = PlayerManager.GetPlayerController();
		playerController.CastAbility.ResetAbilityAmmo(CastAbilityType.Weapon, false);
		playerController.CastAbility.ResetAbilityAmmo(CastAbilityType.Talent, false);
		playerController.CastAbility.ResetAbilityAmmo(CastAbilityType.Spell, false);
		playerController.CastAbility.ResetAbilityCooldowns(CastAbilityType.Weapon, false);
		playerController.CastAbility.ResetAbilityCooldowns(CastAbilityType.Talent, false);
		playerController.CastAbility.ResetAbilityCooldowns(CastAbilityType.Spell, false);
	}

	// Token: 0x0600476C RID: 18284 RVA: 0x00115CE8 File Offset: 0x00113EE8
	protected override void OnPlayerInteractedWithTunnel(GameObject otherObj)
	{
		if (base.IsLocked && !base.IsCutsceneTeleport)
		{
			DialogueManager.StartNewDialogue(null, NPCState.Idle);
			DialogueManager.AddDialogue(null, "LOC_ID_MISC_EVENT_TEXT_SPELLSWORD_DOOR_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
			WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
			return;
		}
		base.OnPlayerInteractedWithTunnel(otherObj);
	}

	// Token: 0x0600476D RID: 18285 RVA: 0x00115D40 File Offset: 0x00113F40
	protected virtual PlayerSaveFlag GetBossFreeHealUsedFlag()
	{
		BiomeType biomeType = base.Room.BiomeType;
		if (biomeType <= BiomeType.Garden)
		{
			if (biomeType <= BiomeType.Cave)
			{
				if (biomeType == BiomeType.Castle)
				{
					return PlayerSaveFlag.CastleBoss_FreeHeal_Used;
				}
				if (biomeType == BiomeType.Cave)
				{
					return PlayerSaveFlag.CaveBoss_FreeHeal_Used;
				}
			}
			else
			{
				if (biomeType == BiomeType.Forest)
				{
					return PlayerSaveFlag.ForestBoss_FreeHeal_Used;
				}
				if (biomeType == BiomeType.Garden)
				{
					return PlayerSaveFlag.FinalBoss_FreeHeal_Used;
				}
			}
		}
		else if (biomeType <= BiomeType.Study)
		{
			if (biomeType == BiomeType.Stone)
			{
				return PlayerSaveFlag.BridgeBoss_FreeHeal_Used;
			}
			if (biomeType == BiomeType.Study)
			{
				return PlayerSaveFlag.StudyBoss_FreeHeal_Used;
			}
		}
		else if (biomeType == BiomeType.Tower || biomeType == BiomeType.TowerExterior)
		{
			return PlayerSaveFlag.TowerBoss_FreeHeal_Used;
		}
		return PlayerSaveFlag.None;
	}
}
