using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000834 RID: 2100
public class WanderingMusicianSpawnController : MonoBehaviour
{
	// Token: 0x17001761 RID: 5985
	// (get) Token: 0x060040D1 RID: 16593 RVA: 0x00023D46 File Offset: 0x00021F46
	// (set) Token: 0x060040D2 RID: 16594 RVA: 0x00023D4D File Offset: 0x00021F4D
	public static BiomeType BiomeControllerType { get; private set; }

	// Token: 0x17001762 RID: 5986
	// (get) Token: 0x060040D3 RID: 16595 RVA: 0x00023D55 File Offset: 0x00021F55
	// (set) Token: 0x060040D4 RID: 16596 RVA: 0x00023D5C File Offset: 0x00021F5C
	public static int BiomeControllerIndex { get; private set; }

	// Token: 0x17001763 RID: 5987
	// (get) Token: 0x060040D5 RID: 16597 RVA: 0x00023D64 File Offset: 0x00021F64
	// (set) Token: 0x060040D6 RID: 16598 RVA: 0x00023D6B File Offset: 0x00021F6B
	public static bool IsInitialized { get; private set; }

	// Token: 0x060040D7 RID: 16599 RVA: 0x00023D73 File Offset: 0x00021F73
	private void Awake()
	{
		this.m_onWorldCreationComplete = new Action<MonoBehaviour, EventArgs>(this.OnWorldCreationComplete);
		this.m_onLevelEditorCreationComplete = new Action<MonoBehaviour, EventArgs>(this.OnLevelEditorCreationComplete);
	}

	// Token: 0x060040D8 RID: 16600 RVA: 0x00023D99 File Offset: 0x00021F99
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

	// Token: 0x060040D9 RID: 16601 RVA: 0x00023DD4 File Offset: 0x00021FD4
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

	// Token: 0x060040DA RID: 16602 RVA: 0x00104058 File Offset: 0x00102258
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

	// Token: 0x060040DB RID: 16603 RVA: 0x00104130 File Offset: 0x00102330
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

	// Token: 0x040032AF RID: 12975
	private const float WANDERING_MUSICIAN_SPAWN_ODDS = 0.5f;

	// Token: 0x040032B0 RID: 12976
	private bool m_isInLevelEditor;

	// Token: 0x040032B1 RID: 12977
	private Action<MonoBehaviour, EventArgs> m_onWorldCreationComplete;

	// Token: 0x040032B2 RID: 12978
	private Action<MonoBehaviour, EventArgs> m_onLevelEditorCreationComplete;
}
