using System;
using Rewired;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace RL_Windows
{
	// Token: 0x020008BE RID: 2238
	public abstract class WindowController : MonoBehaviour
	{
		// Token: 0x170017E2 RID: 6114
		// (get) Token: 0x06004942 RID: 18754 RVA: 0x00108759 File Offset: 0x00106959
		public virtual int SortOrderOverride
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x170017E3 RID: 6115
		// (get) Token: 0x06004943 RID: 18755 RVA: 0x0010875C File Offset: 0x0010695C
		public Canvas WindowCanvas
		{
			get
			{
				return this.m_windowCanvas;
			}
		}

		// Token: 0x170017E4 RID: 6116
		// (get) Token: 0x06004944 RID: 18756
		public abstract WindowID ID { get; }

		// Token: 0x170017E5 RID: 6117
		// (get) Token: 0x06004945 RID: 18757 RVA: 0x00108764 File Offset: 0x00106964
		public Player RewiredPlayer
		{
			get
			{
				return Rewired_RL.Player;
			}
		}

		// Token: 0x170017E6 RID: 6118
		// (get) Token: 0x06004946 RID: 18758 RVA: 0x0010876B File Offset: 0x0010696B
		// (set) Token: 0x06004947 RID: 18759 RVA: 0x00108773 File Offset: 0x00106973
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

		// Token: 0x170017E7 RID: 6119
		// (get) Token: 0x06004948 RID: 18760 RVA: 0x0010877C File Offset: 0x0010697C
		// (set) Token: 0x06004949 RID: 18761 RVA: 0x00108784 File Offset: 0x00106984
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

		// Token: 0x170017E8 RID: 6120
		// (get) Token: 0x0600494A RID: 18762 RVA: 0x0010878D File Offset: 0x0010698D
		// (set) Token: 0x0600494B RID: 18763 RVA: 0x00108795 File Offset: 0x00106995
		public bool IsInitialized { get; protected set; }

		// Token: 0x0600494C RID: 18764 RVA: 0x001087A0 File Offset: 0x001069A0
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

		// Token: 0x0600494D RID: 18765 RVA: 0x001087FC File Offset: 0x001069FC
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

		// Token: 0x0600494E RID: 18766 RVA: 0x00108875 File Offset: 0x00106A75
		public virtual void SetHasFocus(bool hasFocus)
		{
			if (hasFocus)
			{
				this.OnFocus();
				return;
			}
			this.OnLostFocus();
		}

		// Token: 0x0600494F RID: 18767
		protected abstract void OnOpen();

		// Token: 0x06004950 RID: 18768
		protected abstract void OnClose();

		// Token: 0x06004951 RID: 18769
		protected abstract void OnFocus();

		// Token: 0x06004952 RID: 18770
		protected abstract void OnLostFocus();

		// Token: 0x06004953 RID: 18771 RVA: 0x00108887 File Offset: 0x00106A87
		protected virtual void OnPause()
		{
		}

		// Token: 0x06004954 RID: 18772 RVA: 0x00108889 File Offset: 0x00106A89
		protected virtual void OnUnpause()
		{
		}

		// Token: 0x04003DC8 RID: 15816
		[SerializeField]
		private bool m_pauseGameWhenOpen = true;

		// Token: 0x04003DC9 RID: 15817
		[SerializeField]
		private bool m_openOnSceneLoad;

		// Token: 0x04003DCA RID: 15818
		[SerializeField]
		protected Canvas m_windowCanvas;

		// Token: 0x04003DCB RID: 15819
		[SerializeField]
		[FormerlySerializedAs("m_windowOpenedUnityEvent")]
		public UnityEvent WindowOpenedEvent;

		// Token: 0x04003DCC RID: 15820
		[SerializeField]
		[FormerlySerializedAs("m_windowOpenedUnityEvent")]
		public UnityEvent WindowClosedEvent;
	}
}
