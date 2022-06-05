using System;
using UnityEngine;

// Token: 0x020004E9 RID: 1257
public class ArenaRoomController : BaseSpecialRoomController
{
	// Token: 0x06002F17 RID: 12055 RVA: 0x000A0A58 File Offset: 0x0009EC58
	protected override void OnPlayerEnterRoom(object sender, RoomViaDoorEventArgs eventArgs)
	{
		base.OnPlayerEnterRoom(sender, eventArgs);
		Tunnel tunnel = this.m_tunnelSpawner.Tunnel;
		if (!base.IsRoomComplete && tunnel)
		{
			tunnel.SetIsLocked(false);
			tunnel.SetIsVisible(true);
			ObjectReferenceFinder component = tunnel.GameObject.GetComponent<ObjectReferenceFinder>();
			if (component)
			{
				component.GetObject("PropPointLight_Lantern").SetActive(true);
				component.GetObject("Warp").SetActive(true);
				component.GetObject("PortalGlowSprite").SetActive(true);
				component.GetObject("Particles").SetActive(true);
			}
			SummonRuleController componentInChildren = tunnel.Destination.Room.gameObject.GetComponentInChildren<SummonRuleController>();
			if (componentInChildren && componentInChildren.IsArenaComplete)
			{
				this.RoomCompleted();
			}
		}
		if (base.IsRoomComplete && tunnel)
		{
			ObjectReferenceFinder component2 = tunnel.GameObject.GetComponent<ObjectReferenceFinder>();
			if (component2)
			{
				component2.GetObject("PropPointLight_Lantern").SetActive(false);
				component2.GetObject("Warp").SetActive(false);
				component2.GetObject("PortalGlowSprite").SetActive(false);
				component2.GetObject("Particles").SetActive(false);
			}
			else
			{
				tunnel.SetIsVisible(false);
			}
			tunnel.SetIsLocked(true);
		}
	}

	// Token: 0x0400258A RID: 9610
	[SerializeField]
	private TunnelSpawnController m_tunnelSpawner;
}
