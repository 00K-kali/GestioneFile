namespace GestioneFile
{
    public partial class MainWinUI : ContentPage
    {
        public MainWinUI()
        {
            InitializeComponent();
            BUT_Settings.Source = Path.Combine(FileSystem.AppDataDirectory, "settings.png");
            BUT_LogFile.Source = Path.Combine(FileSystem.AppDataDirectory, "logfile.png");
        }

        string dbfilename = "destinationfiles.txt";
        string pathsfilename = "paths.txt";
        string destpathsfilename = "destpaths.txt";
        string logfilename = "log.csv";
        string logtitles = "Percorso File;Data creazione;Data ultima modifica;Azione;Data azione";

        bool run = false;
        Sync sync;
        Settings settings = new Settings();

        private async void BUT_Settings_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ImpostazioniWinUI(pathsfilename, destpathsfilename, settings));
        }

        public async void BUT_Avvia_Clicked(object sender, EventArgs e)
        {
            if (run == false)
            {
                if (settings.path != null)
                {
                    run = true;
                    BUT_Avvia.BorderWidth = 3;
                    BUT_Stop.BorderWidth = 0;
                    BUT_Stop.TextColor = Colors.White;
                    BUT_Avvia.TextColor = Colors.Black;
                    BUT_Settings.IsEnabled = false;
                    BUT_LogFile.IsEnabled = false;

                    sync = new Sync(run, settings, LBL_NumeroFile, LST_Output, PGB_Timer, dbfilename, logfilename, logtitles);
                    sync.Start();
                }
                else
                {
                    await DisplayAlert("Errore", "Inserire le impostazioni dalla pagina Impostazioni", "OK");
                }
            }
        }

        private async void BUT_Stop_Clicked(object sender, EventArgs e)
        {
            if (run == true)
            {
                run = false;
                sync.run = false;
                await DisplayAlert("Info", "Stop in corso...", "OK");
                BUT_Avvia.BorderWidth = 0;
                BUT_Stop.BorderWidth = 3;
                BUT_Stop.TextColor = Colors.Black;
                BUT_Avvia.TextColor = Colors.White;
                PGB_Timer.Progress = 0;
                BUT_Settings.IsEnabled = true;
                BUT_LogFile.IsEnabled = true;
            }
        }

        private async void LST_Output_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            DatiFile selected = LST_Output.SelectedItem as DatiFile;
            if (Path.GetExtension(Path.Combine(settings.path, selected.nome)) == ".txt")
            {
                if (File.Exists(Path.Combine(settings.path, selected.nome)))
                {
                    await Navigation.PushAsync(new ModificaFile(settings.path, selected.nome));
                }
                else
                {
                    await DisplayAlert("Errore", "Il file non esiste", "OK");
                }
            }
        }

        private async void BUT_LogFile_Clicked(object sender, EventArgs e)
        {
            if (settings.path != null)
            {
                await Navigation.PushAsync(new LogWinUI(logfilename, settings.path));
            }
            else
            {
                await DisplayAlert("Errore", "Inserire le impostazioni dalla pagina Impostazioni", "OK");
            }
        }
    }
}