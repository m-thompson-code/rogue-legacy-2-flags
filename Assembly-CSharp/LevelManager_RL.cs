using System;
using System.Collections;
using System.Diagnostics;
using GameEventTracking;
using SceneManagement_RL;
using UnityEngine;

// Token: 0x0200069E RID: 1694
[RequireComponent(typeof(BiomeCreator))]
public class LevelManager_RL : MonoBehaviour
{
	// Token: 0x1700154F RID: 5455
	// (get) Token: 0x06003DD9 RID: 15833 RVA: 0x000D8504 File Offset: 0x000D6704
	// (set) Token: 0x06003DDA RID: 15834 RVA: 0x000D850C File Offset: 0x000D670C
	public bool IsComplete { get; private set; }

	// Token: 0x17001550 RID: 5456
	// (get) Token: 0x06003DDB RID: 15835 RVA: 0x000D8515 File Offset: 0x000D6715
	// (set) Token: 0x06003DDC RID: 15836 RVA: 0x000D8518 File Offset: 0x000D6718
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

	// Token: 0x17001551 RID: 5457
	// (get) Token: 0x06003DDD RID: 15837 RVA: 0x000D8521 File Offset: 0x000D6721
	public bool InstantiatePlayer
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17001552 RID: 5458
	// (get) Token: 0x06003DDE RID: 15838 RVA: 0x000D8524 File Offset: 0x000D6724
	// (set) Token: 0x06003DDF RID: 15839 RVA: 0x000D8537 File Offset: 0x000D6737
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

	// Token: 0x06003DE0 RID: 15840 RVA: 0x000D8540 File Offset: 0x000D6740
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

	// Token: 0x06003DE1 RID: 15841 RVA: 0x000D8578 File Offset: 0x000D6778
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

	// Token: 0x06003DE2 RID: 15842 RVA: 0x000D85AA File Offset: 0x000D67AA
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

	// Token: 0x06003DE3 RID: 15843 RVA: 0x000D85B9 File Offset: 0x000D67B9
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

	// Token: 0x06003DE4 RID: 15844 RVA: 0x000D85C8 File Offset: 0x000D67C8
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

	// Token: 0x06003DE5 RID: 15845 RVA: 0x000D85FE File Offset: 0x000D67FE
	private void OnDestroy()
	{
		LevelManager_RL.m_instance = null;
		if (LevelManager_RL.m_buildWorldCoroutine != null)
		{
			base.StopCoroutine(LevelManager_RL.m_buildWorldCoroutine);
			LevelManager_RL.m_buildWorldCoroutine = null;
		}
	}

	// Token: 0x06003DE6 RID: 15846 RVA: 0x000D8620 File Offset: 0x000D6820
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

	// Token: 0x06003DE7 RID: 15847 RVA: 0x000D86B6 File Offset: 0x000D68B6
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

	// Token: 0x06003DE8 RID: 15848 RVA: 0x000D86C8 File Offset: 0x000D68C8
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

	// Token: 0x06003DE9 RID: 15849 RVA: 0x000D8724 File Offset: 0x000D6924
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

	// Token: 0x04002E17 RID: 11799
	[SerializeField]
	private bool m_instantiatePlayer = true;

	// Token: 0x04002E18 RID: 11800
	[SerializeField]
	private int m_maxNumberAttempts = 10;

	// Token: 0x04002E19 RID: 11801
	[SerializeField]
	private LevelManagerMode m_mode;

	// Token: 0x04002E1A RID: 11802
	[SerializeField]
	private Vector2Int m_seedRange = new Vector2Int(0, 0);

	// Token: 0x04002E1B RID: 11803
	[SerializeField]
	private float m_timeBetweenLoops = 5f;

	// Token: 0x04002E1C RID: 11804
	[SerializeField]
	private bool m_exportRoomLibrary;

	// Token: 0x04002E1D RID: 11805
	[SerializeField]
	private int m_testProfileIndex = -1;

	// Token: 0x04002E1E RID: 11806
	[SerializeField]
	private bool m_generateRoomWeights;

	// Token: 0x04002E1F RID: 11807
	private static Coroutine m_buildWorldCoroutine;

	// Token: 0x04002E20 RID: 11808
	private static LevelManager_RL m_instance;

	// Token: 0x04002E21 RID: 11809
	private WorldBuilder m_worldBuilder;

	// Token: 0x04002E22 RID: 11810
	private WaitUntil m_waitUntilWorldBuilderNotRunning;

	// Token: 0x04002E23 RID: 11811
	private WaitForSeconds m_waitBetweenLoops;

	// Token: 0x04002E24 RID: 11812
	private bool m_resetSeedOnFail;
}
