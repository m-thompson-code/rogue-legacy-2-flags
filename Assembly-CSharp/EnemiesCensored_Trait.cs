using System;
using System.Collections;
using System.Linq;
using UnityEngine;

// Token: 0x0200058F RID: 1423
public class EnemiesCensored_Trait : BaseTrait
{
	// Token: 0x170011FD RID: 4605
	// (get) Token: 0x06002CFD RID: 11517 RVA: 0x00018DD1 File Offset: 0x00016FD1
	public override TraitType TraitType
	{
		get
		{
			return TraitType.EnemiesCensored;
		}
	}

	// Token: 0x06002CFE RID: 11518 RVA: 0x00018DD8 File Offset: 0x00016FD8
	protected override void Awake()
	{
		base.Awake();
		this.m_onPlayerEnterRoom = new Action<MonoBehaviour, EventArgs>(this.OnPlayerEnterRoom);
		this.m_onEnemySummoned = new Action<MonoBehaviour, EventArgs>(this.OnEnemySummoned);
	}

	// Token: 0x06002CFF RID: 11519 RVA: 0x00018E04 File Offset: 0x00017004
	private void Start()
	{
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.EnemySummoned, this.m_onEnemySummoned);
	}

	// Token: 0x06002D00 RID: 11520 RVA: 0x00018E1F File Offset: 0x0001701F
	private void OnDestroy()
	{
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.EnemySummoned, this.m_onEnemySummoned);
	}

	// Token: 0x06002D01 RID: 11521 RVA: 0x000C6858 File Offset: 0x000C4A58
	public override void AssignGreenMask()
	{
		base.AssignGreenMask();
		this.m_postProcessOverrideController.Profile.OverridePixelGreenChannel = true;
		this.m_postProcessOverrideController.Profile.PixelGreenChannel = this.m_postProcessOverrideController.Profile.PixelRedChannel;
		this.m_postProcessOverrideController.Profile.PixelRedChannel = 0f;
		this.m_postProcessOverrideController.Profile.OverridePixelRedChannel = false;
	}

	// Token: 0x06002D02 RID: 11522 RVA: 0x000C68C4 File Offset: 0x000C4AC4
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

	// Token: 0x06002D03 RID: 11523 RVA: 0x000C68F8 File Offset: 0x000C4AF8
	private void OnPlayerEnterRoom(MonoBehaviour sender, EventArgs args)
	{
		base.StopAllCoroutines();
		foreach (EnemySpawnController enemySpawnController in PlayerManager.GetCurrentPlayerRoom().SpawnControllerManager.EnemySpawnControllers)
		{
			if (enemySpawnController.ShouldSpawn)
			{
				EnemyController enemyInstance = enemySpawnController.EnemyInstance;
				if (enemyInstance)
				{
					base.StartCoroutine(this.EnableTraitCoroutine(enemyInstance));
				}
			}
		}
	}

	// Token: 0x06002D04 RID: 11524 RVA: 0x00018E3A File Offset: 0x0001703A
	private IEnumerator EnableTraitCoroutine(EnemyController enemyInstance)
	{
		if (Trait_EV.TRAIT_EFFECT_EXCEPTION_ARRAY.Contains(enemyInstance.EnemyType))
		{
			yield break;
		}
		while (!enemyInstance.IsInitialized)
		{
			yield return null;
		}
		if (enemyInstance.EnemyType == EnemyType.Zombie)
		{
			yield break;
		}
		if (!enemyInstance.IsSummoned)
		{
			if (!enemyInstance.ActivatedByFairyRoomTrigger)
			{
				goto IL_D0;
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
		IL_D0:
		EnemiesCensored_Trait.ApplyCensoredEffect(enemyInstance);
		yield break;
	}

	// Token: 0x06002D05 RID: 11525 RVA: 0x000C6954 File Offset: 0x000C4B54
	public static BaseEffect ApplyCensoredEffect(BaseCharacterController charController)
	{
		BaseEffect baseEffect = EffectManager.PlayEffect(charController.gameObject, charController.Animator, "EnemiesCensored_Trait_Effect", charController.Midpoint, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None);
		BaseTrait activeTrait = TraitManager.GetActiveTrait(TraitType.EnemiesCensored);
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
		Bounds visualBounds = charController.VisualBounds;
		float num = 1f * charController.transform.lossyScale.x;
		float num2 = visualBounds.size.x + num;
		float num3 = visualBounds.size.y + num;
		bool flag = num3 > num2;
		Bounds bounds = new Bounds(baseEffect.transform.localPosition, new Vector3(0.52f, 0.52f, 0.52f));
		float num4 = flag ? (num3 / bounds.size.y) : (num2 / bounds.size.x);
		Vector2 vector = charController.VisualBoundsObj.Collider.offset * charController.gameObject.transform.localScale.x;
		baseEffect.transform.position = charController.gameObject.transform.localPosition + new Vector3(vector.x, vector.y, 0f);
		baseEffect.transform.localScale = new Vector3(num4, num4, baseEffect.transform.localScale.z);
		baseEffect.transform.SetParent(charController.gameObject.transform, true);
		return baseEffect;
	}

	// Token: 0x0400259D RID: 9629
	public const string TRAIT_EFFECT_NAME = "EnemiesCensored_Trait_Effect";

	// Token: 0x0400259E RID: 9630
	private Action<MonoBehaviour, EventArgs> m_onPlayerEnterRoom;

	// Token: 0x0400259F RID: 9631
	private Action<MonoBehaviour, EventArgs> m_onEnemySummoned;
}
