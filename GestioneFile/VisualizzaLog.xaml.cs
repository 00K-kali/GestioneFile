namespace GestioneFile;

public partial class VisualizzaLog : ContentPage
{
	DatiLog log;
	string path;
	public VisualizzaLog(DatiLog log, string path)
	{
		InitializeComponent();
		this.log = log;
		this.path = path;
		Load();
	}

	void Load()
	{
		LBL_File.Text = Path.Combine(path, log.file);
		LBL_Creazione.Text = log.data_creazione;
		LBL_Modifica.Text = log.data_modifica;
		LBL_Evento.Text = log.azione;
		LBL_DataEvento.Text = log.data;
	}

}