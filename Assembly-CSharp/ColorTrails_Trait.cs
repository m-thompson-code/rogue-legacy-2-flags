using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200032D RID: 813
public class ColorTrails_Trait : BaseTrait
{
	// Token: 0x17000DB1 RID: 3505
	// (get) Token: 0x06001FCF RID: 8143 RVA: 0x0006574D File Offset: 0x0006394D
	public override TraitType TraitType
	{
		get
		{
			return TraitType.ColorTrails;
		}
	}

	// Token: 0x06001FD0 RID: 8144 RVA: 0x00065754 File Offset: 0x00063954
	protected override void Awake()
	{
		base.Awake();
		this.m_onEnemySummoned = new Action<MonoBehaviour, EventArgs>(this.OnEnemySummoned);
		this.m_onPlayerEnterRoom = new Action<MonoBehaviour, EventArgs>(this.OnPlayerEnterRoom);
	}

	// Token: 0x06001FD1 RID: 8145 RVA: 0x00065780 File Offset: 0x00063980
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

	// Token: 0x06001FD2 RID: 8146 RVA: 0x00065790 File Offset: 0x00063990
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

	// Token: 0x06001FD3 RID: 8147 RVA: 0x00065899 File Offset: 0x00063A99
	private void OnDestroy()
	{
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.EnemySummoned, this.m_onEnemySummoned);
		EffectManager.DisableAllEffectWithName("ColorTrails_Trait_Effect");
	}

	// Token: 0x06001FD4 RID: 8148 RVA: 0x000658C0 File Offset: 0x00063AC0
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

	// Token: 0x06001FD5 RID: 8149 RVA: 0x00065994 File Offset: 0x00063B94
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

	// Token: 0x06001FD6 RID: 8150 RVA: 0x00065AD0 File Offset: 0x00063CD0
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

	// Token: 0x06001FD7 RID: 8151 RVA: 0x00065B04 File Offset: 0x00063D04
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

	// Token: 0x04001C40 RID: 7232
	private BaseEffect m_playerTrailEffect;

	// Token: 0x04001C41 RID: 7233
	private Action<MonoBehaviour, EventArgs> m_onPlayerEnterRoom;

	// Token: 0x04001C42 RID: 7234
	private Action<MonoBehaviour, EventArgs> m_onEnemySummoned;

	// Token: 0x04001C43 RID: 7235
	public const string TRAIT_EFFECT_NAME = "ColorTrails_Trait_Effect";
}
