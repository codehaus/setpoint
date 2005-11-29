using System.Reflection;
using setPoint.messageReifying;
using setPoint.semantics;

namespace setPoint.weaving {
	/// <summary>
	/// Summary description for ITrigger.
	/// </summary>
	public class Trigger {		
		public readonly PointCutSet on;
		
		private readonly MethodInfo _fires;
		private readonly Advice _parent;
		private readonly bool _isBefore;
		private readonly bool _isAfter;
		
		internal Trigger(Advice parent, MethodInfo fires, bool isBefore, bool isAfter) {
			this._parent = parent;			
			this._fires = fires;
			this._isBefore = isBefore;
			this._isAfter = isAfter;
			this.on = new PointCutSet();
		}

		internal void applyOn(IJoinPoint aJoinPoint) {
			object[] parameters = new object[1];
			parameters[0] = aJoinPoint;
			this._fires.Invoke(this._parent.instanceFor(aJoinPoint), parameters);
		}

		internal bool isBefore {
			get {
				return this._isBefore;
			}
		}

		internal bool isAfter {
			get {
				return this._isAfter;
			}
		}

		internal Advice parent
		{
			get
			{
				return this._parent;
			}
		}

	}
}
