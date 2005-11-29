using System;

namespace setPoint.programAnnotation
{
	/// <summary>
	/// Summary description for ProgramAnnotationAttribute.
	/// </summary>
	public class ProgramAnnotationAttribute:Attribute
	{
		private readonly string uri;

		public ProgramAnnotationAttribute(string uri)
		{
			this.uri = uri;
		}
	}
}
