using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x020002C4 RID: 708
	internal static class OpCodes
	{
		// Token: 0x0400084B RID: 2123
		internal static readonly OpCode[] OneByteOpCode = new OpCode[225];

		// Token: 0x0400084C RID: 2124
		internal static readonly OpCode[] TwoBytesOpCode = new OpCode[31];

		// Token: 0x0400084D RID: 2125
		public static readonly OpCode Nop = new OpCode(83886335, 318768389);

		// Token: 0x0400084E RID: 2126
		public static readonly OpCode Break = new OpCode(16843263, 318768389);

		// Token: 0x0400084F RID: 2127
		public static readonly OpCode Ldarg_0 = new OpCode(84017919, 335545601);

		// Token: 0x04000850 RID: 2128
		public static readonly OpCode Ldarg_1 = new OpCode(84083711, 335545601);

		// Token: 0x04000851 RID: 2129
		public static readonly OpCode Ldarg_2 = new OpCode(84149503, 335545601);

		// Token: 0x04000852 RID: 2130
		public static readonly OpCode Ldarg_3 = new OpCode(84215295, 335545601);

		// Token: 0x04000853 RID: 2131
		public static readonly OpCode Ldloc_0 = new OpCode(84281087, 335545601);

		// Token: 0x04000854 RID: 2132
		public static readonly OpCode Ldloc_1 = new OpCode(84346879, 335545601);

		// Token: 0x04000855 RID: 2133
		public static readonly OpCode Ldloc_2 = new OpCode(84412671, 335545601);

		// Token: 0x04000856 RID: 2134
		public static readonly OpCode Ldloc_3 = new OpCode(84478463, 335545601);

		// Token: 0x04000857 RID: 2135
		public static readonly OpCode Stloc_0 = new OpCode(84544255, 318833921);

		// Token: 0x04000858 RID: 2136
		public static readonly OpCode Stloc_1 = new OpCode(84610047, 318833921);

		// Token: 0x04000859 RID: 2137
		public static readonly OpCode Stloc_2 = new OpCode(84675839, 318833921);

		// Token: 0x0400085A RID: 2138
		public static readonly OpCode Stloc_3 = new OpCode(84741631, 318833921);

		// Token: 0x0400085B RID: 2139
		public static readonly OpCode Ldarg_S = new OpCode(84807423, 335549185);

		// Token: 0x0400085C RID: 2140
		public static readonly OpCode Ldarga_S = new OpCode(84873215, 369103617);

		// Token: 0x0400085D RID: 2141
		public static readonly OpCode Starg_S = new OpCode(84939007, 318837505);

		// Token: 0x0400085E RID: 2142
		public static readonly OpCode Ldloc_S = new OpCode(85004799, 335548929);

		// Token: 0x0400085F RID: 2143
		public static readonly OpCode Ldloca_S = new OpCode(85070591, 369103361);

		// Token: 0x04000860 RID: 2144
		public static readonly OpCode Stloc_S = new OpCode(85136383, 318837249);

		// Token: 0x04000861 RID: 2145
		public static readonly OpCode Ldnull = new OpCode(85202175, 436208901);

		// Token: 0x04000862 RID: 2146
		public static readonly OpCode Ldc_I4_M1 = new OpCode(85267967, 369100033);

		// Token: 0x04000863 RID: 2147
		public static readonly OpCode Ldc_I4_0 = new OpCode(85333759, 369100033);

		// Token: 0x04000864 RID: 2148
		public static readonly OpCode Ldc_I4_1 = new OpCode(85399551, 369100033);

		// Token: 0x04000865 RID: 2149
		public static readonly OpCode Ldc_I4_2 = new OpCode(85465343, 369100033);

		// Token: 0x04000866 RID: 2150
		public static readonly OpCode Ldc_I4_3 = new OpCode(85531135, 369100033);

		// Token: 0x04000867 RID: 2151
		public static readonly OpCode Ldc_I4_4 = new OpCode(85596927, 369100033);

		// Token: 0x04000868 RID: 2152
		public static readonly OpCode Ldc_I4_5 = new OpCode(85662719, 369100033);

		// Token: 0x04000869 RID: 2153
		public static readonly OpCode Ldc_I4_6 = new OpCode(85728511, 369100033);

		// Token: 0x0400086A RID: 2154
		public static readonly OpCode Ldc_I4_7 = new OpCode(85794303, 369100033);

		// Token: 0x0400086B RID: 2155
		public static readonly OpCode Ldc_I4_8 = new OpCode(85860095, 369100033);

		// Token: 0x0400086C RID: 2156
		public static readonly OpCode Ldc_I4_S = new OpCode(85925887, 369102849);

		// Token: 0x0400086D RID: 2157
		public static readonly OpCode Ldc_I4 = new OpCode(85991679, 369099269);

		// Token: 0x0400086E RID: 2158
		public static readonly OpCode Ldc_I8 = new OpCode(86057471, 385876741);

		// Token: 0x0400086F RID: 2159
		public static readonly OpCode Ldc_R4 = new OpCode(86123263, 402657541);

		// Token: 0x04000870 RID: 2160
		public static readonly OpCode Ldc_R8 = new OpCode(86189055, 419432197);

		// Token: 0x04000871 RID: 2161
		public static readonly OpCode Dup = new OpCode(86255103, 352388357);

		// Token: 0x04000872 RID: 2162
		public static readonly OpCode Pop = new OpCode(86320895, 318833925);

		// Token: 0x04000873 RID: 2163
		public static readonly OpCode Jmp = new OpCode(36055039, 318768133);

		// Token: 0x04000874 RID: 2164
		public static readonly OpCode Call = new OpCode(36120831, 471532549);

		// Token: 0x04000875 RID: 2165
		public static readonly OpCode Calli = new OpCode(36186623, 471533573);

		// Token: 0x04000876 RID: 2166
		public static readonly OpCode Ret = new OpCode(120138495, 320537861);

		// Token: 0x04000877 RID: 2167
		public static readonly OpCode Br_S = new OpCode(2763775, 318770945);

		// Token: 0x04000878 RID: 2168
		public static readonly OpCode Brfalse_S = new OpCode(53161215, 318967553);

		// Token: 0x04000879 RID: 2169
		public static readonly OpCode Brtrue_S = new OpCode(53227007, 318967553);

		// Token: 0x0400087A RID: 2170
		public static readonly OpCode Beq_S = new OpCode(53292799, 318902017);

		// Token: 0x0400087B RID: 2171
		public static readonly OpCode Bge_S = new OpCode(53358591, 318902017);

		// Token: 0x0400087C RID: 2172
		public static readonly OpCode Bgt_S = new OpCode(53424383, 318902017);

		// Token: 0x0400087D RID: 2173
		public static readonly OpCode Ble_S = new OpCode(53490175, 318902017);

		// Token: 0x0400087E RID: 2174
		public static readonly OpCode Blt_S = new OpCode(53555967, 318902017);

		// Token: 0x0400087F RID: 2175
		public static readonly OpCode Bne_Un_S = new OpCode(53621759, 318902017);

		// Token: 0x04000880 RID: 2176
		public static readonly OpCode Bge_Un_S = new OpCode(53687551, 318902017);

		// Token: 0x04000881 RID: 2177
		public static readonly OpCode Bgt_Un_S = new OpCode(53753343, 318902017);

		// Token: 0x04000882 RID: 2178
		public static readonly OpCode Ble_Un_S = new OpCode(53819135, 318902017);

		// Token: 0x04000883 RID: 2179
		public static readonly OpCode Blt_Un_S = new OpCode(53884927, 318902017);

		// Token: 0x04000884 RID: 2180
		public static readonly OpCode Br = new OpCode(3619071, 318767109);

		// Token: 0x04000885 RID: 2181
		public static readonly OpCode Brfalse = new OpCode(54016511, 318963717);

		// Token: 0x04000886 RID: 2182
		public static readonly OpCode Brtrue = new OpCode(54082303, 318963717);

		// Token: 0x04000887 RID: 2183
		public static readonly OpCode Beq = new OpCode(54148095, 318898177);

		// Token: 0x04000888 RID: 2184
		public static readonly OpCode Bge = new OpCode(54213887, 318898177);

		// Token: 0x04000889 RID: 2185
		public static readonly OpCode Bgt = new OpCode(54279679, 318898177);

		// Token: 0x0400088A RID: 2186
		public static readonly OpCode Ble = new OpCode(54345471, 318898177);

		// Token: 0x0400088B RID: 2187
		public static readonly OpCode Blt = new OpCode(54411263, 318898177);

		// Token: 0x0400088C RID: 2188
		public static readonly OpCode Bne_Un = new OpCode(54477055, 318898177);

		// Token: 0x0400088D RID: 2189
		public static readonly OpCode Bge_Un = new OpCode(54542847, 318898177);

		// Token: 0x0400088E RID: 2190
		public static readonly OpCode Bgt_Un = new OpCode(54608639, 318898177);

		// Token: 0x0400088F RID: 2191
		public static readonly OpCode Ble_Un = new OpCode(54674431, 318898177);

		// Token: 0x04000890 RID: 2192
		public static readonly OpCode Blt_Un = new OpCode(54740223, 318898177);

		// Token: 0x04000891 RID: 2193
		public static readonly OpCode Switch = new OpCode(54806015, 318966277);

		// Token: 0x04000892 RID: 2194
		public static readonly OpCode Ldind_I1 = new OpCode(88426239, 369296645);

		// Token: 0x04000893 RID: 2195
		public static readonly OpCode Ldind_U1 = new OpCode(88492031, 369296645);

		// Token: 0x04000894 RID: 2196
		public static readonly OpCode Ldind_I2 = new OpCode(88557823, 369296645);

		// Token: 0x04000895 RID: 2197
		public static readonly OpCode Ldind_U2 = new OpCode(88623615, 369296645);

		// Token: 0x04000896 RID: 2198
		public static readonly OpCode Ldind_I4 = new OpCode(88689407, 369296645);

		// Token: 0x04000897 RID: 2199
		public static readonly OpCode Ldind_U4 = new OpCode(88755199, 369296645);

		// Token: 0x04000898 RID: 2200
		public static readonly OpCode Ldind_I8 = new OpCode(88820991, 386073861);

		// Token: 0x04000899 RID: 2201
		public static readonly OpCode Ldind_I = new OpCode(88886783, 369296645);

		// Token: 0x0400089A RID: 2202
		public static readonly OpCode Ldind_R4 = new OpCode(88952575, 402851077);

		// Token: 0x0400089B RID: 2203
		public static readonly OpCode Ldind_R8 = new OpCode(89018367, 419628293);

		// Token: 0x0400089C RID: 2204
		public static readonly OpCode Ldind_Ref = new OpCode(89084159, 436405509);

		// Token: 0x0400089D RID: 2205
		public static readonly OpCode Stind_Ref = new OpCode(89149951, 319096069);

		// Token: 0x0400089E RID: 2206
		public static readonly OpCode Stind_I1 = new OpCode(89215743, 319096069);

		// Token: 0x0400089F RID: 2207
		public static readonly OpCode Stind_I2 = new OpCode(89281535, 319096069);

		// Token: 0x040008A0 RID: 2208
		public static readonly OpCode Stind_I4 = new OpCode(89347327, 319096069);

		// Token: 0x040008A1 RID: 2209
		public static readonly OpCode Stind_I8 = new OpCode(89413119, 319161605);

		// Token: 0x040008A2 RID: 2210
		public static readonly OpCode Stind_R4 = new OpCode(89478911, 319292677);

		// Token: 0x040008A3 RID: 2211
		public static readonly OpCode Stind_R8 = new OpCode(89544703, 319358213);

		// Token: 0x040008A4 RID: 2212
		public static readonly OpCode Add = new OpCode(89610495, 335676677);

		// Token: 0x040008A5 RID: 2213
		public static readonly OpCode Sub = new OpCode(89676287, 335676677);

		// Token: 0x040008A6 RID: 2214
		public static readonly OpCode Mul = new OpCode(89742079, 335676677);

		// Token: 0x040008A7 RID: 2215
		public static readonly OpCode Div = new OpCode(89807871, 335676677);

		// Token: 0x040008A8 RID: 2216
		public static readonly OpCode Div_Un = new OpCode(89873663, 335676677);

		// Token: 0x040008A9 RID: 2217
		public static readonly OpCode Rem = new OpCode(89939455, 335676677);

		// Token: 0x040008AA RID: 2218
		public static readonly OpCode Rem_Un = new OpCode(90005247, 335676677);

		// Token: 0x040008AB RID: 2219
		public static readonly OpCode And = new OpCode(90071039, 335676677);

		// Token: 0x040008AC RID: 2220
		public static readonly OpCode Or = new OpCode(90136831, 335676677);

		// Token: 0x040008AD RID: 2221
		public static readonly OpCode Xor = new OpCode(90202623, 335676677);

		// Token: 0x040008AE RID: 2222
		public static readonly OpCode Shl = new OpCode(90268415, 335676677);

		// Token: 0x040008AF RID: 2223
		public static readonly OpCode Shr = new OpCode(90334207, 335676677);

		// Token: 0x040008B0 RID: 2224
		public static readonly OpCode Shr_Un = new OpCode(90399999, 335676677);

		// Token: 0x040008B1 RID: 2225
		public static readonly OpCode Neg = new OpCode(90465791, 335611141);

		// Token: 0x040008B2 RID: 2226
		public static readonly OpCode Not = new OpCode(90531583, 335611141);

		// Token: 0x040008B3 RID: 2227
		public static readonly OpCode Conv_I1 = new OpCode(90597375, 369165573);

		// Token: 0x040008B4 RID: 2228
		public static readonly OpCode Conv_I2 = new OpCode(90663167, 369165573);

		// Token: 0x040008B5 RID: 2229
		public static readonly OpCode Conv_I4 = new OpCode(90728959, 369165573);

		// Token: 0x040008B6 RID: 2230
		public static readonly OpCode Conv_I8 = new OpCode(90794751, 385942789);

		// Token: 0x040008B7 RID: 2231
		public static readonly OpCode Conv_R4 = new OpCode(90860543, 402720005);

		// Token: 0x040008B8 RID: 2232
		public static readonly OpCode Conv_R8 = new OpCode(90926335, 419497221);

		// Token: 0x040008B9 RID: 2233
		public static readonly OpCode Conv_U4 = new OpCode(90992127, 369165573);

		// Token: 0x040008BA RID: 2234
		public static readonly OpCode Conv_U8 = new OpCode(91057919, 385942789);

		// Token: 0x040008BB RID: 2235
		public static readonly OpCode Callvirt = new OpCode(40792063, 471532547);

		// Token: 0x040008BC RID: 2236
		public static readonly OpCode Cpobj = new OpCode(91189503, 319097859);

		// Token: 0x040008BD RID: 2237
		public static readonly OpCode Ldobj = new OpCode(91255295, 335744003);

		// Token: 0x040008BE RID: 2238
		public static readonly OpCode Ldstr = new OpCode(91321087, 436209923);

		// Token: 0x040008BF RID: 2239
		public static readonly OpCode Newobj = new OpCode(41055231, 437978115);

		// Token: 0x040008C0 RID: 2240
		public static readonly OpCode Castclass = new OpCode(91452671, 436866051);

		// Token: 0x040008C1 RID: 2241
		public static readonly OpCode Isinst = new OpCode(91518463, 369757187);

		// Token: 0x040008C2 RID: 2242
		public static readonly OpCode Conv_R_Un = new OpCode(91584255, 419497221);

		// Token: 0x040008C3 RID: 2243
		public static readonly OpCode Unbox = new OpCode(91650559, 369757189);

		// Token: 0x040008C4 RID: 2244
		public static readonly OpCode Throw = new OpCode(142047999, 319423747);

		// Token: 0x040008C5 RID: 2245
		public static readonly OpCode Ldfld = new OpCode(91782143, 336199939);

		// Token: 0x040008C6 RID: 2246
		public static readonly OpCode Ldflda = new OpCode(91847935, 369754371);

		// Token: 0x040008C7 RID: 2247
		public static readonly OpCode Stfld = new OpCode(91913727, 319488259);

		// Token: 0x040008C8 RID: 2248
		public static readonly OpCode Ldsfld = new OpCode(91979519, 335544579);

		// Token: 0x040008C9 RID: 2249
		public static readonly OpCode Ldsflda = new OpCode(92045311, 369099011);

		// Token: 0x040008CA RID: 2250
		public static readonly OpCode Stsfld = new OpCode(92111103, 318832899);

		// Token: 0x040008CB RID: 2251
		public static readonly OpCode Stobj = new OpCode(92176895, 319032323);

		// Token: 0x040008CC RID: 2252
		public static readonly OpCode Conv_Ovf_I1_Un = new OpCode(92242687, 369165573);

		// Token: 0x040008CD RID: 2253
		public static readonly OpCode Conv_Ovf_I2_Un = new OpCode(92308479, 369165573);

		// Token: 0x040008CE RID: 2254
		public static readonly OpCode Conv_Ovf_I4_Un = new OpCode(92374271, 369165573);

		// Token: 0x040008CF RID: 2255
		public static readonly OpCode Conv_Ovf_I8_Un = new OpCode(92440063, 385942789);

		// Token: 0x040008D0 RID: 2256
		public static readonly OpCode Conv_Ovf_U1_Un = new OpCode(92505855, 369165573);

		// Token: 0x040008D1 RID: 2257
		public static readonly OpCode Conv_Ovf_U2_Un = new OpCode(92571647, 369165573);

		// Token: 0x040008D2 RID: 2258
		public static readonly OpCode Conv_Ovf_U4_Un = new OpCode(92637439, 369165573);

		// Token: 0x040008D3 RID: 2259
		public static readonly OpCode Conv_Ovf_U8_Un = new OpCode(92703231, 385942789);

		// Token: 0x040008D4 RID: 2260
		public static readonly OpCode Conv_Ovf_I_Un = new OpCode(92769023, 369165573);

		// Token: 0x040008D5 RID: 2261
		public static readonly OpCode Conv_Ovf_U_Un = new OpCode(92834815, 369165573);

		// Token: 0x040008D6 RID: 2262
		public static readonly OpCode Box = new OpCode(92900607, 436276229);

		// Token: 0x040008D7 RID: 2263
		public static readonly OpCode Newarr = new OpCode(92966399, 436407299);

		// Token: 0x040008D8 RID: 2264
		public static readonly OpCode Ldlen = new OpCode(93032191, 369755395);

		// Token: 0x040008D9 RID: 2265
		public static readonly OpCode Ldelema = new OpCode(93097983, 369888259);

		// Token: 0x040008DA RID: 2266
		public static readonly OpCode Ldelem_I1 = new OpCode(93163775, 369886467);

		// Token: 0x040008DB RID: 2267
		public static readonly OpCode Ldelem_U1 = new OpCode(93229567, 369886467);

		// Token: 0x040008DC RID: 2268
		public static readonly OpCode Ldelem_I2 = new OpCode(93295359, 369886467);

		// Token: 0x040008DD RID: 2269
		public static readonly OpCode Ldelem_U2 = new OpCode(93361151, 369886467);

		// Token: 0x040008DE RID: 2270
		public static readonly OpCode Ldelem_I4 = new OpCode(93426943, 369886467);

		// Token: 0x040008DF RID: 2271
		public static readonly OpCode Ldelem_U4 = new OpCode(93492735, 369886467);

		// Token: 0x040008E0 RID: 2272
		public static readonly OpCode Ldelem_I8 = new OpCode(93558527, 386663683);

		// Token: 0x040008E1 RID: 2273
		public static readonly OpCode Ldelem_I = new OpCode(93624319, 369886467);

		// Token: 0x040008E2 RID: 2274
		public static readonly OpCode Ldelem_R4 = new OpCode(93690111, 403440899);

		// Token: 0x040008E3 RID: 2275
		public static readonly OpCode Ldelem_R8 = new OpCode(93755903, 420218115);

		// Token: 0x040008E4 RID: 2276
		public static readonly OpCode Ldelem_Ref = new OpCode(93821695, 436995331);

		// Token: 0x040008E5 RID: 2277
		public static readonly OpCode Stelem_I = new OpCode(93887487, 319620355);

		// Token: 0x040008E6 RID: 2278
		public static readonly OpCode Stelem_I1 = new OpCode(93953279, 319620355);

		// Token: 0x040008E7 RID: 2279
		public static readonly OpCode Stelem_I2 = new OpCode(94019071, 319620355);

		// Token: 0x040008E8 RID: 2280
		public static readonly OpCode Stelem_I4 = new OpCode(94084863, 319620355);

		// Token: 0x040008E9 RID: 2281
		public static readonly OpCode Stelem_I8 = new OpCode(94150655, 319685891);

		// Token: 0x040008EA RID: 2282
		public static readonly OpCode Stelem_R4 = new OpCode(94216447, 319751427);

		// Token: 0x040008EB RID: 2283
		public static readonly OpCode Stelem_R8 = new OpCode(94282239, 319816963);

		// Token: 0x040008EC RID: 2284
		public static readonly OpCode Stelem_Ref = new OpCode(94348031, 319882499);

		// Token: 0x040008ED RID: 2285
		public static readonly OpCode Ldelem_Any = new OpCode(94413823, 336333827);

		// Token: 0x040008EE RID: 2286
		public static readonly OpCode Stelem_Any = new OpCode(94479615, 319884291);

		// Token: 0x040008EF RID: 2287
		public static readonly OpCode Unbox_Any = new OpCode(94545407, 336202755);

		// Token: 0x040008F0 RID: 2288
		public static readonly OpCode Conv_Ovf_I1 = new OpCode(94614527, 369165573);

		// Token: 0x040008F1 RID: 2289
		public static readonly OpCode Conv_Ovf_U1 = new OpCode(94680319, 369165573);

		// Token: 0x040008F2 RID: 2290
		public static readonly OpCode Conv_Ovf_I2 = new OpCode(94746111, 369165573);

		// Token: 0x040008F3 RID: 2291
		public static readonly OpCode Conv_Ovf_U2 = new OpCode(94811903, 369165573);

		// Token: 0x040008F4 RID: 2292
		public static readonly OpCode Conv_Ovf_I4 = new OpCode(94877695, 369165573);

		// Token: 0x040008F5 RID: 2293
		public static readonly OpCode Conv_Ovf_U4 = new OpCode(94943487, 369165573);

		// Token: 0x040008F6 RID: 2294
		public static readonly OpCode Conv_Ovf_I8 = new OpCode(95009279, 385942789);

		// Token: 0x040008F7 RID: 2295
		public static readonly OpCode Conv_Ovf_U8 = new OpCode(95075071, 385942789);

		// Token: 0x040008F8 RID: 2296
		public static readonly OpCode Refanyval = new OpCode(95142655, 369167365);

		// Token: 0x040008F9 RID: 2297
		public static readonly OpCode Ckfinite = new OpCode(95208447, 419497221);

		// Token: 0x040008FA RID: 2298
		public static readonly OpCode Mkrefany = new OpCode(95274751, 335744005);

		// Token: 0x040008FB RID: 2299
		public static readonly OpCode Ldtoken = new OpCode(95342847, 369101573);

		// Token: 0x040008FC RID: 2300
		public static readonly OpCode Conv_U2 = new OpCode(95408639, 369165573);

		// Token: 0x040008FD RID: 2301
		public static readonly OpCode Conv_U1 = new OpCode(95474431, 369165573);

		// Token: 0x040008FE RID: 2302
		public static readonly OpCode Conv_I = new OpCode(95540223, 369165573);

		// Token: 0x040008FF RID: 2303
		public static readonly OpCode Conv_Ovf_I = new OpCode(95606015, 369165573);

		// Token: 0x04000900 RID: 2304
		public static readonly OpCode Conv_Ovf_U = new OpCode(95671807, 369165573);

		// Token: 0x04000901 RID: 2305
		public static readonly OpCode Add_Ovf = new OpCode(95737599, 335676677);

		// Token: 0x04000902 RID: 2306
		public static readonly OpCode Add_Ovf_Un = new OpCode(95803391, 335676677);

		// Token: 0x04000903 RID: 2307
		public static readonly OpCode Mul_Ovf = new OpCode(95869183, 335676677);

		// Token: 0x04000904 RID: 2308
		public static readonly OpCode Mul_Ovf_Un = new OpCode(95934975, 335676677);

		// Token: 0x04000905 RID: 2309
		public static readonly OpCode Sub_Ovf = new OpCode(96000767, 335676677);

		// Token: 0x04000906 RID: 2310
		public static readonly OpCode Sub_Ovf_Un = new OpCode(96066559, 335676677);

		// Token: 0x04000907 RID: 2311
		public static readonly OpCode Endfinally = new OpCode(129686783, 318768389);

		// Token: 0x04000908 RID: 2312
		public static readonly OpCode Leave = new OpCode(12312063, 319946757);

		// Token: 0x04000909 RID: 2313
		public static readonly OpCode Leave_S = new OpCode(12377855, 319950593);

		// Token: 0x0400090A RID: 2314
		public static readonly OpCode Stind_I = new OpCode(96329727, 319096069);

		// Token: 0x0400090B RID: 2315
		public static readonly OpCode Conv_U = new OpCode(96395519, 369165573);

		// Token: 0x0400090C RID: 2316
		public static readonly OpCode Arglist = new OpCode(96403710, 369100037);

		// Token: 0x0400090D RID: 2317
		public static readonly OpCode Ceq = new OpCode(96469502, 369231109);

		// Token: 0x0400090E RID: 2318
		public static readonly OpCode Cgt = new OpCode(96535294, 369231109);

		// Token: 0x0400090F RID: 2319
		public static readonly OpCode Cgt_Un = new OpCode(96601086, 369231109);

		// Token: 0x04000910 RID: 2320
		public static readonly OpCode Clt = new OpCode(96666878, 369231109);

		// Token: 0x04000911 RID: 2321
		public static readonly OpCode Clt_Un = new OpCode(96732670, 369231109);

		// Token: 0x04000912 RID: 2322
		public static readonly OpCode Ldftn = new OpCode(96798462, 369099781);

		// Token: 0x04000913 RID: 2323
		public static readonly OpCode Ldvirtftn = new OpCode(96864254, 369755141);

		// Token: 0x04000914 RID: 2324
		public static readonly OpCode Ldarg = new OpCode(96930302, 335547909);

		// Token: 0x04000915 RID: 2325
		public static readonly OpCode Ldarga = new OpCode(96996094, 369102341);

		// Token: 0x04000916 RID: 2326
		public static readonly OpCode Starg = new OpCode(97061886, 318836229);

		// Token: 0x04000917 RID: 2327
		public static readonly OpCode Ldloc = new OpCode(97127678, 335547653);

		// Token: 0x04000918 RID: 2328
		public static readonly OpCode Ldloca = new OpCode(97193470, 369102085);

		// Token: 0x04000919 RID: 2329
		public static readonly OpCode Stloc = new OpCode(97259262, 318835973);

		// Token: 0x0400091A RID: 2330
		public static readonly OpCode Localloc = new OpCode(97325054, 369296645);

		// Token: 0x0400091B RID: 2331
		public static readonly OpCode Endfilter = new OpCode(130945534, 318964997);

		// Token: 0x0400091C RID: 2332
		public static readonly OpCode Unaligned = new OpCode(80679678, 318771204);

		// Token: 0x0400091D RID: 2333
		public static readonly OpCode Volatile = new OpCode(80745470, 318768388);

		// Token: 0x0400091E RID: 2334
		public static readonly OpCode Tail = new OpCode(80811262, 318768388);

		// Token: 0x0400091F RID: 2335
		public static readonly OpCode Initobj = new OpCode(97654270, 318966787);

		// Token: 0x04000920 RID: 2336
		public static readonly OpCode Constrained = new OpCode(97720062, 318770180);

		// Token: 0x04000921 RID: 2337
		public static readonly OpCode Cpblk = new OpCode(97785854, 319227141);

		// Token: 0x04000922 RID: 2338
		public static readonly OpCode Initblk = new OpCode(97851646, 319227141);

		// Token: 0x04000923 RID: 2339
		public static readonly OpCode No = new OpCode(97917438, 318771204);

		// Token: 0x04000924 RID: 2340
		public static readonly OpCode Rethrow = new OpCode(148314878, 318768387);

		// Token: 0x04000925 RID: 2341
		public static readonly OpCode Sizeof = new OpCode(98049278, 369101829);

		// Token: 0x04000926 RID: 2342
		public static readonly OpCode Refanytype = new OpCode(98115070, 369165573);

		// Token: 0x04000927 RID: 2343
		public static readonly OpCode Readonly = new OpCode(98180862, 318768388);
	}
}
