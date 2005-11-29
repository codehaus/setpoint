using System;
using System.Reflection;

namespace setPoint.messageReifying {
	/// <summary>
	/// Constructor Call JoinPoint 
	/// </summary>
	public abstract class MethodJoinPoint : IJoinPoint{
		protected Method _message;
		protected IObject _sender;
		protected IObject _receiver;
		protected object _returnValue;
		private OntologicalJoinPoint _ontologicalJoinPoint;		
		
		public IObject sender {
			get {
				return _sender;
			}
		}

		public IObject receiver {
			get {
				return _receiver;
			}
		}

		public IMessage message {
			get {
				return _message;
			}
		}

		public Method method {
			get {
				return _message as Method;
			}
		}

		public object returnValue {
			get {
				return _returnValue;
			}
			set {
				_returnValue = value;
			}
		}

		public object[] arguments {
			get {
				return this.method.arguments;
			}
		}

		public OntologicalJoinPoint ontologicalJoinPoint{
			get {
				return _ontologicalJoinPoint;
			}
		}

		public MethodJoinPoint(IObject sender, IObject receiver, Method message) {
			this._message = message;
			this._receiver = receiver;
			this._sender = sender;
			this._ontologicalJoinPoint = new OntologicalJoinPoint(sender.uri, receiver.uri, message.uri);
		}

		public override int GetHashCode() {
			return this.sender.uri.GetHashCode() + this.receiver.uri.GetHashCode() + this.message.uri.GetHashCode();
		}

		 
		public override bool Equals(Object obj) {			
			return this._ontologicalJoinPoint.Equals(obj);
		}


		public abstract void execute();
				
	}
}