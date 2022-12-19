using System;
using System.Reflection.Emit;

namespace HarmonyLib
{
	// Token: 0x0200009F RID: 159
	public static class Code
	{
		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000350 RID: 848 RVA: 0x00010C9D File Offset: 0x0000EE9D
		public static Code.Operand_ Operand
		{
			get
			{
				return new Code.Operand_();
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000351 RID: 849 RVA: 0x00010CA4 File Offset: 0x0000EEA4
		public static Code.Nop_ Nop
		{
			get
			{
				return new Code.Nop_
				{
					opcode = OpCodes.Nop
				};
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000352 RID: 850 RVA: 0x00010CB6 File Offset: 0x0000EEB6
		public static Code.Break_ Break
		{
			get
			{
				return new Code.Break_
				{
					opcode = OpCodes.Break
				};
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000353 RID: 851 RVA: 0x00010CC8 File Offset: 0x0000EEC8
		public static Code.Ldarg_0_ Ldarg_0
		{
			get
			{
				return new Code.Ldarg_0_
				{
					opcode = OpCodes.Ldarg_0
				};
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000354 RID: 852 RVA: 0x00010CDA File Offset: 0x0000EEDA
		public static Code.Ldarg_1_ Ldarg_1
		{
			get
			{
				return new Code.Ldarg_1_
				{
					opcode = OpCodes.Ldarg_1
				};
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000355 RID: 853 RVA: 0x00010CEC File Offset: 0x0000EEEC
		public static Code.Ldarg_2_ Ldarg_2
		{
			get
			{
				return new Code.Ldarg_2_
				{
					opcode = OpCodes.Ldarg_2
				};
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000356 RID: 854 RVA: 0x00010CFE File Offset: 0x0000EEFE
		public static Code.Ldarg_3_ Ldarg_3
		{
			get
			{
				return new Code.Ldarg_3_
				{
					opcode = OpCodes.Ldarg_3
				};
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000357 RID: 855 RVA: 0x00010D10 File Offset: 0x0000EF10
		public static Code.Ldloc_0_ Ldloc_0
		{
			get
			{
				return new Code.Ldloc_0_
				{
					opcode = OpCodes.Ldloc_0
				};
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000358 RID: 856 RVA: 0x00010D22 File Offset: 0x0000EF22
		public static Code.Ldloc_1_ Ldloc_1
		{
			get
			{
				return new Code.Ldloc_1_
				{
					opcode = OpCodes.Ldloc_1
				};
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000359 RID: 857 RVA: 0x00010D34 File Offset: 0x0000EF34
		public static Code.Ldloc_2_ Ldloc_2
		{
			get
			{
				return new Code.Ldloc_2_
				{
					opcode = OpCodes.Ldloc_2
				};
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600035A RID: 858 RVA: 0x00010D46 File Offset: 0x0000EF46
		public static Code.Ldloc_3_ Ldloc_3
		{
			get
			{
				return new Code.Ldloc_3_
				{
					opcode = OpCodes.Ldloc_3
				};
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600035B RID: 859 RVA: 0x00010D58 File Offset: 0x0000EF58
		public static Code.Stloc_0_ Stloc_0
		{
			get
			{
				return new Code.Stloc_0_
				{
					opcode = OpCodes.Stloc_0
				};
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600035C RID: 860 RVA: 0x00010D6A File Offset: 0x0000EF6A
		public static Code.Stloc_1_ Stloc_1
		{
			get
			{
				return new Code.Stloc_1_
				{
					opcode = OpCodes.Stloc_1
				};
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600035D RID: 861 RVA: 0x00010D7C File Offset: 0x0000EF7C
		public static Code.Stloc_2_ Stloc_2
		{
			get
			{
				return new Code.Stloc_2_
				{
					opcode = OpCodes.Stloc_2
				};
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600035E RID: 862 RVA: 0x00010D8E File Offset: 0x0000EF8E
		public static Code.Stloc_3_ Stloc_3
		{
			get
			{
				return new Code.Stloc_3_
				{
					opcode = OpCodes.Stloc_3
				};
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x0600035F RID: 863 RVA: 0x00010DA0 File Offset: 0x0000EFA0
		public static Code.Ldarg_S_ Ldarg_S
		{
			get
			{
				return new Code.Ldarg_S_
				{
					opcode = OpCodes.Ldarg_S
				};
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000360 RID: 864 RVA: 0x00010DB2 File Offset: 0x0000EFB2
		public static Code.Ldarga_S_ Ldarga_S
		{
			get
			{
				return new Code.Ldarga_S_
				{
					opcode = OpCodes.Ldarga_S
				};
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000361 RID: 865 RVA: 0x00010DC4 File Offset: 0x0000EFC4
		public static Code.Starg_S_ Starg_S
		{
			get
			{
				return new Code.Starg_S_
				{
					opcode = OpCodes.Starg_S
				};
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000362 RID: 866 RVA: 0x00010DD6 File Offset: 0x0000EFD6
		public static Code.Ldloc_S_ Ldloc_S
		{
			get
			{
				return new Code.Ldloc_S_
				{
					opcode = OpCodes.Ldloc_S
				};
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000363 RID: 867 RVA: 0x00010DE8 File Offset: 0x0000EFE8
		public static Code.Ldloca_S_ Ldloca_S
		{
			get
			{
				return new Code.Ldloca_S_
				{
					opcode = OpCodes.Ldloca_S
				};
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000364 RID: 868 RVA: 0x00010DFA File Offset: 0x0000EFFA
		public static Code.Stloc_S_ Stloc_S
		{
			get
			{
				return new Code.Stloc_S_
				{
					opcode = OpCodes.Stloc_S
				};
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000365 RID: 869 RVA: 0x00010E0C File Offset: 0x0000F00C
		public static Code.Ldnull_ Ldnull
		{
			get
			{
				return new Code.Ldnull_
				{
					opcode = OpCodes.Ldnull
				};
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000366 RID: 870 RVA: 0x00010E1E File Offset: 0x0000F01E
		public static Code.Ldc_I4_M1_ Ldc_I4_M1
		{
			get
			{
				return new Code.Ldc_I4_M1_
				{
					opcode = OpCodes.Ldc_I4_M1
				};
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000367 RID: 871 RVA: 0x00010E30 File Offset: 0x0000F030
		public static Code.Ldc_I4_0_ Ldc_I4_0
		{
			get
			{
				return new Code.Ldc_I4_0_
				{
					opcode = OpCodes.Ldc_I4_0
				};
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000368 RID: 872 RVA: 0x00010E42 File Offset: 0x0000F042
		public static Code.Ldc_I4_1_ Ldc_I4_1
		{
			get
			{
				return new Code.Ldc_I4_1_
				{
					opcode = OpCodes.Ldc_I4_1
				};
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000369 RID: 873 RVA: 0x00010E54 File Offset: 0x0000F054
		public static Code.Ldc_I4_2_ Ldc_I4_2
		{
			get
			{
				return new Code.Ldc_I4_2_
				{
					opcode = OpCodes.Ldc_I4_2
				};
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600036A RID: 874 RVA: 0x00010E66 File Offset: 0x0000F066
		public static Code.Ldc_I4_3_ Ldc_I4_3
		{
			get
			{
				return new Code.Ldc_I4_3_
				{
					opcode = OpCodes.Ldc_I4_3
				};
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600036B RID: 875 RVA: 0x00010E78 File Offset: 0x0000F078
		public static Code.Ldc_I4_4_ Ldc_I4_4
		{
			get
			{
				return new Code.Ldc_I4_4_
				{
					opcode = OpCodes.Ldc_I4_4
				};
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x0600036C RID: 876 RVA: 0x00010E8A File Offset: 0x0000F08A
		public static Code.Ldc_I4_5_ Ldc_I4_5
		{
			get
			{
				return new Code.Ldc_I4_5_
				{
					opcode = OpCodes.Ldc_I4_5
				};
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x0600036D RID: 877 RVA: 0x00010E9C File Offset: 0x0000F09C
		public static Code.Ldc_I4_6_ Ldc_I4_6
		{
			get
			{
				return new Code.Ldc_I4_6_
				{
					opcode = OpCodes.Ldc_I4_6
				};
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x0600036E RID: 878 RVA: 0x00010EAE File Offset: 0x0000F0AE
		public static Code.Ldc_I4_7_ Ldc_I4_7
		{
			get
			{
				return new Code.Ldc_I4_7_
				{
					opcode = OpCodes.Ldc_I4_7
				};
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x0600036F RID: 879 RVA: 0x00010EC0 File Offset: 0x0000F0C0
		public static Code.Ldc_I4_8_ Ldc_I4_8
		{
			get
			{
				return new Code.Ldc_I4_8_
				{
					opcode = OpCodes.Ldc_I4_8
				};
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000370 RID: 880 RVA: 0x00010ED2 File Offset: 0x0000F0D2
		public static Code.Ldc_I4_S_ Ldc_I4_S
		{
			get
			{
				return new Code.Ldc_I4_S_
				{
					opcode = OpCodes.Ldc_I4_S
				};
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000371 RID: 881 RVA: 0x00010EE4 File Offset: 0x0000F0E4
		public static Code.Ldc_I4_ Ldc_I4
		{
			get
			{
				return new Code.Ldc_I4_
				{
					opcode = OpCodes.Ldc_I4
				};
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000372 RID: 882 RVA: 0x00010EF6 File Offset: 0x0000F0F6
		public static Code.Ldc_I8_ Ldc_I8
		{
			get
			{
				return new Code.Ldc_I8_
				{
					opcode = OpCodes.Ldc_I8
				};
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000373 RID: 883 RVA: 0x00010F08 File Offset: 0x0000F108
		public static Code.Ldc_R4_ Ldc_R4
		{
			get
			{
				return new Code.Ldc_R4_
				{
					opcode = OpCodes.Ldc_R4
				};
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000374 RID: 884 RVA: 0x00010F1A File Offset: 0x0000F11A
		public static Code.Ldc_R8_ Ldc_R8
		{
			get
			{
				return new Code.Ldc_R8_
				{
					opcode = OpCodes.Ldc_R8
				};
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000375 RID: 885 RVA: 0x00010F2C File Offset: 0x0000F12C
		public static Code.Dup_ Dup
		{
			get
			{
				return new Code.Dup_
				{
					opcode = OpCodes.Dup
				};
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000376 RID: 886 RVA: 0x00010F3E File Offset: 0x0000F13E
		public static Code.Pop_ Pop
		{
			get
			{
				return new Code.Pop_
				{
					opcode = OpCodes.Pop
				};
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000377 RID: 887 RVA: 0x00010F50 File Offset: 0x0000F150
		public static Code.Jmp_ Jmp
		{
			get
			{
				return new Code.Jmp_
				{
					opcode = OpCodes.Jmp
				};
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000378 RID: 888 RVA: 0x00010F62 File Offset: 0x0000F162
		public static Code.Call_ Call
		{
			get
			{
				return new Code.Call_
				{
					opcode = OpCodes.Call
				};
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000379 RID: 889 RVA: 0x00010F74 File Offset: 0x0000F174
		public static Code.Calli_ Calli
		{
			get
			{
				return new Code.Calli_
				{
					opcode = OpCodes.Calli
				};
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x0600037A RID: 890 RVA: 0x00010F86 File Offset: 0x0000F186
		public static Code.Ret_ Ret
		{
			get
			{
				return new Code.Ret_
				{
					opcode = OpCodes.Ret
				};
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x0600037B RID: 891 RVA: 0x00010F98 File Offset: 0x0000F198
		public static Code.Br_S_ Br_S
		{
			get
			{
				return new Code.Br_S_
				{
					opcode = OpCodes.Br_S
				};
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x0600037C RID: 892 RVA: 0x00010FAA File Offset: 0x0000F1AA
		public static Code.Brfalse_S_ Brfalse_S
		{
			get
			{
				return new Code.Brfalse_S_
				{
					opcode = OpCodes.Brfalse_S
				};
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x0600037D RID: 893 RVA: 0x00010FBC File Offset: 0x0000F1BC
		public static Code.Brtrue_S_ Brtrue_S
		{
			get
			{
				return new Code.Brtrue_S_
				{
					opcode = OpCodes.Brtrue_S
				};
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x0600037E RID: 894 RVA: 0x00010FCE File Offset: 0x0000F1CE
		public static Code.Beq_S_ Beq_S
		{
			get
			{
				return new Code.Beq_S_
				{
					opcode = OpCodes.Beq_S
				};
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x0600037F RID: 895 RVA: 0x00010FE0 File Offset: 0x0000F1E0
		public static Code.Bge_S_ Bge_S
		{
			get
			{
				return new Code.Bge_S_
				{
					opcode = OpCodes.Bge_S
				};
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000380 RID: 896 RVA: 0x00010FF2 File Offset: 0x0000F1F2
		public static Code.Bgt_S_ Bgt_S
		{
			get
			{
				return new Code.Bgt_S_
				{
					opcode = OpCodes.Bgt_S
				};
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000381 RID: 897 RVA: 0x00011004 File Offset: 0x0000F204
		public static Code.Ble_S_ Ble_S
		{
			get
			{
				return new Code.Ble_S_
				{
					opcode = OpCodes.Ble_S
				};
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000382 RID: 898 RVA: 0x00011016 File Offset: 0x0000F216
		public static Code.Blt_S_ Blt_S
		{
			get
			{
				return new Code.Blt_S_
				{
					opcode = OpCodes.Blt_S
				};
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000383 RID: 899 RVA: 0x00011028 File Offset: 0x0000F228
		public static Code.Bne_Un_S_ Bne_Un_S
		{
			get
			{
				return new Code.Bne_Un_S_
				{
					opcode = OpCodes.Bne_Un_S
				};
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000384 RID: 900 RVA: 0x0001103A File Offset: 0x0000F23A
		public static Code.Bge_Un_S_ Bge_Un_S
		{
			get
			{
				return new Code.Bge_Un_S_
				{
					opcode = OpCodes.Bge_Un_S
				};
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000385 RID: 901 RVA: 0x0001104C File Offset: 0x0000F24C
		public static Code.Bgt_Un_S_ Bgt_Un_S
		{
			get
			{
				return new Code.Bgt_Un_S_
				{
					opcode = OpCodes.Bgt_Un_S
				};
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000386 RID: 902 RVA: 0x0001105E File Offset: 0x0000F25E
		public static Code.Ble_Un_S_ Ble_Un_S
		{
			get
			{
				return new Code.Ble_Un_S_
				{
					opcode = OpCodes.Ble_Un_S
				};
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000387 RID: 903 RVA: 0x00011070 File Offset: 0x0000F270
		public static Code.Blt_Un_S_ Blt_Un_S
		{
			get
			{
				return new Code.Blt_Un_S_
				{
					opcode = OpCodes.Blt_Un_S
				};
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000388 RID: 904 RVA: 0x00011082 File Offset: 0x0000F282
		public static Code.Br_ Br
		{
			get
			{
				return new Code.Br_
				{
					opcode = OpCodes.Br
				};
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000389 RID: 905 RVA: 0x00011094 File Offset: 0x0000F294
		public static Code.Brfalse_ Brfalse
		{
			get
			{
				return new Code.Brfalse_
				{
					opcode = OpCodes.Brfalse
				};
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x0600038A RID: 906 RVA: 0x000110A6 File Offset: 0x0000F2A6
		public static Code.Brtrue_ Brtrue
		{
			get
			{
				return new Code.Brtrue_
				{
					opcode = OpCodes.Brtrue
				};
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x0600038B RID: 907 RVA: 0x000110B8 File Offset: 0x0000F2B8
		public static Code.Beq_ Beq
		{
			get
			{
				return new Code.Beq_
				{
					opcode = OpCodes.Beq
				};
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x0600038C RID: 908 RVA: 0x000110CA File Offset: 0x0000F2CA
		public static Code.Bge_ Bge
		{
			get
			{
				return new Code.Bge_
				{
					opcode = OpCodes.Bge
				};
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x0600038D RID: 909 RVA: 0x000110DC File Offset: 0x0000F2DC
		public static Code.Bgt_ Bgt
		{
			get
			{
				return new Code.Bgt_
				{
					opcode = OpCodes.Bgt
				};
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x0600038E RID: 910 RVA: 0x000110EE File Offset: 0x0000F2EE
		public static Code.Ble_ Ble
		{
			get
			{
				return new Code.Ble_
				{
					opcode = OpCodes.Ble
				};
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x0600038F RID: 911 RVA: 0x00011100 File Offset: 0x0000F300
		public static Code.Blt_ Blt
		{
			get
			{
				return new Code.Blt_
				{
					opcode = OpCodes.Blt
				};
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000390 RID: 912 RVA: 0x00011112 File Offset: 0x0000F312
		public static Code.Bne_Un_ Bne_Un
		{
			get
			{
				return new Code.Bne_Un_
				{
					opcode = OpCodes.Bne_Un
				};
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000391 RID: 913 RVA: 0x00011124 File Offset: 0x0000F324
		public static Code.Bge_Un_ Bge_Un
		{
			get
			{
				return new Code.Bge_Un_
				{
					opcode = OpCodes.Bge_Un
				};
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000392 RID: 914 RVA: 0x00011136 File Offset: 0x0000F336
		public static Code.Bgt_Un_ Bgt_Un
		{
			get
			{
				return new Code.Bgt_Un_
				{
					opcode = OpCodes.Bgt_Un
				};
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000393 RID: 915 RVA: 0x00011148 File Offset: 0x0000F348
		public static Code.Ble_Un_ Ble_Un
		{
			get
			{
				return new Code.Ble_Un_
				{
					opcode = OpCodes.Ble_Un
				};
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000394 RID: 916 RVA: 0x0001115A File Offset: 0x0000F35A
		public static Code.Blt_Un_ Blt_Un
		{
			get
			{
				return new Code.Blt_Un_
				{
					opcode = OpCodes.Blt_Un
				};
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000395 RID: 917 RVA: 0x0001116C File Offset: 0x0000F36C
		public static Code.Switch_ Switch
		{
			get
			{
				return new Code.Switch_
				{
					opcode = OpCodes.Switch
				};
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000396 RID: 918 RVA: 0x0001117E File Offset: 0x0000F37E
		public static Code.Ldind_I1_ Ldind_I1
		{
			get
			{
				return new Code.Ldind_I1_
				{
					opcode = OpCodes.Ldind_I1
				};
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000397 RID: 919 RVA: 0x00011190 File Offset: 0x0000F390
		public static Code.Ldind_U1_ Ldind_U1
		{
			get
			{
				return new Code.Ldind_U1_
				{
					opcode = OpCodes.Ldind_U1
				};
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000398 RID: 920 RVA: 0x000111A2 File Offset: 0x0000F3A2
		public static Code.Ldind_I2_ Ldind_I2
		{
			get
			{
				return new Code.Ldind_I2_
				{
					opcode = OpCodes.Ldind_I2
				};
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000399 RID: 921 RVA: 0x000111B4 File Offset: 0x0000F3B4
		public static Code.Ldind_U2_ Ldind_U2
		{
			get
			{
				return new Code.Ldind_U2_
				{
					opcode = OpCodes.Ldind_U2
				};
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x0600039A RID: 922 RVA: 0x000111C6 File Offset: 0x0000F3C6
		public static Code.Ldind_I4_ Ldind_I4
		{
			get
			{
				return new Code.Ldind_I4_
				{
					opcode = OpCodes.Ldind_I4
				};
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x0600039B RID: 923 RVA: 0x000111D8 File Offset: 0x0000F3D8
		public static Code.Ldind_U4_ Ldind_U4
		{
			get
			{
				return new Code.Ldind_U4_
				{
					opcode = OpCodes.Ldind_U4
				};
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x0600039C RID: 924 RVA: 0x000111EA File Offset: 0x0000F3EA
		public static Code.Ldind_I8_ Ldind_I8
		{
			get
			{
				return new Code.Ldind_I8_
				{
					opcode = OpCodes.Ldind_I8
				};
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x0600039D RID: 925 RVA: 0x000111FC File Offset: 0x0000F3FC
		public static Code.Ldind_I_ Ldind_I
		{
			get
			{
				return new Code.Ldind_I_
				{
					opcode = OpCodes.Ldind_I
				};
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x0600039E RID: 926 RVA: 0x0001120E File Offset: 0x0000F40E
		public static Code.Ldind_R4_ Ldind_R4
		{
			get
			{
				return new Code.Ldind_R4_
				{
					opcode = OpCodes.Ldind_R4
				};
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x0600039F RID: 927 RVA: 0x00011220 File Offset: 0x0000F420
		public static Code.Ldind_R8_ Ldind_R8
		{
			get
			{
				return new Code.Ldind_R8_
				{
					opcode = OpCodes.Ldind_R8
				};
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060003A0 RID: 928 RVA: 0x00011232 File Offset: 0x0000F432
		public static Code.Ldind_Ref_ Ldind_Ref
		{
			get
			{
				return new Code.Ldind_Ref_
				{
					opcode = OpCodes.Ldind_Ref
				};
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x060003A1 RID: 929 RVA: 0x00011244 File Offset: 0x0000F444
		public static Code.Stind_Ref_ Stind_Ref
		{
			get
			{
				return new Code.Stind_Ref_
				{
					opcode = OpCodes.Stind_Ref
				};
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x060003A2 RID: 930 RVA: 0x00011256 File Offset: 0x0000F456
		public static Code.Stind_I1_ Stind_I1
		{
			get
			{
				return new Code.Stind_I1_
				{
					opcode = OpCodes.Stind_I1
				};
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x060003A3 RID: 931 RVA: 0x00011268 File Offset: 0x0000F468
		public static Code.Stind_I2_ Stind_I2
		{
			get
			{
				return new Code.Stind_I2_
				{
					opcode = OpCodes.Stind_I2
				};
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x060003A4 RID: 932 RVA: 0x0001127A File Offset: 0x0000F47A
		public static Code.Stind_I4_ Stind_I4
		{
			get
			{
				return new Code.Stind_I4_
				{
					opcode = OpCodes.Stind_I4
				};
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x060003A5 RID: 933 RVA: 0x0001128C File Offset: 0x0000F48C
		public static Code.Stind_I8_ Stind_I8
		{
			get
			{
				return new Code.Stind_I8_
				{
					opcode = OpCodes.Stind_I8
				};
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x060003A6 RID: 934 RVA: 0x0001129E File Offset: 0x0000F49E
		public static Code.Stind_R4_ Stind_R4
		{
			get
			{
				return new Code.Stind_R4_
				{
					opcode = OpCodes.Stind_R4
				};
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x060003A7 RID: 935 RVA: 0x000112B0 File Offset: 0x0000F4B0
		public static Code.Stind_R8_ Stind_R8
		{
			get
			{
				return new Code.Stind_R8_
				{
					opcode = OpCodes.Stind_R8
				};
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x060003A8 RID: 936 RVA: 0x000112C2 File Offset: 0x0000F4C2
		public static Code.Add_ Add
		{
			get
			{
				return new Code.Add_
				{
					opcode = OpCodes.Add
				};
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x060003A9 RID: 937 RVA: 0x000112D4 File Offset: 0x0000F4D4
		public static Code.Sub_ Sub
		{
			get
			{
				return new Code.Sub_
				{
					opcode = OpCodes.Sub
				};
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x060003AA RID: 938 RVA: 0x000112E6 File Offset: 0x0000F4E6
		public static Code.Mul_ Mul
		{
			get
			{
				return new Code.Mul_
				{
					opcode = OpCodes.Mul
				};
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x060003AB RID: 939 RVA: 0x000112F8 File Offset: 0x0000F4F8
		public static Code.Div_ Div
		{
			get
			{
				return new Code.Div_
				{
					opcode = OpCodes.Div
				};
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x060003AC RID: 940 RVA: 0x0001130A File Offset: 0x0000F50A
		public static Code.Div_Un_ Div_Un
		{
			get
			{
				return new Code.Div_Un_
				{
					opcode = OpCodes.Div_Un
				};
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x060003AD RID: 941 RVA: 0x0001131C File Offset: 0x0000F51C
		public static Code.Rem_ Rem
		{
			get
			{
				return new Code.Rem_
				{
					opcode = OpCodes.Rem
				};
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x060003AE RID: 942 RVA: 0x0001132E File Offset: 0x0000F52E
		public static Code.Rem_Un_ Rem_Un
		{
			get
			{
				return new Code.Rem_Un_
				{
					opcode = OpCodes.Rem_Un
				};
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x060003AF RID: 943 RVA: 0x00011340 File Offset: 0x0000F540
		public static Code.And_ And
		{
			get
			{
				return new Code.And_
				{
					opcode = OpCodes.And
				};
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x060003B0 RID: 944 RVA: 0x00011352 File Offset: 0x0000F552
		public static Code.Or_ Or
		{
			get
			{
				return new Code.Or_
				{
					opcode = OpCodes.Or
				};
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x060003B1 RID: 945 RVA: 0x00011364 File Offset: 0x0000F564
		public static Code.Xor_ Xor
		{
			get
			{
				return new Code.Xor_
				{
					opcode = OpCodes.Xor
				};
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x060003B2 RID: 946 RVA: 0x00011376 File Offset: 0x0000F576
		public static Code.Shl_ Shl
		{
			get
			{
				return new Code.Shl_
				{
					opcode = OpCodes.Shl
				};
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x060003B3 RID: 947 RVA: 0x00011388 File Offset: 0x0000F588
		public static Code.Shr_ Shr
		{
			get
			{
				return new Code.Shr_
				{
					opcode = OpCodes.Shr
				};
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x060003B4 RID: 948 RVA: 0x0001139A File Offset: 0x0000F59A
		public static Code.Shr_Un_ Shr_Un
		{
			get
			{
				return new Code.Shr_Un_
				{
					opcode = OpCodes.Shr_Un
				};
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x060003B5 RID: 949 RVA: 0x000113AC File Offset: 0x0000F5AC
		public static Code.Neg_ Neg
		{
			get
			{
				return new Code.Neg_
				{
					opcode = OpCodes.Neg
				};
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x060003B6 RID: 950 RVA: 0x000113BE File Offset: 0x0000F5BE
		public static Code.Not_ Not
		{
			get
			{
				return new Code.Not_
				{
					opcode = OpCodes.Not
				};
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x060003B7 RID: 951 RVA: 0x000113D0 File Offset: 0x0000F5D0
		public static Code.Conv_I1_ Conv_I1
		{
			get
			{
				return new Code.Conv_I1_
				{
					opcode = OpCodes.Conv_I1
				};
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x060003B8 RID: 952 RVA: 0x000113E2 File Offset: 0x0000F5E2
		public static Code.Conv_I2_ Conv_I2
		{
			get
			{
				return new Code.Conv_I2_
				{
					opcode = OpCodes.Conv_I2
				};
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x060003B9 RID: 953 RVA: 0x000113F4 File Offset: 0x0000F5F4
		public static Code.Conv_I4_ Conv_I4
		{
			get
			{
				return new Code.Conv_I4_
				{
					opcode = OpCodes.Conv_I4
				};
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x060003BA RID: 954 RVA: 0x00011406 File Offset: 0x0000F606
		public static Code.Conv_I8_ Conv_I8
		{
			get
			{
				return new Code.Conv_I8_
				{
					opcode = OpCodes.Conv_I8
				};
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x060003BB RID: 955 RVA: 0x00011418 File Offset: 0x0000F618
		public static Code.Conv_R4_ Conv_R4
		{
			get
			{
				return new Code.Conv_R4_
				{
					opcode = OpCodes.Conv_R4
				};
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x060003BC RID: 956 RVA: 0x0001142A File Offset: 0x0000F62A
		public static Code.Conv_R8_ Conv_R8
		{
			get
			{
				return new Code.Conv_R8_
				{
					opcode = OpCodes.Conv_R8
				};
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x060003BD RID: 957 RVA: 0x0001143C File Offset: 0x0000F63C
		public static Code.Conv_U4_ Conv_U4
		{
			get
			{
				return new Code.Conv_U4_
				{
					opcode = OpCodes.Conv_U4
				};
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x060003BE RID: 958 RVA: 0x0001144E File Offset: 0x0000F64E
		public static Code.Conv_U8_ Conv_U8
		{
			get
			{
				return new Code.Conv_U8_
				{
					opcode = OpCodes.Conv_U8
				};
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060003BF RID: 959 RVA: 0x00011460 File Offset: 0x0000F660
		public static Code.Callvirt_ Callvirt
		{
			get
			{
				return new Code.Callvirt_
				{
					opcode = OpCodes.Callvirt
				};
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060003C0 RID: 960 RVA: 0x00011472 File Offset: 0x0000F672
		public static Code.Cpobj_ Cpobj
		{
			get
			{
				return new Code.Cpobj_
				{
					opcode = OpCodes.Cpobj
				};
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060003C1 RID: 961 RVA: 0x00011484 File Offset: 0x0000F684
		public static Code.Ldobj_ Ldobj
		{
			get
			{
				return new Code.Ldobj_
				{
					opcode = OpCodes.Ldobj
				};
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060003C2 RID: 962 RVA: 0x00011496 File Offset: 0x0000F696
		public static Code.Ldstr_ Ldstr
		{
			get
			{
				return new Code.Ldstr_
				{
					opcode = OpCodes.Ldstr
				};
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060003C3 RID: 963 RVA: 0x000114A8 File Offset: 0x0000F6A8
		public static Code.Newobj_ Newobj
		{
			get
			{
				return new Code.Newobj_
				{
					opcode = OpCodes.Newobj
				};
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060003C4 RID: 964 RVA: 0x000114BA File Offset: 0x0000F6BA
		public static Code.Castclass_ Castclass
		{
			get
			{
				return new Code.Castclass_
				{
					opcode = OpCodes.Castclass
				};
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060003C5 RID: 965 RVA: 0x000114CC File Offset: 0x0000F6CC
		public static Code.Isinst_ Isinst
		{
			get
			{
				return new Code.Isinst_
				{
					opcode = OpCodes.Isinst
				};
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060003C6 RID: 966 RVA: 0x000114DE File Offset: 0x0000F6DE
		public static Code.Conv_R_Un_ Conv_R_Un
		{
			get
			{
				return new Code.Conv_R_Un_
				{
					opcode = OpCodes.Conv_R_Un
				};
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060003C7 RID: 967 RVA: 0x000114F0 File Offset: 0x0000F6F0
		public static Code.Unbox_ Unbox
		{
			get
			{
				return new Code.Unbox_
				{
					opcode = OpCodes.Unbox
				};
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060003C8 RID: 968 RVA: 0x00011502 File Offset: 0x0000F702
		public static Code.Throw_ Throw
		{
			get
			{
				return new Code.Throw_
				{
					opcode = OpCodes.Throw
				};
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060003C9 RID: 969 RVA: 0x00011514 File Offset: 0x0000F714
		public static Code.Ldfld_ Ldfld
		{
			get
			{
				return new Code.Ldfld_
				{
					opcode = OpCodes.Ldfld
				};
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060003CA RID: 970 RVA: 0x00011526 File Offset: 0x0000F726
		public static Code.Ldflda_ Ldflda
		{
			get
			{
				return new Code.Ldflda_
				{
					opcode = OpCodes.Ldflda
				};
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x060003CB RID: 971 RVA: 0x00011538 File Offset: 0x0000F738
		public static Code.Stfld_ Stfld
		{
			get
			{
				return new Code.Stfld_
				{
					opcode = OpCodes.Stfld
				};
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x060003CC RID: 972 RVA: 0x0001154A File Offset: 0x0000F74A
		public static Code.Ldsfld_ Ldsfld
		{
			get
			{
				return new Code.Ldsfld_
				{
					opcode = OpCodes.Ldsfld
				};
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x060003CD RID: 973 RVA: 0x0001155C File Offset: 0x0000F75C
		public static Code.Ldsflda_ Ldsflda
		{
			get
			{
				return new Code.Ldsflda_
				{
					opcode = OpCodes.Ldsflda
				};
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x060003CE RID: 974 RVA: 0x0001156E File Offset: 0x0000F76E
		public static Code.Stsfld_ Stsfld
		{
			get
			{
				return new Code.Stsfld_
				{
					opcode = OpCodes.Stsfld
				};
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x060003CF RID: 975 RVA: 0x00011580 File Offset: 0x0000F780
		public static Code.Stobj_ Stobj
		{
			get
			{
				return new Code.Stobj_
				{
					opcode = OpCodes.Stobj
				};
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x060003D0 RID: 976 RVA: 0x00011592 File Offset: 0x0000F792
		public static Code.Conv_Ovf_I1_Un_ Conv_Ovf_I1_Un
		{
			get
			{
				return new Code.Conv_Ovf_I1_Un_
				{
					opcode = OpCodes.Conv_Ovf_I1_Un
				};
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x060003D1 RID: 977 RVA: 0x000115A4 File Offset: 0x0000F7A4
		public static Code.Conv_Ovf_I2_Un_ Conv_Ovf_I2_Un
		{
			get
			{
				return new Code.Conv_Ovf_I2_Un_
				{
					opcode = OpCodes.Conv_Ovf_I2_Un
				};
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x060003D2 RID: 978 RVA: 0x000115B6 File Offset: 0x0000F7B6
		public static Code.Conv_Ovf_I4_Un_ Conv_Ovf_I4_Un
		{
			get
			{
				return new Code.Conv_Ovf_I4_Un_
				{
					opcode = OpCodes.Conv_Ovf_I4_Un
				};
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x060003D3 RID: 979 RVA: 0x000115C8 File Offset: 0x0000F7C8
		public static Code.Conv_Ovf_I8_Un_ Conv_Ovf_I8_Un
		{
			get
			{
				return new Code.Conv_Ovf_I8_Un_
				{
					opcode = OpCodes.Conv_Ovf_I8_Un
				};
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x060003D4 RID: 980 RVA: 0x000115DA File Offset: 0x0000F7DA
		public static Code.Conv_Ovf_U1_Un_ Conv_Ovf_U1_Un
		{
			get
			{
				return new Code.Conv_Ovf_U1_Un_
				{
					opcode = OpCodes.Conv_Ovf_U1_Un
				};
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x060003D5 RID: 981 RVA: 0x000115EC File Offset: 0x0000F7EC
		public static Code.Conv_Ovf_U2_Un_ Conv_Ovf_U2_Un
		{
			get
			{
				return new Code.Conv_Ovf_U2_Un_
				{
					opcode = OpCodes.Conv_Ovf_U2_Un
				};
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x060003D6 RID: 982 RVA: 0x000115FE File Offset: 0x0000F7FE
		public static Code.Conv_Ovf_U4_Un_ Conv_Ovf_U4_Un
		{
			get
			{
				return new Code.Conv_Ovf_U4_Un_
				{
					opcode = OpCodes.Conv_Ovf_U4_Un
				};
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x060003D7 RID: 983 RVA: 0x00011610 File Offset: 0x0000F810
		public static Code.Conv_Ovf_U8_Un_ Conv_Ovf_U8_Un
		{
			get
			{
				return new Code.Conv_Ovf_U8_Un_
				{
					opcode = OpCodes.Conv_Ovf_U8_Un
				};
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x060003D8 RID: 984 RVA: 0x00011622 File Offset: 0x0000F822
		public static Code.Conv_Ovf_I_Un_ Conv_Ovf_I_Un
		{
			get
			{
				return new Code.Conv_Ovf_I_Un_
				{
					opcode = OpCodes.Conv_Ovf_I_Un
				};
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x060003D9 RID: 985 RVA: 0x00011634 File Offset: 0x0000F834
		public static Code.Conv_Ovf_U_Un_ Conv_Ovf_U_Un
		{
			get
			{
				return new Code.Conv_Ovf_U_Un_
				{
					opcode = OpCodes.Conv_Ovf_U_Un
				};
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x060003DA RID: 986 RVA: 0x00011646 File Offset: 0x0000F846
		public static Code.Box_ Box
		{
			get
			{
				return new Code.Box_
				{
					opcode = OpCodes.Box
				};
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x060003DB RID: 987 RVA: 0x00011658 File Offset: 0x0000F858
		public static Code.Newarr_ Newarr
		{
			get
			{
				return new Code.Newarr_
				{
					opcode = OpCodes.Newarr
				};
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x060003DC RID: 988 RVA: 0x0001166A File Offset: 0x0000F86A
		public static Code.Ldlen_ Ldlen
		{
			get
			{
				return new Code.Ldlen_
				{
					opcode = OpCodes.Ldlen
				};
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x060003DD RID: 989 RVA: 0x0001167C File Offset: 0x0000F87C
		public static Code.Ldelema_ Ldelema
		{
			get
			{
				return new Code.Ldelema_
				{
					opcode = OpCodes.Ldelema
				};
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x060003DE RID: 990 RVA: 0x0001168E File Offset: 0x0000F88E
		public static Code.Ldelem_I1_ Ldelem_I1
		{
			get
			{
				return new Code.Ldelem_I1_
				{
					opcode = OpCodes.Ldelem_I1
				};
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x060003DF RID: 991 RVA: 0x000116A0 File Offset: 0x0000F8A0
		public static Code.Ldelem_U1_ Ldelem_U1
		{
			get
			{
				return new Code.Ldelem_U1_
				{
					opcode = OpCodes.Ldelem_U1
				};
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x060003E0 RID: 992 RVA: 0x000116B2 File Offset: 0x0000F8B2
		public static Code.Ldelem_I2_ Ldelem_I2
		{
			get
			{
				return new Code.Ldelem_I2_
				{
					opcode = OpCodes.Ldelem_I2
				};
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x060003E1 RID: 993 RVA: 0x000116C4 File Offset: 0x0000F8C4
		public static Code.Ldelem_U2_ Ldelem_U2
		{
			get
			{
				return new Code.Ldelem_U2_
				{
					opcode = OpCodes.Ldelem_U2
				};
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x060003E2 RID: 994 RVA: 0x000116D6 File Offset: 0x0000F8D6
		public static Code.Ldelem_I4_ Ldelem_I4
		{
			get
			{
				return new Code.Ldelem_I4_
				{
					opcode = OpCodes.Ldelem_I4
				};
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x060003E3 RID: 995 RVA: 0x000116E8 File Offset: 0x0000F8E8
		public static Code.Ldelem_U4_ Ldelem_U4
		{
			get
			{
				return new Code.Ldelem_U4_
				{
					opcode = OpCodes.Ldelem_U4
				};
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x060003E4 RID: 996 RVA: 0x000116FA File Offset: 0x0000F8FA
		public static Code.Ldelem_I8_ Ldelem_I8
		{
			get
			{
				return new Code.Ldelem_I8_
				{
					opcode = OpCodes.Ldelem_I8
				};
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x060003E5 RID: 997 RVA: 0x0001170C File Offset: 0x0000F90C
		public static Code.Ldelem_I_ Ldelem_I
		{
			get
			{
				return new Code.Ldelem_I_
				{
					opcode = OpCodes.Ldelem_I
				};
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x060003E6 RID: 998 RVA: 0x0001171E File Offset: 0x0000F91E
		public static Code.Ldelem_R4_ Ldelem_R4
		{
			get
			{
				return new Code.Ldelem_R4_
				{
					opcode = OpCodes.Ldelem_R4
				};
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x060003E7 RID: 999 RVA: 0x00011730 File Offset: 0x0000F930
		public static Code.Ldelem_R8_ Ldelem_R8
		{
			get
			{
				return new Code.Ldelem_R8_
				{
					opcode = OpCodes.Ldelem_R8
				};
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x060003E8 RID: 1000 RVA: 0x00011742 File Offset: 0x0000F942
		public static Code.Ldelem_Ref_ Ldelem_Ref
		{
			get
			{
				return new Code.Ldelem_Ref_
				{
					opcode = OpCodes.Ldelem_Ref
				};
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x060003E9 RID: 1001 RVA: 0x00011754 File Offset: 0x0000F954
		public static Code.Stelem_I_ Stelem_I
		{
			get
			{
				return new Code.Stelem_I_
				{
					opcode = OpCodes.Stelem_I
				};
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x060003EA RID: 1002 RVA: 0x00011766 File Offset: 0x0000F966
		public static Code.Stelem_I1_ Stelem_I1
		{
			get
			{
				return new Code.Stelem_I1_
				{
					opcode = OpCodes.Stelem_I1
				};
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x060003EB RID: 1003 RVA: 0x00011778 File Offset: 0x0000F978
		public static Code.Stelem_I2_ Stelem_I2
		{
			get
			{
				return new Code.Stelem_I2_
				{
					opcode = OpCodes.Stelem_I2
				};
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x060003EC RID: 1004 RVA: 0x0001178A File Offset: 0x0000F98A
		public static Code.Stelem_I4_ Stelem_I4
		{
			get
			{
				return new Code.Stelem_I4_
				{
					opcode = OpCodes.Stelem_I4
				};
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x060003ED RID: 1005 RVA: 0x0001179C File Offset: 0x0000F99C
		public static Code.Stelem_I8_ Stelem_I8
		{
			get
			{
				return new Code.Stelem_I8_
				{
					opcode = OpCodes.Stelem_I8
				};
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x060003EE RID: 1006 RVA: 0x000117AE File Offset: 0x0000F9AE
		public static Code.Stelem_R4_ Stelem_R4
		{
			get
			{
				return new Code.Stelem_R4_
				{
					opcode = OpCodes.Stelem_R4
				};
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x060003EF RID: 1007 RVA: 0x000117C0 File Offset: 0x0000F9C0
		public static Code.Stelem_R8_ Stelem_R8
		{
			get
			{
				return new Code.Stelem_R8_
				{
					opcode = OpCodes.Stelem_R8
				};
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x060003F0 RID: 1008 RVA: 0x000117D2 File Offset: 0x0000F9D2
		public static Code.Stelem_Ref_ Stelem_Ref
		{
			get
			{
				return new Code.Stelem_Ref_
				{
					opcode = OpCodes.Stelem_Ref
				};
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x060003F1 RID: 1009 RVA: 0x000117E4 File Offset: 0x0000F9E4
		public static Code.Ldelem_ Ldelem
		{
			get
			{
				return new Code.Ldelem_
				{
					opcode = OpCodes.Ldelem
				};
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x060003F2 RID: 1010 RVA: 0x000117F6 File Offset: 0x0000F9F6
		public static Code.Stelem_ Stelem
		{
			get
			{
				return new Code.Stelem_
				{
					opcode = OpCodes.Stelem
				};
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x060003F3 RID: 1011 RVA: 0x00011808 File Offset: 0x0000FA08
		public static Code.Unbox_Any_ Unbox_Any
		{
			get
			{
				return new Code.Unbox_Any_
				{
					opcode = OpCodes.Unbox_Any
				};
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x060003F4 RID: 1012 RVA: 0x0001181A File Offset: 0x0000FA1A
		public static Code.Conv_Ovf_I1_ Conv_Ovf_I1
		{
			get
			{
				return new Code.Conv_Ovf_I1_
				{
					opcode = OpCodes.Conv_Ovf_I1
				};
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x060003F5 RID: 1013 RVA: 0x0001182C File Offset: 0x0000FA2C
		public static Code.Conv_Ovf_U1_ Conv_Ovf_U1
		{
			get
			{
				return new Code.Conv_Ovf_U1_
				{
					opcode = OpCodes.Conv_Ovf_U1
				};
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x060003F6 RID: 1014 RVA: 0x0001183E File Offset: 0x0000FA3E
		public static Code.Conv_Ovf_I2_ Conv_Ovf_I2
		{
			get
			{
				return new Code.Conv_Ovf_I2_
				{
					opcode = OpCodes.Conv_Ovf_I2
				};
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x060003F7 RID: 1015 RVA: 0x00011850 File Offset: 0x0000FA50
		public static Code.Conv_Ovf_U2_ Conv_Ovf_U2
		{
			get
			{
				return new Code.Conv_Ovf_U2_
				{
					opcode = OpCodes.Conv_Ovf_U2
				};
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x060003F8 RID: 1016 RVA: 0x00011862 File Offset: 0x0000FA62
		public static Code.Conv_Ovf_I4_ Conv_Ovf_I4
		{
			get
			{
				return new Code.Conv_Ovf_I4_
				{
					opcode = OpCodes.Conv_Ovf_I4
				};
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x060003F9 RID: 1017 RVA: 0x00011874 File Offset: 0x0000FA74
		public static Code.Conv_Ovf_U4_ Conv_Ovf_U4
		{
			get
			{
				return new Code.Conv_Ovf_U4_
				{
					opcode = OpCodes.Conv_Ovf_U4
				};
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x060003FA RID: 1018 RVA: 0x00011886 File Offset: 0x0000FA86
		public static Code.Conv_Ovf_I8_ Conv_Ovf_I8
		{
			get
			{
				return new Code.Conv_Ovf_I8_
				{
					opcode = OpCodes.Conv_Ovf_I8
				};
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x060003FB RID: 1019 RVA: 0x00011898 File Offset: 0x0000FA98
		public static Code.Conv_Ovf_U8_ Conv_Ovf_U8
		{
			get
			{
				return new Code.Conv_Ovf_U8_
				{
					opcode = OpCodes.Conv_Ovf_U8
				};
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x060003FC RID: 1020 RVA: 0x000118AA File Offset: 0x0000FAAA
		public static Code.Refanyval_ Refanyval
		{
			get
			{
				return new Code.Refanyval_
				{
					opcode = OpCodes.Refanyval
				};
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x060003FD RID: 1021 RVA: 0x000118BC File Offset: 0x0000FABC
		public static Code.Ckfinite_ Ckfinite
		{
			get
			{
				return new Code.Ckfinite_
				{
					opcode = OpCodes.Ckfinite
				};
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x060003FE RID: 1022 RVA: 0x000118CE File Offset: 0x0000FACE
		public static Code.Mkrefany_ Mkrefany
		{
			get
			{
				return new Code.Mkrefany_
				{
					opcode = OpCodes.Mkrefany
				};
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x060003FF RID: 1023 RVA: 0x000118E0 File Offset: 0x0000FAE0
		public static Code.Ldtoken_ Ldtoken
		{
			get
			{
				return new Code.Ldtoken_
				{
					opcode = OpCodes.Ldtoken
				};
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x06000400 RID: 1024 RVA: 0x000118F2 File Offset: 0x0000FAF2
		public static Code.Conv_U2_ Conv_U2
		{
			get
			{
				return new Code.Conv_U2_
				{
					opcode = OpCodes.Conv_U2
				};
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x06000401 RID: 1025 RVA: 0x00011904 File Offset: 0x0000FB04
		public static Code.Conv_U1_ Conv_U1
		{
			get
			{
				return new Code.Conv_U1_
				{
					opcode = OpCodes.Conv_U1
				};
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000402 RID: 1026 RVA: 0x00011916 File Offset: 0x0000FB16
		public static Code.Conv_I_ Conv_I
		{
			get
			{
				return new Code.Conv_I_
				{
					opcode = OpCodes.Conv_I
				};
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000403 RID: 1027 RVA: 0x00011928 File Offset: 0x0000FB28
		public static Code.Conv_Ovf_I_ Conv_Ovf_I
		{
			get
			{
				return new Code.Conv_Ovf_I_
				{
					opcode = OpCodes.Conv_Ovf_I
				};
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000404 RID: 1028 RVA: 0x0001193A File Offset: 0x0000FB3A
		public static Code.Conv_Ovf_U_ Conv_Ovf_U
		{
			get
			{
				return new Code.Conv_Ovf_U_
				{
					opcode = OpCodes.Conv_Ovf_U
				};
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000405 RID: 1029 RVA: 0x0001194C File Offset: 0x0000FB4C
		public static Code.Add_Ovf_ Add_Ovf
		{
			get
			{
				return new Code.Add_Ovf_
				{
					opcode = OpCodes.Add_Ovf
				};
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x06000406 RID: 1030 RVA: 0x0001195E File Offset: 0x0000FB5E
		public static Code.Add_Ovf_Un_ Add_Ovf_Un
		{
			get
			{
				return new Code.Add_Ovf_Un_
				{
					opcode = OpCodes.Add_Ovf_Un
				};
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x06000407 RID: 1031 RVA: 0x00011970 File Offset: 0x0000FB70
		public static Code.Mul_Ovf_ Mul_Ovf
		{
			get
			{
				return new Code.Mul_Ovf_
				{
					opcode = OpCodes.Mul_Ovf
				};
			}
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x06000408 RID: 1032 RVA: 0x00011982 File Offset: 0x0000FB82
		public static Code.Mul_Ovf_Un_ Mul_Ovf_Un
		{
			get
			{
				return new Code.Mul_Ovf_Un_
				{
					opcode = OpCodes.Mul_Ovf_Un
				};
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x06000409 RID: 1033 RVA: 0x00011994 File Offset: 0x0000FB94
		public static Code.Sub_Ovf_ Sub_Ovf
		{
			get
			{
				return new Code.Sub_Ovf_
				{
					opcode = OpCodes.Sub_Ovf
				};
			}
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x0600040A RID: 1034 RVA: 0x000119A6 File Offset: 0x0000FBA6
		public static Code.Sub_Ovf_Un_ Sub_Ovf_Un
		{
			get
			{
				return new Code.Sub_Ovf_Un_
				{
					opcode = OpCodes.Sub_Ovf_Un
				};
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x0600040B RID: 1035 RVA: 0x000119B8 File Offset: 0x0000FBB8
		public static Code.Endfinally_ Endfinally
		{
			get
			{
				return new Code.Endfinally_
				{
					opcode = OpCodes.Endfinally
				};
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x0600040C RID: 1036 RVA: 0x000119CA File Offset: 0x0000FBCA
		public static Code.Leave_ Leave
		{
			get
			{
				return new Code.Leave_
				{
					opcode = OpCodes.Leave
				};
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x0600040D RID: 1037 RVA: 0x000119DC File Offset: 0x0000FBDC
		public static Code.Leave_S_ Leave_S
		{
			get
			{
				return new Code.Leave_S_
				{
					opcode = OpCodes.Leave_S
				};
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x0600040E RID: 1038 RVA: 0x000119EE File Offset: 0x0000FBEE
		public static Code.Stind_I_ Stind_I
		{
			get
			{
				return new Code.Stind_I_
				{
					opcode = OpCodes.Stind_I
				};
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x0600040F RID: 1039 RVA: 0x00011A00 File Offset: 0x0000FC00
		public static Code.Conv_U_ Conv_U
		{
			get
			{
				return new Code.Conv_U_
				{
					opcode = OpCodes.Conv_U
				};
			}
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x06000410 RID: 1040 RVA: 0x00011A12 File Offset: 0x0000FC12
		public static Code.Prefix7_ Prefix7
		{
			get
			{
				return new Code.Prefix7_
				{
					opcode = OpCodes.Prefix7
				};
			}
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x06000411 RID: 1041 RVA: 0x00011A24 File Offset: 0x0000FC24
		public static Code.Prefix6_ Prefix6
		{
			get
			{
				return new Code.Prefix6_
				{
					opcode = OpCodes.Prefix6
				};
			}
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x06000412 RID: 1042 RVA: 0x00011A36 File Offset: 0x0000FC36
		public static Code.Prefix5_ Prefix5
		{
			get
			{
				return new Code.Prefix5_
				{
					opcode = OpCodes.Prefix5
				};
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x06000413 RID: 1043 RVA: 0x00011A48 File Offset: 0x0000FC48
		public static Code.Prefix4_ Prefix4
		{
			get
			{
				return new Code.Prefix4_
				{
					opcode = OpCodes.Prefix4
				};
			}
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x06000414 RID: 1044 RVA: 0x00011A5A File Offset: 0x0000FC5A
		public static Code.Prefix3_ Prefix3
		{
			get
			{
				return new Code.Prefix3_
				{
					opcode = OpCodes.Prefix3
				};
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x06000415 RID: 1045 RVA: 0x00011A6C File Offset: 0x0000FC6C
		public static Code.Prefix2_ Prefix2
		{
			get
			{
				return new Code.Prefix2_
				{
					opcode = OpCodes.Prefix2
				};
			}
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x06000416 RID: 1046 RVA: 0x00011A7E File Offset: 0x0000FC7E
		public static Code.Prefix1_ Prefix1
		{
			get
			{
				return new Code.Prefix1_
				{
					opcode = OpCodes.Prefix1
				};
			}
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x06000417 RID: 1047 RVA: 0x00011A90 File Offset: 0x0000FC90
		public static Code.Prefixref_ Prefixref
		{
			get
			{
				return new Code.Prefixref_
				{
					opcode = OpCodes.Prefixref
				};
			}
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x06000418 RID: 1048 RVA: 0x00011AA2 File Offset: 0x0000FCA2
		public static Code.Arglist_ Arglist
		{
			get
			{
				return new Code.Arglist_
				{
					opcode = OpCodes.Arglist
				};
			}
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x06000419 RID: 1049 RVA: 0x00011AB4 File Offset: 0x0000FCB4
		public static Code.Ceq_ Ceq
		{
			get
			{
				return new Code.Ceq_
				{
					opcode = OpCodes.Ceq
				};
			}
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x0600041A RID: 1050 RVA: 0x00011AC6 File Offset: 0x0000FCC6
		public static Code.Cgt_ Cgt
		{
			get
			{
				return new Code.Cgt_
				{
					opcode = OpCodes.Cgt
				};
			}
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x0600041B RID: 1051 RVA: 0x00011AD8 File Offset: 0x0000FCD8
		public static Code.Cgt_Un_ Cgt_Un
		{
			get
			{
				return new Code.Cgt_Un_
				{
					opcode = OpCodes.Cgt_Un
				};
			}
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x0600041C RID: 1052 RVA: 0x00011AEA File Offset: 0x0000FCEA
		public static Code.Clt_ Clt
		{
			get
			{
				return new Code.Clt_
				{
					opcode = OpCodes.Clt
				};
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x0600041D RID: 1053 RVA: 0x00011AFC File Offset: 0x0000FCFC
		public static Code.Clt_Un_ Clt_Un
		{
			get
			{
				return new Code.Clt_Un_
				{
					opcode = OpCodes.Clt_Un
				};
			}
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x0600041E RID: 1054 RVA: 0x00011B0E File Offset: 0x0000FD0E
		public static Code.Ldftn_ Ldftn
		{
			get
			{
				return new Code.Ldftn_
				{
					opcode = OpCodes.Ldftn
				};
			}
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x0600041F RID: 1055 RVA: 0x00011B20 File Offset: 0x0000FD20
		public static Code.Ldvirtftn_ Ldvirtftn
		{
			get
			{
				return new Code.Ldvirtftn_
				{
					opcode = OpCodes.Ldvirtftn
				};
			}
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x06000420 RID: 1056 RVA: 0x00011B32 File Offset: 0x0000FD32
		public static Code.Ldarg_ Ldarg
		{
			get
			{
				return new Code.Ldarg_
				{
					opcode = OpCodes.Ldarg
				};
			}
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x06000421 RID: 1057 RVA: 0x00011B44 File Offset: 0x0000FD44
		public static Code.Ldarga_ Ldarga
		{
			get
			{
				return new Code.Ldarga_
				{
					opcode = OpCodes.Ldarga
				};
			}
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x06000422 RID: 1058 RVA: 0x00011B56 File Offset: 0x0000FD56
		public static Code.Starg_ Starg
		{
			get
			{
				return new Code.Starg_
				{
					opcode = OpCodes.Starg
				};
			}
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x06000423 RID: 1059 RVA: 0x00011B68 File Offset: 0x0000FD68
		public static Code.Ldloc_ Ldloc
		{
			get
			{
				return new Code.Ldloc_
				{
					opcode = OpCodes.Ldloc
				};
			}
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x06000424 RID: 1060 RVA: 0x00011B7A File Offset: 0x0000FD7A
		public static Code.Ldloca_ Ldloca
		{
			get
			{
				return new Code.Ldloca_
				{
					opcode = OpCodes.Ldloca
				};
			}
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x06000425 RID: 1061 RVA: 0x00011B8C File Offset: 0x0000FD8C
		public static Code.Stloc_ Stloc
		{
			get
			{
				return new Code.Stloc_
				{
					opcode = OpCodes.Stloc
				};
			}
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x06000426 RID: 1062 RVA: 0x00011B9E File Offset: 0x0000FD9E
		public static Code.Localloc_ Localloc
		{
			get
			{
				return new Code.Localloc_
				{
					opcode = OpCodes.Localloc
				};
			}
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x06000427 RID: 1063 RVA: 0x00011BB0 File Offset: 0x0000FDB0
		public static Code.Endfilter_ Endfilter
		{
			get
			{
				return new Code.Endfilter_
				{
					opcode = OpCodes.Endfilter
				};
			}
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x06000428 RID: 1064 RVA: 0x00011BC2 File Offset: 0x0000FDC2
		public static Code.Unaligned_ Unaligned
		{
			get
			{
				return new Code.Unaligned_
				{
					opcode = OpCodes.Unaligned
				};
			}
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x06000429 RID: 1065 RVA: 0x00011BD4 File Offset: 0x0000FDD4
		public static Code.Volatile_ Volatile
		{
			get
			{
				return new Code.Volatile_
				{
					opcode = OpCodes.Volatile
				};
			}
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x0600042A RID: 1066 RVA: 0x00011BE6 File Offset: 0x0000FDE6
		public static Code.Tailcall_ Tailcall
		{
			get
			{
				return new Code.Tailcall_
				{
					opcode = OpCodes.Tailcall
				};
			}
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x0600042B RID: 1067 RVA: 0x00011BF8 File Offset: 0x0000FDF8
		public static Code.Initobj_ Initobj
		{
			get
			{
				return new Code.Initobj_
				{
					opcode = OpCodes.Initobj
				};
			}
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x0600042C RID: 1068 RVA: 0x00011C0A File Offset: 0x0000FE0A
		public static Code.Constrained_ Constrained
		{
			get
			{
				return new Code.Constrained_
				{
					opcode = OpCodes.Constrained
				};
			}
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x0600042D RID: 1069 RVA: 0x00011C1C File Offset: 0x0000FE1C
		public static Code.Cpblk_ Cpblk
		{
			get
			{
				return new Code.Cpblk_
				{
					opcode = OpCodes.Cpblk
				};
			}
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x0600042E RID: 1070 RVA: 0x00011C2E File Offset: 0x0000FE2E
		public static Code.Initblk_ Initblk
		{
			get
			{
				return new Code.Initblk_
				{
					opcode = OpCodes.Initblk
				};
			}
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x0600042F RID: 1071 RVA: 0x00011C40 File Offset: 0x0000FE40
		public static Code.Rethrow_ Rethrow
		{
			get
			{
				return new Code.Rethrow_
				{
					opcode = OpCodes.Rethrow
				};
			}
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x06000430 RID: 1072 RVA: 0x00011C52 File Offset: 0x0000FE52
		public static Code.Sizeof_ Sizeof
		{
			get
			{
				return new Code.Sizeof_
				{
					opcode = OpCodes.Sizeof
				};
			}
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x06000431 RID: 1073 RVA: 0x00011C64 File Offset: 0x0000FE64
		public static Code.Refanytype_ Refanytype
		{
			get
			{
				return new Code.Refanytype_
				{
					opcode = OpCodes.Refanytype
				};
			}
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x06000432 RID: 1074 RVA: 0x00011C76 File Offset: 0x0000FE76
		public static Code.Readonly_ Readonly
		{
			get
			{
				return new Code.Readonly_
				{
					opcode = OpCodes.Readonly
				};
			}
		}

		// Token: 0x020000A0 RID: 160
		public class Operand_ : CodeMatch
		{
			// Token: 0x170000F9 RID: 249
			public Code.Operand_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Operand_)base.Set(operand, name);
				}
			}

			// Token: 0x06000434 RID: 1076 RVA: 0x00011C98 File Offset: 0x0000FE98
			public Operand_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000A1 RID: 161
		public class Nop_ : CodeMatch
		{
			// Token: 0x170000FA RID: 250
			public Code.Nop_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Nop_)base.Set(operand, name);
				}
			}

			// Token: 0x06000436 RID: 1078 RVA: 0x00011CC8 File Offset: 0x0000FEC8
			public Nop_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000A2 RID: 162
		public class Break_ : CodeMatch
		{
			// Token: 0x170000FB RID: 251
			public Code.Break_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Break_)base.Set(operand, name);
				}
			}

			// Token: 0x06000438 RID: 1080 RVA: 0x00011CF8 File Offset: 0x0000FEF8
			public Break_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000A3 RID: 163
		public class Ldarg_0_ : CodeMatch
		{
			// Token: 0x170000FC RID: 252
			public Code.Ldarg_0_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldarg_0_)base.Set(operand, name);
				}
			}

			// Token: 0x0600043A RID: 1082 RVA: 0x00011D28 File Offset: 0x0000FF28
			public Ldarg_0_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000A4 RID: 164
		public class Ldarg_1_ : CodeMatch
		{
			// Token: 0x170000FD RID: 253
			public Code.Ldarg_1_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldarg_1_)base.Set(operand, name);
				}
			}

			// Token: 0x0600043C RID: 1084 RVA: 0x00011D58 File Offset: 0x0000FF58
			public Ldarg_1_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000A5 RID: 165
		public class Ldarg_2_ : CodeMatch
		{
			// Token: 0x170000FE RID: 254
			public Code.Ldarg_2_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldarg_2_)base.Set(operand, name);
				}
			}

			// Token: 0x0600043E RID: 1086 RVA: 0x00011D88 File Offset: 0x0000FF88
			public Ldarg_2_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000A6 RID: 166
		public class Ldarg_3_ : CodeMatch
		{
			// Token: 0x170000FF RID: 255
			public Code.Ldarg_3_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldarg_3_)base.Set(operand, name);
				}
			}

			// Token: 0x06000440 RID: 1088 RVA: 0x00011DB8 File Offset: 0x0000FFB8
			public Ldarg_3_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000A7 RID: 167
		public class Ldloc_0_ : CodeMatch
		{
			// Token: 0x17000100 RID: 256
			public Code.Ldloc_0_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldloc_0_)base.Set(operand, name);
				}
			}

			// Token: 0x06000442 RID: 1090 RVA: 0x00011DE8 File Offset: 0x0000FFE8
			public Ldloc_0_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000A8 RID: 168
		public class Ldloc_1_ : CodeMatch
		{
			// Token: 0x17000101 RID: 257
			public Code.Ldloc_1_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldloc_1_)base.Set(operand, name);
				}
			}

			// Token: 0x06000444 RID: 1092 RVA: 0x00011E18 File Offset: 0x00010018
			public Ldloc_1_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000A9 RID: 169
		public class Ldloc_2_ : CodeMatch
		{
			// Token: 0x17000102 RID: 258
			public Code.Ldloc_2_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldloc_2_)base.Set(operand, name);
				}
			}

			// Token: 0x06000446 RID: 1094 RVA: 0x00011E48 File Offset: 0x00010048
			public Ldloc_2_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000AA RID: 170
		public class Ldloc_3_ : CodeMatch
		{
			// Token: 0x17000103 RID: 259
			public Code.Ldloc_3_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldloc_3_)base.Set(operand, name);
				}
			}

			// Token: 0x06000448 RID: 1096 RVA: 0x00011E78 File Offset: 0x00010078
			public Ldloc_3_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000AB RID: 171
		public class Stloc_0_ : CodeMatch
		{
			// Token: 0x17000104 RID: 260
			public Code.Stloc_0_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Stloc_0_)base.Set(operand, name);
				}
			}

			// Token: 0x0600044A RID: 1098 RVA: 0x00011EA8 File Offset: 0x000100A8
			public Stloc_0_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000AC RID: 172
		public class Stloc_1_ : CodeMatch
		{
			// Token: 0x17000105 RID: 261
			public Code.Stloc_1_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Stloc_1_)base.Set(operand, name);
				}
			}

			// Token: 0x0600044C RID: 1100 RVA: 0x00011ED8 File Offset: 0x000100D8
			public Stloc_1_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000AD RID: 173
		public class Stloc_2_ : CodeMatch
		{
			// Token: 0x17000106 RID: 262
			public Code.Stloc_2_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Stloc_2_)base.Set(operand, name);
				}
			}

			// Token: 0x0600044E RID: 1102 RVA: 0x00011F08 File Offset: 0x00010108
			public Stloc_2_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000AE RID: 174
		public class Stloc_3_ : CodeMatch
		{
			// Token: 0x17000107 RID: 263
			public Code.Stloc_3_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Stloc_3_)base.Set(operand, name);
				}
			}

			// Token: 0x06000450 RID: 1104 RVA: 0x00011F38 File Offset: 0x00010138
			public Stloc_3_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000AF RID: 175
		public class Ldarg_S_ : CodeMatch
		{
			// Token: 0x17000108 RID: 264
			public Code.Ldarg_S_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldarg_S_)base.Set(operand, name);
				}
			}

			// Token: 0x06000452 RID: 1106 RVA: 0x00011F68 File Offset: 0x00010168
			public Ldarg_S_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000B0 RID: 176
		public class Ldarga_S_ : CodeMatch
		{
			// Token: 0x17000109 RID: 265
			public Code.Ldarga_S_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldarga_S_)base.Set(operand, name);
				}
			}

			// Token: 0x06000454 RID: 1108 RVA: 0x00011F98 File Offset: 0x00010198
			public Ldarga_S_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000B1 RID: 177
		public class Starg_S_ : CodeMatch
		{
			// Token: 0x1700010A RID: 266
			public Code.Starg_S_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Starg_S_)base.Set(operand, name);
				}
			}

			// Token: 0x06000456 RID: 1110 RVA: 0x00011FC8 File Offset: 0x000101C8
			public Starg_S_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000B2 RID: 178
		public class Ldloc_S_ : CodeMatch
		{
			// Token: 0x1700010B RID: 267
			public Code.Ldloc_S_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldloc_S_)base.Set(operand, name);
				}
			}

			// Token: 0x06000458 RID: 1112 RVA: 0x00011FF8 File Offset: 0x000101F8
			public Ldloc_S_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000B3 RID: 179
		public class Ldloca_S_ : CodeMatch
		{
			// Token: 0x1700010C RID: 268
			public Code.Ldloca_S_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldloca_S_)base.Set(operand, name);
				}
			}

			// Token: 0x0600045A RID: 1114 RVA: 0x00012028 File Offset: 0x00010228
			public Ldloca_S_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000B4 RID: 180
		public class Stloc_S_ : CodeMatch
		{
			// Token: 0x1700010D RID: 269
			public Code.Stloc_S_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Stloc_S_)base.Set(operand, name);
				}
			}

			// Token: 0x0600045C RID: 1116 RVA: 0x00012058 File Offset: 0x00010258
			public Stloc_S_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000B5 RID: 181
		public class Ldnull_ : CodeMatch
		{
			// Token: 0x1700010E RID: 270
			public Code.Ldnull_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldnull_)base.Set(operand, name);
				}
			}

			// Token: 0x0600045E RID: 1118 RVA: 0x00012088 File Offset: 0x00010288
			public Ldnull_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000B6 RID: 182
		public class Ldc_I4_M1_ : CodeMatch
		{
			// Token: 0x1700010F RID: 271
			public Code.Ldc_I4_M1_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldc_I4_M1_)base.Set(operand, name);
				}
			}

			// Token: 0x06000460 RID: 1120 RVA: 0x000120B8 File Offset: 0x000102B8
			public Ldc_I4_M1_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000B7 RID: 183
		public class Ldc_I4_0_ : CodeMatch
		{
			// Token: 0x17000110 RID: 272
			public Code.Ldc_I4_0_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldc_I4_0_)base.Set(operand, name);
				}
			}

			// Token: 0x06000462 RID: 1122 RVA: 0x000120E8 File Offset: 0x000102E8
			public Ldc_I4_0_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000B8 RID: 184
		public class Ldc_I4_1_ : CodeMatch
		{
			// Token: 0x17000111 RID: 273
			public Code.Ldc_I4_1_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldc_I4_1_)base.Set(operand, name);
				}
			}

			// Token: 0x06000464 RID: 1124 RVA: 0x00012118 File Offset: 0x00010318
			public Ldc_I4_1_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000B9 RID: 185
		public class Ldc_I4_2_ : CodeMatch
		{
			// Token: 0x17000112 RID: 274
			public Code.Ldc_I4_2_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldc_I4_2_)base.Set(operand, name);
				}
			}

			// Token: 0x06000466 RID: 1126 RVA: 0x00012148 File Offset: 0x00010348
			public Ldc_I4_2_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000BA RID: 186
		public class Ldc_I4_3_ : CodeMatch
		{
			// Token: 0x17000113 RID: 275
			public Code.Ldc_I4_3_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldc_I4_3_)base.Set(operand, name);
				}
			}

			// Token: 0x06000468 RID: 1128 RVA: 0x00012178 File Offset: 0x00010378
			public Ldc_I4_3_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000BB RID: 187
		public class Ldc_I4_4_ : CodeMatch
		{
			// Token: 0x17000114 RID: 276
			public Code.Ldc_I4_4_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldc_I4_4_)base.Set(operand, name);
				}
			}

			// Token: 0x0600046A RID: 1130 RVA: 0x000121A8 File Offset: 0x000103A8
			public Ldc_I4_4_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000BC RID: 188
		public class Ldc_I4_5_ : CodeMatch
		{
			// Token: 0x17000115 RID: 277
			public Code.Ldc_I4_5_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldc_I4_5_)base.Set(operand, name);
				}
			}

			// Token: 0x0600046C RID: 1132 RVA: 0x000121D8 File Offset: 0x000103D8
			public Ldc_I4_5_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000BD RID: 189
		public class Ldc_I4_6_ : CodeMatch
		{
			// Token: 0x17000116 RID: 278
			public Code.Ldc_I4_6_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldc_I4_6_)base.Set(operand, name);
				}
			}

			// Token: 0x0600046E RID: 1134 RVA: 0x00012208 File Offset: 0x00010408
			public Ldc_I4_6_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000BE RID: 190
		public class Ldc_I4_7_ : CodeMatch
		{
			// Token: 0x17000117 RID: 279
			public Code.Ldc_I4_7_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldc_I4_7_)base.Set(operand, name);
				}
			}

			// Token: 0x06000470 RID: 1136 RVA: 0x00012238 File Offset: 0x00010438
			public Ldc_I4_7_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000BF RID: 191
		public class Ldc_I4_8_ : CodeMatch
		{
			// Token: 0x17000118 RID: 280
			public Code.Ldc_I4_8_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldc_I4_8_)base.Set(operand, name);
				}
			}

			// Token: 0x06000472 RID: 1138 RVA: 0x00012268 File Offset: 0x00010468
			public Ldc_I4_8_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000C0 RID: 192
		public class Ldc_I4_S_ : CodeMatch
		{
			// Token: 0x17000119 RID: 281
			public Code.Ldc_I4_S_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldc_I4_S_)base.Set(operand, name);
				}
			}

			// Token: 0x06000474 RID: 1140 RVA: 0x00012298 File Offset: 0x00010498
			public Ldc_I4_S_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000C1 RID: 193
		public class Ldc_I4_ : CodeMatch
		{
			// Token: 0x1700011A RID: 282
			public Code.Ldc_I4_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldc_I4_)base.Set(operand, name);
				}
			}

			// Token: 0x06000476 RID: 1142 RVA: 0x000122C8 File Offset: 0x000104C8
			public Ldc_I4_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000C2 RID: 194
		public class Ldc_I8_ : CodeMatch
		{
			// Token: 0x1700011B RID: 283
			public Code.Ldc_I8_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldc_I8_)base.Set(operand, name);
				}
			}

			// Token: 0x06000478 RID: 1144 RVA: 0x000122F8 File Offset: 0x000104F8
			public Ldc_I8_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000C3 RID: 195
		public class Ldc_R4_ : CodeMatch
		{
			// Token: 0x1700011C RID: 284
			public Code.Ldc_R4_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldc_R4_)base.Set(operand, name);
				}
			}

			// Token: 0x0600047A RID: 1146 RVA: 0x00012328 File Offset: 0x00010528
			public Ldc_R4_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000C4 RID: 196
		public class Ldc_R8_ : CodeMatch
		{
			// Token: 0x1700011D RID: 285
			public Code.Ldc_R8_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldc_R8_)base.Set(operand, name);
				}
			}

			// Token: 0x0600047C RID: 1148 RVA: 0x00012358 File Offset: 0x00010558
			public Ldc_R8_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000C5 RID: 197
		public class Dup_ : CodeMatch
		{
			// Token: 0x1700011E RID: 286
			public Code.Dup_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Dup_)base.Set(operand, name);
				}
			}

			// Token: 0x0600047E RID: 1150 RVA: 0x00012388 File Offset: 0x00010588
			public Dup_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000C6 RID: 198
		public class Pop_ : CodeMatch
		{
			// Token: 0x1700011F RID: 287
			public Code.Pop_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Pop_)base.Set(operand, name);
				}
			}

			// Token: 0x06000480 RID: 1152 RVA: 0x000123B8 File Offset: 0x000105B8
			public Pop_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000C7 RID: 199
		public class Jmp_ : CodeMatch
		{
			// Token: 0x17000120 RID: 288
			public Code.Jmp_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Jmp_)base.Set(operand, name);
				}
			}

			// Token: 0x06000482 RID: 1154 RVA: 0x000123E8 File Offset: 0x000105E8
			public Jmp_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000C8 RID: 200
		public class Call_ : CodeMatch
		{
			// Token: 0x17000121 RID: 289
			public Code.Call_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Call_)base.Set(operand, name);
				}
			}

			// Token: 0x06000484 RID: 1156 RVA: 0x00012418 File Offset: 0x00010618
			public Call_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000C9 RID: 201
		public class Calli_ : CodeMatch
		{
			// Token: 0x17000122 RID: 290
			public Code.Calli_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Calli_)base.Set(operand, name);
				}
			}

			// Token: 0x06000486 RID: 1158 RVA: 0x00012448 File Offset: 0x00010648
			public Calli_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000CA RID: 202
		public class Ret_ : CodeMatch
		{
			// Token: 0x17000123 RID: 291
			public Code.Ret_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ret_)base.Set(operand, name);
				}
			}

			// Token: 0x06000488 RID: 1160 RVA: 0x00012478 File Offset: 0x00010678
			public Ret_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000CB RID: 203
		public class Br_S_ : CodeMatch
		{
			// Token: 0x17000124 RID: 292
			public Code.Br_S_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Br_S_)base.Set(operand, name);
				}
			}

			// Token: 0x0600048A RID: 1162 RVA: 0x000124A8 File Offset: 0x000106A8
			public Br_S_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000CC RID: 204
		public class Brfalse_S_ : CodeMatch
		{
			// Token: 0x17000125 RID: 293
			public Code.Brfalse_S_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Brfalse_S_)base.Set(operand, name);
				}
			}

			// Token: 0x0600048C RID: 1164 RVA: 0x000124D8 File Offset: 0x000106D8
			public Brfalse_S_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000CD RID: 205
		public class Brtrue_S_ : CodeMatch
		{
			// Token: 0x17000126 RID: 294
			public Code.Brtrue_S_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Brtrue_S_)base.Set(operand, name);
				}
			}

			// Token: 0x0600048E RID: 1166 RVA: 0x00012508 File Offset: 0x00010708
			public Brtrue_S_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000CE RID: 206
		public class Beq_S_ : CodeMatch
		{
			// Token: 0x17000127 RID: 295
			public Code.Beq_S_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Beq_S_)base.Set(operand, name);
				}
			}

			// Token: 0x06000490 RID: 1168 RVA: 0x00012538 File Offset: 0x00010738
			public Beq_S_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000CF RID: 207
		public class Bge_S_ : CodeMatch
		{
			// Token: 0x17000128 RID: 296
			public Code.Bge_S_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Bge_S_)base.Set(operand, name);
				}
			}

			// Token: 0x06000492 RID: 1170 RVA: 0x00012568 File Offset: 0x00010768
			public Bge_S_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000D0 RID: 208
		public class Bgt_S_ : CodeMatch
		{
			// Token: 0x17000129 RID: 297
			public Code.Bgt_S_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Bgt_S_)base.Set(operand, name);
				}
			}

			// Token: 0x06000494 RID: 1172 RVA: 0x00012598 File Offset: 0x00010798
			public Bgt_S_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000D1 RID: 209
		public class Ble_S_ : CodeMatch
		{
			// Token: 0x1700012A RID: 298
			public Code.Ble_S_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ble_S_)base.Set(operand, name);
				}
			}

			// Token: 0x06000496 RID: 1174 RVA: 0x000125C8 File Offset: 0x000107C8
			public Ble_S_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000D2 RID: 210
		public class Blt_S_ : CodeMatch
		{
			// Token: 0x1700012B RID: 299
			public Code.Blt_S_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Blt_S_)base.Set(operand, name);
				}
			}

			// Token: 0x06000498 RID: 1176 RVA: 0x000125F8 File Offset: 0x000107F8
			public Blt_S_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000D3 RID: 211
		public class Bne_Un_S_ : CodeMatch
		{
			// Token: 0x1700012C RID: 300
			public Code.Bne_Un_S_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Bne_Un_S_)base.Set(operand, name);
				}
			}

			// Token: 0x0600049A RID: 1178 RVA: 0x00012628 File Offset: 0x00010828
			public Bne_Un_S_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000D4 RID: 212
		public class Bge_Un_S_ : CodeMatch
		{
			// Token: 0x1700012D RID: 301
			public Code.Bge_Un_S_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Bge_Un_S_)base.Set(operand, name);
				}
			}

			// Token: 0x0600049C RID: 1180 RVA: 0x00012658 File Offset: 0x00010858
			public Bge_Un_S_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000D5 RID: 213
		public class Bgt_Un_S_ : CodeMatch
		{
			// Token: 0x1700012E RID: 302
			public Code.Bgt_Un_S_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Bgt_Un_S_)base.Set(operand, name);
				}
			}

			// Token: 0x0600049E RID: 1182 RVA: 0x00012688 File Offset: 0x00010888
			public Bgt_Un_S_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000D6 RID: 214
		public class Ble_Un_S_ : CodeMatch
		{
			// Token: 0x1700012F RID: 303
			public Code.Ble_Un_S_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ble_Un_S_)base.Set(operand, name);
				}
			}

			// Token: 0x060004A0 RID: 1184 RVA: 0x000126B8 File Offset: 0x000108B8
			public Ble_Un_S_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000D7 RID: 215
		public class Blt_Un_S_ : CodeMatch
		{
			// Token: 0x17000130 RID: 304
			public Code.Blt_Un_S_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Blt_Un_S_)base.Set(operand, name);
				}
			}

			// Token: 0x060004A2 RID: 1186 RVA: 0x000126E8 File Offset: 0x000108E8
			public Blt_Un_S_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000D8 RID: 216
		public class Br_ : CodeMatch
		{
			// Token: 0x17000131 RID: 305
			public Code.Br_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Br_)base.Set(operand, name);
				}
			}

			// Token: 0x060004A4 RID: 1188 RVA: 0x00012718 File Offset: 0x00010918
			public Br_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000D9 RID: 217
		public class Brfalse_ : CodeMatch
		{
			// Token: 0x17000132 RID: 306
			public Code.Brfalse_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Brfalse_)base.Set(operand, name);
				}
			}

			// Token: 0x060004A6 RID: 1190 RVA: 0x00012748 File Offset: 0x00010948
			public Brfalse_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000DA RID: 218
		public class Brtrue_ : CodeMatch
		{
			// Token: 0x17000133 RID: 307
			public Code.Brtrue_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Brtrue_)base.Set(operand, name);
				}
			}

			// Token: 0x060004A8 RID: 1192 RVA: 0x00012778 File Offset: 0x00010978
			public Brtrue_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000DB RID: 219
		public class Beq_ : CodeMatch
		{
			// Token: 0x17000134 RID: 308
			public Code.Beq_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Beq_)base.Set(operand, name);
				}
			}

			// Token: 0x060004AA RID: 1194 RVA: 0x000127A8 File Offset: 0x000109A8
			public Beq_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000DC RID: 220
		public class Bge_ : CodeMatch
		{
			// Token: 0x17000135 RID: 309
			public Code.Bge_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Bge_)base.Set(operand, name);
				}
			}

			// Token: 0x060004AC RID: 1196 RVA: 0x000127D8 File Offset: 0x000109D8
			public Bge_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000DD RID: 221
		public class Bgt_ : CodeMatch
		{
			// Token: 0x17000136 RID: 310
			public Code.Bgt_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Bgt_)base.Set(operand, name);
				}
			}

			// Token: 0x060004AE RID: 1198 RVA: 0x00012808 File Offset: 0x00010A08
			public Bgt_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000DE RID: 222
		public class Ble_ : CodeMatch
		{
			// Token: 0x17000137 RID: 311
			public Code.Ble_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ble_)base.Set(operand, name);
				}
			}

			// Token: 0x060004B0 RID: 1200 RVA: 0x00012838 File Offset: 0x00010A38
			public Ble_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000DF RID: 223
		public class Blt_ : CodeMatch
		{
			// Token: 0x17000138 RID: 312
			public Code.Blt_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Blt_)base.Set(operand, name);
				}
			}

			// Token: 0x060004B2 RID: 1202 RVA: 0x00012868 File Offset: 0x00010A68
			public Blt_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000E0 RID: 224
		public class Bne_Un_ : CodeMatch
		{
			// Token: 0x17000139 RID: 313
			public Code.Bne_Un_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Bne_Un_)base.Set(operand, name);
				}
			}

			// Token: 0x060004B4 RID: 1204 RVA: 0x00012898 File Offset: 0x00010A98
			public Bne_Un_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000E1 RID: 225
		public class Bge_Un_ : CodeMatch
		{
			// Token: 0x1700013A RID: 314
			public Code.Bge_Un_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Bge_Un_)base.Set(operand, name);
				}
			}

			// Token: 0x060004B6 RID: 1206 RVA: 0x000128C8 File Offset: 0x00010AC8
			public Bge_Un_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000E2 RID: 226
		public class Bgt_Un_ : CodeMatch
		{
			// Token: 0x1700013B RID: 315
			public Code.Bgt_Un_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Bgt_Un_)base.Set(operand, name);
				}
			}

			// Token: 0x060004B8 RID: 1208 RVA: 0x000128F8 File Offset: 0x00010AF8
			public Bgt_Un_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000E3 RID: 227
		public class Ble_Un_ : CodeMatch
		{
			// Token: 0x1700013C RID: 316
			public Code.Ble_Un_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ble_Un_)base.Set(operand, name);
				}
			}

			// Token: 0x060004BA RID: 1210 RVA: 0x00012928 File Offset: 0x00010B28
			public Ble_Un_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000E4 RID: 228
		public class Blt_Un_ : CodeMatch
		{
			// Token: 0x1700013D RID: 317
			public Code.Blt_Un_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Blt_Un_)base.Set(operand, name);
				}
			}

			// Token: 0x060004BC RID: 1212 RVA: 0x00012958 File Offset: 0x00010B58
			public Blt_Un_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000E5 RID: 229
		public class Switch_ : CodeMatch
		{
			// Token: 0x1700013E RID: 318
			public Code.Switch_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Switch_)base.Set(operand, name);
				}
			}

			// Token: 0x060004BE RID: 1214 RVA: 0x00012988 File Offset: 0x00010B88
			public Switch_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000E6 RID: 230
		public class Ldind_I1_ : CodeMatch
		{
			// Token: 0x1700013F RID: 319
			public Code.Ldind_I1_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldind_I1_)base.Set(operand, name);
				}
			}

			// Token: 0x060004C0 RID: 1216 RVA: 0x000129B8 File Offset: 0x00010BB8
			public Ldind_I1_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000E7 RID: 231
		public class Ldind_U1_ : CodeMatch
		{
			// Token: 0x17000140 RID: 320
			public Code.Ldind_U1_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldind_U1_)base.Set(operand, name);
				}
			}

			// Token: 0x060004C2 RID: 1218 RVA: 0x000129E8 File Offset: 0x00010BE8
			public Ldind_U1_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000E8 RID: 232
		public class Ldind_I2_ : CodeMatch
		{
			// Token: 0x17000141 RID: 321
			public Code.Ldind_I2_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldind_I2_)base.Set(operand, name);
				}
			}

			// Token: 0x060004C4 RID: 1220 RVA: 0x00012A18 File Offset: 0x00010C18
			public Ldind_I2_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000E9 RID: 233
		public class Ldind_U2_ : CodeMatch
		{
			// Token: 0x17000142 RID: 322
			public Code.Ldind_U2_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldind_U2_)base.Set(operand, name);
				}
			}

			// Token: 0x060004C6 RID: 1222 RVA: 0x00012A48 File Offset: 0x00010C48
			public Ldind_U2_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000EA RID: 234
		public class Ldind_I4_ : CodeMatch
		{
			// Token: 0x17000143 RID: 323
			public Code.Ldind_I4_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldind_I4_)base.Set(operand, name);
				}
			}

			// Token: 0x060004C8 RID: 1224 RVA: 0x00012A78 File Offset: 0x00010C78
			public Ldind_I4_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000EB RID: 235
		public class Ldind_U4_ : CodeMatch
		{
			// Token: 0x17000144 RID: 324
			public Code.Ldind_U4_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldind_U4_)base.Set(operand, name);
				}
			}

			// Token: 0x060004CA RID: 1226 RVA: 0x00012AA8 File Offset: 0x00010CA8
			public Ldind_U4_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000EC RID: 236
		public class Ldind_I8_ : CodeMatch
		{
			// Token: 0x17000145 RID: 325
			public Code.Ldind_I8_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldind_I8_)base.Set(operand, name);
				}
			}

			// Token: 0x060004CC RID: 1228 RVA: 0x00012AD8 File Offset: 0x00010CD8
			public Ldind_I8_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000ED RID: 237
		public class Ldind_I_ : CodeMatch
		{
			// Token: 0x17000146 RID: 326
			public Code.Ldind_I_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldind_I_)base.Set(operand, name);
				}
			}

			// Token: 0x060004CE RID: 1230 RVA: 0x00012B08 File Offset: 0x00010D08
			public Ldind_I_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000EE RID: 238
		public class Ldind_R4_ : CodeMatch
		{
			// Token: 0x17000147 RID: 327
			public Code.Ldind_R4_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldind_R4_)base.Set(operand, name);
				}
			}

			// Token: 0x060004D0 RID: 1232 RVA: 0x00012B38 File Offset: 0x00010D38
			public Ldind_R4_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000EF RID: 239
		public class Ldind_R8_ : CodeMatch
		{
			// Token: 0x17000148 RID: 328
			public Code.Ldind_R8_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldind_R8_)base.Set(operand, name);
				}
			}

			// Token: 0x060004D2 RID: 1234 RVA: 0x00012B68 File Offset: 0x00010D68
			public Ldind_R8_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000F0 RID: 240
		public class Ldind_Ref_ : CodeMatch
		{
			// Token: 0x17000149 RID: 329
			public Code.Ldind_Ref_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldind_Ref_)base.Set(operand, name);
				}
			}

			// Token: 0x060004D4 RID: 1236 RVA: 0x00012B98 File Offset: 0x00010D98
			public Ldind_Ref_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000F1 RID: 241
		public class Stind_Ref_ : CodeMatch
		{
			// Token: 0x1700014A RID: 330
			public Code.Stind_Ref_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Stind_Ref_)base.Set(operand, name);
				}
			}

			// Token: 0x060004D6 RID: 1238 RVA: 0x00012BC8 File Offset: 0x00010DC8
			public Stind_Ref_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000F2 RID: 242
		public class Stind_I1_ : CodeMatch
		{
			// Token: 0x1700014B RID: 331
			public Code.Stind_I1_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Stind_I1_)base.Set(operand, name);
				}
			}

			// Token: 0x060004D8 RID: 1240 RVA: 0x00012BF8 File Offset: 0x00010DF8
			public Stind_I1_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000F3 RID: 243
		public class Stind_I2_ : CodeMatch
		{
			// Token: 0x1700014C RID: 332
			public Code.Stind_I2_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Stind_I2_)base.Set(operand, name);
				}
			}

			// Token: 0x060004DA RID: 1242 RVA: 0x00012C28 File Offset: 0x00010E28
			public Stind_I2_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000F4 RID: 244
		public class Stind_I4_ : CodeMatch
		{
			// Token: 0x1700014D RID: 333
			public Code.Stind_I4_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Stind_I4_)base.Set(operand, name);
				}
			}

			// Token: 0x060004DC RID: 1244 RVA: 0x00012C58 File Offset: 0x00010E58
			public Stind_I4_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000F5 RID: 245
		public class Stind_I8_ : CodeMatch
		{
			// Token: 0x1700014E RID: 334
			public Code.Stind_I8_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Stind_I8_)base.Set(operand, name);
				}
			}

			// Token: 0x060004DE RID: 1246 RVA: 0x00012C88 File Offset: 0x00010E88
			public Stind_I8_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000F6 RID: 246
		public class Stind_R4_ : CodeMatch
		{
			// Token: 0x1700014F RID: 335
			public Code.Stind_R4_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Stind_R4_)base.Set(operand, name);
				}
			}

			// Token: 0x060004E0 RID: 1248 RVA: 0x00012CB8 File Offset: 0x00010EB8
			public Stind_R4_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000F7 RID: 247
		public class Stind_R8_ : CodeMatch
		{
			// Token: 0x17000150 RID: 336
			public Code.Stind_R8_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Stind_R8_)base.Set(operand, name);
				}
			}

			// Token: 0x060004E2 RID: 1250 RVA: 0x00012CE8 File Offset: 0x00010EE8
			public Stind_R8_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000F8 RID: 248
		public class Add_ : CodeMatch
		{
			// Token: 0x17000151 RID: 337
			public Code.Add_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Add_)base.Set(operand, name);
				}
			}

			// Token: 0x060004E4 RID: 1252 RVA: 0x00012D18 File Offset: 0x00010F18
			public Add_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000F9 RID: 249
		public class Sub_ : CodeMatch
		{
			// Token: 0x17000152 RID: 338
			public Code.Sub_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Sub_)base.Set(operand, name);
				}
			}

			// Token: 0x060004E6 RID: 1254 RVA: 0x00012D48 File Offset: 0x00010F48
			public Sub_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000FA RID: 250
		public class Mul_ : CodeMatch
		{
			// Token: 0x17000153 RID: 339
			public Code.Mul_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Mul_)base.Set(operand, name);
				}
			}

			// Token: 0x060004E8 RID: 1256 RVA: 0x00012D78 File Offset: 0x00010F78
			public Mul_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000FB RID: 251
		public class Div_ : CodeMatch
		{
			// Token: 0x17000154 RID: 340
			public Code.Div_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Div_)base.Set(operand, name);
				}
			}

			// Token: 0x060004EA RID: 1258 RVA: 0x00012DA8 File Offset: 0x00010FA8
			public Div_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000FC RID: 252
		public class Div_Un_ : CodeMatch
		{
			// Token: 0x17000155 RID: 341
			public Code.Div_Un_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Div_Un_)base.Set(operand, name);
				}
			}

			// Token: 0x060004EC RID: 1260 RVA: 0x00012DD8 File Offset: 0x00010FD8
			public Div_Un_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000FD RID: 253
		public class Rem_ : CodeMatch
		{
			// Token: 0x17000156 RID: 342
			public Code.Rem_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Rem_)base.Set(operand, name);
				}
			}

			// Token: 0x060004EE RID: 1262 RVA: 0x00012E08 File Offset: 0x00011008
			public Rem_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000FE RID: 254
		public class Rem_Un_ : CodeMatch
		{
			// Token: 0x17000157 RID: 343
			public Code.Rem_Un_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Rem_Un_)base.Set(operand, name);
				}
			}

			// Token: 0x060004F0 RID: 1264 RVA: 0x00012E38 File Offset: 0x00011038
			public Rem_Un_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x020000FF RID: 255
		public class And_ : CodeMatch
		{
			// Token: 0x17000158 RID: 344
			public Code.And_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.And_)base.Set(operand, name);
				}
			}

			// Token: 0x060004F2 RID: 1266 RVA: 0x00012E68 File Offset: 0x00011068
			public And_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000100 RID: 256
		public class Or_ : CodeMatch
		{
			// Token: 0x17000159 RID: 345
			public Code.Or_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Or_)base.Set(operand, name);
				}
			}

			// Token: 0x060004F4 RID: 1268 RVA: 0x00012E98 File Offset: 0x00011098
			public Or_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000101 RID: 257
		public class Xor_ : CodeMatch
		{
			// Token: 0x1700015A RID: 346
			public Code.Xor_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Xor_)base.Set(operand, name);
				}
			}

			// Token: 0x060004F6 RID: 1270 RVA: 0x00012EC8 File Offset: 0x000110C8
			public Xor_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000102 RID: 258
		public class Shl_ : CodeMatch
		{
			// Token: 0x1700015B RID: 347
			public Code.Shl_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Shl_)base.Set(operand, name);
				}
			}

			// Token: 0x060004F8 RID: 1272 RVA: 0x00012EF8 File Offset: 0x000110F8
			public Shl_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000103 RID: 259
		public class Shr_ : CodeMatch
		{
			// Token: 0x1700015C RID: 348
			public Code.Shr_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Shr_)base.Set(operand, name);
				}
			}

			// Token: 0x060004FA RID: 1274 RVA: 0x00012F28 File Offset: 0x00011128
			public Shr_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000104 RID: 260
		public class Shr_Un_ : CodeMatch
		{
			// Token: 0x1700015D RID: 349
			public Code.Shr_Un_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Shr_Un_)base.Set(operand, name);
				}
			}

			// Token: 0x060004FC RID: 1276 RVA: 0x00012F58 File Offset: 0x00011158
			public Shr_Un_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000105 RID: 261
		public class Neg_ : CodeMatch
		{
			// Token: 0x1700015E RID: 350
			public Code.Neg_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Neg_)base.Set(operand, name);
				}
			}

			// Token: 0x060004FE RID: 1278 RVA: 0x00012F88 File Offset: 0x00011188
			public Neg_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000106 RID: 262
		public class Not_ : CodeMatch
		{
			// Token: 0x1700015F RID: 351
			public Code.Not_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Not_)base.Set(operand, name);
				}
			}

			// Token: 0x06000500 RID: 1280 RVA: 0x00012FB8 File Offset: 0x000111B8
			public Not_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000107 RID: 263
		public class Conv_I1_ : CodeMatch
		{
			// Token: 0x17000160 RID: 352
			public Code.Conv_I1_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Conv_I1_)base.Set(operand, name);
				}
			}

			// Token: 0x06000502 RID: 1282 RVA: 0x00012FE8 File Offset: 0x000111E8
			public Conv_I1_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000108 RID: 264
		public class Conv_I2_ : CodeMatch
		{
			// Token: 0x17000161 RID: 353
			public Code.Conv_I2_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Conv_I2_)base.Set(operand, name);
				}
			}

			// Token: 0x06000504 RID: 1284 RVA: 0x00013018 File Offset: 0x00011218
			public Conv_I2_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000109 RID: 265
		public class Conv_I4_ : CodeMatch
		{
			// Token: 0x17000162 RID: 354
			public Code.Conv_I4_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Conv_I4_)base.Set(operand, name);
				}
			}

			// Token: 0x06000506 RID: 1286 RVA: 0x00013048 File Offset: 0x00011248
			public Conv_I4_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200010A RID: 266
		public class Conv_I8_ : CodeMatch
		{
			// Token: 0x17000163 RID: 355
			public Code.Conv_I8_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Conv_I8_)base.Set(operand, name);
				}
			}

			// Token: 0x06000508 RID: 1288 RVA: 0x00013078 File Offset: 0x00011278
			public Conv_I8_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200010B RID: 267
		public class Conv_R4_ : CodeMatch
		{
			// Token: 0x17000164 RID: 356
			public Code.Conv_R4_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Conv_R4_)base.Set(operand, name);
				}
			}

			// Token: 0x0600050A RID: 1290 RVA: 0x000130A8 File Offset: 0x000112A8
			public Conv_R4_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200010C RID: 268
		public class Conv_R8_ : CodeMatch
		{
			// Token: 0x17000165 RID: 357
			public Code.Conv_R8_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Conv_R8_)base.Set(operand, name);
				}
			}

			// Token: 0x0600050C RID: 1292 RVA: 0x000130D8 File Offset: 0x000112D8
			public Conv_R8_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200010D RID: 269
		public class Conv_U4_ : CodeMatch
		{
			// Token: 0x17000166 RID: 358
			public Code.Conv_U4_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Conv_U4_)base.Set(operand, name);
				}
			}

			// Token: 0x0600050E RID: 1294 RVA: 0x00013108 File Offset: 0x00011308
			public Conv_U4_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200010E RID: 270
		public class Conv_U8_ : CodeMatch
		{
			// Token: 0x17000167 RID: 359
			public Code.Conv_U8_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Conv_U8_)base.Set(operand, name);
				}
			}

			// Token: 0x06000510 RID: 1296 RVA: 0x00013138 File Offset: 0x00011338
			public Conv_U8_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200010F RID: 271
		public class Callvirt_ : CodeMatch
		{
			// Token: 0x17000168 RID: 360
			public Code.Callvirt_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Callvirt_)base.Set(operand, name);
				}
			}

			// Token: 0x06000512 RID: 1298 RVA: 0x00013168 File Offset: 0x00011368
			public Callvirt_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000110 RID: 272
		public class Cpobj_ : CodeMatch
		{
			// Token: 0x17000169 RID: 361
			public Code.Cpobj_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Cpobj_)base.Set(operand, name);
				}
			}

			// Token: 0x06000514 RID: 1300 RVA: 0x00013198 File Offset: 0x00011398
			public Cpobj_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000111 RID: 273
		public class Ldobj_ : CodeMatch
		{
			// Token: 0x1700016A RID: 362
			public Code.Ldobj_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldobj_)base.Set(operand, name);
				}
			}

			// Token: 0x06000516 RID: 1302 RVA: 0x000131C8 File Offset: 0x000113C8
			public Ldobj_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000112 RID: 274
		public class Ldstr_ : CodeMatch
		{
			// Token: 0x1700016B RID: 363
			public Code.Ldstr_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldstr_)base.Set(operand, name);
				}
			}

			// Token: 0x06000518 RID: 1304 RVA: 0x000131F8 File Offset: 0x000113F8
			public Ldstr_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000113 RID: 275
		public class Newobj_ : CodeMatch
		{
			// Token: 0x1700016C RID: 364
			public Code.Newobj_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Newobj_)base.Set(operand, name);
				}
			}

			// Token: 0x0600051A RID: 1306 RVA: 0x00013228 File Offset: 0x00011428
			public Newobj_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000114 RID: 276
		public class Castclass_ : CodeMatch
		{
			// Token: 0x1700016D RID: 365
			public Code.Castclass_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Castclass_)base.Set(operand, name);
				}
			}

			// Token: 0x0600051C RID: 1308 RVA: 0x00013258 File Offset: 0x00011458
			public Castclass_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000115 RID: 277
		public class Isinst_ : CodeMatch
		{
			// Token: 0x1700016E RID: 366
			public Code.Isinst_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Isinst_)base.Set(operand, name);
				}
			}

			// Token: 0x0600051E RID: 1310 RVA: 0x00013288 File Offset: 0x00011488
			public Isinst_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000116 RID: 278
		public class Conv_R_Un_ : CodeMatch
		{
			// Token: 0x1700016F RID: 367
			public Code.Conv_R_Un_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Conv_R_Un_)base.Set(operand, name);
				}
			}

			// Token: 0x06000520 RID: 1312 RVA: 0x000132B8 File Offset: 0x000114B8
			public Conv_R_Un_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000117 RID: 279
		public class Unbox_ : CodeMatch
		{
			// Token: 0x17000170 RID: 368
			public Code.Unbox_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Unbox_)base.Set(operand, name);
				}
			}

			// Token: 0x06000522 RID: 1314 RVA: 0x000132E8 File Offset: 0x000114E8
			public Unbox_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000118 RID: 280
		public class Throw_ : CodeMatch
		{
			// Token: 0x17000171 RID: 369
			public Code.Throw_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Throw_)base.Set(operand, name);
				}
			}

			// Token: 0x06000524 RID: 1316 RVA: 0x00013318 File Offset: 0x00011518
			public Throw_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000119 RID: 281
		public class Ldfld_ : CodeMatch
		{
			// Token: 0x17000172 RID: 370
			public Code.Ldfld_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldfld_)base.Set(operand, name);
				}
			}

			// Token: 0x06000526 RID: 1318 RVA: 0x00013348 File Offset: 0x00011548
			public Ldfld_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200011A RID: 282
		public class Ldflda_ : CodeMatch
		{
			// Token: 0x17000173 RID: 371
			public Code.Ldflda_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldflda_)base.Set(operand, name);
				}
			}

			// Token: 0x06000528 RID: 1320 RVA: 0x00013378 File Offset: 0x00011578
			public Ldflda_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200011B RID: 283
		public class Stfld_ : CodeMatch
		{
			// Token: 0x17000174 RID: 372
			public Code.Stfld_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Stfld_)base.Set(operand, name);
				}
			}

			// Token: 0x0600052A RID: 1322 RVA: 0x000133A8 File Offset: 0x000115A8
			public Stfld_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200011C RID: 284
		public class Ldsfld_ : CodeMatch
		{
			// Token: 0x17000175 RID: 373
			public Code.Ldsfld_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldsfld_)base.Set(operand, name);
				}
			}

			// Token: 0x0600052C RID: 1324 RVA: 0x000133D8 File Offset: 0x000115D8
			public Ldsfld_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200011D RID: 285
		public class Ldsflda_ : CodeMatch
		{
			// Token: 0x17000176 RID: 374
			public Code.Ldsflda_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldsflda_)base.Set(operand, name);
				}
			}

			// Token: 0x0600052E RID: 1326 RVA: 0x00013408 File Offset: 0x00011608
			public Ldsflda_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200011E RID: 286
		public class Stsfld_ : CodeMatch
		{
			// Token: 0x17000177 RID: 375
			public Code.Stsfld_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Stsfld_)base.Set(operand, name);
				}
			}

			// Token: 0x06000530 RID: 1328 RVA: 0x00013438 File Offset: 0x00011638
			public Stsfld_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200011F RID: 287
		public class Stobj_ : CodeMatch
		{
			// Token: 0x17000178 RID: 376
			public Code.Stobj_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Stobj_)base.Set(operand, name);
				}
			}

			// Token: 0x06000532 RID: 1330 RVA: 0x00013468 File Offset: 0x00011668
			public Stobj_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000120 RID: 288
		public class Conv_Ovf_I1_Un_ : CodeMatch
		{
			// Token: 0x17000179 RID: 377
			public Code.Conv_Ovf_I1_Un_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Conv_Ovf_I1_Un_)base.Set(operand, name);
				}
			}

			// Token: 0x06000534 RID: 1332 RVA: 0x00013498 File Offset: 0x00011698
			public Conv_Ovf_I1_Un_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000121 RID: 289
		public class Conv_Ovf_I2_Un_ : CodeMatch
		{
			// Token: 0x1700017A RID: 378
			public Code.Conv_Ovf_I2_Un_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Conv_Ovf_I2_Un_)base.Set(operand, name);
				}
			}

			// Token: 0x06000536 RID: 1334 RVA: 0x000134C8 File Offset: 0x000116C8
			public Conv_Ovf_I2_Un_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000122 RID: 290
		public class Conv_Ovf_I4_Un_ : CodeMatch
		{
			// Token: 0x1700017B RID: 379
			public Code.Conv_Ovf_I4_Un_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Conv_Ovf_I4_Un_)base.Set(operand, name);
				}
			}

			// Token: 0x06000538 RID: 1336 RVA: 0x000134F8 File Offset: 0x000116F8
			public Conv_Ovf_I4_Un_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000123 RID: 291
		public class Conv_Ovf_I8_Un_ : CodeMatch
		{
			// Token: 0x1700017C RID: 380
			public Code.Conv_Ovf_I8_Un_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Conv_Ovf_I8_Un_)base.Set(operand, name);
				}
			}

			// Token: 0x0600053A RID: 1338 RVA: 0x00013528 File Offset: 0x00011728
			public Conv_Ovf_I8_Un_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000124 RID: 292
		public class Conv_Ovf_U1_Un_ : CodeMatch
		{
			// Token: 0x1700017D RID: 381
			public Code.Conv_Ovf_U1_Un_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Conv_Ovf_U1_Un_)base.Set(operand, name);
				}
			}

			// Token: 0x0600053C RID: 1340 RVA: 0x00013558 File Offset: 0x00011758
			public Conv_Ovf_U1_Un_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000125 RID: 293
		public class Conv_Ovf_U2_Un_ : CodeMatch
		{
			// Token: 0x1700017E RID: 382
			public Code.Conv_Ovf_U2_Un_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Conv_Ovf_U2_Un_)base.Set(operand, name);
				}
			}

			// Token: 0x0600053E RID: 1342 RVA: 0x00013588 File Offset: 0x00011788
			public Conv_Ovf_U2_Un_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000126 RID: 294
		public class Conv_Ovf_U4_Un_ : CodeMatch
		{
			// Token: 0x1700017F RID: 383
			public Code.Conv_Ovf_U4_Un_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Conv_Ovf_U4_Un_)base.Set(operand, name);
				}
			}

			// Token: 0x06000540 RID: 1344 RVA: 0x000135B8 File Offset: 0x000117B8
			public Conv_Ovf_U4_Un_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000127 RID: 295
		public class Conv_Ovf_U8_Un_ : CodeMatch
		{
			// Token: 0x17000180 RID: 384
			public Code.Conv_Ovf_U8_Un_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Conv_Ovf_U8_Un_)base.Set(operand, name);
				}
			}

			// Token: 0x06000542 RID: 1346 RVA: 0x000135E8 File Offset: 0x000117E8
			public Conv_Ovf_U8_Un_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000128 RID: 296
		public class Conv_Ovf_I_Un_ : CodeMatch
		{
			// Token: 0x17000181 RID: 385
			public Code.Conv_Ovf_I_Un_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Conv_Ovf_I_Un_)base.Set(operand, name);
				}
			}

			// Token: 0x06000544 RID: 1348 RVA: 0x00013618 File Offset: 0x00011818
			public Conv_Ovf_I_Un_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000129 RID: 297
		public class Conv_Ovf_U_Un_ : CodeMatch
		{
			// Token: 0x17000182 RID: 386
			public Code.Conv_Ovf_U_Un_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Conv_Ovf_U_Un_)base.Set(operand, name);
				}
			}

			// Token: 0x06000546 RID: 1350 RVA: 0x00013648 File Offset: 0x00011848
			public Conv_Ovf_U_Un_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200012A RID: 298
		public class Box_ : CodeMatch
		{
			// Token: 0x17000183 RID: 387
			public Code.Box_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Box_)base.Set(operand, name);
				}
			}

			// Token: 0x06000548 RID: 1352 RVA: 0x00013678 File Offset: 0x00011878
			public Box_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200012B RID: 299
		public class Newarr_ : CodeMatch
		{
			// Token: 0x17000184 RID: 388
			public Code.Newarr_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Newarr_)base.Set(operand, name);
				}
			}

			// Token: 0x0600054A RID: 1354 RVA: 0x000136A8 File Offset: 0x000118A8
			public Newarr_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200012C RID: 300
		public class Ldlen_ : CodeMatch
		{
			// Token: 0x17000185 RID: 389
			public Code.Ldlen_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldlen_)base.Set(operand, name);
				}
			}

			// Token: 0x0600054C RID: 1356 RVA: 0x000136D8 File Offset: 0x000118D8
			public Ldlen_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200012D RID: 301
		public class Ldelema_ : CodeMatch
		{
			// Token: 0x17000186 RID: 390
			public Code.Ldelema_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldelema_)base.Set(operand, name);
				}
			}

			// Token: 0x0600054E RID: 1358 RVA: 0x00013708 File Offset: 0x00011908
			public Ldelema_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200012E RID: 302
		public class Ldelem_I1_ : CodeMatch
		{
			// Token: 0x17000187 RID: 391
			public Code.Ldelem_I1_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldelem_I1_)base.Set(operand, name);
				}
			}

			// Token: 0x06000550 RID: 1360 RVA: 0x00013738 File Offset: 0x00011938
			public Ldelem_I1_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200012F RID: 303
		public class Ldelem_U1_ : CodeMatch
		{
			// Token: 0x17000188 RID: 392
			public Code.Ldelem_U1_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldelem_U1_)base.Set(operand, name);
				}
			}

			// Token: 0x06000552 RID: 1362 RVA: 0x00013768 File Offset: 0x00011968
			public Ldelem_U1_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000130 RID: 304
		public class Ldelem_I2_ : CodeMatch
		{
			// Token: 0x17000189 RID: 393
			public Code.Ldelem_I2_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldelem_I2_)base.Set(operand, name);
				}
			}

			// Token: 0x06000554 RID: 1364 RVA: 0x00013798 File Offset: 0x00011998
			public Ldelem_I2_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000131 RID: 305
		public class Ldelem_U2_ : CodeMatch
		{
			// Token: 0x1700018A RID: 394
			public Code.Ldelem_U2_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldelem_U2_)base.Set(operand, name);
				}
			}

			// Token: 0x06000556 RID: 1366 RVA: 0x000137C8 File Offset: 0x000119C8
			public Ldelem_U2_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000132 RID: 306
		public class Ldelem_I4_ : CodeMatch
		{
			// Token: 0x1700018B RID: 395
			public Code.Ldelem_I4_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldelem_I4_)base.Set(operand, name);
				}
			}

			// Token: 0x06000558 RID: 1368 RVA: 0x000137F8 File Offset: 0x000119F8
			public Ldelem_I4_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000133 RID: 307
		public class Ldelem_U4_ : CodeMatch
		{
			// Token: 0x1700018C RID: 396
			public Code.Ldelem_U4_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldelem_U4_)base.Set(operand, name);
				}
			}

			// Token: 0x0600055A RID: 1370 RVA: 0x00013828 File Offset: 0x00011A28
			public Ldelem_U4_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000134 RID: 308
		public class Ldelem_I8_ : CodeMatch
		{
			// Token: 0x1700018D RID: 397
			public Code.Ldelem_I8_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldelem_I8_)base.Set(operand, name);
				}
			}

			// Token: 0x0600055C RID: 1372 RVA: 0x00013858 File Offset: 0x00011A58
			public Ldelem_I8_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000135 RID: 309
		public class Ldelem_I_ : CodeMatch
		{
			// Token: 0x1700018E RID: 398
			public Code.Ldelem_I_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldelem_I_)base.Set(operand, name);
				}
			}

			// Token: 0x0600055E RID: 1374 RVA: 0x00013888 File Offset: 0x00011A88
			public Ldelem_I_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000136 RID: 310
		public class Ldelem_R4_ : CodeMatch
		{
			// Token: 0x1700018F RID: 399
			public Code.Ldelem_R4_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldelem_R4_)base.Set(operand, name);
				}
			}

			// Token: 0x06000560 RID: 1376 RVA: 0x000138B8 File Offset: 0x00011AB8
			public Ldelem_R4_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000137 RID: 311
		public class Ldelem_R8_ : CodeMatch
		{
			// Token: 0x17000190 RID: 400
			public Code.Ldelem_R8_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldelem_R8_)base.Set(operand, name);
				}
			}

			// Token: 0x06000562 RID: 1378 RVA: 0x000138E8 File Offset: 0x00011AE8
			public Ldelem_R8_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000138 RID: 312
		public class Ldelem_Ref_ : CodeMatch
		{
			// Token: 0x17000191 RID: 401
			public Code.Ldelem_Ref_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldelem_Ref_)base.Set(operand, name);
				}
			}

			// Token: 0x06000564 RID: 1380 RVA: 0x00013918 File Offset: 0x00011B18
			public Ldelem_Ref_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000139 RID: 313
		public class Stelem_I_ : CodeMatch
		{
			// Token: 0x17000192 RID: 402
			public Code.Stelem_I_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Stelem_I_)base.Set(operand, name);
				}
			}

			// Token: 0x06000566 RID: 1382 RVA: 0x00013948 File Offset: 0x00011B48
			public Stelem_I_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200013A RID: 314
		public class Stelem_I1_ : CodeMatch
		{
			// Token: 0x17000193 RID: 403
			public Code.Stelem_I1_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Stelem_I1_)base.Set(operand, name);
				}
			}

			// Token: 0x06000568 RID: 1384 RVA: 0x00013978 File Offset: 0x00011B78
			public Stelem_I1_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200013B RID: 315
		public class Stelem_I2_ : CodeMatch
		{
			// Token: 0x17000194 RID: 404
			public Code.Stelem_I2_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Stelem_I2_)base.Set(operand, name);
				}
			}

			// Token: 0x0600056A RID: 1386 RVA: 0x000139A8 File Offset: 0x00011BA8
			public Stelem_I2_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200013C RID: 316
		public class Stelem_I4_ : CodeMatch
		{
			// Token: 0x17000195 RID: 405
			public Code.Stelem_I4_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Stelem_I4_)base.Set(operand, name);
				}
			}

			// Token: 0x0600056C RID: 1388 RVA: 0x000139D8 File Offset: 0x00011BD8
			public Stelem_I4_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200013D RID: 317
		public class Stelem_I8_ : CodeMatch
		{
			// Token: 0x17000196 RID: 406
			public Code.Stelem_I8_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Stelem_I8_)base.Set(operand, name);
				}
			}

			// Token: 0x0600056E RID: 1390 RVA: 0x00013A08 File Offset: 0x00011C08
			public Stelem_I8_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200013E RID: 318
		public class Stelem_R4_ : CodeMatch
		{
			// Token: 0x17000197 RID: 407
			public Code.Stelem_R4_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Stelem_R4_)base.Set(operand, name);
				}
			}

			// Token: 0x06000570 RID: 1392 RVA: 0x00013A38 File Offset: 0x00011C38
			public Stelem_R4_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200013F RID: 319
		public class Stelem_R8_ : CodeMatch
		{
			// Token: 0x17000198 RID: 408
			public Code.Stelem_R8_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Stelem_R8_)base.Set(operand, name);
				}
			}

			// Token: 0x06000572 RID: 1394 RVA: 0x00013A68 File Offset: 0x00011C68
			public Stelem_R8_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000140 RID: 320
		public class Stelem_Ref_ : CodeMatch
		{
			// Token: 0x17000199 RID: 409
			public Code.Stelem_Ref_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Stelem_Ref_)base.Set(operand, name);
				}
			}

			// Token: 0x06000574 RID: 1396 RVA: 0x00013A98 File Offset: 0x00011C98
			public Stelem_Ref_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000141 RID: 321
		public class Ldelem_ : CodeMatch
		{
			// Token: 0x1700019A RID: 410
			public Code.Ldelem_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldelem_)base.Set(operand, name);
				}
			}

			// Token: 0x06000576 RID: 1398 RVA: 0x00013AC8 File Offset: 0x00011CC8
			public Ldelem_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000142 RID: 322
		public class Stelem_ : CodeMatch
		{
			// Token: 0x1700019B RID: 411
			public Code.Stelem_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Stelem_)base.Set(operand, name);
				}
			}

			// Token: 0x06000578 RID: 1400 RVA: 0x00013AF8 File Offset: 0x00011CF8
			public Stelem_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000143 RID: 323
		public class Unbox_Any_ : CodeMatch
		{
			// Token: 0x1700019C RID: 412
			public Code.Unbox_Any_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Unbox_Any_)base.Set(operand, name);
				}
			}

			// Token: 0x0600057A RID: 1402 RVA: 0x00013B28 File Offset: 0x00011D28
			public Unbox_Any_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000144 RID: 324
		public class Conv_Ovf_I1_ : CodeMatch
		{
			// Token: 0x1700019D RID: 413
			public Code.Conv_Ovf_I1_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Conv_Ovf_I1_)base.Set(operand, name);
				}
			}

			// Token: 0x0600057C RID: 1404 RVA: 0x00013B58 File Offset: 0x00011D58
			public Conv_Ovf_I1_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000145 RID: 325
		public class Conv_Ovf_U1_ : CodeMatch
		{
			// Token: 0x1700019E RID: 414
			public Code.Conv_Ovf_U1_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Conv_Ovf_U1_)base.Set(operand, name);
				}
			}

			// Token: 0x0600057E RID: 1406 RVA: 0x00013B88 File Offset: 0x00011D88
			public Conv_Ovf_U1_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000146 RID: 326
		public class Conv_Ovf_I2_ : CodeMatch
		{
			// Token: 0x1700019F RID: 415
			public Code.Conv_Ovf_I2_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Conv_Ovf_I2_)base.Set(operand, name);
				}
			}

			// Token: 0x06000580 RID: 1408 RVA: 0x00013BB8 File Offset: 0x00011DB8
			public Conv_Ovf_I2_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000147 RID: 327
		public class Conv_Ovf_U2_ : CodeMatch
		{
			// Token: 0x170001A0 RID: 416
			public Code.Conv_Ovf_U2_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Conv_Ovf_U2_)base.Set(operand, name);
				}
			}

			// Token: 0x06000582 RID: 1410 RVA: 0x00013BE8 File Offset: 0x00011DE8
			public Conv_Ovf_U2_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000148 RID: 328
		public class Conv_Ovf_I4_ : CodeMatch
		{
			// Token: 0x170001A1 RID: 417
			public Code.Conv_Ovf_I4_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Conv_Ovf_I4_)base.Set(operand, name);
				}
			}

			// Token: 0x06000584 RID: 1412 RVA: 0x00013C18 File Offset: 0x00011E18
			public Conv_Ovf_I4_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000149 RID: 329
		public class Conv_Ovf_U4_ : CodeMatch
		{
			// Token: 0x170001A2 RID: 418
			public Code.Conv_Ovf_U4_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Conv_Ovf_U4_)base.Set(operand, name);
				}
			}

			// Token: 0x06000586 RID: 1414 RVA: 0x00013C48 File Offset: 0x00011E48
			public Conv_Ovf_U4_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200014A RID: 330
		public class Conv_Ovf_I8_ : CodeMatch
		{
			// Token: 0x170001A3 RID: 419
			public Code.Conv_Ovf_I8_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Conv_Ovf_I8_)base.Set(operand, name);
				}
			}

			// Token: 0x06000588 RID: 1416 RVA: 0x00013C78 File Offset: 0x00011E78
			public Conv_Ovf_I8_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200014B RID: 331
		public class Conv_Ovf_U8_ : CodeMatch
		{
			// Token: 0x170001A4 RID: 420
			public Code.Conv_Ovf_U8_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Conv_Ovf_U8_)base.Set(operand, name);
				}
			}

			// Token: 0x0600058A RID: 1418 RVA: 0x00013CA8 File Offset: 0x00011EA8
			public Conv_Ovf_U8_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200014C RID: 332
		public class Refanyval_ : CodeMatch
		{
			// Token: 0x170001A5 RID: 421
			public Code.Refanyval_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Refanyval_)base.Set(operand, name);
				}
			}

			// Token: 0x0600058C RID: 1420 RVA: 0x00013CD8 File Offset: 0x00011ED8
			public Refanyval_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200014D RID: 333
		public class Ckfinite_ : CodeMatch
		{
			// Token: 0x170001A6 RID: 422
			public Code.Ckfinite_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ckfinite_)base.Set(operand, name);
				}
			}

			// Token: 0x0600058E RID: 1422 RVA: 0x00013D08 File Offset: 0x00011F08
			public Ckfinite_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200014E RID: 334
		public class Mkrefany_ : CodeMatch
		{
			// Token: 0x170001A7 RID: 423
			public Code.Mkrefany_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Mkrefany_)base.Set(operand, name);
				}
			}

			// Token: 0x06000590 RID: 1424 RVA: 0x00013D38 File Offset: 0x00011F38
			public Mkrefany_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200014F RID: 335
		public class Ldtoken_ : CodeMatch
		{
			// Token: 0x170001A8 RID: 424
			public Code.Ldtoken_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldtoken_)base.Set(operand, name);
				}
			}

			// Token: 0x06000592 RID: 1426 RVA: 0x00013D68 File Offset: 0x00011F68
			public Ldtoken_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000150 RID: 336
		public class Conv_U2_ : CodeMatch
		{
			// Token: 0x170001A9 RID: 425
			public Code.Conv_U2_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Conv_U2_)base.Set(operand, name);
				}
			}

			// Token: 0x06000594 RID: 1428 RVA: 0x00013D98 File Offset: 0x00011F98
			public Conv_U2_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000151 RID: 337
		public class Conv_U1_ : CodeMatch
		{
			// Token: 0x170001AA RID: 426
			public Code.Conv_U1_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Conv_U1_)base.Set(operand, name);
				}
			}

			// Token: 0x06000596 RID: 1430 RVA: 0x00013DC8 File Offset: 0x00011FC8
			public Conv_U1_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000152 RID: 338
		public class Conv_I_ : CodeMatch
		{
			// Token: 0x170001AB RID: 427
			public Code.Conv_I_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Conv_I_)base.Set(operand, name);
				}
			}

			// Token: 0x06000598 RID: 1432 RVA: 0x00013DF8 File Offset: 0x00011FF8
			public Conv_I_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000153 RID: 339
		public class Conv_Ovf_I_ : CodeMatch
		{
			// Token: 0x170001AC RID: 428
			public Code.Conv_Ovf_I_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Conv_Ovf_I_)base.Set(operand, name);
				}
			}

			// Token: 0x0600059A RID: 1434 RVA: 0x00013E28 File Offset: 0x00012028
			public Conv_Ovf_I_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000154 RID: 340
		public class Conv_Ovf_U_ : CodeMatch
		{
			// Token: 0x170001AD RID: 429
			public Code.Conv_Ovf_U_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Conv_Ovf_U_)base.Set(operand, name);
				}
			}

			// Token: 0x0600059C RID: 1436 RVA: 0x00013E58 File Offset: 0x00012058
			public Conv_Ovf_U_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000155 RID: 341
		public class Add_Ovf_ : CodeMatch
		{
			// Token: 0x170001AE RID: 430
			public Code.Add_Ovf_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Add_Ovf_)base.Set(operand, name);
				}
			}

			// Token: 0x0600059E RID: 1438 RVA: 0x00013E88 File Offset: 0x00012088
			public Add_Ovf_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000156 RID: 342
		public class Add_Ovf_Un_ : CodeMatch
		{
			// Token: 0x170001AF RID: 431
			public Code.Add_Ovf_Un_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Add_Ovf_Un_)base.Set(operand, name);
				}
			}

			// Token: 0x060005A0 RID: 1440 RVA: 0x00013EB8 File Offset: 0x000120B8
			public Add_Ovf_Un_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000157 RID: 343
		public class Mul_Ovf_ : CodeMatch
		{
			// Token: 0x170001B0 RID: 432
			public Code.Mul_Ovf_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Mul_Ovf_)base.Set(operand, name);
				}
			}

			// Token: 0x060005A2 RID: 1442 RVA: 0x00013EE8 File Offset: 0x000120E8
			public Mul_Ovf_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000158 RID: 344
		public class Mul_Ovf_Un_ : CodeMatch
		{
			// Token: 0x170001B1 RID: 433
			public Code.Mul_Ovf_Un_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Mul_Ovf_Un_)base.Set(operand, name);
				}
			}

			// Token: 0x060005A4 RID: 1444 RVA: 0x00013F18 File Offset: 0x00012118
			public Mul_Ovf_Un_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000159 RID: 345
		public class Sub_Ovf_ : CodeMatch
		{
			// Token: 0x170001B2 RID: 434
			public Code.Sub_Ovf_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Sub_Ovf_)base.Set(operand, name);
				}
			}

			// Token: 0x060005A6 RID: 1446 RVA: 0x00013F48 File Offset: 0x00012148
			public Sub_Ovf_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200015A RID: 346
		public class Sub_Ovf_Un_ : CodeMatch
		{
			// Token: 0x170001B3 RID: 435
			public Code.Sub_Ovf_Un_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Sub_Ovf_Un_)base.Set(operand, name);
				}
			}

			// Token: 0x060005A8 RID: 1448 RVA: 0x00013F78 File Offset: 0x00012178
			public Sub_Ovf_Un_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200015B RID: 347
		public class Endfinally_ : CodeMatch
		{
			// Token: 0x170001B4 RID: 436
			public Code.Endfinally_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Endfinally_)base.Set(operand, name);
				}
			}

			// Token: 0x060005AA RID: 1450 RVA: 0x00013FA8 File Offset: 0x000121A8
			public Endfinally_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200015C RID: 348
		public class Leave_ : CodeMatch
		{
			// Token: 0x170001B5 RID: 437
			public Code.Leave_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Leave_)base.Set(operand, name);
				}
			}

			// Token: 0x060005AC RID: 1452 RVA: 0x00013FD8 File Offset: 0x000121D8
			public Leave_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200015D RID: 349
		public class Leave_S_ : CodeMatch
		{
			// Token: 0x170001B6 RID: 438
			public Code.Leave_S_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Leave_S_)base.Set(operand, name);
				}
			}

			// Token: 0x060005AE RID: 1454 RVA: 0x00014008 File Offset: 0x00012208
			public Leave_S_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200015E RID: 350
		public class Stind_I_ : CodeMatch
		{
			// Token: 0x170001B7 RID: 439
			public Code.Stind_I_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Stind_I_)base.Set(operand, name);
				}
			}

			// Token: 0x060005B0 RID: 1456 RVA: 0x00014038 File Offset: 0x00012238
			public Stind_I_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200015F RID: 351
		public class Conv_U_ : CodeMatch
		{
			// Token: 0x170001B8 RID: 440
			public Code.Conv_U_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Conv_U_)base.Set(operand, name);
				}
			}

			// Token: 0x060005B2 RID: 1458 RVA: 0x00014068 File Offset: 0x00012268
			public Conv_U_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000160 RID: 352
		public class Prefix7_ : CodeMatch
		{
			// Token: 0x170001B9 RID: 441
			public Code.Prefix7_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Prefix7_)base.Set(operand, name);
				}
			}

			// Token: 0x060005B4 RID: 1460 RVA: 0x00014098 File Offset: 0x00012298
			public Prefix7_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000161 RID: 353
		public class Prefix6_ : CodeMatch
		{
			// Token: 0x170001BA RID: 442
			public Code.Prefix6_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Prefix6_)base.Set(operand, name);
				}
			}

			// Token: 0x060005B6 RID: 1462 RVA: 0x000140C8 File Offset: 0x000122C8
			public Prefix6_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000162 RID: 354
		public class Prefix5_ : CodeMatch
		{
			// Token: 0x170001BB RID: 443
			public Code.Prefix5_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Prefix5_)base.Set(operand, name);
				}
			}

			// Token: 0x060005B8 RID: 1464 RVA: 0x000140F8 File Offset: 0x000122F8
			public Prefix5_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000163 RID: 355
		public class Prefix4_ : CodeMatch
		{
			// Token: 0x170001BC RID: 444
			public Code.Prefix4_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Prefix4_)base.Set(operand, name);
				}
			}

			// Token: 0x060005BA RID: 1466 RVA: 0x00014128 File Offset: 0x00012328
			public Prefix4_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000164 RID: 356
		public class Prefix3_ : CodeMatch
		{
			// Token: 0x170001BD RID: 445
			public Code.Prefix3_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Prefix3_)base.Set(operand, name);
				}
			}

			// Token: 0x060005BC RID: 1468 RVA: 0x00014158 File Offset: 0x00012358
			public Prefix3_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000165 RID: 357
		public class Prefix2_ : CodeMatch
		{
			// Token: 0x170001BE RID: 446
			public Code.Prefix2_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Prefix2_)base.Set(operand, name);
				}
			}

			// Token: 0x060005BE RID: 1470 RVA: 0x00014188 File Offset: 0x00012388
			public Prefix2_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000166 RID: 358
		public class Prefix1_ : CodeMatch
		{
			// Token: 0x170001BF RID: 447
			public Code.Prefix1_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Prefix1_)base.Set(operand, name);
				}
			}

			// Token: 0x060005C0 RID: 1472 RVA: 0x000141B8 File Offset: 0x000123B8
			public Prefix1_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000167 RID: 359
		public class Prefixref_ : CodeMatch
		{
			// Token: 0x170001C0 RID: 448
			public Code.Prefixref_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Prefixref_)base.Set(operand, name);
				}
			}

			// Token: 0x060005C2 RID: 1474 RVA: 0x000141E8 File Offset: 0x000123E8
			public Prefixref_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000168 RID: 360
		public class Arglist_ : CodeMatch
		{
			// Token: 0x170001C1 RID: 449
			public Code.Arglist_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Arglist_)base.Set(operand, name);
				}
			}

			// Token: 0x060005C4 RID: 1476 RVA: 0x00014218 File Offset: 0x00012418
			public Arglist_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000169 RID: 361
		public class Ceq_ : CodeMatch
		{
			// Token: 0x170001C2 RID: 450
			public Code.Ceq_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ceq_)base.Set(operand, name);
				}
			}

			// Token: 0x060005C6 RID: 1478 RVA: 0x00014248 File Offset: 0x00012448
			public Ceq_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200016A RID: 362
		public class Cgt_ : CodeMatch
		{
			// Token: 0x170001C3 RID: 451
			public Code.Cgt_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Cgt_)base.Set(operand, name);
				}
			}

			// Token: 0x060005C8 RID: 1480 RVA: 0x00014278 File Offset: 0x00012478
			public Cgt_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200016B RID: 363
		public class Cgt_Un_ : CodeMatch
		{
			// Token: 0x170001C4 RID: 452
			public Code.Cgt_Un_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Cgt_Un_)base.Set(operand, name);
				}
			}

			// Token: 0x060005CA RID: 1482 RVA: 0x000142A8 File Offset: 0x000124A8
			public Cgt_Un_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200016C RID: 364
		public class Clt_ : CodeMatch
		{
			// Token: 0x170001C5 RID: 453
			public Code.Clt_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Clt_)base.Set(operand, name);
				}
			}

			// Token: 0x060005CC RID: 1484 RVA: 0x000142D8 File Offset: 0x000124D8
			public Clt_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200016D RID: 365
		public class Clt_Un_ : CodeMatch
		{
			// Token: 0x170001C6 RID: 454
			public Code.Clt_Un_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Clt_Un_)base.Set(operand, name);
				}
			}

			// Token: 0x060005CE RID: 1486 RVA: 0x00014308 File Offset: 0x00012508
			public Clt_Un_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200016E RID: 366
		public class Ldftn_ : CodeMatch
		{
			// Token: 0x170001C7 RID: 455
			public Code.Ldftn_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldftn_)base.Set(operand, name);
				}
			}

			// Token: 0x060005D0 RID: 1488 RVA: 0x00014338 File Offset: 0x00012538
			public Ldftn_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200016F RID: 367
		public class Ldvirtftn_ : CodeMatch
		{
			// Token: 0x170001C8 RID: 456
			public Code.Ldvirtftn_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldvirtftn_)base.Set(operand, name);
				}
			}

			// Token: 0x060005D2 RID: 1490 RVA: 0x00014368 File Offset: 0x00012568
			public Ldvirtftn_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000170 RID: 368
		public class Ldarg_ : CodeMatch
		{
			// Token: 0x170001C9 RID: 457
			public Code.Ldarg_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldarg_)base.Set(operand, name);
				}
			}

			// Token: 0x060005D4 RID: 1492 RVA: 0x00014398 File Offset: 0x00012598
			public Ldarg_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000171 RID: 369
		public class Ldarga_ : CodeMatch
		{
			// Token: 0x170001CA RID: 458
			public Code.Ldarga_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldarga_)base.Set(operand, name);
				}
			}

			// Token: 0x060005D6 RID: 1494 RVA: 0x000143C8 File Offset: 0x000125C8
			public Ldarga_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000172 RID: 370
		public class Starg_ : CodeMatch
		{
			// Token: 0x170001CB RID: 459
			public Code.Starg_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Starg_)base.Set(operand, name);
				}
			}

			// Token: 0x060005D8 RID: 1496 RVA: 0x000143F8 File Offset: 0x000125F8
			public Starg_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000173 RID: 371
		public class Ldloc_ : CodeMatch
		{
			// Token: 0x170001CC RID: 460
			public Code.Ldloc_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldloc_)base.Set(operand, name);
				}
			}

			// Token: 0x060005DA RID: 1498 RVA: 0x00014428 File Offset: 0x00012628
			public Ldloc_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000174 RID: 372
		public class Ldloca_ : CodeMatch
		{
			// Token: 0x170001CD RID: 461
			public Code.Ldloca_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Ldloca_)base.Set(operand, name);
				}
			}

			// Token: 0x060005DC RID: 1500 RVA: 0x00014458 File Offset: 0x00012658
			public Ldloca_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000175 RID: 373
		public class Stloc_ : CodeMatch
		{
			// Token: 0x170001CE RID: 462
			public Code.Stloc_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Stloc_)base.Set(operand, name);
				}
			}

			// Token: 0x060005DE RID: 1502 RVA: 0x00014488 File Offset: 0x00012688
			public Stloc_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000176 RID: 374
		public class Localloc_ : CodeMatch
		{
			// Token: 0x170001CF RID: 463
			public Code.Localloc_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Localloc_)base.Set(operand, name);
				}
			}

			// Token: 0x060005E0 RID: 1504 RVA: 0x000144B8 File Offset: 0x000126B8
			public Localloc_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000177 RID: 375
		public class Endfilter_ : CodeMatch
		{
			// Token: 0x170001D0 RID: 464
			public Code.Endfilter_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Endfilter_)base.Set(operand, name);
				}
			}

			// Token: 0x060005E2 RID: 1506 RVA: 0x000144E8 File Offset: 0x000126E8
			public Endfilter_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000178 RID: 376
		public class Unaligned_ : CodeMatch
		{
			// Token: 0x170001D1 RID: 465
			public Code.Unaligned_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Unaligned_)base.Set(operand, name);
				}
			}

			// Token: 0x060005E4 RID: 1508 RVA: 0x00014518 File Offset: 0x00012718
			public Unaligned_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000179 RID: 377
		public class Volatile_ : CodeMatch
		{
			// Token: 0x170001D2 RID: 466
			public Code.Volatile_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Volatile_)base.Set(operand, name);
				}
			}

			// Token: 0x060005E6 RID: 1510 RVA: 0x00014548 File Offset: 0x00012748
			public Volatile_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200017A RID: 378
		public class Tailcall_ : CodeMatch
		{
			// Token: 0x170001D3 RID: 467
			public Code.Tailcall_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Tailcall_)base.Set(operand, name);
				}
			}

			// Token: 0x060005E8 RID: 1512 RVA: 0x00014578 File Offset: 0x00012778
			public Tailcall_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200017B RID: 379
		public class Initobj_ : CodeMatch
		{
			// Token: 0x170001D4 RID: 468
			public Code.Initobj_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Initobj_)base.Set(operand, name);
				}
			}

			// Token: 0x060005EA RID: 1514 RVA: 0x000145A8 File Offset: 0x000127A8
			public Initobj_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200017C RID: 380
		public class Constrained_ : CodeMatch
		{
			// Token: 0x170001D5 RID: 469
			public Code.Constrained_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Constrained_)base.Set(operand, name);
				}
			}

			// Token: 0x060005EC RID: 1516 RVA: 0x000145D8 File Offset: 0x000127D8
			public Constrained_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200017D RID: 381
		public class Cpblk_ : CodeMatch
		{
			// Token: 0x170001D6 RID: 470
			public Code.Cpblk_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Cpblk_)base.Set(operand, name);
				}
			}

			// Token: 0x060005EE RID: 1518 RVA: 0x00014608 File Offset: 0x00012808
			public Cpblk_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200017E RID: 382
		public class Initblk_ : CodeMatch
		{
			// Token: 0x170001D7 RID: 471
			public Code.Initblk_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Initblk_)base.Set(operand, name);
				}
			}

			// Token: 0x060005F0 RID: 1520 RVA: 0x00014638 File Offset: 0x00012838
			public Initblk_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x0200017F RID: 383
		public class Rethrow_ : CodeMatch
		{
			// Token: 0x170001D8 RID: 472
			public Code.Rethrow_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Rethrow_)base.Set(operand, name);
				}
			}

			// Token: 0x060005F2 RID: 1522 RVA: 0x00014668 File Offset: 0x00012868
			public Rethrow_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000180 RID: 384
		public class Sizeof_ : CodeMatch
		{
			// Token: 0x170001D9 RID: 473
			public Code.Sizeof_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Sizeof_)base.Set(operand, name);
				}
			}

			// Token: 0x060005F4 RID: 1524 RVA: 0x00014698 File Offset: 0x00012898
			public Sizeof_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000181 RID: 385
		public class Refanytype_ : CodeMatch
		{
			// Token: 0x170001DA RID: 474
			public Code.Refanytype_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Refanytype_)base.Set(operand, name);
				}
			}

			// Token: 0x060005F6 RID: 1526 RVA: 0x000146C8 File Offset: 0x000128C8
			public Refanytype_()
				: base(null, null, null)
			{
			}
		}

		// Token: 0x02000182 RID: 386
		public class Readonly_ : CodeMatch
		{
			// Token: 0x170001DB RID: 475
			public Code.Readonly_ this[object operand = null, string name = null]
			{
				get
				{
					return (Code.Readonly_)base.Set(operand, name);
				}
			}

			// Token: 0x060005F8 RID: 1528 RVA: 0x000146F8 File Offset: 0x000128F8
			public Readonly_()
				: base(null, null, null)
			{
			}
		}
	}
}
