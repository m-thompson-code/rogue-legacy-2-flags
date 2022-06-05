using System;
using System.Collections;
using RLAudio;
using UnityEngine;

// Token: 0x020005F9 RID: 1529
[CreateAssetMenu(menuName = "Custom/Biome Rules/Tower Biome Up")]
public class TowerBiomeUp_BiomeRule : BiomeRule
{
	// Token: 0x1700137F RID: 4991
	// (get) Token: 0x0600371D RID: 14109 RVA: 0x000BD0FF File Offset: 0x000BB2FF
	public override BiomeRuleExecutionTime ExecutionTime
	{
		get
		{
			return BiomeRuleExecutionTime.PlayerEnterBiome;
		}
	}

	// Token: 0x0600371E RID: 14110 RVA: 0x000BD104 File Offset: 0x000BB304
	private void Initialize()
	{
		this.m_lancerWarningProjectiles = new Projectile_RL[this.m_lancerAttackRandomAmount.y];
		this.m_lancerProjectiles = new Projectile_RL[this.m_lancerAttackRandomAmount.y];
		this.m_fireProjectileCoroutines = new Coroutine[this.m_lancerAttackRandomAmount.y];
		this.m_onPlayerEnterRoom = new Action<object, EventArgs>(this.OnPlayerEnterRoom);
		this.m_onPlayerExitRoom = new Action<object, EventArgs>(this.OnPlayerExitRoom);
		this.m_isInitialized = true;
	}

	// Token: 0x0600371F RID: 14111 RVA: 0x000BD187 File Offset: 0x000BB387
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

	// Token: 0x06003720 RID: 14112 RVA: 0x000BD196 File Offset: 0x000BB396
	public override void UndoRule(BiomeType biome)
	{
		if (this.m_isInitialized)
		{
			Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
			Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerExitRoom, this.m_onPlayerExitRoom);
		}
	}

	// Token: 0x06003721 RID: 14113 RVA: 0x000BD1BC File Offset: 0x000BB3BC
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

	// Token: 0x06003722 RID: 14114 RVA: 0x000BD296 File Offset: 0x000BB496
	private void OnPlayerExitRoom(object sender, EventArgs args)
	{
		if (this.m_isInitialized)
		{
			this.StopLancerAttackCoroutine();
		}
	}

	// Token: 0x17001380 RID: 4992
	// (get) Token: 0x06003723 RID: 14115 RVA: 0x000BD2A6 File Offset: 0x000BB4A6
	private Vector2 m_lancerAttack_InitialDelay
	{
		get
		{
			return Hazard_EV.NIGHTMARE_LANCER_INITIAL_DELAY;
		}
	}

	// Token: 0x17001381 RID: 4993
	// (get) Token: 0x06003724 RID: 14116 RVA: 0x000BD2AD File Offset: 0x000BB4AD
	private Vector2 m_lancerAttackRandomDelay
	{
		get
		{
			return Hazard_EV.NIGHTMARE_LANCER_RANDOM_DELAY;
		}
	}

	// Token: 0x17001382 RID: 4994
	// (get) Token: 0x06003725 RID: 14117 RVA: 0x000BD2B4 File Offset: 0x000BB4B4
	private Vector2Int m_lancerAttackRandomAmount
	{
		get
		{
			return Hazard_EV.NIGHTMARE_LANCER_RANDOM_AMOUNT;
		}
	}

	// Token: 0x17001383 RID: 4995
	// (get) Token: 0x06003726 RID: 14118 RVA: 0x000BD2BB File Offset: 0x000BB4BB
	private Vector2 m_lancerAttack_RandomSpawnInterval
	{
		get
		{
			return Hazard_EV.NIGHTMARE_LANCER_RANDOM_SPAWN_INTERVAL;
		}
	}

	// Token: 0x17001384 RID: 4996
	// (get) Token: 0x06003727 RID: 14119 RVA: 0x000BD2C2 File Offset: 0x000BB4C2
	private Vector2 m_lancerAttack_RandomPosXOffset
	{
		get
		{
			return Hazard_EV.NIGHTMARE_LANCER_RANDOM_X_OFFSET;
		}
	}

	// Token: 0x17001385 RID: 4997
	// (get) Token: 0x06003728 RID: 14120 RVA: 0x000BD2C9 File Offset: 0x000BB4C9
	private Vector2 m_lancerAttack_RandomPosYOffset
	{
		get
		{
			return Hazard_EV.NIGHTMARE_LANCER_RANDOM_Y_OFFSET;
		}
	}

	// Token: 0x06003729 RID: 14121 RVA: 0x000BD2D0 File Offset: 0x000BB4D0
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

	// Token: 0x0600372A RID: 14122 RVA: 0x000BD2DF File Offset: 0x000BB4DF
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

	// Token: 0x0600372B RID: 14123 RVA: 0x000BD2F8 File Offset: 0x000BB4F8
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

	// Token: 0x0600372C RID: 14124 RVA: 0x000BD39C File Offset: 0x000BB59C
	protected void StopProjectile(ref Projectile_RL projectile)
	{
		if (projectile && projectile.isActiveAndEnabled)
		{
			projectile.FlagForDestruction(null);
		}
		projectile = null;
	}

	// Token: 0x04002A70 RID: 10864
	private const string TOWER_WARNING_PROJECTILE = "TowerBiomeUpWarningProjectile";

	// Token: 0x04002A71 RID: 10865
	private const string TOWER_EXPLOSION_PROJECTILE = "TowerBiomeUpExplosionProjectile";

	// Token: 0x04002A72 RID: 10866
	private const string TOWER_LANCER_PROJECTILE = "TowerBiomeUpLancerProjectile";

	// Token: 0x04002A73 RID: 10867
	private BaseRoom m_currentRoom;

	// Token: 0x04002A74 RID: 10868
	private Projectile_RL[] m_lancerWarningProjectiles;

	// Token: 0x04002A75 RID: 10869
	private Projectile_RL[] m_lancerProjectiles;

	// Token: 0x04002A76 RID: 10870
	private Coroutine m_lancerAttackCoroutine;

	// Token: 0x04002A77 RID: 10871
	private Coroutine[] m_fireProjectileCoroutines;

	// Token: 0x04002A78 RID: 10872
	private Action<object, EventArgs> m_onPlayerEnterRoom;

	// Token: 0x04002A79 RID: 10873
	private Action<object, EventArgs> m_onPlayerExitRoom;

	// Token: 0x04002A7A RID: 10874
	private bool m_isInitialized;

	// Token: 0x04002A7B RID: 10875
	private float m_lancerAttackWarningDuration = 1.75f;
}
