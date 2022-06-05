using System;
using System.Collections;
using RLAudio;
using UnityEngine;

// Token: 0x02000A13 RID: 2579
[CreateAssetMenu(menuName = "Custom/Biome Rules/Tower Biome Up")]
public class TowerBiomeUp_BiomeRule : BiomeRule
{
	// Token: 0x17001ACC RID: 6860
	// (get) Token: 0x06004D94 RID: 19860 RVA: 0x000046FA File Offset: 0x000028FA
	public override BiomeRuleExecutionTime ExecutionTime
	{
		get
		{
			return BiomeRuleExecutionTime.PlayerEnterBiome;
		}
	}

	// Token: 0x06004D95 RID: 19861 RVA: 0x0012BCE0 File Offset: 0x00129EE0
	private void Initialize()
	{
		this.m_lancerWarningProjectiles = new Projectile_RL[this.m_lancerAttackRandomAmount.y];
		this.m_lancerProjectiles = new Projectile_RL[this.m_lancerAttackRandomAmount.y];
		this.m_fireProjectileCoroutines = new Coroutine[this.m_lancerAttackRandomAmount.y];
		this.m_onPlayerEnterRoom = new Action<object, EventArgs>(this.OnPlayerEnterRoom);
		this.m_onPlayerExitRoom = new Action<object, EventArgs>(this.OnPlayerExitRoom);
		this.m_isInitialized = true;
	}

	// Token: 0x06004D96 RID: 19862 RVA: 0x0002A203 File Offset: 0x00028403
	public override IEnumerator RunRule(BiomeType biome)
	{
		if (BurdenManager.IsBurdenActive(BurdenType.TowerBiomeUp))
		{
			if (!this.m_isInitialized)
			{
				this.Initialize();
			}
			ProjectileManager.Instance.AddProjectileToPool("TowerBiomeUpWarningProjectile");
			ProjectileManager.Instance.AddProjectileToPool("TowerBiomeUpExplosionProjectile");
			ProjectileManager.Instance.AddProjectileToPool("TowerBiomeUpLancerProjectile");
			Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
			Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerExitRoom, this.m_onPlayerExitRoom);
		}
		yield break;
	}

	// Token: 0x06004D97 RID: 19863 RVA: 0x0002A212 File Offset: 0x00028412
	public override void UndoRule(BiomeType biome)
	{
		if (this.m_isInitialized)
		{
			Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
			Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerExitRoom, this.m_onPlayerExitRoom);
		}
	}

	// Token: 0x06004D98 RID: 19864 RVA: 0x0012BD64 File Offset: 0x00129F64
	private void OnPlayerEnterRoom(object sender, EventArgs args)
	{
		RoomViaDoorEventArgs roomViaDoorEventArgs = args as RoomViaDoorEventArgs;
		if (roomViaDoorEventArgs != null)
		{
			BaseRoom room = roomViaDoorEventArgs.Room;
			Room room2 = room as Room;
			MergeRoom mergeRoom = room as MergeRoom;
			bool flag = false;
			if (mergeRoom)
			{
				GridPointManager[] standaloneGridPointManagers = mergeRoom.StandaloneGridPointManagers;
				for (int i = 0; i < standaloneGridPointManagers.Length; i++)
				{
					if (standaloneGridPointManagers[i].RoomMetaData.SpecialRoomType == SpecialRoomType.BossEntrance)
					{
						flag = true;
						break;
					}
				}
			}
			if ((room.RoomType == RoomType.Standard || room.RoomType == RoomType.Trap) && room.AppearanceBiomeType == BiomeType.TowerExterior && !flag && ((room2 && !room2.GridPointManager.IsTunnelDestination) || (mergeRoom && !mergeRoom.StandaloneGridPointManagers[0].IsTunnelDestination)))
			{
				this.m_currentRoom = room;
				this.m_lancerAttackCoroutine = this.m_currentRoom.StartCoroutine(this.Lancer_Attack_Coroutine());
			}
		}
	}

	// Token: 0x06004D99 RID: 19865 RVA: 0x0002A235 File Offset: 0x00028435
	private void OnPlayerExitRoom(object sender, EventArgs args)
	{
		if (this.m_isInitialized)
		{
			this.StopLancerAttackCoroutine();
		}
	}

	// Token: 0x17001ACD RID: 6861
	// (get) Token: 0x06004D9A RID: 19866 RVA: 0x0002A245 File Offset: 0x00028445
	private Vector2 m_lancerAttack_InitialDelay
	{
		get
		{
			return Hazard_EV.NIGHTMARE_LANCER_INITIAL_DELAY;
		}
	}

	// Token: 0x17001ACE RID: 6862
	// (get) Token: 0x06004D9B RID: 19867 RVA: 0x0002A24C File Offset: 0x0002844C
	private Vector2 m_lancerAttackRandomDelay
	{
		get
		{
			return Hazard_EV.NIGHTMARE_LANCER_RANDOM_DELAY;
		}
	}

	// Token: 0x17001ACF RID: 6863
	// (get) Token: 0x06004D9C RID: 19868 RVA: 0x0002A253 File Offset: 0x00028453
	private Vector2Int m_lancerAttackRandomAmount
	{
		get
		{
			return Hazard_EV.NIGHTMARE_LANCER_RANDOM_AMOUNT;
		}
	}

	// Token: 0x17001AD0 RID: 6864
	// (get) Token: 0x06004D9D RID: 19869 RVA: 0x0002A25A File Offset: 0x0002845A
	private Vector2 m_lancerAttack_RandomSpawnInterval
	{
		get
		{
			return Hazard_EV.NIGHTMARE_LANCER_RANDOM_SPAWN_INTERVAL;
		}
	}

	// Token: 0x17001AD1 RID: 6865
	// (get) Token: 0x06004D9E RID: 19870 RVA: 0x0002A261 File Offset: 0x00028461
	private Vector2 m_lancerAttack_RandomPosXOffset
	{
		get
		{
			return Hazard_EV.NIGHTMARE_LANCER_RANDOM_X_OFFSET;
		}
	}

	// Token: 0x17001AD2 RID: 6866
	// (get) Token: 0x06004D9F RID: 19871 RVA: 0x0002A268 File Offset: 0x00028468
	private Vector2 m_lancerAttack_RandomPosYOffset
	{
		get
		{
			return Hazard_EV.NIGHTMARE_LANCER_RANDOM_Y_OFFSET;
		}
	}

	// Token: 0x06004DA0 RID: 19872 RVA: 0x0002A26F File Offset: 0x0002846F
	public IEnumerator Lancer_Attack_Coroutine()
	{
		float delay = Time.time + UnityEngine.Random.Range(this.m_lancerAttack_InitialDelay.x, this.m_lancerAttack_InitialDelay.y);
		for (;;)
		{
			if (Time.time >= delay)
			{
				int randAmount = UnityEngine.Random.Range(this.m_lancerAttackRandomAmount.x, this.m_lancerAttackRandomAmount.y + 1);
				int num2;
				for (int i = 0; i < randAmount; i = num2 + 1)
				{
					this.m_fireProjectileCoroutines[i] = this.m_currentRoom.StartCoroutine(this.FireLancerProjectile(i));
					float num = UnityEngine.Random.Range(this.m_lancerAttack_RandomSpawnInterval.x, this.m_lancerAttack_RandomSpawnInterval.y);
					delay = num + Time.time;
					while (Time.time < delay)
					{
						yield return null;
					}
					num2 = i;
				}
				delay = Time.time + UnityEngine.Random.Range(this.m_lancerAttackRandomDelay.x, this.m_lancerAttackRandomDelay.y);
			}
			else
			{
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x06004DA1 RID: 19873 RVA: 0x0002A27E File Offset: 0x0002847E
	private IEnumerator FireLancerProjectile(int index)
	{
		PlayerController playerController = PlayerManager.GetPlayerController();
		float num = UnityEngine.Random.Range(this.m_lancerAttack_RandomPosXOffset.x, this.m_lancerAttack_RandomPosXOffset.y);
		float num2 = UnityEngine.Random.Range(this.m_lancerAttack_RandomPosYOffset.x, this.m_lancerAttack_RandomPosYOffset.y);
		if (CDGHelper.RandomPlusMinus() > 0)
		{
			num *= -1f;
		}
		if (CDGHelper.RandomPlusMinus() > 0)
		{
			num2 *= -1f;
		}
		Vector2 vector = CameraController.GameCamera.transform.position + new Vector2(num, num2);
		AudioManager.PlayOneShot(null, "event:/SFX/Enemies/sfx_hazard_shadowKnight_prep", vector);
		this.m_lancerWarningProjectiles[index] = ProjectileManager.FireProjectile(this.m_currentRoom.gameObject, "TowerBiomeUpWarningProjectile", vector, false, 0f, 1f, true, true, true, true);
		float delay = Time.time + this.m_lancerAttackWarningDuration;
		while (Time.time < delay)
		{
			yield return null;
		}
		Vector3 position = this.m_lancerWarningProjectiles[index].transform.position;
		this.StopProjectile(ref this.m_lancerWarningProjectiles[index]);
		ProjectileManager.FireProjectile(this.m_currentRoom.gameObject, "TowerBiomeUpExplosionProjectile", position, false, 0f, 1f, true, true, true, true);
		EffectManager.PlayEffect(this.m_currentRoom.gameObject, null, "CameraShakeVerySmall_Effect", position, 0.1f, EffectStopType.Gracefully, EffectTriggerDirection.None);
		float num3 = CDGHelper.VectorToAngle(playerController.Midpoint - position);
		this.m_lancerProjectiles[index] = ProjectileManager.FireProjectile(this.m_currentRoom.gameObject, "TowerBiomeUpLancerProjectile", position, true, num3, 1f, true, true, true, true);
		this.m_lancerProjectiles[index].Orientation = CDGHelper.ToRadians(num3);
		yield break;
	}

	// Token: 0x06004DA2 RID: 19874 RVA: 0x0012BE40 File Offset: 0x0012A040
	private void StopLancerAttackCoroutine()
	{
		for (int i = 0; i < this.m_lancerProjectiles.Length; i++)
		{
			this.StopProjectile(ref this.m_lancerProjectiles[i]);
			this.StopProjectile(ref this.m_lancerWarningProjectiles[i]);
			if (this.m_fireProjectileCoroutines[i] != null && this.m_currentRoom)
			{
				this.m_currentRoom.StopCoroutine(this.m_fireProjectileCoroutines[i]);
			}
			this.m_fireProjectileCoroutines[i] = null;
		}
		if (this.m_lancerAttackCoroutine != null && this.m_currentRoom)
		{
			this.m_currentRoom.StopCoroutine(this.m_lancerAttackCoroutine);
		}
		this.m_lancerAttackCoroutine = null;
	}

	// Token: 0x06004DA3 RID: 19875 RVA: 0x00029EAA File Offset: 0x000280AA
	protected void StopProjectile(ref Projectile_RL projectile)
	{
		if (projectile && projectile.isActiveAndEnabled)
		{
			projectile.FlagForDestruction(null);
		}
		projectile = null;
	}

	// Token: 0x04003AB3 RID: 15027
	private const string TOWER_WARNING_PROJECTILE = "TowerBiomeUpWarningProjectile";

	// Token: 0x04003AB4 RID: 15028
	private const string TOWER_EXPLOSION_PROJECTILE = "TowerBiomeUpExplosionProjectile";

	// Token: 0x04003AB5 RID: 15029
	private const string TOWER_LANCER_PROJECTILE = "TowerBiomeUpLancerProjectile";

	// Token: 0x04003AB6 RID: 15030
	private BaseRoom m_currentRoom;

	// Token: 0x04003AB7 RID: 15031
	private Projectile_RL[] m_lancerWarningProjectiles;

	// Token: 0x04003AB8 RID: 15032
	private Projectile_RL[] m_lancerProjectiles;

	// Token: 0x04003AB9 RID: 15033
	private Coroutine m_lancerAttackCoroutine;

	// Token: 0x04003ABA RID: 15034
	private Coroutine[] m_fireProjectileCoroutines;

	// Token: 0x04003ABB RID: 15035
	private Action<object, EventArgs> m_onPlayerEnterRoom;

	// Token: 0x04003ABC RID: 15036
	private Action<object, EventArgs> m_onPlayerExitRoom;

	// Token: 0x04003ABD RID: 15037
	private bool m_isInitialized;

	// Token: 0x04003ABE RID: 15038
	private float m_lancerAttackWarningDuration = 1.75f;
}
