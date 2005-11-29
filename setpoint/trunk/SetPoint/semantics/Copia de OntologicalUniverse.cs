using System.IO;
using System.Reflection;
using java.io;
using org.openrdf.sesame;
using org.openrdf.sesame.config;
using org.openrdf.sesame.repository.local;
using org.openrdf.sesame.repository;
using org.openrdf.sesame.admin;
using org.openrdf.sesame.query;
using org.openrdf.sesame.constants;
using org.openrdf.sesame.query.serql.model;
using org.openrdf.model;
using setPoint.configuration;
using setPoint.messageReifying;
using setPoint.util;
using StringWriter = System.IO.StringWriter;
using System.Collections;

namespace setPoint.semantics
{
	/// <summary>
	/// Summary description for SemanticUniverse.
	/// </summary>
	internal class SemanticUniverse
	{
		private SesameRepository repository;
		private Hashtable queryCache = new Hashtable();
		private readonly string REPOSITORY_NAME = "SetPoint";
		private readonly AdminMsgCollector messageCollector = new AdminMsgCollector();

		internal SemanticUniverse(CurrentAppDomain currentAppDomain, ConfigurationLoader loader)
		{
			loader.hasLoadedConfiguration += new ConfigurationLoadingHandler(this.handleConfigurationLoading);
			this.handleConfigurationLoading(loader.configuration);

			this.initializeRepository();

			this.initializeBootstrapping(currentAppDomain);
		}

		internal bool queryHasAnyResult(SfwQuery query, IJoinPoint jp) {			
			QueryResultsTable resultsTable;
			if(this.queryCache.Contains(query))
				resultsTable = (QueryResultsTable)this.queryCache[query];
			else{
				QueryResultsTableBuilder builder = new QueryResultsTableBuilder();				
				(this.repository as LocalRepository).performSeRQLSelectQuery(query, builder);
				resultsTable = builder.getQueryResultsTable();
				/*resultsTable = (this.repository as LocalRepository).performTableQuery(QueryLanguage.SERQL, 
					"SELECT * FROM {sender} <!semantics://programElements/objectOriented/CTS#hasAnnotation> {<!semantics://perspectives/architecture#view>},"+
					"{receiver} <!semantics://programElements/objectOriented/CTS#hasAnnotation> {<!semantics://perspectives/architecture#model>}");*/
				this.queryCache.Add(query, resultsTable);
			}
			
			Value v;
			if(resultsTable.getRowCount()>0){
				 v= resultsTable.getValue(0,0);
			}
			return resultsTable.getRowCount()>0;
		}

		private void handleConfigurationLoading(IConfiguration configuration) {
			// Ontology loading
		}

		private void initializeRepository() {
			RepositoryConfig config = new RepositoryConfig(this.REPOSITORY_NAME);
				
			SailConfig memSail = new SailConfig("org.openrdf.sesame.sailimpl.memory.RdfSchemaRepository");
		
			config.addSail(memSail);
			config.setWorldReadable(true);
			config.setWorldWriteable(true);
	
			LocalService service = Sesame.getService();
			this.repository = service.createRepository(config);
	
			this.addToRepository(this.ooOntology());
		}

		private void initializeBootstrapping(CurrentAppDomain currentAppDomain) {
			foreach (Assembly assembly in currentAppDomain.loadedAssemblies) {
				this.handleAssemblyLoading(assembly);
			}
			currentAppDomain.hastJustLoaded += new HandleLoading(this.handleAssemblyLoading);
		}

		internal void handleAssemblyLoading(Assembly assembly) {						
			this.addToRepository(this.embeddedOntologyFrom(assembly));			
			this.flushQueryCache();
		}

		private void flushQueryCache() {
			this.queryCache.Clear();	
		}

		private void inferencerPatch() {
			Graph g = this.repository.performGraphQuery(QueryLanguage.SERQL, @"CONSTRUCT DISTINCT" + 
				@"    {B} <semantics://programElements/objectOriented/CTS#hasAnnotation> {X}" + 
				@"FROM" + 
				@"    {A} <semantics://programElements/objectOriented/CTS#hasProgramElement> {B},    " + 
				@"    {A} <semantics://programElements/objectOriented/CTS#hasAnnotation>" +  @"   {X}");
			this.repository.addGraph(g);
			g = this.repository.performGraphQuery(QueryLanguage.SERQL, @"CONSTRUCT DISTINCT" + 
				@"    {B} <semantics://programElements/objectOriented/CTS#hasAnnotation> {X}" + 
				@"FROM" + 
				@"    {A} <semantics://programElements/objectOriented/CTS#hasProgramElement> {B},    " + 
				@"    {A} <semantics://programElements/objectOriented/CTS#hasAnnotation>" +  @"   {X}");
			this.repository.addGraph(g);
			g = this.repository.performGraphQuery(QueryLanguage.SERQL, @"CONSTRUCT DISTINCT" + 
				@"    {B} <semantics://programElements/objectOriented/CTS#hasAnnotation> {X}" + 
				@"FROM" + 
				@"    {A} <semantics://programElements/objectOriented/CTS#hasProgramElement> {B},    " + 
				@"    {A} <semantics://programElements/objectOriented/CTS#hasAnnotation>" +  @"   {X}");
			this.repository.addGraph(g);
			java.io.ByteArrayInputStream i = (java.io.ByteArrayInputStream)this.repository.extractRDF(RDFFormat.RDFXML, true, true, false, false);
			StringWriter s = new StringWriter();
			int c,count;
			count = i.available();
			for(int j=0;j<count;j++)
				s.Write((char)i.read());			
			string sg = s.ToString();			
			StreamWriter sw = new StreamWriter(@"d:\current.owl");
			sw.Write(sg);
			sw.Close();
		}

		private void addToRepository(string rdfDoc) {
			this.repository.addData(rdfDoc,"", 
			                        RDFFormat.RDFXML,false,this.messageCollector);
		}

		internal SfwQuery parseQueryString(string queryString) {
			return (this.repository as LocalRepository).parseSeRQLQueryString(queryString);
		}

		private string embeddedOntologyFrom(Assembly assembly) {
			return (new StreamReader(assembly.GetManifestResourceStream("ontology.rdf"))).ReadToEnd();
		}

		private string ooOntology() {			
			return (new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("SetPoint.OOOntology.owl"))).ReadToEnd();
		}

	}
}
