using System;
using UnityEngine;

// Token: 0x020001B3 RID: 435
public class ChangeHitboxesBehaviour : StateMachineBehaviour
{
	// Token: 0x06001129 RID: 4393 RVA: 0x00031930 File Offset: 0x0002FB30
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

	// Token: 0x0600112A RID: 4394 RVA: 0x00031A14 File Offset: 0x0002FC14
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

	// Token: 0x0600112B RID: 4395 RVA: 0x00031A78 File Offset: 0x0002FC78
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

	// Token: 0x0400121B RID: 4635
	[Space(10f)]
	[SerializeField]
	private string[] m_hitboxesToDisable;

	// Token: 0x0400121C RID: 4636
	[Space(10f)]
	[SerializeField]
	private string[] m_hitboxesToEnable;

	// Token: 0x0400121D RID: 4637
	private GameObject[] m_hitboxesToDisableObjList;

	// Token: 0x0400121E RID: 4638
	private GameObject[] m_hitboxesToEnableObjList;

	// Token: 0x0400121F RID: 4639
	private bool m_cached;
}
