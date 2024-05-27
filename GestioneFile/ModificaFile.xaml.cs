using Microsoft.Maui.Storage;

namespace GestioneFile;

public partial class ModificaFile : ContentPage
{
    string file;

	public ModificaFile(string path, string filename)
    {
		InitializeComponent();

        file = Path.Combine(path, filename);
        LBL_NomeFile.Text = filename;
        TXT_Testo.Text = File.ReadAllText(file);
    }

    private async void Salva_Clicked(object sender, EventArgs e)
    {
        try
        {
            if(File.Exists(file))
            {
                File.WriteAllText(file, TXT_Testo.Text);
                await DisplayAlert("Operazione completata", $"File {Path.GetFileName(file)} salvato con successo", "OK");
            }
            else
            {
                await DisplayAlert("Errore", "Il file non esiste", "OK");
            }
        }
        catch
        {
            await DisplayAlert("Errore", "Impossibile salvare il file", "OK");
        }
    }

    private async void Elimina_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (File.Exists(file))
            {
                File.Delete(file);
                await DisplayAlert("Operazione completata", $"File {Path.GetFileName(file)} eliminato con successo", "OK");
                if (DeviceInfo.Platform == DevicePlatform.Android)
                {
                    await Navigation.PushAsync(new MainAndroid());
                }
                else if (DeviceInfo.Platform == DevicePlatform.WinUI)
                {
                    await Navigation.PushAsync(new MainWinUI());
                }
            }
            else
            {
                await DisplayAlert("Errore", "Il file non esiste", "OK");
            }
        }
        catch
        {
            await DisplayAlert("Errore", "Impossibile eliminare il file", "OK");
        }
        
    }
}