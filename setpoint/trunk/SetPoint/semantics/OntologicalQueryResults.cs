using System;
using System.Collections;
using org.openrdf.sesame.query;
using setPoint.messageReifying;

namespace setPoint.semantics
{
	/// <summary>
	/// Summary description for OntologicalQueryResults.
	/// </summary>
	internal class OntologicalQueryResults
	{
		private readonly Hashtable _ontologicalJoinPoints = new Hashtable();
		private readonly ArrayList _columnNames = new ArrayList();

		internal OntologicalQueryResults(QueryResultsTable queryResults)
		{
			this.initializeColumnNames(queryResults);
			this.createTable(queryResults);
		}

		internal bool includes(IJoinPoint jp) {
			return this._ontologicalJoinPoints.Contains(new HashTableComposedKey(jp, this._columnNames));
		}

		private void createTable(QueryResultsTable queryResults) {
			for(int rowNumber=0;rowNumber<queryResults.getRowCount();rowNumber++)				
				this._ontologicalJoinPoints.Add(new HashTableComposedKey(queryResults, rowNumber),null);			
		}

		private void initializeColumnNames(QueryResultsTable queryResults) {
			foreach(string columnName in queryResults.getColumnNames())
				this._columnNames.Add(columnName);
		}		

		internal class HashTableComposedKey {
			private ArrayList _columns = new ArrayList();
		
			internal HashTableComposedKey(QueryResultsTable resultsTable, int rowNumber) {
				for(int columnNumber=0;columnNumber<resultsTable.getColumnCount();columnNumber++){
					string s = resultsTable.getValue(rowNumber, columnNumber).ToString();
					this._columns.Add(s);
				}
			}

			internal HashTableComposedKey(IJoinPoint jp, IList columnNames) {
				foreach(string columnName in columnNames){
					if(columnName=="sender")
						this._columns.Add(jp.sender.uri);
					else if (columnName=="receiver")
						this._columns.Add(jp.receiver.uri);
					else if (columnName=="message")
						this._columns.Add(jp.message.uri);
				}
			}

			public override int GetHashCode() {
				int result=0;				
				foreach(string column in this._columns)
					result += column.GetHashCode();				
				return result;
			}
		 
			public override bool Equals(Object obj) {
				if(!(obj is HashTableComposedKey))
					return false;
				bool result=true;
				HashTableComposedKey cmp = obj as HashTableComposedKey;
				for(int i=0;i<this._columns.Count;i++)
					result = result && this._columns[i].Equals(cmp._columns[i]);				
				return result;
			}
		}
	}	
}
