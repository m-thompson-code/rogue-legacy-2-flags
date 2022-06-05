using System;
using System.Collections;
using FMOD.Studio;
using Rewired;
using RLAudio;
using RL_Windows;
using UnityEngine;

// Token: 0x02000181 RID: 385
public class CrowsNest_Ability : BaseAbility_RL, ITalent, IAbility
{
	// Token: 0x06000D9D RID: 3485 RVA: 0x000299C4 File Offset: 0x00027BC4
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			this.m_projectileName,
			this.m_crowsNestDestroyProjectile,
			this.m_fireRepeatProjectileName
		};
	}

	// Token: 0x17000753 RID: 1875
	// (get) Token: 0x06000D9E RID: 3486 RVA: 0x000299ED File Offset: 0x00027BED
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 2.25f;
		}
	}

	// Token: 0x17000754 RID: 1876
	// (get) Token: 0x06000D9F RID: 3487 RVA: 0x000299F4 File Offset: 0x00027BF4
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000755 RID: 1877
	// (get) Token: 0x06000DA0 RID: 3488 RVA: 0x000299FB File Offset: 0x00027BFB
	protected override float TellAnimSpeed
	{
		get
		{
			return 10f;
		}
	}

	// Token: 0x17000756 RID: 1878
	// (get) Token: 0x06000DA1 RID: 3489 RVA: 0x00029A02 File Offset: 0x00027C02
	protected override float TellAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000757 RID: 1879
	// (get) Token: 0x06000DA2 RID: 3490 RVA: 0x00029A09 File Offset: 0x00027C09
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			return 10f;
		}
	}

	// Token: 0x17000758 RID: 1880
	// (get) Token: 0x06000DA3 RID: 3491 RVA: 0x00029A10 File Offset: 0x00027C10
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000759 RID: 1881
	// (get) Token: 0x06000DA4 RID: 3492 RVA: 0x00029A17 File Offset: 0x00027C17
	protected override float AttackAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700075A RID: 1882
	// (get) Token: 0x06000DA5 RID: 3493 RVA: 0x00029A1E File Offset: 0x00027C1E
	protected override float AttackAnimExitDelay
	{
		get
		{
			return 0.15f;
		}
	}

	// Token: 0x1700075B RID: 1883
	// (get) Token: 0x06000DA6 RID: 3494 RVA: 0x00029A25 File Offset: 0x00027C25
	protected override float ExitAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700075C RID: 1884
	// (get) Token: 0x06000DA7 RID: 3495 RVA: 0x00029A2C File Offset: 0x00027C2C
	protected override float ExitAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700075D RID: 1885
	// (get) Token: 0x06000DA8 RID: 3496 RVA: 0x00029A33 File Offset: 0x00027C33
	public bool CrowsNestActive
	{
		get
		{
			return this.m_crowsNestActive;
		}
	}

	// Token: 0x06000DA9 RID: 3497 RVA: 0x00029A3C File Offset: 0x00027C3C
	protected override void Awake()
	{
		base.Awake();
		this.m_onPlayerExitRoom = new Action<MonoBehaviour, EventArgs>(this.OnPlayerExitRoom);
		this.m_resumeCooldownIfPlayerExitsRoom = new Action<MonoBehaviour, EventArgs>(this.ResumeCooldownIfPlayerExitsRoom);
		this.m_onPlayerDash = new Action<MonoBehaviour, EventArgs>(this.OnPlayerDash);
		this.m_resumeCooldown = new Action<Projectile_RL, GameObject>(this.ResumeCooldown);
		this.m_decreasesCDOverTime = base.DecreaseCooldownOverTime;
		this.m_decreasesCDOnHit = base.DecreaseCooldownWhenHit;
		this.m_onTimeout = new Action<GameObject>(this.OnTimeout);
		this.m_onPlayerDeath = new Action<MonoBehaviour, EventArgs>(this.OnPlayerDeath);
		this.m_onPlayerFakeDeath = new Action<MonoBehaviour, EventArgs>(this.OnPlayerFakeDeath);
		this.m_stingerInstance = AudioUtility.GetEventInstance("event:/Stingers/sting_pirateShip_loop", base.transform);
		this.m_spawnLoopInstance = AudioUtility.GetEventInstance("event:/SFX/Weapons/sfx_weapon_pirateShip_spawn_loop", base.transform);
	}

	// Token: 0x06000DAA RID: 3498 RVA: 0x00029B11 File Offset: 0x00027D11
	private void OnEnable()
	{
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerExitRoom, this.m_onPlayerExitRoom);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerDeath, this.m_onPlayerDeath);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerFakedDeath, this.m_onPlayerFakeDeath);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerEnterRoom, this.m_resumeCooldownIfPlayerExitsRoom);
	}

	// Token: 0x06000DAB RID: 3499 RVA: 0x00029B46 File Offset: 0x00027D46
	private void OnDisable()
	{
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerExitRoom, this.m_onPlayerExitRoom);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerDeath, this.m_onPlayerDeath);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerFakedDeath, this.m_onPlayerFakeDeath);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerEnterRoom, this.m_resumeCooldownIfPlayerExitsRoom);
	}

	// Token: 0x06000DAC RID: 3500 RVA: 0x00029B7B File Offset: 0x00027D7B
	public override void PreCastAbility()
	{
		this.DestroyCrowsNest(true);
		base.PreCastAbility();
	}

	// Token: 0x06000DAD RID: 3501 RVA: 0x00029B8C File Offset: 0x00027D8C
	protected override void FireProjectile()
	{
		base.FireProjectile();
		this.m_crowsNestProjectile = this.m_firedProjectile;
		this.m_crowsNestCollider = (this.m_crowsNestProjectile.HitboxController.GetCollider(HitboxType.Platform) as BoxCollider2D);
		this.m_crowsNestCollider.gameObject.layer = 11;
		this.m_abilityController.PlayerController.CharacterJump.ResetBrakeForce();
		this.m_abilityController.PlayerController.SetVelocity(0f, 10f, false);
		this.m_crowsNestProjectile.HitboxController.DisableAllCollisions = false;
		this.m_crowsNestProjectile.OnTimeoutEffectTriggerRelay.AddOnce(this.m_onTimeout, false);
		this.m_fireRepeatTimer = Time.time + 0.15f;
		bool flag = true;
		if (ChallengeManager.IsInChallenge && ChallengeManager.ActiveChallenge.ChallengeData.ScoringType == ChallengeScoringType.Battle)
		{
			flag = false;
		}
		else
		{
			foreach (EnemySpawnController enemySpawnController in this.m_abilityController.PlayerController.CurrentlyInRoom.SpawnControllerManager.EnemySpawnControllers)
			{
				if (enemySpawnController.EnemyInstance && enemySpawnController.EnemyInstance.IsBoss && enemySpawnController.EnemyInstance.EnemyType != EnemyType.Eggplant)
				{
					flag = false;
					break;
				}
			}
		}
		if (this.m_stingerInstance.isValid() && flag && MusicManager.CurrentSong != SongID.JUKEBOX_Misc_PirateSong_1_ASITP_PirateLoop_1)
		{
			AudioManager.Play(this, this.m_stingerInstance);
			this.m_stingerInstance.setParameterByName("pirateShip_on", 1f, false);
		}
		if (this.m_spawnLoopInstance.isValid())
		{
			AudioManager.Play(this, this.m_spawnLoopInstance);
			this.m_spawnLoopInstance.setParameterByName("pirateShip_move", 0f, false);
		}
		for (int j = 0; j < this.m_crowsNestProjectile.HitboxController.gameObject.transform.childCount; j++)
		{
			GameObject gameObject = this.m_crowsNestProjectile.HitboxController.gameObject.transform.GetChild(j).gameObject;
			if (gameObject.CompareTag("Barricade"))
			{
				this.m_crowsNextSideColliderGO = gameObject.gameObject;
				this.m_crowsNextSideColliderGO.SetActive(false);
				break;
			}
		}
		if (this.m_decreasesCDOverTime)
		{
			base.DecreaseCooldownOverTime = false;
		}
		if (this.m_decreasesCDOnHit)
		{
			base.DecreaseCooldownWhenHit = false;
		}
		base.DisplayPausedAbilityCooldown = true;
		this.m_crowsNestProjectile.OnDeathRelay.AddOnce(this.m_resumeCooldown, false);
		this.m_crowsNextMoveAmount = 0f;
		this.m_crowsNestActive = true;
		this.m_wasMoving = false;
		this.m_movedThisFrame = false;
	}

	// Token: 0x06000DAE RID: 3502 RVA: 0x00029E00 File Offset: 0x00028000
	private void FireProjectileOnRepeat()
	{
		float num = this.m_crowsNestProjectile.Lifespan * 0.19999999f;
		if (this.m_crowsNestProjectile.LifespanTimer <= num)
		{
			return;
		}
		float num2 = UnityEngine.Random.Range(Ability_EV.CROWS_NEST_RANDOM_BULLET_ANGLES.x, Ability_EV.CROWS_NEST_RANDOM_BULLET_ANGLES.y);
		Vector2 offset = this.m_crowsNestProjectile.transform.position + new Vector2(1.15f, 0f);
		if (this.m_crowsNestProjectile.transform.localScale.x < 0f)
		{
			num2 = 180f - num2;
			offset = this.m_crowsNestProjectile.transform.position - new Vector2(1.15f, 0f);
		}
		Projectile_RL projectile = ProjectileManager.FireProjectile(this.m_abilityController.PlayerController.gameObject, this.m_fireRepeatProjectileName, offset, false, num2, 1f, true, true, true, true);
		this.m_abilityController.InitializeProjectile(projectile, CastAbilityType.Talent);
	}

	// Token: 0x06000DAF RID: 3503 RVA: 0x00029EF5 File Offset: 0x000280F5
	private void ResumeCooldownIfPlayerExitsRoom(object sender, EventArgs args)
	{
		if (this.m_decreasesCDOverTime)
		{
			base.DecreaseCooldownOverTime = true;
		}
		if (this.m_decreasesCDOnHit)
		{
			base.DecreaseCooldownWhenHit = true;
		}
		base.DisplayPausedAbilityCooldown = false;
	}

	// Token: 0x06000DB0 RID: 3504 RVA: 0x00029F1C File Offset: 0x0002811C
	private void ResumeCooldown(Projectile_RL proj, GameObject obj)
	{
		if (this.m_decreasesCDOverTime)
		{
			base.DecreaseCooldownOverTime = true;
		}
		if (this.m_decreasesCDOnHit)
		{
			base.DecreaseCooldownWhenHit = true;
		}
		base.DisplayPausedAbilityCooldown = false;
	}

	// Token: 0x06000DB1 RID: 3505 RVA: 0x00029F44 File Offset: 0x00028144
	private void LateUpdate()
	{
		if (this.m_crowsNestActive)
		{
			bool flag = this.m_crowsNestProjectile && this.m_crowsNestProjectile.gameObject.activeSelf && !this.m_abilityController.PlayerController.CharacterDash.IsDashing && ((!this.m_platformActivated && this.m_abilityController.PlayerController.ControllerCorgi.StandingOnCollider && this.m_abilityController.PlayerController.ControllerCorgi.StandingOnCollider == this.m_crowsNestCollider) || (this.m_platformActivated && Mathf.Abs(this.m_abilityController.PlayerController.transform.localPosition.y - this.m_crowsNestProjectile.transform.localPosition.y) <= 0.1f));
			if (!this.m_platformActivated && flag)
			{
				this.ActivatePlatform();
			}
			if (this.m_platformActivated)
			{
				if (flag)
				{
					this.m_movedThisFrame = false;
					this.HandlePlatformControls();
					if (Time.time > this.m_fireRepeatTimer)
					{
						this.m_fireRepeatTimer = Time.time + 0.15f;
						this.FireProjectileOnRepeat();
					}
					if (this.m_crowsNestProjectile.IsFlipped == this.m_abilityController.PlayerController.IsFacingRight)
					{
						this.m_crowsNestProjectile.Flip();
					}
					if (this.m_movedThisFrame)
					{
						if (!this.m_wasMoving)
						{
							AudioManager.PlayOneShotAttached(this, "event:/SFX/Weapons/sfx_weapon_pirateShip_move_start", base.gameObject);
						}
						this.m_spawnLoopInstance.setParameterByName("pirateShip_move", 1f, false);
					}
					else
					{
						if (this.m_wasMoving)
						{
							AudioManager.PlayOneShotAttached(this, "event:/SFX/Weapons/sfx_weapon_pirateShip_move_stop", base.gameObject);
						}
						this.m_spawnLoopInstance.setParameterByName("pirateShip_move", 0f, false);
					}
					this.m_wasMoving = this.m_movedThisFrame;
				}
				else
				{
					this.DeactivatePlatform();
				}
			}
			else if (Time.time > this.m_fireRepeatTimer)
			{
				this.m_fireRepeatTimer = Time.time + 0.15f;
				this.FireProjectileOnRepeat();
			}
			if (Rewired_RL.Player.GetButtonDown(this.m_abilityController.GetAbilityInputString(base.CastAbilityType)))
			{
				this.DestroyCrowsNest(true);
			}
		}
	}

	// Token: 0x06000DB2 RID: 3506 RVA: 0x0002A178 File Offset: 0x00028378
	private void ActivatePlatform()
	{
		this.m_crowsNestCollider.gameObject.layer = 9;
		Vector3 localPosition = this.m_abilityController.PlayerController.transform.localPosition;
		localPosition.x = this.m_crowsNestProjectile.transform.localPosition.x;
		this.m_abilityController.PlayerController.transform.localPosition = localPosition;
		this.m_abilityController.PlayerController.ControllerCorgi.SetRaysParameters();
		if (this.m_crowsNextSideColliderGO)
		{
			this.m_crowsNextSideColliderGO.SetActive(true);
		}
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerDash, this.m_onPlayerDash);
		this.m_platformActivated = true;
		this.m_abilityController.PlayerController.ControllerCorgi.DisableOneWayCollision = true;
	}

	// Token: 0x06000DB3 RID: 3507 RVA: 0x0002A238 File Offset: 0x00028438
	private void DeactivatePlatform()
	{
		this.m_crowsNestCollider.gameObject.layer = 11;
		if (this.m_crowsNextSideColliderGO)
		{
			this.m_crowsNextSideColliderGO.SetActive(false);
		}
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerDash, this.m_onPlayerDash);
		this.m_platformActivated = false;
		this.m_abilityController.PlayerController.ControllerCorgi.DisableOneWayCollision = false;
	}

	// Token: 0x06000DB4 RID: 3508 RVA: 0x0002A29A File Offset: 0x0002849A
	private void OnPlayerDash(object sender, EventArgs args)
	{
	}

	// Token: 0x06000DB5 RID: 3509 RVA: 0x0002A29C File Offset: 0x0002849C
	private void HandlePlatformControls()
	{
		global::PlayerController playerController = this.m_abilityController.PlayerController;
		if (playerController.CastAbility.IsAiming)
		{
			return;
		}
		if (ReInput.isReady && Rewired_RL.Player.GetButton("FreeLook"))
		{
			return;
		}
		if (Rewired_RL.Player.GetButtonDown("Jump"))
		{
			this.DeactivatePlatform();
			return;
		}
		if (playerController.CharacterDash.IsDashing)
		{
			this.DeactivatePlatform();
			return;
		}
		int num = 0;
		num |= 1280;
		float y = playerController.CurrentlyInRoom.Bounds.max.y;
		Bounds bounds = default(Bounds);
		bounds.center = playerController.transform.position;
		bounds.Encapsulate(playerController.HitboxController.GetCollider(HitboxType.Platform).bounds);
		bounds.Encapsulate(this.m_crowsNestCollider.bounds);
		Vector2 origin = bounds.center;
		Vector2 size = bounds.size;
		float num2 = 0.5f;
		RaycastHit2D hit = Physics2D.BoxCast(origin, size, 0f, Vector2.up, num2, num);
		bool flag = (!hit || (hit && bounds.center.y > hit.point.y)) && bounds.max.y < y - num2;
		float y2 = playerController.CurrentlyInRoom.Bounds.min.y;
		RaycastHit2D hit2 = Physics2D.BoxCast(origin, size, 0f, Vector2.down, num2, num);
		bool flag2 = (!hit2 || (hit2 && bounds.center.y < hit2.point.y)) && bounds.min.y > y2 + num2;
		float num3 = 14f * Time.deltaTime;
		float x = playerController.CurrentlyInRoom.Bounds.min.x;
		RaycastHit2D hit3 = Physics2D.BoxCast(origin, size, 0f, Vector2.left, num2, num);
		bool flag3 = (!hit3 || (hit3 && bounds.center.x < hit3.point.x && hit3.normal.x == -1f)) && bounds.min.x > x + num2;
		float x2 = playerController.CurrentlyInRoom.Bounds.max.x;
		RaycastHit2D hit4 = Physics2D.BoxCast(origin, size, 0f, Vector2.right, num2, num);
		bool flag4 = (!hit4 || (hit4 && bounds.center.x > hit4.point.x && hit4.normal.x == 1f)) && bounds.max.x < x2 - num2;
		float num4 = Rewired_RL.Player.GetAxis("MoveVertical");
		float num5 = Rewired_RL.Player.GetAxis("MoveHorizontal");
		if (!flag && num4 > 0f)
		{
			num4 = 0f;
		}
		if (!flag2 && num4 < 0f)
		{
			num4 = 0f;
		}
		if (!flag3 && num5 < 0f)
		{
			num5 = 0f;
		}
		if (!flag4 && num5 > 0f)
		{
			num5 = 0f;
		}
		playerController.transform.SetPositionX(playerController.transform.localPosition.x + num5 * num3);
		playerController.transform.SetPositionY(playerController.transform.localPosition.y + num4 * num3);
		playerController.ControllerCorgi.SetRaysParameters();
		playerController.ControllerCorgi.ResetState();
		this.m_crowsNestProjectile.transform.SetPositionX(this.m_crowsNestProjectile.transform.position.x + num5 * num3);
		this.m_crowsNestProjectile.transform.SetPositionY(this.m_crowsNestProjectile.transform.position.y + num4 * num3);
		playerController.ControllerCorgi.ForceStandingOn(this.m_crowsNestCollider);
		if (num5 != 0f || num4 != 0f)
		{
			this.m_movedThisFrame = true;
		}
	}

	// Token: 0x06000DB6 RID: 3510 RVA: 0x0002A6CC File Offset: 0x000288CC
	private void OnPlayerExitRoom(object sender, EventArgs args)
	{
		this.DestroyCrowsNest(false);
	}

	// Token: 0x06000DB7 RID: 3511 RVA: 0x0002A6D5 File Offset: 0x000288D5
	private void OnPlayerDeath(object sender, EventArgs args)
	{
		this.DestroyCrowsNest(false);
	}

	// Token: 0x06000DB8 RID: 3512 RVA: 0x0002A6DE File Offset: 0x000288DE
	private void OnPlayerFakeDeath(object sender, EventArgs args)
	{
		base.StartCoroutine(this.OnPlayerFakeDeathCoroutine());
	}

	// Token: 0x06000DB9 RID: 3513 RVA: 0x0002A6ED File Offset: 0x000288ED
	private IEnumerator OnPlayerFakeDeathCoroutine()
	{
		if (this.m_stingerInstance.isValid())
		{
			PLAYBACK_STATE playback_STATE;
			this.m_stingerInstance.getPlaybackState(out playback_STATE);
			if (playback_STATE != PLAYBACK_STATE.STOPPED && playback_STATE != PLAYBACK_STATE.STOPPING)
			{
				this.m_stingerInstance.setParameterByName("pirateShip_on", 0f, false);
			}
		}
		yield return null;
		while (WindowManager.GetIsWindowOpen(WindowID.DeathDefy))
		{
			yield return null;
		}
		if (this.m_crowsNestActive && this.m_stingerInstance.isValid())
		{
			AudioManager.Play(null, this.m_stingerInstance);
			this.m_stingerInstance.setParameterByName("pirateShip_on", 1f, false);
		}
		yield break;
	}

	// Token: 0x06000DBA RID: 3514 RVA: 0x0002A6FC File Offset: 0x000288FC
	private void OnTimeout(GameObject obj)
	{
		this.DestroyCrowsNest(true);
	}

	// Token: 0x06000DBB RID: 3515 RVA: 0x0002A708 File Offset: 0x00028908
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (!GameManager.IsApplicationClosing)
		{
			this.DestroyCrowsNest(false);
		}
		if (this.m_stingerInstance.isValid())
		{
			this.m_stingerInstance.release();
		}
		if (this.m_spawnLoopInstance.isValid())
		{
			this.m_spawnLoopInstance.release();
		}
	}

	// Token: 0x06000DBC RID: 3516 RVA: 0x0002A75C File Offset: 0x0002895C
	private void DestroyCrowsNest(bool fireDestroyProjectile)
	{
		if (this.m_crowsNestProjectile && this.m_crowsNestProjectile.gameObject.activeSelf && !this.m_crowsNestProjectile.IsFreePoolObj)
		{
			if (fireDestroyProjectile)
			{
				float angleInDeg = 0f;
				Projectile_RL projectile_RL = ProjectileManager.FireProjectile(this.m_crowsNestProjectile.gameObject, this.m_crowsNestDestroyProjectile, Vector2.zero, true, angleInDeg, 1f, false, true, true, true);
				this.m_abilityController.InitializeProjectile(projectile_RL, CastAbilityType.Talent);
				projectile_RL.Owner = this.m_abilityController.gameObject;
				ProjectileManager.ApplyProjectileDamage(this.m_abilityController.PlayerController, projectile_RL);
				projectile_RL.RotationSpeed = projectile_RL.InitialRotationSpeed;
				if (this.m_crowsNestProjectile.IsFlipped)
				{
					projectile_RL.Heading = new Vector2(-projectile_RL.Heading.x, projectile_RL.Heading.y);
				}
			}
			if (this.m_crowsNextSideColliderGO)
			{
				this.m_crowsNextSideColliderGO.SetActive(false);
			}
			this.m_crowsNestProjectile.HitboxController.DisableAllCollisions = true;
			this.m_crowsNestProjectile.FlagForDestruction(null);
		}
		if (this.m_stingerInstance.isValid())
		{
			PLAYBACK_STATE playback_STATE;
			this.m_stingerInstance.getPlaybackState(out playback_STATE);
			if (playback_STATE != PLAYBACK_STATE.STOPPED && playback_STATE != PLAYBACK_STATE.STOPPING)
			{
				this.m_stingerInstance.setParameterByName("pirateShip_on", 0f, false);
			}
		}
		if (this.m_spawnLoopInstance.isValid())
		{
			AudioManager.Stop(this.m_spawnLoopInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}
		this.m_abilityController.PlayerController.ControllerCorgi.DisableOneWayCollision = false;
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerDash, this.m_onPlayerDash);
		this.m_crowsNextSideColliderGO = null;
		this.m_crowsNestCollider = null;
		this.m_platformActivated = false;
		this.m_crowsNestActive = false;
	}

	// Token: 0x040010E5 RID: 4325
	private const string CROWS_NEST_STINGER = "event:/Stingers/sting_pirateShip_loop";

	// Token: 0x040010E6 RID: 4326
	private const string CROWS_NEST_MOVE_START = "event:/SFX/Weapons/sfx_weapon_pirateShip_move_start";

	// Token: 0x040010E7 RID: 4327
	private const string CROWS_NEST_MOVE_STOP = "event:/SFX/Weapons/sfx_weapon_pirateShip_move_stop";

	// Token: 0x040010E8 RID: 4328
	private const string CROWS_NEST_SPAWN_LOOP = "event:/SFX/Weapons/sfx_weapon_pirateShip_spawn_loop";

	// Token: 0x040010E9 RID: 4329
	[SerializeField]
	private string m_crowsNestDestroyProjectile;

	// Token: 0x040010EA RID: 4330
	[SerializeField]
	private string m_fireRepeatProjectileName;

	// Token: 0x040010EB RID: 4331
	private bool m_crowsNestActive;

	// Token: 0x040010EC RID: 4332
	private bool m_platformActivated;

	// Token: 0x040010ED RID: 4333
	private Projectile_RL m_crowsNestProjectile;

	// Token: 0x040010EE RID: 4334
	private BoxCollider2D m_crowsNestCollider;

	// Token: 0x040010EF RID: 4335
	private float m_crowsNextMoveAmount;

	// Token: 0x040010F0 RID: 4336
	private GameObject m_crowsNextSideColliderGO;

	// Token: 0x040010F1 RID: 4337
	private bool m_wasMoving;

	// Token: 0x040010F2 RID: 4338
	private bool m_movedThisFrame;

	// Token: 0x040010F3 RID: 4339
	private Action<MonoBehaviour, EventArgs> m_onPlayerExitRoom;

	// Token: 0x040010F4 RID: 4340
	private Action<MonoBehaviour, EventArgs> m_resumeCooldownIfPlayerExitsRoom;

	// Token: 0x040010F5 RID: 4341
	private Action<MonoBehaviour, EventArgs> m_onPlayerDash;

	// Token: 0x040010F6 RID: 4342
	private Action<MonoBehaviour, EventArgs> m_onPlayerDeath;

	// Token: 0x040010F7 RID: 4343
	private Action<MonoBehaviour, EventArgs> m_onPlayerFakeDeath;

	// Token: 0x040010F8 RID: 4344
	private Action<Projectile_RL, GameObject> m_resumeCooldown;

	// Token: 0x040010F9 RID: 4345
	private bool m_decreasesCDOverTime;

	// Token: 0x040010FA RID: 4346
	private bool m_decreasesCDOnHit;

	// Token: 0x040010FB RID: 4347
	private Action<GameObject> m_onTimeout;

	// Token: 0x040010FC RID: 4348
	private EventInstance m_stingerInstance;

	// Token: 0x040010FD RID: 4349
	private EventInstance m_spawnLoopInstance;

	// Token: 0x040010FE RID: 4350
	private float m_fireRepeatTimer;
}
