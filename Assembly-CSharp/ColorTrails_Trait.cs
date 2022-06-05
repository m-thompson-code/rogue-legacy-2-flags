using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000580 RID: 1408
public class ColorTrails_Trait : BaseTrait
{
	// Token: 0x170011EC RID: 4588
	// (get) Token: 0x06002CB8 RID: 11448 RVA: 0x00018C49 File Offset: 0x00016E49
	public override TraitType TraitType
	{
		get
		{
			return TraitType.ColorTrails;
		}
	}

	// Token: 0x06002CB9 RID: 11449 RVA: 0x00018C50 File Offset: 0x00016E50
	protected override void Awake()
	{
		base.Awake();
		this.m_onEnemySummoned = new Action<MonoBehaviour, EventArgs>(this.OnEnemySummoned);
		this.m_onPlayerEnterRoom = new Action<MonoBehaviour, EventArgs>(this.OnPlayerEnterRoom);
	}

	// Token: 0x06002CBA RID: 11450 RVA: 0x00018C7C File Offset: 0x00016E7C
	private IEnumerator Start()
	{
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.EnemySummoned, this.m_onEnemySummoned);
		while (!PlayerManager.IsInstantiated)
		{
			yield return null;
		}
		PlayerController playerController = PlayerManager.GetPlayerController();
		while (!playerController.IsInitialized)
		{
			yield return null;
		}
		this.CreatePlayerColorTrails();
		yield break;
	}

	// Token: 0x06002CBB RID: 11451 RVA: 0x000C6038 File Offset: 0x000C4238
	private void CreatePlayerColorTrails()
	{
		PlayerController playerController = PlayerManager.GetPlayerController();
		Debug.Log("Creating player color trail");
		BaseEffect baseEffect = EffectManager.PlayEffect(playerController.gameObject, playerController.Animator, "ColorTrails_Trait_Effect", playerController.Midpoint, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None);
		Bounds visualBounds = playerController.VisualBounds;
		Vector3 size = visualBounds.size;
		Vector3 size2 = visualBounds.size;
		TrailRenderer componentInChildren = baseEffect.GetComponentInChildren<TrailRenderer>();
		Bounds bounds = componentInChildren.bounds;
		componentInChildren.startColor = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
		componentInChildren.endColor = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
		componentInChildren.widthMultiplier = 1.5f;
		componentInChildren.time = 18f;
		baseEffect.transform.SetParent(playerController.gameObject.transform, true);
		baseEffect.DisableDestroyOnRoomChange = true;
		baseEffect.transform.GetChild(0).gameObject.layer = 29;
		this.m_playerTrailEffect = baseEffect;
	}

	// Token: 0x06002CBC RID: 11452 RVA: 0x00018C8B File Offset: 0x00016E8B
	private void OnDestroy()
	{
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.EnemySummoned, this.m_onEnemySummoned);
		EffectManager.DisableAllEffectWithName("ColorTrails_Trait_Effect");
	}

	// Token: 0x06002CBD RID: 11453 RVA: 0x000C6144 File Offset: 0x000C4344
	private void OnPlayerEnterRoom(MonoBehaviour sender, EventArgs args)
	{
		if (!this.m_playerTrailEffect || (this.m_playerTrailEffect && (!this.m_playerTrailEffect.gameObject.activeSelf || !this.m_playerTrailEffect.Source.CompareTag("Player"))))
		{
			this.CreatePlayerColorTrails();
		}
		if (this.m_playerTrailEffect)
		{
			this.m_playerTrailEffect.GetComponentInChildren<TrailRenderer>().Clear();
		}
		base.StopAllCoroutines();
		BaseRoom currentPlayerRoom = PlayerManager.GetCurrentPlayerRoom();
		if (currentPlayerRoom.BiomeType == BiomeType.HubTown)
		{
			this.EnableTraitOnPizzaGirl();
		}
		foreach (EnemySpawnController enemySpawnController in currentPlayerRoom.SpawnControllerManager.EnemySpawnControllers)
		{
			if (enemySpawnController.ShouldSpawn)
			{
				EnemyController enemyInstance = enemySpawnController.EnemyInstance;
				if (enemyInstance != null)
				{
					base.StartCoroutine(this.EnableTraitCoroutine(enemyInstance));
				}
			}
		}
	}

	// Token: 0x06002CBE RID: 11454 RVA: 0x000C6218 File Offset: 0x000C4418
	private void EnableTraitOnPizzaGirl()
	{
		PropSpawnController propSpawnController = PlayerManager.GetCurrentPlayerRoom().gameObject.FindObjectReference("PizzaGirl", false, false);
		Prop prop = null;
		if (propSpawnController)
		{
			prop = propSpawnController.PropInstance;
		}
		if (prop && NPCType_RL.IsNPCUnlocked(NPCType.PizzaGirl))
		{
			Vector3 position = prop.transform.position;
			position.y += prop.HitboxController.GetCollider(HitboxType.Platform).bounds.extents.y;
			BaseEffect baseEffect = EffectManager.PlayEffect(prop.gameObject, prop.Animators[0], "ColorTrails_Trait_Effect", position, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None);
			TrailRenderer componentInChildren = baseEffect.GetComponentInChildren<TrailRenderer>();
			componentInChildren.startColor = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
			componentInChildren.endColor = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
			componentInChildren.widthMultiplier = 1.5f;
			componentInChildren.time = 18f;
			baseEffect.transform.GetChild(0).gameObject.layer = 24;
			baseEffect.transform.SetParent(prop.gameObject.transform, true);
		}
	}

	// Token: 0x06002CBF RID: 11455 RVA: 0x000C6354 File Offset: 0x000C4554
	private void OnEnemySummoned(MonoBehaviour sender, EventArgs args)
	{
		EnemySummonedEventArgs enemySummonedEventArgs = args as EnemySummonedEventArgs;
		if (enemySummonedEventArgs != null)
		{
			EnemyController summonedEnemy = enemySummonedEventArgs.SummonedEnemy;
			if (summonedEnemy)
			{
				base.StartCoroutine(this.EnableTraitCoroutine(summonedEnemy));
			}
		}
	}

	// Token: 0x06002CC0 RID: 11456 RVA: 0x00018CB0 File Offset: 0x00016EB0
	private IEnumerator EnableTraitCoroutine(EnemyController enemyInstance)
	{
		while (!enemyInstance.IsInitialized)
		{
			yield return null;
		}
		if (enemyInstance.EnemyType == EnemyType.Zombie)
		{
			yield break;
		}
		BaseEffect baseEffect = EffectManager.PlayEffect(enemyInstance.gameObject, enemyInstance.Animator, "ColorTrails_Trait_Effect", enemyInstance.Midpoint, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None);
		TrailRenderer componentInChildren = baseEffect.GetComponentInChildren<TrailRenderer>();
		componentInChildren.startColor = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
		componentInChildren.endColor = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
		componentInChildren.widthMultiplier = 1.5f;
		componentInChildren.time = 18f;
		baseEffect.transform.GetChild(0).gameObject.layer = 29;
		baseEffect.transform.SetParent(enemyInstance.gameObject.transform, true);
		yield break;
	}

	// Token: 0x04002585 RID: 9605
	private BaseEffect m_playerTrailEffect;

	// Token: 0x04002586 RID: 9606
	private Action<MonoBehaviour, EventArgs> m_onPlayerEnterRoom;

	// Token: 0x04002587 RID: 9607
	private Action<MonoBehaviour, EventArgs> m_onEnemySummoned;

	// Token: 0x04002588 RID: 9608
	public const string TRAIT_EFFECT_NAME = "ColorTrails_Trait_Effect";
}
