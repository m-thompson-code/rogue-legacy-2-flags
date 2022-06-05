using System;
using System.Collections;
using FMOD.Studio;
using RLAudio;
using UnityEngine;

// Token: 0x020005F1 RID: 1521
[CreateAssetMenu(menuName = "Custom/Biome Rules/Cave Biome Up")]
public class CaveBiomeUp_BiomeRule : BiomeRule
{
	// Token: 0x17001370 RID: 4976
	// (get) Token: 0x060036E5 RID: 14053 RVA: 0x000BCB26 File Offset: 0x000BAD26
	public override BiomeRuleExecutionTime ExecutionTime
	{
		get
		{
			return BiomeRuleExecutionTime.PlayerEnterBiome;
		}
	}

	// Token: 0x060036E6 RID: 14054 RVA: 0x000BCB2A File Offset: 0x000BAD2A
	public override IEnumerator RunRule(BiomeType biome)
	{
		if (!this.m_actionsInitialized)
		{
			if (!this.m_prepLeftEventInstance.isValid())
			{
				this.m_prepLeftEventInstance = AudioUtility.GetEventInstance("event:/SFX/Enemies/sfx_hazard_bladeStamped_prep_start_loop_left", CameraController.UICamera.transform);
			}
			if (!this.m_prepRightEventInstance.isValid())
			{
				this.m_prepRightEventInstance = AudioUtility.GetEventInstance("event:/SFX/Enemies/sfx_hazard_bladeStamped_prep_start_loop_right", CameraController.UICamera.transform);
			}
			if (!this.m_lockedLeftEventInstance.isValid())
			{
				this.m_lockedLeftEventInstance = AudioUtility.GetEventInstance("event:/SFX/Enemies/sfx_hazard_bladeStamped_prep_telegraph_left", CameraController.UICamera.transform);
			}
			if (!this.m_lockedRightEventInstance.isValid())
			{
				this.m_lockedRightEventInstance = AudioUtility.GetEventInstance("event:/SFX/Enemies/sfx_hazard_bladeStamped_prep_telegraph_right", CameraController.UICamera.transform);
			}
			this.m_onPlayerEnterRoom = new Action<object, EventArgs>(this.OnPlayerEnterRoom);
			this.m_onPlayerExitRoom = new Action<object, EventArgs>(this.OnPlayerExitRoom);
			this.m_actionsInitialized = true;
		}
		if (BurdenManager.IsBurdenActive(BurdenType.CaveBiomeUp))
		{
			ProjectileManager.Instance.AddProjectileToPool("CaveBossWaveWarningProjectile");
			ProjectileManager.Instance.AddProjectileToPool("CaveBossWaveWarningLockedProjectile");
			ProjectileManager.Instance.AddProjectileToPool("CaveBossWaveProjectile");
			Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
			Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerExitRoom, this.m_onPlayerExitRoom);
		}
		yield break;
	}

	// Token: 0x060036E7 RID: 14055 RVA: 0x000BCB3C File Offset: 0x000BAD3C
	public override void UndoRule(BiomeType biome)
	{
		if (this.m_prepLeftEventInstance.isValid())
		{
			this.m_prepLeftEventInstance.release();
		}
		if (this.m_prepRightEventInstance.isValid())
		{
			this.m_prepRightEventInstance.release();
		}
		if (this.m_lockedLeftEventInstance.isValid())
		{
			this.m_lockedLeftEventInstance.release();
		}
		if (this.m_lockedRightEventInstance.isValid())
		{
			this.m_lockedRightEventInstance.release();
		}
		if (this.m_actionsInitialized)
		{
			Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
			Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerExitRoom, this.m_onPlayerExitRoom);
		}
	}

	// Token: 0x060036E8 RID: 14056 RVA: 0x000BCBCE File Offset: 0x000BADCE
	protected override void OnDisable()
	{
		base.OnDisable();
		if (!GameManager.IsApplicationClosing)
		{
			this.OnPlayerExitRoom(null, null);
		}
	}

	// Token: 0x060036E9 RID: 14057 RVA: 0x000BCBE8 File Offset: 0x000BADE8
	private void OnPlayerEnterRoom(object sender, EventArgs args)
	{
		RoomViaDoorEventArgs roomViaDoorEventArgs = args as RoomViaDoorEventArgs;
		if (roomViaDoorEventArgs != null)
		{
			BaseRoom room = roomViaDoorEventArgs.Room;
			Room room2 = room as Room;
			MergeRoom mergeRoom = room as MergeRoom;
			if ((room.RoomType == RoomType.Standard || room.RoomType == RoomType.Trap) && ((room2 && !room2.GridPointManager.IsTunnelDestination) || (mergeRoom && !mergeRoom.StandaloneGridPointManagers[0].IsTunnelDestination)))
			{
				this.m_currentRoom = room;
				this.m_waveAttackCoroutine = this.m_currentRoom.StartCoroutine(this.Wave_Attack_Coroutine());
			}
		}
	}

	// Token: 0x060036EA RID: 14058 RVA: 0x000BCC70 File Offset: 0x000BAE70
	private void OnPlayerExitRoom(object sender, EventArgs args)
	{
		this.StopWaveAttackCoroutine();
		this.m_waveAttackCoroutine = null;
		if (this.m_lockedLeftEventInstance.isValid())
		{
			AudioManager.Stop(this.m_lockedLeftEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}
		if (this.m_lockedRightEventInstance.isValid())
		{
			AudioManager.Stop(this.m_lockedRightEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}
		if (this.m_prepLeftEventInstance.isValid())
		{
			AudioManager.Stop(this.m_prepLeftEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}
		if (this.m_prepRightEventInstance.isValid())
		{
			AudioManager.Stop(this.m_prepRightEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}
	}

	// Token: 0x17001371 RID: 4977
	// (get) Token: 0x060036EB RID: 14059 RVA: 0x000BCCEE File Offset: 0x000BAEEE
	private Vector2 m_waveAttack_Initial_Delay
	{
		get
		{
			return Hazard_EV.FLYING_WEAPON_WAVE_INITIAL_DELAY;
		}
	}

	// Token: 0x17001372 RID: 4978
	// (get) Token: 0x060036EC RID: 14060 RVA: 0x000BCCF5 File Offset: 0x000BAEF5
	private Vector2 m_waveAttackRandomDelay
	{
		get
		{
			return Hazard_EV.FLYING_WEAPON_WAVE_RANDOM_DELAY;
		}
	}

	// Token: 0x17001373 RID: 4979
	// (get) Token: 0x060036ED RID: 14061 RVA: 0x000BCCFC File Offset: 0x000BAEFC
	private Vector2 m_waveAttackRandomPosOffset
	{
		get
		{
			return Hazard_EV.FLYING_WEAPON_WAVE_RANDOM_POS_OFFSET;
		}
	}

	// Token: 0x17001374 RID: 4980
	// (get) Token: 0x060036EE RID: 14062 RVA: 0x000BCD03 File Offset: 0x000BAF03
	private Vector2 m_waveAttackRandomAngle
	{
		get
		{
			return Hazard_EV.FLYING_WEAPON_WAVE_RANDOM_ANGLE;
		}
	}

	// Token: 0x060036EF RID: 14063 RVA: 0x000BCD0A File Offset: 0x000BAF0A
	public IEnumerator Wave_Attack_Coroutine()
	{
		float delay = Time.time + UnityEngine.Random.Range(this.m_waveAttack_Initial_Delay.x, this.m_waveAttack_Initial_Delay.y);
		for (;;)
		{
			if (Time.time >= delay)
			{
				yield return this.FireWaveProjectile();
				delay = Time.time + UnityEngine.Random.Range(this.m_waveAttackRandomDelay.x, this.m_waveAttackRandomDelay.y);
			}
			else
			{
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x060036F0 RID: 14064 RVA: 0x000BCD19 File Offset: 0x000BAF19
	private IEnumerator FireWaveProjectile()
	{
		PlayerController playerController = PlayerManager.GetPlayerController();
		float startingAngle = 0f;
		Vector2 vector = playerController.Midpoint;
		int num = CDGHelper.RandomPlusMinus();
		int num2 = CDGHelper.RandomPlusMinus();
		float num3 = UnityEngine.Random.Range(this.m_waveAttackRandomPosOffset.x, this.m_waveAttackRandomPosOffset.y);
		if (num > 0)
		{
			if (num2 > 0)
			{
				startingAngle = 0f;
				vector.x -= 32f;
			}
			else
			{
				startingAngle = 180f;
				vector.x += 32f;
			}
			vector.y += num3;
		}
		else
		{
			if (num2 > 0)
			{
				startingAngle = -90f;
				vector.y += 18f;
			}
			else
			{
				startingAngle = 90f;
				vector.y -= 18f;
			}
			vector.x += num3;
		}
		float num4 = UnityEngine.Random.Range(this.m_waveAttackRandomAngle.x, this.m_waveAttackRandomAngle.y);
		startingAngle += num4;
		startingAngle = CDGHelper.WrapAngleDegrees(startingAngle, true);
		bool playSFXLeft = vector.x < playerController.Midpoint.x;
		if (playSFXLeft)
		{
			if (this.m_prepLeftEventInstance.isValid())
			{
				AudioManager.Play(null, this.m_prepLeftEventInstance);
			}
		}
		else if (this.m_prepRightEventInstance.isValid())
		{
			AudioManager.Play(null, this.m_prepRightEventInstance);
		}
		this.m_waveAttack_WarningProjectile = ProjectileManager.FireProjectile(this.m_currentRoom.gameObject, "CaveBossWaveWarningProjectile", vector, false, startingAngle, 1f, true, true, true, true);
		Vector3 localPosition = this.m_waveAttack_WarningProjectile.transform.localPosition;
		float delay = Time.time + this.m_waveAttackWarningDuration;
		while (Time.time < delay)
		{
			yield return null;
		}
		Vector3 warningProjPos = this.m_waveAttack_WarningProjectile.transform.position;
		this.StopProjectile(ref this.m_waveAttack_WarningProjectile);
		if (playSFXLeft)
		{
			if (this.m_prepLeftEventInstance.isValid())
			{
				AudioManager.Stop(this.m_prepLeftEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			}
			if (this.m_lockedLeftEventInstance.isValid())
			{
				AudioManager.Play(null, this.m_lockedLeftEventInstance);
			}
		}
		else
		{
			if (this.m_prepRightEventInstance.isValid())
			{
				AudioManager.Stop(this.m_prepRightEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			}
			if (this.m_lockedRightEventInstance.isValid())
			{
				AudioManager.Play(null, this.m_lockedRightEventInstance);
			}
		}
		this.m_waveAttack_WarningLockedProjectile = ProjectileManager.FireProjectile(this.m_currentRoom.gameObject, "CaveBossWaveWarningLockedProjectile", warningProjPos, false, startingAngle, 1f, true, true, true, true);
		float delayLocked = Time.time + this.m_waveAttackWarningLockedDuration;
		while (Time.time < delayLocked)
		{
			yield return null;
		}
		this.StopProjectile(ref this.m_waveAttack_WarningLockedProjectile);
		if (playSFXLeft)
		{
			if (this.m_lockedLeftEventInstance.isValid())
			{
				AudioManager.Stop(this.m_lockedLeftEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			}
		}
		else if (this.m_lockedRightEventInstance.isValid())
		{
			AudioManager.Stop(this.m_lockedRightEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}
		if (playSFXLeft)
		{
			AudioManager.PlayOneShot(null, "event:/SFX/Enemies/sfx_hazard_bladeStamped_launch_left", CameraController.UICamera.transform.position);
		}
		else
		{
			AudioManager.PlayOneShot(null, "event:/SFX/Enemies/sfx_hazard_bladeStamped_launch_right", CameraController.UICamera.transform.position);
		}
		this.m_waveAttack_Projectile = ProjectileManager.FireProjectile(this.m_currentRoom.gameObject, "CaveBossWaveProjectile", warningProjPos, false, startingAngle, 1f, true, true, true, true);
		yield break;
	}

	// Token: 0x060036F1 RID: 14065 RVA: 0x000BCD28 File Offset: 0x000BAF28
	private void StopWaveAttackCoroutine()
	{
		this.StopProjectile(ref this.m_waveAttack_Projectile);
		this.StopProjectile(ref this.m_waveAttack_WarningProjectile);
		this.StopProjectile(ref this.m_waveAttack_WarningLockedProjectile);
		if (this.m_waveAttackCoroutine != null && this.m_currentRoom)
		{
			this.m_currentRoom.StopCoroutine(this.m_waveAttackCoroutine);
		}
	}

	// Token: 0x060036F2 RID: 14066 RVA: 0x000BCD7F File Offset: 0x000BAF7F
	protected void StopProjectile(ref Projectile_RL projectile)
	{
		if (projectile && projectile.isActiveAndEnabled)
		{
			projectile.FlagForDestruction(null);
		}
		projectile = null;
	}

	// Token: 0x04002A3F RID: 10815
	private const string WAVE_ATTACK_WARNING_PROJECTILE = "CaveBossWaveWarningProjectile";

	// Token: 0x04002A40 RID: 10816
	private const string WAVE_ATTACK_WARNING_LOCKED_PROJECTILE = "CaveBossWaveWarningLockedProjectile";

	// Token: 0x04002A41 RID: 10817
	private const string WAVE_ATTACK_PROJECTILE = "CaveBossWaveProjectile";

	// Token: 0x04002A42 RID: 10818
	private const string WAVE_ATTACK_SFX_PREP_LEFT_NAME = "event:/SFX/Enemies/sfx_hazard_bladeStamped_prep_start_loop_left";

	// Token: 0x04002A43 RID: 10819
	private const string WAVE_ATTACK_SFX_PREP_RIGHT_NAME = "event:/SFX/Enemies/sfx_hazard_bladeStamped_prep_start_loop_right";

	// Token: 0x04002A44 RID: 10820
	private const string WAVE_ATTACK_SFX_LOCKED_LEFT_NAME = "event:/SFX/Enemies/sfx_hazard_bladeStamped_prep_telegraph_left";

	// Token: 0x04002A45 RID: 10821
	private const string WAVE_ATTACK_SFX_LOCKED_RIGHT_NAME = "event:/SFX/Enemies/sfx_hazard_bladeStamped_prep_telegraph_right";

	// Token: 0x04002A46 RID: 10822
	private const string WAVE_ATTACK_SFX_LAUNCH_LEFT_NAME = "event:/SFX/Enemies/sfx_hazard_bladeStamped_launch_left";

	// Token: 0x04002A47 RID: 10823
	private const string WAVE_ATTACK_SFX_LAUNCH_RIGHT_NAME = "event:/SFX/Enemies/sfx_hazard_bladeStamped_launch_right";

	// Token: 0x04002A48 RID: 10824
	private BaseRoom m_currentRoom;

	// Token: 0x04002A49 RID: 10825
	private Action<object, EventArgs> m_onPlayerEnterRoom;

	// Token: 0x04002A4A RID: 10826
	private Action<object, EventArgs> m_onPlayerExitRoom;

	// Token: 0x04002A4B RID: 10827
	private bool m_actionsInitialized;

	// Token: 0x04002A4C RID: 10828
	private EventInstance m_prepLeftEventInstance;

	// Token: 0x04002A4D RID: 10829
	private EventInstance m_prepRightEventInstance;

	// Token: 0x04002A4E RID: 10830
	private EventInstance m_lockedLeftEventInstance;

	// Token: 0x04002A4F RID: 10831
	private EventInstance m_lockedRightEventInstance;

	// Token: 0x04002A50 RID: 10832
	private float m_waveAttackWarningDuration = 1.5f;

	// Token: 0x04002A51 RID: 10833
	private float m_waveAttackWarningLockedDuration = 0.35f;

	// Token: 0x04002A52 RID: 10834
	private Projectile_RL m_waveAttack_WarningProjectile;

	// Token: 0x04002A53 RID: 10835
	private Projectile_RL m_waveAttack_WarningLockedProjectile;

	// Token: 0x04002A54 RID: 10836
	private Projectile_RL m_waveAttack_Projectile;

	// Token: 0x04002A55 RID: 10837
	private Coroutine m_waveAttackCoroutine;
}
