using System;
using System.Reflection;
using System.Diagnostics;
using System.IO;

namespace setPoint.messageReifying {
	/// <summary>
	/// Method Call JoinPoint 
	/// </summary>
	public class MethodCallJoinPoint : MethodJoinPoint {
		public MethodCallJoinPoint(IObject sender, IObject receiver, Method message) : base(sender, receiver, message) {}

		public override void execute() {			

			MethodInfo mi = MethodInfo.GetMethodFromHandle(this.method.methodHandle) as MethodInfo;
			object o = this.receiver.asMethodInvokeReference();
			this._returnValue = mi.Invoke(o, this.arguments);
		}		
		
	}
}