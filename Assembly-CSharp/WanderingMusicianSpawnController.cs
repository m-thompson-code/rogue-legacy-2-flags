using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004E7 RID: 1255
public class WanderingMusicianSpawnController : MonoBehaviour
{
	// Token: 0x17001196 RID: 4502
	// (get) Token: 0x06002EF9 RID: 12025 RVA: 0x000A0293 File Offset: 0x0009E493
	// (set) Token: 0x06002EFA RID: 12026 RVA: 0x000A029A File Offset: 0x0009E49A
	public static BiomeType BiomeControllerType { get; private set; }

	// Token: 0x17001197 RID: 4503
	// (get) Token: 0x06002EFB RID: 12027 RVA: 0x000A02A2 File Offset: 0x0009E4A2
	// (set) Token: 0x06002EFC RID: 12028 RVA: 0x000A02A9 File Offset: 0x0009E4A9
	public static int BiomeControllerIndex { get; private set; }

	// Token: 0x17001198 RID: 4504
	// (get) Token: 0x06002EFD RID: 12029 RVA: 0x000A02B1 File Offset: 0x0009E4B1
	// (set) Token: 0x06002EFE RID: 12030 RVA: 0x000A02B8 File Offset: 0x0009E4B8
	public static bool IsInitialized { get; private set; }

	// Token: 0x06002EFF RID: 12031 RVA: 0x000A02C0 File Offset: 0x0009E4C0
	private void Awake()
	{
		this.m_onWorldCreationComplete = new Action<MonoBehaviour, EventArgs>(this.OnWorldCreationComplete);
		this.m_onLevelEditorCreationComplete = new Action<MonoBehaviour, EventArgs>(this.OnLevelEditorCreationComplete);
	}

	// Token: 0x06002F00 RID: 12032 RVA: 0x000A02E6 File Offset: 0x0009E4E6
	private void OnEnable()
	{
		WanderingMusicianSpawnController.BiomeControllerType = BiomeType.None;
		WanderingMusicianSpawnController.BiomeControllerIndex = -1;
		this.m_isInLevelEditor = GameUtility.IsInLevelEditor;
		if (!this.m_isInLevelEditor)
		{
			Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.WorldCreationComplete, this.m_onWorldCreationComplete);
			return;
		}
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.LevelEditorWorldCreationComplete, this.m_onLevelEditorCreationComplete);
	}

	// Token: 0x06002F01 RID: 12033 RVA: 0x000A0321 File Offset: 0x0009E521
	private void OnDisable()
	{
		if (!GameUtility.IsApplicationQuitting)
		{
			if (!this.m_isInLevelEditor)
			{
				Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.WorldCreationComplete, this.m_onWorldCreationComplete);
				return;
			}
			Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.LevelEditorWorldCreationComplete, this.m_onLevelEditorCreationComplete);
		}
	}

	// Token: 0x06002F02 RID: 12034 RVA: 0x000A034C File Offset: 0x0009E54C
	private void OnWorldCreationComplete(MonoBehaviour sender, EventArgs eventArgs)
	{
		if (RNGManager.GetRandomNumber(RngID.SpecialRoomProps, "WandererSpawnChance - WanderingMusicianSpawnController.OnWorldCreationComplete()", 0f, 1f) <= 0.5f)
		{
			List<BaseRoom> list = new List<BaseRoom>();
			foreach (KeyValuePair<BiomeType, BiomeController> keyValuePair in WorldBuilder.BiomeControllers)
			{
				list.Add(keyValuePair.Value.TransitionRoom);
			}
			if (list.Count > 0)
			{
				int randomNumber = RNGManager.GetRandomNumber(RngID.SpecialRoomProps, "WandererRoomSpawnIndex - WanderingMusicianSpawnController.OnWorldCreationComplete()", 0, list.Count);
				if (list[randomNumber] != null)
				{
					WanderingMusicianSpawnController.BiomeControllerType = list[randomNumber].BiomeType;
					WanderingMusicianSpawnController.BiomeControllerIndex = list[randomNumber].BiomeControllerIndex;
				}
			}
		}
		WanderingMusicianSpawnController.IsInitialized = true;
	}

	// Token: 0x06002F03 RID: 12035 RVA: 0x000A0424 File Offset: 0x0009E624
	private void OnLevelEditorCreationComplete(MonoBehaviour sender, EventArgs eventArgs)
	{
		if (RNGManager.GetRandomNumber(RngID.SpecialRoomProps, "WandererSpawnChance - WanderingMusicianSpawnController.OnWorldCreationComplete()", 0f, 1f) <= 0.5f)
		{
			LevelEditorWorldCreationCompleteEventArgs levelEditorWorldCreationCompleteEventArgs = eventArgs as LevelEditorWorldCreationCompleteEventArgs;
			WanderingMusicianSpawnController.BiomeControllerType = levelEditorWorldCreationCompleteEventArgs.BuiltRoom.BiomeType;
			WanderingMusicianSpawnController.BiomeControllerIndex = levelEditorWorldCreationCompleteEventArgs.BuiltRoom.BiomeControllerIndex;
		}
		WanderingMusicianSpawnController.IsInitialized = true;
	}

	// Token: 0x04002567 RID: 9575
	private const float WANDERING_MUSICIAN_SPAWN_ODDS = 0.5f;

	// Token: 0x04002568 RID: 9576
	private bool m_isInLevelEditor;

	// Token: 0x04002569 RID: 9577
	private Action<MonoBehaviour, EventArgs> m_onWorldCreationComplete;

	// Token: 0x0400256A RID: 9578
	private Action<MonoBehaviour, EventArgs> m_onLevelEditorCreationComplete;
}
