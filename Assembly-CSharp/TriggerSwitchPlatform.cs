using System;
using System.Collections;
using Rewired;
using UnityEngine;

// Token: 0x020004CD RID: 1229
public class TriggerSwitchPlatform : SpecialPlatform
{
	// Token: 0x06002DAC RID: 11692 RVA: 0x0009A2B4 File Offset: 0x000984B4
	private void OnEnable()
	{
		Player player = Rewired_RL.Player;
		if (ReInput.isReady && Rewired_RL.Player != null)
		{
			player.AddInputEventDelegate(this.m_onSwitch, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Interact");
		}
	}

	// Token: 0x06002DAD RID: 11693 RVA: 0x0009A2E8 File Offset: 0x000984E8
	private void OnDisable()
	{
		Player player = Rewired_RL.Player;
		if (ReInput.isReady && Rewired_RL.Player != null)
		{
			player.RemoveInputEventDelegate(this.m_onSwitch, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Interact");
		}
	}

	// Token: 0x06002DAE RID: 11694 RVA: 0x0009A31C File Offset: 0x0009851C
	protected override void Awake()
	{
		this.m_onSwitch = new Action<InputActionEventData>(this.OnSwitch);
		base.Awake();
		this.m_animator = base.GetComponent<Animator>();
	}

	// Token: 0x06002DAF RID: 11695 RVA: 0x0009A342 File Offset: 0x00098542
	protected override IEnumerator Start()
	{
		yield return base.Start();
		this.m_hbController.RepeatHitDuration = 0f;
		this.m_platformCollider = (this.m_hbController.GetCollider(HitboxType.Platform) as BoxCollider2D);
		if (base.Width > 0f)
		{
			SpriteRenderer componentInChildren = base.GetComponentInChildren<SpriteRenderer>();
			componentInChildren.size = new Vector2(base.Width, componentInChildren.size.y);
			this.m_platformCollider.size = new Vector2(base.Width, this.m_platformCollider.size.y);
		}
		this.m_storedPlatformLayer = this.m_platformCollider.gameObject.layer;
		this.m_isInitialized = true;
		this.SetPlatformState();
		yield break;
	}

	// Token: 0x06002DB0 RID: 11696 RVA: 0x0009A351 File Offset: 0x00098551
	private void OnSwitch(InputActionEventData data)
	{
		this.m_platformVisible = !this.m_platformVisible;
		this.SetPlatformState();
	}

	// Token: 0x06002DB1 RID: 11697 RVA: 0x0009A368 File Offset: 0x00098568
	private void SetPlatformState()
	{
		if (this.m_platformVisible)
		{
			this.m_animator.SetBool("Active", true);
			this.m_platformCollider.gameObject.layer = this.m_storedPlatformLayer;
			return;
		}
		this.m_animator.SetBool("Active", false);
		this.m_platformCollider.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
	}

	// Token: 0x06002DB2 RID: 11698 RVA: 0x0009A3D0 File Offset: 0x000985D0
	public override void SetState(StateID state)
	{
		if (state != StateID.One)
		{
			if (state != StateID.Two)
			{
				if (state == StateID.Random)
				{
					if (CDGHelper.RandomPlusMinus() > 0)
					{
						this.m_platformVisible = true;
					}
					else
					{
						this.m_platformVisible = false;
					}
				}
			}
			else
			{
				this.m_platformVisible = false;
			}
		}
		else
		{
			this.m_platformVisible = true;
		}
		if (this.m_isInitialized)
		{
			this.SetPlatformState();
		}
	}

	// Token: 0x04002490 RID: 9360
	private bool m_platformVisible = true;

	// Token: 0x04002491 RID: 9361
	private BoxCollider2D m_platformCollider;

	// Token: 0x04002492 RID: 9362
	private int m_storedTerrainLayer;

	// Token: 0x04002493 RID: 9363
	private int m_storedPlatformLayer;

	// Token: 0x04002494 RID: 9364
	private bool m_isInitialized;

	// Token: 0x04002495 RID: 9365
	private Animator m_animator;

	// Token: 0x04002496 RID: 9366
	private Action<InputActionEventData> m_onSwitch;
}
