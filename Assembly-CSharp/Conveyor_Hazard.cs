using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200071B RID: 1819
public class Conveyor_Hazard : SingleLine_Multi_Hazard, ITerrainOnStayHitResponse, IHitResponse
{
	// Token: 0x0600379E RID: 14238 RVA: 0x0001E91F File Offset: 0x0001CB1F
	protected override void Awake()
	{
		base.Awake();
		this.m_hbController = base.GetComponentInChildren<IHitboxController>();
	}

	// Token: 0x0600379F RID: 14239 RVA: 0x000E6868 File Offset: 0x000E4A68
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

	// Token: 0x060037A0 RID: 14240 RVA: 0x0001E933 File Offset: 0x0001CB33
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

	// Token: 0x060037A1 RID: 14241 RVA: 0x000E68D0 File Offset: 0x000E4AD0
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

	// Token: 0x060037A2 RID: 14242 RVA: 0x00002FCA File Offset: 0x000011CA
	public override void ResetHazard()
	{
	}

	// Token: 0x04002CCA RID: 11466
	[SerializeField]
	private SpriteRenderer m_lineSprite;

	// Token: 0x04002CCB RID: 11467
	private bool m_moveRight = true;

	// Token: 0x04002CCC RID: 11468
	private IHitboxController m_hbController;
}
