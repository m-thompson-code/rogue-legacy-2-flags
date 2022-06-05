using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200031D RID: 797
public class AngryOnHit_Trait : BaseTrait
{
	// Token: 0x17000D9A RID: 3482
	// (get) Token: 0x06001F74 RID: 8052 RVA: 0x00064C83 File Offset: 0x00062E83
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

	// Token: 0x17000D9B RID: 3483
	// (get) Token: 0x06001F75 RID: 8053 RVA: 0x00064C98 File Offset: 0x00062E98
	public override TraitType TraitType
	{
		get
		{
			return TraitType.AngryOnHit;
		}
	}

	// Token: 0x06001F76 RID: 8054 RVA: 0x00064C9F File Offset: 0x00062E9F
	protected override void Awake()
	{
		base.Awake();
		this.m_onPlayerHealthChange = new Action<MonoBehaviour, EventArgs>(this.OnPlayerHealthChange);
		this.m_onPlayerEnterRoom = new Action<MonoBehaviour, EventArgs>(this.OnPlayerEnterRoom);
	}

	// Token: 0x06001F77 RID: 8055 RVA: 0x00064CCB File Offset: 0x00062ECB
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

	// Token: 0x06001F78 RID: 8056 RVA: 0x00064CDC File Offset: 0x00062EDC
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

	// Token: 0x06001F79 RID: 8057 RVA: 0x00064D2D File Offset: 0x00062F2D
	private void OnPlayerEnterRoom(MonoBehaviour sender, EventArgs args)
	{
		this.m_gameStarted = true;
	}

	// Token: 0x06001F7A RID: 8058 RVA: 0x00064D36 File Offset: 0x00062F36
	private IEnumerator AngryCoroutine()
	{
		this.m_waitYield.CreateNew(7.5f, false);
		this.StartAnger();
		yield return this.m_waitYield;
		this.StopAnger();
		yield break;
	}

	// Token: 0x06001F7B RID: 8059 RVA: 0x00064D48 File Offset: 0x00062F48
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

	// Token: 0x06001F7C RID: 8060 RVA: 0x00064D9C File Offset: 0x00062F9C
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

	// Token: 0x06001F7D RID: 8061 RVA: 0x00064E08 File Offset: 0x00063008
	private void OnDestroy()
	{
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerHealthChange, this.m_onPlayerHealthChange);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
		if (!PlayerManager.IsDisposed)
		{
			this.StopAnger();
		}
	}

	// Token: 0x04001C25 RID: 7205
	private bool m_isAngry;

	// Token: 0x04001C26 RID: 7206
	private Coroutine m_angryCoroutine;

	// Token: 0x04001C27 RID: 7207
	private WaitRL_Yield m_waitYield;

	// Token: 0x04001C28 RID: 7208
	private bool m_gameStarted;

	// Token: 0x04001C29 RID: 7209
	private AngryOnHit_Effect m_effect;

	// Token: 0x04001C2A RID: 7210
	private Action<MonoBehaviour, EventArgs> m_onPlayerHealthChange;

	// Token: 0x04001C2B RID: 7211
	private Action<MonoBehaviour, EventArgs> m_onPlayerEnterRoom;
}
