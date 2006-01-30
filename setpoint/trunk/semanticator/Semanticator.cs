using System;
using System.Text;
using java.io;
using Mono.Cecil;
using org.openrdf.model;
using org.openrdf.model.impl;
using org.openrdf.rio;
using org.openrdf.rio.rdfxml;

namespace semanticator {
	public class Semanticator {
		private AssemblyDefinition semanticatedFile;
		private RdfDocumentWriter programElementsKB;

		private readonly URI TYPE;
		private readonly string CTS = @"semantics://programElements/objectOriented/CTS#";

		public Semanticator() {
			this.TYPE = new URIImpl(@"http://www.w3.org/1999/02/22-rdf-syntax-ns#type");
		}

		public void semanticate(string fileName) {
			string ontologyString = sourceCodeSemantics(fileName);
			addOntologyToAssembly(fileName, ontologyString);
		}

		public string sourceCodeSemantics(string fileName) {
			this.openFile(fileName);
			ByteArrayOutputStream stream = this.initializeKB();
			this.addProgramElement(semanticatedFile);
			this.addProgramElement(semanticatedFile.MainModule);
			// Not yet suporting multimodule assemblies
			/*foreach (ModuleDefinition module in semanticatedFile.Modules) {
				addModuleSemantics(module);
			}*/
			addModuleSemantics(semanticatedFile.MainModule);
			this.programElementsKB.endDocument();
			return stream.toString();
		}

		private void addModuleSemantics(ModuleDefinition module) {
			foreach (TypeDefinition type in module.Types) {
				if (type.Name != "<Module>") {
					addProgramElement(type);
					addFieldsSemantics(type);
					addMethodsSemantics(type);
				}
			}
		}

		private void addMethodsSemantics(TypeDefinition type) {
			foreach (MethodDefinition constructor in type.Constructors) {
				this.addProgramElement(constructor);
			}
			foreach (MethodDefinition method in type.Methods) {
				this.addProgramElement(method);
			}
		}

		private void addFieldsSemantics(TypeDefinition type) {
			foreach (FieldDefinition field in type.Fields) {
				this.addProgramElement(field);
			}
		}

		public void addOntologyToAssembly(string fileName, string ontologyString) {
			this.openFile(fileName);
			byte[] ontologyData = Encoding.Default.GetBytes(ontologyString);
			addEmbeddedResource("ontology.rdf", ontologyData);
			this.closeFile(fileName);
		}

		private void addEmbeddedResource(string name, byte[] data) {
			EmbeddedResource resource = new EmbeddedResource(name, ManifestResourceAttributes.Public, data);
			this.semanticatedFile.MainModule.Resources.Add(resource);
		}

		private ByteArrayOutputStream initializeKB() {
			ByteArrayOutputStream stream = new ByteArrayOutputStream();
			this.programElementsKB = new RdfXmlWriter(stream);
			this.programElementsKB.startDocument();
			//Resource pe = new URIImpl(moduleURI());
			//this.programElementsKB.writeStatement(pe, this.TYPE, new URIImpl(@"http://www.w3.org/2002/07/owl#Ontology"));
			return stream;
		}

		private void openFile(string fileName) {
			this.semanticatedFile = AssemblyFactory.GetAssembly(fileName);
		}

		private void closeFile(string fileName) {
			AssemblyFactory.SaveAssembly(this.semanticatedFile, fileName);
		}

		#region adding program elements

		private void addProgramElement(AssemblyDefinition assembly) {
			URI uri = new URIImpl(this.assemblyURI());
			this.programElementsKB.writeStatement(uri, this.TYPE, new URIImpl(this.CTS + "Assembly"));
			this.addAnnotations(assembly, uri);
		}

		private void addProgramElement(ModuleDefinition module) {
			URI uri = new URIImpl(this.moduleURI());
			this.programElementsKB.writeStatement(uri, this.TYPE, new URIImpl(this.CTS + "Module"));
			this.programElementsKB.writeStatement(new URIImpl(this.assemblyURI()), new URIImpl(this.CTS + "hasModule"), uri);
		}

		private void addProgramElement(TypeDefinition type) {
			URI uri = new URIImpl(this.moduleURI() + "#" + type.Namespace + "." + type.Name);
			this.programElementsKB.writeStatement(uri, this.TYPE, new URIImpl(this.CTS + "Class"));

			if (type.BaseType.Name == "MulticastDelegate")
				this.programElementsKB.writeStatement(uri, new URIImpl(this.CTS + "isDelegate"), new URIImpl(this.CTS + "true"));
			else
				this.programElementsKB.writeStatement(uri, new URIImpl(this.CTS + "isDelegate"), new URIImpl(this.CTS + "false"));
			this.programElementsKB.writeStatement(new URIImpl(this.moduleURI()), new URIImpl(this.CTS + "hasType"), uri);
		}

		private void addProgramElement(FieldDefinition field) {
			TypeReference parentClass = field.DeclaringType;
			string classURI = classUri(parentClass);
			URI uri = new URIImpl(classURI + ".." + field.Name);
			this.programElementsKB.writeStatement(uri, this.TYPE, new URIImpl(this.CTS + "Field"));
			this.programElementsKB.writeStatement(new URIImpl(classURI), new URIImpl(this.CTS + "hasMember"), uri);
		}

		public void addProgramElement(MethodDefinition method) {
			TypeReference parentClass = method.DeclaringType;
			string classURI = classUri(parentClass);
			URI uri = new URIImpl(methodUri(classURI, method));
			if (method.IsConstructor)
				this.programElementsKB.writeStatement(uri, this.TYPE, new URIImpl(this.CTS + "Constructor"));
			else
				this.programElementsKB.writeStatement(uri, this.TYPE, new URIImpl(this.CTS + "Method"));
			this.programElementsKB.writeStatement(new URIImpl(classURI),
			                                      new URIImpl(this.CTS + "hasMember"), uri);
			this.addAnnotations(method, uri);
		}

		private string moduleURI() {
			return this.assemblyURI() + "/" + this.semanticatedFile.MainModule.Name;
		}

		private string assemblyURI() {
			return @"semantics://CTS/" + this.semanticatedFile.Name.Name;
		}

		private string methodUri(string declaringClassUri, MethodDefinition method) {
			/*
			string methodIdentifier = method.Name;
			foreach (ParameterDefinition param in method.Parameters) {
				methodIdentifier += ":" + param.ParameterType.Name;
			}
			return declaringClassUri + ".." + methodIdentifier;
			*/
			return declaringClassUri + ".." + method.Name;
		}

		private string classUri(TypeReference type) {
			return this.moduleURI() + "#" + type.FullName;
		}

		private void addAnnotations(ICustomAttributeProvider attProvider, URI uri) {
			foreach (CustomAttribute att in attProvider.CustomAttributes) {
				if (att.Constructor.DeclaringType.Name == "ProgramAnnotationAttribute") {
					this.programElementsKB.writeStatement(new URIImpl(uri.getURI()),
					                                      new URIImpl(this.CTS + "hasAnnotation"),
					                                      new URIImpl(att.ConstructorParameters[0] as string));
				}
			}
		}

		#endregion

		[STAThread]
		public static void Main(string[] args) {
			new semanticator.Semanticator().semanticate(args[0]);
		}
	}
}