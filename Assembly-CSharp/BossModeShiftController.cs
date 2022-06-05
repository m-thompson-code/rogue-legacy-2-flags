using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000098 RID: 152
public class BossModeShiftController : MonoBehaviour
{
	// Token: 0x06000289 RID: 649 RVA: 0x00012EE0 File Offset: 0x000110E0
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

	// Token: 0x0600028A RID: 650 RVA: 0x00012F7D File Offset: 0x0001117D
	private void OnEnable()
	{
		this.Reset();
	}

	// Token: 0x0600028B RID: 651 RVA: 0x00012F88 File Offset: 0x00011188
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

	// Token: 0x0600028C RID: 652 RVA: 0x00012FDA File Offset: 0x000111DA
	public void DissolveMaterials(float duration)
	{
		base.StopAllCoroutines();
		base.StartCoroutine(this.DissolveCoroutine(duration));
	}

	// Token: 0x0600028D RID: 653 RVA: 0x00012FF0 File Offset: 0x000111F0
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

	// Token: 0x0600028E RID: 654 RVA: 0x00013008 File Offset: 0x00011208
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

	// Token: 0x04000687 RID: 1671
	[SerializeField]
	private GameObject m_visuals;

	// Token: 0x04000688 RID: 1672
	[SerializeField]
	private Renderer[] m_materialsToChangeFrom;

	// Token: 0x04000689 RID: 1673
	[SerializeField]
	private Material[] m_materialsToChangeTo;

	// Token: 0x0400068A RID: 1674
	[SerializeField]
	private Renderer[] m_renderersToNotDissolve;

	// Token: 0x0400068B RID: 1675
	private Material[] m_initialMats;

	// Token: 0x0400068C RID: 1676
	private Renderer[] m_renderersToDissolve;

	// Token: 0x0400068D RID: 1677
	private static MaterialPropertyBlock m_matBlock_STATIC;
}
