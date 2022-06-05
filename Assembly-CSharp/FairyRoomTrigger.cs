using System;
using UnityEngine;

// Token: 0x02000436 RID: 1078
public class FairyRoomTrigger : BaseSpecialPropController
{
	// Token: 0x060027A1 RID: 10145 RVA: 0x00083E77 File Offset: 0x00082077
	protected override void Awake()
	{
		base.Awake();
		this.m_onStateChange = new Action<MonoBehaviour, EventArgs>(this.OnStateChange);
	}

	// Token: 0x060027A2 RID: 10146 RVA: 0x00083E91 File Offset: 0x00082091
	private void OnEnable()
	{
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.FairyRoomStateChange, this.m_onStateChange);
	}

	// Token: 0x060027A3 RID: 10147 RVA: 0x00083EA0 File Offset: 0x000820A0
	protected override void OnDisable()
	{
		base.OnDisable();
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.FairyRoomStateChange, this.m_onStateChange);
	}

	// Token: 0x060027A4 RID: 10148 RVA: 0x00083EB5 File Offset: 0x000820B5
	public override void SetRoom(BaseRoom room)
	{
		base.SetRoom(room);
		this.m_fairyRoomController = base.Room.gameObject.GetComponent<FairyRoomController>();
	}

	// Token: 0x060027A5 RID: 10149 RVA: 0x00083ED4 File Offset: 0x000820D4
	protected override void InitializePooledPropOnEnter()
	{
		this.InitializeTrigger();
		if (base.IsPropComplete)
		{
			this.DisableProp(false);
		}
	}

	// Token: 0x060027A6 RID: 10150 RVA: 0x00083EEC File Offset: 0x000820EC
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

	// Token: 0x060027A7 RID: 10151 RVA: 0x00083F90 File Offset: 0x00082190
	private void OnStateChange(object sender, EventArgs args)
	{
		FairyRoomEnteredEventArgs fairyRoomEnteredEventArgs = args as FairyRoomEnteredEventArgs;
		this.SetTriggerState(fairyRoomEnteredEventArgs.FairyRoomController.State);
	}

	// Token: 0x060027A8 RID: 10152 RVA: 0x00083FB8 File Offset: 0x000821B8
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

	// Token: 0x060027A9 RID: 10153 RVA: 0x00084085 File Offset: 0x00082285
	public void ActivateTrigger()
	{
		if (this.m_fairyRoomController)
		{
			this.m_fairyRoomController.StartFairyRoom();
		}
	}

	// Token: 0x0400211D RID: 8477
	public const string OFF_COLOR = "#000000";

	// Token: 0x0400211E RID: 8478
	public const string ON_COLOR = "#E7BF51";

	// Token: 0x0400211F RID: 8479
	public const string PASSED_COLOR = "#3ACF00";

	// Token: 0x04002120 RID: 8480
	public const string FAILED_COLOR = "#CA000B";

	// Token: 0x04002121 RID: 8481
	[SerializeField]
	private SpriteRenderer m_bottomSprite;

	// Token: 0x04002122 RID: 8482
	[SerializeField]
	private SpriteRenderer m_lightSprite;

	// Token: 0x04002123 RID: 8483
	private FairyRoomController m_fairyRoomController;

	// Token: 0x04002124 RID: 8484
	private Action<MonoBehaviour, EventArgs> m_onStateChange;
}
