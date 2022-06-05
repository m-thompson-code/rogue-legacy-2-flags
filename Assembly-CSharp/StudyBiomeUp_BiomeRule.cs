using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000A0F RID: 2575
[CreateAssetMenu(menuName = "Custom/Biome Rules/Study Biome Up")]
public class StudyBiomeUp_BiomeRule : BiomeRule
{
	// Token: 0x17001AC3 RID: 6851
	// (get) Token: 0x06004D75 RID: 19829 RVA: 0x000046FA File Offset: 0x000028FA
	public override BiomeRuleExecutionTime ExecutionTime
	{
		get
		{
			return BiomeRuleExecutionTime.PlayerEnterBiome;
		}
	}

	// Token: 0x06004D76 RID: 19830 RVA: 0x0002A0D4 File Offset: 0x000282D4
	public override IEnumerator RunRule(BiomeType biome)
	{
		if (!this.m_actionsInitialized)
		{
			this.m_onPlayerEnterRoom = new Action<object, EventArgs>(this.OnPlayerEnterRoom);
			this.m_onPlayerExitRoom = new Action<object, EventArgs>(this.OnPlayerExitRoom);
			this.m_actionsInitialized = true;
		}
		if (BurdenManager.IsBurdenActive(BurdenType.StudyBiomeUp))
		{
			ProjectileManager.Instance.AddProjectileToPool("StudyBiomeUpVoidWarningProjectile");
			ProjectileManager.Instance.AddProjectileToPool("StudyBiomeUpVoidProjectile");
			Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
			Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerExitRoom, this.m_onPlayerExitRoom);
		}
		yield break;
	}

	// Token: 0x06004D77 RID: 19831 RVA: 0x0002A0E3 File Offset: 0x000282E3
	public override void UndoRule(BiomeType biome)
	{
		if (this.m_actionsInitialized)
		{
			Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
			Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerExitRoom, this.m_onPlayerExitRoom);
		}
	}

	// Token: 0x06004D78 RID: 19832 RVA: 0x0012B8FC File Offset: 0x00129AFC
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
				this.m_voidAttackCoroutine = this.m_currentRoom.StartCoroutine(this.Void_Attack_Coroutine());
			}
		}
	}

	// Token: 0x06004D79 RID: 19833 RVA: 0x0002A106 File Offset: 0x00028306
	private void OnPlayerExitRoom(object sender, EventArgs args)
	{
		this.StopVoidAttack();
		this.m_voidAttackCoroutine = null;
	}

	// Token: 0x17001AC4 RID: 6852
	// (get) Token: 0x06004D7A RID: 19834 RVA: 0x0002A115 File Offset: 0x00028315
	private Vector2 m_voidAttack_Initial_Delay
	{
		get
		{
			return Hazard_EV.VOID_WAVE_INITIAL_DELAY;
		}
	}

	// Token: 0x17001AC5 RID: 6853
	// (get) Token: 0x06004D7B RID: 19835 RVA: 0x0002A11C File Offset: 0x0002831C
	private Vector2 m_voidAttackRandomDelay
	{
		get
		{
			return Hazard_EV.VOID_WAVE_RANDOM_DELAY;
		}
	}

	// Token: 0x06004D7C RID: 19836 RVA: 0x0002A123 File Offset: 0x00028323
	public IEnumerator Void_Attack_Coroutine()
	{
		float delay = Time.time + UnityEngine.Random.Range(this.m_voidAttack_Initial_Delay.x, this.m_voidAttack_Initial_Delay.y);
		for (;;)
		{
			if (Time.time >= delay)
			{
				yield return this.FireVoidProjectile();
				delay = Time.time + UnityEngine.Random.Range(this.m_voidAttackRandomDelay.x, this.m_voidAttackRandomDelay.y);
			}
			else
			{
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x06004D7D RID: 19837 RVA: 0x0002A132 File Offset: 0x00028332
	private IEnumerator FireVoidProjectile()
	{
		float startingAngle = 0f;
		Vector2 offset = CameraController.GameCamera.transform.position;
		bool startOnLeft = CDGHelper.RandomPlusMinus() > 0;
		if (startOnLeft)
		{
			startingAngle = 0f;
			offset.x = CameraController.GameCamera.transform.position.x - 16f - 1f;
		}
		else
		{
			startingAngle = 180f;
			offset.x = CameraController.GameCamera.transform.position.x + 16f + 1f;
		}
		this.m_voidAttack_WarningProjectile = ProjectileManager.FireProjectile(this.m_currentRoom.gameObject, "StudyBiomeUpVoidWarningProjectile", offset, false, 90f, 1f, true, true, true, true);
		this.m_voidAttack_WarningProjectile.transform.SetParent(CameraController.ForegroundOrthoCam.transform, true);
		float delay = Time.time + this.m_voidAttackWarningDuration;
		while (Time.time < delay)
		{
			yield return null;
		}
		Vector3 position = this.m_voidAttack_WarningProjectile.transform.position;
		this.StopProjectile(ref this.m_voidAttack_WarningProjectile);
		this.m_voidAttack_Projectile = ProjectileManager.FireProjectile(this.m_currentRoom.gameObject, "StudyBiomeUpVoidProjectile", position, false, startingAngle, 1f, true, true, true, true);
		Vector3 localScale = this.m_voidAttack_Projectile.transform.localScale;
		if (startOnLeft)
		{
			localScale.x = Mathf.Abs(localScale.x);
		}
		else
		{
			localScale.x = Mathf.Abs(localScale.x) * -1f;
		}
		this.m_voidAttack_Projectile.transform.localScale = localScale;
		yield break;
	}

	// Token: 0x06004D7E RID: 19838 RVA: 0x0002A141 File Offset: 0x00028341
	private void StopVoidAttack()
	{
		this.StopProjectile(ref this.m_voidAttack_Projectile);
		this.StopProjectile(ref this.m_voidAttack_WarningProjectile);
		if (this.m_voidAttackCoroutine != null && this.m_currentRoom)
		{
			this.m_currentRoom.StopCoroutine(this.m_voidAttackCoroutine);
		}
	}

	// Token: 0x06004D7F RID: 19839 RVA: 0x00029EAA File Offset: 0x000280AA
	protected void StopProjectile(ref Projectile_RL projectile)
	{
		if (projectile && projectile.isActiveAndEnabled)
		{
			projectile.FlagForDestruction(null);
		}
		projectile = null;
	}

	// Token: 0x04003A99 RID: 15001
	private const string VOID_ATTACK_WARNING_PROJECTILE = "StudyBiomeUpVoidWarningProjectile";

	// Token: 0x04003A9A RID: 15002
	private const string VOID_ATTACK_PROJECTILE = "StudyBiomeUpVoidProjectile";

	// Token: 0x04003A9B RID: 15003
	private BaseRoom m_currentRoom;

	// Token: 0x04003A9C RID: 15004
	private Action<object, EventArgs> m_onPlayerEnterRoom;

	// Token: 0x04003A9D RID: 15005
	private Action<object, EventArgs> m_onPlayerExitRoom;

	// Token: 0x04003A9E RID: 15006
	private bool m_actionsInitialized;

	// Token: 0x04003A9F RID: 15007
	public static Vector2 VOID_WAVE_INITIAL_DELAY = new Vector2(1.5f, 3.5f);

	// Token: 0x04003AA0 RID: 15008
	public static Vector2 VOID_WAVE_RANDOM_DELAY = new Vector2(10f, 12f);

	// Token: 0x04003AA1 RID: 15009
	public const float VOID_WAVE_WARNING_DURATION = 1.5f;

	// Token: 0x04003AA2 RID: 15010
	private float m_voidAttackWarningDuration = 2.5f;

	// Token: 0x04003AA3 RID: 15011
	private Projectile_RL m_voidAttack_WarningProjectile;

	// Token: 0x04003AA4 RID: 15012
	private Projectile_RL m_voidAttack_Projectile;

	// Token: 0x04003AA5 RID: 15013
	private Coroutine m_voidAttackCoroutine;
}
