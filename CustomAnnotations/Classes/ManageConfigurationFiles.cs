using HitHelpersNetCore.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace HitCustomAnnotations.Classes
{
    public abstract class ManageConfigurationFiles
    {

        /// <summary>
        /// Dictionary for configuration file
        /// </summary>
        public Dictionary<string, dynamic> ConfigDictionary;

        /// <summary>
        /// Discriptions for each configuration element
        /// </summary>
        public Dictionary<string, List<DescriptorsModel>> DescriptorsDictionary;

        /// <summary>
        /// full path for settings.json
        /// </summary>
        protected string settingsPath;

        /// <summary>
        /// full path for descriptor.json
        /// </summary>
        protected string descriptorPath;

        /// <summary>
        /// folder Path
        /// </summary>
        protected string folderPath;

        /// <summary>
        /// Plugin's path
        /// </summary>
        /// <returns></returns>
        public abstract string PluginBasePath();

        /// <summary>
        /// Plugin unique name based on Plugin Interface, ex: GoogleMaps and TerraMaps plugins will have CategoryName='MapGeocodePlugin'.
        /// If Plugin Interface is not suitable for CategoryName then another description should be selected.
        /// MUST ALWAYS ends WITH suffix 'Plugin' otherwise backoffice will do not recognize as plugin config  ...  
        /// </summary>
        public abstract string CategoryName();

        /// <summary>
        /// Plugin's version
        /// </summary>
        public abstract string Version();

        /// <summary>
        /// lock read, write json files
        /// </summary>
        private object lockJsons = new object();

        public ManageConfigurationFiles()
        {
            getPaths();
            readConfig();
        }

        /// <summary>
        /// Get Paths for each plugin
        /// </summary>
        protected void getPaths()
        {
            string codeBase = PluginBasePath();
            codeBase = codeBase.Substring(0, codeBase.LastIndexOf("/"));
            var uri = new Uri(codeBase).LocalPath;
            settingsPath = uri + "\\settings.json";
            descriptorPath = uri + "\\descriptor.json";
        }

        /// <summary>
        /// Reads configurations
        /// </summary>
        protected void readConfig()
        {
            lock (lockJsons)
            {
                string conf = System.IO.File.ReadAllText(settingsPath, Encoding.Default);
                ConfigDictionary = JsonSerializer.Deserialize<Dictionary<string, dynamic>>(conf);

                string descr = System.IO.File.ReadAllText(descriptorPath, Encoding.Default);
                DescriptorsDictionary = JsonSerializer.Deserialize<Dictionary<string, List<DescriptorsModel>>>(descr);
            }
        } 

        /// <summary>
        /// Add a main dictionary key with plugin's configuration. key must be the unique plugin name
        /// </summary>
        /// <param name="configurationDictionary">main configuration dictionary. Key SHOULD BE the Name of the plugin</param>
        /// <param name="descriptorsDictionary">descriptors dictionary. Key SHOULD BE the Name of the plugin</param>
        public virtual void AppendConfiguration(Dictionary<string, Dictionary<string, dynamic>> configurationDictionary, 
            Dictionary<string, Dictionary<string, List<DescriptorsModel>>> descriptorsDictionary)
        {
            if (ConfigDictionary != null && ConfigDictionary.Keys.Count > 0)
            {
                if (configurationDictionary.ContainsKey(CategoryName())) throw new Exception($"ConfigurationDictionary already contains key '{CategoryName()}'. See plugin {PluginBasePath()}");
                lock (configurationDictionary)
                {
                    configurationDictionary.Add(CategoryName(), ConfigDictionary);
                }
            }

            if (DescriptorsDictionary != null && DescriptorsDictionary.Keys.Count > 0) descriptorsDictionary.Add(CategoryName(), DescriptorsDictionary);
        }

        /// <summary>
        /// Saves configuration changes to plugin configuration directory
        /// </summary>
        /// <param name="configurationDictionary"></param>
        public virtual void SaveConfiguration(Dictionary<string, Dictionary<string, dynamic>> configurationDictionary)
        {
            try
            {
                lock (lockJsons)
                {
                    string json = JsonSerializer.Serialize(configurationDictionary[CategoryName()]);
                    System.IO.File.WriteAllText(settingsPath, json, Encoding.Default);
                    ConfigDictionary = configurationDictionary[CategoryName()];
                }
            }
            catch
            {

            }
        }


    }
}
