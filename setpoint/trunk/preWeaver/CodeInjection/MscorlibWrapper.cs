using System;
using PERWAPI;

namespace preWeaverPERWAPI.CodeInjection
{
	/// <summary>
	/// It wrapps (kinda adapts) the SetPoint framework, so that it can be easily accessed by RAIL objects
	/// </summary>
	public class MscorlibWrapper
	{
		private static MscorlibWrapper _instance = null;
		public static MscorlibWrapper instance
		{
			get
			{
				if (MscorlibWrapper._instance == null)
					MscorlibWrapper._instance = new MscorlibWrapper();
				return MscorlibWrapper._instance;
			}
		}

		private RTypeRef _objectTypeRef = null;
		public RTypeRef objectTypeRef {
			get {
				if (_objectTypeRef == null)
					_objectTypeRef = instance.rAssemblyDef.GetRTypeRef(typeof(Object));
				return _objectTypeRef;
			}
		}

		private RTypeRef _objectArrayTypeRef = null;
		public RTypeRef objectArrayTypeRef {
			get {
				if (_objectArrayTypeRef == null)
					_objectArrayTypeRef = instance.rAssemblyDef.GetRTypeRef(typeof(Object[]));
				return _objectArrayTypeRef;
			}
		}

		private RTypeRef _runtimeTypeHandleTypeRef = null;
		public RTypeRef runtimeTypeHandleTypeRef {
			get {
				if (_runtimeTypeHandleTypeRef == null)
					_runtimeTypeHandleTypeRef = instance.rAssemblyDef.GetRTypeRef(typeof(RuntimeTypeHandle));
				return _runtimeTypeHandleTypeRef;
			}
		}

		private RTypeRef _runtimeMethodHandleTypeRef = null;
		public RTypeRef runtimeMethodHandleTypeRef {
			get {
				if (_runtimeMethodHandleTypeRef == null)
					_runtimeMethodHandleTypeRef = instance.rAssemblyDef.GetRTypeRef(typeof(RuntimeMethodHandle));
				return _runtimeMethodHandleTypeRef;
			}
		}

		private RTypeRef _voidTypeRef = null;
		public RTypeRef voidTypeRef {
			get {
				if (_voidTypeRef == null)
					_voidTypeRef = instance.rAssemblyDef.GetRTypeRef(typeof(void));
				return _voidTypeRef;
			}
		}

		// RAIL equivalents of SetPoint's assembly constituing parts
		private RAssemblyDef rAssemblyDef;
/*		public RTypeDef rJoinPointTypeDef, rWeaverTypeDef;
		public RFieldDef rWeaverInstanceFieldDef;
		public RMethodDef rWeavingMethodMethodDef;
		public RMethodRef rWeavingMethodMethodRef;
		public RFieldRef rWeaverInstanceFieldRef;
		public System.Type WeaverType;
		public RMethod reifyVoidMethodMessage;
		public RMethod reifyMethodMessage;
		public RMethod reifyVoidVirtualMethodMessage;
		public RMethod reifyVirtualMethodMessage;
		public RMethod reifyConstructorMessage;
*/
		/// <summary>
		/// Constructor - loads & parses SetPoint's assembly so that it can be used from RAIL
		/// </summary>
		public MscorlibWrapper()
		{
			rAssemblyDef	  = RAssemblyDef.LoadAssembly("SetPoint.dll");
			/*rJoinPointTypeDef = (RTypeDef)(rAssemblyDef.RModuleDef.GetType("setPoint.messageReifying.JoinPoint"));
			rWeaverTypeDef    = (RTypeDef)(rAssemblyDef.RModuleDef.GetType("setPoint.messageReifying.MessageReifier"));
			rWeaverInstanceFieldDef = (RFieldDef)(rWeaverTypeDef.GetField("instance"));
			rWeavingMethodMethodDef = (RMethodDef)(rWeaverTypeDef.GetMethod("Weave", rAssemblyDef.GetType("System.Object"), new RParameter[0]));			
			System.Reflection.Assembly ass = System.Reflection.Assembly.LoadFrom("SetPoint.dll");
			System.Type weaverType = ass.GetType("setPoint.messageReifying.MessageReifier");		
			WeaverType = weaverType;
			*/
		}
	}
}
