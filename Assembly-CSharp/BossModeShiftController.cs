using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x020000BD RID: 189
public class BossModeShiftController : MonoBehaviour
{
	// Token: 0x0600034B RID: 843 RVA: 0x000513A0 File Offset: 0x0004F5A0
	private void Awake()
	{
		if (BossModeShiftController.m_matBlock_STATIC == null)
		{
			BossModeShiftController.m_matBlock_STATIC = new MaterialPropertyBlock();
		}
		List<Renderer> list = this.m_visuals.GetComponentsInChildren<Renderer>().ToList<Renderer>();
		foreach (Renderer item in this.m_renderersToNotDissolve)
		{
			list.Remove(item);
		}
		this.m_renderersToDissolve = list.ToArray();
		this.m_initialMats = new Material[this.m_materialsToChangeFrom.Length];
		for (int j = 0; j < this.m_materialsToChangeFrom.Length; j++)
		{
			this.m_initialMats[j] = this.m_materialsToChangeFrom[j].material;
		}
	}

	// Token: 0x0600034C RID: 844 RVA: 0x0000465C File Offset: 0x0000285C
	private void OnEnable()
	{
		this.Reset();
	}

	// Token: 0x0600034D RID: 845 RVA: 0x00051440 File Offset: 0x0004F640
	public void ChangeMaterials()
	{
		if (this.m_materialsToChangeFrom.Length != this.m_materialsToChangeTo.Length)
		{
			throw new Exception("Cannot Change Materials. Material Change arrays must be same size.");
		}
		for (int i = 0; i < this.m_materialsToChangeFrom.Length; i++)
		{
			this.m_materialsToChangeFrom[i].material = this.m_materialsToChangeTo[i];
		}
	}

	// Token: 0x0600034E RID: 846 RVA: 0x00004664 File Offset: 0x00002864
	public void DissolveMaterials(float duration)
	{
		base.StopAllCoroutines();
		base.StartCoroutine(this.DissolveCoroutine(duration));
	}

	// Token: 0x0600034F RID: 847 RVA: 0x0000467A File Offset: 0x0000287A
	private IEnumerator DissolveCoroutine(float duration)
	{
		float startTime = Time.time;
		while (Time.time < startTime + duration)
		{
			float value = (Time.time - startTime) / duration;
			foreach (Renderer renderer in this.m_renderersToDissolve)
			{
				renderer.GetPropertyBlock(BossModeShiftController.m_matBlock_STATIC);
				BossModeShiftController.m_matBlock_STATIC.SetFloat(ShaderID_RL._Dissolve, value);
				renderer.SetPropertyBlock(BossModeShiftController.m_matBlock_STATIC);
			}
			yield return null;
		}
		foreach (Renderer renderer2 in this.m_renderersToDissolve)
		{
			renderer2.GetPropertyBlock(BossModeShiftController.m_matBlock_STATIC);
			BossModeShiftController.m_matBlock_STATIC.SetFloat(ShaderID_RL._Dissolve, 1f);
			renderer2.SetPropertyBlock(BossModeShiftController.m_matBlock_STATIC);
		}
		yield break;
	}

	// Token: 0x06000350 RID: 848 RVA: 0x00051494 File Offset: 0x0004F694
	public void Reset()
	{
		for (int i = 0; i < this.m_materialsToChangeFrom.Length; i++)
		{
			this.m_materialsToChangeFrom[i].material = this.m_initialMats[i];
		}
		foreach (Renderer renderer in this.m_renderersToDissolve)
		{
			renderer.GetPropertyBlock(BossModeShiftController.m_matBlock_STATIC);
			BossModeShiftController.m_matBlock_STATIC.SetFloat(ShaderID_RL._Dissolve, 0f);
			renderer.SetPropertyBlock(BossModeShiftController.m_matBlock_STATIC);
		}
	}

	// Token: 0x04000725 RID: 1829
	[SerializeField]
	private GameObject m_visuals;

	// Token: 0x04000726 RID: 1830
	[SerializeField]
	private Renderer[] m_materialsToChangeFrom;

	// Token: 0x04000727 RID: 1831
	[SerializeField]
	private Material[] m_materialsToChangeTo;

	// Token: 0x04000728 RID: 1832
	[SerializeField]
	private Renderer[] m_renderersToNotDissolve;

	// Token: 0x04000729 RID: 1833
	private Material[] m_initialMats;

	// Token: 0x0400072A RID: 1834
	private Renderer[] m_renderersToDissolve;

	// Token: 0x0400072B RID: 1835
	private static MaterialPropertyBlock m_matBlock_STATIC;
}
