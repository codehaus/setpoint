using System;
using System.Reflection;
using setPoint.weaving;

namespace setPoint.messageReifying {
	/// <summary>
	/// Turns every "message sent" (method/constructor call, field access, etc) into an object
	/// </summary>
	public class MessageReifier {		
		
		#region public interface

		#region instance messages		

		public JoinPoint reifyMethodMessage(object sender, object receiver, RuntimeMethodHandle methodHandle, Object[] parameters) {
			JoinPoint jp = new MethodJoinPoint(sender, receiver, methodHandle, parameters);
			this.weaver.weave(jp);			
			return jp.returnValue;			
		}

		public void reifyVoidMethodMessage(object sender, object receiver, RuntimeMethodHandle methodHandle, Object[] parameters) {
			this.reifyMethodMessage(sender,receiver,methodHandle,parameters);
		}
		#endregion

		#region virtual instance messages		

		public object reifyVirtualMethodMessage(object sender, object receiver, RuntimeMethodHandle methodHandle, Object[] parameters) {
			JoinPoint jp = new MethodJoinPoint(sender, receiver, methodHandle, parameters);
			this.weaver.weave(jp);			
			return jp.returnValue;			
		}

		public void reifyVoidVirtualMethodMessage(object sender, object receiver, RuntimeMethodHandle methodHandle, Object[] parameters) {
			this.reifyMethodMessage(sender,receiver,methodHandle,parameters);
		}
		#endregion

		#region constructors 				

		public object reifyConstructorMessage(object sender, object receiver, RuntimeMethodHandle methodHandle, Object[] parameters) {
			JoinPoint jp = new ConstructorJoinPoint(sender, receiver, methodHandle, parameters);
			this.weaver.weave(jp);			
			return jp.returnValue;
		}		

		#endregion

		#endregion
	}
}