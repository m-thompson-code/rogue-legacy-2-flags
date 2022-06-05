using System;
using UnityEngine;

// Token: 0x0200048C RID: 1164
public class OffscreenIconObj : MonoBehaviour, IGenericPoolObj
{
	// Token: 0x1700108B RID: 4235
	// (get) Token: 0x06002AEC RID: 10988 RVA: 0x0009155D File Offset: 0x0008F75D
	// (set) Token: 0x06002AED RID: 10989 RVA: 0x00091565 File Offset: 0x0008F765
	public bool IsFreePoolObj { get; set; }

	// Token: 0x1700108C RID: 4236
	// (get) Token: 0x06002AEE RID: 10990 RVA: 0x0009156E File Offset: 0x0008F76E
	// (set) Token: 0x06002AEF RID: 10991 RVA: 0x00091576 File Offset: 0x0008F776
	public bool IsAwakeCalled { get; protected set; } = true;

	// Token: 0x06002AF0 RID: 10992 RVA: 0x00091580 File Offset: 0x0008F780
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

	// Token: 0x06002AF1 RID: 10993 RVA: 0x000915F0 File Offset: 0x0008F7F0
	private void OnEnable()
	{
		if (CameraController.IsInstantiated && !base.transform.parent)
		{
			base.transform.SetParent(CameraController.GameCamera.transform);
		}
	}

	// Token: 0x06002AF2 RID: 10994 RVA: 0x00091620 File Offset: 0x0008F820
	private void OnDisable()
	{
		this.m_offscreenObj = null;
		DisablePooledObjectManager.DisablePooledObject(this, false);
	}

	// Token: 0x06002AF3 RID: 10995 RVA: 0x00091630 File Offset: 0x0008F830
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

	// Token: 0x06002AF4 RID: 10996 RVA: 0x00091B1D File Offset: 0x0008FD1D
	public void ResetValues()
	{
	}

	// Token: 0x06002AF6 RID: 10998 RVA: 0x00091B2E File Offset: 0x0008FD2E
	GameObject IGenericPoolObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04002306 RID: 8966
	private const float ENEMY_BOUNDS_CHECK_BUFFER = 0.5f;

	// Token: 0x04002307 RID: 8967
	[SerializeField]
	private SpriteRenderer m_sprite;

	// Token: 0x04002308 RID: 8968
	[SerializeField]
	private SpriteRenderer m_enemySprite;

	// Token: 0x04002309 RID: 8969
	[SerializeField]
	private GameObject m_bg;

	// Token: 0x0400230A RID: 8970
	[SerializeField]
	private GameObject m_iconGO;

	// Token: 0x0400230B RID: 8971
	private IOffscreenObj m_offscreenObj;

	// Token: 0x0400230C RID: 8972
	private bool m_isEnemy;
}
