using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Xml;
using java.io;
using org.openrdf.model;
using org.openrdf.sesame;
using org.openrdf.sesame.admin;
using org.openrdf.sesame.config;
using org.openrdf.sesame.constants;
using org.openrdf.sesame.query;
using org.openrdf.sesame.query.serql.model;
using org.openrdf.sesame.repository;
using org.openrdf.sesame.repository.local;
using org.openrdf.sesame.sail;
using setPoint.configuration;
using setPoint.messageReifying;
using setPoint.util;
using StringWriter = System.IO.StringWriter;
using org.openrdf.sesame.query.serql.parser;

namespace setPoint.semantics
{
	/// <summary>
	/// Summary description for SemanticUniverse.
	/// </summary>
	internal class OntologicalUniverse
	{
		private SesameRepository repository;
		private Hashtable queryCache = new Hashtable();
		private readonly string REPOSITORY_NAME = "SetPoint";
		private readonly AdminMsgCollector messageCollector = new 
AdminMsgCollector();

		private static int iterations = 0;
		internal OntologicalUniverse(CurrentAppDomain currentAppDomain, 
ConfigurationLoader loader)
		{
			loader.hasLoadedConfiguration += new 
ConfigurationLoadingHandler(this.handleConfigurationLoading);

			this.initializeRepository(loader.configuration);
			this.initializeBootstrapping(currentAppDomain);

			this.handleConfigurationLoading(loader.configuration);
		}

		internal bool includes(IJoinPoint jp, SfwQuery query) {
			OntologicalQueryResults includedOntologicalJoinPoints;
			if(this.queryCache.Contains(query))
				includedOntologicalJoinPoints = 
(OntologicalQueryResults)this.queryCache[query];
			else{
				QueryResultsTableBuilder builder = new QueryResultsTableBuilder();
				/*(this.repository as LocalRepository).performSeRQLSelectQuery(
					query, builder);*/
				query.evaluate((this.repository as LocalRepository).getSail() as RdfSource, builder );
				QueryResultsTable resultsTable = builder.getQueryResultsTable();
				includedOntologicalJoinPoints = new 
OntologicalQueryResults(resultsTable);
				this.queryCache.Add(query, includedOntologicalJoinPoints);
			}
				bool res = includedOntologicalJoinPoints.includes(jp);
			return res;
		}

		private void handleConfigurationLoading(IConfiguration configuration) {
			foreach(Ontology ontology in configuration.ontologies)
				this.addToRepository(ontology.rdfXml);
		}

		private void initializeRepository(IConfiguration configuration) {
			RepositoryConfig config = new RepositoryConfig(this.REPOSITORY_NAME);

			SailConfig memSail = new 
SailConfig("org.openrdf.sesame.sailimpl.memory.RdfSchemaRepository");
			memSail.setParameter(org.openrdf.sesame.sailimpl.memory.RdfSchemaRepository.RULES_STRING_KEY,
				this.rulesString(configuration.rules));
			
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
			currentAppDomain.hastJustLoaded += new 
HandleLoading(this.handleAssemblyLoading);
		}

		internal void handleAssemblyLoading(Assembly assembly) {
			iterations++;
			this.addToRepository(this.embeddedOntologyFrom(assembly));
			this.flushQueryCache();
			if(iterations>1)
				this.inferencerPatch();
		}

		private void flushQueryCache() {
			this.queryCache.Clear();
		}

		private void inferencerPatch() {
			/*Graph g = this.repository.performGraphQuery(QueryLanguage.SERQL, 
@"CONSTRUCT DISTINCT" +
				@"    {B} <semantics://programElements/objectOriented/CTS#hasAnnotation> 
{X}" +
				@"FROM" +
				@"    {A} 
<semantics://programElements/objectOriented/CTS#hasProgramElement> {B},    " 
+
				@"    {A} 
<semantics://programElements/objectOriented/CTS#hasAnnotation>" +  @"   
{X}");
			this.repository.addGraph(g);
			g = this.repository.performGraphQuery(QueryLanguage.SERQL, @"CONSTRUCT 
DISTINCT" +
				@"    {B} <semantics://programElements/objectOriented/CTS#hasAnnotation> 
{X}" +
				@"FROM" +
				@"    {A} 
<semantics://programElements/objectOriented/CTS#hasProgramElement> {B},    " 
+
				@"    {A} 
<semantics://programElements/objectOriented/CTS#hasAnnotation>" +  @"   
{X}");
			this.repository.addGraph(g);
			g = this.repository.performGraphQuery(QueryLanguage.SERQL, @"CONSTRUCT 
DISTINCT" +
				@"    {B} <semantics://programElements/objectOriented/CTS#hasAnnotation> 
{X}" +
				@"FROM" +
				@"    {A} 
<semantics://programElements/objectOriented/CTS#hasProgramElement> {B},    " 
+
				@"    {A} 
<semantics://programElements/objectOriented/CTS#hasAnnotation>" +  @"   
{X}");
			this.repository.addGraph(g);*/
			ByteArrayInputStream i = 
(ByteArrayInputStream)this.repository.extractRDF(RDFFormat.RDFXML, true, 
true, false, false);
			System.IO.StringWriter s = new StringWriter();
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
		

		private string embeddedOntologyFrom(Assembly assembly) {
			return (new 
StreamReader(assembly.GetManifestResourceStream("ontology.rdf"))).ReadToEnd();
		}

		private string ooOntology() {
			return (new 
StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("SetPoint.OOOntology.owl"))).ReadToEnd();
		}

		public string rulesString(Rule[] rules) {
			XmlDocument rulesDocument = new XmlDocument();
			rulesDocument.Load(new 
				StreamReader(
					Assembly.GetExecutingAssembly().GetManifestResourceStream("SetPoint.RDFS.xml")));
			foreach(Rule rule in rules){
				XmlElement ruleElement = rulesDocument.CreateElement("rule");								
				ruleElement.AppendChild(rulesDocument.CreateTextNode(rule.SeRQL));
				rulesDocument.DocumentElement.AppendChild(ruleElement);
			}
			return rulesDocument.OuterXml;
		}

	}
}


