/*
 * Created by SharpDevelop.
 * User: USER
 * Date: 10/18/2018
 * Time: 14:54
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using nthareneapi;
using System.Configuration;
using System.Runtime.InteropServices;

namespace nyax_console
{
    public class Program
    {
        public string TAG;
        public event EventHandler<notificationmessageEventArgs> _notificationmessageEventname;
        public event EventHandler<progressBarNotificationEventArgs> _progressBarNotificationEventname;
        public string _apppath;
        public string _xmlpathfolder;
        public string _txtpathfolder;
        public string _xml_loga_file;
        public string _xml_loga_file_fluentsyntax;
        public string _txt_loga_file;
        public List<notificationdto> _lstnotificationdto = new List<notificationdto>();
        public string _working_db;
        copy_data_utils data_utils;

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private const int SW_MAXIMIZE = 3;

        public static void Main(string[] args)
        {
            Program _Program = new Program(); 
        }

        void UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = (Exception)e.ExceptionObject;
            Console.ForegroundColor = ConsoleColor.Red;
            this._notificationmessageEventname.Invoke(sender, new notificationmessageEventArgs(ex.Message, TAG));
            Console.ForegroundColor = ConsoleColor.Green;
        }

        void ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            Exception ex = e.Exception;
            Console.ForegroundColor = ConsoleColor.Red;
            this._notificationmessageEventname.Invoke(sender, new notificationmessageEventArgs(ex.Message, TAG));
            Console.ForegroundColor = ConsoleColor.Green;
        }

        //Event handler declaration:
        public void notificationmessageHandler(object sender, notificationmessageEventArgs args)
        {
            /* Handler logic */
            //notificationdto _notificationdto = new notificationdto();
            DateTime currentDate = DateTime.Now;
            String dateTimenow = currentDate.ToString("dd-MM-yyyy HH:mm:ss");
            String _logtext = "[ " + dateTimenow + " ]   " + args.message + "\n";

            Console.WriteLine(args.message);
            Log.Write_To_Log_File_temp_dir(new Exception(_logtext));

            //_notificationdto._notification_message = _logtext;
            //_notificationdto._created_datetime = dateTimenow;
            //_notificationdto.TAG = args.TAG;
            //_lstnotificationdto.Add(_notificationdto);
            //var _lstmsgdto = from msgdto in _lstnotificationdto
            //                 orderby msgdto._created_datetime descending
            //                 select msgdto._notification_message;
            //String[] _logflippedlines = _lstmsgdto.ToArray();
            Console.Beep();
            globalwritetoconsole(_logtext);
        }

        void globalwritetoconsole(string _stringtoprint)
        {
            try
            {
                Console.WriteLine(_stringtoprint);
                addnotification(_stringtoprint, TAG);
            }
            catch (Exception ex)
            {
                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs(ex.Message, TAG));
            }
        }

        void addnotification(string message, string TAG)
        {
            notificationdto _notificationdto = new notificationdto();
            DateTime currentDate = DateTime.Now;
            String dateTimenow = currentDate.ToString("dd-MM-yyyy HH:mm:ss");
            //String _logtext = "[ " + dateTimenow + " ]   " + message + "\n";
            _notificationdto._notification_message = message;
            _notificationdto._created_datetime = dateTimenow;
            _notificationdto.TAG = TAG;
            _lstnotificationdto.Add(_notificationdto);
            Log.Write_To_Log_File_temp_dir(new Exception(message));
        }
        public void progressbarHandler(object sender, progressBarNotificationEventArgs args)
        {
            /* Handler logic */
            DateTime currentDate = DateTime.Now;
            String dateTimenow = currentDate.Ticks.ToString();
            String _logtext = "" + currentDate + " " + "\n";
            Console.Beep();
            globalwritetoconsole(_logtext);
        }

        public Program()
        {

            TAG = this.GetType().Name;

            Console.Title = "Ntharene App Command Line Interface";

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Green;
             
            IntPtr handle = GetConsoleWindow();
            ShowWindow(handle, SW_MAXIMIZE);
             
            _notificationmessageEventname += notificationmessageHandler;
            _progressBarNotificationEventname += progressbarHandler;

            data_utils = new copy_data_utils(_notificationmessageEventname, _progressBarNotificationEventname);

            DateTime currentDate = DateTime.Now;
            String dateTimenow = currentDate.ToString("dd-MM-yyyy HH:mm:ss");

            _apppath = Application.StartupPath;
            _xmlpathfolder = _apppath + @"\xmlloga\";
            _txtpathfolder = _apppath + @"\txtloga\";
            _xml_loga_file = _xmlpathfolder + "xmllogautilz.xml";
            _xml_loga_file_fluentsyntax = _xmlpathfolder + "xmllogautilzfluentsyntax.xml";
            _txt_loga_file = _txtpathfolder + "txtlogautilz.txt";

            this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("Ntharene app command line interface started.", TAG));

            showversioninfocliutils();

            this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("Type a command to do a task. Following is a list of commands.", TAG));

            showcommands();
            checkcommands();

        }

        public void checkcommands()
        {
            string _command = Console.ReadLine();
            switch (_command)
            {
                case commandsutilsclass.connections:
                    showconnectionsinfocliutils();
                    checkcommands();
                    break;
                case commandsutilsclass.createcrop:
                    createcropcliutils();
                    listcropscliutils();
                    checkcommands();
                    break;
                case commandsutilsclass.listcrops:
                    listcropscliutils();
                    checkcommands();
                    break;
                case commandsutilsclass.createcropvariety:
                    createcropvarietycliutils();
                    listcropsvarietiescliutils();
                    checkcommands();
                    break;
                case commandsutilsclass.listcropsvarieties:
                    listcropsvarietiescliutils();
                    checkcommands();
                    break;
                case commandsutilsclass.creatediseasepest:
                    createcropdiseasecliutils();
                    listcropsdiseasescliutils();
                    checkcommands();
                    break;
                case commandsutilsclass.listdiseasespests:
                    listcropsdiseasescliutils();
                    checkcommands();
                    break;
                case commandsutilsclass.createmanufacturer:
                    createmanufacturercliutils();
                    listmanufacturerscliutils();
                    checkcommands();
                    break;
                case commandsutilsclass.listmanufacturers:
                    listmanufacturerscliutils();
                    checkcommands();
                    break;
                case commandsutilsclass.createpestinsecticide:
                    createpestinsecticidecliutils();
                    listpestsinsecticidescliutils();
                    checkcommands();
                    break;
                case commandsutilsclass.listpestsinsecticides:
                    listpestsinsecticidescliutils();
                    checkcommands();
                    break;
                case commandsutilsclass.createsetting:
                    createsettingcliutils();
                    listsettingscliutils();
                    checkcommands();
                    break;
                case commandsutilsclass.listsettings:
                    listsettingscliutils();
                    checkcommands();
                    break;
                case commandsutilsclass.createcategory:
                    createcategorycliutils();
                    listcategoriescliutils();
                    checkcommands();
                    break;
                case commandsutilsclass.listcategories:
                    listcategoriescliutils();
                    checkcommands();
                    break;
                case commandsutilsclass.listall:
                    listallcliutils();
                    checkcommands();
                    break;
                case commandsutilsclass.createdatabase:
                    createdatabasecliutils();
                    checkcommands();
                    break;
                case commandsutilsclass.help:
                    showcommands();
                    break;
                case commandsutilsclass.help_shortcut:
                    showcommands();
                    break;
                case commandsutilsclass.start:
                    startnewconsolecliutils();
                    checkcommands();
                    break;
                case commandsutilsclass.version:
                    showversioninfocliutils();
                    checkcommands();
                    break;
                case commandsutilsclass.copydata:
                    copy_data();
                    checkcommands();
                    break;
                case commandsutilsclass.exit:
                    this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("committing transactions...", TAG));
                    Thread.Sleep(500);
                    this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("closing connections...", TAG));
                    Thread.Sleep(500);
                    this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("cleaning temporary objects...", TAG));
                    persistconsolecontenttofile();
                    Thread.Sleep(500);
                    Environment.Exit(0);
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("Unrecognised Command [ " + _command + " ]. Type help or h to see a list of available commands.", TAG));
                    Console.ForegroundColor = ConsoleColor.Green;
                    persistconsolecontenttofile();
                    checkcommands();
                    break;
            }
        }

        public void showcommands()
        {
            try
            {

                string _strcommands = "command \t  -\t description \n\n";
                _strcommands += "createcrop \t  -\t create crop. \n";
                _strcommands += "listcrops \t  -\t show a list of crops. \n";
                _strcommands += "createcropvariety \t  -\t create crop variety. \n";
                _strcommands += "listcropsvarieties \t  -\t show a list of crops varieties. \n";
                _strcommands += "creatediseasepest \t  -\t create disease/pest. \n";
                _strcommands += "listdiseasespests \t  -\t show a list of diseases/pests. \n";
                _strcommands += "createmanufacturer \t  -\t create manufacturer. \n";
                _strcommands += "listmanufacturers \t  -\t show a list of manufacturer. \n";
                _strcommands += "createpestinsecticide \t  -\t create pesticide/insecticide. \n";
                _strcommands += "listpestsinsecticides \t  -\t show a list of pesticides/insecticides. \n";
                _strcommands += "createsetting \t  -\t create setting. \n";
                _strcommands += "listsettings \t  -\t show a list of settings. \n";
                _strcommands += "createcategory \t  -\t create category. \n";
                _strcommands += "listcategories \t  -\t show a list of categories. \n";
                _strcommands += "listall \t  -\t show a list of all records. \n";
                _strcommands += "createdatabase \t  -\t create database. \n";
                _strcommands += "help\\h \t  -\t show a list of commands. \n";
                _strcommands += "exit \t  -\t exit the system. \n";
                _strcommands += "start \t  -\t start a new window to run commands. \n";
                _strcommands += "version \t  -\t display the app version. \n";
                _strcommands += "connections \t  -\t show datastores connection states. \n";
                _strcommands += "copydata \t  -\t show datastores connection states. \n";

                globalwritetoconsole(_strcommands);
                checkcommands();

            }
            catch (Exception ex)
            {
                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs(ex.Message, TAG));
            }
        }

        void printtablelistseparator()
        {

            globalwritetoconsole("**********************************************************************************************************");
        }

        void createdatabasecliutils()
        {
            try
            {

                string _database_name = "";
                string _system = "";

                globalwritetoconsole("");
                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("loading create database interface...", TAG));

                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("provide detail for given fields...", TAG));

                globalwritetoconsole("database name:");
                _database_name = Console.ReadLine();

                while (String.IsNullOrEmpty(_database_name))
                {
                    _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("database name cannot be null.", TAG));
                    globalwritetoconsole("database name:");
                    _database_name = Console.ReadLine();
                }

                globalwritetoconsole(String.Format("\nsystem: [ {0}, {1}, {2}, {3} ]", DBContract.mssql, DBContract.mysql, DBContract.postgresql, DBContract.sqlite));
                _system = Console.ReadLine();

                while (String.IsNullOrEmpty(_system))
                {
                    _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("system cannot be null.", TAG));
                    globalwritetoconsole(String.Format("\nsystem: [ {0}, {1}, {2}, {3} ]", DBContract.mssql, DBContract.mysql, DBContract.postgresql, DBContract.sqlite));
                    _system = Console.ReadLine();
                }

                while (!String.IsNullOrEmpty(_system))
                {
                    if (_system == "mssql")
                    {
                        globalwritetoconsole("");

                        this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("create database task execution running against server [ " + DBContract.mssql + " ]...", TAG));

                        bool _exists_in_mssql = mssqlapisingleton.getInstance(_notificationmessageEventname, _progressBarNotificationEventname).check_if_mssql_database_exists(_database_name);

                        if (!_exists_in_mssql)
                        {

                            this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("database existence check complete.", TAG));

                            this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("database with name [ " + _database_name + " ] does not exists in " + DBContract.mssql + ".", TAG));

                            this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("commencing with the database creation...", TAG));

                            responsedto _responsedto = mssqlapisingleton.getInstance(_notificationmessageEventname, _progressBarNotificationEventname).createdatabasegivennamefromconsole(_database_name);

                            globalwritetoconsole(_responsedto.responsesuccessmessage);
                            globalwritetoconsole(_responsedto.responseerrormessage);

                            mssqlconnectionstringdto _connectionstringdto = mssqlapisingleton.getInstance(_notificationmessageEventname, _progressBarNotificationEventname).getmssqlconnectionstringdto();
                            _connectionstringdto.database = _database_name;
                            _connectionstringdto.new_database_name = _database_name;

                            _responsedto = mssqlapisingleton.getInstance(_notificationmessageEventname, _progressBarNotificationEventname).createtables(_connectionstringdto);

                            globalwritetoconsole(_responsedto.responsesuccessmessage);
                            globalwritetoconsole(_responsedto.responseerrormessage);

                        }
                        else
                        {
                            this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("database with name [ " + _database_name + " ] exists in " + DBContract.mssql + ".", TAG));
                        }
                        //Indicates that the iteration has ended
                        break;
                    }
                    else if (_system == "mysql")
                    {
                        globalwritetoconsole("");

                        this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("create database task execution running against server [ " + DBContract.mysql + " ]...", TAG));

                        bool _exists_in_mysql = mysqlapisingleton.getInstance(_notificationmessageEventname).check_if_mysql_database_exists(_database_name);

                        if (!_exists_in_mysql)
                        {

                            this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("database existence check complete.", TAG));

                            this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("database with name [ " + _database_name + " ] does not exists in " + DBContract.mysql + ".", TAG));

                            this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("commencing with the database creation...", TAG));

                            responsedto _responsedto = mysqlapisingleton.getInstance(_notificationmessageEventname).createdatabasegivennamefromconsole(_database_name);

                            globalwritetoconsole(_responsedto.responsesuccessmessage);
                            globalwritetoconsole(_responsedto.responseerrormessage);

                            mysqlconnectionstringdto _connectionstringdto = mysqlapisingleton.getInstance(_notificationmessageEventname).getmysqlconnectionstringdto();
                            _connectionstringdto.database = _database_name;
                            _connectionstringdto.new_database_name = _database_name;

                            _responsedto = mysqlapisingleton.getInstance(_notificationmessageEventname).createtables(_connectionstringdto);

                            globalwritetoconsole(_responsedto.responsesuccessmessage);
                            globalwritetoconsole(_responsedto.responseerrormessage);

                        }
                        else
                        {
                            this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("database with name [ " + _database_name + " ] exists in " + DBContract.mysql + ".", TAG));
                        }
                        //Indicates that the iteration has ended
                        break;
                    }
                    else if (_system == "postgresql")
                    {
                        globalwritetoconsole("");

                        this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("create database task execution running against server [ " + DBContract.postgresql + " ]...", TAG));

                        bool _exists_in_postgresql = postgresqlapisingleton.getInstance(_notificationmessageEventname).check_if_postgresql_database_exists(_database_name);

                        if (!_exists_in_postgresql)
                        {

                            this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("database existence check complete.", TAG));

                            this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("database with name [ " + _database_name + " ] does not exists in " + DBContract.postgresql + ".", TAG));

                            this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("commencing with the database creation...", TAG));

                            responsedto _responsedto = postgresqlapisingleton.getInstance(_notificationmessageEventname).createdatabasegivennamefromconsole(_database_name);

                            globalwritetoconsole(_responsedto.responsesuccessmessage);
                            globalwritetoconsole(_responsedto.responseerrormessage);

                            postgresqlconnectionstringdto _connectionstringdto = postgresqlapisingleton.getInstance(_notificationmessageEventname).getpostgresqlconnectionstringdto();
                            _connectionstringdto.database = _database_name;
                            _connectionstringdto.new_database_name = _database_name;

                            _responsedto = postgresqlapisingleton.getInstance(_notificationmessageEventname).createtables(_connectionstringdto);

                            globalwritetoconsole(_responsedto.responsesuccessmessage);
                            globalwritetoconsole(_responsedto.responseerrormessage);

                        }
                        else
                        {
                            this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("database with name [ " + _database_name + " ] exists in " + DBContract.postgresql + ".", TAG));
                        }
                        //Indicates that the iteration has ended
                        break;
                    }
                    else if (_system == "sqlite")
                    {
                        globalwritetoconsole("");

                        this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("create database task execution running against server [ " + DBContract.sqlite + " ]...", TAG));

                        bool _exists_in_sqlite = sqliteapisingleton.getInstance(_notificationmessageEventname).check_if_sqlite_database_exists(_database_name);

                        if (!_exists_in_sqlite)
                        {

                            this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("database existence check complete.", TAG));

                            this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("database with name [ " + _database_name + " ] does not exists in " + DBContract.sqlite + ".", TAG));

                            this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("commencing with the database creation...", TAG));

                            responsedto _responsedto = sqliteapisingleton.getInstance(_notificationmessageEventname).createdatabasegivennamefromconsole(_database_name);

                            globalwritetoconsole(_responsedto.responsesuccessmessage);
                            globalwritetoconsole(_responsedto.responseerrormessage);

                            sqliteconnectionstringdto _connectionstringdto = sqliteapisingleton.getInstance(_notificationmessageEventname).getsqliteconnectionstringdto();
                            _connectionstringdto.database = _database_name;
                            _connectionstringdto.new_database_name = _database_name;

                            _responsedto = sqliteapisingleton.getInstance(_notificationmessageEventname).createtables(_connectionstringdto);

                            globalwritetoconsole(_responsedto.responsesuccessmessage);
                            globalwritetoconsole(_responsedto.responseerrormessage);

                        }
                        else
                        {
                            this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("database with name [ " + _database_name + " ] exists in " + DBContract.sqlite + ".", TAG));
                        }
                        //Indicates that the iteration has ended
                        break;
                    }
                    else
                    {
                        _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs(String.Format("\nsystem value must either be: [ {0}, {1}, {2}, {3} ]", DBContract.mssql, DBContract.mysql, DBContract.postgresql, DBContract.sqlite), TAG));
                        globalwritetoconsole(String.Format("\nsystem value must either be: [ {0}, {1}, {2}, {3} ]", DBContract.mssql, DBContract.mysql, DBContract.postgresql, DBContract.sqlite));
                        _system = Console.ReadLine();
                        continue;
                    }

                }

            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs(ex.Message, TAG));
                Console.ForegroundColor = ConsoleColor.Green;
            }
        }

        void listallcliutils()
        {
            try
            {
                long _start = DateTime.Now.Ticks;
                DateTime _begin = DateTime.Now;

                listcropscliutils();
                listmanufacturerscliutils();
                listcategoriescliutils();
                listsettingscliutils();
                listcropsdiseasescliutils();
                listcropsvarietiescliutils();
                listpestsinsecticidescliutils();

                long _end = DateTime.Now.Ticks;
                DateTime _terminate = DateTime.Now;
                long _duration = _end - _start;
                TimeSpan _iduration = _terminate - _begin;

                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("Task took [ " + _iduration + " ] seconds.", TAG));

            }
            catch (Exception ex)
            {
                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs(ex.Message, TAG));
            }
        }

        void printcrops(DataTable dt, string _datastore)
        {
            try
            {

                globalwritetoconsole("");
                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("begin printing records from [ " + _datastore + " ]...", TAG));

                var _recordscount = dt.Rows.Count;
                for (int i = 0; i < _recordscount; i++)
                {

                    cropdto _cropdto = new cropdto();
                    _cropdto.crop_id = Convert.ToString(dt.Rows[i]["crop_id"]);
                    _cropdto.crop_name = Convert.ToString(dt.Rows[i]["crop_name"]);
                    _cropdto.crop_status = Convert.ToString(dt.Rows[i]["crop_status"]);
                    _cropdto.created_date = Convert.ToString(dt.Rows[i]["created_date"]);

                    string _strlstdto = "id: " + String.Format("{0}", _cropdto.crop_id) + "\t";
                    _strlstdto += "name: " + _cropdto.crop_name + "\t";
                    _strlstdto += "status: " + _cropdto.crop_status + "\t";
                    _strlstdto += "created date: " + _cropdto.created_date + "\n";

                    globalwritetoconsole(_strlstdto);
                }

                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("fetch crops list task finished. " + _datastore + " records count [ " + _recordscount + " ].", TAG));

                printtablelistseparator();

            }
            catch (Exception ex)
            {
                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs(ex.Message, TAG));
            }
        }

        void printcropsvarieties(DataTable dt, string _datastore)
        {
            try
            {

                globalwritetoconsole("");
                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("begin printing records from [ " + _datastore + " ]...", TAG));

                var _recordscount = dt.Rows.Count;
                for (int i = 0; i < _recordscount; i++)
                {

                    cropvarietydto _cropvarietydto = new cropvarietydto();
                    _cropvarietydto.crop_variety_id = Convert.ToString(dt.Rows[i]["crop_variety_id"]);
                    _cropvarietydto.crop_variety_name = Convert.ToString(dt.Rows[i]["crop_variety_name"]);
                    _cropvarietydto.crop_variety_status = Convert.ToString(dt.Rows[i]["crop_variety_status"]);
                    _cropvarietydto.created_date = Convert.ToString(dt.Rows[i]["created_date"]);
                    _cropvarietydto.crop_variety_crop_id = Convert.ToString(dt.Rows[i]["crop_variety_crop_id"]);
                    _cropvarietydto.crop_variety_manufacturer_id = Convert.ToString(dt.Rows[i]["crop_variety_manufacturer_id"]);

                    string _strlstdto = "id: " + String.Format("{0}", _cropvarietydto.crop_variety_id) + "\t";
                    _strlstdto += "name: " + _cropvarietydto.crop_variety_name + "\t";
                    _strlstdto += "status: " + _cropvarietydto.crop_variety_status + "\t";
                    _strlstdto += "crop: " + get_crop_name_given_id(_cropvarietydto.crop_variety_crop_id) + "\t";
                    _strlstdto += "manufacturer: " + get_manufacturer_name_given_id(_cropvarietydto.crop_variety_manufacturer_id) + "\t";
                    _strlstdto += "created date: " + _cropvarietydto.created_date + "\n";

                    globalwritetoconsole(_strlstdto);
                }

                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("fetch crops list task finished. " + _datastore + " records count [ " + _recordscount + " ].", TAG));

                printtablelistseparator();

            }
            catch (Exception ex)
            {
                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs(ex.Message, TAG));
            }
        }

        void printcategories(DataTable dt, string _datastore)
        {
            try
            {

                globalwritetoconsole("");
                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("begin printing records from [ " + _datastore + " ]...", TAG));

                var _recordscount = dt.Rows.Count;
                for (int i = 0; i < _recordscount; i++)
                {

                    categorydto _categorydto = new categorydto();
                    _categorydto.category_id = Convert.ToString(dt.Rows[i]["category_id"]);
                    _categorydto.category_name = Convert.ToString(dt.Rows[i]["category_name"]);
                    _categorydto.category_status = Convert.ToString(dt.Rows[i]["category_status"]);
                    _categorydto.created_date = Convert.ToString(dt.Rows[i]["created_date"]);

                    string _strlstdto = "id: " + String.Format("{0}", _categorydto.category_id) + "\t";
                    _strlstdto += "name: " + _categorydto.category_name + "\t";
                    _strlstdto += "status: " + _categorydto.category_status + "\t";
                    _strlstdto += "created date: " + _categorydto.created_date + "\n";

                    globalwritetoconsole(_strlstdto);
                }

                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("fetch categories list task finished. " + _datastore + " records count [ " + _recordscount + " ].", TAG));

                printtablelistseparator();

            }
            catch (Exception ex)
            {
                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs(ex.Message, TAG));
            }
        }

        void printcropsdiseases(DataTable dt, string _datastore)
        {
            try
            {

                globalwritetoconsole("");
                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("begin printing records from [ " + _datastore + " ]...", TAG));

                var _recordscount = dt.Rows.Count;
                for (int i = 0; i < _recordscount; i++)
                {

                    cropdiseasedto _cropdiseasedto = new cropdiseasedto();
                    _cropdiseasedto.crop_disease_id = Convert.ToString(dt.Rows[i]["crop_disease_id"]);
                    _cropdiseasedto.crop_disease_name = Convert.ToString(dt.Rows[i]["crop_disease_name"]);
                    _cropdiseasedto.crop_disease_category = Convert.ToString(dt.Rows[i]["crop_disease_category"]);
                    _cropdiseasedto.crop_disease_status = Convert.ToString(dt.Rows[i]["crop_disease_status"]);
                    _cropdiseasedto.created_date = Convert.ToString(dt.Rows[i]["created_date"]);

                    string _strlstdto = "id: " + String.Format("{0}", _cropdiseasedto.crop_disease_id) + "\t";
                    _strlstdto += "name: " + _cropdiseasedto.crop_disease_name + "\t";
                    _strlstdto += "category: " + _cropdiseasedto.crop_disease_category + "\t";
                    _strlstdto += "status: " + _cropdiseasedto.crop_disease_status + "\t";
                    _strlstdto += "created date: " + _cropdiseasedto.created_date + "\n";

                    globalwritetoconsole(_strlstdto);
                }

                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("fetch diseases/pests list task finished. " + _datastore + " records count [ " + _recordscount + " ].", TAG));

                printtablelistseparator();

            }
            catch (Exception ex)
            {
                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs(ex.Message, TAG));
            }
        }

        void printmanufacturers(DataTable dt, string _datastore)
        {
            try
            {

                globalwritetoconsole("");
                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("begin printing records from [ " + _datastore + " ]...", TAG));

                var _recordscount = dt.Rows.Count;
                for (int i = 0; i < _recordscount; i++)
                {

                    manufacturerdto _manufacturerdto = new manufacturerdto();
                    _manufacturerdto.manufacturer_id = Convert.ToString(dt.Rows[i]["manufacturer_id"]);
                    _manufacturerdto.manufacturer_name = Convert.ToString(dt.Rows[i]["manufacturer_name"]);
                    _manufacturerdto.manufacturer_status = Convert.ToString(dt.Rows[i]["manufacturer_status"]);
                    _manufacturerdto.created_date = Convert.ToString(dt.Rows[i]["created_date"]);

                    string _strlstdto = "id: " + String.Format("{0}", _manufacturerdto.manufacturer_id) + "\t";
                    _strlstdto += "name: " + _manufacturerdto.manufacturer_name + "\t";
                    _strlstdto += "status: " + _manufacturerdto.manufacturer_status + "\t";
                    _strlstdto += "created date: " + _manufacturerdto.created_date + "\n";

                    globalwritetoconsole(_strlstdto);
                }

                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("fetch manufacturers list task finished. " + _datastore + " records count [ " + _recordscount + " ].", TAG));

                printtablelistseparator();

            }
            catch (Exception ex)
            {
                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs(ex.Message, TAG));
            }
        }

        void printpestsinsecticides(DataTable dt, string _datastore)
        {
            try
            {

                globalwritetoconsole("");
                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("begin printing records from [ " + _datastore + " ]...", TAG));

                var _recordscount = dt.Rows.Count;
                for (int i = 0; i < _recordscount; i++)
                {

                    pestinsecticidedto _pestinsecticidedto = new pestinsecticidedto();
                    _pestinsecticidedto.pestinsecticide_id = Convert.ToString(dt.Rows[i]["pestinsecticide_id"]);
                    _pestinsecticidedto.pestinsecticide_name = Convert.ToString(dt.Rows[i]["pestinsecticide_name"]);
                    _pestinsecticidedto.pestinsecticide_category = Convert.ToString(dt.Rows[i]["pestinsecticide_category"]);
                    _pestinsecticidedto.pestinsecticide_manufacturer_id = Convert.ToString(dt.Rows[i]["pestinsecticide_manufacturer_id"]);
                    _pestinsecticidedto.pestinsecticide_crop_disease_id = Convert.ToString(dt.Rows[i]["pestinsecticide_crop_disease_id"]);
                    _pestinsecticidedto.pestinsecticide_status = Convert.ToString(dt.Rows[i]["pestinsecticide_status"]);
                    _pestinsecticidedto.created_date = Convert.ToString(dt.Rows[i]["created_date"]);

                    string _strlstdto = "id: " + String.Format("{0}", _pestinsecticidedto.pestinsecticide_id) + "\t";
                    _strlstdto += "name: " + _pestinsecticidedto.pestinsecticide_name + "\t";
                    _strlstdto += "category: " + get_category_name_given_id(_pestinsecticidedto.pestinsecticide_category) + "\t";
                    _strlstdto += "disease/pest: " + get_diseasepest_name_given_id(_pestinsecticidedto.pestinsecticide_crop_disease_id) + "\t";
                    _strlstdto += "manufacturer: " + get_manufacturer_name_given_id(_pestinsecticidedto.pestinsecticide_manufacturer_id) + "\t";
                    _strlstdto += "status: " + _pestinsecticidedto.pestinsecticide_status + "\t";
                    _strlstdto += "created date: " + _pestinsecticidedto.created_date + "\n";

                    globalwritetoconsole(_strlstdto);
                }

                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("fetch pesticides/insecticides list task finished. " + _datastore + " records count [ " + _recordscount + " ].", TAG));

                printtablelistseparator();

            }
            catch (Exception ex)
            {
                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs(ex.Message, TAG));
            }
        }

        void printsettings(DataTable dt, string _datastore)
        {
            try
            {

                globalwritetoconsole("");
                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("begin printing records from [ " + _datastore + " ]...", TAG));

                var _recordscount = dt.Rows.Count;
                for (int i = 0; i < _recordscount; i++)
                {

                    settingdto _settingdto = new settingdto();
                    _settingdto.setting_id = Convert.ToString(dt.Rows[i]["setting_id"]);
                    _settingdto.setting_name = Convert.ToString(dt.Rows[i]["setting_name"]);
                    _settingdto.setting_value = Convert.ToString(dt.Rows[i]["setting_value"]);
                    _settingdto.setting_status = Convert.ToString(dt.Rows[i]["setting_status"]);
                    _settingdto.created_date = Convert.ToString(dt.Rows[i]["created_date"]);

                    string _strlstdto = "id: " + String.Format("{0}", _settingdto.setting_id) + "\t";
                    _strlstdto += "name: " + _settingdto.setting_name + "\t";
                    _strlstdto += "value: " + _settingdto.setting_value + "\t";
                    _strlstdto += "status: " + _settingdto.setting_status + "\t";
                    _strlstdto += "created date: " + _settingdto.created_date + "\n";

                    globalwritetoconsole(_strlstdto);
                }

                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("fetch settings list task finished. " + _datastore + " records count [ " + _recordscount + " ].", TAG));

                printtablelistseparator();

            }
            catch (Exception ex)
            {
                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs(ex.Message, TAG));
            }
        }

        DataTable getallrecordsfrommssql(string query)
        {
            try
            {

                DataTable mssql_dt = mssqlapisingleton.getInstance(_notificationmessageEventname, _progressBarNotificationEventname).getallrecordsglobal(query);
                if (mssql_dt != null && mssql_dt.Rows.Count != 0)
                {
                    //_notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("mssql records count: " + mssql_dt.Rows.Count, TAG));
                }
                return mssql_dt;

            }
            catch (Exception ex)
            {
                _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs(ex.Message, TAG));
                return null;
            }
        }

        DataTable getallrecordsfrommysql(string query)
        {
            try
            {

                DataTable mysql_dt = mysqlapisingleton.getInstance(_notificationmessageEventname).getallrecordsglobal(query);
                if (mysql_dt != null && mysql_dt.Rows.Count != 0)
                {
                    //_notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("mysql records count: " + mysql_dt.Rows.Count, TAG));
                }
                return mysql_dt;

            }
            catch (Exception ex)
            {
                _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs(ex.Message, TAG));
                return null;
            }
        }

        DataTable getallrecordsfromsqlite(string query)
        {
            try
            {

                DataTable sqlite_dt = sqliteapisingleton.getInstance(_notificationmessageEventname).getallrecordsglobal(query);
                if (sqlite_dt != null && sqlite_dt.Rows.Count != 0)
                {
                    //_notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("sqlite records count: " + sqlite_dt.Rows.Count, TAG));
                }
                return sqlite_dt;

            }
            catch (Exception ex)
            {
                _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs(ex.Message, TAG));
                return null;
            }
        }

        DataTable getallrecordsfrompostgresql(string query)
        {
            try
            {

                DataTable postgresql_dt = postgresqlapisingleton.getInstance(_notificationmessageEventname).getallrecordsglobal(query);
                if (postgresql_dt != null && postgresql_dt.Rows.Count != 0)
                {
                    //_notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("postgresql records count: " + postgresql_dt.Rows.Count, TAG));
                }
                return postgresql_dt;

            }
            catch (Exception ex)
            {
                _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs(ex.Message, TAG));
                return null;
            }
        }

        public void createcropcliutils()
        {
            try
            {
                cropdto _cropdto = new cropdto();
                DateTime currentDate = DateTime.Now;
                String dateTimenow = currentDate.ToString("dd-MM-yyyy HH:mm:ss");

                globalwritetoconsole("");
                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("loading create crop interface...", TAG));

                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("provide detail for given fields...", TAG));

                globalwritetoconsole("crop name:");
                _cropdto.crop_name = Console.ReadLine();

                while (String.IsNullOrEmpty(_cropdto.crop_name))
                {
                    _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("crop name cannot be null.", TAG));
                    globalwritetoconsole("crop name:");
                    _cropdto.crop_name = Console.ReadLine();
                }

                globalwritetoconsole("\ncrop status: { active or inactive }");
                _cropdto.crop_status = Console.ReadLine();

                while (String.IsNullOrEmpty(_cropdto.crop_status))
                {
                    _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("crop status cannot be null.", TAG));
                    globalwritetoconsole("\ncrop status: { active or inactive }");
                    _cropdto.crop_status = Console.ReadLine();
                }

                while (!String.IsNullOrEmpty(_cropdto.crop_status))
                {
                    if (_cropdto.crop_status == "active")
                    {
                        //Indicates that the iteration has ended
                        break;
                    }
                    else if (_cropdto.crop_status == "inactive")
                    {
                        //Indicates that the iteration has ended
                        break;
                    }
                    else
                    {
                        _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("crop status value must either be active or inactive.", TAG));
                        globalwritetoconsole("\ncrop status: { active or inactive }");
                        _cropdto.crop_status = Console.ReadLine();
                        continue;
                    }

                }

                _cropdto.created_date = dateTimenow;

                globalwritetoconsole("");

                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("crop insert task execution running....", TAG));

                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("checking if crop with name [ " + _cropdto.crop_name + " ] exists in " + DBContract.mssql + "....", TAG));

                bool _exists_in_mssql = mssqlapisingleton.getInstance(_notificationmessageEventname, _progressBarNotificationEventname).checkifcropexists(_cropdto.crop_name, DBContract.getdefaultmssqlconnectionstring());

                if (!_exists_in_mssql)
                {
                    this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("crop with name [ " + _cropdto.crop_name + " ] does not exists in " + DBContract.mssql + "\nproceeding with insertion....", TAG));

                    databasehelpersingleton.getInstance(_notificationmessageEventname, _progressBarNotificationEventname).savecropinmssqldb(_cropdto);
                }
                else
                {
                    this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("crop with name [ " + _cropdto.crop_name + " ] exists in " + DBContract.mssql + ".", TAG));
                }

                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("checking if crop with name [ " + _cropdto.crop_name + " ] exists in " + DBContract.sqlite + "....", TAG));

                bool _exists_in_sqlite = sqliteapisingleton.getInstance(_notificationmessageEventname).checkifcropexists(_cropdto.crop_name, DBContract.getdefaultsqliteconnectionstring());

                if (!_exists_in_sqlite)
                {
                    this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("crop with name [ " + _cropdto.crop_name + " ] does not exists in " + DBContract.sqlite + ",\nproceeding with insertion....", TAG));

                    databasehelpersingleton.getInstance(_notificationmessageEventname, _progressBarNotificationEventname).savecropinsqlitedb(_cropdto);
                }
                else
                {
                    this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("crop with name [ " + _cropdto.crop_name + " ] exists in " + DBContract.sqlite + ".", TAG));
                }

                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("checking if crop with name [ " + _cropdto.crop_name + " ] exists in " + DBContract.mysql + "....", TAG));

                bool _exists_in_mysql = mysqlapisingleton.getInstance(_notificationmessageEventname).checkifcropexists(_cropdto.crop_name, DBContract.getdefaultmysqlconnectionstring());

                if (!_exists_in_mysql)
                {
                    this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("crop with name [ " + _cropdto.crop_name + " ] does not exists in " + DBContract.mysql + ",\nproceeding with insertion....", TAG));

                    databasehelpersingleton.getInstance(_notificationmessageEventname, _progressBarNotificationEventname).savecropinmysqldb(_cropdto);
                }
                else
                {
                    this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("crop with name [ " + _cropdto.crop_name + " ] exists in " + DBContract.mysql + ".", TAG));
                }

                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("checking if crop with name [ " + _cropdto.crop_name + " ] exists in " + DBContract.postgresql + "....", TAG));

                bool _exists_in_postgresql = postgresqlapisingleton.getInstance(_notificationmessageEventname).checkifcropexists(_cropdto.crop_name, DBContract.getdefaultpostgresqlconnectionstring());

                if (!_exists_in_postgresql)
                {
                    this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("crop with name [ " + _cropdto.crop_name + " ] does not exists in " + DBContract.postgresql + ",\nproceeding with insertion....", TAG));

                    databasehelpersingleton.getInstance(_notificationmessageEventname, _progressBarNotificationEventname).savecropinpostgresqldb(_cropdto);

                }
                else
                {
                    this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("crop with name [ " + _cropdto.crop_name + " ] exists in " + DBContract.postgresql + ".", TAG));
                }

            }
            catch (Exception ex)
            {
                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs(ex.Message, TAG));
            }
        }

        public void listcropscliutils()
        {
            try
            {
                globalwritetoconsole("");
                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("fetch crops list task running...", TAG));

                string query = DBContract.CROPS_SELECT_ALL_QUERY;

                DataTable mssql_dt = null;
                DataTable mysql_dt = null;
                DataTable sqlite_dt = null;
                DataTable postgresql_dt = null;

                mssql_dt = getallrecordsfrommssql(query);
                mysql_dt = getallrecordsfrommysql(query);
                sqlite_dt = getallrecordsfromsqlite(query);
                postgresql_dt = getallrecordsfrompostgresql(query);

                if (mssql_dt != null && mssql_dt.Rows.Count != 0)
                {
                    printtablelistseparator();
                    printcrops(mssql_dt, datastoreconstants.mssql);
                }
                if (mysql_dt != null && mysql_dt.Rows.Count != 0)
                {
                    printcrops(mysql_dt, datastoreconstants.mysql);
                }
                if (sqlite_dt != null && sqlite_dt.Rows.Count != 0)
                {
                    printcrops(sqlite_dt, datastoreconstants.sqlite);
                }
                if (postgresql_dt != null && postgresql_dt.Rows.Count != 0)
                {
                    printcrops(postgresql_dt, datastoreconstants.postgresql);
                }

            }
            catch (Exception ex)
            {
                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs(ex.Message, TAG));
            }
        }

        public void createcropvarietycliutils()
        {
            try
            {
                cropvarietydto _cropvarietydto = new cropvarietydto();
                DateTime currentDate = DateTime.Now;
                String dateTimenow = currentDate.ToString("dd-MM-yyyy HH:mm:ss");

                globalwritetoconsole("");
                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("loading create crop variety interface...", TAG));

                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("provide detail for given fields...", TAG));

                globalwritetoconsole("variety name:");
                _cropvarietydto.crop_variety_name = Console.ReadLine();

                while (String.IsNullOrEmpty(_cropvarietydto.crop_variety_name))
                {
                    _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("variety name cannot be null.", TAG));
                    globalwritetoconsole("variety name:");
                    _cropvarietydto.crop_variety_name = Console.ReadLine();
                }

                _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("\ncrop must be one of the following...", TAG));

                print_all_crops();

                globalwritetoconsole("\ncrop variety crop:");

                string _crop_name = Console.ReadLine();

                while (String.IsNullOrEmpty(_crop_name))
                {
                    _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("\ncrop variety crop cannot be null.", TAG));

                    _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("\ncrop must be one of the following...", TAG));

                    print_all_crops();

                    globalwritetoconsole("\ncrop variety crop:");

                    _crop_name = Console.ReadLine();

                }

                while (!String.IsNullOrEmpty(_crop_name))
                {
                    bool _does_entered_crop_exist = check_if_crop_exists(_crop_name);

                    if (_does_entered_crop_exist)
                    {
                        break;
                    }
                    else
                    {
                        _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("\ncrop does not exist. must be one of the following...", TAG));

                        print_all_crops();

                        globalwritetoconsole("\ncrop variety crop:");

                        _crop_name = Console.ReadLine();
                    }
                }

                _cropvarietydto.crop_variety_crop_id = get_crop_id_given_name(_crop_name);

                _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("\nmanufacturer must be one of the following...", TAG));

                print_all_manufacturers();

                globalwritetoconsole("\ncrop variety manufacturer:");

                string _maufacturer_name = Console.ReadLine();

                while (String.IsNullOrEmpty(_maufacturer_name))
                {
                    _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("\ncrop variety manufacturer cannot be null.", TAG));

                    _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("\nmanufacturer must be one of the following...", TAG));

                    print_all_manufacturers();

                    globalwritetoconsole("\ncrop variety manufacturer:");

                    _maufacturer_name = Console.ReadLine();

                }

                while (!String.IsNullOrEmpty(_maufacturer_name))
                {
                    bool _does_entered_manufacturer_exist = check_if_manufacturer_exists(_maufacturer_name);

                    if (_does_entered_manufacturer_exist)
                    {
                        break;
                    }
                    else
                    {
                        _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("\nmanufacturer does not exist. must be one of the following...", TAG));

                        print_all_manufacturers();

                        globalwritetoconsole("\ncrop variety manufacturer:");

                        _maufacturer_name = Console.ReadLine();
                    }
                }

                _cropvarietydto.crop_variety_manufacturer_id = get_manufacturer_id_given_name(_maufacturer_name);

                globalwritetoconsole("\ncrop variety status: { active or inactive }");
                _cropvarietydto.crop_variety_status = Console.ReadLine();

                while (String.IsNullOrEmpty(_cropvarietydto.crop_variety_status))
                {
                    _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("crop variety status cannot be null.", TAG));
                    globalwritetoconsole("\ncrop variety status: { active or inactive }");
                    _cropvarietydto.crop_variety_status = Console.ReadLine();
                }

                while (!String.IsNullOrEmpty(_cropvarietydto.crop_variety_status))
                {
                    if (_cropvarietydto.crop_variety_status == "active")
                    {
                        //Indicates that the iteration has ended
                        break;
                    }
                    else if (_cropvarietydto.crop_variety_status == "inactive")
                    {
                        //Indicates that the iteration has ended
                        break;
                    }
                    else
                    {
                        _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("crop variety status value must either be active or inactive.", TAG));
                        globalwritetoconsole("\ncrop variety status: { active or inactive }");
                        _cropvarietydto.crop_variety_status = Console.ReadLine();
                        continue;
                    }

                }

                _cropvarietydto.created_date = dateTimenow;

                globalwritetoconsole("");

                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("crop variety insert task execution running.....", TAG));

                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("checking if crop variety with name [ " + _cropvarietydto.crop_variety_name + " ] exists in " + DBContract.mssql + "....", TAG));

                bool _exists_in_mssql = mssqlapisingleton.getInstance(_notificationmessageEventname, _progressBarNotificationEventname).checkifcropvarietyexists(_cropvarietydto.crop_variety_name, DBContract.getdefaultmssqlconnectionstring());

                if (!_exists_in_mssql)
                {
                    this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("crop variety with name [ " + _cropvarietydto.crop_variety_name + " ] does not exists in " + DBContract.mssql + "\nproceeding with insertion....", TAG));

                    databasehelpersingleton.getInstance(_notificationmessageEventname, _progressBarNotificationEventname).savecropvarietyinmssqldb(_cropvarietydto);
                }
                else
                {
                    this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("crop variety with name [ " + _cropvarietydto.crop_variety_name + " ] exists in " + DBContract.mssql + ".", TAG));
                }

                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("checking if crop variety with name [ " + _cropvarietydto.crop_variety_name + " ] exists in " + DBContract.sqlite + "....", TAG));

                bool _exists_in_sqlite = sqliteapisingleton.getInstance(_notificationmessageEventname).checkifcropvarietyexists(_cropvarietydto.crop_variety_name, DBContract.getdefaultsqliteconnectionstring());

                if (!_exists_in_sqlite)
                {
                    this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("crop variety with name [ " + _cropvarietydto.crop_variety_name + " ] does not exists in " + DBContract.sqlite + "\nproceeding with insertion....", TAG));

                    databasehelpersingleton.getInstance(_notificationmessageEventname, _progressBarNotificationEventname).savecropvarietyinsqlitedb(_cropvarietydto);
                }
                else
                {
                    this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("crop variety with name [ " + _cropvarietydto.crop_variety_name + " ] exists in " + DBContract.sqlite + ".", TAG));
                }

                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("checking if crop variety with name [ " + _cropvarietydto.crop_variety_name + " ] exists in " + DBContract.mysql + "....", TAG));

                bool _exists_in_mysql = mysqlapisingleton.getInstance(_notificationmessageEventname).checkifcropvarietyexists(_cropvarietydto.crop_variety_name, DBContract.getdefaultmysqlconnectionstring());

                if (!_exists_in_mysql)
                {
                    this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("crop variety with name [ " + _cropvarietydto.crop_variety_name + " ] does not exists in " + DBContract.mysql + "\nproceeding with insertion....", TAG));

                    databasehelpersingleton.getInstance(_notificationmessageEventname, _progressBarNotificationEventname).savecropvarietyinmysqldb(_cropvarietydto);
                }
                else
                {
                    this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("crop variety with name [ " + _cropvarietydto.crop_variety_name + " ] exists in " + DBContract.mysql + ".", TAG));
                }

                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("checking if crop variety with name [ " + _cropvarietydto.crop_variety_name + " ] exists in " + DBContract.postgresql + "....", TAG));

                bool _exists_in_postgresql = postgresqlapisingleton.getInstance(_notificationmessageEventname).checkifcropvarietyexists(_cropvarietydto.crop_variety_name, DBContract.getdefaultpostgresqlconnectionstring());

                if (!_exists_in_postgresql)
                {
                    this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("crop variety with name [ " + _cropvarietydto.crop_variety_name + " ] does not exists in " + DBContract.postgresql + "\nproceeding with insertion....", TAG));

                    databasehelpersingleton.getInstance(_notificationmessageEventname, _progressBarNotificationEventname).savecropvarietyinpostgresqldb(_cropvarietydto);
                }
                else
                {
                    this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("crop variety with name [ " + _cropvarietydto.crop_variety_name + " ] exists in " + DBContract.postgresql + ".", TAG));
                }

            }
            catch (Exception ex)
            {
                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs(ex.Message, TAG));
            }
        }

        public void listcropsvarietiescliutils()
        {
            try
            {
                globalwritetoconsole("");
                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("fetch crops varieties list task running...", TAG));

                string query = DBContract.CROPS_VARIETIES_SELECT_ALL_QUERY;

                DataTable mssql_dt = null;
                DataTable mysql_dt = null;
                DataTable sqlite_dt = null;
                DataTable postgresql_dt = null;

                mssql_dt = getallrecordsfrommssql(query);
                mysql_dt = getallrecordsfrommysql(query);
                sqlite_dt = getallrecordsfromsqlite(query);
                postgresql_dt = getallrecordsfrompostgresql(query);

                if (mssql_dt != null && mssql_dt.Rows.Count != 0)
                {
                    printtablelistseparator();
                    printcropsvarieties(mssql_dt, datastoreconstants.mssql);
                }
                if (mysql_dt != null && mysql_dt.Rows.Count != 0)
                {
                    printcropsvarieties(mysql_dt, datastoreconstants.mysql);
                }
                if (sqlite_dt != null && sqlite_dt.Rows.Count != 0)
                {
                    printcropsvarieties(sqlite_dt, datastoreconstants.sqlite);
                }
                if (postgresql_dt != null && postgresql_dt.Rows.Count != 0)
                {
                    printcropsvarieties(postgresql_dt, datastoreconstants.postgresql);
                }

            }
            catch (Exception ex)
            {
                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs(ex.Message, TAG));
            }
        }

        public void createcropdiseasecliutils()
        {
            try
            {
                cropdiseasedto _cropdiseasedto = new cropdiseasedto();
                DateTime currentDate = DateTime.Now;
                String dateTimenow = currentDate.ToString("dd-MM-yyyy HH:mm:ss");

                globalwritetoconsole("");
                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("loading create disease/pest interface...", TAG));

                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("provide detail for given fields...", TAG));

                globalwritetoconsole("disease/pest name:");
                _cropdiseasedto.crop_disease_name = Console.ReadLine();

                while (String.IsNullOrEmpty(_cropdiseasedto.crop_disease_name))
                {
                    _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("disease/pest name cannot be null.", TAG));
                    globalwritetoconsole("disease/pest name:");
                    _cropdiseasedto.crop_disease_name = Console.ReadLine();
                }

                globalwritetoconsole("\ndisease/pest category: { disease or pest }");
                _cropdiseasedto.crop_disease_category = Console.ReadLine();

                while (String.IsNullOrEmpty(_cropdiseasedto.crop_disease_category))
                {
                    _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("disease/pest category cannot be null.", TAG));
                    globalwritetoconsole("\ndisease/pest category: { disease or pest }");
                    _cropdiseasedto.crop_disease_category = Console.ReadLine();
                }

                while (!String.IsNullOrEmpty(_cropdiseasedto.crop_disease_category))
                {
                    if (_cropdiseasedto.crop_disease_category == "disease")
                    {
                        //Indicates that the iteration has ended
                        break;
                    }
                    else if (_cropdiseasedto.crop_disease_category == "pest")
                    {
                        //Indicates that the iteration has ended
                        break;
                    }
                    else
                    {
                        _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("disease/pest category value must either be disease or pest.", TAG));
                        globalwritetoconsole("\ndisease/pest category: { disease or pest }");
                        _cropdiseasedto.crop_disease_category = Console.ReadLine();
                        continue;
                    }

                }

                globalwritetoconsole("\ndisease/pest status: { active or inactive }");
                _cropdiseasedto.crop_disease_status = Console.ReadLine();

                while (String.IsNullOrEmpty(_cropdiseasedto.crop_disease_status))
                {
                    _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("disease/pest status cannot be null.", TAG));
                    globalwritetoconsole("\ndisease/pest status: { active or inactive }");
                    _cropdiseasedto.crop_disease_status = Console.ReadLine();
                }

                while (!String.IsNullOrEmpty(_cropdiseasedto.crop_disease_status))
                {
                    if (_cropdiseasedto.crop_disease_status == "active")
                    {
                        //Indicates that the iteration has ended
                        break;
                    }
                    else if (_cropdiseasedto.crop_disease_status == "inactive")
                    {
                        //Indicates that the iteration has ended
                        break;
                    }
                    else
                    {
                        _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("disease/pest status value must either be active or inactive.", TAG));
                        globalwritetoconsole("\ndisease/pest status: { active or inactive }");
                        _cropdiseasedto.crop_disease_status = Console.ReadLine();
                        continue;
                    }

                }

                _cropdiseasedto.created_date = dateTimenow;

                globalwritetoconsole("");
                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("disease/pest insert task execution running...", TAG));

                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("checking if disease/pest with name [ " + _cropdiseasedto.crop_disease_name + " ] exists in " + DBContract.mssql + "....", TAG));

                bool _exists_in_mssql = mssqlapisingleton.getInstance(_notificationmessageEventname, _progressBarNotificationEventname).checkifcropexists(_cropdiseasedto.crop_disease_name, DBContract.getdefaultmssqlconnectionstring());

                if (!_exists_in_mssql)
                {
                    this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("disease/pest with name [ " + _cropdiseasedto.crop_disease_name + " ] does not exists in " + DBContract.mssql + "\nproceeding with insertion....", TAG));

                    databasehelpersingleton.getInstance(_notificationmessageEventname, _progressBarNotificationEventname).savecropdiseaseinmssqldb(_cropdiseasedto);
                }
                else
                {
                    this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("disease/pest with name [ " + _cropdiseasedto.crop_disease_name + " ] exists in " + DBContract.mssql + ".", TAG));
                }

                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("checking if disease/pest with name [ " + _cropdiseasedto.crop_disease_name + " ] exists in " + DBContract.sqlite + "....", TAG));

                bool _exists_in_sqlite = sqliteapisingleton.getInstance(_notificationmessageEventname).checkifcropexists(_cropdiseasedto.crop_disease_name, DBContract.getdefaultsqliteconnectionstring());

                if (!_exists_in_sqlite)
                {
                    this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("disease/pest with name [ " + _cropdiseasedto.crop_disease_name + " ] does not exists in " + DBContract.sqlite + ",\nproceeding with insertion....", TAG));

                    databasehelpersingleton.getInstance(_notificationmessageEventname, _progressBarNotificationEventname).savecropdiseaseinsqlitedb(_cropdiseasedto);
                }
                else
                {
                    this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("disease/pest with name [ " + _cropdiseasedto.crop_disease_name + " ] exists in " + DBContract.sqlite + ".", TAG));
                }

                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("checking if disease/pest with name [ " + _cropdiseasedto.crop_disease_name + " ] exists in " + DBContract.mysql + "....", TAG));

                bool _exists_in_mysql = mysqlapisingleton.getInstance(_notificationmessageEventname).checkifcropexists(_cropdiseasedto.crop_disease_name, DBContract.getdefaultmysqlconnectionstring());

                if (!_exists_in_mysql)
                {
                    this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("disease/pest with name [ " + _cropdiseasedto.crop_disease_name + " ] does not exists in " + DBContract.mysql + ",\nproceeding with insertion....", TAG));

                    databasehelpersingleton.getInstance(_notificationmessageEventname, _progressBarNotificationEventname).savecropdiseaseinmysqldb(_cropdiseasedto);
                }
                else
                {
                    this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("disease/pest with name [ " + _cropdiseasedto.crop_disease_name + " ] exists in " + DBContract.mysql + ".", TAG));
                }

                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("checking if disease/pest with name [ " + _cropdiseasedto.crop_disease_name + " ] exists in " + DBContract.postgresql + "....", TAG));

                bool _exists_in_postgresql = postgresqlapisingleton.getInstance(_notificationmessageEventname).checkifcropexists(_cropdiseasedto.crop_disease_name, DBContract.getdefaultpostgresqlconnectionstring());

                if (!_exists_in_postgresql)
                {
                    this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("disease/pest with name [ " + _cropdiseasedto.crop_disease_name + " ] does not exists in " + DBContract.postgresql + ",\nproceeding with insertion....", TAG));

                    databasehelpersingleton.getInstance(_notificationmessageEventname, _progressBarNotificationEventname).savecropdiseaseinpostgresqldb(_cropdiseasedto);
                }
                else
                {
                    this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("disease/pest with name [ " + _cropdiseasedto.crop_disease_name + " ] exists in " + DBContract.postgresql + ".", TAG));
                }

            }
            catch (Exception ex)
            {
                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs(ex.Message, TAG));
            }
        }

        public void listcropsdiseasescliutils()
        {
            try
            {
                globalwritetoconsole("");
                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("fetch diseases/pests list task running...", TAG));

                string query = DBContract.CROPS_DISEASES_SELECT_ALL_QUERY;

                DataTable mssql_dt = null;
                DataTable mysql_dt = null;
                DataTable sqlite_dt = null;
                DataTable postgresql_dt = null;

                mssql_dt = getallrecordsfrommssql(query);
                mysql_dt = getallrecordsfrommysql(query);
                sqlite_dt = getallrecordsfromsqlite(query);
                postgresql_dt = getallrecordsfrompostgresql(query);

                if (mssql_dt != null && mssql_dt.Rows.Count != 0)
                {
                    printtablelistseparator();
                    printcropsdiseases(mssql_dt, datastoreconstants.mssql);
                }
                if (mysql_dt != null && mysql_dt.Rows.Count != 0)
                {
                    printcropsdiseases(mysql_dt, datastoreconstants.mysql);
                }
                if (sqlite_dt != null && sqlite_dt.Rows.Count != 0)
                {
                    printcropsdiseases(sqlite_dt, datastoreconstants.sqlite);
                }
                if (postgresql_dt != null && postgresql_dt.Rows.Count != 0)
                {
                    printcropsdiseases(postgresql_dt, datastoreconstants.postgresql);
                }

            }
            catch (Exception ex)
            {
                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs(ex.Message, TAG));
            }
        }

        public void createmanufacturercliutils()
        {
            try
            {
                manufacturerdto _manufacturerdto = new manufacturerdto();
                DateTime currentDate = DateTime.Now;
                String dateTimenow = currentDate.ToString("dd-MM-yyyy HH:mm:ss");

                globalwritetoconsole("");
                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("loading create manufacturer interface...", TAG));

                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("provide detail for given fields...", TAG));

                globalwritetoconsole("manufacturer name:");
                _manufacturerdto.manufacturer_name = Console.ReadLine();

                while (String.IsNullOrEmpty(_manufacturerdto.manufacturer_name))
                {
                    _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("manufacturer name cannot be null.", TAG));
                    globalwritetoconsole("manufacturer name:");
                    _manufacturerdto.manufacturer_name = Console.ReadLine();
                }

                globalwritetoconsole("\nmanufacturer status: { active or inactive }");
                _manufacturerdto.manufacturer_status = Console.ReadLine();

                while (String.IsNullOrEmpty(_manufacturerdto.manufacturer_status))
                {
                    _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("nmanufacturer status cannot be null.", TAG));
                    globalwritetoconsole("\nnmanufacturer status: { active or inactive }");
                    _manufacturerdto.manufacturer_status = Console.ReadLine();
                }

                while (!String.IsNullOrEmpty(_manufacturerdto.manufacturer_status))
                {
                    if (_manufacturerdto.manufacturer_status == "active")
                    {
                        //Indicates that the iteration has ended
                        break;
                    }
                    else if (_manufacturerdto.manufacturer_status == "inactive")
                    {
                        //Indicates that the iteration has ended
                        break;
                    }
                    else
                    {
                        _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("nmanufacturer status value must either be active or inactive.", TAG));
                        globalwritetoconsole("\nnmanufacturer status: { active or inactive }");
                        _manufacturerdto.manufacturer_status = Console.ReadLine();
                        continue;
                    }

                }

                _manufacturerdto.created_date = dateTimenow;

                globalwritetoconsole("");
                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("manufacturer insert task execution running...", TAG));

                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("checking if manufacturer with name [ " + _manufacturerdto.manufacturer_name + " ] exists in " + DBContract.mssql + "....", TAG));

                bool _exists_in_mssql = mssqlapisingleton.getInstance(_notificationmessageEventname, _progressBarNotificationEventname).checkifmanufacturerexists(_manufacturerdto.manufacturer_name, DBContract.getdefaultmssqlconnectionstring());

                if (!_exists_in_mssql)
                {
                    this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("manufacturer with name [ " + _manufacturerdto.manufacturer_name + " ] does not exists in " + DBContract.mssql + "\nproceeding with insertion....", TAG));

                    databasehelpersingleton.getInstance(_notificationmessageEventname, _progressBarNotificationEventname).savemanufacturerinmssqldb(_manufacturerdto);
                }
                else
                {
                    this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("manufacturer with name [ " + _manufacturerdto.manufacturer_name + " ] exists in " + DBContract.mssql + ".", TAG));
                }

                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("checking if manufacturer with name [ " + _manufacturerdto.manufacturer_name + " ] exists in " + DBContract.sqlite + "....", TAG));

                bool _exists_in_sqlite = sqliteapisingleton.getInstance(_notificationmessageEventname).checkifmanufacturerexists(_manufacturerdto.manufacturer_name, DBContract.getdefaultsqliteconnectionstring());

                if (!_exists_in_sqlite)
                {
                    this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("manufacturer with name [ " + _manufacturerdto.manufacturer_name + " ] does not exists in " + DBContract.sqlite + "\nproceeding with insertion....", TAG));

                    databasehelpersingleton.getInstance(_notificationmessageEventname, _progressBarNotificationEventname).savemanufacturerinsqlitedb(_manufacturerdto);
                }
                else
                {
                    this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("manufacturer with name [ " + _manufacturerdto.manufacturer_name + " ] exists in " + DBContract.sqlite + ".", TAG));
                }

                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("checking if manufacturer with name [ " + _manufacturerdto.manufacturer_name + " ] exists in " + DBContract.mysql + "....", TAG));

                bool _exists_in_mysql = mysqlapisingleton.getInstance(_notificationmessageEventname).checkifmanufacturerexists(_manufacturerdto.manufacturer_name, DBContract.getdefaultmysqlconnectionstring());

                if (!_exists_in_mysql)
                {
                    this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("manufacturer with name [ " + _manufacturerdto.manufacturer_name + " ] does not exists in " + DBContract.mysql + "\nproceeding with insertion....", TAG));

                    databasehelpersingleton.getInstance(_notificationmessageEventname, _progressBarNotificationEventname).savemanufacturerinmysqldb(_manufacturerdto);
                }
                else
                {
                    this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("manufacturer with name [ " + _manufacturerdto.manufacturer_name + " ] exists in " + DBContract.mysql + ".", TAG));
                }

                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("checking if manufacturer with name [ " + _manufacturerdto.manufacturer_name + " ] exists in " + DBContract.postgresql + "....", TAG));

                bool _exists_in_postgresql = postgresqlapisingleton.getInstance(_notificationmessageEventname).checkifmanufacturerexists(_manufacturerdto.manufacturer_name, DBContract.getdefaultpostgresqlconnectionstring());

                if (!_exists_in_postgresql)
                {
                    this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("manufacturer with name [ " + _manufacturerdto.manufacturer_name + " ] does not exists in " + DBContract.postgresql + "\nproceeding with insertion....", TAG));

                    databasehelpersingleton.getInstance(_notificationmessageEventname, _progressBarNotificationEventname).savemanufacturerinpostgresqldb(_manufacturerdto);
                }
                else
                {
                    this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("manufacturer with name [ " + _manufacturerdto.manufacturer_name + " ] exists in " + DBContract.postgresql + ".", TAG));
                }

            }
            catch (Exception ex)
            {
                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs(ex.Message, TAG));
            }
        }

        public void listmanufacturerscliutils()
        {
            try
            {

                globalwritetoconsole("");
                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("fetch manufacturers list task running...", TAG));

                string query = DBContract.MANUFACTURERS_SELECT_ALL_QUERY;

                DataTable mssql_dt = null;
                DataTable mysql_dt = null;
                DataTable sqlite_dt = null;
                DataTable postgresql_dt = null;

                mssql_dt = getallrecordsfrommssql(query);
                mysql_dt = getallrecordsfrommysql(query);
                sqlite_dt = getallrecordsfromsqlite(query);
                postgresql_dt = getallrecordsfrompostgresql(query);

                if (mssql_dt != null && mssql_dt.Rows.Count != 0)
                {
                    printtablelistseparator();
                    printmanufacturers(mssql_dt, datastoreconstants.mssql);
                }
                if (mysql_dt != null && mysql_dt.Rows.Count != 0)
                {
                    printmanufacturers(mysql_dt, datastoreconstants.mysql);
                }
                if (sqlite_dt != null && sqlite_dt.Rows.Count != 0)
                {
                    printmanufacturers(sqlite_dt, datastoreconstants.sqlite);
                }
                if (postgresql_dt != null && postgresql_dt.Rows.Count != 0)
                {
                    printmanufacturers(postgresql_dt, datastoreconstants.postgresql);
                }

            }
            catch (Exception ex)
            {
                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs(ex.Message, TAG));
            }
        }

        public void createpestinsecticidecliutils()
        {
            try
            {
                pestinsecticidedto _pestinsecticidedto = new pestinsecticidedto();
                DateTime currentDate = DateTime.Now;
                String dateTimenow = currentDate.ToString("dd-MM-yyyy HH:mm:ss");

                globalwritetoconsole("");
                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("loading create pesticide/insecticide interface...", TAG));

                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("provide detail for given fields...", TAG));

                globalwritetoconsole("pesticide/insecticide name:");
                _pestinsecticidedto.pestinsecticide_name = Console.ReadLine();

                while (String.IsNullOrEmpty(_pestinsecticidedto.pestinsecticide_name))
                {
                    _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("pesticide/insecticide name cannot be null.", TAG));
                    globalwritetoconsole("pesticide/insecticide name:");
                    _pestinsecticidedto.pestinsecticide_name = Console.ReadLine();
                }

                _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("\ncategory must be one of the following...", TAG));

                print_all_categories();

                globalwritetoconsole("\npesticide/insecticide category:");

                string _category_name = Console.ReadLine();

                while (String.IsNullOrEmpty(_category_name))
                {
                    _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("\npesticide/insecticide category cannot be null.", TAG));

                    _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("\ncategory must be one of the following...", TAG));

                    print_all_categories();

                    globalwritetoconsole("\npesticide/insecticide category:");

                    _category_name = Console.ReadLine();

                }

                while (!String.IsNullOrEmpty(_category_name))
                {
                    bool _does_entered_category_exist = check_if_category_exists(_category_name);

                    if (_does_entered_category_exist)
                    {
                        break;
                    }
                    else
                    {
                        _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("\ncategory does not exist. must be one of the following...", TAG));

                        print_all_categories();

                        globalwritetoconsole("\npesticide/insecticide category:");

                        _category_name = Console.ReadLine();
                    }
                }

                _pestinsecticidedto.pestinsecticide_category = get_category_id_given_name(_category_name);

                _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("\nmanufacturer must be one of the following...", TAG));

                print_all_manufacturers();

                globalwritetoconsole("\npesticide/insecticide manufacturer:");

                string _maufacturer_name = Console.ReadLine();

                while (String.IsNullOrEmpty(_maufacturer_name))
                {
                    _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("\npesticide/insecticide manufacturer cannot be null.", TAG));

                    _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("\nmanufacturer must be one of the following...", TAG));

                    print_all_manufacturers();

                    globalwritetoconsole("\npesticide/insecticide manufacturer:");

                    _maufacturer_name = Console.ReadLine();

                }

                while (!String.IsNullOrEmpty(_maufacturer_name))
                {
                    bool _does_entered_manufacturer_exist = check_if_manufacturer_exists(_maufacturer_name);

                    if (_does_entered_manufacturer_exist)
                    {
                        break;
                    }
                    else
                    {
                        _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("\nmanufacturer does not exist. must be one of the following...", TAG));

                        print_all_manufacturers();

                        globalwritetoconsole("\npesticide/insecticide manufacturer:");

                        _maufacturer_name = Console.ReadLine();
                    }
                }

                _pestinsecticidedto.pestinsecticide_manufacturer_id = get_manufacturer_id_given_name(_maufacturer_name);

                _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("\ndisease/pest must be one of the following...", TAG));

                print_all_diseasespests();

                globalwritetoconsole("\npesticide/insecticide disease/pest:");

                string _diseasepest_name = Console.ReadLine();

                while (String.IsNullOrEmpty(_diseasepest_name))
                {
                    _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("\npesticide/insecticide disease/pest cannot be null.", TAG));

                    _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("\ndisease/pest must be one of the following...", TAG));

                    print_all_diseasespests();

                    globalwritetoconsole("\npesticide/insecticide disease/pest:");

                    _diseasepest_name = Console.ReadLine();

                }

                while (!String.IsNullOrEmpty(_diseasepest_name))
                {
                    bool _does_entered_diseasepest_exist = check_if_diseasepest_exists(_diseasepest_name);

                    if (_does_entered_diseasepest_exist)
                    {
                        break;
                    }
                    else
                    {
                        _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("\ndisease/pest does not exist. must be one of the following...", TAG));

                        print_all_diseasespests();

                        globalwritetoconsole("\npesticide/insecticide disease/pest:");

                        _diseasepest_name = Console.ReadLine();
                    }
                }

                _pestinsecticidedto.pestinsecticide_crop_disease_id = get_diseasepest_id_given_name(_diseasepest_name);

                globalwritetoconsole("\npesticide/insecticide status: { active or inactive }");
                _pestinsecticidedto.pestinsecticide_status = Console.ReadLine();

                while (String.IsNullOrEmpty(_pestinsecticidedto.pestinsecticide_status))
                {
                    _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("pesticide/insecticide status cannot be null.", TAG));
                    globalwritetoconsole("\npesticide/insecticide status: { active or inactive }");
                    _pestinsecticidedto.pestinsecticide_status = Console.ReadLine();
                }

                while (!String.IsNullOrEmpty(_pestinsecticidedto.pestinsecticide_status))
                {
                    if (_pestinsecticidedto.pestinsecticide_status == "active")
                    {
                        //Indicates that the iteration has ended
                        break;
                    }
                    else if (_pestinsecticidedto.pestinsecticide_status == "inactive")
                    {
                        //Indicates that the iteration has ended
                        break;
                    }
                    else
                    {
                        _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("npesticide/insecticide status value must either be active or inactive.", TAG));
                        globalwritetoconsole("\npesticide/insecticide status: { active or inactive }");
                        _pestinsecticidedto.pestinsecticide_status = Console.ReadLine();
                        continue;
                    }

                }

                _pestinsecticidedto.created_date = dateTimenow;

                globalwritetoconsole("");
                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("pesticide/insecticide insert task execution running...", TAG));

                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("checking if pesticide/insecticide with name [ " + _pestinsecticidedto.pestinsecticide_name + " ] exists in " + DBContract.mssql + "....", TAG));

                bool _exists_in_mssql = mssqlapisingleton.getInstance(_notificationmessageEventname, _progressBarNotificationEventname).checkifpesticideinsecticideexists(_pestinsecticidedto.pestinsecticide_name, DBContract.getdefaultmssqlconnectionstring());

                if (!_exists_in_mssql)
                {
                    this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("pesticide/insecticide with name [ " + _pestinsecticidedto.pestinsecticide_name + " ] does not exists in " + DBContract.mssql + "\nproceeding with insertion....", TAG));

                    databasehelpersingleton.getInstance(_notificationmessageEventname, _progressBarNotificationEventname).savepestinsecticideinmssqldb(_pestinsecticidedto);
                }
                else
                {
                    this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("pesticide/insecticide with name [ " + _pestinsecticidedto.pestinsecticide_name + " ] exists in " + DBContract.mssql + ".", TAG));
                }

                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("checking if pesticide/insecticide with name [ " + _pestinsecticidedto.pestinsecticide_name + " ] exists in " + DBContract.sqlite + "....", TAG));

                bool _exists_in_sqlite = sqliteapisingleton.getInstance(_notificationmessageEventname).checkifpesticideinsecticideexists(_pestinsecticidedto.pestinsecticide_name, DBContract.getdefaultsqliteconnectionstring());

                if (!_exists_in_sqlite)
                {
                    this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("pesticide/insecticide with name [ " + _pestinsecticidedto.pestinsecticide_name + " ] does not exists in " + DBContract.sqlite + "\nproceeding with insertion....", TAG));

                    databasehelpersingleton.getInstance(_notificationmessageEventname, _progressBarNotificationEventname).savepestinsecticideinsqlitedb(_pestinsecticidedto);
                }
                else
                {
                    this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("pesticide/insecticide with name [ " + _pestinsecticidedto.pestinsecticide_name + " ] exists in " + DBContract.sqlite + ".", TAG));
                }

                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("checking if pesticide/insecticide with name [ " + _pestinsecticidedto.pestinsecticide_name + " ] exists in " + DBContract.mysql + "....", TAG));

                bool _exists_in_mysql = mysqlapisingleton.getInstance(_notificationmessageEventname).checkifpesticideinsecticideexists(_pestinsecticidedto.pestinsecticide_name, DBContract.getdefaultmysqlconnectionstring());

                if (!_exists_in_mysql)
                {
                    this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("pesticide/insecticide with name [ " + _pestinsecticidedto.pestinsecticide_name + " ] does not exists in " + DBContract.mysql + "\nproceeding with insertion....", TAG));

                    databasehelpersingleton.getInstance(_notificationmessageEventname, _progressBarNotificationEventname).savepestinsecticideinmysqldb(_pestinsecticidedto);
                }
                else
                {
                    this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("pesticide/insecticide with name [ " + _pestinsecticidedto.pestinsecticide_name + " ] exists in " + DBContract.mysql + ".", TAG));
                }

                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("checking if pesticide/insecticide with name [ " + _pestinsecticidedto.pestinsecticide_name + " ] exists in " + DBContract.postgresql + "....", TAG));

                bool _exists_in_postgresql = postgresqlapisingleton.getInstance(_notificationmessageEventname).checkifpesticideinsecticideexists(_pestinsecticidedto.pestinsecticide_name, DBContract.getdefaultpostgresqlconnectionstring());

                if (!_exists_in_postgresql)
                {
                    this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("pesticide/insecticide with name [ " + _pestinsecticidedto.pestinsecticide_name + " ] does not exists in " + DBContract.postgresql + "\nproceeding with insertion....", TAG));

                    databasehelpersingleton.getInstance(_notificationmessageEventname, _progressBarNotificationEventname).savepestinsecticideinpostgresqldb(_pestinsecticidedto);
                }
                else
                {
                    this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("pesticide/insecticide with name [ " + _pestinsecticidedto.pestinsecticide_name + " ] exists in " + DBContract.postgresql + ".", TAG));
                }

            }
            catch (Exception ex)
            {
                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs(ex.Message, TAG));
            }
        }

        public void listpestsinsecticidescliutils()
        {
            try
            {
                globalwritetoconsole("");
                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("fetch pesticides/insecticides list task running...", TAG));

                string query = DBContract.PESTSINSECTICIDES_SELECT_ALL_QUERY;

                DataTable mssql_dt = null;
                DataTable mysql_dt = null;
                DataTable sqlite_dt = null;
                DataTable postgresql_dt = null;

                mssql_dt = getallrecordsfrommssql(query);
                mysql_dt = getallrecordsfrommysql(query);
                sqlite_dt = getallrecordsfromsqlite(query);
                postgresql_dt = getallrecordsfrompostgresql(query);

                if (mssql_dt != null && mssql_dt.Rows.Count != 0)
                {
                    printtablelistseparator();
                    printpestsinsecticides(mssql_dt, datastoreconstants.mssql);
                }
                if (mysql_dt != null && mysql_dt.Rows.Count != 0)
                {
                    printpestsinsecticides(mysql_dt, datastoreconstants.mysql);
                }
                if (sqlite_dt != null && sqlite_dt.Rows.Count != 0)
                {
                    printpestsinsecticides(sqlite_dt, datastoreconstants.sqlite);
                }
                if (postgresql_dt != null && postgresql_dt.Rows.Count != 0)
                {
                    printpestsinsecticides(postgresql_dt, datastoreconstants.postgresql);
                }

            }
            catch (Exception ex)
            {
                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs(ex.Message, TAG));
            }
        }

        public void createsettingcliutils()
        {
            try
            {
                settingdto _settingdto = new settingdto();
                DateTime currentDate = DateTime.Now;
                String dateTimenow = currentDate.ToString("dd-MM-yyyy HH:mm:ss");

                globalwritetoconsole("");
                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("loading create setting interface...", TAG));

                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("provide detail for given fields...", TAG));

                globalwritetoconsole("setting name:");
                _settingdto.setting_name = Console.ReadLine();

                while (String.IsNullOrEmpty(_settingdto.setting_name))
                {
                    _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("setting name cannot be null.", TAG));
                    globalwritetoconsole("setting name:");
                    _settingdto.setting_name = Console.ReadLine();
                }

                globalwritetoconsole("\nsetting value:");
                _settingdto.setting_value = Console.ReadLine();

                while (String.IsNullOrEmpty(_settingdto.setting_value))
                {
                    _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("setting value cannot be null.", TAG));
                    globalwritetoconsole("\nsetting value:");
                    _settingdto.setting_value = Console.ReadLine();
                }

                globalwritetoconsole("\nsetting status: { active or inactive }");
                _settingdto.setting_status = Console.ReadLine();

                while (String.IsNullOrEmpty(_settingdto.setting_status))
                {
                    _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("setting status cannot be null.", TAG));
                    globalwritetoconsole("\nsetting status: { active or inactive }");
                    _settingdto.setting_status = Console.ReadLine();
                }

                while (!String.IsNullOrEmpty(_settingdto.setting_status))
                {
                    if (_settingdto.setting_status == "active")
                    {
                        //Indicates that the iteration has ended
                        break;
                    }
                    else if (_settingdto.setting_status == "inactive")
                    {
                        //Indicates that the iteration has ended
                        break;
                    }
                    else
                    {
                        _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("setting status value must either be active or inactive.", TAG));
                        globalwritetoconsole("\nsetting status: { active or inactive }");
                        _settingdto.setting_status = Console.ReadLine();
                        continue;
                    }

                }

                _settingdto.created_date = dateTimenow;

                globalwritetoconsole("");
                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("setting insert task execution running...", TAG));

                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("checking if setting with name [ " + _settingdto.setting_name + " ] exists in " + DBContract.mssql + "....", TAG));

                bool _exists_in_mssql = mssqlapisingleton.getInstance(_notificationmessageEventname, _progressBarNotificationEventname).checkifsettingexists(_settingdto.setting_name, DBContract.getdefaultmssqlconnectionstring());

                if (!_exists_in_mssql)
                {
                    this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("setting with name [ " + _settingdto.setting_name + " ] does not exists in " + DBContract.mssql + "\nproceeding with insertion....", TAG));

                    databasehelpersingleton.getInstance(_notificationmessageEventname, _progressBarNotificationEventname).savesettinginmssqldb(_settingdto);
                }
                else
                {
                    this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("setting with name [ " + _settingdto.setting_name + " ] exists in " + DBContract.mssql + ".", TAG));
                }

                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("checking if setting with name [ " + _settingdto.setting_name + " ] exists in " + DBContract.sqlite + "....", TAG));

                bool _exists_in_sqlite = sqliteapisingleton.getInstance(_notificationmessageEventname).checkifsettingexists(_settingdto.setting_name, DBContract.getdefaultsqliteconnectionstring());

                if (!_exists_in_sqlite)
                {
                    this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("setting with name [ " + _settingdto.setting_name + " ] does not exists in " + DBContract.sqlite + "\nproceeding with insertion....", TAG));

                    databasehelpersingleton.getInstance(_notificationmessageEventname, _progressBarNotificationEventname).savesettinginsqlitedb(_settingdto);
                }
                else
                {
                    this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("setting with name [ " + _settingdto.setting_name + " ] exists in " + DBContract.sqlite + ".", TAG));
                }

                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("checking if setting with name [ " + _settingdto.setting_name + " ] exists in " + DBContract.mysql + "....", TAG));

                bool _exists_in_mysql = mysqlapisingleton.getInstance(_notificationmessageEventname).checkifsettingexists(_settingdto.setting_name, DBContract.getdefaultmysqlconnectionstring());

                if (!_exists_in_mysql)
                {
                    this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("setting with name [ " + _settingdto.setting_name + " ] does not exists in " + DBContract.mysql + "\nproceeding with insertion....", TAG));

                    databasehelpersingleton.getInstance(_notificationmessageEventname, _progressBarNotificationEventname).savesettinginmysqldb(_settingdto);
                }
                else
                {
                    this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("setting with name [ " + _settingdto.setting_name + " ] exists in " + DBContract.mysql + ".", TAG));
                }

                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("checking if setting with name [ " + _settingdto.setting_name + " ] exists in " + DBContract.postgresql + "....", TAG));

                bool _exists_in_postgresql = postgresqlapisingleton.getInstance(_notificationmessageEventname).checkifsettingexists(_settingdto.setting_name, DBContract.getdefaultpostgresqlconnectionstring());

                if (!_exists_in_postgresql)
                {
                    this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("setting with name [ " + _settingdto.setting_name + " ] does not exists in " + DBContract.postgresql + "\nproceeding with insertion....", TAG));

                    databasehelpersingleton.getInstance(_notificationmessageEventname, _progressBarNotificationEventname).savesettinginpostgresqldb(_settingdto);
                }
                else
                {
                    this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("setting with name [ " + _settingdto.setting_name + " ] exists in " + DBContract.postgresql + ".", TAG));
                }

            }
            catch (Exception ex)
            {
                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs(ex.Message, TAG));
            }
        }

        public void listsettingscliutils()
        {
            try
            {
                globalwritetoconsole("");
                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("fetch settings list task running...", TAG));

                string query = DBContract.SETTINGS_SELECT_ALL_QUERY;

                DataTable mssql_dt = null;
                DataTable mysql_dt = null;
                DataTable sqlite_dt = null;
                DataTable postgresql_dt = null;

                mssql_dt = getallrecordsfrommssql(query);
                mysql_dt = getallrecordsfrommysql(query);
                sqlite_dt = getallrecordsfromsqlite(query);
                postgresql_dt = getallrecordsfrompostgresql(query);

                if (mssql_dt != null && mssql_dt.Rows.Count != 0)
                {
                    printtablelistseparator();
                    printsettings(mssql_dt, datastoreconstants.mssql);
                }
                if (mysql_dt != null && mysql_dt.Rows.Count != 0)
                {
                    printsettings(mysql_dt, datastoreconstants.mysql);
                }
                if (sqlite_dt != null && sqlite_dt.Rows.Count != 0)
                {
                    printsettings(sqlite_dt, datastoreconstants.sqlite);
                }
                if (postgresql_dt != null && postgresql_dt.Rows.Count != 0)
                {
                    printsettings(postgresql_dt, datastoreconstants.postgresql);
                }

            }
            catch (Exception ex)
            {
                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs(ex.Message, TAG));
            }
        }

        public void createcategorycliutils()
        {
            try
            {
                categorydto _categorydto = new categorydto();
                DateTime currentDate = DateTime.Now;
                String dateTimenow = currentDate.ToString("dd-MM-yyyy HH:mm:ss");

                globalwritetoconsole("");
                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("loading create category interface...", TAG));

                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("provide detail for given fields...", TAG));

                globalwritetoconsole("category name:");
                _categorydto.category_name = Console.ReadLine();

                while (String.IsNullOrEmpty(_categorydto.category_name))
                {
                    _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("category name cannot be null.", TAG));
                    globalwritetoconsole("category name:");
                    _categorydto.category_name = Console.ReadLine();
                }

                globalwritetoconsole("\ncategory status: { active or inactive }");
                _categorydto.category_status = Console.ReadLine();

                while (String.IsNullOrEmpty(_categorydto.category_status))
                {
                    _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("category status cannot be null.", TAG));
                    globalwritetoconsole("\ncategory status: { active or inactive }");
                    _categorydto.category_status = Console.ReadLine();
                }

                while (!String.IsNullOrEmpty(_categorydto.category_status))
                {
                    if (_categorydto.category_status == "active")
                    {
                        //Indicates that the iteration has ended
                        break;
                    }
                    else if (_categorydto.category_status == "inactive")
                    {
                        //Indicates that the iteration has ended
                        break;
                    }
                    else
                    {
                        _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("category status value must either be active or inactive.", TAG));
                        globalwritetoconsole("\ncategory status: { active or inactive }");
                        _categorydto.category_status = Console.ReadLine();
                        continue;
                    }

                }

                _categorydto.created_date = dateTimenow;

                globalwritetoconsole("");

                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("category insert task execution running.....", TAG));

                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("checking if category with name [ " + _categorydto.category_name + " ] exists in " + DBContract.mssql + "....", TAG));

                bool _exists_in_mssql = mssqlapisingleton.getInstance(_notificationmessageEventname, _progressBarNotificationEventname).checkifcategoryexists(_categorydto.category_name, DBContract.getdefaultmssqlconnectionstring());

                if (!_exists_in_mssql)
                {
                    this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("category with name [ " + _categorydto.category_name + " ] does not exists in " + DBContract.mssql + "\nproceeding with insertion....", TAG));

                    databasehelpersingleton.getInstance(_notificationmessageEventname, _progressBarNotificationEventname).savecategoryinmssqldb(_categorydto);
                }
                else
                {
                    this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("category with name [ " + _categorydto.category_name + " ] exists in " + DBContract.mssql + ".", TAG));
                }

                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("checking if category with name [ " + _categorydto.category_name + " ] exists in " + DBContract.sqlite + "....", TAG));

                bool _exists_in_sqlite = sqliteapisingleton.getInstance(_notificationmessageEventname).checkifcategoryexists(_categorydto.category_name, DBContract.getdefaultsqliteconnectionstring());

                if (!_exists_in_sqlite)
                {
                    this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("category with name [ " + _categorydto.category_name + " ] does not exists in " + DBContract.sqlite + "\nproceeding with insertion....", TAG));

                    databasehelpersingleton.getInstance(_notificationmessageEventname, _progressBarNotificationEventname).savecategoryinsqlitedb(_categorydto);
                }
                else
                {
                    this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("category with name [ " + _categorydto.category_name + " ] exists in " + DBContract.sqlite + ".", TAG));
                }

                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("checking if category with name [ " + _categorydto.category_name + " ] exists in " + DBContract.mysql + "....", TAG));

                bool _exists_in_mysql = mysqlapisingleton.getInstance(_notificationmessageEventname).checkifcategoryexists(_categorydto.category_name, DBContract.getdefaultmysqlconnectionstring());

                if (!_exists_in_mysql)
                {
                    this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("category with name [ " + _categorydto.category_name + " ] does not exists in " + DBContract.mysql + "\nproceeding with insertion....", TAG));

                    databasehelpersingleton.getInstance(_notificationmessageEventname, _progressBarNotificationEventname).savecategoryinmysqldb(_categorydto);
                }
                else
                {
                    this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("category with name [ " + _categorydto.category_name + " ] exists in " + DBContract.mysql + ".", TAG));
                }

                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("checking if category with name [ " + _categorydto.category_name + " ] exists in " + DBContract.postgresql + "....", TAG));

                bool _exists_in_postgresql = postgresqlapisingleton.getInstance(_notificationmessageEventname).checkifcategoryexists(_categorydto.category_name, DBContract.getdefaultpostgresqlconnectionstring());

                if (!_exists_in_postgresql)
                {
                    this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("category with name [ " + _categorydto.category_name + " ] does not exists in " + DBContract.postgresql + "\nproceeding with insertion....", TAG));

                    databasehelpersingleton.getInstance(_notificationmessageEventname, _progressBarNotificationEventname).savecategoryinpostgresqldb(_categorydto);
                }
                else
                {
                    this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("category with name [ " + _categorydto.category_name + " ] exists in " + DBContract.postgresql + ".", TAG));
                }

            }
            catch (Exception ex)
            {
                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs(ex.Message, TAG));
            }
        }

        public void listcategoriescliutils()
        {
            try
            {
                globalwritetoconsole("");
                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("fetch categories list task running...", TAG));

                string query = DBContract.CATEGORIES_SELECT_ALL_QUERY;

                DataTable mssql_dt = null;
                DataTable mysql_dt = null;
                DataTable sqlite_dt = null;
                DataTable postgresql_dt = null;

                mssql_dt = getallrecordsfrommssql(query);
                mysql_dt = getallrecordsfrommysql(query);
                sqlite_dt = getallrecordsfromsqlite(query);
                postgresql_dt = getallrecordsfrompostgresql(query);

                if (mssql_dt != null && mssql_dt.Rows.Count != 0)
                {

                    _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs(DBContract.mssql + " records count: " + mssql_dt.Rows.Count, TAG));

                    printtablelistseparator();
                    printcategories(mssql_dt, datastoreconstants.mssql);
                }
                if (mysql_dt != null && mysql_dt.Rows.Count != 0)
                {

                    _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs(DBContract.mysql + " records count: " + mysql_dt.Rows.Count, TAG));

                    printcategories(mysql_dt, datastoreconstants.mysql);
                }
                if (sqlite_dt != null && sqlite_dt.Rows.Count != 0)
                {

                    _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs(DBContract.sqlite + " records count: " + sqlite_dt.Rows.Count, TAG));

                    printcategories(sqlite_dt, datastoreconstants.sqlite);
                }
                if (postgresql_dt != null && postgresql_dt.Rows.Count != 0)
                {

                    _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs(DBContract.postgresql + " records count: " + postgresql_dt.Rows.Count, TAG));

                    printcategories(postgresql_dt, datastoreconstants.postgresql);
                }

            }
            catch (Exception ex)
            {
                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs(ex.Message, TAG));
            }
        }

        void persistconsolecontenttofile()
        {
            try
            {
                savetotxt();
                savetoxml();
                savetoxmlfluentsyntax();
            }
            catch (Exception ex)
            {
                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs(ex.Message, TAG));
            }
        }

        public void savetotxt()
        {
            try
            {
                if (!Directory.Exists(_txtpathfolder))
                    Directory.CreateDirectory(_txtpathfolder);

                if (!File.Exists(_txt_loga_file))
                    File.Create(_txt_loga_file).Close();

                using (FileStream _fileStream = new FileStream(_txt_loga_file, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
                {

                    StreamWriter sw = new StreamWriter(_fileStream);

                    foreach (var _log in _lstnotificationdto)
                    {

                        sw.WriteLine("..............................");
                        sw.WriteLine("TAG: " + _log.TAG);
                        sw.WriteLine("NOTIFICATION_MESSAGE: " + _log._notification_message);
                        sw.WriteLine("..............................\n");

                    }

                    //sw.Close(); This will close ms and when we try to use ms later it will cause an exception
                    sw.Flush();
                }

            }
            catch (Exception ex)
            {
                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs(ex.Message, TAG));
            }

        }

        public void savetoxml()
        {
            try
            {
                if (!Directory.Exists(_xmlpathfolder))
                    Directory.CreateDirectory(_xmlpathfolder);

                if (!File.Exists(_xml_loga_file))
                    File.Create(_xml_loga_file).Close();

                using (FileStream _fileStream = new FileStream(_xml_loga_file, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
                { // you can write to the fileStream

                    var xmlDoc = new XmlDocument();
                    //			xml.Load(_fileStream);
                    xmlDoc.CreateXmlDeclaration("1.0", "utf-8", "yes");
                    var root = xmlDoc.CreateElement("com.tech.nyax");
                    // Creates an attribute, so the element will now be "<element attribute='value' />"
                    root.SetAttribute("APPLICATION_NAME", Application.ProductName);
                    root.SetAttribute("APPLICATION_VERSION", Application.ProductVersion);
                    // All XML documents must have one, and only one, root element
                    xmlDoc.AppendChild(root);
                    // Adding data to an XML document
                    foreach (var _log in _lstnotificationdto)
                    {
                        var child = xmlDoc.CreateElement("LOG");
                        child.SetAttribute("TAG", _log.TAG);
                        child.SetAttribute("CREATED_DATE", _log._created_datetime);
                        child.SetAttribute("NOTIFICATION_MESSAGE", _log._notification_message);
                        //child.InnerText = _log._notification_message;
                        // Don't forget to add the new value to the current document!
                        root.AppendChild(child);
                    }

                    // Displays the XML document in the screen; 
                    // optionally can be saved to a file
                    xmlDoc.Save(_fileStream);

                }

            }
            catch (Exception ex)
            {
                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs(ex.Message, TAG));
            }

        }

        public void savetoxmlfluentsyntax()
        {
            try
            {
                if (!Directory.Exists(_xmlpathfolder))
                    Directory.CreateDirectory(_xmlpathfolder);

                if (!File.Exists(_xml_loga_file_fluentsyntax))
                    File.Create(_xml_loga_file_fluentsyntax).Close();

                using (FileStream _fileStream = new FileStream(_xml_loga_file_fluentsyntax, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
                { // you can write to the fileStream

                    XNamespace xns = "http://com.tech.nyax";
                    XDeclaration xDeclaration = new XDeclaration("1.0", "utf-8", "yes");
                    XDocument xDoc = new XDocument(xDeclaration);
                    XElement xRoot = new XElement(xns + "com.tech.nyax");
                    xDoc.Add(xRoot);

                    foreach (var _log in _lstnotificationdto)
                    {
                        XElement xelparent = new XElement(xns + "LOG");
                        XAttribute xAttribute1 = new XAttribute("APPLICATION_NAME", Application.ProductName);
                        XAttribute xAttribute2 = new XAttribute("APPLICATION_VERSION", Application.ProductVersion);
                        xelparent.Add(xAttribute1);
                        xelparent.Add(xAttribute2);
                        XElement xelchild1 = new XElement(xns + "TAG", _log.TAG);
                        XElement xelchild2 = new XElement(xns + "CREATED_DATE", _log._created_datetime);
                        XElement xelchild3 = new XElement(xns + "NOTIFICATION_MESSAGE", _log._notification_message);
                        xelparent.Add(xelchild1);
                        xelparent.Add(xelchild2);
                        xelparent.Add(xelchild3);
                        xRoot.Add(xelparent);
                    }

                    xDoc.Save(_fileStream);

                }

            }
            catch (Exception ex)
            {
                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs(ex.Message, TAG));
            }
        }

        string get_category_name_given_id(string dto_id)
        {
            try
            {

                DataTable mssql_dt = null;
                DataTable mysql_dt = null;
                DataTable sqlite_dt = null;
                DataTable dt = null;

                string query = DBContract.CATEGORIES_SELECT_ALL_QUERY;

                mssql_dt = getallrecordsfrommssql(query);

                mysql_dt = getallrecordsfrommysql(query);

                sqlite_dt = getallrecordsfromsqlite(query);

                if (mssql_dt != null)
                {
                    dt = mssql_dt;
                    _working_db = "mssql";
                }
                if (mssql_dt == null && mysql_dt != null)
                {
                    dt = mysql_dt;
                    _working_db = "mysql";
                }
                if (mssql_dt == null && mysql_dt == null && sqlite_dt != null)
                {
                    dt = sqlite_dt;
                    _working_db = "sqlite";
                }

                if (dt == null) return null;

                var _recordscount = dt.Rows.Count;

                for (int i = 0; i < _recordscount; i++)
                {

                    categorydto _categorydto = utilzsingleton.getInstance(_notificationmessageEventname).buildcategorydtogivendatatable(dt, i);

                    var _record_from_server = Convert.ToString(dt.Rows[i]["category_id"]);

                    if (dto_id == _record_from_server)
                    {
                        return Convert.ToString(dt.Rows[i]["category_name"]);
                    }

                }
                return null;
            }
            catch (Exception ex)
            {
                _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs(ex.Message, TAG));
                return null;
            }
        }

        string get_manufacturer_name_given_id(string dto_id)
        {
            try
            {

                DataTable mssql_dt = null;
                DataTable mysql_dt = null;
                DataTable sqlite_dt = null;
                DataTable dt = null;

                string query = DBContract.MANUFACTURERS_SELECT_ALL_QUERY;

                mssql_dt = getallrecordsfrommssql(query);

                mysql_dt = getallrecordsfrommysql(query);

                sqlite_dt = getallrecordsfromsqlite(query);

                if (mssql_dt != null)
                {
                    dt = mssql_dt;
                    _working_db = "mssql";
                }
                if (mssql_dt == null && mysql_dt != null)
                {
                    dt = mysql_dt;
                    _working_db = "mysql";
                }
                if (mssql_dt == null && mysql_dt == null && sqlite_dt != null)
                {
                    dt = sqlite_dt;
                    _working_db = "sqlite";
                }

                if (dt == null) return null;

                var _recordscount = dt.Rows.Count;

                for (int i = 0; i < _recordscount; i++)
                {

                    manufacturerdto _manufacturerdto = utilzsingleton.getInstance(_notificationmessageEventname).buildmanufacturerdtogivendatatable(dt, i);

                    var _record_from_server = Convert.ToString(dt.Rows[i]["manufacturer_id"]);

                    if (dto_id == _record_from_server)
                    {
                        return Convert.ToString(dt.Rows[i]["manufacturer_name"]);
                    }

                }
                return null;
            }
            catch (Exception ex)
            {
                _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs(ex.Message, TAG));
                return null;
            }
        }

        string get_diseasepest_name_given_id(string dto_id)
        {
            try
            {

                DataTable mssql_dt = null;
                DataTable mysql_dt = null;
                DataTable sqlite_dt = null;
                DataTable dt = null;

                string query = DBContract.CROPS_DISEASES_SELECT_ALL_QUERY;

                mssql_dt = getallrecordsfrommssql(query);

                mysql_dt = getallrecordsfrommysql(query);

                sqlite_dt = getallrecordsfromsqlite(query);

                if (mssql_dt != null)
                {
                    dt = mssql_dt;
                    _working_db = "mssql";
                }
                if (mssql_dt == null && mysql_dt != null)
                {
                    dt = mysql_dt;
                    _working_db = "mysql";
                }
                if (mssql_dt == null && mysql_dt == null && sqlite_dt != null)
                {
                    dt = sqlite_dt;
                    _working_db = "sqlite";
                }

                if (dt == null) return null;

                var _recordscount = dt.Rows.Count;

                for (int i = 0; i < _recordscount; i++)
                {

                    cropdiseasedto _cropdiseasedto = utilzsingleton.getInstance(_notificationmessageEventname).buildcropdiseasedtogivendatatable(dt, i);

                    var _record_from_server = Convert.ToString(dt.Rows[i]["crop_disease_id"]);

                    if (dto_id == _record_from_server)
                    {
                        return Convert.ToString(dt.Rows[i]["crop_disease_name"]);
                    }

                }
                return null;
            }
            catch (Exception ex)
            {
                _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs(ex.Message, TAG));
                return null;
            }
        }

        string get_crop_name_given_id(string dto_id)
        {
            try
            {

                DataTable mssql_dt = null;
                DataTable mysql_dt = null;
                DataTable sqlite_dt = null;
                DataTable dt = null;

                string query = DBContract.CROPS_SELECT_ALL_QUERY;

                mssql_dt = getallrecordsfrommssql(query);

                mysql_dt = getallrecordsfrommysql(query);

                sqlite_dt = getallrecordsfromsqlite(query);

                if (mssql_dt != null)
                {
                    dt = mssql_dt;
                    _working_db = "mssql";
                }
                if (mssql_dt == null && mysql_dt != null)
                {
                    dt = mysql_dt;
                    _working_db = "mysql";
                }
                if (mssql_dt == null && mysql_dt == null && sqlite_dt != null)
                {
                    dt = sqlite_dt;
                    _working_db = "sqlite";
                }

                if (dt == null) return null;

                var _recordscount = dt.Rows.Count;

                for (int i = 0; i < _recordscount; i++)
                {

                    cropdto _cropdto = utilzsingleton.getInstance(_notificationmessageEventname).buildcropdtogivendatatable(dt, i);

                    var _record_from_server = Convert.ToString(dt.Rows[i]["crop_id"]);

                    if (dto_id == _record_from_server)
                    {
                        return Convert.ToString(dt.Rows[i]["crop_name"]);
                    }

                }
                return null;
            }
            catch (Exception ex)
            {
                _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs(ex.Message, TAG));
                return null;
            }
        }

        void print_all_categories()
        {
            try
            {

                DataTable mssql_dt = null;
                DataTable mysql_dt = null;
                DataTable sqlite_dt = null;
                DataTable dt = null;

                string query = DBContract.CATEGORIES_SELECT_ALL_QUERY;

                mssql_dt = getallrecordsfrommssql(query);

                mysql_dt = getallrecordsfrommysql(query);

                sqlite_dt = getallrecordsfromsqlite(query);

                if (mssql_dt != null)
                {
                    dt = mssql_dt;
                    _working_db = "mssql";
                }
                if (mssql_dt == null && mysql_dt != null)
                {
                    dt = mysql_dt;
                    _working_db = "mysql";
                }
                if (mssql_dt == null && mysql_dt == null && sqlite_dt != null)
                {
                    dt = sqlite_dt;
                    _working_db = "sqlite";
                }

                if (dt == null) return;

                var _recordscount = dt.Rows.Count;

                for (int i = 0; i < _recordscount; i++)
                {

                    categorydto _dto = utilzsingleton.getInstance(_notificationmessageEventname).buildcategorydtogivendatatable(dt, i);

                    var _dto_name = Convert.ToString(dt.Rows[i]["category_name"]);

                    if (i == _recordscount)
                    {
                        globalwritetoconsole(_dto_name);
                    }
                    else
                    {
                        globalwritetoconsole(_dto_name + ",");
                    }

                }
            }
            catch (Exception ex)
            {
                _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs(ex.Message, TAG));
            }
        }

        bool check_if_category_exists(string dto_name)
        {
            bool _exists = false;
            try
            {

                DataTable mssql_dt = null;
                DataTable mysql_dt = null;
                DataTable sqlite_dt = null;
                DataTable dt = null;

                string query = DBContract.CATEGORIES_SELECT_ALL_QUERY;

                mssql_dt = getallrecordsfrommssql(query);

                mysql_dt = getallrecordsfrommysql(query);

                sqlite_dt = getallrecordsfromsqlite(query);

                if (mssql_dt != null)
                {
                    dt = mssql_dt;
                    _working_db = "mssql";
                }
                if (mssql_dt == null && mysql_dt != null)
                {
                    dt = mysql_dt;
                    _working_db = "mysql";
                }
                if (mssql_dt == null && mysql_dt == null && sqlite_dt != null)
                {
                    dt = sqlite_dt;
                    _working_db = "sqlite";
                }

                if (dt == null) return _exists;

                var _recordscount = dt.Rows.Count;

                for (int i = 0; i < _recordscount; i++)
                {

                    categorydto _dto = utilzsingleton.getInstance(_notificationmessageEventname).buildcategorydtogivendatatable(dt, i);

                    var _record_from_server = Convert.ToString(dt.Rows[i]["category_name"]);

                    if (dto_name == _record_from_server)
                    {
                        _exists = true;
                        return _exists;
                    }

                }
                return _exists;
            }
            catch (Exception ex)
            {
                _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs(ex.Message, TAG));
                _exists = false;
                return _exists;
            }
        }

        string get_category_id_given_name(string dto_name)
        {
            try
            {

                DataTable mssql_dt = null;
                DataTable mysql_dt = null;
                DataTable sqlite_dt = null;
                DataTable dt = null;

                string query = DBContract.CATEGORIES_SELECT_ALL_QUERY;

                mssql_dt = getallrecordsfrommssql(query);

                mysql_dt = getallrecordsfrommysql(query);

                sqlite_dt = getallrecordsfromsqlite(query);

                if (mssql_dt != null)
                {
                    dt = mssql_dt;
                    _working_db = "mssql";
                }
                if (mssql_dt == null && mysql_dt != null)
                {
                    dt = mysql_dt;
                    _working_db = "mysql";
                }
                if (mssql_dt == null && mysql_dt == null && sqlite_dt != null)
                {
                    dt = sqlite_dt;
                    _working_db = "sqlite";
                }

                if (dt == null) return null;

                var _recordscount = dt.Rows.Count;

                for (int i = 0; i < _recordscount; i++)
                {

                    categorydto _dto = utilzsingleton.getInstance(_notificationmessageEventname).buildcategorydtogivendatatable(dt, i);

                    var _record_from_server = Convert.ToString(dt.Rows[i]["category_name"]);

                    if (dto_name == _record_from_server)
                    {
                        return Convert.ToString(dt.Rows[i]["category_id"]);
                    }

                }
                return null;
            }
            catch (Exception ex)
            {
                _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs(ex.Message, TAG));
                return null;
            }
        }

        void print_all_manufacturers()
        {
            try
            {

                DataTable mssql_dt = null;
                DataTable mysql_dt = null;
                DataTable sqlite_dt = null;
                DataTable dt = null;

                string query = DBContract.MANUFACTURERS_SELECT_ALL_QUERY;

                mssql_dt = getallrecordsfrommssql(query);

                mysql_dt = getallrecordsfrommysql(query);

                sqlite_dt = getallrecordsfromsqlite(query);

                if (mssql_dt != null)
                {
                    dt = mssql_dt;
                    _working_db = "mssql";
                }
                if (mssql_dt == null && mysql_dt != null)
                {
                    dt = mysql_dt;
                    _working_db = "mysql";
                }
                if (mssql_dt == null && mysql_dt == null && sqlite_dt != null)
                {
                    dt = sqlite_dt;
                    _working_db = "sqlite";
                }

                if (dt == null) return;

                var _recordscount = dt.Rows.Count;

                for (int i = 0; i < _recordscount; i++)
                {

                    manufacturerdto _manufacturerdto = utilzsingleton.getInstance(_notificationmessageEventname).buildmanufacturerdtogivendatatable(dt, i);

                    var _manufacturer_name = Convert.ToString(dt.Rows[i]["manufacturer_name"]);

                    if (i == _recordscount)
                    {
                        globalwritetoconsole(_manufacturer_name);
                    }
                    else
                    {
                        globalwritetoconsole(_manufacturer_name + ",");
                    }

                }
            }
            catch (Exception ex)
            {
                _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs(ex.Message, TAG));
            }
        }

        void print_all_crops()
        {
            try
            {

                DataTable mssql_dt = null;
                DataTable mysql_dt = null;
                DataTable sqlite_dt = null;
                DataTable dt = null;

                string query = DBContract.CROPS_SELECT_ALL_QUERY;

                mssql_dt = getallrecordsfrommssql(query);

                mysql_dt = getallrecordsfrommysql(query);

                sqlite_dt = getallrecordsfromsqlite(query);

                if (mssql_dt != null)
                {
                    dt = mssql_dt;
                    _working_db = "mssql";
                }
                if (mssql_dt == null && mysql_dt != null)
                {
                    dt = mysql_dt;
                    _working_db = "mysql";
                }
                if (mssql_dt == null && mysql_dt == null && sqlite_dt != null)
                {
                    dt = sqlite_dt;
                    _working_db = "sqlite";
                }

                if (dt == null) return;

                var _recordscount = dt.Rows.Count;

                for (int i = 0; i < _recordscount; i++)
                {

                    cropdto _cropdto = utilzsingleton.getInstance(_notificationmessageEventname).buildcropdtogivendatatable(dt, i);

                    var _crop_name = Convert.ToString(dt.Rows[i]["crop_name"]);

                    if (i == _recordscount)
                    {
                        globalwritetoconsole(_crop_name);
                    }
                    else
                    {
                        globalwritetoconsole(_crop_name + ",");
                    }

                }
            }
            catch (Exception ex)
            {
                _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs(ex.Message, TAG));
            }
        }

        bool check_if_manufacturer_exists(string dto_name)
        {
            bool _exists = false;
            try
            {

                DataTable mssql_dt = null;
                DataTable mysql_dt = null;
                DataTable sqlite_dt = null;
                DataTable dt = null;

                string query = DBContract.MANUFACTURERS_SELECT_ALL_QUERY;

                mssql_dt = getallrecordsfrommssql(query);

                mysql_dt = getallrecordsfrommysql(query);

                sqlite_dt = getallrecordsfromsqlite(query);

                if (mssql_dt != null)
                {
                    dt = mssql_dt;
                    _working_db = "mssql";
                }
                if (mssql_dt == null && mysql_dt != null)
                {
                    dt = mysql_dt;
                    _working_db = "mysql";
                }
                if (mssql_dt == null && mysql_dt == null && sqlite_dt != null)
                {
                    dt = sqlite_dt;
                    _working_db = "sqlite";
                }

                if (dt == null) return _exists;

                var _recordscount = dt.Rows.Count;

                for (int i = 0; i < _recordscount; i++)
                {

                    manufacturerdto _manufacturerdto = utilzsingleton.getInstance(_notificationmessageEventname).buildmanufacturerdtogivendatatable(dt, i);

                    var _record_from_server = Convert.ToString(dt.Rows[i]["manufacturer_name"]);

                    if (dto_name == _record_from_server)
                    {
                        _exists = true;
                        return _exists;
                    }

                }
                return _exists;
            }
            catch (Exception ex)
            {
                _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs(ex.Message, TAG));
                _exists = false;
                return _exists;
            }
        }

        bool check_if_crop_exists(string dto_name)
        {
            bool _exists = false;
            try
            {

                DataTable mssql_dt = null;
                DataTable mysql_dt = null;
                DataTable sqlite_dt = null;
                DataTable dt = null;

                string query = DBContract.CROPS_SELECT_ALL_QUERY;

                mssql_dt = getallrecordsfrommssql(query);

                mysql_dt = getallrecordsfrommysql(query);

                sqlite_dt = getallrecordsfromsqlite(query);

                if (mssql_dt != null)
                {
                    dt = mssql_dt;
                    _working_db = "mssql";
                }
                if (mssql_dt == null && mysql_dt != null)
                {
                    dt = mysql_dt;
                    _working_db = "mysql";
                }
                if (mssql_dt == null && mysql_dt == null && sqlite_dt != null)
                {
                    dt = sqlite_dt;
                    _working_db = "sqlite";
                }

                if (dt == null) return _exists;

                var _recordscount = dt.Rows.Count;

                for (int i = 0; i < _recordscount; i++)
                {

                    cropdto _cropdto = utilzsingleton.getInstance(_notificationmessageEventname).buildcropdtogivendatatable(dt, i);

                    var _record_from_server = Convert.ToString(dt.Rows[i]["crop_name"]);

                    if (dto_name == _record_from_server)
                    {
                        _exists = true;
                        return _exists;
                    }

                }
                return _exists;
            }
            catch (Exception ex)
            {
                _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs(ex.Message, TAG));
                _exists = false;
                return _exists;
            }
        }

        string get_manufacturer_id_given_name(string dto_name)
        {
            try
            {

                DataTable mssql_dt = null;
                DataTable mysql_dt = null;
                DataTable sqlite_dt = null;
                DataTable dt = null;

                string query = DBContract.MANUFACTURERS_SELECT_ALL_QUERY;

                mssql_dt = getallrecordsfrommssql(query);

                mysql_dt = getallrecordsfrommysql(query);

                sqlite_dt = getallrecordsfromsqlite(query);

                if (mssql_dt != null)
                {
                    dt = mssql_dt;
                    _working_db = "mssql";
                }
                if (mssql_dt == null && mysql_dt != null)
                {
                    dt = mysql_dt;
                    _working_db = "mysql";
                }
                if (mssql_dt == null && mysql_dt == null && sqlite_dt != null)
                {
                    dt = sqlite_dt;
                    _working_db = "sqlite";
                }

                if (dt == null) return null;

                var _recordscount = dt.Rows.Count;

                for (int i = 0; i < _recordscount; i++)
                {

                    manufacturerdto _manufacturerdto = utilzsingleton.getInstance(_notificationmessageEventname).buildmanufacturerdtogivendatatable(dt, i);

                    var _record_from_server = Convert.ToString(dt.Rows[i]["manufacturer_name"]);

                    if (dto_name == _record_from_server)
                    {
                        return Convert.ToString(dt.Rows[i]["manufacturer_id"]);
                    }

                }
                return null;
            }
            catch (Exception ex)
            {
                _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs(ex.Message, TAG));
                return null;
            }
        }

        string get_crop_id_given_name(string dto_name)
        {
            try
            {

                DataTable mssql_dt = null;
                DataTable mysql_dt = null;
                DataTable sqlite_dt = null;
                DataTable dt = null;

                string query = DBContract.CROPS_SELECT_ALL_QUERY;

                mssql_dt = getallrecordsfrommssql(query);

                mysql_dt = getallrecordsfrommysql(query);

                sqlite_dt = getallrecordsfromsqlite(query);

                if (mssql_dt != null)
                {
                    dt = mssql_dt;
                    _working_db = "mssql";
                }
                if (mssql_dt == null && mysql_dt != null)
                {
                    dt = mysql_dt;
                    _working_db = "mysql";
                }
                if (mssql_dt == null && mysql_dt == null && sqlite_dt != null)
                {
                    dt = sqlite_dt;
                    _working_db = "sqlite";
                }

                if (dt == null) return null;

                var _recordscount = dt.Rows.Count;

                for (int i = 0; i < _recordscount; i++)
                {

                    cropdto _cropdto = utilzsingleton.getInstance(_notificationmessageEventname).buildcropdtogivendatatable(dt, i);

                    var _record_from_server = Convert.ToString(dt.Rows[i]["crop_name"]);

                    if (dto_name == _record_from_server)
                    {
                        return Convert.ToString(dt.Rows[i]["crop_id"]);
                    }

                }
                return null;
            }
            catch (Exception ex)
            {
                _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs(ex.Message, TAG));
                return null;
            }
        }

        void print_all_diseasespests()
        {
            try
            {

                DataTable mssql_dt = null;
                DataTable mysql_dt = null;
                DataTable sqlite_dt = null;
                DataTable dt = null;

                string query = DBContract.CROPS_DISEASES_SELECT_ALL_QUERY;

                mssql_dt = getallrecordsfrommssql(query);

                mysql_dt = getallrecordsfrommysql(query);

                sqlite_dt = getallrecordsfromsqlite(query);

                if (mssql_dt != null)
                {
                    dt = mssql_dt;
                    _working_db = "mssql";
                }
                if (mssql_dt == null && mysql_dt != null)
                {
                    dt = mysql_dt;
                    _working_db = "mysql";
                }
                if (mssql_dt == null && mysql_dt == null && sqlite_dt != null)
                {
                    dt = sqlite_dt;
                    _working_db = "sqlite";
                }

                if (dt == null) return;

                var _recordscount = dt.Rows.Count;

                for (int i = 0; i < _recordscount; i++)
                {

                    cropdiseasedto _dto = utilzsingleton.getInstance(_notificationmessageEventname).buildcropdiseasedtogivendatatable(dt, i);

                    var _diseasepest_name = Convert.ToString(dt.Rows[i]["crop_disease_name"]);

                    if (i == _recordscount)
                    {
                        globalwritetoconsole(_diseasepest_name);
                    }
                    else
                    {
                        globalwritetoconsole(_diseasepest_name + ",");
                    }

                }
            }
            catch (Exception ex)
            {
                _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs(ex.Message, TAG));
            }
        }

        bool check_if_diseasepest_exists(string dto_name)
        {
            bool _exists = false;
            try
            {

                DataTable mssql_dt = null;
                DataTable mysql_dt = null;
                DataTable sqlite_dt = null;
                DataTable dt = null;

                string query = DBContract.CROPS_DISEASES_SELECT_ALL_QUERY;

                mssql_dt = getallrecordsfrommssql(query);

                mysql_dt = getallrecordsfrommysql(query);

                sqlite_dt = getallrecordsfromsqlite(query);

                if (mssql_dt != null)
                {
                    dt = mssql_dt;
                    _working_db = "mssql";
                }
                if (mssql_dt == null && mysql_dt != null)
                {
                    dt = mysql_dt;
                    _working_db = "mysql";
                }
                if (mssql_dt == null && mysql_dt == null && sqlite_dt != null)
                {
                    dt = sqlite_dt;
                    _working_db = "sqlite";
                }

                if (dt == null) return _exists;

                var _recordscount = dt.Rows.Count;

                for (int i = 0; i < _recordscount; i++)
                {

                    cropdiseasedto _dto = utilzsingleton.getInstance(_notificationmessageEventname).buildcropdiseasedtogivendatatable(dt, i);

                    var _record_from_server = Convert.ToString(dt.Rows[i]["crop_disease_name"]);

                    if (dto_name == _record_from_server)
                    {
                        _exists = true;
                        return _exists;
                    }

                }
                return _exists;
            }
            catch (Exception ex)
            {
                _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs(ex.Message, TAG));
                _exists = false;
                return _exists;
            }
        }

        string get_diseasepest_id_given_name(string dto_name)
        {
            try
            {

                DataTable mssql_dt = null;
                DataTable mysql_dt = null;
                DataTable sqlite_dt = null;
                DataTable dt = null;

                string query = DBContract.CROPS_DISEASES_SELECT_ALL_QUERY;

                mssql_dt = getallrecordsfrommssql(query);

                mysql_dt = getallrecordsfrommysql(query);

                sqlite_dt = getallrecordsfromsqlite(query);

                if (mssql_dt != null)
                {
                    dt = mssql_dt;
                    _working_db = "mssql";
                }
                if (mssql_dt == null && mysql_dt != null)
                {
                    dt = mysql_dt;
                    _working_db = "mysql";
                }
                if (mssql_dt == null && mysql_dt == null && sqlite_dt != null)
                {
                    dt = sqlite_dt;
                    _working_db = "sqlite";
                }

                if (dt == null) return null;

                var _recordscount = dt.Rows.Count;

                for (int i = 0; i < _recordscount; i++)
                {

                    cropdiseasedto _dto = utilzsingleton.getInstance(_notificationmessageEventname).buildcropdiseasedtogivendatatable(dt, i);

                    var _record_from_server = Convert.ToString(dt.Rows[i]["crop_disease_name"]);

                    if (dto_name == _record_from_server)
                    {
                        return Convert.ToString(dt.Rows[i]["crop_disease_id"]);
                    }

                }
                return null;
            }
            catch (Exception ex)
            {
                _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs(ex.Message, TAG));
                return null;
            }
        }

        void showversioninfocliutils()
        {
            try
            {
                printtablelistseparator();

                StringBuilder sb = new StringBuilder();
                sb.Append("\tProduct Name [ " + Application.ProductName + " ]");
                sb.Append(Environment.NewLine);
                sb.Append("\tCopyright @ [ " + DateTime.Now.Year + " ]");
                sb.Append(Environment.NewLine);
                sb.Append("\tProduct Version [ " + Application.ProductVersion + " ]");
                sb.Append(Environment.NewLine);
                sb.Append("\tCompany Name [ " + Application.CompanyName + " ]");

                globalwritetoconsole(sb.ToString());

                printtablelistseparator();

            }
            catch (Exception ex)
            {
                _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs(ex.Message, TAG));
            }
        }

        void startnewconsolecliutils()
        {
            try
            {
                string base_dir = Environment.CurrentDirectory;
                string process_name = "nthareneapp.exe";
                string full_process_name_with_path = base_dir + @"\" + process_name;

                string _process_path = @full_process_name_with_path;

                var _process_info = Process.Start(_process_path);

                globalwritetoconsole(String.Format("successfully started process [ {0} ] with id [ {1} ] at [ {2} ] took [ {3} ] seconds.", _process_info.StartInfo.FileName, _process_info.Id, _process_info.StartTime, _process_info.TotalProcessorTime));

            }
            catch (Exception ex)
            {
                _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs(ex.Message, TAG));
            }
        }
        void showconnectionsinfocliutils()
        {
            try
            {
                checkmssqlconnectionstate();
                checkmysqlconnectionstate();
                checkpostgresqlconnectionstate();
                checksqliteconnectionstate();
            }
            catch (Exception ex)
            {
                _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs(ex.Message, TAG));
            }
        }
        void checkmssqlconnectionstate()
        {
            try
            {
                _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("checking mssql connection state..."));

                responsedto _responsedto = mssqlapisingleton.getInstance(_notificationmessageEventname, _progressBarNotificationEventname).checkmssqlconnectionstate();

                if (_responsedto.isresponseresultsuccessful)
                {
                    _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs(String.Format("{0}", _responsedto.responsesuccessmessage)));

                }
                else
                {
                    _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs(String.Format("{0}", _responsedto.responseerrormessage)));

                }

            }
            catch (Exception ex)
            {
                _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs(ex.Message, TAG));
            }
        }
        void checkmysqlconnectionstate()
        {
            try
            {
                _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("checking mysql connection state..."));

                responsedto _responsedto = mysqlapisingleton.getInstance(_notificationmessageEventname).checkmysqlconnectionstate();

                if (_responsedto.isresponseresultsuccessful)
                {
                    _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs(String.Format("{0}", _responsedto.responsesuccessmessage)));

                }
                else
                {
                    _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs(String.Format("{0}", _responsedto.responseerrormessage)));

                }

            }
            catch (Exception ex)
            {
                _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs(ex.Message, TAG));
            }
        }
        void checkpostgresqlconnectionstate()
        {
            try
            {
                _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("checking postgresql connection state..."));

                responsedto _responsedto = postgresqlapisingleton.getInstance(_notificationmessageEventname).checkpostgresqlconnectionstate();

                if (_responsedto.isresponseresultsuccessful)
                {
                    _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs(String.Format("{0}", _responsedto.responsesuccessmessage)));

                }
                else
                {
                    _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs(String.Format("{0}", _responsedto.responseerrormessage)));

                }

            }
            catch (Exception ex)
            {
                _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs(ex.Message, TAG));
            }
        }
        void checksqliteconnectionstate()
        {
            try
            {
                _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("checking sqlite connection state..."));

                responsedto _responsedto = sqliteapisingleton.getInstance(_notificationmessageEventname).checksqliteconnectionstate();

                if (_responsedto.isresponseresultsuccessful)
                {
                    _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs(String.Format("{0}", _responsedto.responsesuccessmessage)));

                }
                else
                {
                    _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs(String.Format("{0}", _responsedto.responseerrormessage)));

                }

            }
            catch (Exception ex)
            {
                _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs(ex.Message, TAG));
            }
        }
        void changeconfigurations()
        {
            try
            {
                string _base_dir = Environment.CurrentDirectory;
                string _base_dir_with_filename = Application.ExecutablePath;
                string[] _path_levels_arr = _base_dir_with_filename.Split(Path.DirectorySeparatorChar);
                int _file_name_index_in_path = _path_levels_arr.Length - 1;
                string _exe_file_name = _path_levels_arr[_file_name_index_in_path];

                Configuration _Configuration = System.Configuration.ConfigurationManager.OpenExeConfiguration(_base_dir_with_filename);

                bool _isuserdetailsvalid = true;
                string _errormsg = "";

                //if (String.IsNullOrEmpty(txtsysmssqldatasource.Text))
                //{
                //    _isuserdetailsvalid = false;
                //    _errormsg += "server name cannot be null.";
                //    _notificationmessageEventname.Invoke(sender, new notificationmessageEventArgs("server name cannot be null.", TAG));
                //    errorProvider.SetError(txtsysmssqldatasource, "server name cannot be null.");
                //}
                //if (String.IsNullOrEmpty(txtsysmssqldatabase.Text))
                //{
                //    _isuserdetailsvalid = false;
                //    _errormsg += Environment.NewLine + "database name cannot be null.";
                //    _notificationmessageEventname.Invoke(sender, new notificationmessageEventArgs("database name cannot be null.", TAG));
                //    errorProvider.SetError(txtsysmssqldatabase, "database name cannot be null.");
                //}
                //if (String.IsNullOrEmpty(txtsysmssqlusername.Text))
                //{
                //    _isuserdetailsvalid = false;
                //    _errormsg += Environment.NewLine + "user name cannot be null.";
                //    _notificationmessageEventname.Invoke(sender, new notificationmessageEventArgs("user name cannot be null.", TAG));
                //    errorProvider.SetError(txtsysmssqlusername, "user name cannot be null.");
                //}
                //if (String.IsNullOrEmpty(txtsysmssqlpassword.Text))
                //{
                //    _isuserdetailsvalid = false;
                //    _errormsg += Environment.NewLine + "password cannot be null.";
                //    _notificationmessageEventname.Invoke(sender, new notificationmessageEventArgs("password cannot be null.", TAG));
                //    errorProvider.SetError(txtsysmssqlpassword, "password cannot be null.");
                //}
                //if (String.IsNullOrEmpty(txtsysmssqlport.Text))
                //{
                //    _isuserdetailsvalid = false;
                //    _errormsg += Environment.NewLine + "port cannot be null.";
                //    _notificationmessageEventname.Invoke(sender, new notificationmessageEventArgs("port cannot be null.", TAG));
                //    errorProvider.SetError(txtsysmssqlport, "port cannot be null.");
                //}

                if (_isuserdetailsvalid)
                {

                    mssqlconnectionstringdto _connectionstringdto = new mssqlconnectionstringdto();
                    //_connectionstringdto.datasource = txtsysmssqldatasource.Text;
                    //_connectionstringdto.database = txtsysmssqldatabase.Text;
                    //_connectionstringdto.userid = txtsysmssqlusername.Text;
                    //_connectionstringdto.password = txtsysmssqlpassword.Text;
                    //_connectionstringdto.port = txtsysmssqlport.Text;

                    _Configuration.AppSettings.Settings.Remove("mssql_datasource");
                    _Configuration.AppSettings.Settings.Remove("mssql_database");
                    _Configuration.AppSettings.Settings.Remove("mssql_userid");
                    _Configuration.AppSettings.Settings.Remove("mssql_password");
                    _Configuration.AppSettings.Settings.Remove("mssql_port");

                    _Configuration.AppSettings.Settings.Add("mssql_datasource", _connectionstringdto.datasource);
                    _Configuration.AppSettings.Settings.Add("mssql_database", _connectionstringdto.database);
                    _Configuration.AppSettings.Settings.Add("mssql_userid", _connectionstringdto.userid);
                    _Configuration.AppSettings.Settings.Add("mssql_password", _connectionstringdto.password);
                    _Configuration.AppSettings.Settings.Add("mssql_port", _connectionstringdto.port);

                    _Configuration.Save();

                    System.Configuration.ConfigurationManager.RefreshSection("AppSettings");

                    _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs(DBContract.mssql + " configurations persisted successfully."));

                }
                else
                {
                    _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs(_errormsg));
                }

            }
            catch (Exception ex)
            {
                _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs(ex.Message));
            }
        }

        void copy_data()
        {
            try
            {
                long _start = DateTime.Now.Ticks;
                DateTime _begin = DateTime.Now;

                string serverfrom = string.Empty;
                string databasefrom = string.Empty;
                string serverto = string.Empty;
                string databaseto = string.Empty;
                bool _database_exists = false;

                //string _database_name = "";
                //string _system = "";

                globalwritetoconsole("");
                globalwritetoconsole("loading copy data interface...");

                globalwritetoconsole("provide detail for given fields...");

                //let the user select the server to copy from.
                globalwritetoconsole(String.Format("\nServer From: [ {0}, {1}, {2}, {3} ]", DBContract.mssql, DBContract.mysql, DBContract.postgresql, DBContract.sqlite));
                serverfrom = Console.ReadLine();

                //loop until the user provides a server
                while (String.IsNullOrEmpty(serverfrom))
                {
                    globalwritetoconsole("system cannot be null.");
                    globalwritetoconsole(String.Format("\nServer From: [ {0}, {1}, {2}, {3} ]", DBContract.mssql, DBContract.mysql, DBContract.postgresql, DBContract.sqlite));
                    serverfrom = Console.ReadLine();
                }

                while (!String.IsNullOrEmpty(serverfrom))
                {
                    //check the selected server
                    if (serverfrom == "mssql")
                    {
                        globalwritetoconsole("");
                        globalwritetoconsole("fetching a list of databases...");
                        globalwritetoconsole("select from the folowing databases.");
                        List<string> databases = data_utils.get_mssql_databases();
                        globalwritetoconsole("");
                        foreach (string db in databases)
                        {
                            //list all the available table for user to choose from.
                            globalwritetoconsole(db);
                        }
                        //get the database
                        globalwritetoconsole("database name:");
                        globalwritetoconsole("");
                        //wait for the user to press the enter key.
                        databasefrom = Console.ReadLine();

                        //loop until the user gives a database.
                        while (String.IsNullOrEmpty(databasefrom))
                        {
                            globalwritetoconsole("database name cannot be null.");

                            globalwritetoconsole("");
                            globalwritetoconsole("fetching a list of databases...");
                            globalwritetoconsole("select from the folowing databases.");
                            globalwritetoconsole("");
                            foreach (string db in databases)
                            {
                                //list all the available table for user to choose from.
                                globalwritetoconsole(db);
                            }

                            globalwritetoconsole("database name:");
                            databasefrom = Console.ReadLine();
                        }

                        globalwritetoconsole("checking if the database exists...");

                        //check if the provided database exists.
                        _database_exists = mssqlapisingleton.getInstance(_notificationmessageEventname, _progressBarNotificationEventname).check_if_mssql_database_exists(databasefrom);

                        while (!_database_exists)
                        {
                            globalwritetoconsole("database with name [ " + databasefrom + " ] does not exists in " + DBContract.mssql + ".");

                            globalwritetoconsole("");
                            globalwritetoconsole("fetching a list of databases...");
                            globalwritetoconsole("select from the folowing databases.");
                            globalwritetoconsole("");
                            foreach (string db in databases)
                            {
                                //list all the available table for user to choose from.
                                globalwritetoconsole(db);
                            }

                            //get the database
                            globalwritetoconsole("database name:");
                            databasefrom = Console.ReadLine();

                            globalwritetoconsole("checking if the database exists...");

                            //check if the provided database exists.
                            _database_exists = mssqlapisingleton.getInstance(_notificationmessageEventname, _progressBarNotificationEventname).check_if_mssql_database_exists(databasefrom);
                        }

                        //Indicates that the iteration has ended
                        globalwritetoconsole("database selected [ " + databasefrom + " ]");
                        break;
                    }
                    else if (serverfrom == "mysql")
                    {
                        globalwritetoconsole("");

                        this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("create database task execution running against server [ " + DBContract.mysql + " ]...", TAG));

                        //bool _exists_in_mysql = mysqlapisingleton.getInstance(_notificationmessageEventname).check_if_mysql_database_exists(_database_name);

                        //if (!_exists_in_mysql)
                        //{

                        //    this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("database existence check complete.", TAG));

                        //    this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("database with name [ " + _database_name + " ] does not exists in " + DBContract.mysql + ".", TAG));

                        //    this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("commencing with the database creation...", TAG));

                        //    responsedto _responsedto = mysqlapisingleton.getInstance(_notificationmessageEventname).createdatabasegivennamefromconsole(_database_name);

                        //    globalwritetoconsole(_responsedto.responsesuccessmessage);
                        //    globalwritetoconsole(_responsedto.responseerrormessage);

                        //    mysqlconnectionstringdto _connectionstringdto = mysqlapisingleton.getInstance(_notificationmessageEventname).getmysqlconnectionstringdto();
                        //    _connectionstringdto.database = _database_name;
                        //    _connectionstringdto.new_database_name = _database_name;

                        //    _responsedto = mysqlapisingleton.getInstance(_notificationmessageEventname).createtables(_connectionstringdto);

                        //    globalwritetoconsole(_responsedto.responsesuccessmessage);
                        //    globalwritetoconsole(_responsedto.responseerrormessage);

                        //}
                        //else
                        //{
                        //    //this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("database with name [ " + _database_name + " ] exists in " + DBContract.mysql + ".", TAG));
                        //}
                        //Indicates that the iteration has ended
                        break;
                    }
                    else if (serverfrom == "postgresql")
                    {
                        //globalwritetoconsole("");

                        //this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("create database task execution running against server [ " + DBContract.postgresql + " ]...", TAG));

                        //bool _exists_in_postgresql = postgresqlapisingleton.getInstance(_notificationmessageEventname).check_if_postgresql_database_exists(_database_name);

                        //if (!_exists_in_postgresql)
                        //{

                        //    this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("database existence check complete.", TAG));

                        //    this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("database with name [ " + _database_name + " ] does not exists in " + DBContract.postgresql + ".", TAG));

                        //    this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("commencing with the database creation...", TAG));

                        //    responsedto _responsedto = postgresqlapisingleton.getInstance(_notificationmessageEventname).createdatabasegivennamefromconsole(_database_name);

                        //    globalwritetoconsole(_responsedto.responsesuccessmessage);
                        //    globalwritetoconsole(_responsedto.responseerrormessage);

                        //    postgresqlconnectionstringdto _connectionstringdto = postgresqlapisingleton.getInstance(_notificationmessageEventname).getpostgresqlconnectionstringdto();
                        //    _connectionstringdto.database = _database_name;
                        //    _connectionstringdto.new_database_name = _database_name;

                        //    _responsedto = postgresqlapisingleton.getInstance(_notificationmessageEventname).createtables(_connectionstringdto);

                        //    globalwritetoconsole(_responsedto.responsesuccessmessage);
                        //    globalwritetoconsole(_responsedto.responseerrormessage);

                        //}
                        //else
                        //{
                        //    this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("database with name [ " + _database_name + " ] exists in " + DBContract.postgresql + ".", TAG));
                        //}
                        //Indicates that the iteration has ended
                        break;
                    }
                    else if (serverfrom == "sqlite")
                    {
                        globalwritetoconsole("");

                        this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("create database task execution running against server [ " + DBContract.sqlite + " ]...", TAG));

                        //bool _exists_in_sqlite = sqliteapisingleton.getInstance(_notificationmessageEventname).check_if_sqlite_database_exists(_database_name);

                        //if (!_exists_in_sqlite)
                        //{

                        //    this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("database existence check complete.", TAG));

                        //    this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("database with name [ " + _database_name + " ] does not exists in " + DBContract.sqlite + ".", TAG));

                        //    this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("commencing with the database creation...", TAG));

                        //    responsedto _responsedto = sqliteapisingleton.getInstance(_notificationmessageEventname).createdatabasegivennamefromconsole(_database_name);

                        //    globalwritetoconsole(_responsedto.responsesuccessmessage);
                        //    globalwritetoconsole(_responsedto.responseerrormessage);

                        //    sqliteconnectionstringdto _connectionstringdto = sqliteapisingleton.getInstance(_notificationmessageEventname).getsqliteconnectionstringdto();
                        //    _connectionstringdto.database = _database_name;
                        //    _connectionstringdto.new_database_name = _database_name;

                        //    _responsedto = sqliteapisingleton.getInstance(_notificationmessageEventname).createtables(_connectionstringdto);

                        //    globalwritetoconsole(_responsedto.responsesuccessmessage);
                        //    globalwritetoconsole(_responsedto.responseerrormessage);

                        //}
                        //else
                        //{
                        //    this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("database with name [ " + _database_name + " ] exists in " + DBContract.sqlite + ".", TAG));
                        //}
                        //Indicates that the iteration has ended
                        break;
                    }
                    else
                    {
                        _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs(String.Format("\nsystem value must either be: [ {0}, {1}, {2}, {3} ]", DBContract.mssql, DBContract.mysql, DBContract.postgresql, DBContract.sqlite), TAG));
                        globalwritetoconsole(String.Format("\nsystem value must either be: [ {0}, {1}, {2}, {3} ]", DBContract.mssql, DBContract.mysql, DBContract.postgresql, DBContract.sqlite));
                        serverfrom = Console.ReadLine();
                        continue;
                    }

                }

                dbsyncdto _dbsyncdto = new dbsyncdto();
                _dbsyncdto.serverfrom = serverfrom.Trim();
                _dbsyncdto.databasefrom = databasefrom.Trim();
                _dbsyncdto.serverto = serverto.Trim();
                _dbsyncdto.databaseto = databaseto.Trim();

                copy_data_utils datautils = new copy_data_utils(_notificationmessageEventname, _progressBarNotificationEventname);
                datautils.sync_data_from_source_to_destination(_dbsyncdto);

                long _end = DateTime.Now.Ticks;
                DateTime _terminate = DateTime.Now;
                long _duration = _end - _start;
                TimeSpan _iduration = _terminate - _begin;

                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("Task took [ " + _iduration + " ] seconds.", TAG));

            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs(ex.Message, TAG));
                Console.ForegroundColor = ConsoleColor.Green;
            }
        }













    }

}







