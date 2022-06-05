using System;
using System.Collections;
using Rewired;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RL_Windows
{
	// Token: 0x020008B8 RID: 2232
	public class MapWindowController : WindowController
	{
		// Token: 0x170017D4 RID: 6100
		// (get) Token: 0x060048BD RID: 18621 RVA: 0x00104FEB File Offset: 0x001031EB
		public override WindowID ID
		{
			get
			{
				return WindowID.Map;
			}
		}

		// Token: 0x170017D5 RID: 6101
		// (get) Token: 0x060048BE RID: 18622 RVA: 0x00104FEE File Offset: 0x001031EE
		public Scene Scene
		{
			get
			{
				return base.gameObject.scene;
			}
		}

		// Token: 0x170017D6 RID: 6102
		// (get) Token: 0x060048BF RID: 18623 RVA: 0x00104FFB File Offset: 0x001031FB
		// (set) Token: 0x060048C0 RID: 18624 RVA: 0x00105003 File Offset: 0x00103203
		private bool IsMapMade { get; set; }

		// Token: 0x060048C1 RID: 18625 RVA: 0x0010500C File Offset: 0x0010320C
		private void Awake()
		{
			this.m_onCancelButtonDown = new Action<InputActionEventData>(this.OnCancelButtonDown);
			this.m_moveMapHorizontal = new Action<InputActionEventData>(this.MoveMapHorizontal);
			this.m_moveMapVertical = new Action<InputActionEventData>(this.MoveMapVertical);
			this.m_onYButtonDown = new Action<InputActionEventData>(this.OnYButtonDown);
			this.m_onConfirmButtonDown = new Action<InputActionEventData>(this.OnConfirmButtonDown);
			this.m_zoomMapVertical = new Action<InputActionEventData>(this.ZoomMapVertical);
		}

		// Token: 0x060048C2 RID: 18626 RVA: 0x0010508C File Offset: 0x0010328C
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

		// Token: 0x060048C3 RID: 18627 RVA: 0x00105170 File Offset: 0x00103370
		protected override void OnClose()
		{
			MapWindowController.EnteredFromOtherSubmenu = false;
			this.m_windowCanvas.gameObject.SetActive(false);
			MapController.SetCameraFollowIsOn(true);
			MapController.MapWindowCamera.orthographicSize = 2f;
			MapController.MapWindowCamera.gameObject.SetActive(false);
			this.UnsubscribeFromRewiredInputEvents();
		}

		// Token: 0x060048C4 RID: 18628 RVA: 0x001051BF File Offset: 0x001033BF
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

		// Token: 0x060048C5 RID: 18629 RVA: 0x001051CE File Offset: 0x001033CE
		protected override void OnFocus()
		{
			this.SubscribeToRewiredInputEvents();
		}

		// Token: 0x060048C6 RID: 18630 RVA: 0x001051D6 File Offset: 0x001033D6
		protected override void OnLostFocus()
		{
			this.UnsubscribeFromRewiredInputEvents();
		}

		// Token: 0x060048C7 RID: 18631 RVA: 0x001051DE File Offset: 0x001033DE
		protected override void OnPause()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060048C8 RID: 18632 RVA: 0x001051E5 File Offset: 0x001033E5
		protected override void OnUnpause()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060048C9 RID: 18633 RVA: 0x001051EC File Offset: 0x001033EC
		protected virtual void OnCancelButtonDown(InputActionEventData obj)
		{
			if (WindowManager.GetIsWindowOpen(WindowID.Pause))
			{
				WindowManager.CloseAllOpenWindows();
				return;
			}
			WindowManager.SetWindowIsOpen(this.ID, false);
		}

		// Token: 0x060048CA RID: 18634 RVA: 0x00105208 File Offset: 0x00103408
		protected virtual void OnConfirmButtonDown(InputActionEventData obj)
		{
			base.StartCoroutine(this.TweenCameraToPlayer(0.25f));
		}

		// Token: 0x060048CB RID: 18635 RVA: 0x0010521C File Offset: 0x0010341C
		protected virtual void OnYButtonDown(InputActionEventData obj)
		{
			this.ToggleLegend();
		}

		// Token: 0x060048CC RID: 18636 RVA: 0x00105224 File Offset: 0x00103424
		public override void Initialize()
		{
			base.Initialize();
		}

		// Token: 0x060048CD RID: 18637 RVA: 0x0010522C File Offset: 0x0010342C
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

		// Token: 0x060048CE RID: 18638 RVA: 0x001052F0 File Offset: 0x001034F0
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

		// Token: 0x060048CF RID: 18639 RVA: 0x001053B2 File Offset: 0x001035B2
		private void ToggleLegend()
		{
			this.m_legend.SetActive(!this.m_legend.activeSelf);
		}

		// Token: 0x060048D0 RID: 18640 RVA: 0x001053D0 File Offset: 0x001035D0
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

		// Token: 0x060048D1 RID: 18641 RVA: 0x00105464 File Offset: 0x00103664
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

		// Token: 0x060048D2 RID: 18642 RVA: 0x001054F8 File Offset: 0x001036F8
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

		// Token: 0x060048D3 RID: 18643 RVA: 0x001055C4 File Offset: 0x001037C4
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

		// Token: 0x04003D61 RID: 15713
		public static bool EnteredFromOtherSubmenu;

		// Token: 0x04003D62 RID: 15714
		[SerializeField]
		private Camera m_mapCamera;

		// Token: 0x04003D63 RID: 15715
		[SerializeField]
		private Ferr2DT_PathTerrain m_mapFerr2DObj;

		// Token: 0x04003D64 RID: 15716
		[SerializeField]
		private GameObject m_mapHorizontalDoor;

		// Token: 0x04003D65 RID: 15717
		[SerializeField]
		private GameObject m_mapVerticalDoor;

		// Token: 0x04003D66 RID: 15718
		[SerializeField]
		private GameObject m_mapEnemyIcon;

		// Token: 0x04003D67 RID: 15719
		[SerializeField]
		private GameObject m_legend;

		// Token: 0x04003D68 RID: 15720
		[Header("Animation Objects")]
		[SerializeField]
		private CanvasGroup m_shadowsCanvasGroup;

		// Token: 0x04003D69 RID: 15721
		[SerializeField]
		private CanvasGroup m_fadeBGCanvasGroup;

		// Token: 0x04003D6A RID: 15722
		[SerializeField]
		private CanvasGroup m_gridAndRTCanvasGroup;

		// Token: 0x04003D6B RID: 15723
		[SerializeField]
		private CanvasGroup m_legendCanvasGroup;

		// Token: 0x04003D6C RID: 15724
		[SerializeField]
		private RawImage m_uvGridImage;

		// Token: 0x04003D6D RID: 15725
		[SerializeField]
		private GameObject m_yellowStarsGO;

		// Token: 0x04003D6E RID: 15726
		private GameObject m_mapObj;

		// Token: 0x04003D6F RID: 15727
		private Action<InputActionEventData> m_onCancelButtonDown;

		// Token: 0x04003D70 RID: 15728
		private Action<InputActionEventData> m_moveMapHorizontal;

		// Token: 0x04003D71 RID: 15729
		private Action<InputActionEventData> m_moveMapVertical;

		// Token: 0x04003D72 RID: 15730
		private Action<InputActionEventData> m_onYButtonDown;

		// Token: 0x04003D73 RID: 15731
		private Action<InputActionEventData> m_onConfirmButtonDown;

		// Token: 0x04003D74 RID: 15732
		private Action<InputActionEventData> m_zoomMapVertical;
	}
}
