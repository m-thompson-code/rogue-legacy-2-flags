using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000562 RID: 1378
public class AngryOnHit_Trait : BaseTrait
{
	// Token: 0x170011BD RID: 4541
	// (get) Token: 0x06002C0F RID: 11279 RVA: 0x000187B8 File Offset: 0x000169B8
	public float StrengthMultiplier
	{
		get
		{
			if (this.m_isAngry)
			{
				return 0.25f;
			}
			return 0f;
		}
	}

	// Token: 0x170011BE RID: 4542
	// (get) Token: 0x06002C10 RID: 11280 RVA: 0x00017BA1 File Offset: 0x00015DA1
	public override TraitType TraitType
	{
		get
		{
			return TraitType.AngryOnHit;
		}
	}

	// Token: 0x06002C11 RID: 11281 RVA: 0x000187CD File Offset: 0x000169CD
	protected override void Awake()
	{
		base.Awake();
		this.m_onPlayerHealthChange = new Action<MonoBehaviour, EventArgs>(this.OnPlayerHealthChange);
		this.m_onPlayerEnterRoom = new Action<MonoBehaviour, EventArgs>(this.OnPlayerEnterRoom);
	}

	// Token: 0x06002C12 RID: 11282 RVA: 0x000187F9 File Offset: 0x000169F9
	private IEnumerator Start()
	{
		while (!PlayerManager.IsInstantiated)
		{
			yield return null;
		}
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerHealthChange, this.m_onPlayerHealthChange);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
		this.m_waitYield = new WaitRL_Yield(0f, false);
		this.m_effect = CameraController.ForegroundPerspCam.GetComponent<AngryOnHit_Effect>();
		this.m_isAngry = false;
		this.m_gameStarted = false;
		yield break;
	}

	// Token: 0x06002C13 RID: 11283 RVA: 0x000C5118 File Offset: 0x000C3318
	private void OnPlayerHealthChange(MonoBehaviour sender, EventArgs args)
	{
		if (!this.m_gameStarted)
		{
			return;
		}
		HealthChangeEventArgs healthChangeEventArgs = args as HealthChangeEventArgs;
		if (healthChangeEventArgs.PrevHealthValue > healthChangeEventArgs.NewHealthValue)
		{
			if (this.m_angryCoroutine != null)
			{
				base.StopCoroutine(this.m_angryCoroutine);
			}
			this.m_angryCoroutine = base.StartCoroutine(this.AngryCoroutine());
		}
	}

	// Token: 0x06002C14 RID: 11284 RVA: 0x00018808 File Offset: 0x00016A08
	private void OnPlayerEnterRoom(MonoBehaviour sender, EventArgs args)
	{
		this.m_gameStarted = true;
	}

	// Token: 0x06002C15 RID: 11285 RVA: 0x00018811 File Offset: 0x00016A11
	private IEnumerator AngryCoroutine()
	{
		this.m_waitYield.CreateNew(7.5f, false);
		this.StartAnger();
		yield return this.m_waitYield;
		this.StopAnger();
		yield break;
	}

	// Token: 0x06002C16 RID: 11286 RVA: 0x000C516C File Offset: 0x000C336C
	private void StartAnger()
	{
		if (!this.m_isAngry)
		{
			this.m_isAngry = true;
			PlayerController playerController = PlayerManager.GetPlayerController();
			playerController.MovementSpeedMod += 0.5f;
			playerController.InitializeStrengthMods();
			this.m_effect.enabled = true;
			this.m_effect.Amount = 0.75f;
		}
	}

	// Token: 0x06002C17 RID: 11287 RVA: 0x000C51C0 File Offset: 0x000C33C0
	private void StopAnger()
	{
		if (this.m_isAngry)
		{
			this.m_isAngry = false;
			PlayerController playerController = PlayerManager.GetPlayerController();
			playerController.MovementSpeedMod -= 0.5f;
			playerController.InitializeStrengthMods();
			TweenManager.TweenTo(this.m_effect, 0.5f, new EaseDelegate(Ease.None), new object[]
			{
				"Amount",
				0
			});
		}
	}

	// Token: 0x06002C18 RID: 11288 RVA: 0x00018820 File Offset: 0x00016A20
	private void OnDestroy()
	{
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerHealthChange, this.m_onPlayerHealthChange);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
		if (!PlayerManager.IsDisposed)
		{
			this.StopAnger();
		}
	}

	// Token: 0x04002543 RID: 9539
	private bool m_isAngry;

	// Token: 0x04002544 RID: 9540
	private Coroutine m_angryCoroutine;

	// Token: 0x04002545 RID: 9541
	private WaitRL_Yield m_waitYield;

	// Token: 0x04002546 RID: 9542
	private bool m_gameStarted;

	// Token: 0x04002547 RID: 9543
	private AngryOnHit_Effect m_effect;

	// Token: 0x04002548 RID: 9544
	private Action<MonoBehaviour, EventArgs> m_onPlayerHealthChange;

	// Token: 0x04002549 RID: 9545
	private Action<MonoBehaviour, EventArgs> m_onPlayerEnterRoom;
}
