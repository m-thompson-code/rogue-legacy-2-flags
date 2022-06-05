using System;
using UnityEngine;

// Token: 0x02000322 RID: 802
public class ChangeHitboxesBehaviour : StateMachineBehaviour
{
	// Token: 0x06001972 RID: 6514 RVA: 0x0008FD7C File Offset: 0x0008DF7C
	public void CacheHitboxes(Animator animator)
	{
		if (!this.m_cached)
		{
			this.m_hitboxesToEnableObjList = new GameObject[this.m_hitboxesToEnable.Length];
			this.m_hitboxesToDisableObjList = new GameObject[this.m_hitboxesToDisable.Length];
			IHitboxController componentInChildren = animator.GetRoot(false).GetComponentInChildren<IHitboxController>(true);
			if (componentInChildren != null)
			{
				for (int i = 0; i < this.m_hitboxesToDisable.Length; i++)
				{
					string objName = this.m_hitboxesToDisable[i];
					GameObject gameObject = this.CheckForHitbox(objName, componentInChildren);
					if (gameObject != null)
					{
						gameObject.SetActive(false);
						this.m_hitboxesToDisableObjList[i] = gameObject;
					}
				}
				for (int j = 0; j < this.m_hitboxesToEnable.Length; j++)
				{
					string objName2 = this.m_hitboxesToEnable[j];
					GameObject gameObject2 = this.CheckForHitbox(objName2, componentInChildren);
					if (gameObject2 != null)
					{
						gameObject2.SetActive(true);
						this.m_hitboxesToEnableObjList[j] = gameObject2;
					}
				}
			}
			this.m_cached = true;
		}
	}

	// Token: 0x06001973 RID: 6515 RVA: 0x0008FE60 File Offset: 0x0008E060
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		this.CacheHitboxes(animator);
		foreach (GameObject gameObject in this.m_hitboxesToEnableObjList)
		{
			if (gameObject)
			{
				gameObject.SetActive(true);
			}
		}
		foreach (GameObject gameObject2 in this.m_hitboxesToDisableObjList)
		{
			if (gameObject2)
			{
				gameObject2.SetActive(false);
			}
		}
	}

	// Token: 0x06001974 RID: 6516 RVA: 0x0008FEC4 File Offset: 0x0008E0C4
	private GameObject CheckForHitbox(string objName, IHitboxController hbController)
	{
		GameObject gameObject = hbController.ContainsHitbox(HitboxType.Body, objName);
		if (gameObject != null)
		{
			return gameObject;
		}
		gameObject = hbController.ContainsHitbox(HitboxType.Weapon, objName);
		if (gameObject != null)
		{
			return gameObject;
		}
		gameObject = hbController.ContainsHitbox(HitboxType.Terrain, objName);
		if (gameObject != null)
		{
			return gameObject;
		}
		return null;
	}

	// Token: 0x04001824 RID: 6180
	[Space(10f)]
	[SerializeField]
	private string[] m_hitboxesToDisable;

	// Token: 0x04001825 RID: 6181
	[Space(10f)]
	[SerializeField]
	private string[] m_hitboxesToEnable;

	// Token: 0x04001826 RID: 6182
	private GameObject[] m_hitboxesToDisableObjList;

	// Token: 0x04001827 RID: 6183
	private GameObject[] m_hitboxesToEnableObjList;

	// Token: 0x04001828 RID: 6184
	private bool m_cached;
}
