using System;
using System.Collections;
using RLAudio;
using UnityEngine;

// Token: 0x020005ED RID: 1517
[CreateAssetMenu(menuName = "Custom/Biome Rules/Bridge Biome Up")]
public class BridgeBiomeUp_BiomeRule : BiomeRule, IAudioEventEmitter
{
	// Token: 0x1700136A RID: 4970
	// (get) Token: 0x060036CD RID: 14029 RVA: 0x000BC93B File Offset: 0x000BAB3B
	public override BiomeRuleExecutionTime ExecutionTime
	{
		get
		{
			return BiomeRuleExecutionTime.PlayerEnterBiome;
		}
	}

	// Token: 0x1700136B RID: 4971
	// (get) Token: 0x060036CE RID: 14030 RVA: 0x000BC93F File Offset: 0x000BAB3F
	public string Description
	{
		get
		{
			return "BridgeBiomeUp_BiomeRule";
		}
	}

	// Token: 0x060036CF RID: 14031 RVA: 0x000BC946 File Offset: 0x000BAB46
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

	// Token: 0x060036D0 RID: 14032 RVA: 0x000BC955 File Offset: 0x000BAB55
	public override void UndoRule(BiomeType biome)
	{
		if (this.m_actionsInitialized)
		{
			Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
			Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerExitRoom, this.m_onPlayerExitRoom);
		}
	}

	// Token: 0x060036D1 RID: 14033 RVA: 0x000BC978 File Offset: 0x000BAB78
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

	// Token: 0x060036D2 RID: 14034 RVA: 0x000BC9C1 File Offset: 0x000BABC1
	private void OnPlayerExitRoom(object sender, EventArgs args)
	{
		if (this.m_attackCoroutine != null && this.m_currentRoom)
		{
			this.m_currentRoom.StopCoroutine(this.m_attackCoroutine);
		}
		this.m_attackCoroutine = null;
	}

	// Token: 0x060036D3 RID: 14035 RVA: 0x000BC9F0 File Offset: 0x000BABF0
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

	// Token: 0x04002A33 RID: 10803
	private const string WARNING_PROJECTILE = "BridgeBiomeUpCannonBallWarningProjectile";

	// Token: 0x04002A34 RID: 10804
	private const string CANNONBALL_PROJECTILE = "BridgeBiomeUpCannonBallProjectile";

	// Token: 0x04002A35 RID: 10805
	private BaseRoom m_currentRoom;

	// Token: 0x04002A36 RID: 10806
	private Action<object, EventArgs> m_onPlayerEnterRoom;

	// Token: 0x04002A37 RID: 10807
	private Action<object, EventArgs> m_onPlayerExitRoom;

	// Token: 0x04002A38 RID: 10808
	private bool m_actionsInitialized;

	// Token: 0x04002A39 RID: 10809
	private Coroutine m_attackCoroutine;

	// Token: 0x04002A3A RID: 10810
	private WaitRL_Yield m_waitBetweenCannonballRows;
}
