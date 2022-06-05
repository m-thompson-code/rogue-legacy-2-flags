using System;
using System.Collections;
using FMOD.Studio;
using Rewired;
using RLAudio;
using RL_Windows;
using UnityEngine;

// Token: 0x020002C8 RID: 712
public class CrowsNest_Ability : BaseAbility_RL, ITalent, IAbility
{
	// Token: 0x06001512 RID: 5394 RVA: 0x0000A885 File Offset: 0x00008A85
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			this.m_projectileName,
			this.m_crowsNestDestroyProjectile,
			this.m_fireRepeatProjectileName
		};
	}

	// Token: 0x170009CD RID: 2509
	// (get) Token: 0x06001513 RID: 5395 RVA: 0x00004A6C File Offset: 0x00002C6C
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 2.25f;
		}
	}

	// Token: 0x170009CE RID: 2510
	// (get) Token: 0x06001514 RID: 5396 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170009CF RID: 2511
	// (get) Token: 0x06001515 RID: 5397 RVA: 0x00003C70 File Offset: 0x00001E70
	protected override float TellAnimSpeed
	{
		get
		{
			return 10f;
		}
	}

	// Token: 0x170009D0 RID: 2512
	// (get) Token: 0x06001516 RID: 5398 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float TellAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170009D1 RID: 2513
	// (get) Token: 0x06001517 RID: 5399 RVA: 0x00003C70 File Offset: 0x00001E70
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			return 10f;
		}
	}

	// Token: 0x170009D2 RID: 2514
	// (get) Token: 0x06001518 RID: 5400 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170009D3 RID: 2515
	// (get) Token: 0x06001519 RID: 5401 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected override float AttackAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170009D4 RID: 2516
	// (get) Token: 0x0600151A RID: 5402 RVA: 0x00003CBD File Offset: 0x00001EBD
	protected override float AttackAnimExitDelay
	{
		get
		{
			return 0.15f;
		}
	}

	// Token: 0x170009D5 RID: 2517
	// (get) Token: 0x0600151B RID: 5403 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected override float ExitAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170009D6 RID: 2518
	// (get) Token: 0x0600151C RID: 5404 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float ExitAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170009D7 RID: 2519
	// (get) Token: 0x0600151D RID: 5405 RVA: 0x0000A8AE File Offset: 0x00008AAE
	public bool CrowsNestActive
	{
		get
		{
			return this.m_crowsNestActive;
		}
	}

	// Token: 0x0600151E RID: 5406 RVA: 0x00088C34 File Offset: 0x00086E34
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

	// Token: 0x0600151F RID: 5407 RVA: 0x0000A8B6 File Offset: 0x00008AB6
	private void OnEnable()
	{
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerExitRoom, this.m_onPlayerExitRoom);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerDeath, this.m_onPlayerDeath);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerFakedDeath, this.m_onPlayerFakeDeath);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerEnterRoom, this.m_resumeCooldownIfPlayerExitsRoom);
	}

	// Token: 0x06001520 RID: 5408 RVA: 0x0000A8EB File Offset: 0x00008AEB
	private void OnDisable()
	{
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerExitRoom, this.m_onPlayerExitRoom);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerDeath, this.m_onPlayerDeath);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerFakedDeath, this.m_onPlayerFakeDeath);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerEnterRoom, this.m_resumeCooldownIfPlayerExitsRoom);
	}

	// Token: 0x06001521 RID: 5409 RVA: 0x0000A920 File Offset: 0x00008B20
	public override void PreCastAbility()
	{
		this.DestroyCrowsNest(true);
		base.PreCastAbility();
	}

	// Token: 0x06001522 RID: 5410 RVA: 0x00088D0C File Offset: 0x00086F0C
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

	// Token: 0x06001523 RID: 5411 RVA: 0x00088F80 File Offset: 0x00087180
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

	// Token: 0x06001524 RID: 5412 RVA: 0x0000A92F File Offset: 0x00008B2F
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

	// Token: 0x06001525 RID: 5413 RVA: 0x0000A92F File Offset: 0x00008B2F
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

	// Token: 0x06001526 RID: 5414 RVA: 0x00089078 File Offset: 0x00087278
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

	// Token: 0x06001527 RID: 5415 RVA: 0x000892AC File Offset: 0x000874AC
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

	// Token: 0x06001528 RID: 5416 RVA: 0x0008936C File Offset: 0x0008756C
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

	// Token: 0x06001529 RID: 5417 RVA: 0x00002FCA File Offset: 0x000011CA
	private void OnPlayerDash(object sender, EventArgs args)
	{
	}

	// Token: 0x0600152A RID: 5418 RVA: 0x000893D0 File Offset: 0x000875D0
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

	// Token: 0x0600152B RID: 5419 RVA: 0x0000A956 File Offset: 0x00008B56
	private void OnPlayerExitRoom(object sender, EventArgs args)
	{
		this.DestroyCrowsNest(false);
	}

	// Token: 0x0600152C RID: 5420 RVA: 0x0000A956 File Offset: 0x00008B56
	private void OnPlayerDeath(object sender, EventArgs args)
	{
		this.DestroyCrowsNest(false);
	}

	// Token: 0x0600152D RID: 5421 RVA: 0x0000A95F File Offset: 0x00008B5F
	private void OnPlayerFakeDeath(object sender, EventArgs args)
	{
		base.StartCoroutine(this.OnPlayerFakeDeathCoroutine());
	}

	// Token: 0x0600152E RID: 5422 RVA: 0x0000A96E File Offset: 0x00008B6E
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

	// Token: 0x0600152F RID: 5423 RVA: 0x0000A97D File Offset: 0x00008B7D
	private void OnTimeout(GameObject obj)
	{
		this.DestroyCrowsNest(true);
	}

	// Token: 0x06001530 RID: 5424 RVA: 0x00089800 File Offset: 0x00087A00
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

	// Token: 0x06001531 RID: 5425 RVA: 0x00089854 File Offset: 0x00087A54
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

	// Token: 0x04001654 RID: 5716
	private const string CROWS_NEST_STINGER = "event:/Stingers/sting_pirateShip_loop";

	// Token: 0x04001655 RID: 5717
	private const string CROWS_NEST_MOVE_START = "event:/SFX/Weapons/sfx_weapon_pirateShip_move_start";

	// Token: 0x04001656 RID: 5718
	private const string CROWS_NEST_MOVE_STOP = "event:/SFX/Weapons/sfx_weapon_pirateShip_move_stop";

	// Token: 0x04001657 RID: 5719
	private const string CROWS_NEST_SPAWN_LOOP = "event:/SFX/Weapons/sfx_weapon_pirateShip_spawn_loop";

	// Token: 0x04001658 RID: 5720
	[SerializeField]
	private string m_crowsNestDestroyProjectile;

	// Token: 0x04001659 RID: 5721
	[SerializeField]
	private string m_fireRepeatProjectileName;

	// Token: 0x0400165A RID: 5722
	private bool m_crowsNestActive;

	// Token: 0x0400165B RID: 5723
	private bool m_platformActivated;

	// Token: 0x0400165C RID: 5724
	private Projectile_RL m_crowsNestProjectile;

	// Token: 0x0400165D RID: 5725
	private BoxCollider2D m_crowsNestCollider;

	// Token: 0x0400165E RID: 5726
	private float m_crowsNextMoveAmount;

	// Token: 0x0400165F RID: 5727
	private GameObject m_crowsNextSideColliderGO;

	// Token: 0x04001660 RID: 5728
	private bool m_wasMoving;

	// Token: 0x04001661 RID: 5729
	private bool m_movedThisFrame;

	// Token: 0x04001662 RID: 5730
	private Action<MonoBehaviour, EventArgs> m_onPlayerExitRoom;

	// Token: 0x04001663 RID: 5731
	private Action<MonoBehaviour, EventArgs> m_resumeCooldownIfPlayerExitsRoom;

	// Token: 0x04001664 RID: 5732
	private Action<MonoBehaviour, EventArgs> m_onPlayerDash;

	// Token: 0x04001665 RID: 5733
	private Action<MonoBehaviour, EventArgs> m_onPlayerDeath;

	// Token: 0x04001666 RID: 5734
	private Action<MonoBehaviour, EventArgs> m_onPlayerFakeDeath;

	// Token: 0x04001667 RID: 5735
	private Action<Projectile_RL, GameObject> m_resumeCooldown;

	// Token: 0x04001668 RID: 5736
	private bool m_decreasesCDOverTime;

	// Token: 0x04001669 RID: 5737
	private bool m_decreasesCDOnHit;

	// Token: 0x0400166A RID: 5738
	private Action<GameObject> m_onTimeout;

	// Token: 0x0400166B RID: 5739
	private EventInstance m_stingerInstance;

	// Token: 0x0400166C RID: 5740
	private EventInstance m_spawnLoopInstance;

	// Token: 0x0400166D RID: 5741
	private float m_fireRepeatTimer;
}
