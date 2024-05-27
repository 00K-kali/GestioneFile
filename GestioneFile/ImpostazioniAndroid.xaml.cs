namespace GestioneFile;

public partial class ImpostazioniAndroid : ContentPage
{
    string pathsfilename;
    string destpathsfilename;
    Settings settings;
    public ImpostazioniAndroid(string pathsfilename, string destpathsfilename, Settings settings)
    {
        InitializeComponent();
        this.pathsfilename = pathsfilename;
        this.destpathsfilename = destpathsfilename;
        this.settings = settings;
        UpdatePathsPicker(Path.Combine(FileSystem.AppDataDirectory, this.pathsfilename), PCK_Paths);
        UpdatePathsPicker(Path.Combine(FileSystem.AppDataDirectory, this.destpathsfilename), PCK_DestPaths);
        LoadSettings();
        SyncCopia_DestPaths();
    }

    void LoadSettings()
    {
        if (settings.path != null)
        {
            PCK_Paths.SelectedItem = settings.path;
            TXT_Giorni.Text = settings.delay.Days.ToString();
            TXT_Ore.Text = settings.delay.Hours.ToString();
            TXT_Minuti.Text = settings.delay.Minutes.ToString();
            TXT_Secondi.Text = settings.delay.Seconds.ToString();
            CHB_Copia.IsChecked = settings.copy;
            CHB_Aggiorna.IsChecked = settings.firstrun;
            PCK_DestPaths.SelectedItem = settings.destpath;
        }

    }

    private async void BUT_Salva_Clicked(object sender, EventArgs e)
    {
        ButAnimation(BUT_Salva);
        int days, hours, minutes, seconds;
        if (TXT_Giorni.Text != string.Empty)
            days = Convert.ToInt32(TXT_Giorni.Text);
        else
            days = 0;

        if (TXT_Ore.Text != string.Empty)
            hours = Convert.ToInt32(TXT_Ore.Text);
        else
            hours = 0;

        if (TXT_Minuti.Text != string.Empty)
            minutes = Convert.ToInt32(TXT_Minuti.Text);
        else
            minutes = 0;

        if (TXT_Secondi.Text != string.Empty)
            seconds = Convert.ToInt32(TXT_Secondi.Text);
        else
            seconds = 0;

        settings.delay = new TimeSpan(days, hours, minutes, seconds);
        settings.copy = CHB_Copia.IsChecked;
        settings.firstrun = CHB_Aggiorna.IsChecked;
        try
        {
            if (settings.delay >= TimeSpan.FromSeconds(10))
            {
                if (PCK_Paths.SelectedIndex > -1)
                {
                    if (!(settings.copy && PCK_DestPaths.SelectedIndex <= -1))
                    {
                        settings.path = PCK_Paths.SelectedItem.ToString();
                        if (settings.copy)
                        {
                            settings.destpath = PCK_DestPaths.SelectedItem.ToString();
                        }
                        else
                            settings.destpath = null;
                    }
                    else
                    {
                        await DisplayAlert("Errore", "Selezionare un percorso di destinazione o disattivare la funzione Copia", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Errore", "Selezionare un percorso di origine", "OK");
                }
            }
            else
            {
                await DisplayAlert("Errore", "Il delay è troppo basso (minimo 10 secondi)", "OK");
            }
        }
        catch
        {
            await DisplayAlert("Errore", "Delay impostato non valido", "OK");
        }
    }

    private void CHB_Copia_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        SyncCopia_DestPaths();
    }

    private void SyncCopia_DestPaths()
    {
        PCK_DestPaths.IsEnabled = CHB_Copia.IsChecked;
    }

    private async void BUT_AddPath_Clicked(object sender, EventArgs e)
    {
        ButAnimation(BUT_AddPath);
        if (TXT_NewPath != null)
        {
            string path = TXT_NewPath.Text;
            if (Directory.Exists(path))
            {
                UpdatePaths(Path.Combine(FileSystem.AppDataDirectory, pathsfilename), path + Environment.NewLine, PCK_Paths);
            }
            else
            {
                await DisplayAlert("Errore", "Il percorso non esiste", "OK");
            }
        }
        else
        {
            await DisplayAlert("Errore", "Il percorso non può essere vuoto", "OK");
        }
    }

    private async void BUT_AddDestPath_Clicked(object sender, EventArgs e)
    {
        ButAnimation(BUT_AddDestPath);
        if (TXT_NewDestPath != null)
        {
            string path = TXT_NewDestPath.Text;
            if (Directory.Exists(path))
            {
                UpdatePaths(Path.Combine(FileSystem.AppDataDirectory, destpathsfilename), path + Environment.NewLine, PCK_DestPaths);
            }
            else
            {
                await DisplayAlert("Errore", "Il percorso non esiste", "OK");
            }
        }
        else
        {
            await DisplayAlert("Errore", "Il percorso non può essere vuoto", "OK");
        }
    }

    async void UpdatePaths(string filepath, string path, Picker PCK)
    {
        FileStream file = new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.Read);
        using (StreamReader rt = new StreamReader(file))
        {
            bool flag = false;
            while (!rt.EndOfStream && !flag)
            {
                if (rt.ReadLine() == path.TrimEnd('\n'))
                {
                    flag = true;
                }
            }
            if (!flag)
            {
                File.AppendAllText(filepath, path);
                UpdatePathsPicker(filepath, PCK);
            }
            else
            {
                await DisplayAlert("Errore", "Il percorso è già nella lista", "OK");
            }
        }
    }

    void UpdatePathsPicker(string filepath, Picker PCK)
    {
        List<string> paths = new List<string>();
        FileStream file = new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.Read);
        using (StreamReader rt = new StreamReader(file))
        {
            while (!rt.EndOfStream)
            {
                paths.Add(rt.ReadLine());
            }
            rt.Close();
            file.Close();
        }

        PCK.ItemsSource = paths;
    }

    async void ButAnimation(Button button)
    {
        await button.ScaleTo(0.8, 100, Easing.CubicInOut);
        await button.ScaleTo(1, 100, Easing.CubicInOut);
    }
}