using System;
using UnityEngine;

// Token: 0x02000712 RID: 1810
public class Bodies_Hazard : Hazard
{
	// Token: 0x06003741 RID: 14145 RVA: 0x000E5368 File Offset: 0x000E3568
	public override void Initialize(HazardArgs hazardArgs)
	{
		base.Initialize(hazardArgs);
		foreach (GameObject gameObject in this.m_bodies)
		{
			if (gameObject.activeSelf)
			{
				gameObject.SetActive(false);
			}
		}
		if (base.transform.localEulerAngles.z > 45f && base.transform.localEulerAngles.z < 315f)
		{
			return;
		}
		int num = 37;
		int num2 = RNGManager.GetRandomNumber(RngID.SpecialProps_RoomSeed, "Bodies_Hazard.Initialize()", 0, 100);
		if (GameUtility.IsInLevelEditor)
		{
			num2 = UnityEngine.Random.Range(0, 100);
		}
		if (num2 <= num)
		{
			this.m_activeBodyIndex = RNGManager.GetRandomNumber(RngID.SpecialProps_RoomSeed, "Bodies_Hazard.Initialize()", 0, this.m_bodies.Length);
			bool flag = RNGManager.GetRandomNumber(RngID.SpecialProps_RoomSeed, "Bodies_Hazard.Initialize()", 0, 2) == 1;
			GameObject gameObject2 = this.m_bodies[this.m_activeBodyIndex];
			gameObject2.SetActive(true);
			Vector3 localScale = gameObject2.transform.localScale;
			localScale.x = Mathf.Abs(localScale.x);
			if (flag)
			{
				localScale.x = -localScale.x;
			}
			gameObject2.transform.localScale = localScale;
			SpriteRenderer componentInChildren = gameObject2.GetComponentInChildren<SpriteRenderer>();
			Vector2 leftSide = componentInChildren.bounds.center - componentInChildren.bounds.extents;
			leftSide.y = base.transform.position.y;
			Vector2 rightSide = componentInChildren.bounds.center + componentInChildren.bounds.extents;
			rightSide.y = base.transform.position.y;
			this.CheckGroundRaycast(leftSide, rightSide);
		}
	}

	// Token: 0x06003742 RID: 14146 RVA: 0x000E5524 File Offset: 0x000E3724
	private void CheckGroundRaycast(Vector2 leftSide, Vector2 rightSide)
	{
		if (!Physics2D.Raycast(leftSide, Vector2.down, 0.5f, PlayerManager.GetPlayerController().ControllerCorgi.SavedPlatformMask) || !Physics2D.Raycast(rightSide, Vector2.down, 0.5f, PlayerManager.GetPlayerController().ControllerCorgi.SavedPlatformMask))
		{
			this.m_bodies[this.m_activeBodyIndex].SetActive(false);
		}
	}

	// Token: 0x06003743 RID: 14147 RVA: 0x00002FCA File Offset: 0x000011CA
	public override void ResetHazard()
	{
	}

	// Token: 0x04002C87 RID: 11399
	[SerializeField]
	private GameObject[] m_bodies;

	// Token: 0x04002C88 RID: 11400
	private int m_activeBodyIndex;
}
