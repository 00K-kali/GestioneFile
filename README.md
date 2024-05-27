# Folder Sync Program

## Objectives

- Synchronize two folders, one source and one destination.
- Update the destination folder periodically after a user-defined delay.
- Check the modification and creation dates of files to update the destination folder if they fall within the specified time range.

## Features

- Option to display changes in a list without actually updating the destination folder.
- "Start" and "Stop" buttons to control the running state.
- Source and destination paths selectable from a list saved in a .txt file, updatable via the interface.
- Non-blocking wait for the interface, divided into 5-second segments.
- Detection of files modified or created within the specified time range.
- Logging operations in a .csv file viewable with Excel.
- Automatic creation of the log file with column headers on the first run.
- Synchronization of files regardless of creation date if manually inserted.
- Management of renamed files by comparing file names in source and destination folders.
- Use of a text file as a database to avoid read overload on the destination folder.

## Known Issues and Solutions

- **Detecting Renamed Files**: The program now deletes the file from the destination folder and recopies it from the source folder.
- **Non-Functioning ProgressBar**: Fixed, now updates correctly after each partial wait.
- **Restarting the Cycle on "Start" Button Press During Execution**: Fixed, checks the "run" variable status before starting a new cycle.

## Possible Improvements

- Upload files to cloud services (e.g., OneDrive, Google Drive, Dropbox).
- Integration of a cloud service login page.
- Implementation of a local database-based system to reduce server workload on cloud services.
- Initial and minimum window sizes to ensure interface readability.
- Addition of a settings page to configure settings before starting.

## Operation

The program is developed using the .NET MAUI framework to support cross-platform programming with unified C# code. Due to screen size differences across operating systems, platform-specific XAML files have been created while the C# code remains identical.

### Main Screen

In the main interface, you can:

- Manage the system synchronization status using the "Start" and "Stop" buttons.
- Access the settings screen by clicking on the gear icon.

### Settings Screen

Allows you to:

- Set a source path.
- Enable or disable copying to the destination folder.
- Set a destination path.
- Enable or disable copying all files on the first sync.
- Set the delay for synchronization.

### Running Main Screen

When you click the "Start" button:

- The program loads settings and starts synchronization.
- On the first run, all files are copied from the source to the destination folder.
- From the second synchronization onwards, only files modified or created within the specified time range are copied.

### Log Screen

Allows you to view the log of operations performed by the program.

## Code Structure

### Main Classes

- **MainPage**: Main page class, contains functions for user interaction.
- **Settings**: Settings screen class.
- **Log**: Class for managing the log view window.
- **ViewLog**: Class for detailed viewing of a selected log.
- **FileData**: Class containing details of new files found in the source folder.
- **LogData**: Class for creating templates for each row of the log page.
- **Settings**: Class containing system synchronization settings.
- **Sync**: Class containing attributes and methods for folder synchronization.

### Synchronization

The **Sync** class manages the synchronization cycle:

- **Start()**: Main function that starts the synchronization cycle.
- **Filter()**: Recursive function that filters files based on creation and modification dates.
- **FileList()**: Function to create a list of files in the folders.

## Execution

1. **Program Start**: Configure initial settings via the settings screen.
2. **Synchronization**: Press "Start" to begin the synchronization cycle.
3. **Log Viewing**: Use the log screen to view operations performed.

---

We hope this documentation is helpful. For further questions or contributions, please feel free to contact us.

Happy syncing!
