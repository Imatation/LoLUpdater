using System.IO;
using System.Linq;

namespace LoLUpdater
{
    internal class LoLFiles
    {
        public LoLFiles()
        {
            if (Directory.Exists("RADS"))
            {
                GameVersion = "RADS";
            }
            else if (Directory.Exists("Game"))
            {
                GameVersion = "Game";
            }
        }

        public string GameVersion { get; set; }

        public string BackupCgPath
        {
            get { return Path.Combine("Backup", "Cg.dll"); }
        }

        public string BackupCgGlPath
        {
            get { return Path.Combine("Backup", "CgGL.dll"); }
        }

        public string BackupCgD3D9Path
        {
            get { return Path.Combine("Backup", "CgD3D9.dll"); }
        }

        public string BackupTbbPath
        {
            get { return Path.Combine("Backup", "tbb.dll"); }
        }

        public string BackupNpswf32Path
        {
            get { return Path.Combine("Backup", "NPSWF32.dll"); }
        }

        public string BackupAdobeAirPath
        {
            get { return Path.Combine("Backup", "Adobe AIR.dll"); }
        }

        public string LolGameClientSlnPath
        {
            get
            {
                return Path.Combine("RADS", "solutions", "lol_game_client_sln", "releases") + @"\" +
                       new DirectoryInfo(Path.Combine("RADS", "solutions", "lol_game_client_sln", "releases"))
                           .GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\";
            }
        }

        public string LolAirClientPath
        {
            get
            {
                return Path.Combine("RADS", "projects", "lol_air_client", "releases") + @"\" +
                       new DirectoryInfo(Path.Combine("RADS", "projects", "lol_air_client", "releases")).GetDirectories()
                           .OrderByDescending(d => d.CreationTime)
                           .FirstOrDefault() + @"\";
            }
        }

        public string DeployCgPath
        {
            get { return Path.Combine("deploy", "Cg.dll"); }
        }

        public string DeployCgGlPath
        {
            get { return Path.Combine("deploy", "CgGL.dll"); }
        }

        public string DeployCgD3D9Path
        {
            get { return Path.Combine("deploy", "CgD3D9.dll"); }
        }

        public string DeployTbbPath
        {
            get { return Path.Combine("deploy", "tbb.dll"); }
        }

        public string DeployAdobeAirPath
        {
            get
            {
                return GameVersion.Equals("RADS")
                    ? Path.Combine("deploy", "Adobe Air", "Versions", "1.0", "Adobe AIR.dll")
                    : Path.Combine("Air", "Adobe AIR", "Versions", "1.0", "Adobe AIR.dll");
            }
        }

        public string DeployNpswf32Path
        {
            get
            {
                return GameVersion.Equals("RADS")
                    ? Path.Combine("deploy", "Adobe Air", "Versions", "1.0", "Resources", "NPSWF32.dll")
                    : Path.Combine("Air", "Adobe AIR", "Versions", "1.0", "Resources", "NPSWF32.dll");
            }
        }
    }
}