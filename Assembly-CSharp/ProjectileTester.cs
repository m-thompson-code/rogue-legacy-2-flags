using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200049F RID: 1183
public class ProjectileTester : MonoBehaviour
{
	// Token: 0x06002622 RID: 9762 RVA: 0x000152D6 File Offset: 0x000134D6
	private void OnEnable()
	{
		base.StartCoroutine(this.FireCoroutine());
	}

	// Token: 0x06002623 RID: 9763 RVA: 0x000152E5 File Offset: 0x000134E5
	private IEnumerator FireCoroutine()
	{
		float delay = this.m_initialDelay + Time.time;
		while (Time.time < delay)
		{
			yield return null;
		}
		Vector2 spawnOffset = new Vector2(1f, 1f);
		if (string.IsNullOrWhiteSpace(this.m_projectilePrefabName))
		{
			ProjectileEntry[] array = ProjectileLibrary.ProjectileEntryArray;
			for (int i = 0; i < array.Length; i++)
			{
				string name = array[i].ProjectilePrefab.name;
				Debug.Log("Firing projectile: " + name);
				ProjectileManager.Instance.AddProjectileToPool(name);
				ProjectileManager.FireProjectile(base.gameObject, name, spawnOffset, false, 0f, 1f, false, true, true, true);
				delay = this.m_fireInterval + Time.time;
				while (Time.time < delay)
				{
					yield return null;
				}
			}
			array = null;
		}
		else
		{
			bool flag = false;
			ProjectileEntry[] projectileEntryArray = ProjectileLibrary.ProjectileEntryArray;
			for (int j = 0; j < projectileEntryArray.Length; j++)
			{
				if (projectileEntryArray[j].ProjectilePrefab.name == this.m_projectilePrefabName)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				for (;;)
				{
					string projectilePrefabName = this.m_projectilePrefabName;
					ProjectileManager.Instance.AddProjectileToPool(projectilePrefabName);
					ProjectileManager.FireProjectile(base.gameObject, projectilePrefabName, spawnOffset, false, 0f, 1f, false, true, true, true);
					delay = this.m_fireInterval + Time.time;
					while (Time.time < delay)
					{
						yield return null;
					}
				}
			}
			else
			{
				Debug.Log("<color=red>Could not fire projectile: " + this.m_projectilePrefabName + ". Projectile not found in ProjectileLibrary.</color>");
			}
		}
		yield break;
	}

	// Token: 0x0400210C RID: 8460
	[SerializeField]
	private string m_projectilePrefabName;

	// Token: 0x0400210D RID: 8461
	[SerializeField]
	private float m_initialDelay = 1f;

	// Token: 0x0400210E RID: 8462
	[SerializeField]
	private float m_fireInterval = 0.25f;
}
