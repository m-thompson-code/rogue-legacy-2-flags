using System;
using System.Collections;
using MoreMountains.CorgiEngine;
using UnityEngine;

// Token: 0x0200030C RID: 780
public class Scythe_Ability : BaseAbility_RL, IAttack, IAbility
{
	// Token: 0x060018AD RID: 6317 RVA: 0x0000C7E7 File Offset: 0x0000A9E7
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			this.m_projectileName,
			this.m_secondProjectileName
		};
	}

	// Token: 0x17000BEF RID: 3055
	// (get) Token: 0x060018AE RID: 6318 RVA: 0x00003CCB File Offset: 0x00001ECB
	public override float AirMovementMod
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000BF0 RID: 3056
	// (get) Token: 0x060018AF RID: 6319 RVA: 0x00003CCB File Offset: 0x00001ECB
	public override float MovementMod
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000BF1 RID: 3057
	// (get) Token: 0x060018B0 RID: 6320 RVA: 0x0000C807 File Offset: 0x0000AA07
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

	// Token: 0x17000BF2 RID: 3058
	// (get) Token: 0x060018B1 RID: 6321 RVA: 0x0000C81E File Offset: 0x0000AA1E
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

	// Token: 0x17000BF3 RID: 3059
	// (get) Token: 0x060018B2 RID: 6322 RVA: 0x0000C833 File Offset: 0x0000AA33
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

	// Token: 0x17000BF4 RID: 3060
	// (get) Token: 0x060018B3 RID: 6323 RVA: 0x0000C848 File Offset: 0x0000AA48
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

	// Token: 0x17000BF5 RID: 3061
	// (get) Token: 0x060018B4 RID: 6324 RVA: 0x0000C833 File Offset: 0x0000AA33
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

	// Token: 0x17000BF6 RID: 3062
	// (get) Token: 0x060018B5 RID: 6325 RVA: 0x0000C848 File Offset: 0x0000AA48
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

	// Token: 0x17000BF7 RID: 3063
	// (get) Token: 0x060018B6 RID: 6326 RVA: 0x0000C833 File Offset: 0x0000AA33
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

	// Token: 0x17000BF8 RID: 3064
	// (get) Token: 0x060018B7 RID: 6327 RVA: 0x0000C848 File Offset: 0x0000AA48
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

	// Token: 0x17000BF9 RID: 3065
	// (get) Token: 0x060018B8 RID: 6328 RVA: 0x0000C85D File Offset: 0x0000AA5D
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

	// Token: 0x17000BFA RID: 3066
	// (get) Token: 0x060018B9 RID: 6329 RVA: 0x0000C848 File Offset: 0x0000AA48
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

	// Token: 0x17000BFB RID: 3067
	// (get) Token: 0x060018BA RID: 6330 RVA: 0x0000C833 File Offset: 0x0000AA33
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

	// Token: 0x060018BB RID: 6331 RVA: 0x0000C872 File Offset: 0x0000AA72
	protected override void Awake()
	{
		base.Awake();
		this.m_stopScythePartSystem = new Action<MonoBehaviour, EventArgs>(this.StopScythePartSystem);
	}

	// Token: 0x060018BC RID: 6332 RVA: 0x0008E51C File Offset: 0x0008C71C
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

	// Token: 0x060018BD RID: 6333 RVA: 0x0000C88C File Offset: 0x0000AA8C
	public override void OnPreDestroy()
	{
		if (this.m_partSysAdded)
		{
			this.m_scythePartSys.transform.SetParent(null);
		}
		base.OnPreDestroy();
	}

	// Token: 0x060018BE RID: 6334 RVA: 0x0008E61C File Offset: 0x0008C81C
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

	// Token: 0x060018BF RID: 6335 RVA: 0x0000C8AD File Offset: 0x0000AAAD
	private void StopScythePartSystem(object sender, EventArgs args)
	{
		if (this.m_partSysAdded)
		{
			this.m_scythePartSys.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
		}
	}

	// Token: 0x060018C0 RID: 6336 RVA: 0x0008E670 File Offset: 0x0008C870
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

	// Token: 0x060018C1 RID: 6337 RVA: 0x0000C8C4 File Offset: 0x0000AAC4
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

	// Token: 0x060018C2 RID: 6338 RVA: 0x0000C8DA File Offset: 0x0000AADA
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

	// Token: 0x060018C3 RID: 6339 RVA: 0x0008E704 File Offset: 0x0008C904
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

	// Token: 0x060018C4 RID: 6340 RVA: 0x0000C8E9 File Offset: 0x0000AAE9
	protected override void OnEnterExitLogic()
	{
		this.m_continueDashing = false;
		base.OnEnterExitLogic();
	}

	// Token: 0x060018C5 RID: 6341 RVA: 0x0008E7DC File Offset: 0x0008C9DC
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

	// Token: 0x040017B9 RID: 6073
	[SerializeField]
	private string m_secondProjectileName;

	// Token: 0x040017BA RID: 6074
	[Space(10f)]
	[SerializeField]
	private ParticleSystem m_scytheParticlePartSysPrefab;

	// Token: 0x040017BB RID: 6075
	private float m_storedFallMultiplier;

	// Token: 0x040017BC RID: 6076
	private bool m_performedFirstAttack;

	// Token: 0x040017BD RID: 6077
	private bool m_continueDashing;

	// Token: 0x040017BE RID: 6078
	private bool m_partSysAdded;

	// Token: 0x040017BF RID: 6079
	private Transform m_weaponTransform;

	// Token: 0x040017C0 RID: 6080
	private ParticleSystem m_scythePartSys;

	// Token: 0x040017C1 RID: 6081
	private Action<MonoBehaviour, EventArgs> m_stopScythePartSystem;

	// Token: 0x040017C2 RID: 6082
	private bool m_oneWayCollisionDisabled;
}
