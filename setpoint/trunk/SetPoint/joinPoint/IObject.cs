using System;

namespace setPoint.messageReifying
{
	/// <summary>
	/// Summary description for IObject.
	/// </summary>
	public interface IObject
	{
		object asMethodInvokeReference();

		string uri{get;}
		Type type{get;} 
	} 

}
