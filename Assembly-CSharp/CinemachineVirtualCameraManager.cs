using System;
using Cinemachine;
using UnityEngine;

// Token: 0x02000691 RID: 1681
public class CinemachineVirtualCameraManager : MonoBehaviour
{
	// Token: 0x17001531 RID: 5425
	// (get) Token: 0x06003D08 RID: 15624 RVA: 0x000D3BED File Offset: 0x000D1DED
	// (set) Token: 0x06003D09 RID: 15625 RVA: 0x000D3C03 File Offset: 0x000D1E03
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

	// Token: 0x17001532 RID: 5426
	// (get) Token: 0x06003D0A RID: 15626 RVA: 0x000D3C0C File Offset: 0x000D1E0C
	// (set) Token: 0x06003D0B RID: 15627 RVA: 0x000D3C14 File Offset: 0x000D1E14
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

	// Token: 0x17001533 RID: 5427
	// (get) Token: 0x06003D0C RID: 15628 RVA: 0x000D3C1D File Offset: 0x000D1E1D
	// (set) Token: 0x06003D0D RID: 15629 RVA: 0x000D3C25 File Offset: 0x000D1E25
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

	// Token: 0x17001534 RID: 5428
	// (get) Token: 0x06003D0E RID: 15630 RVA: 0x000D3C2E File Offset: 0x000D1E2E
	public CinemachineVirtualCamera VirtualCamera
	{
		get
		{
			return this.m_virtualCamera;
		}
	}

	// Token: 0x06003D0F RID: 15631 RVA: 0x000D3C36 File Offset: 0x000D1E36
	private void Start()
	{
	}

	// Token: 0x06003D10 RID: 15632 RVA: 0x000D3C38 File Offset: 0x000D1E38
	private void OnDisable()
	{
		this.IsActiveVirtualCamera = false;
		this.SetFollowTarget(null);
	}

	// Token: 0x06003D11 RID: 15633 RVA: 0x000D3C48 File Offset: 0x000D1E48
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

	// Token: 0x06003D12 RID: 15634 RVA: 0x000D3CD4 File Offset: 0x000D1ED4
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

	// Token: 0x06003D13 RID: 15635 RVA: 0x000D3D3F File Offset: 0x000D1F3F
	public void SetFollowTarget(Transform target)
	{
		this.VirtualCamera.Follow = target;
	}

	// Token: 0x06003D14 RID: 15636 RVA: 0x000D3D4D File Offset: 0x000D1F4D
	public void SetLensSize(float size)
	{
		this.VirtualCamera.m_Lens.OrthographicSize = size;
	}

	// Token: 0x04002DC6 RID: 11718
	[SerializeField]
	private CinemachineVirtualCamera m_virtualCamera;

	// Token: 0x04002DC7 RID: 11719
	private bool m_isActiveVirtualCamera;

	// Token: 0x04002DC8 RID: 11720
	private bool m_isInitialised;

	// Token: 0x04002DC9 RID: 11721
	private Vector2 m_confinerSize;
}
