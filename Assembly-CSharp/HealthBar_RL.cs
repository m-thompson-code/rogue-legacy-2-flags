using System;
using UnityEngine;

// Token: 0x02000205 RID: 517
public class HealthBar_RL : MonoBehaviour
{
	// Token: 0x060015DA RID: 5594 RVA: 0x000440C5 File Offset: 0x000422C5
	public float GetCurrentHP()
	{
		return this.m_currentHP;
	}

	// Token: 0x060015DB RID: 5595 RVA: 0x000440CD File Offset: 0x000422CD
	public void SetCurrentHP(float amount, bool additive)
	{
		if (additive)
		{
			this.m_currentHP += amount;
			return;
		}
		this.m_currentHP = amount;
	}

	// Token: 0x060015DC RID: 5596 RVA: 0x000440E8 File Offset: 0x000422E8
	public float GetMaxHP()
	{
		return this.m_maxHP;
	}

	// Token: 0x060015DD RID: 5597 RVA: 0x000440F0 File Offset: 0x000422F0
	public void SetMaxHP(float amount)
	{
		this.m_maxHP = amount;
	}

	// Token: 0x060015DE RID: 5598 RVA: 0x000440F9 File Offset: 0x000422F9
	private void Awake()
	{
		this.GenerateDebugUI();
	}

	// Token: 0x060015DF RID: 5599 RVA: 0x00044104 File Offset: 0x00042304
	private void GenerateDebugUI()
	{
		this.m_healthBarUI = new GameObject();
		this.m_healthBarUI.name = "Health Bar Debug UI";
		this.m_healthBarUI.transform.SetParent(base.transform);
		this.m_healthBarUI.transform.localPosition = new Vector3(this.m_UIOffset.x, this.m_UIOffset.y, 0f);
		this.m_healthBarMaxHPUI = GameObject.CreatePrimitive(PrimitiveType.Cube);
		this.m_healthBarMaxHPUI.transform.SetParent(this.m_healthBarUI.transform);
		this.m_healthBarMaxHPUI.transform.localScale = new Vector3(2.5f, 0.25f, 1f);
		this.m_healthBarMaxHPUI.transform.localPosition = Vector3.zero;
		Renderer component = this.m_healthBarMaxHPUI.GetComponent<Renderer>();
		component.sortingLayerName = "Player";
		component.sortingOrder = 0;
		component.material.shader = Shader.Find("UI/Unlit/Transparent");
		component.material.color = Color.red;
		Collider component2 = this.m_healthBarMaxHPUI.GetComponent<Collider>();
		if (component2 != null)
		{
			UnityEngine.Object.Destroy(component2);
		}
		this.m_healthBarScaler = new GameObject();
		this.m_healthBarScaler.transform.SetParent(this.m_healthBarUI.transform);
		this.m_healthBarScaler.transform.localPosition = new Vector3(-1.25f, 0f, 0f);
		this.m_healthBarCurrentHPUI = GameObject.CreatePrimitive(PrimitiveType.Cube);
		this.m_healthBarCurrentHPUI.transform.SetParent(this.m_healthBarScaler.transform);
		this.m_healthBarCurrentHPUI.transform.localScale = new Vector3(2.5f, 0.25f, 1f);
		this.m_healthBarCurrentHPUI.transform.localPosition = new Vector3(1.25f, 0f, 0f);
		Renderer component3 = this.m_healthBarCurrentHPUI.GetComponent<Renderer>();
		component3.sortingLayerName = "Player";
		component3.sortingOrder = 1;
		component3.material.shader = Shader.Find("UI/Unlit/Transparent");
		component3.material.color = Color.green;
		component2 = this.m_healthBarCurrentHPUI.GetComponent<Collider>();
		if (component2 != null)
		{
			UnityEngine.Object.Destroy(component2);
		}
	}

	// Token: 0x060015E0 RID: 5600 RVA: 0x00044344 File Offset: 0x00042544
	private void Update()
	{
		if (Application.isPlaying)
		{
			if (this.m_previousDisplayUI != this.m_displayDebugUI && this.m_displayDebugUI)
			{
				this.m_healthBarUI.SetActive(true);
			}
			else if (this.m_previousDisplayUI != this.m_displayDebugUI && !this.m_displayDebugUI)
			{
				this.m_healthBarUI.SetActive(false);
			}
			this.m_previousDisplayUI = this.m_displayDebugUI;
		}
		if (this.m_maxHP < 1f)
		{
			this.m_maxHP = 1f;
		}
		if (this.m_currentHP > this.m_maxHP)
		{
			this.m_currentHP = this.m_maxHP;
		}
		else if (this.m_currentHP <= 0f)
		{
			this.m_currentHP = 0f;
		}
		this.m_healthBarUI.transform.localPosition = new Vector3(this.m_UIOffset.x, this.m_UIOffset.y, 0f);
		this.m_healthBarScaler.transform.localScale = new Vector3(this.m_currentHP / this.m_maxHP, 1f, 1f);
	}

	// Token: 0x04001509 RID: 5385
	private const float DEBUG_BAR_WIDTH = 2.5f;

	// Token: 0x0400150A RID: 5386
	[Space(10f)]
	[SerializeField]
	private float m_currentHP;

	// Token: 0x0400150B RID: 5387
	[SerializeField]
	private float m_maxHP = 1f;

	// Token: 0x0400150C RID: 5388
	[Space(10f)]
	[SerializeField]
	private bool m_displayDebugUI = true;

	// Token: 0x0400150D RID: 5389
	[SerializeField]
	private Vector2 m_UIOffset;

	// Token: 0x0400150E RID: 5390
	private bool m_previousDisplayUI;

	// Token: 0x0400150F RID: 5391
	private GameObject m_healthBarUI;

	// Token: 0x04001510 RID: 5392
	private GameObject m_healthBarMaxHPUI;

	// Token: 0x04001511 RID: 5393
	private GameObject m_healthBarCurrentHPUI;

	// Token: 0x04001512 RID: 5394
	private GameObject m_healthBarScaler;
}
