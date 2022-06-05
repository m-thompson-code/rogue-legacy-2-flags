using System;
using System.Collections;
using Rewired;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RL_Windows
{
	// Token: 0x02000DF0 RID: 3568
	public class MapWindowController : WindowController
	{
		// Token: 0x17002058 RID: 8280
		// (get) Token: 0x06006443 RID: 25667 RVA: 0x00004792 File Offset: 0x00002992
		public override WindowID ID
		{
			get
			{
				return WindowID.Map;
			}
		}

		// Token: 0x17002059 RID: 8281
		// (get) Token: 0x06006444 RID: 25668 RVA: 0x0002836B File Offset: 0x0002656B
		public Scene Scene
		{
			get
			{
				return base.gameObject.scene;
			}
		}

		// Token: 0x1700205A RID: 8282
		// (get) Token: 0x06006445 RID: 25669 RVA: 0x00037667 File Offset: 0x00035867
		// (set) Token: 0x06006446 RID: 25670 RVA: 0x0003766F File Offset: 0x0003586F
		private bool IsMapMade { get; set; }

		// Token: 0x06006447 RID: 25671 RVA: 0x00173ED0 File Offset: 0x001720D0
		private void Awake()
		{
			this.m_onCancelButtonDown = new Action<InputActionEventData>(this.OnCancelButtonDown);
			this.m_moveMapHorizontal = new Action<InputActionEventData>(this.MoveMapHorizontal);
			this.m_moveMapVertical = new Action<InputActionEventData>(this.MoveMapVertical);
			this.m_onYButtonDown = new Action<InputActionEventData>(this.OnYButtonDown);
			this.m_onConfirmButtonDown = new Action<InputActionEventData>(this.OnConfirmButtonDown);
			this.m_zoomMapVertical = new Action<InputActionEventData>(this.ZoomMapVertical);
		}

		// Token: 0x06006448 RID: 25672 RVA: 0x00173F50 File Offset: 0x00172150
		protected override void OnOpen()
		{
			this.m_windowCanvas.gameObject.SetActive(true);
			MapController.MapWindowCamera.orthographicSize = 9f;
			MapController.MapWindowCamera.aspect = CameraController.GameCamera.aspect;
			MapController.MapWindowCamera.rect = CameraController.GameCamera.rect;
			MapController.MapWindowCamera.gameObject.SetActive(true);
			Rect uvRect = this.m_uvGridImage.uvRect;
			uvRect = new Rect(0f, 0f, 8.170213f, 8.170213f);
			this.m_uvGridImage.uvRect = uvRect;
			MapController.UpdatePlayerIconPosition();
			if (!TraitManager.IsTraitActive(TraitType.MapReveal))
			{
				MapController.CentreCameraAroundPlayerIcon();
			}
			else
			{
				MapController.SetMapCameraPosition(MapController.GetBiomeRect(PlayerManager.GetCurrentPlayerRoom().BiomeType).center);
			}
			MapController.SetCameraFollowIsOn(false);
			base.StartCoroutine(this.RunEnterAnimation());
		}

		// Token: 0x06006449 RID: 25673 RVA: 0x00174034 File Offset: 0x00172234
		protected override void OnClose()
		{
			MapWindowController.EnteredFromOtherSubmenu = false;
			this.m_windowCanvas.gameObject.SetActive(false);
			MapController.SetCameraFollowIsOn(true);
			MapController.MapWindowCamera.orthographicSize = 2f;
			MapController.MapWindowCamera.gameObject.SetActive(false);
			this.UnsubscribeFromRewiredInputEvents();
		}

		// Token: 0x0600644A RID: 25674 RVA: 0x00037678 File Offset: 0x00035878
		protected IEnumerator RunEnterAnimation()
		{
			RewiredMapController.SetCurrentMapEnabled(false);
			float duration = 0.15f;
			if (!MapWindowController.EnteredFromOtherSubmenu)
			{
				this.m_fadeBGCanvasGroup.alpha = 0f;
				TweenManager.TweenTo_UnscaledTime(this.m_fadeBGCanvasGroup, duration, new EaseDelegate(Ease.Quad.EaseOut), new object[]
				{
					"alpha",
					0.66667f
				});
				this.m_shadowsCanvasGroup.transform.localScale = new Vector3(2f, 2f, 1f);
				this.m_shadowsCanvasGroup.alpha = 0f;
				TweenManager.TweenTo_UnscaledTime(this.m_shadowsCanvasGroup.transform, duration, new EaseDelegate(Ease.Quad.EaseOut), new object[]
				{
					"localScale.x",
					1,
					"localScale.y",
					1
				});
				TweenManager.TweenTo_UnscaledTime(this.m_shadowsCanvasGroup, duration, new EaseDelegate(Ease.Quad.EaseOut), new object[]
				{
					"alpha",
					1
				});
			}
			else
			{
				this.m_fadeBGCanvasGroup.alpha = 1f;
				this.m_shadowsCanvasGroup.alpha = 1f;
				this.m_shadowsCanvasGroup.transform.localScale = new Vector3(1f, 1f, 1f);
			}
			this.m_legendCanvasGroup.alpha = 0f;
			TweenManager.TweenTo_UnscaledTime(this.m_legendCanvasGroup, 0.15f, new EaseDelegate(Ease.Quad.EaseOut), new object[]
			{
				"alpha",
				1
			});
			this.m_gridAndRTCanvasGroup.transform.localScale = new Vector3(2f, 2f, 1f);
			this.m_gridAndRTCanvasGroup.alpha = 0f;
			TweenManager.TweenTo_UnscaledTime(this.m_gridAndRTCanvasGroup.transform, duration, new EaseDelegate(Ease.Quad.EaseOut), new object[]
			{
				"localScale.x",
				1,
				"localScale.y",
				1
			});
			yield return TweenManager.TweenTo_UnscaledTime(this.m_gridAndRTCanvasGroup, duration, new EaseDelegate(Ease.Quad.EaseOut), new object[]
			{
				"alpha",
				1
			}).TweenCoroutine;
			RewiredMapController.SetCurrentMapEnabled(true);
			yield break;
		}

		// Token: 0x0600644B RID: 25675 RVA: 0x00037687 File Offset: 0x00035887
		protected override void OnFocus()
		{
			this.SubscribeToRewiredInputEvents();
		}

		// Token: 0x0600644C RID: 25676 RVA: 0x0003768F File Offset: 0x0003588F
		protected override void OnLostFocus()
		{
			this.UnsubscribeFromRewiredInputEvents();
		}

		// Token: 0x0600644D RID: 25677 RVA: 0x00027E04 File Offset: 0x00026004
		protected override void OnPause()
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600644E RID: 25678 RVA: 0x00027E04 File Offset: 0x00026004
		protected override void OnUnpause()
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600644F RID: 25679 RVA: 0x00028116 File Offset: 0x00026316
		protected virtual void OnCancelButtonDown(InputActionEventData obj)
		{
			if (WindowManager.GetIsWindowOpen(WindowID.Pause))
			{
				WindowManager.CloseAllOpenWindows();
				return;
			}
			WindowManager.SetWindowIsOpen(this.ID, false);
		}

		// Token: 0x06006450 RID: 25680 RVA: 0x00037697 File Offset: 0x00035897
		protected virtual void OnConfirmButtonDown(InputActionEventData obj)
		{
			base.StartCoroutine(this.TweenCameraToPlayer(0.25f));
		}

		// Token: 0x06006451 RID: 25681 RVA: 0x000376AB File Offset: 0x000358AB
		protected virtual void OnYButtonDown(InputActionEventData obj)
		{
			this.ToggleLegend();
		}

		// Token: 0x06006452 RID: 25682 RVA: 0x000376B3 File Offset: 0x000358B3
		public override void Initialize()
		{
			base.Initialize();
		}

		// Token: 0x06006453 RID: 25683 RVA: 0x00174084 File Offset: 0x00172284
		protected virtual void SubscribeToRewiredInputEvents()
		{
			if (ReInput.isReady)
			{
				base.RewiredPlayer.AddInputEventDelegate(this.m_onCancelButtonDown, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Select");
				base.RewiredPlayer.AddInputEventDelegate(this.m_onCancelButtonDown, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
				base.RewiredPlayer.AddInputEventDelegate(this.m_moveMapHorizontal, UpdateLoopType.Update, InputActionEventType.AxisActive, "Window_Horizontal");
				base.RewiredPlayer.AddInputEventDelegate(this.m_moveMapVertical, UpdateLoopType.Update, InputActionEventType.AxisActive, "Window_Vertical");
				base.RewiredPlayer.AddInputEventDelegate(this.m_onYButtonDown, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_R");
				base.RewiredPlayer.AddInputEventDelegate(this.m_onConfirmButtonDown, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Confirm");
				base.RewiredPlayer.AddInputEventDelegate(this.m_zoomMapVertical, UpdateLoopType.Update, InputActionEventType.AxisActive, "Window_Vertical_RStick");
			}
		}

		// Token: 0x06006454 RID: 25684 RVA: 0x00174148 File Offset: 0x00172348
		protected virtual void UnsubscribeFromRewiredInputEvents()
		{
			if (ReInput.isReady)
			{
				base.RewiredPlayer.RemoveInputEventDelegate(this.m_onCancelButtonDown, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Select");
				base.RewiredPlayer.RemoveInputEventDelegate(this.m_onCancelButtonDown, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
				base.RewiredPlayer.RemoveInputEventDelegate(this.m_moveMapHorizontal, UpdateLoopType.Update, InputActionEventType.AxisActive, "Window_Horizontal");
				base.RewiredPlayer.RemoveInputEventDelegate(this.m_moveMapVertical, UpdateLoopType.Update, InputActionEventType.AxisActive, "Window_Vertical");
				base.RewiredPlayer.RemoveInputEventDelegate(this.m_onYButtonDown, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_R");
				base.RewiredPlayer.RemoveInputEventDelegate(this.m_onConfirmButtonDown, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Confirm");
				base.RewiredPlayer.RemoveInputEventDelegate(this.m_zoomMapVertical, UpdateLoopType.Update, InputActionEventType.AxisActive, "Window_Vertical_RStick");
			}
		}

		// Token: 0x06006455 RID: 25685 RVA: 0x000376BB File Offset: 0x000358BB
		private void ToggleLegend()
		{
			this.m_legend.SetActive(!this.m_legend.activeSelf);
		}

		// Token: 0x06006456 RID: 25686 RVA: 0x0017420C File Offset: 0x0017240C
		protected virtual void MoveMapHorizontal(InputActionEventData inputActionEventData)
		{
			float num = inputActionEventData.GetAxis();
			if (num == 0f)
			{
				num = -inputActionEventData.GetAxisPrev();
			}
			Vector3 position = MapController.Camera.transform.position;
			float num2 = MapController.MapWindowCamera.orthographicSize / 9f;
			float num3 = num * num2 * 14f * Time.unscaledDeltaTime;
			position.x += num3;
			Rect uvRect = this.m_uvGridImage.uvRect;
			uvRect.x += num3 / (33f / uvRect.width);
			MapController.SetMapCameraPosition(position);
		}

		// Token: 0x06006457 RID: 25687 RVA: 0x001742A0 File Offset: 0x001724A0
		protected virtual void MoveMapVertical(InputActionEventData inputActionEventData)
		{
			float num = inputActionEventData.GetAxis();
			if (num == 0f)
			{
				num = -inputActionEventData.GetAxisPrev();
			}
			Vector3 position = MapController.Camera.transform.position;
			float num2 = MapController.MapWindowCamera.orthographicSize / 9f;
			float num3 = num * num2 * 14f * Time.unscaledDeltaTime;
			position.y += num3;
			Rect uvRect = this.m_uvGridImage.uvRect;
			uvRect.y += num3 / (32f / uvRect.height);
			MapController.SetMapCameraPosition(position);
		}

		// Token: 0x06006458 RID: 25688 RVA: 0x00174334 File Offset: 0x00172534
		private void ZoomMapVertical(InputActionEventData eventData)
		{
			float num = 14f;
			if (eventData.IsCurrentInputSource(ControllerType.Mouse))
			{
				num *= 10f;
			}
			float num2 = -eventData.GetAxis();
			float num3 = Mathf.Clamp(MapController.MapWindowCamera.orthographicSize + num2 * num * Time.unscaledDeltaTime, 6f, 24f);
			float num4 = MapController.MapWindowCamera.orthographicSize - num3;
			MapController.MapWindowCamera.orthographicSize = num3;
			Rect uvRect = this.m_uvGridImage.uvRect;
			uvRect.width -= num4;
			uvRect.x += num4 / 2f;
			uvRect.height -= num4;
			uvRect.y += num4 / 2f;
			this.m_uvGridImage.uvRect = uvRect;
		}

		// Token: 0x06006459 RID: 25689 RVA: 0x000376D6 File Offset: 0x000358D6
		private IEnumerator TweenCameraToPlayer(float duration)
		{
			RewiredMapController.SetCurrentMapEnabled(false);
			Vector2 vector = MapController.PlayerIconPosition;
			if (TraitManager.IsTraitActive(TraitType.MapReveal))
			{
				vector = MapController.GetBiomeRect(PlayerManager.GetCurrentPlayerRoom().AppearanceBiomeType).center;
			}
			float num = vector.x - MapController.Camera.transform.position.x;
			num /= 5.4166665f;
			float num2 = vector.y - MapController.Camera.transform.position.y;
			num2 /= 5.4166665f;
			TweenManager.TweenBy_UnscaledTime(this.m_uvGridImage, duration, new EaseDelegate(Ease.Quad.EaseInOut), new object[]
			{
				"uvRect.x",
				num,
				"uvRect.y",
				num2
			});
			yield return TweenManager.TweenTo_UnscaledTime(MapController.Camera.transform, duration, new EaseDelegate(Ease.Quad.EaseInOut), new object[]
			{
				"position.x",
				vector.x,
				"position.y",
				vector.y
			}).TweenCoroutine;
			RewiredMapController.SetCurrentMapEnabled(true);
			yield break;
		}

		// Token: 0x040051C0 RID: 20928
		public static bool EnteredFromOtherSubmenu;

		// Token: 0x040051C1 RID: 20929
		[SerializeField]
		private Camera m_mapCamera;

		// Token: 0x040051C2 RID: 20930
		[SerializeField]
		private Ferr2DT_PathTerrain m_mapFerr2DObj;

		// Token: 0x040051C3 RID: 20931
		[SerializeField]
		private GameObject m_mapHorizontalDoor;

		// Token: 0x040051C4 RID: 20932
		[SerializeField]
		private GameObject m_mapVerticalDoor;

		// Token: 0x040051C5 RID: 20933
		[SerializeField]
		private GameObject m_mapEnemyIcon;

		// Token: 0x040051C6 RID: 20934
		[SerializeField]
		private GameObject m_legend;

		// Token: 0x040051C7 RID: 20935
		[Header("Animation Objects")]
		[SerializeField]
		private CanvasGroup m_shadowsCanvasGroup;

		// Token: 0x040051C8 RID: 20936
		[SerializeField]
		private CanvasGroup m_fadeBGCanvasGroup;

		// Token: 0x040051C9 RID: 20937
		[SerializeField]
		private CanvasGroup m_gridAndRTCanvasGroup;

		// Token: 0x040051CA RID: 20938
		[SerializeField]
		private CanvasGroup m_legendCanvasGroup;

		// Token: 0x040051CB RID: 20939
		[SerializeField]
		private RawImage m_uvGridImage;

		// Token: 0x040051CC RID: 20940
		[SerializeField]
		private GameObject m_yellowStarsGO;

		// Token: 0x040051CD RID: 20941
		private GameObject m_mapObj;

		// Token: 0x040051CE RID: 20942
		private Action<InputActionEventData> m_onCancelButtonDown;

		// Token: 0x040051CF RID: 20943
		private Action<InputActionEventData> m_moveMapHorizontal;

		// Token: 0x040051D0 RID: 20944
		private Action<InputActionEventData> m_moveMapVertical;

		// Token: 0x040051D1 RID: 20945
		private Action<InputActionEventData> m_onYButtonDown;

		// Token: 0x040051D2 RID: 20946
		private Action<InputActionEventData> m_onConfirmButtonDown;

		// Token: 0x040051D3 RID: 20947
		private Action<InputActionEventData> m_zoomMapVertical;
	}
}
