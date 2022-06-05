using System;
using System.Collections;
using System.Diagnostics;
using GameEventTracking;
using SceneManagement_RL;
using UnityEngine;

// Token: 0x02000B34 RID: 2868
[RequireComponent(typeof(BiomeCreator))]
public class LevelManager_RL : MonoBehaviour
{
	// Token: 0x17001D31 RID: 7473
	// (get) Token: 0x060056B3 RID: 22195 RVA: 0x0002F262 File Offset: 0x0002D462
	// (set) Token: 0x060056B4 RID: 22196 RVA: 0x0002F26A File Offset: 0x0002D46A
	public bool IsComplete { get; private set; }

	// Token: 0x17001D32 RID: 7474
	// (get) Token: 0x060056B5 RID: 22197 RVA: 0x00003CD2 File Offset: 0x00001ED2
	// (set) Token: 0x060056B6 RID: 22198 RVA: 0x0002F273 File Offset: 0x0002D473
	public LevelManagerMode Mode
	{
		get
		{
			return LevelManagerMode.Standard;
		}
		private set
		{
			this.m_mode = value;
		}
	}

	// Token: 0x17001D33 RID: 7475
	// (get) Token: 0x060056B7 RID: 22199 RVA: 0x00003DA1 File Offset: 0x00001FA1
	public bool InstantiatePlayer
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17001D34 RID: 7476
	// (get) Token: 0x060056B8 RID: 22200 RVA: 0x0002F27C File Offset: 0x0002D47C
	// (set) Token: 0x060056B9 RID: 22201 RVA: 0x0002F28F File Offset: 0x0002D48F
	public bool GenerateRoomWeights
	{
		get
		{
			return this.Mode == LevelManagerMode.Debug && this.m_generateRoomWeights;
		}
		set
		{
			this.m_generateRoomWeights = value;
		}
	}

	// Token: 0x060056BA RID: 22202 RVA: 0x0002F298 File Offset: 0x0002D498
	private void Awake()
	{
		if (!LevelManager_RL.m_instance)
		{
			LevelManager_RL.m_instance = this;
			this.m_worldBuilder = base.gameObject.GetComponent<WorldBuilder>();
			this.m_resetSeedOnFail = true;
			return;
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x060056BB RID: 22203 RVA: 0x0002F2D0 File Offset: 0x0002D4D0
	private void Start()
	{
		LevelManager_RL.Reset();
		if (this.Mode == LevelManagerMode.Standard)
		{
			LevelManager_RL.m_buildWorldCoroutine = base.StartCoroutine(this.BuildWorldCoroutine());
			return;
		}
		LevelManager_RL.m_buildWorldCoroutine = base.StartCoroutine(this.BuildDebugWorldCoroutine());
	}

	// Token: 0x060056BC RID: 22204 RVA: 0x0002F302 File Offset: 0x0002D502
	private IEnumerator BuildDebugWorldCoroutine()
	{
		UnityEngine.Debug.LogFormat("| {0} | <color=yellow>WARNING: Debug Mode is enabled! Unless you're Paul, you probably don't want this.</color>", new object[]
		{
			this
		});
		yield return new WaitUntil(() => RNGManager.IsInstantiated);
		Stopwatch stopWatch = new Stopwatch();
		stopWatch.Start();
		int num;
		for (int seed = this.m_seedRange.x; seed <= this.m_seedRange.y; seed = num + 1)
		{
			if (seed != this.m_seedRange.x)
			{
				if (this.m_waitBetweenLoops == null)
				{
					this.m_waitBetweenLoops = new WaitForSeconds(this.m_timeBetweenLoops);
				}
				yield return this.m_waitBetweenLoops;
			}
			LevelManager_RL.Reset();
			RNGManager.Reset(seed);
			WorldBuilder.CreateWorld();
			if (this.m_waitUntilWorldBuilderNotRunning == null)
			{
				this.m_waitUntilWorldBuilderNotRunning = new WaitUntil(() => WorldBuilder.State != BiomeBuildStateID.Running);
			}
			yield return this.m_waitUntilWorldBuilderNotRunning;
			this.IsComplete = true;
			num = seed;
		}
		stopWatch.Stop();
		LevelManager_RL.m_buildWorldCoroutine = null;
		yield break;
	}

	// Token: 0x060056BD RID: 22205 RVA: 0x0002F311 File Offset: 0x0002D511
	private IEnumerator BuildWorldCoroutine()
	{
		if (!this.m_resetSeedOnFail)
		{
			this.m_maxNumberAttempts = 1;
		}
		int attemptCount = 0;
		while (WorldBuilder.State != BiomeBuildStateID.Complete && attemptCount < this.m_maxNumberAttempts)
		{
			if (this.m_testProfileIndex != -1)
			{
				if (true)
				{
					this.LoadStageSaveData(this.m_testProfileIndex);
				}
			}
			else if ((SceneLoader_RL.PreviousScene == SceneLoadingUtility.GetSceneName(SceneID.MainMenu) || (SceneLoader_RL.PreviousScene != SceneLoadingUtility.GetSceneName(SceneID.MainMenu) && SaveManager.PlayerSaveData.CastleLockState != CastleLockState.NotLocked)) && SceneLoader_RL.CurrentScene == SceneLoadingUtility.GetSceneName(SceneID.World))
			{
				if (SceneLoader_RL.PreviousScene != SceneLoadingUtility.GetSceneName(SceneID.MainMenu) && SaveManager.PlayerSaveData.CastleLockState != CastleLockState.NotLocked)
				{
					RNGSeedManager.SetMasterSeedOverride(SaveManager.PlayerSaveData.PreviousMasterSeed);
				}
				else
				{
					RNGSeedManager.SetMasterSeedOverride(SaveManager.PlayerSaveData.MasterSeed);
				}
				RNGSeedManager.GenerateNewSeed(SceneLoadingUtility.GetSceneName(SceneID.World));
				RNGManager.InitializeRandomNumberGenerators(RNGSeedManager.MasterSeedOverride);
				RNGManager.PrintSeedsToConsole();
				this.LoadStageSaveData(SaveManager.CurrentProfile);
			}
			else
			{
				if (SceneLoadingUtility.ActiveScene.name == SceneLoadingUtility.GetSceneName(SceneID.World))
				{
					if (!RNGSeedManager.IsMasterSeedOverrideSet)
					{
						RNGSeedManager.SetMasterSeedOverride(Environment.TickCount);
					}
					RNGSeedManager.GenerateNewSeed(SceneLoadingUtility.GetSceneName(SceneID.World));
					RNGManager.InitializeRandomNumberGenerators(RNGSeedManager.GetCurrentSeed(SceneLoadingUtility.GetSceneName(SceneID.World)));
				}
				else
				{
					RNGManager.Reset();
				}
				WorldBuilder.CreateWorld();
			}
			if (this.m_waitUntilWorldBuilderNotRunning == null)
			{
				this.m_waitUntilWorldBuilderNotRunning = new WaitUntil(() => WorldBuilder.State != BiomeBuildStateID.Running);
			}
			yield return this.m_waitUntilWorldBuilderNotRunning;
			if (WorldBuilder.State == BiomeBuildStateID.Complete)
			{
				this.OnStandardBuildComplete();
			}
			else
			{
				LevelManager_RL.OnStandardBuildFail();
			}
			int num = attemptCount;
			attemptCount = num + 1;
		}
		LevelManager_RL.m_buildWorldCoroutine = null;
		yield break;
	}

	// Token: 0x060056BE RID: 22206 RVA: 0x00148BB0 File Offset: 0x00146DB0
	private LOAD_RESULT LoadStageSaveData(int profileIndex)
	{
		object obj = null;
		LOAD_RESULT load_RESULT = SaveManager.LoadGameData(profileIndex, SaveDataType.Stage, out obj);
		if (load_RESULT == LOAD_RESULT.OK)
		{
			StageSaveData stageSaveData = SaveManager.StageSaveData = (obj as StageSaveData);
			SaveManager.StageSaveData.CreateAllBiomeLookupTable();
			WorldBuilder.CreateWorld(stageSaveData);
		}
		return load_RESULT;
	}

	// Token: 0x060056BF RID: 22207 RVA: 0x0002F320 File Offset: 0x0002D520
	private void OnDestroy()
	{
		LevelManager_RL.m_instance = null;
		if (LevelManager_RL.m_buildWorldCoroutine != null)
		{
			base.StopCoroutine(LevelManager_RL.m_buildWorldCoroutine);
			LevelManager_RL.m_buildWorldCoroutine = null;
		}
	}

	// Token: 0x060056C0 RID: 22208 RVA: 0x00148BE8 File Offset: 0x00146DE8
	private void OnStandardBuildComplete()
	{
		this.IsComplete = true;
		if (this.InstantiatePlayer)
		{
			global::AnimatorUtility.ClearAnimatorTables();
			GC.Collect();
			if (!PlayerManager.IsInstantiated)
			{
				PlayerManager.GetPlayer();
			}
			this.m_worldBuilder.StartingRoom.PlacePlayerInRoom(null);
		}
		if (PlayerManager.IsInstantiated)
		{
			PlayerController playerController = PlayerManager.GetPlayerController();
			if (!playerController.IsFacingRight)
			{
				playerController.CharacterCorgi.Flip(false, false);
			}
			if (!RewiredMapController.IsInCutscene)
			{
				base.StartCoroutine(this.WaitForTransitionPlayerGravityCoroutine(playerController));
			}
		}
		MapController.SetCameraFollowIsOn(true);
		MapController.FollowMode = CameraFollowMode.Constant;
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.LevelManagerStateChange, this, new LevelManagerStateChangeEventArgs(LevelManagerState.BuildComplete));
	}

	// Token: 0x060056C1 RID: 22209 RVA: 0x0002F340 File Offset: 0x0002D540
	private IEnumerator WaitForTransitionPlayerGravityCoroutine(PlayerController playerController)
	{
		LoadingGate_SceneTransition gateTransition = TransitionLibrary.GetTransitionInstance(SceneLoader_RL.CurrentTransitionID) as LoadingGate_SceneTransition;
		if (gateTransition)
		{
			while (!gateTransition.DropPlayer)
			{
				yield return null;
			}
		}
		else
		{
			float delay = Time.unscaledTime + 1f;
			while (Time.unscaledTime < delay)
			{
				yield return null;
			}
		}
		playerController.ControllerCorgi.GravityActive(true);
		yield break;
	}

	// Token: 0x060056C2 RID: 22210 RVA: 0x00148C80 File Offset: 0x00146E80
	private static void OnStandardBuildFail()
	{
		LevelManager_RL.Reset();
		WorldBuilder.Instance.PreserveFirstBiomeOverride();
		if (LevelManager_RL.m_instance.Mode == LevelManagerMode.Standard && LevelManager_RL.m_instance.m_resetSeedOnFail)
		{
			if (RNGSeedManager.IsOverrideSet)
			{
				RNGSeedManager.ClearOverride();
			}
			RNGSeedManager.GenerateNewSeed(LevelManager_RL.m_instance.gameObject.scene.name);
		}
	}

	// Token: 0x060056C3 RID: 22211 RVA: 0x00148CDC File Offset: 0x00146EDC
	private static void Reset()
	{
		LevelManager_RL.m_instance.IsComplete = false;
		if (PlayerManager.IsInstantiated)
		{
			PlayerManager.GetPlayerController().ResetCharacter();
		}
		GridController.Reset();
		WorldBuilder.Reset();
		if (EnemyManager.IsInitialized)
		{
			EnemyManager.DisableAllEnemies();
		}
		if (MapController.IsInitialized)
		{
			MapController.Reset();
		}
		if (ProjectileManager.IsInitialized)
		{
			ProjectileManager.Reset();
		}
		if (ItemDropManager.IsInitialized)
		{
			ItemDropManager.Reset();
		}
		if (GameEventTrackerManager.IsInstantiated)
		{
			GameEventTrackerManager.Reset();
		}
	}

	// Token: 0x0400401E RID: 16414
	[SerializeField]
	private bool m_instantiatePlayer = true;

	// Token: 0x0400401F RID: 16415
	[SerializeField]
	private int m_maxNumberAttempts = 10;

	// Token: 0x04004020 RID: 16416
	[SerializeField]
	private LevelManagerMode m_mode;

	// Token: 0x04004021 RID: 16417
	[SerializeField]
	private Vector2Int m_seedRange = new Vector2Int(0, 0);

	// Token: 0x04004022 RID: 16418
	[SerializeField]
	private float m_timeBetweenLoops = 5f;

	// Token: 0x04004023 RID: 16419
	[SerializeField]
	private bool m_exportRoomLibrary;

	// Token: 0x04004024 RID: 16420
	[SerializeField]
	private int m_testProfileIndex = -1;

	// Token: 0x04004025 RID: 16421
	[SerializeField]
	private bool m_generateRoomWeights;

	// Token: 0x04004026 RID: 16422
	private static Coroutine m_buildWorldCoroutine;

	// Token: 0x04004027 RID: 16423
	private static LevelManager_RL m_instance;

	// Token: 0x04004028 RID: 16424
	private WorldBuilder m_worldBuilder;

	// Token: 0x04004029 RID: 16425
	private WaitUntil m_waitUntilWorldBuilderNotRunning;

	// Token: 0x0400402A RID: 16426
	private WaitForSeconds m_waitBetweenLoops;

	// Token: 0x0400402B RID: 16427
	private bool m_resetSeedOnFail;
}
