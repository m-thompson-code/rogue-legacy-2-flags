using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000448 RID: 1096
public class Conveyor_Hazard : SingleLine_Multi_Hazard, ITerrainOnStayHitResponse, IHitResponse
{
	// Token: 0x06002850 RID: 10320 RVA: 0x0008597C File Offset: 0x00083B7C
	protected override void Awake()
	{
		base.Awake();
		this.m_hbController = base.GetComponentInChildren<IHitboxController>();
	}

	// Token: 0x06002851 RID: 10321 RVA: 0x00085990 File Offset: 0x00083B90
	public override void Initialize(PivotPoint pivot, int width, HazardArgs hazardArgs)
	{
		base.Initialize(pivot, width, hazardArgs);
		StateID initialState = hazardArgs.InitialState;
		if (initialState != StateID.One)
		{
			if (initialState != StateID.Two)
			{
				if (initialState == StateID.Random)
				{
					if (CDGHelper.RandomPlusMinus() > 0)
					{
						this.m_moveRight = true;
					}
					else
					{
						this.m_moveRight = false;
					}
				}
			}
			else
			{
				this.m_moveRight = false;
			}
		}
		else
		{
			this.m_moveRight = true;
		}
		base.StartCoroutine(this.SetConveyorWidth(width));
	}

	// Token: 0x06002852 RID: 10322 RVA: 0x000859F7 File Offset: 0x00083BF7
	private IEnumerator SetConveyorWidth(int width)
	{
		while (!this.m_hbController.IsInitialized)
		{
			yield return null;
		}
		this.m_hbController.RepeatHitDuration = 0f;
		BoxCollider2D boxCollider2D = this.m_hbController.GetCollider(HitboxType.Terrain) as BoxCollider2D;
		Vector2 size = boxCollider2D.size;
		size.x = (float)width;
		boxCollider2D.size = size;
		this.m_lineSprite.size = new Vector2((float)width, this.m_lineSprite.size.y);
		yield break;
	}

	// Token: 0x06002853 RID: 10323 RVA: 0x00085A10 File Offset: 0x00083C10
	public void TerrainOnStayHitResponse(IHitboxController otherHBController)
	{
		if (otherHBController == null)
		{
			return;
		}
		if (!otherHBController.RootGameObject.CompareTag("Player"))
		{
			return;
		}
		CorgiController_RL component = otherHBController.RootGameObject.GetComponent<CorgiController_RL>();
		if (component != null && component.State.IsGrounded)
		{
			Vector3 localPosition = component.gameObject.transform.localPosition;
			if (this.m_moveRight)
			{
				localPosition.x += 10f * Time.deltaTime;
			}
			else
			{
				localPosition.x -= 10f * Time.deltaTime;
			}
			component.gameObject.transform.localPosition = localPosition;
		}
	}

	// Token: 0x06002854 RID: 10324 RVA: 0x00085AAE File Offset: 0x00083CAE
	public override void ResetHazard()
	{
	}

	// Token: 0x0400216E RID: 8558
	[SerializeField]
	private SpriteRenderer m_lineSprite;

	// Token: 0x0400216F RID: 8559
	private bool m_moveRight = true;

	// Token: 0x04002170 RID: 8560
	private IHitboxController m_hbController;
}
