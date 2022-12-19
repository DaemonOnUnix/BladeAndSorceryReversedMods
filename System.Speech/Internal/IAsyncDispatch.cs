using System;

namespace System.Speech.Internal
{
	// Token: 0x0200000A RID: 10
	internal interface IAsyncDispatch
	{
		// Token: 0x06000012 RID: 18
		void Post(object evt);

		// Token: 0x06000013 RID: 19
		void Post(object[] evt);

		// Token: 0x06000014 RID: 20
		void PostOperation(Delegate callback, params object[] parameters);
	}
}
