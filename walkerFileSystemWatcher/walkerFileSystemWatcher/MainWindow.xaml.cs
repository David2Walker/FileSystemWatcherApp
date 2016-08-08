using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Data.SQLite;
using System.IO;
using System.Windows.Threading;
using System.Timers;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace walkerFileSystemWatcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private delegate void UpdateGridDelegate(DataGrid arg);

        private List<watchedObject> watchedFiles;
        private static SQLiteConnection db;
        private static FileSystemWatcher watcher;
        private static string[] allowedExtensions = { ".txt", ".html", ".css", ".js", ".php", ".exe", ".java", ".cs", ".cpp", ".c" };
        private string extension;
        private string watchpath;
        private bool changed;
        private int count = 0;
        private Timer alertTimer;
        private bool sendEmail;
        private string email;

        public MainWindow()
        {
            InitializeComponent();

            Database();

            statusLabel.Content = "Loading data from DB";

            //loadDB();
            watchedFiles = new List<watchedObject>();

            extensionBox.ItemsSource = allowedExtensions;
            dataGrid.AutoGenerateColumns = false;
            dataGrid.ItemsSource = watchedFiles;
            dataGrid.Items.Refresh();

            statusLabel.Content = "Waiting for start...";

            startButton.IsEnabled = true;
            stopButton.IsEnabled = false;
            watchdirBox.IsEnabled = true;
            extensionBox.IsEnabled = true;
            cleardbMenuButton.IsEnabled = false;
            startMenuButton.IsEnabled = true;
            stopMenuButton.IsEnabled = false;
            emailBox.IsEnabled = false;
            emailLabel.IsEnabled = false;

            alertTimer = new Timer();
            alertTimer.Elapsed += new ElapsedEventHandler(sendAlert);
            alertTimer.Interval = 30000; //300000 = 5 minutes, 60000 = 1 minute
            alertTimer.Enabled = false;
            alertTimer.AutoReset = false;

        }

        private void loadDB()
        {
            dataGrid.Items.Clear();

            String sql = "Select * from watched";
            SQLiteCommand cmd = new SQLiteCommand(sql, db);
            SQLiteDataReader reader = cmd.ExecuteReader();

            watchedFiles = new List<watchedObject>();

            while (reader.Read())
            {
                String watching = reader.GetString(0);
                String userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                String name = reader.GetString(1);
                String path = reader.GetString(2);
                String fileevent = reader.GetString(3);
                String date = reader.GetString(4);
                String time = reader.GetString(5);

                watchedObject watched = new watchedObject(watching, userName, name, path, fileevent, date, time);

                watchedFiles.Add(watched);

            }

            dataGrid.AutoGenerateColumns = false;

            dataGrid.ItemsSource = watchedFiles;

            dataGrid.Items.Refresh();
        }

        private static void writeToDB(watchedObject watched)
        {
            SQLiteCommand cmd = new SQLiteCommand(db);
            cmd.CommandText = "INSERT INTO watched (watching, username, name, path, event, date, time) VALUES (@watching, @username, @name, @path, @event, @date, @time)";
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Parameters.Add(new SQLiteParameter("@watching", watched.watchingPath));
            cmd.Parameters.Add(new SQLiteParameter("@username", watched.userName));
            cmd.Parameters.Add(new SQLiteParameter("@name", watched.fileName));
            cmd.Parameters.Add(new SQLiteParameter("@path", watched.filePath));
            cmd.Parameters.Add(new SQLiteParameter("@event", watched.fileEvent));
            cmd.Parameters.Add(new SQLiteParameter("@date", watched.date));
            cmd.Parameters.Add(new SQLiteParameter("@time", watched.time));
            cmd.ExecuteNonQuery();
        }

        private static void Database()
        {
            if (!File.Exists("FileWatcher.sqlite"))
            {
                SQLiteConnection.CreateFile("FileWatcher.sqlite");
                db = new SQLiteConnection("Data Source=FileWatcher.sqlite;Version=3;");
                db.Open();

                String sql = "CREATE TABLE watched (watching VARCHAR(200), username VARCHAR(100), name VARCHAR(20), path VARCHAR(100), event VARCHAR(10), date DATE, time DATETIME)";
                SQLiteCommand cmd = new SQLiteCommand(sql, db);
                cmd.ExecuteNonQuery();
            }
            else
            {
                db = new SQLiteConnection("Data Source=FileWatcher.sqlite;Version=3;");
                db.Open();
            }
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            statusLabel.Content = "Preparing to watch...";

            if (extensionBox.Text != "")
            {
                extension = extensionBox.Text;
                if(extension.IndexOf('.')!=0)
                {
                    //invalid extension
                    extension = "-1";
                }
                else
                {
                    int count = 0;
                    foreach (char c in extension)
                        if (c == '.')
                            count++;

                    if(count > 1)
                    {
                        extension = "-1";
                    }
                    else if(count == 1)
                    {
                        extension = extensionBox.Text;
                    }
                    else
                    {
                        extension = "-1";
                    }
                }
            }
            else
            {
                extension = "-1";
                //MessageBox.Show("no extension");
            }

            watchpath = watchdirBox.Text;
            if (!Directory.Exists(watchpath))
            {
                statusLabel.Content = "Invalid Path...";
                MessageBox.Show("Must enter a valid path");

                startButton.IsEnabled = true;
                startMenuButton.IsEnabled = true;
                watchdirBox.IsEnabled = true;
                extensionBox.IsEnabled = true;
                stopButton.IsEnabled = false;
                stopMenuButton.IsEnabled = false;
                emailBox.IsEnabled = true;
                emailLabel.IsEnabled = true;
                emailcheckBox.IsEnabled = true;

                return;
            }
            else
            {
                startButton.IsEnabled = false;
                startMenuButton.IsEnabled = false;
                watchdirBox.IsEnabled = false;
                extensionBox.IsEnabled = false;
                stopButton.IsEnabled = true;
                stopMenuButton.IsEnabled = true;
                cleardbMenuButton.IsEnabled = false;
                emailBox.IsEnabled = false;
                emailLabel.IsEnabled = false;
                emailcheckBox.IsEnabled = false;

                Watch();
            }
        }

        private void Watch()
        {
            watcher = new FileSystemWatcher();

            //path validation
            if (!Directory.Exists(watchpath))
            {
                statusLabel.Content = "Invalid Path...";
                MessageBox.Show("Must enter a valid path");
                return;
            }

            String watchext = " for ";
            if (!extension.Equals("-1"))
            {
                watcher.Filter = "*" + extension;
                statusLabel.Content = "Watching..." + watchext + extension;
            }
            else
            {
                statusLabel.Content = "Watching...";
            }

            watcher.Path = watchpath;

            watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName
                | NotifyFilters.DirectoryName;

            watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.Created += new FileSystemEventHandler(OnChanged);
            watcher.Deleted += new FileSystemEventHandler(OnChanged);
            watcher.Renamed += new RenamedEventHandler(OnRenamed);

            watcher.EnableRaisingEvents = true;
        }

        private void OnChanged(object source, FileSystemEventArgs args)
        {
            //write to log
            String path = args.FullPath;
            String userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            String name = args.Name;
            String type = args.ChangeType.ToString();
            DateTime date = System.DateTime.Now;

            //check path for db
            if (name.Equals("FileWatcher.sqlite") || name.Equals("FileWatcher.sqlite-journal"))
            {
                return;
            }

            TimeSpan time = date.TimeOfDay;

            watchedObject watched = new watchedObject(watchpath, userName, name, path, type, date.ToShortDateString(), time.ToString());
            watchedFiles.Add(watched);
            writeToDB(watched);

            refreshTable();

            changed = true;
            count++;

            if (sendEmail)
            {
                if (alertTimer.Enabled)
                {
                    alertTimer.Stop();
                    alertTimer.Start();
                }
                else
                {
                    alertTimer.Start();
                }
            }
        }

        private void OnRenamed(object source, FileSystemEventArgs args)
        {
            //write to log
            String path = args.FullPath;
            String userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            String name = args.Name;
            String type = args.ChangeType.ToString();
            DateTime date = System.DateTime.Now;

            TimeSpan time = date.TimeOfDay;

            //watchedObject(string watching, string name, string path, string fileevent, string date, string time)
            watchedObject watched = new watchedObject(watchpath, userName, name, path, type, date.ToShortDateString(), time.ToString());

            watchedFiles.Add(watched);
            writeToDB(watched);

            refreshTable();

            changed = true;
            count++;

            if (sendEmail)
            {
                if (alertTimer.Enabled)
                {
                    alertTimer.Stop();
                    alertTimer.Start();
                }
                else
                {
                    alertTimer.Start();
                }
            }
        }

        private void sendAlert(object sender, ElapsedEventArgs e)
        {
            //send alert
            try
            {
                var client = new SmtpClient("siteground338.com", 2525)
                {
                    Credentials = new NetworkCredential("filealerter@davidw.us", sendTo),
                    EnableSsl = true
                };
                ServicePointManager.ServerCertificateValidationCallback += (o, c, ch, er) => true;
                String subject = "File changes detected!";
                String msgcontent = "";

                foreach (watchedObject watched in watchedFiles)
                {
                    msgcontent += watched.ToString() + "\n";
                }

                MailMessage message = new MailMessage("filealerter@davidw.us", email, subject, msgcontent);

                client.Send(message);
            }
            catch (Exception)
            {
                MessageBox.Show("error");
            }
        }

        private void aboutMenuButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Author: David Walker \n.NET Framework: 4.5.2 x86");
        }

        private static string sendTo = "dwemPW4fa";
        private void stopButton_Click(object sender, RoutedEventArgs e)
        {
            if (changed == true)
            {
                MessageBox.Show("No longer watching \n" + count + " changes were detected");
            }

            watcher.EnableRaisingEvents = false;
            watchdirBox.IsEnabled = true;
            extensionBox.IsEnabled = true;
            startButton.IsEnabled = true;
            stopButton.IsEnabled = false;
            startMenuButton.IsEnabled = true;
            stopMenuButton.IsEnabled = false;
            cleardbMenuButton.IsEnabled = true;
            emailcheckBox.IsEnabled = true;
        }

        private void cleardbButton_Click(object sender, RoutedEventArgs e)
        {
            if(MessageBox.Show("This will erase all data from the databse \n Are you sure?", "Clear Database", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                //clear db
                SQLiteCommand cmd = new SQLiteCommand(db);
                cmd.CommandText = "DELETE FROM watched";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.ExecuteNonQuery();
                MessageBox.Show("Database cleared");

                watchedFiles.Clear();

                refreshTable();
            }
        }

        private void queryButton_Click(object sender, RoutedEventArgs e)
        {
            Window1 queryWindow = new Window1();
            queryWindow.Show();
        }

        private void emailcheckBox_Checked(object sender, RoutedEventArgs e)
        {
            emailBox.IsEnabled = true;
            emailLabel.IsEnabled = true;
            emailBox.Focus();
        }

        private void emailcheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            sendEmail = false;
            emailBox.IsEnabled = false;
            emailLabel.IsEnabled = false;
        }

        private void emailBox_LostFocus(object sender, RoutedEventArgs e)
        {
            email = emailBox.Text;

            //http://stackoverflow.com/questions/33882173/email-address-input-validation
            Regex regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");

            if(regex.IsMatch(email))
            {
                sendEmail = true;
            }
            else
            {
                MessageBox.Show("Invalid email address");
                sendEmail = false;
                emailBox.Text = "";
                emailBox.IsEnabled = false;
                emailLabel.IsEnabled = false;
                emailcheckBox.IsChecked = false;
            }
        }

        private void refreshTable()
        {
            if (dataGrid.Dispatcher.CheckAccess())
            {
                dataGrid.Items.Refresh();
            }
            else
            {
                dataGrid.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new UpdateGridDelegate(UpdateGrid), dataGrid);
            }
        }

        private void UpdateGrid(Object datagrid)
        {
            DataGrid dg = (DataGrid)datagrid;

            dg.Items.Refresh();
        }
    }
}
