using System;
using System.Collections;
using FMOD.Studio;
using RLAudio;
using UnityEngine;

// Token: 0x02000A00 RID: 2560
[CreateAssetMenu(menuName = "Custom/Biome Rules/Cave Biome Up")]
public class CaveBiomeUp_BiomeRule : BiomeRule
{
	// Token: 0x17001AA7 RID: 6823
	// (get) Token: 0x06004D1A RID: 19738 RVA: 0x000046FA File Offset: 0x000028FA
	public override BiomeRuleExecutionTime ExecutionTime
	{
		get
		{
			return BiomeRuleExecutionTime.PlayerEnterBiome;
		}
	}

	// Token: 0x06004D1B RID: 19739 RVA: 0x00029E4A File Offset: 0x0002804A
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

	// Token: 0x06004D1C RID: 19740 RVA: 0x0012AF1C File Offset: 0x0012911C
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

	// Token: 0x06004D1D RID: 19741 RVA: 0x00029E59 File Offset: 0x00028059
	protected override void OnDisable()
	{
		base.OnDisable();
		if (!GameManager.IsApplicationClosing)
		{
			this.OnPlayerExitRoom(null, null);
		}
	}

	// Token: 0x06004D1E RID: 19742 RVA: 0x0012AFB0 File Offset: 0x001291B0
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

	// Token: 0x06004D1F RID: 19743 RVA: 0x0012B038 File Offset: 0x00129238
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

	// Token: 0x17001AA8 RID: 6824
	// (get) Token: 0x06004D20 RID: 19744 RVA: 0x00029E70 File Offset: 0x00028070
	private Vector2 m_waveAttack_Initial_Delay
	{
		get
		{
			return Hazard_EV.FLYING_WEAPON_WAVE_INITIAL_DELAY;
		}
	}

	// Token: 0x17001AA9 RID: 6825
	// (get) Token: 0x06004D21 RID: 19745 RVA: 0x00029E77 File Offset: 0x00028077
	private Vector2 m_waveAttackRandomDelay
	{
		get
		{
			return Hazard_EV.FLYING_WEAPON_WAVE_RANDOM_DELAY;
		}
	}

	// Token: 0x17001AAA RID: 6826
	// (get) Token: 0x06004D22 RID: 19746 RVA: 0x00029E7E File Offset: 0x0002807E
	private Vector2 m_waveAttackRandomPosOffset
	{
		get
		{
			return Hazard_EV.FLYING_WEAPON_WAVE_RANDOM_POS_OFFSET;
		}
	}

	// Token: 0x17001AAB RID: 6827
	// (get) Token: 0x06004D23 RID: 19747 RVA: 0x00029E85 File Offset: 0x00028085
	private Vector2 m_waveAttackRandomAngle
	{
		get
		{
			return Hazard_EV.FLYING_WEAPON_WAVE_RANDOM_ANGLE;
		}
	}

	// Token: 0x06004D24 RID: 19748 RVA: 0x00029E8C File Offset: 0x0002808C
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

	// Token: 0x06004D25 RID: 19749 RVA: 0x00029E9B File Offset: 0x0002809B
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

	// Token: 0x06004D26 RID: 19750 RVA: 0x0012B0B8 File Offset: 0x001292B8
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

	// Token: 0x06004D27 RID: 19751 RVA: 0x00029EAA File Offset: 0x000280AA
	protected void StopProjectile(ref Projectile_RL projectile)
	{
		if (projectile && projectile.isActiveAndEnabled)
		{
			projectile.FlagForDestruction(null);
		}
		projectile = null;
	}

	// Token: 0x04003A56 RID: 14934
	private const string WAVE_ATTACK_WARNING_PROJECTILE = "CaveBossWaveWarningProjectile";

	// Token: 0x04003A57 RID: 14935
	private const string WAVE_ATTACK_WARNING_LOCKED_PROJECTILE = "CaveBossWaveWarningLockedProjectile";

	// Token: 0x04003A58 RID: 14936
	private const string WAVE_ATTACK_PROJECTILE = "CaveBossWaveProjectile";

	// Token: 0x04003A59 RID: 14937
	private const string WAVE_ATTACK_SFX_PREP_LEFT_NAME = "event:/SFX/Enemies/sfx_hazard_bladeStamped_prep_start_loop_left";

	// Token: 0x04003A5A RID: 14938
	private const string WAVE_ATTACK_SFX_PREP_RIGHT_NAME = "event:/SFX/Enemies/sfx_hazard_bladeStamped_prep_start_loop_right";

	// Token: 0x04003A5B RID: 14939
	private const string WAVE_ATTACK_SFX_LOCKED_LEFT_NAME = "event:/SFX/Enemies/sfx_hazard_bladeStamped_prep_telegraph_left";

	// Token: 0x04003A5C RID: 14940
	private const string WAVE_ATTACK_SFX_LOCKED_RIGHT_NAME = "event:/SFX/Enemies/sfx_hazard_bladeStamped_prep_telegraph_right";

	// Token: 0x04003A5D RID: 14941
	private const string WAVE_ATTACK_SFX_LAUNCH_LEFT_NAME = "event:/SFX/Enemies/sfx_hazard_bladeStamped_launch_left";

	// Token: 0x04003A5E RID: 14942
	private const string WAVE_ATTACK_SFX_LAUNCH_RIGHT_NAME = "event:/SFX/Enemies/sfx_hazard_bladeStamped_launch_right";

	// Token: 0x04003A5F RID: 14943
	private BaseRoom m_currentRoom;

	// Token: 0x04003A60 RID: 14944
	private Action<object, EventArgs> m_onPlayerEnterRoom;

	// Token: 0x04003A61 RID: 14945
	private Action<object, EventArgs> m_onPlayerExitRoom;

	// Token: 0x04003A62 RID: 14946
	private bool m_actionsInitialized;

	// Token: 0x04003A63 RID: 14947
	private EventInstance m_prepLeftEventInstance;

	// Token: 0x04003A64 RID: 14948
	private EventInstance m_prepRightEventInstance;

	// Token: 0x04003A65 RID: 14949
	private EventInstance m_lockedLeftEventInstance;

	// Token: 0x04003A66 RID: 14950
	private EventInstance m_lockedRightEventInstance;

	// Token: 0x04003A67 RID: 14951
	private float m_waveAttackWarningDuration = 1.5f;

	// Token: 0x04003A68 RID: 14952
	private float m_waveAttackWarningLockedDuration = 0.35f;

	// Token: 0x04003A69 RID: 14953
	private Projectile_RL m_waveAttack_WarningProjectile;

	// Token: 0x04003A6A RID: 14954
	private Projectile_RL m_waveAttack_WarningLockedProjectile;

	// Token: 0x04003A6B RID: 14955
	private Projectile_RL m_waveAttack_Projectile;

	// Token: 0x04003A6C RID: 14956
	private Coroutine m_waveAttackCoroutine;
}
