using System;
using UnityEngine;

// Token: 0x020005FA RID: 1530
public class FadeOutHUDCollider : MonoBehaviour
{
	// Token: 0x17001291 RID: 4753
	// (get) Token: 0x06002F20 RID: 12064 RVA: 0x00019CE7 File Offset: 0x00017EE7
	public Collider2D Collider
	{
		get
		{
			return this.m_collider;
		}
	}

	// Token: 0x06002F21 RID: 12065 RVA: 0x000C96E0 File Offset: 0x000C78E0
	private void Awake()
	{
		RectTransform rectTransform = base.transform.parent.transform as RectTransform;
		this.m_screenToWorldScale = 0.016666668f * rectTransform.localScale.x;
		Vector3 vector = CameraController.GameCamera.ScreenToWorldPoint(base.transform.position) - CameraController.GameCamera.transform.position;
		base.transform.SetParent(CameraController.GameCamera.transform, true);
		base.transform.localPosition = vector;
		base.transform.localScale = new Vector3(this.m_screenToWorldScale, this.m_screenToWorldScale, 1f);
		this.m_storedPos = vector;
		this.m_onPlayerEnterRoom = new Action<MonoBehaviour, EventArgs>(this.OnPlayerEnterRoom);
		this.m_onPlayerEnterFairyRoom = new Action<MonoBehaviour, EventArgs>(this.OnPlayerEnterFairyRoom);
		this.m_onPlayerFairyRoomTriggered = new Action<MonoBehaviour, EventArgs>(this.OnPlayerFairyRoomTriggered);
		this.m_onPlayerExitFairyRoom = new Action<MonoBehaviour, EventArgs>(this.OnPlayerExitFairyRoom);
		this.m_onAspectRatioChanged = new Action<MonoBehaviour, EventArgs>(this.OnAspectRatioChanged);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerEnterFairyRoom, this.m_onPlayerEnterFairyRoom);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerFairyRoomTriggered, this.m_onPlayerFairyRoomTriggered);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerExitFairyRoom, this.m_onPlayerExitFairyRoom);
		Messenger<SceneMessenger, SceneEvent>.AddListener(SceneEvent.AspectRatioChanged, this.m_onAspectRatioChanged);
		if (this.m_isFairyHUD)
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x06002F22 RID: 12066 RVA: 0x000C983C File Offset: 0x000C7A3C
	private void OnDestroy()
	{
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerEnterFairyRoom, this.m_onPlayerEnterFairyRoom);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerFairyRoomTriggered, this.m_onPlayerFairyRoomTriggered);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerExitFairyRoom, this.m_onPlayerExitFairyRoom);
		Messenger<SceneMessenger, SceneEvent>.RemoveListener(SceneEvent.AspectRatioChanged, this.m_onAspectRatioChanged);
	}

	// Token: 0x06002F23 RID: 12067 RVA: 0x000C9888 File Offset: 0x000C7A88
	private void OnPlayerEnterRoom(object sender, EventArgs args)
	{
		float zoomLevel = CameraController.ZoomLevel;
		float num = this.m_screenToWorldScale * zoomLevel;
		base.transform.localScale = new Vector3(num, num, 1f);
		Vector3 storedPos = this.m_storedPos;
		if (!AspectRatioManager.Disable_16_9_Aspect)
		{
			float num2 = 1.7777778f / AspectRatioManager.CurrentScreenAspectRatio;
			storedPos.x *= num2;
		}
		Vector3 localPosition = new Vector3(storedPos.x * zoomLevel, storedPos.y * zoomLevel, storedPos.z);
		if (TraitManager.IsTraitActive(TraitType.UpsideDown))
		{
			localPosition.y *= -1f;
		}
		base.transform.localPosition = localPosition;
	}

	// Token: 0x06002F24 RID: 12068 RVA: 0x00019CEF File Offset: 0x00017EEF
	private void OnPlayerEnterFairyRoom(object sender, EventArgs args)
	{
		this.m_inDeactivatedFairyRoom = true;
		if (this.m_isFairyHUD && !base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(true);
		}
	}

	// Token: 0x06002F25 RID: 12069 RVA: 0x000C9928 File Offset: 0x000C7B28
	private void OnPlayerFairyRoomTriggered(object sender, EventArgs args)
	{
		FairyRoomEnteredEventArgs fairyRoomEnteredEventArgs = args as FairyRoomEnteredEventArgs;
		if (fairyRoomEnteredEventArgs != null && !fairyRoomEnteredEventArgs.FairyRoomController.IsRoomComplete)
		{
			this.m_inDeactivatedFairyRoom = false;
		}
	}

	// Token: 0x06002F26 RID: 12070 RVA: 0x00019D19 File Offset: 0x00017F19
	private void OnPlayerExitFairyRoom(object sender, EventArgs args)
	{
		this.m_inDeactivatedFairyRoom = false;
		if (this.m_isFairyHUD && base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x06002F27 RID: 12071 RVA: 0x00019D43 File Offset: 0x00017F43
	private void OnAspectRatioChanged(MonoBehaviour sender, EventArgs args)
	{
		this.OnPlayerEnterRoom(null, null);
	}

	// Token: 0x06002F28 RID: 12072 RVA: 0x00019D4D File Offset: 0x00017F4D
	public void SetCanvasGroup(CanvasGroup canvasGroup)
	{
		this.m_canvasGroup = canvasGroup;
	}

	// Token: 0x06002F29 RID: 12073 RVA: 0x00019D56 File Offset: 0x00017F56
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (this.IsValidCollider(collision))
		{
			this.m_collisionDetected = true;
		}
	}

	// Token: 0x06002F2A RID: 12074 RVA: 0x00019D56 File Offset: 0x00017F56
	private void OnTriggerStay2D(Collider2D collision)
	{
		if (this.IsValidCollider(collision))
		{
			this.m_collisionDetected = true;
		}
	}

	// Token: 0x06002F2B RID: 12075 RVA: 0x000C9954 File Offset: 0x000C7B54
	private bool IsValidCollider(Collider2D collider)
	{
		return !SaveManager.ConfigData.DisableHUDFadeOut && (collider.CompareTag("EnemyProjectile") || (collider.CompareTag("Enemy") && !this.m_inDeactivatedFairyRoom) || collider.CompareTag("Player") || collider.CompareTag("Player_Dodging") || collider.CompareTag("Hazard") || collider.CompareTag("TriggerHazard") || collider.CompareTag("Chest") || collider.CompareTag("NPC"));
	}

	// Token: 0x06002F2C RID: 12076 RVA: 0x000C99F0 File Offset: 0x000C7BF0
	private void FixedUpdate()
	{
		if (!this.m_canvasGroup)
		{
			return;
		}
		if (this.m_isMinimapHUD && TraitManager.IsTraitActive(TraitType.MapReveal))
		{
			return;
		}
		if (this.m_canvasGroup.alpha == 0f)
		{
			return;
		}
		if (this.m_isFaded && !this.m_canvasGroup.isActiveAndEnabled)
		{
			this.m_isFaded = false;
			this.m_canvasGroup.alpha = this.m_fadeInAmount;
		}
		if (this.m_collisionDetected)
		{
			if (!this.m_isFaded)
			{
				this.m_canvasGroup.alpha = this.m_fadeOutAmount;
				this.m_isFaded = true;
			}
		}
		else if (this.m_isFaded)
		{
			this.m_isFaded = false;
			this.m_canvasGroup.alpha = this.m_fadeInAmount;
		}
		this.m_collisionDetected = false;
	}

	// Token: 0x0400268D RID: 9869
	[SerializeField]
	private Collider2D m_collider;

	// Token: 0x0400268E RID: 9870
	[SerializeField]
	private float m_fadeInAmount;

	// Token: 0x0400268F RID: 9871
	[SerializeField]
	private float m_fadeOutAmount;

	// Token: 0x04002690 RID: 9872
	[SerializeField]
	private bool m_isMinimapHUD;

	// Token: 0x04002691 RID: 9873
	[SerializeField]
	private bool m_isFairyHUD;

	// Token: 0x04002692 RID: 9874
	private float m_screenToWorldScale;

	// Token: 0x04002693 RID: 9875
	private CanvasGroup m_canvasGroup;

	// Token: 0x04002694 RID: 9876
	private bool m_isFaded;

	// Token: 0x04002695 RID: 9877
	private bool m_collisionDetected;

	// Token: 0x04002696 RID: 9878
	private Vector3 m_storedPos;

	// Token: 0x04002697 RID: 9879
	private bool m_inDeactivatedFairyRoom;

	// Token: 0x04002698 RID: 9880
	private Action<MonoBehaviour, EventArgs> m_onPlayerEnterRoom;

	// Token: 0x04002699 RID: 9881
	private Action<MonoBehaviour, EventArgs> m_onPlayerEnterFairyRoom;

	// Token: 0x0400269A RID: 9882
	private Action<MonoBehaviour, EventArgs> m_onPlayerFairyRoomTriggered;

	// Token: 0x0400269B RID: 9883
	private Action<MonoBehaviour, EventArgs> m_onPlayerExitFairyRoom;

	// Token: 0x0400269C RID: 9884
	private Action<MonoBehaviour, EventArgs> m_onAspectRatioChanged;
}
