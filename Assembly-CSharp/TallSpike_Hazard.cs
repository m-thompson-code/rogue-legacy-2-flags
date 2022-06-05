using System;
using System.Collections;
using Ferr;
using UnityEngine;

// Token: 0x02000750 RID: 1872
public class TallSpike_Hazard : Hazard, IFerr2DHazard, IHazard, IRootObj, IMirror
{
	// Token: 0x17001545 RID: 5445
	// (get) Token: 0x06003934 RID: 14644 RVA: 0x0001F69C File Offset: 0x0001D89C
	// (set) Token: 0x06003935 RID: 14645 RVA: 0x0001F6A4 File Offset: 0x0001D8A4
	public Ferr2DT_PathTerrain Ferr2D { get; private set; }

	// Token: 0x06003936 RID: 14646 RVA: 0x0001F6AD File Offset: 0x0001D8AD
	protected override void Awake()
	{
		base.Awake();
		this.m_hbController = base.GetComponentInChildren<IHitboxController>();
	}

	// Token: 0x06003937 RID: 14647 RVA: 0x000EA660 File Offset: 0x000E8860
	private void Start()
	{
		if (base.transform.localEulerAngles.z != 0f || base.transform.localScale.x != 1f || base.transform.localScale.y != 1f)
		{
			BaseRoom componentInParent = base.GetComponentInParent<BaseRoom>();
			string text = (componentInParent != null) ? componentInParent.gameObject.name : "NULL ROOM";
			Debug.LogFormat("<color=red>TallSpike Hazard {0} was not created correctly in room {1}.  Check transform values.</color>", new object[]
			{
				this.GetRoot(false).name,
				text
			});
		}
		if (Application.isPlaying)
		{
			CameraLayerController component = base.GetComponent<CameraLayerController>();
			Renderer component2 = base.GetComponent<Renderer>();
			component2.sortingLayerID = CameraLayerUtility.GetLayer(component.CameraLayer);
			component2.sortingOrder = CameraLayerUtility.GetSpriteOrderInLayer(component.CameraLayer, component.SubLayer);
		}
	}

	// Token: 0x06003938 RID: 14648 RVA: 0x000EA738 File Offset: 0x000E8938
	public override void Initialize(HazardArgs hazardArgs)
	{
		Ferr2DHazardArgs ferr2DHazardArgs = hazardArgs as Ferr2DHazardArgs;
		if (ferr2DHazardArgs == null)
		{
			return;
		}
		if (this.m_hbController == null)
		{
			throw new MissingComponentException("HitboxControllerLite");
		}
		this.Ferr2D = base.GetComponent<Ferr2DT_PathTerrain>();
		this.Ferr2D.transform.localPosition = ferr2DHazardArgs.LocalPosition;
		this.Ferr2D.ClearPoints();
		for (int i = 0; i < ferr2DHazardArgs.Points.Count; i++)
		{
			this.Ferr2D.AddPoint(ferr2DHazardArgs.Points[i], -1, PointType.Sharp);
		}
		this.Ferr2D.Build(true);
		if (Time.inFixedTimeStep)
		{
			base.Invoke("InitializeHitboxes", 0f);
			return;
		}
		this.InitializeHitboxes();
	}

	// Token: 0x06003939 RID: 14649 RVA: 0x0001F6C1 File Offset: 0x0001D8C1
	private void InitializeHitboxes()
	{
		this.m_hbController.Initialize();
	}

	// Token: 0x0600393A RID: 14650 RVA: 0x00002FCA File Offset: 0x000011CA
	public override void ResetHazard()
	{
	}

	// Token: 0x0600393B RID: 14651 RVA: 0x0001F6CE File Offset: 0x0001D8CE
	private void OnEnable()
	{
		if (this.m_flipColliders && !this.m_collidersFlipped)
		{
			base.StartCoroutine(this.MirrorTerrain());
		}
	}

	// Token: 0x0600393C RID: 14652 RVA: 0x0001F6ED File Offset: 0x0001D8ED
	public void Mirror()
	{
		if (!this.m_hbController.IsInitialized)
		{
			this.m_flipColliders = true;
		}
	}

	// Token: 0x0600393D RID: 14653 RVA: 0x0001F703 File Offset: 0x0001D903
	private IEnumerator MirrorTerrain()
	{
		yield return new WaitUntil(() => this.m_hbController.IsInitialized);
		Collider2D collider = this.m_hbController.GetCollider(HitboxType.Terrain);
		Collider2D collider2 = this.m_hbController.GetCollider(HitboxType.Platform);
		Collider2D component = base.GetComponent<PolygonCollider2D>();
		if (!(collider2 is PolygonCollider2D))
		{
			collider2.offset = new Vector2(-collider2.offset.x, collider2.offset.y);
			if (component != null)
			{
				UnityEngine.Object.Destroy(component);
			}
		}
		if (collider != null)
		{
			CDGHelper.CopyComponent<Collider2D>(collider2, collider.gameObject, false);
		}
		this.m_collidersFlipped = true;
		yield break;
	}

	// Token: 0x0600393F RID: 14655 RVA: 0x00003713 File Offset: 0x00001913
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04002DCA RID: 11722
	private bool m_flipColliders;

	// Token: 0x04002DCB RID: 11723
	private bool m_collidersFlipped;

	// Token: 0x04002DCC RID: 11724
	private IHitboxController m_hbController;
}
