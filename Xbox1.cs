using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace XDevkit
{
    /// <summary>
    /// Credits To JRPC Project also Yelo debug and PeekPoker All Were Merged.
    /// the Rest Is Created By Me TeddyHammer
    /// </summary>
    public partial class Xbox
    {
        //  5   8E 0001A7DB getpeername
        /// Return Type: int
        ///s: SOCKET->UINT_PTR->unsigned int
        ///name: sockaddr*
        ///namelen: int*
        [DllImport("ws2_32.dll", EntryPoint = "getpeername", CallingConvention = CallingConvention.StdCall)]
        public static extern int getpeername(IntPtr s, ref sockaddr name, ref int namelen);
        #region sockaddr
        /// <summary>
        /// The sockaddr structure is meant to be a protocol agnostic data structure
        /// used to represent socket addresses.  It will only work with IPv4 addresses
        /// though and not IPv6.
        /// For IPv6, there is the sockaddr_in6 data structure used for IPv6 addresses
        /// and it is a different size than the sockaddr structure.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct sockaddr
        {
            public ushort sa_family;    /// u_short->unsigned short

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 14, ArraySubType = UnmanagedType.I1)]
            public string sa_data;      /// char[14]
        }
		#endregion
		//private object[] _DmSendCommand_16(object[] ecx, object[] a2, object[] a3, object[] a4, object[] a5, object[] a6, params object[] LegacyParamArray)
		//{
		//	object ebp7;
		//	object[] v8;
		//	object[] ebx9;
		//	object[] ebx10;
		//	object[] eax11;
		//	object[] v12;
		//	object[] edi13;
		//	object[] v14;
		//	object[] eax15;
		//	object[] v16;
		//	object[] v17;
		//	object[] v18;
		//	object[] v19;
		//	object[] v20;
		//	object[] v21;
		//	object[] v22;
		//	object[] v23;
		//	object[] esi24;
		//	object[] esi25;
		//	object[] eax26;
		//	object[] edx27;
		//	object[] eax28;
		//	object[] ecx29;
		//	object[] v30;
		//	object[] v31;
		//	object[] v32;
		//	object[] v33;
		//	object[] v34;
		//	object[] v35;
		//	object[] v36;
		//	object[] v37;
		//	object[] v38;
		//	object[] v39;
		//	sbyte v40;
		//	object[] v41;
		//	object[] v42;
		//	object[] v43;
		//	object[] eax44;
		//	object[] esi45;
		//	object[] v46;
		//	object[] v47;
		//	object[] v48;
		//	object[] v49;
		//	object[] v50;
		//	object[] v51;
		//	object[] v52;
		//	object[] v53;
		//	object[] v54;
		//	object[] v55;
		//	sbyte v56;
		//	object[] v57;
		//	object[] v58;
		//	object[] v59;
		//	object[] eax60;
		//	object[] v61;
		//	object[] v62;
		//	object[] v63;
		//	object[] v64;
		//	object[] v65;
		//	object[] v66;
		//	object[] v67;
		//	object[] v68;
		//	object[] v69;
		//	object[] v70;
		//	object[] v71;
		//	object[] v72;
		//	object[] v73;
		//	object[] eax74;
		//	object[] edx75;
		//	uint ecx76;
		//	object[] v77;
		//	object[] v78;
		//	object[] v79;
		//	//C++ TO C# CONVERTER TODO TASK: There is no equivalent to 'reinterpret_cast' in C#:
		//	ebp7 = reinterpret_cast<object>(reinterpret_cast<int>(__zero_stack_offset()) - 4);
		//	v8 = ebx9;
		//	ebx10 = a2;
		//	//C++ TO C# CONVERTER TODO TASK: There is no equivalent to 'reinterpret_cast' in C#:
		//	eax11 = reinterpret_cast<object>(0);
		//	v12 = edi13;
		//	//C++ TO C# CONVERTER TODO TASK: There is no equivalent to 'reinterpret_cast' in C#:
		//	*reinterpret_cast<byte>(eax11) = reinterpret_cast<uint1_t>(ebx10 == 0);
		//	v14 = eax11;
		//	if (a4 && !a5)
		//	{
		//		//C++ TO C# CONVERTER TODO TASK: There is no equivalent to 'reinterpret_cast' in C#:
		//		eax15 = reinterpret_cast<object>(0x80070057);
		//		goto addr_43389a_3;
		//	}
		//	if (v14)
		//	{
		//		//C++ TO C# CONVERTER TODO TASK: There is no equivalent to 'reinterpret_cast' in C#:
		//		eax15 = fun_43346f(ecx, 0x4654e0, reinterpret_cast<int>(ebp7) + 8, v12, v8, v16, v17, v18, v19, v20, v21, v22);
		//		//C++ TO C# CONVERTER TODO TASK: There is no equivalent to 'reinterpret_cast' in C#:
		//		if (reinterpret_cast<sbyte>(eax15) < reinterpret_cast<sbyte>(0))
		//		{
		//		addr_43389a_3:
		//			return eax15;
		//		}
		//		else
		//		{
		//			ebx10 = a2;
		//		}
		//	}
		//	if (ebx10)
		//	{
		//		v23 = esi24;
		//		esi25 = a3;
		//		if (esi25)
		//		{
		//			eax26 = esi25;
		//			edx27 = eax26 + 1;
		//			do
		//			{
		//				++eax26;
		//				//C++ TO C# CONVERTER TODO TASK: There is no equivalent to 'reinterpret_cast' in C#:
		//			} while (*reinterpret_cast<object>(eax26));
		//			//C++ TO C# CONVERTER TODO TASK: There is no equivalent to 'reinterpret_cast' in C#:
		//			eax28 = reinterpret_cast<object>(reinterpret_cast<byte>(eax26) - reinterpret_cast<byte>(edx27));
		//			ecx29 = eax28 + 3;
		//			//C++ TO C# CONVERTER TODO TASK: There is no equivalent to 'reinterpret_cast' in C#:
		//			if (reinterpret_cast<byte>(ecx29) < reinterpret_cast<byte>(0x200))
		//			{
		//				goto addr_43381d_12;
		//			}
		//		}
		//		else
		//		{
		//			if (v14)
		//			{
		//				fun_433531(ecx, 0x4654e0, ebx10, v23, v12, v8, v30, v31, v32, v33, v34, v35, v36, v37, v38, v39, v40);
		//			}
		//			//C++ TO C# CONVERTER TODO TASK: There is no equivalent to 'reinterpret_cast' in C#:
		//			eax15 = reinterpret_cast<object>(0x2da0000);
		//			goto addr_433899_16;
		//		}
		//	}
		//	else
		//	{
		//		//C++ TO C# CONVERTER TODO TASK: There is no equivalent to 'reinterpret_cast' in C#:
		//		eax15 = reinterpret_cast<object>(0x82da0101);
		//		goto addr_43389a_3;
		//	}
		//	eax44 = _DmSendBinary_12(ecx29, ebx10, esi25, eax28, v23, v12, v8, v41, v42, v43);
		//	esi45 = eax44;
		//	//C++ TO C# CONVERTER TODO TASK: There is no equivalent to 'reinterpret_cast' in C#:
		//	if (reinterpret_cast<sbyte>(esi45) < reinterpret_cast<sbyte>(0))
		//	{
		//	addr_433887_19:
		//		if (v14)
		//		{
		//			fun_433531(ecx29, 0x4654e0, ebx10, v23, v12, v8, v46, v47, v48, v49, v50, v51, v52, v53, v54, v55, v56);
		//		}
		//	}
		//	else
		//	{
		//		eax60 = _DmSendBinary_12(ecx29, ebx10, "\r\n", 2, v23, v12, v8, v57, v58, v59);
		//		goto addr_433873_22;
		//	}
		//	eax15 = esi45;
		//addr_433899_16:
		//	goto addr_43389a_3;
		//addr_433873_22:
		//	esi45 = eax60;
		//	//C++ TO C# CONVERTER TODO TASK: There is no equivalent to 'reinterpret_cast' in C#:
		//	if (reinterpret_cast<sbyte>(esi45) >= reinterpret_cast<sbyte>(0))
		//	{
		//		eax74 = _DmReceiveStatusResponse_12(ecx29, ebx10, a4, a5, v23, v12, v8, v61, v62, v63, v64, v65, v66, v67, v68, v69, v70, v71, v72, v73);
		//		esi45 = eax74;
		//		goto addr_433887_19;
		//	}
		//addr_43381d_12:
		//	edx75 = eax28;
		//	//C++ TO C# CONVERTER TODO TASK: There is no equivalent to 'reinterpret_cast' in C#:
		//	ecx76 = reinterpret_cast<byte>(eax28) >> 2;
		//	while (ecx76)
		//	{
		//		--ecx76;
		//	}
		//	//C++ TO C# CONVERTER TODO TASK: There is no equivalent to 'reinterpret_cast' in C#:
		//	ecx29 = reinterpret_cast<object>(reinterpret_cast<byte>(edx75) & 3);
		//	while (ecx29)
		//	{
		//		--ecx29;
		//	}
		//	//C++ TO C# CONVERTER TODO TASK: There is no equivalent to 'reinterpret_cast' in C#:
		//	*reinterpret_cast<sbyte>(reinterpret_cast<int>(ebp7) + reinterpret_cast<byte>(eax28) + 0xfffffdfc) = 13;
		//	//C++ TO C# CONVERTER TODO TASK: There is no equivalent to 'reinterpret_cast' in C#:
		//	*reinterpret_cast<sbyte>(reinterpret_cast<int>(ebp7) + reinterpret_cast<byte>(eax28) + 0xfffffdfd) = 10;
		//	//C++ TO C# CONVERTER TODO TASK: There is no equivalent to 'reinterpret_cast' in C#:
		//	eax60 = _DmSendBinary_12(ecx29, ebx10, reinterpret_cast<int>(ebp7) + 0xfffffdfc, eax28 + 2, v23, v12, v8, v77, v78, v79);
		//	goto addr_433873_22;


		//}
		private int _DmScreenShot_4(object[] ecx, object[] a2, object[] a3, object[] a4, object[] a5, object[] a6, object[] a7, object[] a8, sbyte a9)
		{
			int eax10;

			eax10 = fun_4378e0(ecx, 0x4654e0, return_address(), return_address(), a2, a3, a4, a5, a6, a7, a8, a9);
			return eax10;
		}

        private int fun_4378e0(object[] ecx, int v1, object v2, object v3, object[] a2, object[] a3, object[] a4, object[] a5, object[] a6, object[] a7, object[] a8, sbyte a9)
        {
            throw new NotImplementedException();
        }

        private object return_address()
        {
            throw new NotImplementedException();
        }
    }
	}