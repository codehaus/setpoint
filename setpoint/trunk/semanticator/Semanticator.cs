using System;
using java.io;
using org.openrdf.model;
using org.openrdf.model.impl;
using org.openrdf.rio;
using org.openrdf.rio.rdfxml;
using PERWAPI;
using Type = PERWAPI.Type;

namespace semanticator {
	/// <summary>
	/// Summary description for Semanticator.
	/// </summary>
	public class Semanticator {		
		private PEFile semanticatedFile;
		private RdfDocumentWriter programElementsKB;
		
		private readonly URI TYPE;
		private readonly string CTS = @"semantics://programElements/objectOriented/CTS#";

		public Semanticator() {
			this.TYPE = new URIImpl(@"http://www.w3.org/1999/02/22-rdf-syntax-ns#type");
		}
		/// <summary>
		/// 
		/// </summary>
		public void semanticate(string fileName) {
			
			this.openFile(fileName);
			ByteArrayOutputStream stream = this.initializeKB();

			this.addProgramElement(semanticatedFile.GetThisAssembly());
			this.addProgramElement(semanticatedFile);
			foreach (ClassDef type in this.semanticatedFile.GetClasses()) {
				this.addProgramElement(type);
				foreach(FieldDef field in type.GetFields()) {
					this.addProgramElement(field);
				}
				foreach(MethodDef method in type.GetMethods()) {
					this.addProgramElement(method);
				}
			}

			this.endKB(stream);
			this.closeFile();
		}		

		private void endKB(ByteArrayOutputStream stream) {
			this.programElementsKB.endDocument();
			string siome = stream.toString();
			sbyte[] sontology = stream.toByteArray();
			byte[] ontology = new byte[stream.size()];
			for(int i=0;i<ontology.Length;i++)
				ontology[i] = (byte)(sontology[i]);
	
			this.semanticatedFile.AddManagedResource("ontology.rdf", ontology,true);
		}

		private ByteArrayOutputStream initializeKB() {
			ByteArrayOutputStream stream = new ByteArrayOutputStream();
			this.programElementsKB = new RdfXmlWriter(stream);
			this.programElementsKB.startDocument();
			Resource pe = new URIImpl(moduleURI());
			//this.programElementsKB.writeStatement(pe, this.TYPE, new URIImpl(@"http://www.w3.org/2002/07/owl#Ontology"));
			return stream;
		}

		private string moduleURI() {
			return this.assemblyURI() + "/" + this.semanticatedFile.Name();
		}

		private string assemblyURI() {
			return @"semantics://CTS/"+ this.semanticatedFile.GetThisAssembly().Name();
		}

		private void closeFile() {			
			this.semanticatedFile.WritePEFile(true);
		}

		private void openFile(string fileName) {
			this.semanticatedFile = PEFile.ReadPEFile(fileName);
		}

		#region adding program elements

		private void addProgramElement(Assembly assembly) {
			URI uri = new URIImpl(this.assemblyURI());
			this.programElementsKB.writeStatement(uri, this.TYPE, new URIImpl(this.CTS+"Assembly"));

			this.addAnnotations(assembly, uri);
		}

		private void addAnnotations(MetaDataElement metaDataElement, URI uri)
		{
			foreach(CustomAttribute att in metaDataElement.GetCustomAttributes()) {
				if((att.GetCAType().GetParent() as Class).Name()=="ProgramAnnotationAttribute") {
					this.programElementsKB.writeStatement(new URIImpl(uri.getURI()), new URIImpl(this.CTS+"hasAnnotation"), new URIImpl((att.Args[0] as StringConst).GetVal()));
				}
			}
		}

		private void addProgramElement(Module module) {
			URI uri = new URIImpl(this.moduleURI());
			this.programElementsKB.writeStatement(uri, this.TYPE, new URIImpl(this.CTS+"Module"));

			this.programElementsKB.writeStatement(new URIImpl(this.assemblyURI()), new URIImpl(this.CTS+"hasModule"),uri);
		}		

		private void addProgramElement(ClassDef type) {
			URI uri = new URIImpl(this.moduleURI() + "#" + type.NameSpace() + "." + type.Name());				
			this.programElementsKB.writeStatement(uri, this.TYPE, new URIImpl(this.CTS+"Class"));
			if(type.SuperType.Name()=="MulticastDelegate")
				this.programElementsKB.writeStatement(uri, new URIImpl(this.CTS+"isDelegate"),new URIImpl(this.CTS+"true"));
			else
				this.programElementsKB.writeStatement(uri, new URIImpl(this.CTS+"isDelegate"),new URIImpl(this.CTS+"false"));
			this.programElementsKB.writeStatement(new URIImpl(this.moduleURI()), new URIImpl(this.CTS+"hasType"),uri);
		}

		private void addProgramElement(FieldDef field) {
			ClassDef parentClass = field.GetParent() as ClassDef;
			string classURI = this.moduleURI() + "#" + parentClass.NameSpace() + "." + parentClass.Name();
			URI uri = new URIImpl(classURI+".."+field.Name());
			this.programElementsKB.writeStatement(uri, this.TYPE, new URIImpl(this.CTS+"Field"));

			this.programElementsKB.writeStatement(new URIImpl(classURI), new URIImpl(this.CTS+"hasMember"),uri);
		}

		private void addProgramElement(MethodDef method) {
			
			ClassDef parentClass = method.GetParent() as ClassDef;
			string classURI = this.moduleURI() + "#" + parentClass.NameSpace() + "." + parentClass.Name();
			URI uri = new URIImpl(classURI+".."+method.Name());			
			if(method.Name() == ".ctor" || method.Name() == ".cctor")
				this.programElementsKB.writeStatement(uri, this.TYPE, new URIImpl(this.CTS+"Constructor"));
			else
				this.programElementsKB.writeStatement(uri, this.TYPE, new URIImpl(this.CTS+"Method"));

			this.programElementsKB.writeStatement(new URIImpl(classURI), new URIImpl(this.CTS+"hasMember"),uri);

			this.addAnnotations(method, uri);
		}

		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		private static void Main(string[] args) {
			new Semanticator().semanticate("TestLibrary.exe");
		}
	}
}