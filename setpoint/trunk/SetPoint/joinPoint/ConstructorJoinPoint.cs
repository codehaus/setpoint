using System;
using System.Reflection;

namespace setPoint.messageReifying {
	/// <summary>
	/// Constructor Call JoinPoint 
	/// </summary>
	public class ConstructorJoinPoint : MethodJoinPoint {
		public ConstructorJoinPoint(IObject sender, IObject receiver, Method message) : base(sender, receiver, message) {}

		public override void execute() {
			ConstructorInfo ci = MethodBase.GetMethodFromHandle(this.method.methodHandle) as ConstructorInfo;	
			this._returnValue = ci.Invoke(this.arguments);
		}
		
	}
}