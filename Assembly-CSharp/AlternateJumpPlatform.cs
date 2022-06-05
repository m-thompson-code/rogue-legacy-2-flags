using System;
using System.Collections;
using UnityEngine;

// Token: 0x020007DB RID: 2011
public class AlternateJumpPlatform : SpecialPlatform
{
	// Token: 0x06003DF0 RID: 15856 RVA: 0x00022488 File Offset: 0x00020688
	private void OnEnable()
	{
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerJump, this.m_onPlayerJump);
	}

	// Token: 0x06003DF1 RID: 15857 RVA: 0x00022497 File Offset: 0x00020697
	private void OnDisable()
	{
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerJump, this.m_onPlayerJump);
	}

	// Token: 0x06003DF2 RID: 15858 RVA: 0x000224A6 File Offset: 0x000206A6
	protected override void Awake()
	{
		base.Awake();
		this.m_renderer = base.GetComponentInChildren<Renderer>();
		this.m_onPlayerJump = new Action<MonoBehaviour, EventArgs>(this.OnPlayerJump);
	}

	// Token: 0x06003DF3 RID: 15859 RVA: 0x000224CC File Offset: 0x000206CC
	protected override IEnumerator Start()
	{
		yield return base.Start();
		this.m_hbController.RepeatHitDuration = 0f;
		this.m_platformCollider = (this.m_hbController.GetCollider(HitboxType.Platform) as BoxCollider2D);
		if (base.Width > 0f)
		{
			SpriteRenderer componentInChildren = base.GetComponentInChildren<SpriteRenderer>();
			componentInChildren.size = new Vector2(base.Width, componentInChildren.size.y);
			this.m_platformCollider.size = new Vector2(base.Width, this.m_platformCollider.size.y);
		}
		this.m_storedPlatformLayer = this.m_platformCollider.gameObject.layer;
		this.m_isInitialized = true;
		this.SetPlatformState();
		yield break;
	}

	// Token: 0x06003DF4 RID: 15860 RVA: 0x000224DB File Offset: 0x000206DB
	private void OnPlayerJump(MonoBehaviour sender, EventArgs args)
	{
		this.m_platformVisible = !this.m_platformVisible;
		this.SetPlatformState();
	}

	// Token: 0x06003DF5 RID: 15861 RVA: 0x000FA31C File Offset: 0x000F851C
	private void SetPlatformState()
	{
		if (this.m_platformVisible)
		{
			this.m_renderer.enabled = true;
			this.m_platformCollider.gameObject.layer = this.m_storedPlatformLayer;
			return;
		}
		this.m_renderer.enabled = false;
		this.m_platformCollider.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
	}

	// Token: 0x06003DF6 RID: 15862 RVA: 0x000FA37C File Offset: 0x000F857C
	public override void SetState(StateID state)
	{
		if (state != StateID.One)
		{
			if (state != StateID.Two)
			{
				if (state == StateID.Random)
				{
					if (CDGHelper.RandomPlusMinus() > 0)
					{
						this.m_platformVisible = true;
					}
					else
					{
						this.m_platformVisible = false;
					}
				}
			}
			else
			{
				this.m_platformVisible = false;
			}
		}
		else
		{
			this.m_platformVisible = true;
		}
		if (this.m_isInitialized)
		{
			this.SetPlatformState();
		}
	}

	// Token: 0x040030B9 RID: 12473
	private bool m_platformVisible = true;

	// Token: 0x040030BA RID: 12474
	private Renderer m_renderer;

	// Token: 0x040030BB RID: 12475
	private BoxCollider2D m_platformCollider;

	// Token: 0x040030BC RID: 12476
	private int m_storedTerrainLayer;

	// Token: 0x040030BD RID: 12477
	private int m_storedPlatformLayer;

	// Token: 0x040030BE RID: 12478
	private bool m_isInitialized;

	// Token: 0x040030BF RID: 12479
	private Action<MonoBehaviour, EventArgs> m_onPlayerJump;
}
