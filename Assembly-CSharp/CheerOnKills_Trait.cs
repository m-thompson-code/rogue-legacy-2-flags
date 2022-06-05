using System;
using System.Collections;
using Sigtrap.Relays;
using UnityEngine;

// Token: 0x0200057D RID: 1405
public class CheerOnKills_Trait : BaseTrait
{
	// Token: 0x170011E7 RID: 4583
	// (get) Token: 0x06002C9F RID: 11423 RVA: 0x00018B42 File Offset: 0x00016D42
	public override TraitType TraitType
	{
		get
		{
			return TraitType.CheerOnKills;
		}
	}

	// Token: 0x06002CA0 RID: 11424 RVA: 0x00018B49 File Offset: 0x00016D49
	protected override void Awake()
	{
		base.Awake();
		this.m_onPlayerEnterRoom = new Action<MonoBehaviour, EventArgs>(this.OnPlayerEnterRoom);
		this.m_onEnemySummoned = new Action<MonoBehaviour, EventArgs>(this.OnEnemySummoned);
		this.m_onEnemyDeath = new Action<MonoBehaviour, EventArgs>(this.OnEnemyDeath);
	}

	// Token: 0x06002CA1 RID: 11425 RVA: 0x00018B87 File Offset: 0x00016D87
	private IEnumerator Start()
	{
		while (!PlayerManager.IsInstantiated)
		{
			yield return null;
		}
		PlayerController playerController = PlayerManager.GetPlayerController();
		this.m_traitMask.transform.position = playerController.Midpoint;
		this.m_traitMask.transform.SetParent(playerController.transform, true);
		Vector3 localScale = new Vector3(5.5f, 5.5f, 1f);
		this.m_traitMask.transform.localScale = localScale;
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.EnemyDeath, this.m_onEnemyDeath);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.EnemySummoned, this.m_onEnemySummoned);
		yield break;
	}

	// Token: 0x06002CA2 RID: 11426 RVA: 0x000C53C4 File Offset: 0x000C35C4
	public override void AssignGreenMask()
	{
		base.AssignGreenMask();
		this.m_postProcessOverrideController.Profile.OverrideTintGreenChannel = true;
		this.m_postProcessOverrideController.Profile.TintGreenChannel = this.m_postProcessOverrideController.Profile.TintRedChannel;
		this.m_postProcessOverrideController.Profile.TintRedChannel = 0f;
		this.m_postProcessOverrideController.Profile.OverrideTintRedChannel = false;
	}

	// Token: 0x06002CA3 RID: 11427 RVA: 0x000C5B58 File Offset: 0x000C3D58
	private void OnEnemyDeath(MonoBehaviour sender, EventArgs args)
	{
		EnemyDeathEventArgs eventArgs = args as EnemyDeathEventArgs;
		this.m_numEnemiesKilled++;
		this.RunCheerCheck(eventArgs);
	}

	// Token: 0x06002CA4 RID: 11428 RVA: 0x000C5B84 File Offset: 0x000C3D84
	private void RunCheerCheck(EnemyDeathEventArgs eventArgs)
	{
		bool flag = this.m_numEnemiesKilled >= this.m_numEnemiesToKill && this.m_numEnemiesToKill > 0;
		if (this.m_roomSummonRuleController && this.m_roomSummonRuleController.StillSummoning)
		{
			flag = false;
		}
		if (flag)
		{
			EffectManager.PlayEffect(base.gameObject, null, "CheerOnKills_Roses_Effect", CameraController.ForegroundOrthoCam.transform.position, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None);
			this.RosesThrownRelay.Dispatch();
			this.CheerAfterKillRelay.Dispatch(new EnemyTypeAndRank(eventArgs.Victim.EnemyType, eventArgs.Victim.EnemyRank));
		}
	}

	// Token: 0x06002CA5 RID: 11429 RVA: 0x000C5C24 File Offset: 0x000C3E24
	private void OnPlayerEnterRoom(MonoBehaviour sender, EventArgs args)
	{
		this.m_numEnemiesKilled = 0;
		this.m_numEnemiesToKill = 0;
		base.StopAllCoroutines();
		BaseRoom currentPlayerRoom = PlayerManager.GetCurrentPlayerRoom();
		foreach (EnemySpawnController enemySpawnController in currentPlayerRoom.SpawnControllerManager.EnemySpawnControllers)
		{
			if (enemySpawnController.ShouldSpawn)
			{
				EnemyController enemyInstance = enemySpawnController.EnemyInstance;
				if (enemyInstance != null)
				{
					base.StartCoroutine(this.EnableOnEnemyCoroutine(enemyInstance, true));
				}
			}
		}
		this.m_roomSummonRuleController = currentPlayerRoom.GetComponentInChildren<SummonRuleController>();
	}

	// Token: 0x06002CA6 RID: 11430 RVA: 0x000C5CA0 File Offset: 0x000C3EA0
	private void OnEnemySummoned(MonoBehaviour sender, EventArgs args)
	{
		EnemySummonedEventArgs enemySummonedEventArgs = args as EnemySummonedEventArgs;
		if (enemySummonedEventArgs != null)
		{
			EnemyController summonedEnemy = enemySummonedEventArgs.SummonedEnemy;
			if (summonedEnemy)
			{
				base.StartCoroutine(this.EnableOnEnemyCoroutine(summonedEnemy, true));
			}
		}
	}

	// Token: 0x06002CA7 RID: 11431 RVA: 0x00018B96 File Offset: 0x00016D96
	public void ApplyOnEnemy(EnemyController enemy, bool increment)
	{
		base.StartCoroutine(this.EnableOnEnemyCoroutine(enemy, increment));
	}

	// Token: 0x06002CA8 RID: 11432 RVA: 0x00018BA7 File Offset: 0x00016DA7
	private IEnumerator EnableOnEnemyCoroutine(EnemyController enemyInstance, bool increment)
	{
		while (!enemyInstance.IsInitialized)
		{
			yield return null;
		}
		if (!enemyInstance.IsSummoned)
		{
			if (!enemyInstance.ActivatedByFairyRoomTrigger)
			{
				goto IL_AF;
			}
		}
		while (!enemyInstance.IsBeingSummoned)
		{
			if (!enemyInstance.gameObject.activeSelf)
			{
				yield break;
			}
			yield return null;
		}
		IL_AF:
		if (increment)
		{
			EnemyType enemyType = enemyInstance.EnemyType;
			if (enemyType != EnemyType.BouncySpike && enemyType != EnemyType.Dummy && enemyType != EnemyType.Eggplant)
			{
				this.m_numEnemiesToKill++;
			}
		}
		BaseEffect baseEffect = EffectManager.PlayEffect(enemyInstance.gameObject, enemyInstance.Animator, "CheerOnKills_Mask_Effect", enemyInstance.Midpoint, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None);
		BaseTrait activeTrait = TraitManager.GetActiveTrait(this.TraitType);
		if (activeTrait)
		{
			ChangeEffectRendererColor component = baseEffect.GetComponent<ChangeEffectRendererColor>();
			if (activeTrait.IsFirstTrait)
			{
				component.ChangeColor(Color.red);
			}
			else
			{
				component.ChangeColor(Color.green);
			}
		}
		Bounds visualBounds = enemyInstance.VisualBounds;
		float num = 1f * enemyInstance.transform.lossyScale.x;
		float num2 = visualBounds.size.x + num;
		float num3 = visualBounds.size.y + num;
		bool flag = num3 > num2;
		Bounds bounds = new Bounds(baseEffect.transform.localPosition, new Vector3(5.68f, 5.68f, 5.68f));
		float num4 = flag ? (num3 / bounds.size.y) : (num2 / bounds.size.x);
		baseEffect.transform.localScale = new Vector3(num4, num4, baseEffect.transform.localScale.z);
		baseEffect.transform.SetParent(enemyInstance.gameObject.transform, true);
		yield break;
	}

	// Token: 0x06002CA9 RID: 11433 RVA: 0x00018BC4 File Offset: 0x00016DC4
	public override void DisableOnDeath()
	{
		this.PlayerDeathRelay.Dispatch();
		EffectManager.DisableAllEffectWithName("CheerOnKills_Mask_Effect");
		this.m_traitMask.gameObject.SetActive(false);
		base.DisableOnDeath();
	}

	// Token: 0x06002CAA RID: 11434 RVA: 0x000C5CD8 File Offset: 0x000C3ED8
	private void OnDestroy()
	{
		if (this.m_traitMask)
		{
			UnityEngine.Object.Destroy(this.m_traitMask.gameObject);
		}
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.EnemyDeath, this.m_onEnemyDeath);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.EnemySummoned, this.m_onEnemySummoned);
	}

	// Token: 0x04002573 RID: 9587
	public Relay<EnemyTypeAndRank> CheerAfterKillRelay = new Relay<EnemyTypeAndRank>();

	// Token: 0x04002574 RID: 9588
	public Relay RosesThrownRelay = new Relay();

	// Token: 0x04002575 RID: 9589
	public Relay PlayerDeathRelay = new Relay();

	// Token: 0x04002576 RID: 9590
	private int m_numEnemiesToKill;

	// Token: 0x04002577 RID: 9591
	private int m_numEnemiesKilled;

	// Token: 0x04002578 RID: 9592
	private SummonRuleController m_roomSummonRuleController;

	// Token: 0x04002579 RID: 9593
	private Action<MonoBehaviour, EventArgs> m_onPlayerEnterRoom;

	// Token: 0x0400257A RID: 9594
	private Action<MonoBehaviour, EventArgs> m_onEnemySummoned;

	// Token: 0x0400257B RID: 9595
	private Action<MonoBehaviour, EventArgs> m_onEnemyDeath;
}
