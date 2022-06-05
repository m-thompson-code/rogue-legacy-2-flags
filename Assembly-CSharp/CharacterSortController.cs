using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200034F RID: 847
public class CharacterSortController : MonoBehaviour
{
	// Token: 0x17000CE9 RID: 3305
	// (get) Token: 0x06001B4C RID: 6988 RVA: 0x0000E2ED File Offset: 0x0000C4ED
	// (set) Token: 0x06001B4D RID: 6989 RVA: 0x0000E2F5 File Offset: 0x0000C4F5
	public int SortIndex { get; private set; }

	// Token: 0x17000CEA RID: 3306
	// (get) Token: 0x06001B4E RID: 6990 RVA: 0x0000E2FE File Offset: 0x0000C4FE
	// (set) Token: 0x06001B4F RID: 6991 RVA: 0x0000E306 File Offset: 0x0000C506
	public BaseCharacterController CharacterController { get; private set; }

	// Token: 0x17000CEB RID: 3307
	// (get) Token: 0x06001B50 RID: 6992 RVA: 0x0000E30F File Offset: 0x0000C50F
	public int SubLayer
	{
		get
		{
			return this.m_cameraLayerController.SubLayer;
		}
	}

	// Token: 0x17000CEC RID: 3308
	// (get) Token: 0x06001B51 RID: 6993 RVA: 0x0000E31C File Offset: 0x0000C51C
	// (set) Token: 0x06001B52 RID: 6994 RVA: 0x0000E324 File Offset: 0x0000C524
	public List<CharacterSortController> SortList { get; private set; } = new List<CharacterSortController>(5);

	// Token: 0x17000CED RID: 3309
	// (get) Token: 0x06001B53 RID: 6995 RVA: 0x0000E32D File Offset: 0x0000C52D
	// (set) Token: 0x06001B54 RID: 6996 RVA: 0x0000E335 File Offset: 0x0000C535
	public bool IsInitialized { get; private set; }

	// Token: 0x17000CEE RID: 3310
	// (get) Token: 0x06001B55 RID: 6997 RVA: 0x0000E33E File Offset: 0x0000C53E
	// (set) Token: 0x06001B56 RID: 6998 RVA: 0x0000E346 File Offset: 0x0000C546
	public bool IsPlayer { get; private set; }

	// Token: 0x06001B57 RID: 6999 RVA: 0x000950FC File Offset: 0x000932FC
	private void Awake()
	{
		this.CharacterController = base.GetComponentInParent<BaseCharacterController>();
		this.m_cameraLayerController = this.CharacterController.GetComponent<CameraLayerController>();
		this.IsPlayer = (this.CharacterController is PlayerController);
		this.m_collider = this.CharacterController.VisualBoundsObj.Collider;
		this.m_onPlayerEnterRoom = new Action<MonoBehaviour, EventArgs>(this.OnPlayerEnterRoom);
	}

	// Token: 0x06001B58 RID: 7000 RVA: 0x0000E34F File Offset: 0x0000C54F
	private IEnumerator Start()
	{
		this.m_collider.gameObject.layer = 29;
		this.m_collider.isTrigger = true;
		while (!CameraController.IsInstantiated)
		{
			yield return null;
		}
		while (!this.CharacterController.IsInitialized)
		{
			yield return null;
		}
		this.ResetCharacterLayers();
		this.UpdateSortDepth();
		this.IsInitialized = true;
		yield break;
	}

	// Token: 0x06001B59 RID: 7001 RVA: 0x00095164 File Offset: 0x00093364
	private void OnEnable()
	{
		this.m_collider.enabled = false;
		this.m_collider.enabled = true;
		if (!this.SortList.Contains(this))
		{
			this.SortList.Add(this);
		}
		if (this.IsPlayer)
		{
			Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
		}
	}

	// Token: 0x06001B5A RID: 7002 RVA: 0x000951B8 File Offset: 0x000933B8
	private void OnDisable()
	{
		this.SortList.Clear();
		this.SortIndex = 0;
		this.CharacterController.Visuals.transform.SetLocalPositionZ(0f);
		if (this.IsPlayer)
		{
			Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
		}
	}

	// Token: 0x06001B5B RID: 7003 RVA: 0x0000E35E File Offset: 0x0000C55E
	private void OnPlayerEnterRoom(object sender, EventArgs args)
	{
		this.SortList.Clear();
		if (!this.SortList.Contains(this))
		{
			this.SortList.Add(this);
		}
	}

	// Token: 0x06001B5C RID: 7004 RVA: 0x00095208 File Offset: 0x00093408
	private void OnTriggerEnter2D(Collider2D collider)
	{
		if (this.IsPlayer)
		{
			return;
		}
		CharacterSortController component = collider.GetComponent<CharacterSortController>();
		if (component && !this.SortList.Contains(component))
		{
			this.SortList.Add(component);
			this.UpdateSortList();
		}
	}

	// Token: 0x06001B5D RID: 7005 RVA: 0x00095250 File Offset: 0x00093450
	private void OnTriggerExit2D(Collider2D collider)
	{
		if (this.IsPlayer)
		{
			return;
		}
		CharacterSortController component = collider.GetComponent<CharacterSortController>();
		if (component && this.SortList.Contains(component))
		{
			this.SortList.Remove(component);
			if (component.SortIndex < this.SortIndex)
			{
				int sortIndex = this.SortIndex;
				this.SortIndex = sortIndex - 1;
			}
			if (this.SortList.Count <= 1)
			{
				this.SortIndex = 0;
			}
			this.UpdateSortList();
		}
	}

	// Token: 0x06001B5E RID: 7006 RVA: 0x000952CC File Offset: 0x000934CC
	public void UpdateSortList()
	{
		this.SortList.Sort(new Comparison<CharacterSortController>(this.SortBySubLayer));
		for (int i = 0; i < this.SortList.Count; i++)
		{
			CharacterSortController characterSortController = this.SortList[i];
			if (!characterSortController.IsPlayer)
			{
				if (i > characterSortController.SortIndex)
				{
					characterSortController.SortIndex = i;
				}
				else
				{
					bool flag = false;
					for (int j = 0; j < i; j++)
					{
						if (i == this.SortList[j].SortIndex)
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						characterSortController.SortIndex = i + 1;
					}
				}
				characterSortController.UpdateSortDepth();
			}
		}
	}

	// Token: 0x06001B5F RID: 7007 RVA: 0x00095368 File Offset: 0x00093568
	public void UpdateSortDepth()
	{
		float num;
		if (this.m_depthOverride == -1f)
		{
			num = (float)this.SortIndex * 1f;
		}
		else
		{
			num = (float)this.SortIndex * this.m_depthOverride;
		}
		if (this.IsPlayer)
		{
			num += -0.5f;
		}
		else
		{
			num += 0f;
		}
		if (this.CharacterController.Visuals)
		{
			this.CharacterController.Visuals.transform.SetLocalPositionZ(num);
		}
	}

	// Token: 0x06001B60 RID: 7008 RVA: 0x000953E4 File Offset: 0x000935E4
	private int SortBySubLayer(CharacterSortController a, CharacterSortController b)
	{
		if (a.SubLayer > b.SubLayer || a.IsPlayer)
		{
			return -1;
		}
		if (a.SubLayer < b.SubLayer || b.IsPlayer)
		{
			return 1;
		}
		if (a.SortIndex < b.SortIndex)
		{
			return -1;
		}
		if (a.SortIndex > b.SortIndex)
		{
			return 1;
		}
		return 0;
	}

	// Token: 0x06001B61 RID: 7009 RVA: 0x00095444 File Offset: 0x00093644
	public void ResetCharacterLayers()
	{
		int layer = 29;
		for (int i = 0; i < this.CharacterController.RendererArray.Count; i++)
		{
			this.CharacterController.RendererArray[i].gameObject.layer = layer;
		}
	}

	// Token: 0x0400194E RID: 6478
	private const float DEPTH_MULTIPLIER = 1f;

	// Token: 0x0400194F RID: 6479
	private const float PLAYER_STARTING_DEPTH_OFFSET = -0.5f;

	// Token: 0x04001950 RID: 6480
	private const float ENEMY_STARTING_DEPTH_OFFSET = 0f;

	// Token: 0x04001951 RID: 6481
	[SerializeField]
	private float m_depthOverride = -1f;

	// Token: 0x04001957 RID: 6487
	private Collider2D m_collider;

	// Token: 0x04001958 RID: 6488
	private CameraLayerController m_cameraLayerController;

	// Token: 0x04001959 RID: 6489
	private Action<MonoBehaviour, EventArgs> m_onPlayerEnterRoom;
}
