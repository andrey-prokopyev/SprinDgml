namespace SprinDgml
{
    using System;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    public class ConfigurationFileLoader
    {
        public void Load(string configFileName)
        {
            var configFlePath = this.GetConfigFilePath(configFileName);
            
            AppDomain.CurrentDomain.SetData("APP_CONFIG_FILE", configFlePath);
            ResetConfigMechanism();

            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomainOnAssemblyResolve;
        }

        private void ResetConfigMechanism()
        {
            var type = typeof(ConfigurationManager);
            type.GetField("s_initState", BindingFlags.NonPublic | BindingFlags.Static)?.SetValue(null, 0);
            type.GetField("s_configSystem", BindingFlags.NonPublic | BindingFlags.Static)?.SetValue(null, null);
            type.Assembly.GetTypes().First(x => x.FullName == "System.Configuration.ClientConfigPaths").GetField("s_current", BindingFlags.NonPublic | BindingFlags.Static)?.SetValue(null, null);
        }

        private Assembly CurrentDomainOnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            var assemblyName = new AssemblyName(args.Name);

            var assemblyPath = Path.Combine(Environment.CurrentDirectory, assemblyName.Name + ".dll");

            var assembly = Assembly.LoadFrom(assemblyPath);

            return assembly;
        }

        private string GetConfigFilePath(string configFileName)
        {
            if (string.IsNullOrEmpty(configFileName))
            {
                var configFiles = Directory.GetFiles(Environment.CurrentDirectory, "*.config", SearchOption.TopDirectoryOnly);
                if (!configFiles.Any())
                {
                    return null;
                }

                return configFiles.First();
            }

            return Path.Combine(Environment.CurrentDirectory, configFileName);
        }
    }
}