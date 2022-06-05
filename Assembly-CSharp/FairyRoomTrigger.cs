using System;
using UnityEngine;

// Token: 0x020006FE RID: 1790
public class FairyRoomTrigger : BaseSpecialPropController
{
	// Token: 0x060036AD RID: 13997 RVA: 0x0001E10D File Offset: 0x0001C30D
	protected override void Awake()
	{
		base.Awake();
		this.m_onStateChange = new Action<MonoBehaviour, EventArgs>(this.OnStateChange);
	}

	// Token: 0x060036AE RID: 13998 RVA: 0x0001E127 File Offset: 0x0001C327
	private void OnEnable()
	{
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.FairyRoomStateChange, this.m_onStateChange);
	}

	// Token: 0x060036AF RID: 13999 RVA: 0x0001E136 File Offset: 0x0001C336
	protected override void OnDisable()
	{
		base.OnDisable();
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.FairyRoomStateChange, this.m_onStateChange);
	}

	// Token: 0x060036B0 RID: 14000 RVA: 0x0001E14B File Offset: 0x0001C34B
	public override void SetRoom(BaseRoom room)
	{
		base.SetRoom(room);
		this.m_fairyRoomController = base.Room.gameObject.GetComponent<FairyRoomController>();
	}

	// Token: 0x060036B1 RID: 14001 RVA: 0x0001E16A File Offset: 0x0001C36A
	protected override void InitializePooledPropOnEnter()
	{
		this.InitializeTrigger();
		if (base.IsPropComplete)
		{
			this.DisableProp(false);
		}
	}

	// Token: 0x060036B2 RID: 14002 RVA: 0x000E4A88 File Offset: 0x000E2C88
	public void InitializeTrigger()
	{
		PlayerController playerController = PlayerManager.GetPlayerController();
		LayerMask mask = playerController.ControllerCorgi.SavedPlatformMask;
		mask |= playerController.ControllerCorgi.OneWayPlatformMask;
		RaycastHit2D hit = Physics2D.Raycast(base.transform.position, Vector2.down, 16f, mask);
		if (hit)
		{
			Vector2 size = this.m_bottomSprite.size;
			size.y = hit.distance;
			this.m_bottomSprite.size = size;
		}
		this.SetTriggerState(this.m_fairyRoomController.State);
	}

	// Token: 0x060036B3 RID: 14003 RVA: 0x000E4B2C File Offset: 0x000E2D2C
	private void OnStateChange(object sender, EventArgs args)
	{
		FairyRoomEnteredEventArgs fairyRoomEnteredEventArgs = args as FairyRoomEnteredEventArgs;
		this.SetTriggerState(fairyRoomEnteredEventArgs.FairyRoomController.State);
	}

	// Token: 0x060036B4 RID: 14004 RVA: 0x000E4B54 File Offset: 0x000E2D54
	private void SetTriggerState(FairyRoomState state)
	{
		base.Animator.SetBool("On", false);
		Color color;
		if (state <= FairyRoomState.Running)
		{
			if (state != FairyRoomState.NotRunning)
			{
				if (state == FairyRoomState.Running)
				{
					ColorUtility.TryParseHtmlString("#E7BF51", out color);
					base.Animator.SetBool("On", true);
					if (!base.IsPropComplete)
					{
						this.DisableProp(true);
						goto IL_B4;
					}
					goto IL_B4;
				}
			}
		}
		else if (state != FairyRoomState.Passed)
		{
			if (state == FairyRoomState.Failed)
			{
				ColorUtility.TryParseHtmlString("#CA000B", out color);
				if (!base.IsPropComplete)
				{
					this.PropComplete();
					this.DisableProp(true);
					goto IL_B4;
				}
				goto IL_B4;
			}
		}
		else
		{
			ColorUtility.TryParseHtmlString("#3ACF00", out color);
			if (!base.IsPropComplete)
			{
				this.PropComplete();
				this.DisableProp(true);
				goto IL_B4;
			}
			goto IL_B4;
		}
		ColorUtility.TryParseHtmlString("#000000", out color);
		IL_B4:
		this.m_lightSprite.color = color;
	}

	// Token: 0x060036B5 RID: 14005 RVA: 0x0001E181 File Offset: 0x0001C381
	public void ActivateTrigger()
	{
		if (this.m_fairyRoomController)
		{
			this.m_fairyRoomController.StartFairyRoom();
		}
	}

	// Token: 0x04002C4A RID: 11338
	public const string OFF_COLOR = "#000000";

	// Token: 0x04002C4B RID: 11339
	public const string ON_COLOR = "#E7BF51";

	// Token: 0x04002C4C RID: 11340
	public const string PASSED_COLOR = "#3ACF00";

	// Token: 0x04002C4D RID: 11341
	public const string FAILED_COLOR = "#CA000B";

	// Token: 0x04002C4E RID: 11342
	[SerializeField]
	private SpriteRenderer m_bottomSprite;

	// Token: 0x04002C4F RID: 11343
	[SerializeField]
	private SpriteRenderer m_lightSprite;

	// Token: 0x04002C50 RID: 11344
	private FairyRoomController m_fairyRoomController;

	// Token: 0x04002C51 RID: 11345
	private Action<MonoBehaviour, EventArgs> m_onStateChange;
}
