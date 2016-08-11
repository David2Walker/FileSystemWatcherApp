using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.SQLite;
using System.Windows.Threading;

namespace walkerFileSystemWatcher
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private static string[] allowedExtensions = { ".txt", ".html", ".css", ".js", ".php", ".exe", ".java", ".cs", ".cpp", ".c" };
        private List<watchedObject> watchedFiles;
        private static SQLiteConnection db;
        private string extension;
        private delegate void UpdateGridDelegate(DataGrid arg);

        public Window1()
        {
            InitializeComponent();

            Database();
            
            //loadDB();
            watchedFiles = new List<watchedObject>();

            queryforBox.ItemsSource = allowedExtensions;
            queryGrid.AutoGenerateColumns = false;
            queryGrid.ItemsSource = watchedFiles;
            queryGrid.Items.Refresh();

        }

        private void queryButton_Click(object sender, RoutedEventArgs e)
        {
            if (queryforBox.Text == "")
            {
                loadDB("");
            }
            else
            {
                extension = queryforBox.Text;

                if (extension != "")
                {
                    int count = 0;
                    foreach (char c in extension)
                        if (c == '.')
                            count++;

                    if (count > 1)
                    {
                        //more than 1 . - invalid, just load the whole DB
                        loadDB("");
                    }
                    else if (count == 1 && extension.IndexOf('.') == 0)
                    {
                        // 1 . and is first character - basic extension search
                        loadDB("ext");

                    }
                    else if (count == 1 && extension.IndexOf('.') != 0)
                    {
                        // 1 . and is not first character - name + extension search
                        loadDB("full");
                    }
                    else if (count == 0)
                    {
                        //filename only search - no extension
                        loadDB("name");
                    }
                }
                else
                {
                    //empty search box
                    loadDB("");
                }
            }
        }

        private void loadDB(String qfor)
        {
            if(queryGrid.Items.Count != 0)
            {
                queryGrid.ItemsSource = null;
                watchedFiles.Clear();
                refreshTable();
            }

            SQLiteCommand cmd = new SQLiteCommand(db);

            if (qfor.Equals(""))
            {
                cmd.CommandText = "SELECT * FROM watched";
            }
            else if(qfor.Equals("ext"))
            {
                cmd.CommandText = "SELECT * FROM watched WHERE name LIKE @extension";
                cmd.Parameters.Add(new SQLiteParameter("@extension", "%" + extension));
            }
            else if(qfor.Equals("name"))
            {
                cmd.CommandText = "SELECT * FROM watched WHERE name LIKE @name";
                cmd.Parameters.Add(new SQLiteParameter("@name", extension + ".%"));
            }
            else if(qfor.Equals("full"))
            {
                cmd.CommandText = "SELECT * FROM watched WHERE name LIKE @full";
                cmd.Parameters.Add(new SQLiteParameter("@full", extension));
            }
            else
            { 
                cmd.CommandText = "SELECT * FROM watched";
            }

            cmd.CommandType = System.Data.CommandType.Text;
            SQLiteDataReader reader = cmd.ExecuteReader();

            //watching VARCHAR(200), userName VARCHAR(??),name VARCHAR(20), path VARCHAR(100), event VARCHAR(10), date DATE, time DATETIME

            while (reader.Read())
            {
                String watching = reader.GetString(0);
                String userName = reader.GetString(1);
                String name = reader.GetString(2);
                String path = reader.GetString(3);
                String fileevent = reader.GetString(4);
                String date = reader.GetString(5);
                String time = reader.GetString(6);

                watchedObject watched = new watchedObject(watching, userName, name, path, fileevent, date, time);

                watchedFiles.Add(watched);

            }

            queryGrid.AutoGenerateColumns = false;

            queryGrid.ItemsSource = watchedFiles;

            refreshTable();
        }

        private static void Database()
        {
            if (!File.Exists("FileWatcher.sqlite"))
            {
                //MessageBox.Show("FileWatcher.sqlite doesnt exists");
                SQLiteConnection.CreateFile("FileWatcher.sqlite");
                db = new SQLiteConnection("Data Source=FileWatcher.sqlite;Version=3;");
                db.Open();
                String sql = "CREATE TABLE watched (watching VARCHAR(200), username VARCHAR(100), name VARCHAR(20), path VARCHAR(100), event VARCHAR(10), date DATE, time DATETIME)";
                SQLiteCommand cmd = new SQLiteCommand(sql, db);
                cmd.ExecuteNonQuery();
            }
            else
            {
                //MessageBox.Show("FileWatcher.sqlite exists");
                db = new SQLiteConnection("Data Source=FileWatcher.sqlite;Version=3;");
                db.Open();
            }
        }

        private void refreshTable()
        {
            if (queryGrid.Dispatcher.CheckAccess())
            {
                queryGrid.Items.Refresh();
            }
            else
            {
                queryGrid.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new UpdateGridDelegate(UpdateGrid), queryGrid);
            }
        }

        private void UpdateGrid(Object datagrid)
        {
            DataGrid dg = (DataGrid)datagrid;
            dg.Items.Refresh();
        }
    }
}
