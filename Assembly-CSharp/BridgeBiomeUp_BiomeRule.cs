using System;
using System.Collections;
using RLAudio;
using UnityEngine;

// Token: 0x020009F5 RID: 2549
[CreateAssetMenu(menuName = "Custom/Biome Rules/Bridge Biome Up")]
public class BridgeBiomeUp_BiomeRule : BiomeRule, IAudioEventEmitter
{
	// Token: 0x17001A97 RID: 6807
	// (get) Token: 0x06004CDF RID: 19679 RVA: 0x000046FA File Offset: 0x000028FA
	public override BiomeRuleExecutionTime ExecutionTime
	{
		get
		{
			return BiomeRuleExecutionTime.PlayerEnterBiome;
		}
	}

	// Token: 0x17001A98 RID: 6808
	// (get) Token: 0x06004CE0 RID: 19680 RVA: 0x00029C69 File Offset: 0x00027E69
	public string Description
	{
		get
		{
			return "BridgeBiomeUp_BiomeRule";
		}
	}

	// Token: 0x06004CE1 RID: 19681 RVA: 0x00029C70 File Offset: 0x00027E70
	public override IEnumerator RunRule(BiomeType biome)
	{
		if (!this.m_actionsInitialized)
		{
			this.m_onPlayerEnterRoom = new Action<object, EventArgs>(this.OnPlayerEnterRoom);
			this.m_onPlayerExitRoom = new Action<object, EventArgs>(this.OnPlayerExitRoom);
			this.m_waitBetweenCannonballRows = new WaitRL_Yield(0.1f, false);
			this.m_actionsInitialized = true;
		}
		if (BurdenManager.IsBurdenActive(BurdenType.BridgeBiomeUp))
		{
			ProjectileManager.Instance.AddProjectileToPool("BridgeBiomeUpCannonBallWarningProjectile");
			ProjectileManager.Instance.AddProjectileToPool("BridgeBiomeUpCannonBallProjectile");
			Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
			Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerExitRoom, this.m_onPlayerExitRoom);
		}
		yield break;
	}

	// Token: 0x06004CE2 RID: 19682 RVA: 0x00029C7F File Offset: 0x00027E7F
	public override void UndoRule(BiomeType biome)
	{
		if (this.m_actionsInitialized)
		{
			Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
			Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerExitRoom, this.m_onPlayerExitRoom);
		}
	}

	// Token: 0x06004CE3 RID: 19683 RVA: 0x0012A7D8 File Offset: 0x001289D8
	private void OnPlayerEnterRoom(object sender, EventArgs args)
	{
		RoomViaDoorEventArgs roomViaDoorEventArgs = args as RoomViaDoorEventArgs;
		if (roomViaDoorEventArgs != null)
		{
			MergeRoom mergeRoom = roomViaDoorEventArgs.Room as MergeRoom;
			if (mergeRoom)
			{
				this.m_currentRoom = mergeRoom;
				this.m_attackCoroutine = this.m_currentRoom.StartCoroutine(this.Attack_Coroutine());
			}
		}
	}

	// Token: 0x06004CE4 RID: 19684 RVA: 0x00029CA2 File Offset: 0x00027EA2
	private void OnPlayerExitRoom(object sender, EventArgs args)
	{
		if (this.m_attackCoroutine != null && this.m_currentRoom)
		{
			this.m_currentRoom.StopCoroutine(this.m_attackCoroutine);
		}
		this.m_attackCoroutine = null;
	}

	// Token: 0x06004CE5 RID: 19685 RVA: 0x00029CD1 File Offset: 0x00027ED1
	public IEnumerator Attack_Coroutine()
	{
		float num = 5f;
		for (;;)
		{
			float waitTime = Time.time + num;
			while (Time.time < waitTime)
			{
				yield return null;
			}
			float y = PlayerManager.GetCurrentPlayerRoom().Bounds.max.y + 10f;
			float num2 = 5f;
			Vector2 offset5;
			Vector2 offset4;
			Vector2 offset3;
			Vector2 offset2;
			Vector2 offset6 = offset2 = (offset3 = (offset4 = (offset5 = new Vector2(PlayerManager.GetPlayerController().Midpoint.x, y))));
			offset6.y = y;
			offset2.x += num2;
			offset3.x += -num2;
			offset4.x += num2 * 2f;
			offset5.x += -num2 * 2f;
			AudioManager.PlayOneShot(this, "event:/SFX/Enemies/sfx_enemy_skeletonBoss_portal_multiple", PlayerManager.GetPlayerController().Midpoint);
			ProjectileManager.FireProjectile(this.m_currentRoom.gameObject, "BridgeBiomeUpCannonBallWarningProjectile", offset6, false, 270f, 1f, true, true, true, true);
			ProjectileManager.FireProjectile(this.m_currentRoom.gameObject, "BridgeBiomeUpCannonBallWarningProjectile", offset2, false, 270f, 1f, true, true, true, true);
			ProjectileManager.FireProjectile(this.m_currentRoom.gameObject, "BridgeBiomeUpCannonBallWarningProjectile", offset3, false, 270f, 1f, true, true, true, true);
			ProjectileManager.FireProjectile(this.m_currentRoom.gameObject, "BridgeBiomeUpCannonBallWarningProjectile", offset4, false, 270f, 1f, true, true, true, true);
			ProjectileManager.FireProjectile(this.m_currentRoom.gameObject, "BridgeBiomeUpCannonBallWarningProjectile", offset5, false, 270f, 1f, true, true, true, true);
			ProjectileManager.FireProjectile(this.m_currentRoom.gameObject, "BridgeBiomeUpCannonBallProjectile", offset6, true, -90f, 1f, true, true, true, true);
			yield return this.m_waitBetweenCannonballRows;
			ProjectileManager.FireProjectile(this.m_currentRoom.gameObject, "BridgeBiomeUpCannonBallProjectile", offset2, true, -90f, 1f, true, true, true, true);
			Projectile_RL projectile_RL = ProjectileManager.FireProjectile(this.m_currentRoom.gameObject, "BridgeBiomeUpCannonBallProjectile", offset3, true, -90f, 1f, true, true, true, true);
			AudioManager.PlayOneShotAttached(this, "event:/SFX/Enemies/sfx_enemy_skeletonBoss_cannon_projectiles_loop", projectile_RL.gameObject);
			yield return this.m_waitBetweenCannonballRows;
			ProjectileManager.FireProjectile(this.m_currentRoom.gameObject, "BridgeBiomeUpCannonBallProjectile", offset4, true, -90f, 1f, true, true, true, true);
			ProjectileManager.FireProjectile(this.m_currentRoom.gameObject, "BridgeBiomeUpCannonBallProjectile", offset5, true, -90f, 1f, true, true, true, true);
			num = UnityEngine.Random.Range(8f, 13f);
			offset2 = default(Vector2);
			offset3 = default(Vector2);
			offset4 = default(Vector2);
			offset5 = default(Vector2);
		}
		yield break;
	}

	// Token: 0x04003A31 RID: 14897
	private const string WARNING_PROJECTILE = "BridgeBiomeUpCannonBallWarningProjectile";

	// Token: 0x04003A32 RID: 14898
	private const string CANNONBALL_PROJECTILE = "BridgeBiomeUpCannonBallProjectile";

	// Token: 0x04003A33 RID: 14899
	private BaseRoom m_currentRoom;

	// Token: 0x04003A34 RID: 14900
	private Action<object, EventArgs> m_onPlayerEnterRoom;

	// Token: 0x04003A35 RID: 14901
	private Action<object, EventArgs> m_onPlayerExitRoom;

	// Token: 0x04003A36 RID: 14902
	private bool m_actionsInitialized;

	// Token: 0x04003A37 RID: 14903
	private Coroutine m_attackCoroutine;

	// Token: 0x04003A38 RID: 14904
	private WaitRL_Yield m_waitBetweenCannonballRows;
}
