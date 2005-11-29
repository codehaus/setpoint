using System;
using System.Reflection;

namespace setPoint.messageReifying {
	/// <summary>
	/// Abstracts JoinPoint concept.
	/// </summary>
	public interface IJoinPoint {
		
		IObject sender {
			get;
		}
		
		IObject receiver {
			get;
		}

		IMessage message {
			get;
		}

		object returnValue {
			get;set;
		}
		
		object[] arguments {
			get;
		}
		//------------------------------------------------------------------		
		/// <summary>
		/// Executes the jointPoint
		/// </summary>
		/// <param name="thisClass"></param>
		/// <returns></returns>
		void execute();
	}
}