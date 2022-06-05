using System;
using UnityEngine;

// Token: 0x020003B6 RID: 950
public class HealthBar_RL : MonoBehaviour
{
	// Token: 0x06001F6A RID: 8042 RVA: 0x00010784 File Offset: 0x0000E984
	public float GetCurrentHP()
	{
		return this.m_currentHP;
	}

	// Token: 0x06001F6B RID: 8043 RVA: 0x0001078C File Offset: 0x0000E98C
	public void SetCurrentHP(float amount, bool additive)
	{
		if (additive)
		{
			this.m_currentHP += amount;
			return;
		}
		this.m_currentHP = amount;
	}

	// Token: 0x06001F6C RID: 8044 RVA: 0x000107A7 File Offset: 0x0000E9A7
	public float GetMaxHP()
	{
		return this.m_maxHP;
	}

	// Token: 0x06001F6D RID: 8045 RVA: 0x000107AF File Offset: 0x0000E9AF
	public void SetMaxHP(float amount)
	{
		this.m_maxHP = amount;
	}

	// Token: 0x06001F6E RID: 8046 RVA: 0x000107B8 File Offset: 0x0000E9B8
	private void Awake()
	{
		this.GenerateDebugUI();
	}

	// Token: 0x06001F6F RID: 8047 RVA: 0x000A2B60 File Offset: 0x000A0D60
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

	// Token: 0x06001F70 RID: 8048 RVA: 0x000A2DA0 File Offset: 0x000A0FA0
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

	// Token: 0x04001C06 RID: 7174
	private const float DEBUG_BAR_WIDTH = 2.5f;

	// Token: 0x04001C07 RID: 7175
	[Space(10f)]
	[SerializeField]
	private float m_currentHP;

	// Token: 0x04001C08 RID: 7176
	[SerializeField]
	private float m_maxHP = 1f;

	// Token: 0x04001C09 RID: 7177
	[Space(10f)]
	[SerializeField]
	private bool m_displayDebugUI = true;

	// Token: 0x04001C0A RID: 7178
	[SerializeField]
	private Vector2 m_UIOffset;

	// Token: 0x04001C0B RID: 7179
	private bool m_previousDisplayUI;

	// Token: 0x04001C0C RID: 7180
	private GameObject m_healthBarUI;

	// Token: 0x04001C0D RID: 7181
	private GameObject m_healthBarMaxHPUI;

	// Token: 0x04001C0E RID: 7182
	private GameObject m_healthBarCurrentHPUI;

	// Token: 0x04001C0F RID: 7183
	private GameObject m_healthBarScaler;
}
