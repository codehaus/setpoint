using System;

namespace setPoint.messageReifying {
	public class OntologicalJoinPoint {
		internal readonly string senderURI, receiverURI, messageURI;
		
		public OntologicalJoinPoint(string sender, string receiver, string message) {
			this.senderURI = sender;
			this.receiverURI = receiver;
			this.messageURI = message;
		}

		public override int GetHashCode() {
			return this.senderURI.GetHashCode() + this.receiverURI.GetHashCode() + this.messageURI.GetHashCode();
		}

		public override bool Equals(Object obj) {			
			if(!(obj is OntologicalJoinPoint))
				return false;
			OntologicalJoinPoint cmp = obj as OntologicalJoinPoint;
			return this.senderURI.Equals(cmp.senderURI) && 
				   this.receiverURI.Equals(cmp.receiverURI) &&
				   this.messageURI.Equals(cmp.messageURI);
		}
	}
}