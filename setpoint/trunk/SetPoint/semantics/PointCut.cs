using org.openrdf.sesame.query.serql.model;
using org.openrdf.sesame.sail.query;
using java.util;
using setPoint.messageReifying;
using org.openrdf.sesame.query.serql.parser;

namespace setPoint.semantics {
	public class PointCut {		
		private readonly string _predicate;
		public readonly SfwQuery query;		

		public static readonly string SENDER_VAR_NAME = "sender";
		public static readonly string RECEIVER_VAR_NAME = "receiver";
		public static readonly string MESSAGE_VAR_NAME = "message";

		public PointCut(string predicate) {
			this._predicate = predicate += ",{sender} <!http://www.w3.org/1999/02/22-rdf-syntax-ns#type> {<!semantics://programElements/objectOriented/CTS#Class>}"+
					", {receiver} <!http://www.w3.org/1999/02/22-rdf-syntax-ns#type> {<!semantics://programElements/objectOriented/CTS#Class>}" + 
				    ", {message} <!http://www.w3.org/1999/02/22-rdf-syntax-ns#type> {<!semantics://programElements/objectOriented/CTS#MethodBase>}";
			SerqlParser parser = new SerqlParser(
				new java.io.StringReader(
					this.queryStringFrom(predicate)));			
			this.query = parser.parse() as SfwQuery;			
		}

		private string queryStringFrom(string predicate) {
			return "SELECT * FROM " + predicate;
		}

		internal bool includes(IJoinPoint jp, OntologicalUniverse ontologicalUniverse) {
			return ontologicalUniverse.includes(jp, this.query);
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		private List queryProjection() {
			List projection = new Vector();
			projection.add(new Var(SENDER_VAR_NAME));
			/*projection.add(new Var(RECEIVER_VAR_NAME));
			projection.add(new Var(MESSAGE_VAR_NAME));*/
			return projection;
		}
	}
}