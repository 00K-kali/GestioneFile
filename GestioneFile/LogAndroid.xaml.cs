using Microsoft.Maui.Controls;

namespace GestioneFile;

public partial class LogAndroid : ContentPage
{
	string logfile;
	string path;
	public LogAndroid(string logfilename, string path)
	{
		InitializeComponent();
		this.logfile = Path.Combine(FileSystem.AppDataDirectory, logfilename);
		this.path = path;
		LBL_Path.Text = this.path;
		LoadList();
	}

	void LoadList()
	{
		List<DatiLog> log = new List<DatiLog>();
		string line;
        FileStream file = new FileStream(logfile, FileMode.Open, FileAccess.Read);
        using (StreamReader rt = new StreamReader(file))
		{
			rt.ReadLine();
			while(!rt.EndOfStream)
			{
				line = rt.ReadLine();
                if(Path.GetRelativePath(path, line.Split(';')[0]) != line.Split(";")[0])
				{
                    DatiLog logline = new DatiLog();
                    logline.file = Path.GetRelativePath(path, line.Split(';')[0]);
                    logline.data_creazione = line.Split(';')[1];
                    logline.data_modifica = line.Split(';')[2];
                    logline.azione = line.Split(';')[3];
                    logline.data = line.Split(";")[4];
                    log.Add(logline);
                }
            }
            file.Position = 0;
        }
        CLT_Log.ItemsSource = log;
    }

    private async void CLT_Log_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if(CLT_Log.SelectedItem != null)
        {
            DatiLog selected = CLT_Log.SelectedItem as DatiLog;
            await Navigation.PushAsync(new VisualizzaLog(selected, this.path));
        }
    }
}