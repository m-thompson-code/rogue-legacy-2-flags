using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000A21 RID: 2593
[Serializable]
public class DecoLocation
{
	// Token: 0x17001B19 RID: 6937
	// (get) Token: 0x06004E63 RID: 20067 RVA: 0x0002AA88 File Offset: 0x00028C88
	public float ChanceEmpty
	{
		get
		{
			return this.m_chanceEmpty;
		}
	}

	// Token: 0x17001B1A RID: 6938
	// (get) Token: 0x06004E64 RID: 20068 RVA: 0x0002AA90 File Offset: 0x00028C90
	// (set) Token: 0x06004E65 RID: 20069 RVA: 0x0002AA98 File Offset: 0x00028C98
	public DecoController DecoController { get; private set; }

	// Token: 0x17001B1B RID: 6939
	// (get) Token: 0x06004E66 RID: 20070 RVA: 0x0002AAA1 File Offset: 0x00028CA1
	// (set) Token: 0x06004E67 RID: 20071 RVA: 0x0002AAA9 File Offset: 0x00028CA9
	public Deco DecoInstance { get; set; }

	// Token: 0x17001B1C RID: 6940
	// (get) Token: 0x06004E68 RID: 20072 RVA: 0x0002AAB2 File Offset: 0x00028CB2
	// (set) Token: 0x06004E69 RID: 20073 RVA: 0x0002AABA File Offset: 0x00028CBA
	public Deco DecoPrefab { get; set; }

	// Token: 0x17001B1D RID: 6941
	// (get) Token: 0x06004E6A RID: 20074 RVA: 0x0002AAC3 File Offset: 0x00028CC3
	// (set) Token: 0x06004E6B RID: 20075 RVA: 0x0002AACB File Offset: 0x00028CCB
	public int Index { get; set; }

	// Token: 0x17001B1E RID: 6942
	// (get) Token: 0x06004E6C RID: 20076 RVA: 0x0002AAD4 File Offset: 0x00028CD4
	// (set) Token: 0x06004E6D RID: 20077 RVA: 0x0002AADC File Offset: 0x00028CDC
	public bool IsFlipped { get; set; }

	// Token: 0x17001B1F RID: 6943
	// (get) Token: 0x06004E6E RID: 20078 RVA: 0x0002AAE5 File Offset: 0x00028CE5
	public Vector2 Position
	{
		get
		{
			return this.m_position;
		}
	}

	// Token: 0x17001B20 RID: 6944
	// (get) Token: 0x06004E6F RID: 20079 RVA: 0x0002AAED File Offset: 0x00028CED
	public DecoEntry[] PotentialDecos
	{
		get
		{
			return this.m_potentialDecos;
		}
	}

	// Token: 0x06004E70 RID: 20080 RVA: 0x0002AAF5 File Offset: 0x00028CF5
	public void Initialize(DecoController decoController, Deco decoPrefab, bool isFlipped)
	{
		this.DecoController = decoController;
		this.DecoPrefab = decoPrefab;
		if (this.DecoPrefab)
		{
			this.IsFlipped = isFlipped;
			this.SetDecoPropInstance();
		}
	}

	// Token: 0x06004E71 RID: 20081 RVA: 0x0002AB1F File Offset: 0x00028D1F
	public void ReturnDecoToPool()
	{
		if (this.DecoInstance)
		{
			this.DecoInstance.gameObject.SetActive(false);
			this.DecoInstance.gameObject.transform.SetParent(null);
			this.DecoInstance = null;
		}
	}

	// Token: 0x06004E72 RID: 20082 RVA: 0x0012D8E0 File Offset: 0x0012BAE0
	private void SetDecoPropInstance()
	{
		if (this.DecoPrefab)
		{
			this.m_parentPositionOffset = Vector2.zero;
			this.DecoInstance = DecoManager.GetDeco(this.DecoPrefab.NameHash);
			if (this.DecoInstance)
			{
				this.DecoInstance.gameObject.SetActive(true);
				Prop[] props = this.DecoInstance.Props;
				for (int i = 0; i < props.Length; i++)
				{
					props[i].SetRoom(this.DecoController.OwnerProp.Room);
				}
				Transform transform = (this.DecoController.OwnerProp.CameraLayerController as CameraLayerController).Visuals.transform;
				Vector2 a = this.Position;
				if (this.DecoController.OwnerProp.Pivot.transform.localEulerAngles.z != 0f)
				{
					a = CDGHelper.RotatedPoint(this.Position, Vector2.zero, this.DecoController.OwnerProp.Pivot.transform.localEulerAngles.z);
				}
				this.DecoInstance.gameObject.transform.localPosition = a * transform.transform.lossyScale + transform.position;
				this.DecoInstance.SetCameraLayerControllers(this.DecoController.OwnerProp);
				Vector3 vector = this.DecoInstance.gameObject.transform.localScale;
				if (this.IsFlipped)
				{
					vector.x = Mathf.Abs(vector.x) * -1f;
				}
				else
				{
					vector.x = Mathf.Abs(vector.x);
				}
				vector = Vector3.Scale(vector, transform.transform.lossyScale);
				this.DecoInstance.gameObject.transform.localScale = vector;
				this.DecoInstance.gameObject.transform.rotation = this.DecoController.OwnerProp.Pivot.transform.rotation;
				this.m_parentPositionOffset = this.DecoInstance.transform.localPosition - transform.position;
				if (this.DecoController.OwnerProp.CorgiController)
				{
					this.DecoController.StartCoroutine(this.FollowParentPositionCoroutine());
					return;
				}
			}
		}
		else
		{
			this.DecoPrefab = null;
			this.DecoInstance = null;
		}
	}

	// Token: 0x06004E73 RID: 20083 RVA: 0x0002AB5C File Offset: 0x00028D5C
	private IEnumerator AttachToParentCoroutine(Transform parent)
	{
		if (this.DecoController.OwnerProp.HitboxController != null)
		{
			while (!this.DecoController.OwnerProp.HitboxController.ResponseMethodsInitialized)
			{
				yield return null;
			}
		}
		if (this.DecoInstance)
		{
			bool hbInitSuccessful = true;
			foreach (Prop prop in this.DecoInstance.Props)
			{
				if (prop.HitboxController != null)
				{
					while (!prop.HitboxController.ResponseMethodsInitialized)
					{
						if (!prop.gameObject.activeSelf)
						{
							hbInitSuccessful = false;
							yield break;
						}
						yield return null;
					}
				}
				prop = null;
			}
			Prop[] array = null;
			if (hbInitSuccessful)
			{
				this.DecoInstance.gameObject.transform.SetParent(parent, true);
			}
		}
		yield break;
	}

	// Token: 0x06004E74 RID: 20084 RVA: 0x0002AB72 File Offset: 0x00028D72
	private IEnumerator FollowParentPositionCoroutine()
	{
		while (this.DecoController.OwnerProp && !this.DecoInstance.IsNativeNull())
		{
			Vector3 localPosition = this.DecoController.OwnerProp.transform.position + this.m_parentPositionOffset;
			localPosition.z = this.DecoInstance.transform.localPosition.z;
			this.DecoInstance.transform.localPosition = localPosition;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06004E75 RID: 20085 RVA: 0x0002AB81 File Offset: 0x00028D81
	public void SetDecoController(DecoController decoController)
	{
		this.DecoController = decoController;
	}

	// Token: 0x04003B1B RID: 15131
	[SerializeField]
	[Range(0f, 1f)]
	private float m_chanceEmpty;

	// Token: 0x04003B1C RID: 15132
	[SerializeField]
	private Vector2 m_position;

	// Token: 0x04003B1D RID: 15133
	[SerializeField]
	private DecoEntry[] m_potentialDecos;

	// Token: 0x04003B1E RID: 15134
	private Vector2 m_parentPositionOffset;
}
