using System;
using System.Collections;
using Sigtrap.Relays;
using UnityEngine;

// Token: 0x0200032C RID: 812
public class CheerOnKills_Trait : BaseTrait
{
	// Token: 0x17000DB0 RID: 3504
	// (get) Token: 0x06001FC2 RID: 8130 RVA: 0x00065436 File Offset: 0x00063636
	public override TraitType TraitType
	{
		get
		{
			return TraitType.CheerOnKills;
		}
	}

	// Token: 0x06001FC3 RID: 8131 RVA: 0x0006543D File Offset: 0x0006363D
	protected override void Awake()
	{
		base.Awake();
		this.m_onPlayerEnterRoom = new Action<MonoBehaviour, EventArgs>(this.OnPlayerEnterRoom);
		this.m_onEnemySummoned = new Action<MonoBehaviour, EventArgs>(this.OnEnemySummoned);
		this.m_onEnemyDeath = new Action<MonoBehaviour, EventArgs>(this.OnEnemyDeath);
	}

	// Token: 0x06001FC4 RID: 8132 RVA: 0x0006547B File Offset: 0x0006367B
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

	// Token: 0x06001FC5 RID: 8133 RVA: 0x0006548C File Offset: 0x0006368C
	public override void AssignGreenMask()
	{
		base.AssignGreenMask();
		this.m_postProcessOverrideController.Profile.OverrideTintGreenChannel = true;
		this.m_postProcessOverrideController.Profile.TintGreenChannel = this.m_postProcessOverrideController.Profile.TintRedChannel;
		this.m_postProcessOverrideController.Profile.TintRedChannel = 0f;
		this.m_postProcessOverrideController.Profile.OverrideTintRedChannel = false;
	}

	// Token: 0x06001FC6 RID: 8134 RVA: 0x000654F8 File Offset: 0x000636F8
	private void OnEnemyDeath(MonoBehaviour sender, EventArgs args)
	{
		EnemyDeathEventArgs eventArgs = args as EnemyDeathEventArgs;
		this.m_numEnemiesKilled++;
		this.RunCheerCheck(eventArgs);
	}

	// Token: 0x06001FC7 RID: 8135 RVA: 0x00065524 File Offset: 0x00063724
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

	// Token: 0x06001FC8 RID: 8136 RVA: 0x000655C4 File Offset: 0x000637C4
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

	// Token: 0x06001FC9 RID: 8137 RVA: 0x00065640 File Offset: 0x00063840
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

	// Token: 0x06001FCA RID: 8138 RVA: 0x00065675 File Offset: 0x00063875
	public void ApplyOnEnemy(EnemyController enemy, bool increment)
	{
		base.StartCoroutine(this.EnableOnEnemyCoroutine(enemy, increment));
	}

	// Token: 0x06001FCB RID: 8139 RVA: 0x00065686 File Offset: 0x00063886
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

	// Token: 0x06001FCC RID: 8140 RVA: 0x000656A3 File Offset: 0x000638A3
	public override void DisableOnDeath()
	{
		this.PlayerDeathRelay.Dispatch();
		EffectManager.DisableAllEffectWithName("CheerOnKills_Mask_Effect");
		this.m_traitMask.gameObject.SetActive(false);
		base.DisableOnDeath();
	}

	// Token: 0x06001FCD RID: 8141 RVA: 0x000656D4 File Offset: 0x000638D4
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

	// Token: 0x04001C37 RID: 7223
	public Relay<EnemyTypeAndRank> CheerAfterKillRelay = new Relay<EnemyTypeAndRank>();

	// Token: 0x04001C38 RID: 7224
	public Relay RosesThrownRelay = new Relay();

	// Token: 0x04001C39 RID: 7225
	public Relay PlayerDeathRelay = new Relay();

	// Token: 0x04001C3A RID: 7226
	private int m_numEnemiesToKill;

	// Token: 0x04001C3B RID: 7227
	private int m_numEnemiesKilled;

	// Token: 0x04001C3C RID: 7228
	private SummonRuleController m_roomSummonRuleController;

	// Token: 0x04001C3D RID: 7229
	private Action<MonoBehaviour, EventArgs> m_onPlayerEnterRoom;

	// Token: 0x04001C3E RID: 7230
	private Action<MonoBehaviour, EventArgs> m_onEnemySummoned;

	// Token: 0x04001C3F RID: 7231
	private Action<MonoBehaviour, EventArgs> m_onEnemyDeath;
}
