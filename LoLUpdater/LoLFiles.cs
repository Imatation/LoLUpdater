using System;
using System.IO;
using System.Linq;

namespace LoLUpdater
{
    class LoLFiles
    {
        public LoLFiles()
        {
            if (Directory.Exists("RADS"))
            {
                gameVersion = "RADS";
            }
            else if (Directory.Exists("Game"))
            {
                gameVersion = "Game";
            }
        }

        public string gameVersion { get; set; }

        public string backupCgPath
        {
            get
            {
                return Path.Combine("Backup", "Cg.dll");
            }
        }

        public string backupCgGLPath
        {
            get
            {
                return Path.Combine("Backup", "CgGL.dll");
            }
        }

        public string backupCgD3D9Path
        {
            get
            {
                return Path.Combine("Backup", "CgD3D9.dll");
            }
        }

        public string backupTbbPath
        {
            get
            {
                return Path.Combine("Backup", "tbb.dll");
            }
        }

        public string backupNPSWF32Path
        {
            get
            {
                return Path.Combine("Backup", "NPSWF32.dll");
            }
        }

        public string backupAdobeAirPath
        {
            get
            {
                return Path.Combine("Backup", "Adobe AIR.dll");
            }
        }

        public string lolGameClientSlnPath
        {
            get
            {
                return Path.Combine("RADS", "solutions", "lol_game_client_sln", "releases") + @"\" + 
                    new DirectoryInfo(Path.Combine("RADS", "solutions", "lol_game_client_sln", "releases"))
                    .GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\";
            }
        }

        public string lolAirClientPath
        {
            get
            {
                return Path.Combine("RADS", "projects", "lol_air_client", "releases") + @"\" + 
                    new DirectoryInfo(Path.Combine("RADS", "projects", "lol_air_client", "releases"))
                    .GetDirectories().OrderByDescending(d => d.CreationTime).FirstOrDefault() + @"\";
            }
        }

        public string deployCgPath
        {
            get
            {
                return Path.Combine(gameVersion, "Cg.dll");
            }
        }

        public string deployCgGLPath
        {
            get
            {
                return Path.Combine(gameVersion, "CgGL.dll");
            }
        }

        public string deployCgD3D9Path
        {
            get
            {
                return Path.Combine(gameVersion, "CgD3D9.dll");
            }
        }

        public string deployTbbPath
        {
            get
            {
                return Path.Combine(gameVersion, "Cg.dll");
            }
        }

        public string deployAdobeAirPath
        {
            get
            {
                if (gameVersion.Equals("RADS"))
                    return Path.Combine("deploy", "Adobe Air", "Versions", "1.0", "Adobe AIR.dll");
                else
                    return Path.Combine("Air", "Adobe AIR", "Versions", "1.0", "Adobe AIR.dll");
            }
        }

        public string deployNPSWF32Path
        {
            get
            {
                if (gameVersion.Equals("RADS"))
                    return Path.Combine("deploy", "Adobe Air", "Versions", "1.0", "Resources", "NPSWF32.dll");
                else
                    return Path.Combine("Air", "Adobe AIR", "Versions", "1.0", "Resources", "NPSWF32.dll");
            }
        }

    }
}
