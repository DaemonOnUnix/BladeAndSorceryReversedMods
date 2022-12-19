using System;
using System.Runtime.InteropServices;

namespace MonoMod.RuntimeDetour.Platforms
{
	// Token: 0x02000472 RID: 1138
	internal class DetourNativeMonoPosixPlatform : IDetourNativePlatform
	{
		// Token: 0x06001891 RID: 6289 RVA: 0x00055661 File Offset: 0x00053861
		public DetourNativeMonoPosixPlatform(IDetourNativePlatform inner)
		{
			this.Inner = inner;
			this._Pagesize = DetourNativeMonoPosixPlatform.sysconf(DetourNativeMonoPosixPlatform.SysconfName._SC_PAGESIZE, (DetourNativeMonoPosixPlatform.Errno)0);
		}

		// Token: 0x06001892 RID: 6290 RVA: 0x00055680 File Offset: 0x00053880
		private static string GetLastError(string name)
		{
			int num = DetourNativeMonoPosixPlatform._GetLastError();
			DetourNativeMonoPosixPlatform.Errno errno;
			if (DetourNativeMonoPosixPlatform.ToErrno(num, out errno) == 0)
			{
				return string.Format("{0} returned {1}", name, errno);
			}
			return string.Format("{0} returned 0x${1:X8}", name, num);
		}

		// Token: 0x06001893 RID: 6291 RVA: 0x000556C0 File Offset: 0x000538C0
		private void SetMemPerms(IntPtr start, ulong len, DetourNativeMonoPosixPlatform.MmapProts prot)
		{
			long pagesize = this._Pagesize;
			long num = (long)start & ~(pagesize - 1L);
			long num2 = ((long)start + (long)len + pagesize - 1L) & ~(pagesize - 1L);
			if (DetourNativeMonoPosixPlatform.mprotect((IntPtr)num, (ulong)(num2 - num), prot) != 0)
			{
				throw new Exception(DetourNativeMonoPosixPlatform.GetLastError("mprotect"));
			}
		}

		// Token: 0x06001894 RID: 6292 RVA: 0x00055716 File Offset: 0x00053916
		public void MakeWritable(IntPtr src, uint size)
		{
			this.SetMemPerms(src, (ulong)size, DetourNativeMonoPosixPlatform.MmapProts.PROT_READ | DetourNativeMonoPosixPlatform.MmapProts.PROT_WRITE | DetourNativeMonoPosixPlatform.MmapProts.PROT_EXEC);
		}

		// Token: 0x06001895 RID: 6293 RVA: 0x00055716 File Offset: 0x00053916
		public void MakeExecutable(IntPtr src, uint size)
		{
			this.SetMemPerms(src, (ulong)size, DetourNativeMonoPosixPlatform.MmapProts.PROT_READ | DetourNativeMonoPosixPlatform.MmapProts.PROT_WRITE | DetourNativeMonoPosixPlatform.MmapProts.PROT_EXEC);
		}

		// Token: 0x06001896 RID: 6294 RVA: 0x00055716 File Offset: 0x00053916
		public void MakeReadWriteExecutable(IntPtr src, uint size)
		{
			this.SetMemPerms(src, (ulong)size, DetourNativeMonoPosixPlatform.MmapProts.PROT_READ | DetourNativeMonoPosixPlatform.MmapProts.PROT_WRITE | DetourNativeMonoPosixPlatform.MmapProts.PROT_EXEC);
		}

		// Token: 0x06001897 RID: 6295 RVA: 0x00055722 File Offset: 0x00053922
		public void FlushICache(IntPtr src, uint size)
		{
			this.Inner.FlushICache(src, size);
		}

		// Token: 0x06001898 RID: 6296 RVA: 0x00055731 File Offset: 0x00053931
		public NativeDetourData Create(IntPtr from, IntPtr to, byte? type)
		{
			return this.Inner.Create(from, to, type);
		}

		// Token: 0x06001899 RID: 6297 RVA: 0x00055741 File Offset: 0x00053941
		public void Free(NativeDetourData detour)
		{
			this.Inner.Free(detour);
		}

		// Token: 0x0600189A RID: 6298 RVA: 0x0005574F File Offset: 0x0005394F
		public void Apply(NativeDetourData detour)
		{
			this.Inner.Apply(detour);
		}

		// Token: 0x0600189B RID: 6299 RVA: 0x0005575D File Offset: 0x0005395D
		public void Copy(IntPtr src, IntPtr dst, byte type)
		{
			this.Inner.Copy(src, dst, type);
		}

		// Token: 0x0600189C RID: 6300 RVA: 0x0005576D File Offset: 0x0005396D
		public IntPtr MemAlloc(uint size)
		{
			return this.Inner.MemAlloc(size);
		}

		// Token: 0x0600189D RID: 6301 RVA: 0x0005577B File Offset: 0x0005397B
		public void MemFree(IntPtr ptr)
		{
			this.Inner.MemFree(ptr);
		}

		// Token: 0x0600189E RID: 6302
		[DllImport("MonoPosixHelper", EntryPoint = "Mono_Posix_Syscall_sysconf", SetLastError = true)]
		public static extern long sysconf(DetourNativeMonoPosixPlatform.SysconfName name, DetourNativeMonoPosixPlatform.Errno defaultError);

		// Token: 0x0600189F RID: 6303
		[DllImport("MonoPosixHelper", EntryPoint = "Mono_Posix_Syscall_mprotect", SetLastError = true)]
		private static extern int mprotect(IntPtr start, ulong len, DetourNativeMonoPosixPlatform.MmapProts prot);

		// Token: 0x060018A0 RID: 6304
		[DllImport("MonoPosixHelper", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Mono_Posix_Stdlib_GetLastError")]
		private static extern int _GetLastError();

		// Token: 0x060018A1 RID: 6305
		[DllImport("MonoPosixHelper", EntryPoint = "Mono_Posix_ToErrno")]
		private static extern int ToErrno(int value, out DetourNativeMonoPosixPlatform.Errno rval);

		// Token: 0x040010BB RID: 4283
		private readonly IDetourNativePlatform Inner;

		// Token: 0x040010BC RID: 4284
		private readonly long _Pagesize;

		// Token: 0x02000473 RID: 1139
		[Flags]
		private enum MmapProts
		{
			// Token: 0x040010BE RID: 4286
			PROT_READ = 1,
			// Token: 0x040010BF RID: 4287
			PROT_WRITE = 2,
			// Token: 0x040010C0 RID: 4288
			PROT_EXEC = 4,
			// Token: 0x040010C1 RID: 4289
			PROT_NONE = 0,
			// Token: 0x040010C2 RID: 4290
			PROT_GROWSDOWN = 16777216,
			// Token: 0x040010C3 RID: 4291
			PROT_GROWSUP = 33554432
		}

		// Token: 0x02000474 RID: 1140
		public enum SysconfName
		{
			// Token: 0x040010C5 RID: 4293
			_SC_ARG_MAX,
			// Token: 0x040010C6 RID: 4294
			_SC_CHILD_MAX,
			// Token: 0x040010C7 RID: 4295
			_SC_CLK_TCK,
			// Token: 0x040010C8 RID: 4296
			_SC_NGROUPS_MAX,
			// Token: 0x040010C9 RID: 4297
			_SC_OPEN_MAX,
			// Token: 0x040010CA RID: 4298
			_SC_STREAM_MAX,
			// Token: 0x040010CB RID: 4299
			_SC_TZNAME_MAX,
			// Token: 0x040010CC RID: 4300
			_SC_JOB_CONTROL,
			// Token: 0x040010CD RID: 4301
			_SC_SAVED_IDS,
			// Token: 0x040010CE RID: 4302
			_SC_REALTIME_SIGNALS,
			// Token: 0x040010CF RID: 4303
			_SC_PRIORITY_SCHEDULING,
			// Token: 0x040010D0 RID: 4304
			_SC_TIMERS,
			// Token: 0x040010D1 RID: 4305
			_SC_ASYNCHRONOUS_IO,
			// Token: 0x040010D2 RID: 4306
			_SC_PRIORITIZED_IO,
			// Token: 0x040010D3 RID: 4307
			_SC_SYNCHRONIZED_IO,
			// Token: 0x040010D4 RID: 4308
			_SC_FSYNC,
			// Token: 0x040010D5 RID: 4309
			_SC_MAPPED_FILES,
			// Token: 0x040010D6 RID: 4310
			_SC_MEMLOCK,
			// Token: 0x040010D7 RID: 4311
			_SC_MEMLOCK_RANGE,
			// Token: 0x040010D8 RID: 4312
			_SC_MEMORY_PROTECTION,
			// Token: 0x040010D9 RID: 4313
			_SC_MESSAGE_PASSING,
			// Token: 0x040010DA RID: 4314
			_SC_SEMAPHORES,
			// Token: 0x040010DB RID: 4315
			_SC_SHARED_MEMORY_OBJECTS,
			// Token: 0x040010DC RID: 4316
			_SC_AIO_LISTIO_MAX,
			// Token: 0x040010DD RID: 4317
			_SC_AIO_MAX,
			// Token: 0x040010DE RID: 4318
			_SC_AIO_PRIO_DELTA_MAX,
			// Token: 0x040010DF RID: 4319
			_SC_DELAYTIMER_MAX,
			// Token: 0x040010E0 RID: 4320
			_SC_MQ_OPEN_MAX,
			// Token: 0x040010E1 RID: 4321
			_SC_MQ_PRIO_MAX,
			// Token: 0x040010E2 RID: 4322
			_SC_VERSION,
			// Token: 0x040010E3 RID: 4323
			_SC_PAGESIZE,
			// Token: 0x040010E4 RID: 4324
			_SC_RTSIG_MAX,
			// Token: 0x040010E5 RID: 4325
			_SC_SEM_NSEMS_MAX,
			// Token: 0x040010E6 RID: 4326
			_SC_SEM_VALUE_MAX,
			// Token: 0x040010E7 RID: 4327
			_SC_SIGQUEUE_MAX,
			// Token: 0x040010E8 RID: 4328
			_SC_TIMER_MAX,
			// Token: 0x040010E9 RID: 4329
			_SC_BC_BASE_MAX,
			// Token: 0x040010EA RID: 4330
			_SC_BC_DIM_MAX,
			// Token: 0x040010EB RID: 4331
			_SC_BC_SCALE_MAX,
			// Token: 0x040010EC RID: 4332
			_SC_BC_STRING_MAX,
			// Token: 0x040010ED RID: 4333
			_SC_COLL_WEIGHTS_MAX,
			// Token: 0x040010EE RID: 4334
			_SC_EQUIV_CLASS_MAX,
			// Token: 0x040010EF RID: 4335
			_SC_EXPR_NEST_MAX,
			// Token: 0x040010F0 RID: 4336
			_SC_LINE_MAX,
			// Token: 0x040010F1 RID: 4337
			_SC_RE_DUP_MAX,
			// Token: 0x040010F2 RID: 4338
			_SC_CHARCLASS_NAME_MAX,
			// Token: 0x040010F3 RID: 4339
			_SC_2_VERSION,
			// Token: 0x040010F4 RID: 4340
			_SC_2_C_BIND,
			// Token: 0x040010F5 RID: 4341
			_SC_2_C_DEV,
			// Token: 0x040010F6 RID: 4342
			_SC_2_FORT_DEV,
			// Token: 0x040010F7 RID: 4343
			_SC_2_FORT_RUN,
			// Token: 0x040010F8 RID: 4344
			_SC_2_SW_DEV,
			// Token: 0x040010F9 RID: 4345
			_SC_2_LOCALEDEF,
			// Token: 0x040010FA RID: 4346
			_SC_PII,
			// Token: 0x040010FB RID: 4347
			_SC_PII_XTI,
			// Token: 0x040010FC RID: 4348
			_SC_PII_SOCKET,
			// Token: 0x040010FD RID: 4349
			_SC_PII_INTERNET,
			// Token: 0x040010FE RID: 4350
			_SC_PII_OSI,
			// Token: 0x040010FF RID: 4351
			_SC_POLL,
			// Token: 0x04001100 RID: 4352
			_SC_SELECT,
			// Token: 0x04001101 RID: 4353
			_SC_UIO_MAXIOV,
			// Token: 0x04001102 RID: 4354
			_SC_IOV_MAX = 60,
			// Token: 0x04001103 RID: 4355
			_SC_PII_INTERNET_STREAM,
			// Token: 0x04001104 RID: 4356
			_SC_PII_INTERNET_DGRAM,
			// Token: 0x04001105 RID: 4357
			_SC_PII_OSI_COTS,
			// Token: 0x04001106 RID: 4358
			_SC_PII_OSI_CLTS,
			// Token: 0x04001107 RID: 4359
			_SC_PII_OSI_M,
			// Token: 0x04001108 RID: 4360
			_SC_T_IOV_MAX,
			// Token: 0x04001109 RID: 4361
			_SC_THREADS,
			// Token: 0x0400110A RID: 4362
			_SC_THREAD_SAFE_FUNCTIONS,
			// Token: 0x0400110B RID: 4363
			_SC_GETGR_R_SIZE_MAX,
			// Token: 0x0400110C RID: 4364
			_SC_GETPW_R_SIZE_MAX,
			// Token: 0x0400110D RID: 4365
			_SC_LOGIN_NAME_MAX,
			// Token: 0x0400110E RID: 4366
			_SC_TTY_NAME_MAX,
			// Token: 0x0400110F RID: 4367
			_SC_THREAD_DESTRUCTOR_ITERATIONS,
			// Token: 0x04001110 RID: 4368
			_SC_THREAD_KEYS_MAX,
			// Token: 0x04001111 RID: 4369
			_SC_THREAD_STACK_MIN,
			// Token: 0x04001112 RID: 4370
			_SC_THREAD_THREADS_MAX,
			// Token: 0x04001113 RID: 4371
			_SC_THREAD_ATTR_STACKADDR,
			// Token: 0x04001114 RID: 4372
			_SC_THREAD_ATTR_STACKSIZE,
			// Token: 0x04001115 RID: 4373
			_SC_THREAD_PRIORITY_SCHEDULING,
			// Token: 0x04001116 RID: 4374
			_SC_THREAD_PRIO_INHERIT,
			// Token: 0x04001117 RID: 4375
			_SC_THREAD_PRIO_PROTECT,
			// Token: 0x04001118 RID: 4376
			_SC_THREAD_PROCESS_SHARED,
			// Token: 0x04001119 RID: 4377
			_SC_NPROCESSORS_CONF,
			// Token: 0x0400111A RID: 4378
			_SC_NPROCESSORS_ONLN,
			// Token: 0x0400111B RID: 4379
			_SC_PHYS_PAGES,
			// Token: 0x0400111C RID: 4380
			_SC_AVPHYS_PAGES,
			// Token: 0x0400111D RID: 4381
			_SC_ATEXIT_MAX,
			// Token: 0x0400111E RID: 4382
			_SC_PASS_MAX,
			// Token: 0x0400111F RID: 4383
			_SC_XOPEN_VERSION,
			// Token: 0x04001120 RID: 4384
			_SC_XOPEN_XCU_VERSION,
			// Token: 0x04001121 RID: 4385
			_SC_XOPEN_UNIX,
			// Token: 0x04001122 RID: 4386
			_SC_XOPEN_CRYPT,
			// Token: 0x04001123 RID: 4387
			_SC_XOPEN_ENH_I18N,
			// Token: 0x04001124 RID: 4388
			_SC_XOPEN_SHM,
			// Token: 0x04001125 RID: 4389
			_SC_2_CHAR_TERM,
			// Token: 0x04001126 RID: 4390
			_SC_2_C_VERSION,
			// Token: 0x04001127 RID: 4391
			_SC_2_UPE,
			// Token: 0x04001128 RID: 4392
			_SC_XOPEN_XPG2,
			// Token: 0x04001129 RID: 4393
			_SC_XOPEN_XPG3,
			// Token: 0x0400112A RID: 4394
			_SC_XOPEN_XPG4,
			// Token: 0x0400112B RID: 4395
			_SC_CHAR_BIT,
			// Token: 0x0400112C RID: 4396
			_SC_CHAR_MAX,
			// Token: 0x0400112D RID: 4397
			_SC_CHAR_MIN,
			// Token: 0x0400112E RID: 4398
			_SC_INT_MAX,
			// Token: 0x0400112F RID: 4399
			_SC_INT_MIN,
			// Token: 0x04001130 RID: 4400
			_SC_LONG_BIT,
			// Token: 0x04001131 RID: 4401
			_SC_WORD_BIT,
			// Token: 0x04001132 RID: 4402
			_SC_MB_LEN_MAX,
			// Token: 0x04001133 RID: 4403
			_SC_NZERO,
			// Token: 0x04001134 RID: 4404
			_SC_SSIZE_MAX,
			// Token: 0x04001135 RID: 4405
			_SC_SCHAR_MAX,
			// Token: 0x04001136 RID: 4406
			_SC_SCHAR_MIN,
			// Token: 0x04001137 RID: 4407
			_SC_SHRT_MAX,
			// Token: 0x04001138 RID: 4408
			_SC_SHRT_MIN,
			// Token: 0x04001139 RID: 4409
			_SC_UCHAR_MAX,
			// Token: 0x0400113A RID: 4410
			_SC_UINT_MAX,
			// Token: 0x0400113B RID: 4411
			_SC_ULONG_MAX,
			// Token: 0x0400113C RID: 4412
			_SC_USHRT_MAX,
			// Token: 0x0400113D RID: 4413
			_SC_NL_ARGMAX,
			// Token: 0x0400113E RID: 4414
			_SC_NL_LANGMAX,
			// Token: 0x0400113F RID: 4415
			_SC_NL_MSGMAX,
			// Token: 0x04001140 RID: 4416
			_SC_NL_NMAX,
			// Token: 0x04001141 RID: 4417
			_SC_NL_SETMAX,
			// Token: 0x04001142 RID: 4418
			_SC_NL_TEXTMAX,
			// Token: 0x04001143 RID: 4419
			_SC_XBS5_ILP32_OFF32,
			// Token: 0x04001144 RID: 4420
			_SC_XBS5_ILP32_OFFBIG,
			// Token: 0x04001145 RID: 4421
			_SC_XBS5_LP64_OFF64,
			// Token: 0x04001146 RID: 4422
			_SC_XBS5_LPBIG_OFFBIG,
			// Token: 0x04001147 RID: 4423
			_SC_XOPEN_LEGACY,
			// Token: 0x04001148 RID: 4424
			_SC_XOPEN_REALTIME,
			// Token: 0x04001149 RID: 4425
			_SC_XOPEN_REALTIME_THREADS,
			// Token: 0x0400114A RID: 4426
			_SC_ADVISORY_INFO,
			// Token: 0x0400114B RID: 4427
			_SC_BARRIERS,
			// Token: 0x0400114C RID: 4428
			_SC_BASE,
			// Token: 0x0400114D RID: 4429
			_SC_C_LANG_SUPPORT,
			// Token: 0x0400114E RID: 4430
			_SC_C_LANG_SUPPORT_R,
			// Token: 0x0400114F RID: 4431
			_SC_CLOCK_SELECTION,
			// Token: 0x04001150 RID: 4432
			_SC_CPUTIME,
			// Token: 0x04001151 RID: 4433
			_SC_THREAD_CPUTIME,
			// Token: 0x04001152 RID: 4434
			_SC_DEVICE_IO,
			// Token: 0x04001153 RID: 4435
			_SC_DEVICE_SPECIFIC,
			// Token: 0x04001154 RID: 4436
			_SC_DEVICE_SPECIFIC_R,
			// Token: 0x04001155 RID: 4437
			_SC_FD_MGMT,
			// Token: 0x04001156 RID: 4438
			_SC_FIFO,
			// Token: 0x04001157 RID: 4439
			_SC_PIPE,
			// Token: 0x04001158 RID: 4440
			_SC_FILE_ATTRIBUTES,
			// Token: 0x04001159 RID: 4441
			_SC_FILE_LOCKING,
			// Token: 0x0400115A RID: 4442
			_SC_FILE_SYSTEM,
			// Token: 0x0400115B RID: 4443
			_SC_MONOTONIC_CLOCK,
			// Token: 0x0400115C RID: 4444
			_SC_MULTI_PROCESS,
			// Token: 0x0400115D RID: 4445
			_SC_SINGLE_PROCESS,
			// Token: 0x0400115E RID: 4446
			_SC_NETWORKING,
			// Token: 0x0400115F RID: 4447
			_SC_READER_WRITER_LOCKS,
			// Token: 0x04001160 RID: 4448
			_SC_SPIN_LOCKS,
			// Token: 0x04001161 RID: 4449
			_SC_REGEXP,
			// Token: 0x04001162 RID: 4450
			_SC_REGEX_VERSION,
			// Token: 0x04001163 RID: 4451
			_SC_SHELL,
			// Token: 0x04001164 RID: 4452
			_SC_SIGNALS,
			// Token: 0x04001165 RID: 4453
			_SC_SPAWN,
			// Token: 0x04001166 RID: 4454
			_SC_SPORADIC_SERVER,
			// Token: 0x04001167 RID: 4455
			_SC_THREAD_SPORADIC_SERVER,
			// Token: 0x04001168 RID: 4456
			_SC_SYSTEM_DATABASE,
			// Token: 0x04001169 RID: 4457
			_SC_SYSTEM_DATABASE_R,
			// Token: 0x0400116A RID: 4458
			_SC_TIMEOUTS,
			// Token: 0x0400116B RID: 4459
			_SC_TYPED_MEMORY_OBJECTS,
			// Token: 0x0400116C RID: 4460
			_SC_USER_GROUPS,
			// Token: 0x0400116D RID: 4461
			_SC_USER_GROUPS_R,
			// Token: 0x0400116E RID: 4462
			_SC_2_PBS,
			// Token: 0x0400116F RID: 4463
			_SC_2_PBS_ACCOUNTING,
			// Token: 0x04001170 RID: 4464
			_SC_2_PBS_LOCATE,
			// Token: 0x04001171 RID: 4465
			_SC_2_PBS_MESSAGE,
			// Token: 0x04001172 RID: 4466
			_SC_2_PBS_TRACK,
			// Token: 0x04001173 RID: 4467
			_SC_SYMLOOP_MAX,
			// Token: 0x04001174 RID: 4468
			_SC_STREAMS,
			// Token: 0x04001175 RID: 4469
			_SC_2_PBS_CHECKPOINT,
			// Token: 0x04001176 RID: 4470
			_SC_V6_ILP32_OFF32,
			// Token: 0x04001177 RID: 4471
			_SC_V6_ILP32_OFFBIG,
			// Token: 0x04001178 RID: 4472
			_SC_V6_LP64_OFF64,
			// Token: 0x04001179 RID: 4473
			_SC_V6_LPBIG_OFFBIG,
			// Token: 0x0400117A RID: 4474
			_SC_HOST_NAME_MAX,
			// Token: 0x0400117B RID: 4475
			_SC_TRACE,
			// Token: 0x0400117C RID: 4476
			_SC_TRACE_EVENT_FILTER,
			// Token: 0x0400117D RID: 4477
			_SC_TRACE_INHERIT,
			// Token: 0x0400117E RID: 4478
			_SC_TRACE_LOG,
			// Token: 0x0400117F RID: 4479
			_SC_LEVEL1_ICACHE_SIZE,
			// Token: 0x04001180 RID: 4480
			_SC_LEVEL1_ICACHE_ASSOC,
			// Token: 0x04001181 RID: 4481
			_SC_LEVEL1_ICACHE_LINESIZE,
			// Token: 0x04001182 RID: 4482
			_SC_LEVEL1_DCACHE_SIZE,
			// Token: 0x04001183 RID: 4483
			_SC_LEVEL1_DCACHE_ASSOC,
			// Token: 0x04001184 RID: 4484
			_SC_LEVEL1_DCACHE_LINESIZE,
			// Token: 0x04001185 RID: 4485
			_SC_LEVEL2_CACHE_SIZE,
			// Token: 0x04001186 RID: 4486
			_SC_LEVEL2_CACHE_ASSOC,
			// Token: 0x04001187 RID: 4487
			_SC_LEVEL2_CACHE_LINESIZE,
			// Token: 0x04001188 RID: 4488
			_SC_LEVEL3_CACHE_SIZE,
			// Token: 0x04001189 RID: 4489
			_SC_LEVEL3_CACHE_ASSOC,
			// Token: 0x0400118A RID: 4490
			_SC_LEVEL3_CACHE_LINESIZE,
			// Token: 0x0400118B RID: 4491
			_SC_LEVEL4_CACHE_SIZE,
			// Token: 0x0400118C RID: 4492
			_SC_LEVEL4_CACHE_ASSOC,
			// Token: 0x0400118D RID: 4493
			_SC_LEVEL4_CACHE_LINESIZE
		}

		// Token: 0x02000475 RID: 1141
		public enum Errno
		{
			// Token: 0x0400118F RID: 4495
			EPERM = 1,
			// Token: 0x04001190 RID: 4496
			ENOENT,
			// Token: 0x04001191 RID: 4497
			ESRCH,
			// Token: 0x04001192 RID: 4498
			EINTR,
			// Token: 0x04001193 RID: 4499
			EIO,
			// Token: 0x04001194 RID: 4500
			ENXIO,
			// Token: 0x04001195 RID: 4501
			E2BIG,
			// Token: 0x04001196 RID: 4502
			ENOEXEC,
			// Token: 0x04001197 RID: 4503
			EBADF,
			// Token: 0x04001198 RID: 4504
			ECHILD,
			// Token: 0x04001199 RID: 4505
			EAGAIN,
			// Token: 0x0400119A RID: 4506
			ENOMEM,
			// Token: 0x0400119B RID: 4507
			EACCES,
			// Token: 0x0400119C RID: 4508
			EFAULT,
			// Token: 0x0400119D RID: 4509
			ENOTBLK,
			// Token: 0x0400119E RID: 4510
			EBUSY,
			// Token: 0x0400119F RID: 4511
			EEXIST,
			// Token: 0x040011A0 RID: 4512
			EXDEV,
			// Token: 0x040011A1 RID: 4513
			ENODEV,
			// Token: 0x040011A2 RID: 4514
			ENOTDIR,
			// Token: 0x040011A3 RID: 4515
			EISDIR,
			// Token: 0x040011A4 RID: 4516
			EINVAL,
			// Token: 0x040011A5 RID: 4517
			ENFILE,
			// Token: 0x040011A6 RID: 4518
			EMFILE,
			// Token: 0x040011A7 RID: 4519
			ENOTTY,
			// Token: 0x040011A8 RID: 4520
			ETXTBSY,
			// Token: 0x040011A9 RID: 4521
			EFBIG,
			// Token: 0x040011AA RID: 4522
			ENOSPC,
			// Token: 0x040011AB RID: 4523
			ESPIPE,
			// Token: 0x040011AC RID: 4524
			EROFS,
			// Token: 0x040011AD RID: 4525
			EMLINK,
			// Token: 0x040011AE RID: 4526
			EPIPE,
			// Token: 0x040011AF RID: 4527
			EDOM,
			// Token: 0x040011B0 RID: 4528
			ERANGE,
			// Token: 0x040011B1 RID: 4529
			EDEADLK,
			// Token: 0x040011B2 RID: 4530
			ENAMETOOLONG,
			// Token: 0x040011B3 RID: 4531
			ENOLCK,
			// Token: 0x040011B4 RID: 4532
			ENOSYS,
			// Token: 0x040011B5 RID: 4533
			ENOTEMPTY,
			// Token: 0x040011B6 RID: 4534
			ELOOP,
			// Token: 0x040011B7 RID: 4535
			EWOULDBLOCK = 11,
			// Token: 0x040011B8 RID: 4536
			ENOMSG = 42,
			// Token: 0x040011B9 RID: 4537
			EIDRM,
			// Token: 0x040011BA RID: 4538
			ECHRNG,
			// Token: 0x040011BB RID: 4539
			EL2NSYNC,
			// Token: 0x040011BC RID: 4540
			EL3HLT,
			// Token: 0x040011BD RID: 4541
			EL3RST,
			// Token: 0x040011BE RID: 4542
			ELNRNG,
			// Token: 0x040011BF RID: 4543
			EUNATCH,
			// Token: 0x040011C0 RID: 4544
			ENOCSI,
			// Token: 0x040011C1 RID: 4545
			EL2HLT,
			// Token: 0x040011C2 RID: 4546
			EBADE,
			// Token: 0x040011C3 RID: 4547
			EBADR,
			// Token: 0x040011C4 RID: 4548
			EXFULL,
			// Token: 0x040011C5 RID: 4549
			ENOANO,
			// Token: 0x040011C6 RID: 4550
			EBADRQC,
			// Token: 0x040011C7 RID: 4551
			EBADSLT,
			// Token: 0x040011C8 RID: 4552
			EDEADLOCK = 35,
			// Token: 0x040011C9 RID: 4553
			EBFONT = 59,
			// Token: 0x040011CA RID: 4554
			ENOSTR,
			// Token: 0x040011CB RID: 4555
			ENODATA,
			// Token: 0x040011CC RID: 4556
			ETIME,
			// Token: 0x040011CD RID: 4557
			ENOSR,
			// Token: 0x040011CE RID: 4558
			ENONET,
			// Token: 0x040011CF RID: 4559
			ENOPKG,
			// Token: 0x040011D0 RID: 4560
			EREMOTE,
			// Token: 0x040011D1 RID: 4561
			ENOLINK,
			// Token: 0x040011D2 RID: 4562
			EADV,
			// Token: 0x040011D3 RID: 4563
			ESRMNT,
			// Token: 0x040011D4 RID: 4564
			ECOMM,
			// Token: 0x040011D5 RID: 4565
			EPROTO,
			// Token: 0x040011D6 RID: 4566
			EMULTIHOP,
			// Token: 0x040011D7 RID: 4567
			EDOTDOT,
			// Token: 0x040011D8 RID: 4568
			EBADMSG,
			// Token: 0x040011D9 RID: 4569
			EOVERFLOW,
			// Token: 0x040011DA RID: 4570
			ENOTUNIQ,
			// Token: 0x040011DB RID: 4571
			EBADFD,
			// Token: 0x040011DC RID: 4572
			EREMCHG,
			// Token: 0x040011DD RID: 4573
			ELIBACC,
			// Token: 0x040011DE RID: 4574
			ELIBBAD,
			// Token: 0x040011DF RID: 4575
			ELIBSCN,
			// Token: 0x040011E0 RID: 4576
			ELIBMAX,
			// Token: 0x040011E1 RID: 4577
			ELIBEXEC,
			// Token: 0x040011E2 RID: 4578
			EILSEQ,
			// Token: 0x040011E3 RID: 4579
			ERESTART,
			// Token: 0x040011E4 RID: 4580
			ESTRPIPE,
			// Token: 0x040011E5 RID: 4581
			EUSERS,
			// Token: 0x040011E6 RID: 4582
			ENOTSOCK,
			// Token: 0x040011E7 RID: 4583
			EDESTADDRREQ,
			// Token: 0x040011E8 RID: 4584
			EMSGSIZE,
			// Token: 0x040011E9 RID: 4585
			EPROTOTYPE,
			// Token: 0x040011EA RID: 4586
			ENOPROTOOPT,
			// Token: 0x040011EB RID: 4587
			EPROTONOSUPPORT,
			// Token: 0x040011EC RID: 4588
			ESOCKTNOSUPPORT,
			// Token: 0x040011ED RID: 4589
			EOPNOTSUPP,
			// Token: 0x040011EE RID: 4590
			EPFNOSUPPORT,
			// Token: 0x040011EF RID: 4591
			EAFNOSUPPORT,
			// Token: 0x040011F0 RID: 4592
			EADDRINUSE,
			// Token: 0x040011F1 RID: 4593
			EADDRNOTAVAIL,
			// Token: 0x040011F2 RID: 4594
			ENETDOWN,
			// Token: 0x040011F3 RID: 4595
			ENETUNREACH,
			// Token: 0x040011F4 RID: 4596
			ENETRESET,
			// Token: 0x040011F5 RID: 4597
			ECONNABORTED,
			// Token: 0x040011F6 RID: 4598
			ECONNRESET,
			// Token: 0x040011F7 RID: 4599
			ENOBUFS,
			// Token: 0x040011F8 RID: 4600
			EISCONN,
			// Token: 0x040011F9 RID: 4601
			ENOTCONN,
			// Token: 0x040011FA RID: 4602
			ESHUTDOWN,
			// Token: 0x040011FB RID: 4603
			ETOOMANYREFS,
			// Token: 0x040011FC RID: 4604
			ETIMEDOUT,
			// Token: 0x040011FD RID: 4605
			ECONNREFUSED,
			// Token: 0x040011FE RID: 4606
			EHOSTDOWN,
			// Token: 0x040011FF RID: 4607
			EHOSTUNREACH,
			// Token: 0x04001200 RID: 4608
			EALREADY,
			// Token: 0x04001201 RID: 4609
			EINPROGRESS,
			// Token: 0x04001202 RID: 4610
			ESTALE,
			// Token: 0x04001203 RID: 4611
			EUCLEAN,
			// Token: 0x04001204 RID: 4612
			ENOTNAM,
			// Token: 0x04001205 RID: 4613
			ENAVAIL,
			// Token: 0x04001206 RID: 4614
			EISNAM,
			// Token: 0x04001207 RID: 4615
			EREMOTEIO,
			// Token: 0x04001208 RID: 4616
			EDQUOT,
			// Token: 0x04001209 RID: 4617
			ENOMEDIUM,
			// Token: 0x0400120A RID: 4618
			EMEDIUMTYPE,
			// Token: 0x0400120B RID: 4619
			ECANCELED,
			// Token: 0x0400120C RID: 4620
			ENOKEY,
			// Token: 0x0400120D RID: 4621
			EKEYEXPIRED,
			// Token: 0x0400120E RID: 4622
			EKEYREVOKED,
			// Token: 0x0400120F RID: 4623
			EKEYREJECTED,
			// Token: 0x04001210 RID: 4624
			EOWNERDEAD,
			// Token: 0x04001211 RID: 4625
			ENOTRECOVERABLE,
			// Token: 0x04001212 RID: 4626
			EPROCLIM = 1067,
			// Token: 0x04001213 RID: 4627
			EBADRPC = 1072,
			// Token: 0x04001214 RID: 4628
			ERPCMISMATCH,
			// Token: 0x04001215 RID: 4629
			EPROGUNAVAIL,
			// Token: 0x04001216 RID: 4630
			EPROGMISMATCH,
			// Token: 0x04001217 RID: 4631
			EPROCUNAVAIL,
			// Token: 0x04001218 RID: 4632
			EFTYPE = 1079,
			// Token: 0x04001219 RID: 4633
			EAUTH,
			// Token: 0x0400121A RID: 4634
			ENEEDAUTH,
			// Token: 0x0400121B RID: 4635
			EPWROFF,
			// Token: 0x0400121C RID: 4636
			EDEVERR,
			// Token: 0x0400121D RID: 4637
			EBADEXEC = 1085,
			// Token: 0x0400121E RID: 4638
			EBADARCH,
			// Token: 0x0400121F RID: 4639
			ESHLIBVERS,
			// Token: 0x04001220 RID: 4640
			EBADMACHO,
			// Token: 0x04001221 RID: 4641
			ENOATTR = 1093,
			// Token: 0x04001222 RID: 4642
			ENOPOLICY = 1103
		}
	}
}
