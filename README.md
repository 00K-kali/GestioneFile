Overview
This tool synchronizes two folders: a source folder and a destination folder. The synchronization process runs periodically, based on a user-defined delay. The tool monitors file modifications and creations within a specified time range, updating the destination folder accordingly.

Objectives
Synchronize two folders: a source and a destination.
Periodically update the destination folder based on a user-defined delay.
Check file modification and creation dates, and update the destination folder if they fall within the specified time range.
Features
Synchronization Mode: Users can choose to either update the destination folder or just display the changes in a list.
Control Buttons: "Start" and "Stop" buttons manage the synchronization process.
Folder Selection: Users can select source and destination paths from a pre-saved list, which can be updated via the program interface.
Non-blocking Wait: The delay is broken into 5-second intervals, allowing the user to stop the cycle without waiting for the total delay.
File Tracking: The program detects if a file has been modified or created within the time range.
Logging: All operations are logged in a .csv file, viewable with Excel. If the log file does not exist, it is created with column headers.
Initial Sync: The first synchronization copies all files from the source to the destination, regardless of the delay filter.
Renamed File Handling: The program handles renamed files by comparing file names between the source and destination folders and removing files from the destination if they don't exist in the source.
Local Database: A text file acts as a database for the list of files in the destination folder, reducing the load if the destination is not local.
Known Issues and Resolutions
Renamed Files: Previously, the program did not detect renamed files. This has been resolved by deleting the old file in the destination and copying the renamed file from the source.
ProgressBar Functionality: Initially, the ProgressBar did not update correctly. This has been fixed to increment every 5 seconds and reset after each cycle.
Restarting on "Start" Button Press: The program used to restart the synchronization cycle if "Start" was pressed while already running. This has been resolved by checking the "run" variable to detect if synchronization is currently active.
Future Improvements
Cloud Integration: Add the ability to sync files with cloud services (OneDrive, Google Drive, Dropbox). Incorporate API for authentication and file operations.
Enhanced Interface: Incorporate a login page for cloud services within the app, supporting various authentication methods (Two-Factor, Authenticator App).
Database-based Sync: Implement a local database system for storing the list of files in the destination folder to minimize server load.
User Interface Enhancements: Set an initial window size and minimum dimensions for better readability.
Settings Page: Add a settings page to configure paths, delay, copy function, delete function, and initial full sync option.
Usage
Main Screen
Start/Stop: Control synchronization with "Start" and "Stop" buttons.
Settings: Access the settings screen via the gear icon.
Settings Screen
Source Path: Select from pre-saved paths or add new ones.
Destination Path: Enable/disable and select from pre-saved paths or add new ones.
Delay: Set synchronization delay (days, hours, minutes, seconds).
Additional Options: Enable/disable full initial sync and other settings.
Save: Save settings and return to the main screen.
Synchronization in Progress
The program loads settings and begins the synchronization process.
On first run, copies all files from the source to the destination if enabled.
Subsequent synchronizations only copy files modified or created within the delay period.
The ProgressBar updates every 5 seconds.
The synchronization continues indefinitely until stopped by the user.
Technical Details
.NET MAUI Framework
The program uses .NET MAUI for cross-platform development, ensuring consistent functionality across different operating systems with adaptive UI layouts.
Main Components
MainPage: Initial screen with control buttons.
SettingsPage: Screen for configuring paths and delay.
LogPage: View the log of operations.
Data Models: Classes for handling file data and log entries.
Sync Class: Core functionality for synchronization logic and file operations.
Key Functions
UpdatePathsPicker: Loads paths into pickers.
UpdatePaths: Adds new paths and updates pickers.
LoadSettings: Loads current settings into the interface.
SyncCopia_DestPaths: Syncs the state of copy-related controls.
Start: Initiates the synchronization cycle.
Filter: Filters files based on modification and creation dates.
FileList: Generates a list of files in a directory and its subdirectories.
This tool ensures efficient and flexible folder synchronization, with robust handling of file changes and user-friendly controls for a seamless experience.
