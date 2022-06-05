using System;
using UnityEngine;

// Token: 0x0200078D RID: 1933
public class OffscreenIconObj : MonoBehaviour, IGenericPoolObj
{
	// Token: 0x170015CE RID: 5582
	// (get) Token: 0x06003B29 RID: 15145 RVA: 0x000207A9 File Offset: 0x0001E9A9
	// (set) Token: 0x06003B2A RID: 15146 RVA: 0x000207B1 File Offset: 0x0001E9B1
	public bool IsFreePoolObj { get; set; }

	// Token: 0x170015CF RID: 5583
	// (get) Token: 0x06003B2B RID: 15147 RVA: 0x000207BA File Offset: 0x0001E9BA
	// (set) Token: 0x06003B2C RID: 15148 RVA: 0x000207C2 File Offset: 0x0001E9C2
	public bool IsAwakeCalled { get; protected set; } = true;

	// Token: 0x06003B2D RID: 15149 RVA: 0x000F309C File Offset: 0x000F129C
	public void AttachOffscreenObj(IOffscreenObj offscreenObj, bool isEnemy)
	{
		if (isEnemy)
		{
			this.m_sprite.gameObject.SetActive(false);
			this.m_enemySprite.gameObject.SetActive(true);
		}
		else
		{
			this.m_sprite.gameObject.SetActive(true);
			this.m_enemySprite.gameObject.SetActive(false);
		}
		this.m_isEnemy = isEnemy;
		this.m_offscreenObj = offscreenObj;
		this.m_iconGO.SetActive(false);
	}

	// Token: 0x06003B2E RID: 15150 RVA: 0x000207CB File Offset: 0x0001E9CB
	private void OnEnable()
	{
		if (CameraController.IsInstantiated && !base.transform.parent)
		{
			base.transform.SetParent(CameraController.GameCamera.transform);
		}
	}

	// Token: 0x06003B2F RID: 15151 RVA: 0x000207FB File Offset: 0x0001E9FB
	private void OnDisable()
	{
		this.m_offscreenObj = null;
		DisablePooledObjectManager.DisablePooledObject(this, false);
	}

	// Token: 0x06003B30 RID: 15152 RVA: 0x000F310C File Offset: 0x000F130C
	private void FixedUpdate()
	{
		if (!PlayerManager.IsInstantiated)
		{
			return;
		}
		bool flag = this.m_offscreenObj.IsNativeNull();
		if (flag || !this.m_offscreenObj.gameObject.activeSelf || TraitManager.IsTraitActive(TraitType.NoProjectileIndicators))
		{
			if (base.gameObject.activeSelf)
			{
				base.gameObject.SetActive(false);
			}
			return;
		}
		if (!flag && this.m_isEnemy)
		{
			EnemyController enemyController = this.m_offscreenObj as EnemyController;
			if (enemyController && enemyController.DisableOffscreenWarnings)
			{
				if (base.gameObject.activeSelf)
				{
					base.gameObject.SetActive(false);
				}
				return;
			}
		}
		float num = 1f;
		float orthographicSize = CameraController.GameCamera.orthographicSize;
		float num2 = orthographicSize * (float)Screen.width / (float)Screen.height;
		Bounds bounds = new Bounds(CameraController.GameCamera.transform.localPosition, new Vector3(num2 * 2f, orthographicSize * 2f, 999f));
		PlayerController playerController = PlayerManager.GetPlayerController();
		bool flag2 = false;
		if (!this.m_offscreenObj.DisableOffscreenWarnings)
		{
			if (!this.m_isEnemy)
			{
				bool flag3 = bounds.Contains(this.m_offscreenObj.Midpoint);
				bool flag4 = false;
				if (!flag3)
				{
					Vector2 velocity = this.m_offscreenObj.Velocity;
					Vector2 vector = this.m_offscreenObj.gameObject.transform.localPosition;
					Vector2 vector2 = playerController.Midpoint;
					bool flag5 = vector.y < bounds.max.y && vector.y > bounds.min.y;
					bool flag6 = vector.x > bounds.min.x && vector.x < bounds.max.x;
					float num3 = 0.01f;
					bool flag7 = (velocity.x > num3 && vector.x < vector2.x) || (velocity.x < -num3 && vector.x > vector2.x);
					bool flag8 = (velocity.y > num3 && vector.y < vector2.y) || (velocity.y < -num3 && vector.y > vector2.y);
					if ((flag7 && flag8) || (flag7 && flag5) || (flag8 && flag6))
					{
						flag4 = true;
					}
				}
				flag2 = (!flag3 && flag4);
			}
			else
			{
				EnemyController enemyController2 = this.m_offscreenObj as EnemyController;
				if (enemyController2)
				{
					float num4 = CameraController.GameCamera.orthographicSize * 2f;
					float num5 = num4 * (float)Screen.width / (float)Screen.height;
					Bounds bounds2 = new Bounds(CameraController.GameCamera.transform.localPosition, new Vector3(num5 * 2f, num4 * 2f, 999f));
					flag2 = (!bounds.Intersects(enemyController2.VisualBounds) && bounds2.Intersects(enemyController2.VisualBounds) && !enemyController2.IsDead && !enemyController2.IsBeingSummoned);
				}
			}
		}
		if (!flag2 && this.m_iconGO.gameObject.activeSelf)
		{
			this.m_iconGO.gameObject.SetActive(false);
		}
		else if (flag2 && !this.m_iconGO.gameObject.activeSelf)
		{
			this.m_iconGO.gameObject.SetActive(true);
		}
		if (this.m_iconGO.gameObject.activeSelf)
		{
			Vector3 midpoint = this.m_offscreenObj.Midpoint;
			Vector3 vector3 = midpoint - CameraController.GameCamera.gameObject.transform.localPosition;
			float num6 = bounds.min.x + num;
			float num7 = bounds.max.x - num;
			float num8 = bounds.min.y + num;
			float num9 = bounds.max.y - num;
			if (midpoint.x <= num6)
			{
				vector3.x = -num2 + num;
			}
			else if (midpoint.x > num7)
			{
				vector3.x = num2 - num;
			}
			if (midpoint.y <= num8)
			{
				vector3.y = -orthographicSize + num;
			}
			else if (midpoint.y > num9)
			{
				vector3.y = orthographicSize - num;
			}
			float z = 1f + (midpoint.y - bounds.min.y) / (bounds.max.y - bounds.min.y);
			vector3.z = z;
			base.transform.localPosition = vector3;
			float z2 = CDGHelper.AngleBetweenPts(vector3 + CameraController.GameCamera.gameObject.transform.localPosition, this.m_offscreenObj.Midpoint);
			Vector3 localEulerAngles = this.m_bg.transform.localEulerAngles;
			localEulerAngles.z = z2;
			this.m_bg.transform.localEulerAngles = localEulerAngles;
		}
	}

	// Token: 0x06003B31 RID: 15153 RVA: 0x00002FCA File Offset: 0x000011CA
	public void ResetValues()
	{
	}

	// Token: 0x06003B33 RID: 15155 RVA: 0x00003713 File Offset: 0x00001913
	GameObject IGenericPoolObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04002F11 RID: 12049
	private const float ENEMY_BOUNDS_CHECK_BUFFER = 0.5f;

	// Token: 0x04002F12 RID: 12050
	[SerializeField]
	private SpriteRenderer m_sprite;

	// Token: 0x04002F13 RID: 12051
	[SerializeField]
	private SpriteRenderer m_enemySprite;

	// Token: 0x04002F14 RID: 12052
	[SerializeField]
	private GameObject m_bg;

	// Token: 0x04002F15 RID: 12053
	[SerializeField]
	private GameObject m_iconGO;

	// Token: 0x04002F16 RID: 12054
	private IOffscreenObj m_offscreenObj;

	// Token: 0x04002F17 RID: 12055
	private bool m_isEnemy;
}
