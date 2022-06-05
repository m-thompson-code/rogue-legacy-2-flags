using System;
using Rewired;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace RL_Windows
{
	// Token: 0x02000E02 RID: 3586
	public abstract class WindowController : MonoBehaviour
	{
		// Token: 0x1700207C RID: 8316
		// (get) Token: 0x0600650C RID: 25868 RVA: 0x00037B9A File Offset: 0x00035D9A
		public virtual int SortOrderOverride
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x1700207D RID: 8317
		// (get) Token: 0x0600650D RID: 25869 RVA: 0x00037B9D File Offset: 0x00035D9D
		public Canvas WindowCanvas
		{
			get
			{
				return this.m_windowCanvas;
			}
		}

		// Token: 0x1700207E RID: 8318
		// (get) Token: 0x0600650E RID: 25870
		public abstract WindowID ID { get; }

		// Token: 0x1700207F RID: 8319
		// (get) Token: 0x0600650F RID: 25871 RVA: 0x00037BA5 File Offset: 0x00035DA5
		public Player RewiredPlayer
		{
			get
			{
				return Rewired_RL.Player;
			}
		}

		// Token: 0x17002080 RID: 8320
		// (get) Token: 0x06006510 RID: 25872 RVA: 0x00037BAC File Offset: 0x00035DAC
		// (set) Token: 0x06006511 RID: 25873 RVA: 0x00037BB4 File Offset: 0x00035DB4
		public bool PauseGameWhenOpen
		{
			get
			{
				return this.m_pauseGameWhenOpen;
			}
			protected set
			{
				this.m_pauseGameWhenOpen = value;
			}
		}

		// Token: 0x17002081 RID: 8321
		// (get) Token: 0x06006512 RID: 25874 RVA: 0x00037BBD File Offset: 0x00035DBD
		// (set) Token: 0x06006513 RID: 25875 RVA: 0x00037BC5 File Offset: 0x00035DC5
		public bool OpenOnSceneLoad
		{
			get
			{
				return this.m_openOnSceneLoad;
			}
			protected set
			{
				this.m_openOnSceneLoad = value;
			}
		}

		// Token: 0x17002082 RID: 8322
		// (get) Token: 0x06006514 RID: 25876 RVA: 0x00037BCE File Offset: 0x00035DCE
		// (set) Token: 0x06006515 RID: 25877 RVA: 0x00037BD6 File Offset: 0x00035DD6
		public bool IsInitialized { get; protected set; }

		// Token: 0x06006516 RID: 25878 RVA: 0x00177D50 File Offset: 0x00175F50
		public virtual void Initialize()
		{
			if (this.m_windowCanvas)
			{
				if (this.m_windowCanvas.renderMode != RenderMode.WorldSpace)
				{
					this.m_windowCanvas.renderMode = RenderMode.ScreenSpaceCamera;
					this.m_windowCanvas.worldCamera = CameraController.UICamera;
				}
				this.m_windowCanvas.gameObject.SetActive(false);
			}
			this.IsInitialized = true;
		}

		// Token: 0x06006517 RID: 25879 RVA: 0x00177DAC File Offset: 0x00175FAC
		public virtual void SetIsOpen(bool isOpen)
		{
			if (isOpen)
			{
				if (this.WindowOpenedEvent != null)
				{
					this.WindowOpenedEvent.Invoke();
				}
				base.enabled = true;
				if (this.m_windowCanvas)
				{
					CanvasScaler component = this.m_windowCanvas.GetComponent<CanvasScaler>();
					if (component)
					{
						component.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
					}
				}
				this.OnOpen();
				return;
			}
			if (this.WindowClosedEvent != null)
			{
				this.WindowClosedEvent.Invoke();
			}
			base.enabled = false;
			this.OnClose();
		}

		// Token: 0x06006518 RID: 25880 RVA: 0x00037BDF File Offset: 0x00035DDF
		public virtual void SetHasFocus(bool hasFocus)
		{
			if (hasFocus)
			{
				this.OnFocus();
				return;
			}
			this.OnLostFocus();
		}

		// Token: 0x06006519 RID: 25881
		protected abstract void OnOpen();

		// Token: 0x0600651A RID: 25882
		protected abstract void OnClose();

		// Token: 0x0600651B RID: 25883
		protected abstract void OnFocus();

		// Token: 0x0600651C RID: 25884
		protected abstract void OnLostFocus();

		// Token: 0x0600651D RID: 25885 RVA: 0x00002FCA File Offset: 0x000011CA
		protected virtual void OnPause()
		{
		}

		// Token: 0x0600651E RID: 25886 RVA: 0x00002FCA File Offset: 0x000011CA
		protected virtual void OnUnpause()
		{
		}

		// Token: 0x0400524B RID: 21067
		[SerializeField]
		private bool m_pauseGameWhenOpen = true;

		// Token: 0x0400524C RID: 21068
		[SerializeField]
		private bool m_openOnSceneLoad;

		// Token: 0x0400524D RID: 21069
		[SerializeField]
		protected Canvas m_windowCanvas;

		// Token: 0x0400524E RID: 21070
		[SerializeField]
		[FormerlySerializedAs("m_windowOpenedUnityEvent")]
		public UnityEvent WindowOpenedEvent;

		// Token: 0x0400524F RID: 21071
		[SerializeField]
		[FormerlySerializedAs("m_windowOpenedUnityEvent")]
		public UnityEvent WindowClosedEvent;
	}
}
