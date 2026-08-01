using LeagueOfSmurfs.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace LeagueOfSmurfs.Configurations
{
    public class ConfigurationManager
    {
        private const string PasswordKey = "JulienSuçoteMoiMesPetitesBoursesVelues";
        private const string UsernameKey = "JeCiteElleJlaDetruitDansLesBuissons";

        private readonly List<SmurfsConfiguration> accountsConfiguration;
        private readonly string dirPath;

        public string apiKey;

        public ConfigurationManager()
        {
            this.accountsConfiguration = new List<SmurfsConfiguration>();
            this.dirPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".los");
            EnsureDirectory();
            LoadApi();
            Load();
        }

        public List<SmurfsConfiguration> Get()
        {
            return this.accountsConfiguration;
        }

        public bool IsValidAccount(SmurfsConfiguration conf)
        {
            return conf != null
                && !string.IsNullOrWhiteSpace(conf.puuid)
                && !string.IsNullOrWhiteSpace(conf.summonerName)
                && !string.IsNullOrWhiteSpace(conf.username)
                && !string.IsNullOrWhiteSpace(conf.password);
        }

        /// <summary>
        /// Adds or replaces an account. Refuses empty puuid (prevents ".yml" and mass-deletes).
        /// </summary>
        public bool Add(SmurfsConfiguration conf)
        {
            if (!IsValidAccount(conf))
            {
                Debug.WriteLine("Refused to save account: missing puuid/name/credentials");
                return false;
            }

            conf.puuid = conf.puuid.Trim();

            // Replace existing with same puuid only (never match empty/null)
            SmurfsConfiguration existing = this.accountsConfiguration
                .FirstOrDefault(c => !string.IsNullOrWhiteSpace(c.puuid) && c.puuid == conf.puuid);
            if (existing != null)
            {
                this.accountsConfiguration.Remove(existing);
                // File will be overwritten by SaveFile — do not delete then recreate
            }

            this.accountsConfiguration.Add(conf);
            return SaveFile(conf);
        }

        /// <summary>
        /// Updates ranked/profile fields in place without deleting the yaml first.
        /// </summary>
        public bool UpdateAccount(SmurfsConfiguration conf)
        {
            if (!IsValidAccount(conf))
            {
                Debug.WriteLine("Refused to update account: invalid config");
                return false;
            }

            SmurfsConfiguration existing = this.accountsConfiguration
                .FirstOrDefault(c => c.puuid == conf.puuid);
            if (existing == null)
                this.accountsConfiguration.Add(conf);
            else if (!ReferenceEquals(existing, conf))
            {
                int index = this.accountsConfiguration.IndexOf(existing);
                this.accountsConfiguration[index] = conf;
            }

            return SaveFile(conf);
        }

        public void Remove(SmurfsConfiguration conf)
        {
            if (conf == null)
                return;

            this.accountsConfiguration.Remove(conf);

            if (string.IsNullOrWhiteSpace(conf.puuid))
            {
                Debug.WriteLine("Skip file delete: empty puuid (would target '.yml')");
                return;
            }

            string path = GetAccountPath(conf.puuid);
            try
            {
                if (File.Exists(path))
                    File.Delete(path);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to delete account file: " + ex.Message);
            }
        }

        public void LoadApi()
        {
            string path = Path.Combine(dirPath, "api.key");
            if (File.Exists(path))
                apiKey = File.ReadAllText(path);
        }

        public void SaveApi()
        {
            EnsureDirectory();
            File.WriteAllText(Path.Combine(dirPath, "api.key"), this.apiKey ?? string.Empty);
        }

        public void ClearApi()
        {
            apiKey = string.Empty;
            string path = Path.Combine(dirPath, "api.key");
            if (File.Exists(path))
                File.Delete(path);
        }

        public void Load()
        {
            EnsureDirectory();
            CleanupOrphanFiles();

            IDeserializer deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .IgnoreUnmatchedProperties()
                .Build();

            foreach (string filePath in Directory.GetFiles(dirPath, "*.yml"))
            {
                string fileName = Path.GetFileNameWithoutExtension(filePath);
                if (string.IsNullOrWhiteSpace(fileName))
                {
                    TryDelete(filePath);
                    continue;
                }

                try
                {
                    string yaml = File.ReadAllText(filePath);
                    if (string.IsNullOrWhiteSpace(yaml))
                    {
                        TryDelete(filePath);
                        continue;
                    }

                    SmurfsConfiguration config = deserializer.Deserialize<SmurfsConfiguration>(yaml);
                    if (config == null || string.IsNullOrWhiteSpace(config.puuid))
                    {
                        Debug.WriteLine("Skipping invalid account file (no puuid): " + filePath);
                        TryDelete(filePath);
                        continue;
                    }

                    // Prefer filename as source of truth for puuid
                    if (!string.Equals(config.puuid, fileName, StringComparison.Ordinal))
                        config.puuid = fileName;

                    if (string.IsNullOrWhiteSpace(config.summonerName))
                        config.summonerName = config.puuid;

                    config.password = SafeDecrypt(config.password, PasswordKey);
                    config.username = SafeDecrypt(config.username, UsernameKey);

                    this.accountsConfiguration.Add(config);
                    Debug.WriteLine("Loaded file: " + filePath);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error loading file " + filePath + ": " + ex.Message);
                }
            }
        }

        public void Save()
        {
            foreach (SmurfsConfiguration config in accountsConfiguration.ToList())
                SaveFile(config);
        }

        public bool SaveFile(SmurfsConfiguration config)
        {
            if (config == null || string.IsNullOrWhiteSpace(config.puuid))
            {
                Debug.WriteLine("Refused SaveFile: empty puuid");
                return false;
            }

            EnsureDirectory();
            string path = GetAccountPath(config.puuid);

            try
            {
                string plainUser = config.username ?? string.Empty;
                string plainPass = config.password ?? string.Empty;

                config.username = Encryption.Encrypt(plainUser, UsernameKey);
                config.password = Encryption.Encrypt(plainPass, PasswordKey);

                ISerializer serializer = new SerializerBuilder()
                    .WithNamingConvention(CamelCaseNamingConvention.Instance)
                    .Build();
                File.WriteAllText(path, serializer.Serialize(config));

                config.username = plainUser;
                config.password = plainPass;

                Debug.WriteLine("Credentials saved: " + path);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("SaveFile failed: " + ex.Message);
                // Best effort restore plaintext in memory
                try
                {
                    config.username = SafeDecrypt(config.username, UsernameKey);
                    config.password = SafeDecrypt(config.password, PasswordKey);
                }
                catch { /* ignore */ }
                return false;
            }
        }

        private string GetAccountPath(string puuid)
        {
            return Path.Combine(dirPath, puuid.Trim() + ".yml");
        }

        private void EnsureDirectory()
        {
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
                Debug.WriteLine("Created credentials directory");
            }
        }

        private void CleanupOrphanFiles()
        {
            // Remove historic bug artifact: AppData\.los\.yml
            string orphan = Path.Combine(dirPath, ".yml");
            TryDelete(orphan);
        }

        private static void TryDelete(string path)
        {
            try
            {
                if (File.Exists(path))
                    File.Delete(path);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Could not delete " + path + ": " + ex.Message);
            }
        }

        private static string SafeDecrypt(string value, string key)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;
            try
            {
                return Encryption.Decrypt(value, key);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Decrypt failed, keeping raw value: " + ex.Message);
                return value;
            }
        }
    }
}
