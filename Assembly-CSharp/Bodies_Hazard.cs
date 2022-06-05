using System;
using UnityEngine;

// Token: 0x02000444 RID: 1092
public class Bodies_Hazard : Hazard
{
	// Token: 0x06002811 RID: 10257 RVA: 0x000849D8 File Offset: 0x00082BD8
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

	// Token: 0x06002812 RID: 10258 RVA: 0x00084B94 File Offset: 0x00082D94
	private void CheckGroundRaycast(Vector2 leftSide, Vector2 rightSide)
	{
		if (!Physics2D.Raycast(leftSide, Vector2.down, 0.5f, PlayerManager.GetPlayerController().ControllerCorgi.SavedPlatformMask) || !Physics2D.Raycast(rightSide, Vector2.down, 0.5f, PlayerManager.GetPlayerController().ControllerCorgi.SavedPlatformMask))
		{
			this.m_bodies[this.m_activeBodyIndex].SetActive(false);
		}
	}

	// Token: 0x06002813 RID: 10259 RVA: 0x00084C0A File Offset: 0x00082E0A
	public override void ResetHazard()
	{
	}

	// Token: 0x04002144 RID: 8516
	[SerializeField]
	private GameObject[] m_bodies;

	// Token: 0x04002145 RID: 8517
	private int m_activeBodyIndex;
}
