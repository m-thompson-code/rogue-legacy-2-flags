using System;
using System.Collections;
using Rewired;
using UnityEngine;

// Token: 0x020007F7 RID: 2039
public class TriggerSwitchPlatform : SpecialPlatform
{
	// Token: 0x06003EC4 RID: 16068 RVA: 0x000FBAA4 File Offset: 0x000F9CA4
	private void OnEnable()
	{
		Player player = Rewired_RL.Player;
		if (ReInput.isReady && Rewired_RL.Player != null)
		{
			player.AddInputEventDelegate(this.m_onSwitch, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Interact");
		}
	}

	// Token: 0x06003EC5 RID: 16069 RVA: 0x000FBAD8 File Offset: 0x000F9CD8
	private void OnDisable()
	{
		Player player = Rewired_RL.Player;
		if (ReInput.isReady && Rewired_RL.Player != null)
		{
			player.RemoveInputEventDelegate(this.m_onSwitch, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Interact");
		}
	}

	// Token: 0x06003EC6 RID: 16070 RVA: 0x00022B0E File Offset: 0x00020D0E
	protected override void Awake()
	{
		this.m_onSwitch = new Action<InputActionEventData>(this.OnSwitch);
		base.Awake();
		this.m_animator = base.GetComponent<Animator>();
	}

	// Token: 0x06003EC7 RID: 16071 RVA: 0x00022B34 File Offset: 0x00020D34
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

	// Token: 0x06003EC8 RID: 16072 RVA: 0x00022B43 File Offset: 0x00020D43
	private void OnSwitch(InputActionEventData data)
	{
		this.m_platformVisible = !this.m_platformVisible;
		this.SetPlatformState();
	}

	// Token: 0x06003EC9 RID: 16073 RVA: 0x000FBB0C File Offset: 0x000F9D0C
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

	// Token: 0x06003ECA RID: 16074 RVA: 0x000FBB74 File Offset: 0x000F9D74
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

	// Token: 0x04003135 RID: 12597
	private bool m_platformVisible = true;

	// Token: 0x04003136 RID: 12598
	private BoxCollider2D m_platformCollider;

	// Token: 0x04003137 RID: 12599
	private int m_storedTerrainLayer;

	// Token: 0x04003138 RID: 12600
	private int m_storedPlatformLayer;

	// Token: 0x04003139 RID: 12601
	private bool m_isInitialized;

	// Token: 0x0400313A RID: 12602
	private Animator m_animator;

	// Token: 0x0400313B RID: 12603
	private Action<InputActionEventData> m_onSwitch;
}
