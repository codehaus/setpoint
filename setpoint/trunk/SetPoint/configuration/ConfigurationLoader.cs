using System.IO;
using System.Reflection;
using System;
using System.Security;
using System.Security.Policy;
using System.Collections;

namespace setPoint.configuration
{
	/// <summary>
	/// 
	/// </summary>
	delegate void ConfigurationLoadingHandler(IConfiguration loadedConfiguration);	
	
	/// <summary>
	/// Summary description for ConfigurationLoader.
	/// </summary>
	public class ConfigurationLoader
	{
		IConfiguration _configuration;
		AppDomain _appDomain;
		public ConfigurationRemoteLoader _remoteLoader;

		public ConfigurationLoader()
		{
			FileInfo configurationFile = new FileInfo(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\SetPoint.config.dll");
			if(File.Exists( configurationFile.FullName)){
				FileSystemWatcher configurationWatcher = new FileSystemWatcher(configurationFile.DirectoryName, configurationFile.Name);
				this._configuration = this.load(configurationFile);
			}
			else
				this._configuration = new NullConfiguration();
		}

		internal event ConfigurationLoadingHandler hasLoadedConfiguration;

		internal IConfiguration configuration {
			get {				
				return _configuration;
			}
		}

		private IConfiguration load(FileInfo fileInfo) {
			AppDomainSetup setup = new AppDomainSetup();			
			setup.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;			
			setup.ApplicationName = "Config";
			setup.ShadowCopyFiles = "true";
			setup.ShadowCopyDirectories = Directory.GetCurrentDirectory();			
			//setup.CachePath = Directory.GetCurrentDirectory() + "\\shadow";
			_appDomain = AppDomain.CreateDomain(
				"SetPointConfig", null, setup);
			this._remoteLoader = new ConfigurationRemoteLoader();
			/*this._remoteLoader = (ConfigurationRemoteLoader) 
				_appDomain.CreateInstanceAndUnwrap(
				"SetPoint",
				"setPoint.configuration.ConfigurationRemoteLoader");
			*/
			IConfiguration result = this._remoteLoader.LoadAssembly(fileInfo.FullName);						
			
			return result;
		}
		


	public void Unload() {
	AppDomain.Unload(_appDomain);
	_appDomain = null;
}
	}
}
