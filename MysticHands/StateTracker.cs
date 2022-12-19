using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000002 RID: 2
public class StateTracker
{
	// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
	public StateTracker()
	{
		this.states = new List<StateTracker.TrackedState>();
		this.children = new List<StateTracker.ChildState>();
		this.values = new List<StateTracker.ITrackedValue>();
		this.actions = new List<Action>();
	}

	// Token: 0x06000002 RID: 2 RVA: 0x00002088 File Offset: 0x00000288
	public StateTracker.TrackedValue<T> Value<T>(Func<T> definition)
	{
		StateTracker.TrackedValue<T> value = new StateTracker.TrackedValue<T>(definition);
		this.values.Add(value);
		return value;
	}

	// Token: 0x06000003 RID: 3 RVA: 0x000020B0 File Offset: 0x000002B0
	public StateTracker If(Func<bool> condition)
	{
		StateTracker child = new StateTracker();
		this.children.Add(new StateTracker.ChildState(condition, child));
		return child;
	}

	// Token: 0x06000004 RID: 4 RVA: 0x000020DC File Offset: 0x000002DC
	public StateTracker If(StateTracker.TrackedValue<bool> condition)
	{
		return this.If(() => condition);
	}

	// Token: 0x06000005 RID: 5 RVA: 0x00002108 File Offset: 0x00000308
	public StateTracker On(Func<bool> state, Action action, float cooldown = 0f)
	{
		return this.OnTrue(state, action, cooldown);
	}

	// Token: 0x06000006 RID: 6 RVA: 0x00002114 File Offset: 0x00000314
	public StateTracker On(Func<bool?> state, Action action, float cooldown = 0f)
	{
		return this.OnTrue(delegate()
		{
			bool? flag = state();
			bool flag2 = true;
			return (flag.GetValueOrDefault() == flag2) & (flag != null);
		}, action, cooldown);
	}

	// Token: 0x06000007 RID: 7 RVA: 0x00002144 File Offset: 0x00000344
	public StateTracker Do(Action action)
	{
		this.actions.Add(action);
		return this;
	}

	// Token: 0x06000008 RID: 8 RVA: 0x00002164 File Offset: 0x00000364
	public StateTracker While(Func<bool> condition, Action actionTrue, Action actionFalse = null)
	{
		this.If(condition).Do(actionTrue);
		bool flag = actionFalse != null;
		if (flag)
		{
			this.If(() => !condition()).Do(actionFalse);
		}
		return this;
	}

	// Token: 0x06000009 RID: 9 RVA: 0x000021B8 File Offset: 0x000003B8
	public StateTracker On(StateTracker.TrackedValue<bool> state, Action action, float cooldown = 0f)
	{
		return this.OnTrue(() => state, action, cooldown);
	}

	// Token: 0x0600000A RID: 10 RVA: 0x000021E6 File Offset: 0x000003E6
	public StateTracker Toggle(Func<bool> state, Action actionTrue, Action actionFalse, float cooldown = 0f)
	{
		return this.OnTrue(state, actionTrue, cooldown).OnFalse(state, actionFalse, cooldown);
	}

	// Token: 0x0600000B RID: 11 RVA: 0x000021FC File Offset: 0x000003FC
	public StateTracker Toggle(StateTracker.TrackedValue<bool> state, Action actionTrue, Action actionFalse, float cooldown = 0f)
	{
		return this.OnTrue(() => state, actionTrue, cooldown).OnFalse(() => state, actionFalse, cooldown);
	}

	// Token: 0x0600000C RID: 12 RVA: 0x00002240 File Offset: 0x00000440
	public StateTracker Else(Action action)
	{
		bool flag = this.states.Count > 0;
		StateTracker stateTracker;
		if (flag)
		{
			StateTracker.TrackedState lastState = this.states.LastOrDefault<StateTracker.TrackedState>();
			stateTracker = (lastState.trigger ? this.OnFalse(lastState.state, action, 0f) : this.OnTrue(lastState.state, action, 0f));
		}
		else
		{
			stateTracker = this;
		}
		return stateTracker;
	}

	// Token: 0x0600000D RID: 13 RVA: 0x000022A4 File Offset: 0x000004A4
	public StateTracker OnTrue(StateTracker.TrackedValue<bool> state, Action action, float cooldown = 0f)
	{
		return this.OnTrue(() => state, action, cooldown);
	}

	// Token: 0x0600000E RID: 14 RVA: 0x000022D4 File Offset: 0x000004D4
	public StateTracker OnFalse(StateTracker.TrackedValue<bool> state, Action action, float cooldown = 0f)
	{
		return this.OnFalse(() => state, action, cooldown);
	}

	// Token: 0x0600000F RID: 15 RVA: 0x00002304 File Offset: 0x00000504
	public StateTracker OnTrue(Func<bool> state, Action action, float cooldown = 0f)
	{
		StateTracker.TrackedState trackedState = new StateTracker.TrackedState(state, action, true, cooldown);
		this.states.Add(trackedState);
		return this;
	}

	// Token: 0x06000010 RID: 16 RVA: 0x00002330 File Offset: 0x00000530
	public StateTracker OnFalse(Func<bool> state, Action action, float cooldown = 0f)
	{
		StateTracker.TrackedState trackedState = new StateTracker.TrackedState(state, action, false, cooldown);
		this.states.Add(trackedState);
		return this;
	}

	// Token: 0x06000011 RID: 17 RVA: 0x0000235A File Offset: 0x0000055A
	public void Remove(StateTracker.TrackedState state)
	{
		this.states.Remove(state);
	}

	// Token: 0x06000012 RID: 18 RVA: 0x0000236C File Offset: 0x0000056C
	public void Update()
	{
		foreach (StateTracker.ITrackedValue value in this.values)
		{
			value.Update();
		}
		foreach (StateTracker.ChildState child in this.children)
		{
			bool flag = child.condition();
			if (flag)
			{
				child.child.Update();
			}
		}
		foreach (StateTracker.TrackedState state in this.states)
		{
			state.Evaluate();
		}
		foreach (Action action in this.actions)
		{
			action();
		}
	}

	// Token: 0x04000001 RID: 1
	private readonly List<StateTracker.TrackedState> states;

	// Token: 0x04000002 RID: 2
	private readonly List<StateTracker.ChildState> children;

	// Token: 0x04000003 RID: 3
	private readonly List<StateTracker.ITrackedValue> values;

	// Token: 0x04000004 RID: 4
	private readonly List<Action> actions;

	// Token: 0x0200000D RID: 13
	public class TrackedState
	{
		// Token: 0x060000E3 RID: 227 RVA: 0x00007BD6 File Offset: 0x00005DD6
		public TrackedState(Func<bool> state, Action action, bool trigger, float cooldown)
		{
			this.state = state;
			this.action = action;
			this.trigger = trigger;
			this.cooldown = cooldown;
			this.lastTrigger = 0f;
			this.lastState = state();
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00007C14 File Offset: 0x00005E14
		public void Evaluate()
		{
			bool newState = this.state();
			bool flag = (this.cooldown == 0f || Time.time - this.lastTrigger > this.cooldown) && this.lastState != newState && newState == this.trigger;
			if (flag)
			{
				this.lastTrigger = Time.time;
				this.action();
			}
			this.lastState = newState;
		}

		// Token: 0x0400003B RID: 59
		public Func<bool> state;

		// Token: 0x0400003C RID: 60
		public Action action;

		// Token: 0x0400003D RID: 61
		public readonly bool trigger;

		// Token: 0x0400003E RID: 62
		public readonly float cooldown;

		// Token: 0x0400003F RID: 63
		private bool lastState;

		// Token: 0x04000040 RID: 64
		private float lastTrigger;
	}

	// Token: 0x0200000E RID: 14
	public class ChildState
	{
		// Token: 0x060000E5 RID: 229 RVA: 0x00007C88 File Offset: 0x00005E88
		public ChildState(Func<bool> condition, StateTracker child)
		{
			this.condition = condition;
			this.child = child;
		}

		// Token: 0x04000041 RID: 65
		public Func<bool> condition;

		// Token: 0x04000042 RID: 66
		public StateTracker child;
	}

	// Token: 0x0200000F RID: 15
	public interface ITrackedValue
	{
		// Token: 0x060000E6 RID: 230
		void Update();
	}

	// Token: 0x02000010 RID: 16
	public class TrackedValue<T> : StateTracker.ITrackedValue
	{
		// Token: 0x060000E7 RID: 231 RVA: 0x00007CA0 File Offset: 0x00005EA0
		public static implicit operator T(StateTracker.TrackedValue<T> instance)
		{
			return instance.value;
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00007CA8 File Offset: 0x00005EA8
		public TrackedValue(Func<T> definition)
		{
			this.definition = definition;
			this.value = definition();
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00007CC5 File Offset: 0x00005EC5
		public void Update()
		{
			this.value = this.definition();
		}

		// Token: 0x04000043 RID: 67
		public T value;

		// Token: 0x04000044 RID: 68
		public Func<T> definition;
	}
}
