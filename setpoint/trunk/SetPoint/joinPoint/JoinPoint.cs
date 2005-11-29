using System;
using System.Reflection;

namespace setPoint.messageReifying {
	/// <summary>
	/// Abstracts JoinPoint concept.
	/// </summary>
	public abstract class JoinPoint {
		#region properties

		private IObject _sender = null;

		public IObject sender {
			get { return this._sender; }
			set { this._sender = value; }
		}

		private IObject _receiver = null;

		public IObject receiver {
			get { return this._receiver; }
			set { this._receiver = value; }
		}

		private IObject[] _parameters = null;

		public IObject[] parameters {
			get { return this._parameters; }
			set { this._parameters = value; }
		}

		private IMessage _message;

		public IMessage message {
			get { return this._message; }
			set { this._message = value; }
		}

		private object _returnValue = null;

		public object returnValue {
			get { return this._returnValue; }
			set { this._returnValue = value; }
		}

		#endregion

		public JoinPoint(IObject sender, IObject receiver, IMessage message) {
			this.sender = sender;
			this.receiver = receiver;
			this.message = message;			
		}

		//------------------------------------------------------------------		
		/// <summary>
		/// Executes the jointPoint
		/// </summary>
		/// <param name="thisClass"></param>
		/// <returns></returns>
		public abstract void execute();
	}
}