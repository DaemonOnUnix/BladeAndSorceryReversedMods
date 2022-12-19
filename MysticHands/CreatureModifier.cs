using System;
using System.Collections.Generic;
using ThunderRoad;
using UnityEngine;

// Token: 0x02000004 RID: 4
public class CreatureModifier : MonoBehaviour
{
	// Token: 0x06000033 RID: 51 RVA: 0x00003E20 File Offset: 0x00002020
	public void Awake()
	{
		this.creature = base.GetComponent<Creature>();
		this.handlers = new HashSet<object>();
		this.OnBegin();
	}

	// Token: 0x06000034 RID: 52 RVA: 0x00003E44 File Offset: 0x00002044
	public void AddHandler(object handler)
	{
		bool flag = !this.handlers.Contains(handler);
		if (flag)
		{
			this.handlers.Add(handler);
			this.Refresh();
		}
	}

	// Token: 0x06000035 RID: 53 RVA: 0x00003E7C File Offset: 0x0000207C
	public void RemoveHandler(object handler)
	{
		bool flag = this.handlers.Contains(handler);
		if (flag)
		{
			this.handlers.Remove(handler);
			this.Refresh();
		}
	}

	// Token: 0x06000036 RID: 54 RVA: 0x00003EB0 File Offset: 0x000020B0
	public void Refresh()
	{
		bool flag = this.handlers.Count == 0;
		if (flag)
		{
			bool flag2 = this.applied;
			if (flag2)
			{
				this.applied = false;
				this.OnRemove();
			}
		}
		else
		{
			bool flag3 = !this.applied;
			if (flag3)
			{
				this.applied = true;
				this.OnApply();
			}
		}
	}

	// Token: 0x06000037 RID: 55 RVA: 0x00003F0D File Offset: 0x0000210D
	public virtual void OnBegin()
	{
	}

	// Token: 0x06000038 RID: 56 RVA: 0x00003F10 File Offset: 0x00002110
	public virtual void OnRemove()
	{
	}

	// Token: 0x06000039 RID: 57 RVA: 0x00003F13 File Offset: 0x00002113
	public virtual void OnApply()
	{
	}

	// Token: 0x04000005 RID: 5
	public Creature creature;

	// Token: 0x04000006 RID: 6
	public HashSet<object> handlers;

	// Token: 0x04000007 RID: 7
	private bool applied;
}
