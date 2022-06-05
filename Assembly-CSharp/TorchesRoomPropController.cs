using System;
using System.Collections;
using Sigtrap.Relays;
using UnityEngine;

// Token: 0x020004E3 RID: 1251
public class TorchesRoomPropController : BaseSpecialPropController, IBodyOnEnterHitResponse, IHitResponse, IBodyOnStayHitResponse
{
	// Token: 0x1700118D RID: 4493
	// (get) Token: 0x06002ECF RID: 11983 RVA: 0x0009F8D5 File Offset: 0x0009DAD5
	public IRelayLink<TorchesRoomPropController> OnHitRelay
	{
		get
		{
			return this.m_onHitRelay.link;
		}
	}

	// Token: 0x1700118E RID: 4494
	// (get) Token: 0x06002ED0 RID: 11984 RVA: 0x0009F8E2 File Offset: 0x0009DAE2
	public IRelayLink<bool> FlameStateChangeRelay
	{
		get
		{
			return this.m_flameStateChangeRelay.link;
		}
	}

	// Token: 0x1700118F RID: 4495
	// (get) Token: 0x06002ED1 RID: 11985 RVA: 0x0009F8EF File Offset: 0x0009DAEF
	// (set) Token: 0x06002ED2 RID: 11986 RVA: 0x0009F8F7 File Offset: 0x0009DAF7
	public bool IsFlameOn { get; private set; }

	// Token: 0x17001190 RID: 4496
	// (get) Token: 0x06002ED3 RID: 11987 RVA: 0x0009F900 File Offset: 0x0009DB00
	// (set) Token: 0x06002ED4 RID: 11988 RVA: 0x0009F908 File Offset: 0x0009DB08
	public bool KeepFlameOn { get; set; }

	// Token: 0x06002ED5 RID: 11989 RVA: 0x0009F914 File Offset: 0x0009DB14
	protected override void InitializePooledPropOnEnter()
	{
		base.StopAllCoroutines();
		this.IsFlameOn = false;
		this.KeepFlameOn = false;
		base.Animator.SetBool("FlameOn", false);
		if (SaveManager.PlayerSaveData.GetInsightState(InsightType.CastleBoss_DoorOpened) >= InsightState.ResolvedButNotViewed)
		{
			this.m_shaPingGO.SetActive(false);
			this.ForceFlameOn();
		}
	}

	// Token: 0x06002ED6 RID: 11990 RVA: 0x0009F96C File Offset: 0x0009DB6C
	public void ForceFlameOn()
	{
		base.Animator.SetBool("FlameOn", true);
		this.IsFlameOn = true;
		this.KeepFlameOn = true;
		base.Animator.Play("FlameOn");
		base.Animator.Update(10f);
		base.Animator.Update(10f);
		this.m_flameStateChangeRelay.Dispatch(true);
	}

	// Token: 0x06002ED7 RID: 11991 RVA: 0x0009F9D4 File Offset: 0x0009DBD4
	public void BodyOnEnterHitResponse(IHitboxController otherHBController)
	{
		if (this.KeepFlameOn)
		{
			return;
		}
		if (otherHBController.CollisionType == CollisionType.PlayerProjectile && otherHBController.RootGameObject.GetComponent<DownstrikeProjectile_RL>())
		{
			base.StopAllCoroutines();
			base.Animator.SetBool("FlameOn", true);
			this.IsFlameOn = true;
			base.StartCoroutine(this.StartFlameOnCoroutine());
			this.m_onHitRelay.Dispatch(this);
			this.m_flameStateChangeRelay.Dispatch(true);
		}
	}

	// Token: 0x06002ED8 RID: 11992 RVA: 0x0009FA48 File Offset: 0x0009DC48
	public void BodyOnStayHitResponse(IHitboxController otherHBController)
	{
		this.BodyOnEnterHitResponse(otherHBController);
	}

	// Token: 0x06002ED9 RID: 11993 RVA: 0x0009FA51 File Offset: 0x0009DC51
	private void OnGroundTouch()
	{
		if (!this.KeepFlameOn)
		{
			base.StopAllCoroutines();
			base.Animator.SetBool("FlameOn", false);
			this.IsFlameOn = false;
			this.m_flameStateChangeRelay.Dispatch(false);
		}
	}

	// Token: 0x06002EDA RID: 11994 RVA: 0x0009FA85 File Offset: 0x0009DC85
	private IEnumerator StartFlameOnCoroutine()
	{
		float startTime = Time.time;
		while (Time.time < startTime + 1.15f)
		{
			if (PlayerManager.GetPlayerController().IsGrounded)
			{
				this.OnGroundTouch();
				yield break;
			}
			yield return null;
		}
		if (!this.KeepFlameOn)
		{
			base.Animator.SetBool("FlameOn", false);
			this.IsFlameOn = false;
			this.m_flameStateChangeRelay.Dispatch(false);
		}
		yield break;
	}

	// Token: 0x0400254D RID: 9549
	[SerializeField]
	private GameObject m_shaPingGO;

	// Token: 0x0400254E RID: 9550
	private const float TORCH_FLAME_ON_DURATION = 1.15f;

	// Token: 0x0400254F RID: 9551
	private Relay<TorchesRoomPropController> m_onHitRelay = new Relay<TorchesRoomPropController>();

	// Token: 0x04002550 RID: 9552
	private Relay<bool> m_flameStateChangeRelay = new Relay<bool>();
}
