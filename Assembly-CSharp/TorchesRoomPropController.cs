using System;
using System.Collections;
using Sigtrap.Relays;
using UnityEngine;

// Token: 0x0200082D RID: 2093
public class TorchesRoomPropController : BaseSpecialPropController, IBodyOnEnterHitResponse, IHitResponse, IBodyOnStayHitResponse
{
	// Token: 0x17001752 RID: 5970
	// (get) Token: 0x06004095 RID: 16533 RVA: 0x00023AB4 File Offset: 0x00021CB4
	public IRelayLink<TorchesRoomPropController> OnHitRelay
	{
		get
		{
			return this.m_onHitRelay.link;
		}
	}

	// Token: 0x17001753 RID: 5971
	// (get) Token: 0x06004096 RID: 16534 RVA: 0x00023AC1 File Offset: 0x00021CC1
	public IRelayLink<bool> FlameStateChangeRelay
	{
		get
		{
			return this.m_flameStateChangeRelay.link;
		}
	}

	// Token: 0x17001754 RID: 5972
	// (get) Token: 0x06004097 RID: 16535 RVA: 0x00023ACE File Offset: 0x00021CCE
	// (set) Token: 0x06004098 RID: 16536 RVA: 0x00023AD6 File Offset: 0x00021CD6
	public bool IsFlameOn { get; private set; }

	// Token: 0x17001755 RID: 5973
	// (get) Token: 0x06004099 RID: 16537 RVA: 0x00023ADF File Offset: 0x00021CDF
	// (set) Token: 0x0600409A RID: 16538 RVA: 0x00023AE7 File Offset: 0x00021CE7
	public bool KeepFlameOn { get; set; }

	// Token: 0x0600409B RID: 16539 RVA: 0x00103624 File Offset: 0x00101824
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

	// Token: 0x0600409C RID: 16540 RVA: 0x0010367C File Offset: 0x0010187C
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

	// Token: 0x0600409D RID: 16541 RVA: 0x001036E4 File Offset: 0x001018E4
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

	// Token: 0x0600409E RID: 16542 RVA: 0x00023AF0 File Offset: 0x00021CF0
	public void BodyOnStayHitResponse(IHitboxController otherHBController)
	{
		this.BodyOnEnterHitResponse(otherHBController);
	}

	// Token: 0x0600409F RID: 16543 RVA: 0x00023AF9 File Offset: 0x00021CF9
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

	// Token: 0x060040A0 RID: 16544 RVA: 0x00023B2D File Offset: 0x00021D2D
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

	// Token: 0x04003287 RID: 12935
	[SerializeField]
	private GameObject m_shaPingGO;

	// Token: 0x04003288 RID: 12936
	private const float TORCH_FLAME_ON_DURATION = 1.15f;

	// Token: 0x04003289 RID: 12937
	private Relay<TorchesRoomPropController> m_onHitRelay = new Relay<TorchesRoomPropController>();

	// Token: 0x0400328A RID: 12938
	private Relay<bool> m_flameStateChangeRelay = new Relay<bool>();
}
