using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000602 RID: 1538
[Serializable]
public class DecoLocation
{
	// Token: 0x170013C6 RID: 5062
	// (get) Token: 0x060037D2 RID: 14290 RVA: 0x000BF10B File Offset: 0x000BD30B
	public float ChanceEmpty
	{
		get
		{
			return this.m_chanceEmpty;
		}
	}

	// Token: 0x170013C7 RID: 5063
	// (get) Token: 0x060037D3 RID: 14291 RVA: 0x000BF113 File Offset: 0x000BD313
	// (set) Token: 0x060037D4 RID: 14292 RVA: 0x000BF11B File Offset: 0x000BD31B
	public DecoController DecoController { get; private set; }

	// Token: 0x170013C8 RID: 5064
	// (get) Token: 0x060037D5 RID: 14293 RVA: 0x000BF124 File Offset: 0x000BD324
	// (set) Token: 0x060037D6 RID: 14294 RVA: 0x000BF12C File Offset: 0x000BD32C
	public Deco DecoInstance { get; set; }

	// Token: 0x170013C9 RID: 5065
	// (get) Token: 0x060037D7 RID: 14295 RVA: 0x000BF135 File Offset: 0x000BD335
	// (set) Token: 0x060037D8 RID: 14296 RVA: 0x000BF13D File Offset: 0x000BD33D
	public Deco DecoPrefab { get; set; }

	// Token: 0x170013CA RID: 5066
	// (get) Token: 0x060037D9 RID: 14297 RVA: 0x000BF146 File Offset: 0x000BD346
	// (set) Token: 0x060037DA RID: 14298 RVA: 0x000BF14E File Offset: 0x000BD34E
	public int Index { get; set; }

	// Token: 0x170013CB RID: 5067
	// (get) Token: 0x060037DB RID: 14299 RVA: 0x000BF157 File Offset: 0x000BD357
	// (set) Token: 0x060037DC RID: 14300 RVA: 0x000BF15F File Offset: 0x000BD35F
	public bool IsFlipped { get; set; }

	// Token: 0x170013CC RID: 5068
	// (get) Token: 0x060037DD RID: 14301 RVA: 0x000BF168 File Offset: 0x000BD368
	public Vector2 Position
	{
		get
		{
			return this.m_position;
		}
	}

	// Token: 0x170013CD RID: 5069
	// (get) Token: 0x060037DE RID: 14302 RVA: 0x000BF170 File Offset: 0x000BD370
	public DecoEntry[] PotentialDecos
	{
		get
		{
			return this.m_potentialDecos;
		}
	}

	// Token: 0x060037DF RID: 14303 RVA: 0x000BF178 File Offset: 0x000BD378
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

	// Token: 0x060037E0 RID: 14304 RVA: 0x000BF1A2 File Offset: 0x000BD3A2
	public void ReturnDecoToPool()
	{
		if (this.DecoInstance)
		{
			this.DecoInstance.gameObject.SetActive(false);
			this.DecoInstance.gameObject.transform.SetParent(null);
			this.DecoInstance = null;
		}
	}

	// Token: 0x060037E1 RID: 14305 RVA: 0x000BF1E0 File Offset: 0x000BD3E0
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

	// Token: 0x060037E2 RID: 14306 RVA: 0x000BF443 File Offset: 0x000BD643
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

	// Token: 0x060037E3 RID: 14307 RVA: 0x000BF459 File Offset: 0x000BD659
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

	// Token: 0x060037E4 RID: 14308 RVA: 0x000BF468 File Offset: 0x000BD668
	public void SetDecoController(DecoController decoController)
	{
		this.DecoController = decoController;
	}

	// Token: 0x04002AC3 RID: 10947
	[SerializeField]
	[Range(0f, 1f)]
	private float m_chanceEmpty;

	// Token: 0x04002AC4 RID: 10948
	[SerializeField]
	private Vector2 m_position;

	// Token: 0x04002AC5 RID: 10949
	[SerializeField]
	private DecoEntry[] m_potentialDecos;

	// Token: 0x04002AC6 RID: 10950
	private Vector2 m_parentPositionOffset;
}
