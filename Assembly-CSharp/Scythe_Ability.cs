using System;
using System.Collections;
using MoreMountains.CorgiEngine;
using UnityEngine;

// Token: 0x020001A6 RID: 422
public class Scythe_Ability : BaseAbility_RL, IAttack, IAbility
{
	// Token: 0x06001090 RID: 4240 RVA: 0x0002FFBD File Offset: 0x0002E1BD
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			this.m_projectileName,
			this.m_secondProjectileName
		};
	}

	// Token: 0x1700093D RID: 2365
	// (get) Token: 0x06001091 RID: 4241 RVA: 0x0002FFDD File Offset: 0x0002E1DD
	public override float AirMovementMod
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700093E RID: 2366
	// (get) Token: 0x06001092 RID: 4242 RVA: 0x0002FFE4 File Offset: 0x0002E1E4
	public override float MovementMod
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700093F RID: 2367
	// (get) Token: 0x06001093 RID: 4243 RVA: 0x0002FFEB File Offset: 0x0002E1EB
	public override string ProjectileName
	{
		get
		{
			if (!this.m_performedFirstAttack)
			{
				return base.ProjectileName;
			}
			return this.m_secondProjectileName;
		}
	}

	// Token: 0x17000940 RID: 2368
	// (get) Token: 0x06001094 RID: 4244 RVA: 0x00030002 File Offset: 0x0002E202
	protected override float TellIntroAnimSpeed
	{
		get
		{
			if (this.m_performedFirstAttack)
			{
				return 0.8f;
			}
			return 0.8f;
		}
	}

	// Token: 0x17000941 RID: 2369
	// (get) Token: 0x06001095 RID: 4245 RVA: 0x00030017 File Offset: 0x0002E217
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			if (this.m_performedFirstAttack)
			{
				return 0f;
			}
			return 0f;
		}
	}

	// Token: 0x17000942 RID: 2370
	// (get) Token: 0x06001096 RID: 4246 RVA: 0x0003002C File Offset: 0x0002E22C
	protected override float TellAnimSpeed
	{
		get
		{
			if (this.m_performedFirstAttack)
			{
				return 1f;
			}
			return 1f;
		}
	}

	// Token: 0x17000943 RID: 2371
	// (get) Token: 0x06001097 RID: 4247 RVA: 0x00030041 File Offset: 0x0002E241
	protected override float TellAnimExitDelay
	{
		get
		{
			if (this.m_performedFirstAttack)
			{
				return 0f;
			}
			return 0f;
		}
	}

	// Token: 0x17000944 RID: 2372
	// (get) Token: 0x06001098 RID: 4248 RVA: 0x00030056 File Offset: 0x0002E256
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			if (this.m_performedFirstAttack)
			{
				return 1f;
			}
			return 1f;
		}
	}

	// Token: 0x17000945 RID: 2373
	// (get) Token: 0x06001099 RID: 4249 RVA: 0x0003006B File Offset: 0x0002E26B
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			if (this.m_performedFirstAttack)
			{
				return 0f;
			}
			return 0f;
		}
	}

	// Token: 0x17000946 RID: 2374
	// (get) Token: 0x0600109A RID: 4250 RVA: 0x00030080 File Offset: 0x0002E280
	protected override float AttackAnimSpeed
	{
		get
		{
			if (this.m_performedFirstAttack)
			{
				return 1f;
			}
			return 1f;
		}
	}

	// Token: 0x17000947 RID: 2375
	// (get) Token: 0x0600109B RID: 4251 RVA: 0x00030095 File Offset: 0x0002E295
	protected override float AttackAnimExitDelay
	{
		get
		{
			if (this.m_performedFirstAttack)
			{
				return 0f;
			}
			return 0.075f;
		}
	}

	// Token: 0x17000948 RID: 2376
	// (get) Token: 0x0600109C RID: 4252 RVA: 0x000300AA File Offset: 0x0002E2AA
	protected override float ExitAnimSpeed
	{
		get
		{
			if (this.m_performedFirstAttack)
			{
				return 1f;
			}
			return 1f;
		}
	}

	// Token: 0x17000949 RID: 2377
	// (get) Token: 0x0600109D RID: 4253 RVA: 0x000300BF File Offset: 0x0002E2BF
	protected override float ExitAnimExitDelay
	{
		get
		{
			if (this.m_performedFirstAttack)
			{
				return 0f;
			}
			return 0f;
		}
	}

	// Token: 0x0600109E RID: 4254 RVA: 0x000300D4 File Offset: 0x0002E2D4
	protected override void Awake()
	{
		base.Awake();
		this.m_stopScythePartSystem = new Action<MonoBehaviour, EventArgs>(this.StopScythePartSystem);
	}

	// Token: 0x0600109F RID: 4255 RVA: 0x000300F0 File Offset: 0x0002E2F0
	public override void Initialize(CastAbility_RL abilityController, CastAbilityType castAbilityType)
	{
		base.Initialize(abilityController, castAbilityType);
		if (!this.m_partSysAdded)
		{
			this.m_weaponTransform = abilityController.PlayerController.Visuals.transform.FindDeep("weapon_l");
			if (this.m_weaponTransform)
			{
				this.m_scythePartSys = UnityEngine.Object.Instantiate<ParticleSystem>(this.m_scytheParticlePartSysPrefab);
				this.m_scythePartSys.transform.position = this.m_weaponTransform.transform.position;
				this.m_scythePartSys.transform.SetParent(this.m_weaponTransform, false);
				this.m_scythePartSys.Clear(true);
				this.m_scythePartSys.transform.localScale = new Vector3(1f, 1f, 1f);
				this.m_scythePartSys.transform.localPosition = Vector3.zero;
				this.m_partSysAdded = true;
				Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerDeath, this.m_stopScythePartSystem);
				Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerEnterParade, this.m_stopScythePartSystem);
			}
		}
	}

	// Token: 0x060010A0 RID: 4256 RVA: 0x000301EE File Offset: 0x0002E3EE
	public override void OnPreDestroy()
	{
		if (this.m_partSysAdded)
		{
			this.m_scythePartSys.transform.SetParent(null);
		}
		base.OnPreDestroy();
	}

	// Token: 0x060010A1 RID: 4257 RVA: 0x00030210 File Offset: 0x0002E410
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (this.m_partSysAdded)
		{
			UnityEngine.Object.Destroy(this.m_scythePartSys.gameObject);
			this.m_partSysAdded = false;
			Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerDeath, this.m_stopScythePartSystem);
			Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerEnterParade, this.m_stopScythePartSystem);
		}
		this.m_weaponTransform = null;
	}

	// Token: 0x060010A2 RID: 4258 RVA: 0x00030263 File Offset: 0x0002E463
	private void StopScythePartSystem(object sender, EventArgs args)
	{
		if (this.m_partSysAdded)
		{
			this.m_scythePartSys.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
		}
	}

	// Token: 0x060010A3 RID: 4259 RVA: 0x0003027C File Offset: 0x0002E47C
	public override void PreCastAbility()
	{
		base.PreCastAbility();
		this.m_storedFallMultiplier = this.m_abilityController.PlayerController.FallMultiplierOverride;
		this.m_abilityController.PlayerController.CharacterJump.ResetBrakeForce();
		this.m_abilityController.PlayerController.SetVelocity(0f, 0f, false);
		this.m_abilityController.PlayerController.FallMultiplierOverride = 0f;
		this.m_abilityController.PlayerController.ControllerCorgi.StickWhenWalkingDownSlopes = false;
		this.m_performedFirstAttack = false;
		this.m_continueDashing = false;
	}

	// Token: 0x060010A4 RID: 4260 RVA: 0x0003030E File Offset: 0x0002E50E
	protected override IEnumerator ChangeAnim(float duration)
	{
		if (base.CurrentAbilityAnimState == AbilityAnimState.Attack && !this.m_performedFirstAttack)
		{
			base.StartCoroutine(this.PushForward());
			this.m_performedFirstAttack = true;
			yield return base.ChangeAnim(duration);
		}
		else
		{
			yield return base.ChangeAnim(duration);
		}
		yield break;
	}

	// Token: 0x060010A5 RID: 4261 RVA: 0x00030324 File Offset: 0x0002E524
	protected IEnumerator PushForward()
	{
		float speed = 22f;
		this.m_continueDashing = true;
		if (this.m_abilityController.PlayerController.IsFacingRight)
		{
			this.m_abilityController.PlayerController.SetVelocityX(speed, false);
		}
		else
		{
			this.m_abilityController.PlayerController.SetVelocityX(-speed, false);
		}
		if (this.m_abilityController.PlayerController.IsGrounded)
		{
			this.m_abilityController.PlayerController.MovementState = CharacterStates.MovementStates.Idle;
		}
		if (!this.m_abilityController.PlayerController.ControllerCorgi.DisableOneWayCollision)
		{
			this.m_abilityController.PlayerController.ControllerCorgi.DisableOneWayCollision = true;
			this.m_oneWayCollisionDisabled = true;
		}
		while (this.m_continueDashing)
		{
			float num = speed;
			this.m_abilityController.PlayerController.DisableFriction = true;
			this.m_abilityController.PlayerController.DisableDoorBlock = true;
			if (this.m_abilityController.PlayerController.IsFacingRight)
			{
				this.m_abilityController.PlayerController.SetVelocity(num, 0f, false);
			}
			else
			{
				this.m_abilityController.PlayerController.SetVelocity(-num, 0f, false);
			}
			if (this.m_abilityController.PlayerController.IsGrounded)
			{
				this.m_abilityController.PlayerController.ConditionState = CharacterStates.CharacterConditions.DisableHorizontalMovement;
			}
			else
			{
				this.m_abilityController.PlayerController.ConditionState = CharacterStates.CharacterConditions.Normal;
			}
			yield return null;
		}
		this.m_abilityController.PlayerController.DisableDoorBlock = false;
		this.m_abilityController.PlayerController.DisableFriction = false;
		if (this.m_oneWayCollisionDisabled)
		{
			this.m_abilityController.PlayerController.ControllerCorgi.DisableOneWayCollision = false;
			this.m_oneWayCollisionDisabled = false;
		}
		yield break;
	}

	// Token: 0x060010A6 RID: 4262 RVA: 0x00030334 File Offset: 0x0002E534
	public override void StopAbility(bool abilityInterrupted)
	{
		this.m_abilityController.PlayerController.DisableFriction = false;
		this.m_abilityController.PlayerController.DisableDoorBlock = false;
		if (this.m_oneWayCollisionDisabled)
		{
			this.m_abilityController.PlayerController.ControllerCorgi.DisableOneWayCollision = false;
			this.m_oneWayCollisionDisabled = false;
		}
		this.m_abilityController.PlayerController.ControllerCorgi.StickWhenWalkingDownSlopes = true;
		if (this.m_abilityController.PlayerController.ConditionState != CharacterStates.CharacterConditions.Stunned)
		{
			this.m_abilityController.PlayerController.ConditionState = CharacterStates.CharacterConditions.Normal;
		}
		if (this.m_firedProjectile)
		{
			this.m_firedProjectile.FlagForDestruction(null);
			this.m_firedProjectile = null;
		}
		this.m_abilityController.PlayerController.FallMultiplierOverride = this.m_storedFallMultiplier;
		this.m_continueDashing = false;
		this.StartCooldownTimer();
		base.StopAbility(abilityInterrupted);
	}

	// Token: 0x060010A7 RID: 4263 RVA: 0x0003040C File Offset: 0x0002E60C
	protected override void OnEnterExitLogic()
	{
		this.m_continueDashing = false;
		base.OnEnterExitLogic();
	}

	// Token: 0x060010A8 RID: 4264 RVA: 0x0003041C File Offset: 0x0002E61C
	protected override void Update()
	{
		base.Update();
		if (this.m_partSysAdded && this.m_weaponTransform && this.m_weaponTransform.hasChanged)
		{
			if (this.m_weaponTransform.localScale.x < 0.1f)
			{
				if (this.m_scythePartSys.isEmitting)
				{
					this.m_scythePartSys.Stop(false, ParticleSystemStopBehavior.StopEmitting);
					return;
				}
			}
			else if (!this.m_scythePartSys.isPlaying)
			{
				this.m_scythePartSys.Play();
			}
		}
	}

	// Token: 0x040011D3 RID: 4563
	[SerializeField]
	private string m_secondProjectileName;

	// Token: 0x040011D4 RID: 4564
	[Space(10f)]
	[SerializeField]
	private ParticleSystem m_scytheParticlePartSysPrefab;

	// Token: 0x040011D5 RID: 4565
	private float m_storedFallMultiplier;

	// Token: 0x040011D6 RID: 4566
	private bool m_performedFirstAttack;

	// Token: 0x040011D7 RID: 4567
	private bool m_continueDashing;

	// Token: 0x040011D8 RID: 4568
	private bool m_partSysAdded;

	// Token: 0x040011D9 RID: 4569
	private Transform m_weaponTransform;

	// Token: 0x040011DA RID: 4570
	private ParticleSystem m_scythePartSys;

	// Token: 0x040011DB RID: 4571
	private Action<MonoBehaviour, EventArgs> m_stopScythePartSystem;

	// Token: 0x040011DC RID: 4572
	private bool m_oneWayCollisionDisabled;
}
