using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestioneFile
{
    class Sync
    {
        Label LBL_NumeroFile;
        ListView LST_Output;
        ProgressBar PGB_Timer;
        Settings settings;
        string dbfilename;
        string logfilename;
        string logtitles;
        bool firstrun;

        public bool run { get; set; }

        public Sync(bool run, Settings settings, Label LBL_NumeroFile, ListView LST_Output, ProgressBar PGB_Timer, string dbfilename, string logfilename, string logtitles)
        {
            this.run = run;
            this.settings = settings;
            this.LBL_NumeroFile = LBL_NumeroFile;
            this.LST_Output = LST_Output;
            this.PGB_Timer = PGB_Timer;
            this.dbfilename = dbfilename;
            this.logfilename = logfilename;
            this.logtitles = logtitles;
            this.firstrun = settings.firstrun;
        }

        public async void Start()
        {
            while (run)
            {
                DateTime filtro;
                if (firstrun)
                {
                    filtro = DateTime.MinValue;
                }
                else
                {
                    filtro = DateTime.Now - settings.delay;
                }

                if (!File.Exists(Path.Combine(FileSystem.AppDataDirectory, logfilename)))
                    File.AppendAllText(Path.Combine(FileSystem.AppDataDirectory, logfilename), logtitles + Environment.NewLine);

                List<DatiFile> lista_filtrati = new List<DatiFile>();
                lista_filtrati = Filter(settings.path, filtro, lista_filtrati);

                if (settings.copy)
                {
                    List<string> file_origine = new List<string>();
                    List<string> file_destinazione = new List<string>();
                    file_origine = FileList(settings.path, file_origine, settings.path);
                    using (FileStream db = new FileStream(Path.Combine(FileSystem.AppDataDirectory, dbfilename), FileMode.OpenOrCreate, FileAccess.Read))
                    {
                        using (StreamReader rt = new StreamReader(db))
                        {
                            foreach (string file in rt.ReadToEnd().Split('\n').ToList())
                                if (file.TrimEnd('\r') != "")
                                    file_destinazione.Add(Path.GetRelativePath(settings.destpath, file.TrimEnd('\r')));
                        };
                    };
                    LogDeleted(file_destinazione.Except(file_origine).ToList());
                    LogRenamed(file_origine.Except(file_destinazione).ToList());
                }
                

                LST_Output.ItemsSource = lista_filtrati;
                LBL_NumeroFile.Text = "Numero file: " + lista_filtrati.Count.ToString();

                firstrun = false;

                int n_semidelay = (int)Math.Round(settings.delay.TotalSeconds / 5.0);
                int i = 0;
                while (i < n_semidelay && run)
                {
                    PGB_Timer.Progress = i / (double)(n_semidelay);
                    await Task.Delay(TimeSpan.FromSeconds(5));
                    i++;
                }
            }
            if (!run)
            {
                LST_Output.ItemsSource = null;
                LBL_NumeroFile.Text = "Numero file: ";
            }
        }

        List<DatiFile> Filter(string path, DateTime filtro, List<DatiFile> lista_filtrati)
        {
            string logline;
            foreach (string file in Directory.GetFiles(path))
            {
                if (File.GetLastWriteTime(file) >= filtro || File.GetCreationTime(file) >= filtro)
                {
                    DatiFile dati = new DatiFile();
                    dati.nome = Path.GetRelativePath(settings.path, Path.GetFullPath(file));
                    dati.data_creazione = File.GetCreationTime(file).Date.ToString("dd/MM/yyyy", CultureInfo.CreateSpecificCulture("it-IT")) + " " + (File.GetCreationTime(file).Date + File.GetCreationTime(file).TimeOfDay).ToString("HH:mm:ss");
                    dati.data_modifica = File.GetLastWriteTime(file).Date.ToString("dd/MM/yyyy", CultureInfo.CreateSpecificCulture("it-IT")) + " " + (File.GetLastWriteTime(file).Date + File.GetLastWriteTime(file).TimeOfDay).ToString("HH:mm:ss");
                    dati.data_completo = $"Data creazione: {dati.data_creazione} - Data ultima modifica: {dati.data_modifica}";
                    lista_filtrati.Add(dati);
                    logline = $"{Path.GetFullPath(file)};{dati.data_creazione};{dati.data_modifica};File modificato;{NowFormatted()}";

                    if (File.GetCreationTime(file) >= filtro)
                    {
                        logline = $"{Path.GetFullPath(file)};{dati.data_creazione};{dati.data_modifica};File creato;{NowFormatted()}";
                    }

                    if (settings.copy)
                    {
                        Directory.CreateDirectory(Path.Combine(settings.destpath, Path.GetDirectoryName(Path.GetRelativePath(settings.path, Path.GetFullPath(file)))));
                        File.Copy(file, Path.Combine(settings.destpath, Path.GetRelativePath(settings.path, Path.GetFullPath(file))), true);
                        AddToDB(Path.Combine(settings.destpath, Path.GetRelativePath(settings.path, Path.GetFullPath(file))));
                    }
                    File.AppendAllText(Path.Combine(FileSystem.AppDataDirectory, logfilename), logline + Environment.NewLine);
                }
            }
            foreach (string folder in Directory.GetDirectories(path))
            {
                lista_filtrati = Filter(folder, filtro, lista_filtrati);
            }
            return lista_filtrati;
        }

        List<string> FileList(string path, List<string> files, string rootpath)
        {
            foreach (string file in Directory.GetFiles(path))
            {
                string nome = Path.GetRelativePath(rootpath, Path.GetFullPath(file));
                files.Add(nome);
            }
            foreach (string folder in Directory.GetDirectories(path))
            {
                files = FileList(folder, files, rootpath);
            }
            return files;
        }

        void LogDeleted(List<string> file_eliminati)
        {
            string logline;
            foreach (string file in file_eliminati)
            {
                logline = $"{Path.Combine(settings.path, file)};---------;---------;File eliminato;{NowFormatted()}";
                File.Delete(Path.Combine(settings.destpath, file));
                DeleteFromDB(Path.Combine(settings.destpath, file));
                File.AppendAllText(Path.Combine(FileSystem.AppDataDirectory, logfilename), logline + Environment.NewLine);
            }
        }

        void LogRenamed(List<string> file_rinominati)
        {
            string logline;
            foreach (string file in file_rinominati)
            {
                logline = $"{Path.Combine(settings.path, file)};{File.GetCreationTime(Path.Combine(settings.path, file))};{File.GetLastWriteTime(Path.Combine(settings.path, file))};File creato;{NowFormatted()}";
                File.Copy(Path.Combine(settings.path, file), Path.Combine(settings.destpath, file));
                AddToDB(Path.Combine(settings.destpath, file));
                File.AppendAllText(Path.Combine(FileSystem.AppDataDirectory, logfilename), logline + Environment.NewLine);
            }
        }

        void AddToDB(string file)
        {
            bool flag = false;
            using (FileStream db = new FileStream(Path.Combine(FileSystem.AppDataDirectory, dbfilename), FileMode.OpenOrCreate, FileAccess.Read))
            {
                using (StreamReader rt = new StreamReader(db))
                {
                    while (!rt.EndOfStream && !flag)
                    {
                        if (rt.ReadLine() == file)
                        {
                            flag = true;
                        }
                    }
                    db.Position = 0;
                    rt.Close();
                };
                db.Close();
            };

            if (!flag)
            {
                File.AppendAllText(Path.Combine(Path.Combine(FileSystem.AppDataDirectory, dbfilename)), file + Environment.NewLine);
            }
        }

        void DeleteFromDB(string file)
        {
            bool flag = false;
            using (FileStream db = new FileStream(Path.Combine(FileSystem.AppDataDirectory, dbfilename), FileMode.OpenOrCreate, FileAccess.Read))
            {
                using (StreamReader rt = new StreamReader(db))
                {
                    while (!rt.EndOfStream && !flag)
                    {
                        if (rt.ReadLine() == file)
                        {
                            flag = true;
                        }
                    }
                    db.Position = 0;
                    rt.Close();
                };
            };
            if (flag)
            {
                List<string> filedb = filedb = File.ReadAllLines(Path.Combine(FileSystem.AppDataDirectory, dbfilename)).ToList();
                if (File.Exists(Path.Combine(FileSystem.AppDataDirectory, dbfilename)))
                    File.Delete(Path.Combine(FileSystem.AppDataDirectory, dbfilename));
                foreach (string line in filedb)
                {
                    if (line.TrimEnd('\r') != file)
                        File.AppendAllText(Path.Combine(FileSystem.AppDataDirectory, dbfilename), line + Environment.NewLine);
                }
            }
        }

        string NowFormatted()
        {
            return DateTime.Now.Date.ToString("dd/MM/yyyy", CultureInfo.CreateSpecificCulture("it-IT")) + " " + DateTime.Now.ToString("HH:mm:ss");
        }
    }
}