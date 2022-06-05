using System;
using UnityEngine;

// Token: 0x02000376 RID: 886
public class FadeOutHUDCollider : MonoBehaviour
{
	// Token: 0x17000E0E RID: 3598
	// (get) Token: 0x06002139 RID: 8505 RVA: 0x00068724 File Offset: 0x00066924
	public Collider2D Collider
	{
		get
		{
			return this.m_collider;
		}
	}

	// Token: 0x0600213A RID: 8506 RVA: 0x0006872C File Offset: 0x0006692C
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

	// Token: 0x0600213B RID: 8507 RVA: 0x00068888 File Offset: 0x00066A88
	private void OnDestroy()
	{
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerEnterFairyRoom, this.m_onPlayerEnterFairyRoom);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerFairyRoomTriggered, this.m_onPlayerFairyRoomTriggered);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerExitFairyRoom, this.m_onPlayerExitFairyRoom);
		Messenger<SceneMessenger, SceneEvent>.RemoveListener(SceneEvent.AspectRatioChanged, this.m_onAspectRatioChanged);
	}

	// Token: 0x0600213C RID: 8508 RVA: 0x000688D4 File Offset: 0x00066AD4
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

	// Token: 0x0600213D RID: 8509 RVA: 0x00068974 File Offset: 0x00066B74
	private void OnPlayerEnterFairyRoom(object sender, EventArgs args)
	{
		this.m_inDeactivatedFairyRoom = true;
		if (this.m_isFairyHUD && !base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(true);
		}
	}

	// Token: 0x0600213E RID: 8510 RVA: 0x000689A0 File Offset: 0x00066BA0
	private void OnPlayerFairyRoomTriggered(object sender, EventArgs args)
	{
		FairyRoomEnteredEventArgs fairyRoomEnteredEventArgs = args as FairyRoomEnteredEventArgs;
		if (fairyRoomEnteredEventArgs != null && !fairyRoomEnteredEventArgs.FairyRoomController.IsRoomComplete)
		{
			this.m_inDeactivatedFairyRoom = false;
		}
	}

	// Token: 0x0600213F RID: 8511 RVA: 0x000689CB File Offset: 0x00066BCB
	private void OnPlayerExitFairyRoom(object sender, EventArgs args)
	{
		this.m_inDeactivatedFairyRoom = false;
		if (this.m_isFairyHUD && base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x06002140 RID: 8512 RVA: 0x000689F5 File Offset: 0x00066BF5
	private void OnAspectRatioChanged(MonoBehaviour sender, EventArgs args)
	{
		this.OnPlayerEnterRoom(null, null);
	}

	// Token: 0x06002141 RID: 8513 RVA: 0x000689FF File Offset: 0x00066BFF
	public void SetCanvasGroup(CanvasGroup canvasGroup)
	{
		this.m_canvasGroup = canvasGroup;
	}

	// Token: 0x06002142 RID: 8514 RVA: 0x00068A08 File Offset: 0x00066C08
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (this.IsValidCollider(collision))
		{
			this.m_collisionDetected = true;
		}
	}

	// Token: 0x06002143 RID: 8515 RVA: 0x00068A1A File Offset: 0x00066C1A
	private void OnTriggerStay2D(Collider2D collision)
	{
		if (this.IsValidCollider(collision))
		{
			this.m_collisionDetected = true;
		}
	}

	// Token: 0x06002144 RID: 8516 RVA: 0x00068A2C File Offset: 0x00066C2C
	private bool IsValidCollider(Collider2D collider)
	{
		return !SaveManager.ConfigData.DisableHUDFadeOut && (collider.CompareTag("EnemyProjectile") || (collider.CompareTag("Enemy") && !this.m_inDeactivatedFairyRoom) || collider.CompareTag("Player") || collider.CompareTag("Player_Dodging") || collider.CompareTag("Hazard") || collider.CompareTag("TriggerHazard") || collider.CompareTag("Chest") || collider.CompareTag("NPC"));
	}

	// Token: 0x06002145 RID: 8517 RVA: 0x00068AC8 File Offset: 0x00066CC8
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

	// Token: 0x04001CC3 RID: 7363
	[SerializeField]
	private Collider2D m_collider;

	// Token: 0x04001CC4 RID: 7364
	[SerializeField]
	private float m_fadeInAmount;

	// Token: 0x04001CC5 RID: 7365
	[SerializeField]
	private float m_fadeOutAmount;

	// Token: 0x04001CC6 RID: 7366
	[SerializeField]
	private bool m_isMinimapHUD;

	// Token: 0x04001CC7 RID: 7367
	[SerializeField]
	private bool m_isFairyHUD;

	// Token: 0x04001CC8 RID: 7368
	private float m_screenToWorldScale;

	// Token: 0x04001CC9 RID: 7369
	private CanvasGroup m_canvasGroup;

	// Token: 0x04001CCA RID: 7370
	private bool m_isFaded;

	// Token: 0x04001CCB RID: 7371
	private bool m_collisionDetected;

	// Token: 0x04001CCC RID: 7372
	private Vector3 m_storedPos;

	// Token: 0x04001CCD RID: 7373
	private bool m_inDeactivatedFairyRoom;

	// Token: 0x04001CCE RID: 7374
	private Action<MonoBehaviour, EventArgs> m_onPlayerEnterRoom;

	// Token: 0x04001CCF RID: 7375
	private Action<MonoBehaviour, EventArgs> m_onPlayerEnterFairyRoom;

	// Token: 0x04001CD0 RID: 7376
	private Action<MonoBehaviour, EventArgs> m_onPlayerFairyRoomTriggered;

	// Token: 0x04001CD1 RID: 7377
	private Action<MonoBehaviour, EventArgs> m_onPlayerExitFairyRoom;

	// Token: 0x04001CD2 RID: 7378
	private Action<MonoBehaviour, EventArgs> m_onAspectRatioChanged;
}
