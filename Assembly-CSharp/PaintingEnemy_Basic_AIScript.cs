using System;
using System.Collections;
using FMOD.Studio;
using MoreMountains.CorgiEngine;
using RLAudio;
using UnityEngine;

// Token: 0x02000102 RID: 258
public class PaintingEnemy_Basic_AIScript : BaseAIScript
{
	// Token: 0x060007F0 RID: 2032 RVA: 0x0001B58B File Offset: 0x0001978B
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			"PaintingEnemyProjectile"
		};
	}

	// Token: 0x17000449 RID: 1097
	// (get) Token: 0x060007F1 RID: 2033 RVA: 0x0001B5A1 File Offset: 0x000197A1
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.25f, 0.7f);
		}
	}

	// Token: 0x1700044A RID: 1098
	// (get) Token: 0x060007F2 RID: 2034 RVA: 0x0001B5B2 File Offset: 0x000197B2
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(1.25f, 2f);
		}
	}

	// Token: 0x1700044B RID: 1099
	// (get) Token: 0x060007F3 RID: 2035 RVA: 0x0001B5C3 File Offset: 0x000197C3
	protected override Vector2 WalkAwayDuration
	{
		get
		{
			return new Vector2(1.25f, 2f);
		}
	}

	// Token: 0x1700044C RID: 1100
	// (get) Token: 0x060007F4 RID: 2036 RVA: 0x0001B5D4 File Offset: 0x000197D4
	protected override Vector2 RandomFollowOffsetX
	{
		get
		{
			return Vector2.zero;
		}
	}

	// Token: 0x1700044D RID: 1101
	// (get) Token: 0x060007F5 RID: 2037 RVA: 0x0001B5DB File Offset: 0x000197DB
	protected override Vector2 RandomFollowOffsetY
	{
		get
		{
			return Vector2.zero;
		}
	}

	// Token: 0x1700044E RID: 1102
	// (get) Token: 0x060007F6 RID: 2038 RVA: 0x0001B5E2 File Offset: 0x000197E2
	protected virtual float ProjectileFireInterval
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x060007F7 RID: 2039 RVA: 0x0001B5E9 File Offset: 0x000197E9
	private void Awake()
	{
		this.m_onPlayerHit = new Action<object, CharacterHitEventArgs>(this.OnPlayerHit);
		this.m_onEnemyHit = new Action<object, CharacterHitEventArgs>(this.OnEnemyHit);
		this.m_onEnemyControllerDisable = new Action(this.OnEnemyControllerDisable);
	}

	// Token: 0x060007F8 RID: 2040 RVA: 0x0001B624 File Offset: 0x00019824
	public override void Initialize(EnemyController enemyController)
	{
		base.Initialize(enemyController);
		ObjectReferenceFinder component = base.EnemyController.GetComponent<ObjectReferenceFinder>();
		this.m_paintingRenderer = component.GetObject("Sprite").GetComponent<SpriteRenderer>();
		this.m_enemyFaceRenderer = component.GetObject("EnemyFace").GetComponent<SpriteRenderer>();
		base.EnemyController.LogicController.DisableLogicActivationByDistance = true;
		base.EnemyController.LogicController.OverrideLogicDelay(0f);
		base.EnemyController.OnActivatedRelay.AddListener(new Action<object, EnemyActivationStateChangedEventArgs>(this.Awaken), false);
		base.EnemyController.CharacterHitResponse.OnCharacterHitRelay.AddListener(this.m_onEnemyHit, false);
		base.EnemyController.LockFlip = true;
		this.m_isChasing = false;
		base.EnemyController.OnDisableRelay.AddListener(this.m_onEnemyControllerDisable, false);
		this.OnEnemyControllerDisable();
	}

	// Token: 0x060007F9 RID: 2041 RVA: 0x0001B702 File Offset: 0x00019902
	public override void OnEnemyActivated()
	{
		this.m_projectileCDDuration = Time.time + this.ProjectileFireInterval;
		base.OnEnemyActivated();
	}

	// Token: 0x060007FA RID: 2042 RVA: 0x0001B71C File Offset: 0x0001991C
	public override IEnumerator Idle()
	{
		yield return base.WalkTowards();
		yield break;
	}

	// Token: 0x060007FB RID: 2043 RVA: 0x0001B72C File Offset: 0x0001992C
	private void Shoot()
	{
		float num = CDGHelper.AngleBetweenPts(base.EnemyController.TargetController.Midpoint, base.EnemyController.Midpoint);
		if (base.LogicController.EnemyLogicType == EnemyLogicType.Expert)
		{
			num += 15f;
			this.FireProjectile("PaintingEnemyProjectile", 0, false, num, 1f, true, true, true);
			num -= 30f;
			this.FireProjectile("PaintingEnemyProjectile", 0, false, num, 1f, true, true, true);
		}
		else
		{
			this.FireProjectile("PaintingEnemyProjectile", 0, false, num, 1f, true, true, true);
		}
		this.m_projectileCDDuration = Time.time + this.ProjectileFireInterval;
	}

	// Token: 0x060007FC RID: 2044 RVA: 0x0001B7DB File Offset: 0x000199DB
	private void OnEnable()
	{
		if (PlayerManager.IsInstantiated && !PlayerManager.IsDisposed)
		{
			PlayerManager.GetPlayerController().CharacterHitResponse.OnCharacterHitRelay.AddListener(this.m_onPlayerHit, false);
		}
	}

	// Token: 0x060007FD RID: 2045 RVA: 0x0001B808 File Offset: 0x00019A08
	protected override void OnDisable()
	{
		base.OnDisable();
		if (PlayerManager.IsInstantiated && !PlayerManager.IsDisposed)
		{
			PlayerManager.GetPlayerController().CharacterHitResponse.OnCharacterHitRelay.RemoveListener(this.m_onPlayerHit);
		}
		if (base.IsInitialized)
		{
			base.EnemyController.LogicController.DisableLogicActivationByDistance = true;
		}
		if (this.m_flyLoopEventInstance.isValid())
		{
			AudioManager.Stop(this.m_flyLoopEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}
	}

	// Token: 0x060007FE RID: 2046 RVA: 0x0001B878 File Offset: 0x00019A78
	private void OnDestroy()
	{
		if (base.EnemyController)
		{
			base.EnemyController.OnActivatedRelay.RemoveListener(new Action<object, EnemyActivationStateChangedEventArgs>(this.Awaken));
			base.EnemyController.CharacterHitResponse.OnCharacterHitRelay.RemoveListener(this.m_onEnemyHit);
		}
		if (this.m_flyLoopEventInstance.isValid())
		{
			this.m_flyLoopEventInstance.release();
		}
	}

	// Token: 0x060007FF RID: 2047 RVA: 0x0001B8E4 File Offset: 0x00019AE4
	private void OnPlayerHit(object sender, CharacterHitEventArgs args)
	{
		if (args.Attacker == base.EnemyController)
		{
			base.EnemyController.LogicController.TriggerAggro(null, null);
			if (base.EnemyController.StatusEffectController.HasStatusEffect(StatusEffectType.Enemy_Invuln))
			{
				foreach (Renderer renderer in base.EnemyController.RendererArray)
				{
					if (renderer.sharedMaterial.HasProperty(ShaderID_RL._ShieldToggle))
					{
						renderer.GetPropertyBlock(BaseStatusEffect.m_matBlockHelper_STATIC);
						BaseStatusEffect.m_matBlockHelper_STATIC.SetInt(ShaderID_RL._ShieldToggle, 1);
						renderer.SetPropertyBlock(BaseStatusEffect.m_matBlockHelper_STATIC);
					}
				}
			}
		}
	}

	// Token: 0x06000800 RID: 2048 RVA: 0x0001B9A8 File Offset: 0x00019BA8
	private void OnEnemyHit(object sender, CharacterHitEventArgs args)
	{
		base.EnemyController.StatusEffectController.SetAllStatusEffectsHidden(false);
		base.EnemyController.DisableOffscreenWarnings = false;
	}

	// Token: 0x06000801 RID: 2049 RVA: 0x0001B9C8 File Offset: 0x00019BC8
	private void OnEnemyControllerDisable()
	{
		if (!base.IsInitialized)
		{
			return;
		}
		this.m_isChasing = false;
		int num = Mathf.Clamp((RNGManager.GetSeed(RngID.SpecialProps_RoomSeed) + base.EnemyController.EnemyIndex) % this.m_paintingSprites.Length, 0, this.m_paintingSprites.Length - 1);
		this.m_paintingRenderer.sprite = this.m_paintingSprites[num];
		this.m_enemyFaceRenderer.sprite = this.m_enemyFaceSprites[num];
		Color color = this.m_enemyFaceRenderer.color;
		color.a = 0f;
		this.m_enemyFaceRenderer.color = color;
		if (!this.m_enemyFaceTween.IsNativeNull())
		{
			this.m_enemyFaceTween.StopTween(true);
		}
		base.EnemyController.StatusEffectController.SetAllStatusEffectsHidden(true);
		base.EnemyController.DisableOffscreenWarnings = true;
	}

	// Token: 0x06000802 RID: 2050 RVA: 0x0001BA98 File Offset: 0x00019C98
	private void Awaken(object sender, EventArgs args)
	{
		if (this.m_enemyFaceTween != null)
		{
			this.m_enemyFaceTween.StopTweenWithConditionChecks(false, this.m_enemyFaceRenderer, null);
		}
		this.m_enemyFaceTween = TweenManager.TweenTo(this.m_enemyFaceRenderer, 0.5f, new EaseDelegate(Ease.None), new object[]
		{
			"color.a",
			1
		});
		this.m_isChasing = true;
		base.EnemyController.StatusEffectController.SetAllStatusEffectsHidden(false);
		base.EnemyController.DisableOffscreenWarnings = false;
		Vector3 v = base.EnemyController.Midpoint - PlayerManager.GetPlayerController().Midpoint;
		base.EnemyController.Heading = v;
		if (PlayerManager.GetPlayerController().transform.localPosition.x < base.EnemyController.transform.localPosition.x)
		{
			this.m_rotateClockwise = false;
		}
		else
		{
			this.m_rotateClockwise = true;
		}
		base.Animator.SetTrigger("Reveal");
		base.Animator.SetTrigger("Spin");
		if (!this.m_flyLoopEventInstance.isValid())
		{
			this.m_flyLoopEventInstance = AudioUtility.GetEventInstance("event:/SFX/Enemies/sfx_enemy_painting_fly_start_loop", base.transform);
		}
		AudioManager.PlayAttached(this, this.m_flyLoopEventInstance, base.gameObject);
	}

	// Token: 0x06000803 RID: 2051 RVA: 0x0001BBDF File Offset: 0x00019DDF
	public override void ResetScript()
	{
		base.ResetScript();
		base.EnemyController.LockFlip = true;
		base.EnemyController.Pivot.transform.localEulerAngles = Vector3.zero;
	}

	// Token: 0x06000804 RID: 2052 RVA: 0x0001BC10 File Offset: 0x00019E10
	protected void Update()
	{
		if (!base.IsInitialized)
		{
			return;
		}
		if (!this.m_isChasing)
		{
			return;
		}
		if (base.EnemyController.ConditionState != CharacterStates.CharacterConditions.Normal)
		{
			return;
		}
		if (!base.LogicController.IsAggroed)
		{
			base.EnemyController.LogicController.TriggerAggro(null, null);
		}
		Vector3 localEulerAngles = base.EnemyController.Pivot.transform.localEulerAngles;
		float num = localEulerAngles.z;
		if (this.m_rotateClockwise)
		{
			num -= Time.deltaTime * 720f;
			if (num < -360f)
			{
				num += 360f;
			}
		}
		else
		{
			num += Time.deltaTime * 720f;
			if (num > 360f)
			{
				num -= 360f;
			}
		}
		localEulerAngles.z = num;
		base.EnemyController.Pivot.transform.localEulerAngles = localEulerAngles;
		if (this.ProjectileFireInterval > 0f && Time.time >= this.m_projectileCDDuration)
		{
			this.Shoot();
		}
	}

	// Token: 0x04000B58 RID: 2904
	private const float ROTATION_SPEED = 720f;

	// Token: 0x04000B59 RID: 2905
	private const string GENERIC_PROJECTILE_NAME = "PaintingEnemyProjectile";

	// Token: 0x04000B5A RID: 2906
	private const string FLY_LOOP_AUDIO_PATH = "event:/SFX/Enemies/sfx_enemy_painting_fly_start_loop";

	// Token: 0x04000B5B RID: 2907
	[SerializeField]
	private Sprite[] m_paintingSprites;

	// Token: 0x04000B5C RID: 2908
	[SerializeField]
	private Sprite[] m_enemyFaceSprites;

	// Token: 0x04000B5D RID: 2909
	private SpriteRenderer m_paintingRenderer;

	// Token: 0x04000B5E RID: 2910
	private SpriteRenderer m_enemyFaceRenderer;

	// Token: 0x04000B5F RID: 2911
	private bool m_isChasing;

	// Token: 0x04000B60 RID: 2912
	private bool m_rotateClockwise;

	// Token: 0x04000B61 RID: 2913
	private float m_projectileCDDuration;

	// Token: 0x04000B62 RID: 2914
	private Tween m_enemyFaceTween;

	// Token: 0x04000B63 RID: 2915
	private EventInstance m_flyLoopEventInstance;

	// Token: 0x04000B64 RID: 2916
	private Action<object, CharacterHitEventArgs> m_onPlayerHit;

	// Token: 0x04000B65 RID: 2917
	private Action<object, CharacterHitEventArgs> m_onEnemyHit;

	// Token: 0x04000B66 RID: 2918
	private Action m_onEnemyControllerDisable;
}
