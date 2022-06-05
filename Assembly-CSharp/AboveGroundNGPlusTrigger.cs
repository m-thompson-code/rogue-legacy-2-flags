using System;
using RLAudio;
using UnityEngine;

// Token: 0x02000544 RID: 1348
public class AboveGroundNGPlusTrigger : MonoBehaviour
{
	// Token: 0x06003144 RID: 12612 RVA: 0x000A6C30 File Offset: 0x000A4E30
	public void TriggerNGPlus()
	{
		PropSpawnController propSpawnController = PlayerManager.GetCurrentPlayerRoom().gameObject.FindObjectReference("Elpis", false, false);
		if (propSpawnController && propSpawnController.PropInstance && !propSpawnController.PropInstance.gameObject.activeSelf && !SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.Timeline_Unlocked) && SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.SeenParade))
		{
			SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.Timeline_Unlocked, true);
			Vector3 position = propSpawnController.PropInstance.transform.position;
			position.y += 2f;
			EffectManager.PlayEffect(propSpawnController.PropInstance.gameObject, propSpawnController.PropInstance.Animators[0], "EnemyTeleportOut_Effect", position, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None);
			propSpawnController.PropInstance.gameObject.SetActive(true);
			AudioManager.PlayOneShotAttached(null, "event:/Cut_Scenes/sfx_cutscene_ngPlusTeleport_playerAppear", base.gameObject);
			AudioManager.PlayOneShotAttached(null, "event:/SFX/Interactables/sfx_redHood_greeting", base.gameObject);
		}
	}
}
