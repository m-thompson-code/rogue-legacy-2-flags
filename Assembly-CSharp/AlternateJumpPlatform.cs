using System;
using System.Collections;
using UnityEngine;

// Token: 0x020004C0 RID: 1216
public class AlternateJumpPlatform : SpecialPlatform
{
	// Token: 0x06002D32 RID: 11570 RVA: 0x0009937A File Offset: 0x0009757A
	private void OnEnable()
	{
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerJump, this.m_onPlayerJump);
	}

	// Token: 0x06002D33 RID: 11571 RVA: 0x00099389 File Offset: 0x00097589
	private void OnDisable()
	{
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerJump, this.m_onPlayerJump);
	}

	// Token: 0x06002D34 RID: 11572 RVA: 0x00099398 File Offset: 0x00097598
	protected override void Awake()
	{
		base.Awake();
		this.m_renderer = base.GetComponentInChildren<Renderer>();
		this.m_onPlayerJump = new Action<MonoBehaviour, EventArgs>(this.OnPlayerJump);
	}

	// Token: 0x06002D35 RID: 11573 RVA: 0x000993BE File Offset: 0x000975BE
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

	// Token: 0x06002D36 RID: 11574 RVA: 0x000993CD File Offset: 0x000975CD
	private void OnPlayerJump(MonoBehaviour sender, EventArgs args)
	{
		this.m_platformVisible = !this.m_platformVisible;
		this.SetPlatformState();
	}

	// Token: 0x06002D37 RID: 11575 RVA: 0x000993E4 File Offset: 0x000975E4
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

	// Token: 0x06002D38 RID: 11576 RVA: 0x00099444 File Offset: 0x00097644
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

	// Token: 0x04002449 RID: 9289
	private bool m_platformVisible = true;

	// Token: 0x0400244A RID: 9290
	private Renderer m_renderer;

	// Token: 0x0400244B RID: 9291
	private BoxCollider2D m_platformCollider;

	// Token: 0x0400244C RID: 9292
	private int m_storedTerrainLayer;

	// Token: 0x0400244D RID: 9293
	private int m_storedPlatformLayer;

	// Token: 0x0400244E RID: 9294
	private bool m_isInitialized;

	// Token: 0x0400244F RID: 9295
	private Action<MonoBehaviour, EventArgs> m_onPlayerJump;
}
