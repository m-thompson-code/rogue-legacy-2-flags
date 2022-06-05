using System;
using System.Collections;
using UnityEngine;

// Token: 0x020005F8 RID: 1528
[CreateAssetMenu(menuName = "Custom/Biome Rules/Study Biome Up")]
public class StudyBiomeUp_BiomeRule : BiomeRule
{
	// Token: 0x1700137C RID: 4988
	// (get) Token: 0x06003710 RID: 14096 RVA: 0x000BCF6A File Offset: 0x000BB16A
	public override BiomeRuleExecutionTime ExecutionTime
	{
		get
		{
			return BiomeRuleExecutionTime.PlayerEnterBiome;
		}
	}

	// Token: 0x06003711 RID: 14097 RVA: 0x000BCF6E File Offset: 0x000BB16E
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

	// Token: 0x06003712 RID: 14098 RVA: 0x000BCF7D File Offset: 0x000BB17D
	public override void UndoRule(BiomeType biome)
	{
		if (this.m_actionsInitialized)
		{
			Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
			Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerExitRoom, this.m_onPlayerExitRoom);
		}
	}

	// Token: 0x06003713 RID: 14099 RVA: 0x000BCFA0 File Offset: 0x000BB1A0
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

	// Token: 0x06003714 RID: 14100 RVA: 0x000BD028 File Offset: 0x000BB228
	private void OnPlayerExitRoom(object sender, EventArgs args)
	{
		this.StopVoidAttack();
		this.m_voidAttackCoroutine = null;
	}

	// Token: 0x1700137D RID: 4989
	// (get) Token: 0x06003715 RID: 14101 RVA: 0x000BD037 File Offset: 0x000BB237
	private Vector2 m_voidAttack_Initial_Delay
	{
		get
		{
			return Hazard_EV.VOID_WAVE_INITIAL_DELAY;
		}
	}

	// Token: 0x1700137E RID: 4990
	// (get) Token: 0x06003716 RID: 14102 RVA: 0x000BD03E File Offset: 0x000BB23E
	private Vector2 m_voidAttackRandomDelay
	{
		get
		{
			return Hazard_EV.VOID_WAVE_RANDOM_DELAY;
		}
	}

	// Token: 0x06003717 RID: 14103 RVA: 0x000BD045 File Offset: 0x000BB245
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

	// Token: 0x06003718 RID: 14104 RVA: 0x000BD054 File Offset: 0x000BB254
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

	// Token: 0x06003719 RID: 14105 RVA: 0x000BD063 File Offset: 0x000BB263
	private void StopVoidAttack()
	{
		this.StopProjectile(ref this.m_voidAttack_Projectile);
		this.StopProjectile(ref this.m_voidAttack_WarningProjectile);
		if (this.m_voidAttackCoroutine != null && this.m_currentRoom)
		{
			this.m_currentRoom.StopCoroutine(this.m_voidAttackCoroutine);
		}
	}

	// Token: 0x0600371A RID: 14106 RVA: 0x000BD0A3 File Offset: 0x000BB2A3
	protected void StopProjectile(ref Projectile_RL projectile)
	{
		if (projectile && projectile.isActiveAndEnabled)
		{
			projectile.FlagForDestruction(null);
		}
		projectile = null;
	}

	// Token: 0x04002A63 RID: 10851
	private const string VOID_ATTACK_WARNING_PROJECTILE = "StudyBiomeUpVoidWarningProjectile";

	// Token: 0x04002A64 RID: 10852
	private const string VOID_ATTACK_PROJECTILE = "StudyBiomeUpVoidProjectile";

	// Token: 0x04002A65 RID: 10853
	private BaseRoom m_currentRoom;

	// Token: 0x04002A66 RID: 10854
	private Action<object, EventArgs> m_onPlayerEnterRoom;

	// Token: 0x04002A67 RID: 10855
	private Action<object, EventArgs> m_onPlayerExitRoom;

	// Token: 0x04002A68 RID: 10856
	private bool m_actionsInitialized;

	// Token: 0x04002A69 RID: 10857
	public static Vector2 VOID_WAVE_INITIAL_DELAY = new Vector2(1.5f, 3.5f);

	// Token: 0x04002A6A RID: 10858
	public static Vector2 VOID_WAVE_RANDOM_DELAY = new Vector2(10f, 12f);

	// Token: 0x04002A6B RID: 10859
	public const float VOID_WAVE_WARNING_DURATION = 1.5f;

	// Token: 0x04002A6C RID: 10860
	private float m_voidAttackWarningDuration = 2.5f;

	// Token: 0x04002A6D RID: 10861
	private Projectile_RL m_voidAttack_WarningProjectile;

	// Token: 0x04002A6E RID: 10862
	private Projectile_RL m_voidAttack_Projectile;

	// Token: 0x04002A6F RID: 10863
	private Coroutine m_voidAttackCoroutine;
}
