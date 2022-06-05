using System;
using Cinemachine;
using UnityEngine;

// Token: 0x02000B1A RID: 2842
public class CinemachineVirtualCameraManager : MonoBehaviour
{
	// Token: 0x17001CFD RID: 7421
	// (get) Token: 0x0600559D RID: 21917 RVA: 0x0002E835 File Offset: 0x0002CA35
	// (set) Token: 0x0600559E RID: 21918 RVA: 0x0002E84B File Offset: 0x0002CA4B
	public Vector2 ConfinerSize
	{
		get
		{
			if (!this.IsInitialised)
			{
				this.Initialise();
			}
			return this.m_confinerSize;
		}
		private set
		{
			this.m_confinerSize = value;
		}
	}

	// Token: 0x17001CFE RID: 7422
	// (get) Token: 0x0600559F RID: 21919 RVA: 0x0002E854 File Offset: 0x0002CA54
	// (set) Token: 0x060055A0 RID: 21920 RVA: 0x0002E85C File Offset: 0x0002CA5C
	public bool IsActiveVirtualCamera
	{
		get
		{
			return this.m_isActiveVirtualCamera;
		}
		private set
		{
			this.m_isActiveVirtualCamera = value;
		}
	}

	// Token: 0x17001CFF RID: 7423
	// (get) Token: 0x060055A1 RID: 21921 RVA: 0x0002E865 File Offset: 0x0002CA65
	// (set) Token: 0x060055A2 RID: 21922 RVA: 0x0002E86D File Offset: 0x0002CA6D
	public bool IsInitialised
	{
		get
		{
			return this.m_isInitialised;
		}
		private set
		{
			this.m_isInitialised = value;
		}
	}

	// Token: 0x17001D00 RID: 7424
	// (get) Token: 0x060055A3 RID: 21923 RVA: 0x0002E876 File Offset: 0x0002CA76
	public CinemachineVirtualCamera VirtualCamera
	{
		get
		{
			return this.m_virtualCamera;
		}
	}

	// Token: 0x060055A4 RID: 21924 RVA: 0x00002FCA File Offset: 0x000011CA
	private void Start()
	{
	}

	// Token: 0x060055A5 RID: 21925 RVA: 0x0002E87E File Offset: 0x0002CA7E
	private void OnDisable()
	{
		this.IsActiveVirtualCamera = false;
		this.SetFollowTarget(null);
	}

	// Token: 0x060055A6 RID: 21926 RVA: 0x00143D50 File Offset: 0x00141F50
	private void Initialise()
	{
		BaseRoom componentInParent = base.GetComponentInParent<BaseRoom>();
		if (componentInParent)
		{
			CinemachineConfiner_RL component = base.GetComponent<CinemachineConfiner_RL>();
			if (component)
			{
				Collider2D collider2D = componentInParent.Collider2D;
				if (collider2D)
				{
					component.m_BoundingShape2D = collider2D;
					this.ConfinerSize = collider2D.bounds.size / new Vector2(32f, 18f);
				}
				else
				{
					Debug.LogFormat("<color=red>| {0} |: Unable to find Collider2D on Room Root. This is required by the Cinemachine Virtual Camera's Confiner Component.</color>", new object[]
					{
						this
					});
				}
			}
		}
		this.IsInitialised = true;
	}

	// Token: 0x060055A7 RID: 21927 RVA: 0x00143DDC File Offset: 0x00141FDC
	public void SetIsActiveCamera(bool isActiveCamera)
	{
		if (!this.IsInitialised)
		{
			this.Initialise();
		}
		this.IsActiveVirtualCamera = isActiveCamera;
		if (isActiveCamera)
		{
			if (PlayerManager.IsInstantiated && this.VirtualCamera.Follow == null)
			{
				this.SetFollowTarget(PlayerManager.GetPlayerController().FollowTargetGO.transform);
			}
		}
		else
		{
			this.SetFollowTarget(null);
		}
		if (isActiveCamera)
		{
			this.VirtualCamera.MoveToTopOfPrioritySubqueue();
		}
	}

	// Token: 0x060055A8 RID: 21928 RVA: 0x0002E88E File Offset: 0x0002CA8E
	public void SetFollowTarget(Transform target)
	{
		this.VirtualCamera.Follow = target;
	}

	// Token: 0x060055A9 RID: 21929 RVA: 0x0002E89C File Offset: 0x0002CA9C
	public void SetLensSize(float size)
	{
		this.VirtualCamera.m_Lens.OrthographicSize = size;
	}

	// Token: 0x04003F91 RID: 16273
	[SerializeField]
	private CinemachineVirtualCamera m_virtualCamera;

	// Token: 0x04003F92 RID: 16274
	private bool m_isActiveVirtualCamera;

	// Token: 0x04003F93 RID: 16275
	private bool m_isInitialised;

	// Token: 0x04003F94 RID: 16276
	private Vector2 m_confinerSize;
}
