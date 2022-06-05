using System;
using System.Collections;
using System.Linq;
using UnityEngine;

// Token: 0x02000335 RID: 821
public class EnemiesCensored_Trait : BaseTrait
{
	// Token: 0x17000DB8 RID: 3512
	// (get) Token: 0x06001FF1 RID: 8177 RVA: 0x00065D63 File Offset: 0x00063F63
	public override TraitType TraitType
	{
		get
		{
			return TraitType.EnemiesCensored;
		}
	}

	// Token: 0x06001FF2 RID: 8178 RVA: 0x00065D6A File Offset: 0x00063F6A
	protected override void Awake()
	{
		base.Awake();
		this.m_onPlayerEnterRoom = new Action<MonoBehaviour, EventArgs>(this.OnPlayerEnterRoom);
		this.m_onEnemySummoned = new Action<MonoBehaviour, EventArgs>(this.OnEnemySummoned);
	}

	// Token: 0x06001FF3 RID: 8179 RVA: 0x00065D96 File Offset: 0x00063F96
	private void Start()
	{
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.EnemySummoned, this.m_onEnemySummoned);
	}

	// Token: 0x06001FF4 RID: 8180 RVA: 0x00065DB1 File Offset: 0x00063FB1
	private void OnDestroy()
	{
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.EnemySummoned, this.m_onEnemySummoned);
	}

	// Token: 0x06001FF5 RID: 8181 RVA: 0x00065DCC File Offset: 0x00063FCC
	public override void AssignGreenMask()
	{
		base.AssignGreenMask();
		this.m_postProcessOverrideController.Profile.OverridePixelGreenChannel = true;
		this.m_postProcessOverrideController.Profile.PixelGreenChannel = this.m_postProcessOverrideController.Profile.PixelRedChannel;
		this.m_postProcessOverrideController.Profile.PixelRedChannel = 0f;
		this.m_postProcessOverrideController.Profile.OverridePixelRedChannel = false;
	}

	// Token: 0x06001FF6 RID: 8182 RVA: 0x00065E38 File Offset: 0x00064038
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

	// Token: 0x06001FF7 RID: 8183 RVA: 0x00065E6C File Offset: 0x0006406C
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

	// Token: 0x06001FF8 RID: 8184 RVA: 0x00065EC6 File Offset: 0x000640C6
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

	// Token: 0x06001FF9 RID: 8185 RVA: 0x00065ED8 File Offset: 0x000640D8
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

	// Token: 0x04001C45 RID: 7237
	public const string TRAIT_EFFECT_NAME = "EnemiesCensored_Trait_Effect";

	// Token: 0x04001C46 RID: 7238
	private Action<MonoBehaviour, EventArgs> m_onPlayerEnterRoom;

	// Token: 0x04001C47 RID: 7239
	private Action<MonoBehaviour, EventArgs> m_onEnemySummoned;
}
