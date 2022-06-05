using System;
using System.Collections;
using FMOD.Studio;
using MoreMountains.CorgiEngine;
using RLAudio;
using UnityEngine;

// Token: 0x020001A2 RID: 418
public class PaintingEnemy_Basic_AIScript : BaseAIScript
{
	// Token: 0x06000B85 RID: 2949 RVA: 0x00007170 File Offset: 0x00005370
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			"PaintingEnemyProjectile"
		};
	}

	// Token: 0x17000579 RID: 1401
	// (get) Token: 0x06000B86 RID: 2950 RVA: 0x00003DEF File Offset: 0x00001FEF
	protected override Vector2 IdleDuration
	{
		get
		{
			return new Vector2(0.25f, 0.7f);
		}
	}

	// Token: 0x1700057A RID: 1402
	// (get) Token: 0x06000B87 RID: 2951 RVA: 0x00004F67 File Offset: 0x00003167
	protected override Vector2 WalkTowardsDuration
	{
		get
		{
			return new Vector2(1.25f, 2f);
		}
	}

	// Token: 0x1700057B RID: 1403
	// (get) Token: 0x06000B88 RID: 2952 RVA: 0x00004F67 File Offset: 0x00003167
	protected override Vector2 WalkAwayDuration
	{
		get
		{
			return new Vector2(1.25f, 2f);
		}
	}

	// Token: 0x1700057C RID: 1404
	// (get) Token: 0x06000B89 RID: 2953 RVA: 0x00005FA3 File Offset: 0x000041A3
	protected override Vector2 RandomFollowOffsetX
	{
		get
		{
			return Vector2.zero;
		}
	}

	// Token: 0x1700057D RID: 1405
	// (get) Token: 0x06000B8A RID: 2954 RVA: 0x00005FA3 File Offset: 0x000041A3
	protected override Vector2 RandomFollowOffsetY
	{
		get
		{
			return Vector2.zero;
		}
	}

	// Token: 0x1700057E RID: 1406
	// (get) Token: 0x06000B8B RID: 2955 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected virtual float ProjectileFireInterval
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x06000B8C RID: 2956 RVA: 0x00007186 File Offset: 0x00005386
	private void Awake()
	{
		this.m_onPlayerHit = new Action<object, CharacterHitEventArgs>(this.OnPlayerHit);
		this.m_onEnemyHit = new Action<object, CharacterHitEventArgs>(this.OnEnemyHit);
		this.m_onEnemyControllerDisable = new Action(this.OnEnemyControllerDisable);
	}

	// Token: 0x06000B8D RID: 2957 RVA: 0x0006AA00 File Offset: 0x00068C00
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

	// Token: 0x06000B8E RID: 2958 RVA: 0x000071BE File Offset: 0x000053BE
	public override void OnEnemyActivated()
	{
		this.m_projectileCDDuration = Time.time + this.ProjectileFireInterval;
		base.OnEnemyActivated();
	}

	// Token: 0x06000B8F RID: 2959 RVA: 0x000071D8 File Offset: 0x000053D8
	public override IEnumerator Idle()
	{
		yield return base.WalkTowards();
		yield break;
	}

	// Token: 0x06000B90 RID: 2960 RVA: 0x0006AAE0 File Offset: 0x00068CE0
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

	// Token: 0x06000B91 RID: 2961 RVA: 0x000071E7 File Offset: 0x000053E7
	private void OnEnable()
	{
		if (PlayerManager.IsInstantiated && !PlayerManager.IsDisposed)
		{
			PlayerManager.GetPlayerController().CharacterHitResponse.OnCharacterHitRelay.AddListener(this.m_onPlayerHit, false);
		}
	}

	// Token: 0x06000B92 RID: 2962 RVA: 0x0006AB90 File Offset: 0x00068D90
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

	// Token: 0x06000B93 RID: 2963 RVA: 0x0006AC00 File Offset: 0x00068E00
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

	// Token: 0x06000B94 RID: 2964 RVA: 0x0006AC6C File Offset: 0x00068E6C
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

	// Token: 0x06000B95 RID: 2965 RVA: 0x00006FBE File Offset: 0x000051BE
	private void OnEnemyHit(object sender, CharacterHitEventArgs args)
	{
		base.EnemyController.StatusEffectController.SetAllStatusEffectsHidden(false);
		base.EnemyController.DisableOffscreenWarnings = false;
	}

	// Token: 0x06000B96 RID: 2966 RVA: 0x0006AD30 File Offset: 0x00068F30
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

	// Token: 0x06000B97 RID: 2967 RVA: 0x0006AE00 File Offset: 0x00069000
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

	// Token: 0x06000B98 RID: 2968 RVA: 0x00007213 File Offset: 0x00005413
	public override void ResetScript()
	{
		base.ResetScript();
		base.EnemyController.LockFlip = true;
		base.EnemyController.Pivot.transform.localEulerAngles = Vector3.zero;
	}

	// Token: 0x06000B99 RID: 2969 RVA: 0x0006AF48 File Offset: 0x00069148
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

	// Token: 0x04000E1F RID: 3615
	private const float ROTATION_SPEED = 720f;

	// Token: 0x04000E20 RID: 3616
	private const string GENERIC_PROJECTILE_NAME = "PaintingEnemyProjectile";

	// Token: 0x04000E21 RID: 3617
	private const string FLY_LOOP_AUDIO_PATH = "event:/SFX/Enemies/sfx_enemy_painting_fly_start_loop";

	// Token: 0x04000E22 RID: 3618
	[SerializeField]
	private Sprite[] m_paintingSprites;

	// Token: 0x04000E23 RID: 3619
	[SerializeField]
	private Sprite[] m_enemyFaceSprites;

	// Token: 0x04000E24 RID: 3620
	private SpriteRenderer m_paintingRenderer;

	// Token: 0x04000E25 RID: 3621
	private SpriteRenderer m_enemyFaceRenderer;

	// Token: 0x04000E26 RID: 3622
	private bool m_isChasing;

	// Token: 0x04000E27 RID: 3623
	private bool m_rotateClockwise;

	// Token: 0x04000E28 RID: 3624
	private float m_projectileCDDuration;

	// Token: 0x04000E29 RID: 3625
	private Tween m_enemyFaceTween;

	// Token: 0x04000E2A RID: 3626
	private EventInstance m_flyLoopEventInstance;

	// Token: 0x04000E2B RID: 3627
	private Action<object, CharacterHitEventArgs> m_onPlayerHit;

	// Token: 0x04000E2C RID: 3628
	private Action<object, CharacterHitEventArgs> m_onEnemyHit;

	// Token: 0x04000E2D RID: 3629
	private Action m_onEnemyControllerDisable;
}
