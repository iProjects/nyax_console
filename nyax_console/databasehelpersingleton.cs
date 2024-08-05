/*
 * Created by SharpDevelop.
 * User: USER
 * Date: 10/19/2018
 * Time: 18:36
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Data;
using nthareneapi;

namespace nyax_console
{
    /// <summary>
    /// Description of databasehelpersingleton.
    /// </summary>
    public class databasehelpersingleton
    {
        private string TAG;
        private event EventHandler<notificationmessageEventArgs> _notificationmessageEventname;
        public event EventHandler<progressBarNotificationEventArgs> _progressBarNotificationEventname;
        private static databasehelpersingleton singleInstance;

        public static databasehelpersingleton getInstance(EventHandler<notificationmessageEventArgs> notificationmessageEventname, EventHandler<progressBarNotificationEventArgs> progressBarNotificationEventname)
        {
            if (databasehelpersingleton.singleInstance == null)
            {
                databasehelpersingleton.singleInstance = new databasehelpersingleton(notificationmessageEventname, progressBarNotificationEventname);
            }
            return databasehelpersingleton.singleInstance;
        }

        private databasehelpersingleton(EventHandler<notificationmessageEventArgs> notificationmessageEventname, EventHandler<progressBarNotificationEventArgs> progressBarNotificationEventname)
        {
            TAG = this.GetType().Name;
            _notificationmessageEventname = notificationmessageEventname;
            _progressBarNotificationEventname = progressBarNotificationEventname;
        }

        private databasehelpersingleton()
        {

        }

        #region "crops"
        public void savecropinmssqldb(cropdto _cropdto)
        {
            string saveinmssql = utilzsingleton.getInstance(_notificationmessageEventname).getappsettinggivenkey("saveinmssql", "false");

            bool _saveinmssql;
            bool _trysaveinmssql = bool.TryParse(saveinmssql, out _saveinmssql);

            if (_saveinmssql)
            {
                bool numberOfRowsAffected = false;
                numberOfRowsAffected = mssqlapisingleton.getInstance(_notificationmessageEventname, _progressBarNotificationEventname).createcropindatabase(_cropdto, DBContract.getdefaultmssqlconnectionstring());
                if (numberOfRowsAffected)
                {
                    _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("successfully created crop in mssql db { " +
                    Environment.NewLine + "crop name: " + _cropdto.crop_name + "," +
                    Environment.NewLine + "status: " + _cropdto.crop_status + " }.", TAG));
                    printrecordcountoninsert(systementityconstants.crop, datastoreconstants.mssql);
                }
            }
        }

        public void savecropinmysqldb(cropdto _cropdto)
        {
            string saveinmysql = utilzsingleton.getInstance(_notificationmessageEventname).getappsettinggivenkey("saveinmysql", "false");

            bool _saveinmysql;
            bool _trysaveinmysql = bool.TryParse(saveinmysql, out _saveinmysql);

            if (_saveinmysql)
            {
                bool numberOfRowsAffected = false;
                numberOfRowsAffected = mysqlapisingleton.getInstance(_notificationmessageEventname).createcropindatabase(_cropdto);
                if (numberOfRowsAffected)
                {
                    _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("successfully created crop in mysql db { " +
                    Environment.NewLine + "crop name: " + _cropdto.crop_name + "," +
                    Environment.NewLine + "status: " + _cropdto.crop_status + " }.", TAG));
                    printrecordcountoninsert(systementityconstants.crop, datastoreconstants.mysql);
                }
            }
        }

        public void savecropinsqlitedb(cropdto _cropdto)
        {
            string saveinsqlite = utilzsingleton.getInstance(_notificationmessageEventname).getappsettinggivenkey("saveinsqlite", "false");

            bool _saveinsqlite;
            bool _trysaveinsqlite = bool.TryParse(saveinsqlite, out _saveinsqlite);

            if (_saveinsqlite)
            {
                bool numberOfRowsAffected = false;
                numberOfRowsAffected = sqliteapisingleton.getInstance(_notificationmessageEventname).createcropindatabase(_cropdto);
                if (numberOfRowsAffected)
                {
                    _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("successfully created crop in sqlite db { " +
                    Environment.NewLine + "crop name: " + _cropdto.crop_name + "," +
                    Environment.NewLine + "status: " + _cropdto.crop_status + " }.", TAG));
                    printrecordcountoninsert(systementityconstants.crop, datastoreconstants.sqlite);
                }
            }
        }

        public void savecropinpostgresqldb(cropdto _cropdto)
        {
            string saveinsqlite = utilzsingleton.getInstance(_notificationmessageEventname).getappsettinggivenkey("saveinpostgresql", "false");

            bool _saveinsqlite;
            bool _trysaveinsqlite = bool.TryParse(saveinsqlite, out _saveinsqlite);

            if (_saveinsqlite)
            {
                bool numberOfRowsAffected = false;
                numberOfRowsAffected = postgresqlapisingleton.getInstance(_notificationmessageEventname).createcropindatabase(_cropdto);
                if (numberOfRowsAffected)
                {
                    _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("successfully created crop in postgresql db { " +
                    Environment.NewLine + "crop name: " + _cropdto.crop_name + "," +
                    Environment.NewLine + "status: " + _cropdto.crop_status + " }.", TAG));
                    printrecordcountoninsert(systementityconstants.crop, datastoreconstants.postgresql);
                }
            }
        }
        #endregion "crops"

        #region "diseases/pests"
        public void savecropdiseaseinmssqldb(cropdiseasedto _cropdiseasedto)
        {
            string saveinmssql = utilzsingleton.getInstance(_notificationmessageEventname).getappsettinggivenkey("saveinmssql", "false");

            bool _saveinmssql;
            bool _trysaveinmssql = bool.TryParse(saveinmssql, out _saveinmssql);

            if (_saveinmssql)
            {
                bool numberOfRowsAffected = false;
                numberOfRowsAffected = mssqlapisingleton.getInstance(_notificationmessageEventname, _progressBarNotificationEventname).createcropdiseaseindatabase(_cropdiseasedto, DBContract.getdefaultmssqlconnectionstring());
                if (numberOfRowsAffected)
                {
                    _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("successfully created crop disease/pest in mssql db { " +
                    Environment.NewLine + "disease/pest name: " + _cropdiseasedto.crop_disease_name + "," +
                    Environment.NewLine + "category: " + _cropdiseasedto.crop_disease_category + "," +
                    Environment.NewLine + "status: " + _cropdiseasedto.crop_disease_status + " }.", TAG));
                    printrecordcountoninsert(systementityconstants.diseasepest, datastoreconstants.mssql);
                }
            }
        }

        public void savecropdiseaseinmysqldb(cropdiseasedto _cropdiseasedto)
        {
            string saveinmysql = utilzsingleton.getInstance(_notificationmessageEventname).getappsettinggivenkey("saveinmysql", "false");

            bool _saveinmysql;
            bool _trysaveinmysql = bool.TryParse(saveinmysql, out _saveinmysql);

            if (_saveinmysql)
            {
                bool numberOfRowsAffected = false;
                numberOfRowsAffected = mysqlapisingleton.getInstance(_notificationmessageEventname).createcropdiseaseindatabase(_cropdiseasedto);
                if (numberOfRowsAffected)
                {
                    _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("successfully created crop disease/pest in mysql db { " +
                    Environment.NewLine + "disease/pest name: " + _cropdiseasedto.crop_disease_name + "," +
                    Environment.NewLine + "category: " + _cropdiseasedto.crop_disease_category + "," +
                    Environment.NewLine + "status: " + _cropdiseasedto.crop_disease_status + " }.", TAG));
                    printrecordcountoninsert(systementityconstants.diseasepest, datastoreconstants.mysql);
                }
            }
        }

        public void savecropdiseaseinsqlitedb(cropdiseasedto _cropdiseasedto)
        {
            string saveinsqlite = utilzsingleton.getInstance(_notificationmessageEventname).getappsettinggivenkey("saveinsqlite", "false");

            bool _saveinsqlite;
            bool _trysaveinsqlite = bool.TryParse(saveinsqlite, out _saveinsqlite);

            if (_saveinsqlite)
            {
                bool numberOfRowsAffected = false;
                numberOfRowsAffected = sqliteapisingleton.getInstance(_notificationmessageEventname).createcropdiseaseindatabase(_cropdiseasedto);
                if (numberOfRowsAffected)
                {
                    _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("successfully created crop disease/pest in sqlite db { " +
                    Environment.NewLine + "disease/pest name: " + _cropdiseasedto.crop_disease_name + "," +
                    Environment.NewLine + "category: " + _cropdiseasedto.crop_disease_category + "," +
                    Environment.NewLine + "status: " + _cropdiseasedto.crop_disease_status + " }.", TAG));
                    printrecordcountoninsert(systementityconstants.diseasepest, datastoreconstants.sqlite);
                }
            }
        }

        public void savecropdiseaseinpostgresqldb(cropdiseasedto _cropdiseasedto)
        {
            string saveinpostgresql = utilzsingleton.getInstance(_notificationmessageEventname).getappsettinggivenkey("saveinpostgresql", "false");

            bool _saveinpostgresql;
            bool _trysaveinpostgresql = bool.TryParse(saveinpostgresql, out _saveinpostgresql);

            if (_saveinpostgresql)
            {
                bool numberOfRowsAffected = false;
                numberOfRowsAffected = postgresqlapisingleton.getInstance(_notificationmessageEventname).createcropdiseaseindatabase(_cropdiseasedto);
                if (numberOfRowsAffected)
                {
                    _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("successfully created crop disease/pest in postgresql db { " +
                    Environment.NewLine + "disease/pest name: " + _cropdiseasedto.crop_disease_name + "," +
                    Environment.NewLine + "category: " + _cropdiseasedto.crop_disease_category + "," +
                    Environment.NewLine + "status: " + _cropdiseasedto.crop_disease_status + " }.", TAG));
                    printrecordcountoninsert(systementityconstants.diseasepest, datastoreconstants.postgresql);
                }
            }
        }
        #endregion "diseases/pests"

        #region "manufacturers"
        public void savemanufacturerinmssqldb(manufacturerdto _manufacturerdto)
        {
            string saveinmssql = utilzsingleton.getInstance(_notificationmessageEventname).getappsettinggivenkey("saveinmssql", "false");

            bool _saveinmssql;
            bool _trysaveinmssql = bool.TryParse(saveinmssql, out _saveinmssql);

            if (_saveinmssql)
            {
                bool numberOfRowsAffected = false;
                numberOfRowsAffected = mssqlapisingleton.getInstance(_notificationmessageEventname, _progressBarNotificationEventname).createmanufacturerindatabase(_manufacturerdto, DBContract.getdefaultmssqlconnectionstring());
                if (numberOfRowsAffected)
                {
                    _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("successfully created manufacturer in mssql db { " + Environment.NewLine + "manufacturer name: " + _manufacturerdto.manufacturer_name + "," +
                    Environment.NewLine + "status: " + _manufacturerdto.manufacturer_status + " }.", TAG));
                    printrecordcountoninsert(systementityconstants.manufacturer, datastoreconstants.mssql);
                }
            }
        }

        public void savemanufacturerinmysqldb(manufacturerdto _manufacturerdto)
        {
            string saveinmysql = utilzsingleton.getInstance(_notificationmessageEventname).getappsettinggivenkey("saveinmysql", "false");

            bool _saveinmysql;
            bool _trysaveinmysql = bool.TryParse(saveinmysql, out _saveinmysql);

            if (_saveinmysql)
            {
                bool numberOfRowsAffected = false;
                numberOfRowsAffected = mysqlapisingleton.getInstance(_notificationmessageEventname).createmanufacturerindatabase(_manufacturerdto);
                if (numberOfRowsAffected)
                {
                    _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("successfully created manufacturer in mysql db { " + Environment.NewLine + "manufacturer name: " + _manufacturerdto.manufacturer_name + "," +
                    Environment.NewLine + "status: " + _manufacturerdto.manufacturer_status + " }.", TAG));
                    printrecordcountoninsert(systementityconstants.manufacturer, datastoreconstants.mysql);
                }
            }
        }

        public void savemanufacturerinsqlitedb(manufacturerdto _manufacturerdto)
        {
            string saveinsqlite = utilzsingleton.getInstance(_notificationmessageEventname).getappsettinggivenkey("saveinsqlite", "false");

            bool _saveinsqlite;
            bool _trysaveinsqlite = bool.TryParse(saveinsqlite, out _saveinsqlite);

            if (_saveinsqlite)
            {
                bool numberOfRowsAffected = false;
                numberOfRowsAffected = sqliteapisingleton.getInstance(_notificationmessageEventname).createmanufacturerindatabase(_manufacturerdto);
                if (numberOfRowsAffected)
                {
                    _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("successfully created manufacturer in sqlite db { " + Environment.NewLine + "manufacturer name: " + _manufacturerdto.manufacturer_name + "," +
                    Environment.NewLine + "status: " + _manufacturerdto.manufacturer_status + " }.", TAG));
                    printrecordcountoninsert(systementityconstants.manufacturer, datastoreconstants.sqlite);
                }
            }
        }

        public void savemanufacturerinpostgresqldb(manufacturerdto _manufacturerdto)
        {
            string saveinpostgresql = utilzsingleton.getInstance(_notificationmessageEventname).getappsettinggivenkey("saveinpostgresql", "false");

            bool _saveinpostgresql;
            bool _trysaveinpostgresql = bool.TryParse(saveinpostgresql, out _saveinpostgresql);

            if (_saveinpostgresql)
            {
                bool numberOfRowsAffected = false;
                numberOfRowsAffected = postgresqlapisingleton.getInstance(_notificationmessageEventname).createmanufacturerindatabase(_manufacturerdto);
                if (numberOfRowsAffected)
                {
                    _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("successfully created manufacturer in postgresql db { " + Environment.NewLine + "manufacturer name: " + _manufacturerdto.manufacturer_name + "," +
                    Environment.NewLine + "status: " + _manufacturerdto.manufacturer_status + " }.", TAG));
                    printrecordcountoninsert(systementityconstants.manufacturer, datastoreconstants.postgresql);
                }
            }
        }
        #endregion "manufacturers"

        #region "pesticides/insecticides"
        public void savepestinsecticideinmssqldb(pestinsecticidedto _pestinsecticidedto)
        {
            string saveinmssql = utilzsingleton.getInstance(_notificationmessageEventname).getappsettinggivenkey("saveinmssql", "false");

            bool _saveinmssql;
            bool _trysaveinmssql = bool.TryParse(saveinmssql, out _saveinmssql);

            if (_saveinmssql)
            {
                bool numberOfRowsAffected = false;
                numberOfRowsAffected = mssqlapisingleton.getInstance(_notificationmessageEventname, _progressBarNotificationEventname).createpestinsecticideindatabase(_pestinsecticidedto, DBContract.getdefaultmssqlconnectionstring());
                if (numberOfRowsAffected)
                {
                    _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("successfully created pesticide/insecticide in mssql db { " +
                     Environment.NewLine +
                     "pestinsecticidename: " + _pestinsecticidedto.pestinsecticide_name + "," +
                     Environment.NewLine +
                     " category: " + _pestinsecticidedto.pestinsecticide_category + "," +
                     Environment.NewLine +
                     " manufacturer: " + _pestinsecticidedto.pestinsecticide_manufacturer_id + "," + Environment.NewLine +
                     " disease/pest: " + _pestinsecticidedto.pestinsecticide_crop_disease_id + "," + Environment.NewLine +
                     " status: " + _pestinsecticidedto.pestinsecticide_status + " }.", TAG));
                    printrecordcountoninsert(systementityconstants.pestinsecticide, datastoreconstants.mssql);
                }
            }
        }

        public void savepestinsecticideinmysqldb(pestinsecticidedto _pestinsecticidedto)
        {
            string saveinmysql = utilzsingleton.getInstance(_notificationmessageEventname).getappsettinggivenkey("saveinmysql", "false");

            bool _saveinmysql;
            bool _trysaveinmysql = bool.TryParse(saveinmysql, out _saveinmysql);

            if (_saveinmysql)
            {
                bool numberOfRowsAffected = false;
                numberOfRowsAffected = mysqlapisingleton.getInstance(_notificationmessageEventname).createpestinsecticideindatabase(_pestinsecticidedto);
                if (numberOfRowsAffected)
                {
                    _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("successfully created pesticide/insecticide in mysql db { " +
                     Environment.NewLine +
                     "pestinsecticidename: " + _pestinsecticidedto.pestinsecticide_name + "," +
                     Environment.NewLine +
                     " category: " + _pestinsecticidedto.pestinsecticide_category + "," +
                     Environment.NewLine +
                     " manufacturer: " + _pestinsecticidedto.pestinsecticide_manufacturer_id + "," + Environment.NewLine +
                     " disease/pest: " + _pestinsecticidedto.pestinsecticide_crop_disease_id + "," + Environment.NewLine +
                     " status: " + _pestinsecticidedto.pestinsecticide_status + " }.", TAG));
                    printrecordcountoninsert(systementityconstants.pestinsecticide, datastoreconstants.mysql);
                }
            }
        }

        public void savepestinsecticideinsqlitedb(pestinsecticidedto _pestinsecticidedto)
        {
            string saveinsqlite = utilzsingleton.getInstance(_notificationmessageEventname).getappsettinggivenkey("saveinsqlite", "false");

            bool _saveinsqlite;
            bool _trysaveinsqlite = bool.TryParse(saveinsqlite, out _saveinsqlite);

            if (_saveinsqlite)
            {
                bool numberOfRowsAffected = false;
                numberOfRowsAffected = sqliteapisingleton.getInstance(_notificationmessageEventname).createpestinsecticideindatabase(_pestinsecticidedto);
                if (numberOfRowsAffected)
                {
                    _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("successfully created pesticide/insecticide in sqlite db { " +
                     Environment.NewLine +
                     "pestinsecticidename: " + _pestinsecticidedto.pestinsecticide_name + "," +
                     Environment.NewLine +
                     " category: " + _pestinsecticidedto.pestinsecticide_category + "," +
                     Environment.NewLine +
                     " manufacturer: " + _pestinsecticidedto.pestinsecticide_manufacturer_id + "," + Environment.NewLine +
                     " disease/pest: " + _pestinsecticidedto.pestinsecticide_crop_disease_id + "," + Environment.NewLine +
                     " status: " + _pestinsecticidedto.pestinsecticide_status + " }.", TAG));
                    printrecordcountoninsert(systementityconstants.pestinsecticide, datastoreconstants.sqlite);
                }
            }
        }

        public void savepestinsecticideinpostgresqldb(pestinsecticidedto _pestinsecticidedto)
        {
            string saveinpostgresql = utilzsingleton.getInstance(_notificationmessageEventname).getappsettinggivenkey("saveinpostgresql", "false");

            bool _saveinpostgresql;
            bool _trysaveinpostgresql = bool.TryParse(saveinpostgresql, out _saveinpostgresql);

            if (_saveinpostgresql)
            {
                bool numberOfRowsAffected = false;
                numberOfRowsAffected = postgresqlapisingleton.getInstance(_notificationmessageEventname).createpestinsecticideindatabase(_pestinsecticidedto);
                if (numberOfRowsAffected)
                {
                    _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("successfully created pesticide/insecticide in postgresql db { " +
                     Environment.NewLine +
                     "pestinsecticidename: " + _pestinsecticidedto.pestinsecticide_name + "," +
                     Environment.NewLine +
                     " category: " + _pestinsecticidedto.pestinsecticide_category + "," +
                     Environment.NewLine +
                     " manufacturer: " + _pestinsecticidedto.pestinsecticide_manufacturer_id + "," + Environment.NewLine +
                     " disease/pest: " + _pestinsecticidedto.pestinsecticide_crop_disease_id + "," + Environment.NewLine +
                     " status: " + _pestinsecticidedto.pestinsecticide_status + " }.", TAG));
                    printrecordcountoninsert(systementityconstants.pestinsecticide, datastoreconstants.postgresql);
                }
            }
        }
        #endregion "pesticides/insecticides"

        #region "settings"
        public void savesettinginmssqldb(settingdto _settingdto)
        {
            try
            {
                string saveinmssql = utilzsingleton.getInstance(_notificationmessageEventname).getappsettinggivenkey("saveinmssql", "false");

                bool _saveinmssql;
                bool _trysaveinmssql = bool.TryParse(saveinmssql, out _saveinmssql);

                if (_saveinmssql)
                {
                    bool numberOfRowsAffected = false;
                    numberOfRowsAffected = mssqlapisingleton.getInstance(_notificationmessageEventname, _progressBarNotificationEventname).createsettingindatabase(_settingdto, DBContract.getdefaultmssqlconnectionstring());
                    if (numberOfRowsAffected)
                    {
                        _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("successfully created setting in mssql db { " + Environment.NewLine + "setting name: " + _settingdto.setting_name + "," +
                        Environment.NewLine + "setting value: " + _settingdto.setting_value + "," +
                        Environment.NewLine + "status: " + _settingdto.setting_status + " }.", TAG));
                        printrecordcountoninsert(systementityconstants.setting, datastoreconstants.mssql);
                    }
                }
            }
            catch (Exception ex)
            {
                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs(ex.Message, TAG));
            }
        }

        public void savesettinginmysqldb(settingdto _settingdto)
        {
            try
            {
                string saveinmysql = utilzsingleton.getInstance(_notificationmessageEventname).getappsettinggivenkey("saveinmysql", "false");

                bool _saveinmysql;
                bool _trysaveinmysql = bool.TryParse(saveinmysql, out _saveinmysql);

                if (_saveinmysql)
                {
                    bool numberOfRowsAffected = false;
                    numberOfRowsAffected = mysqlapisingleton.getInstance(_notificationmessageEventname).createsettingindatabase(_settingdto);
                    if (numberOfRowsAffected)
                    {
                        _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("successfully created setting in mysql db { " + Environment.NewLine + "setting name: " + _settingdto.setting_name + "," +
                        Environment.NewLine + "setting value: " + _settingdto.setting_value + "," +
                        Environment.NewLine + "status: " + _settingdto.setting_status + " }.", TAG));
                        printrecordcountoninsert(systementityconstants.setting, datastoreconstants.mysql);
                    }
                }
            }
            catch (Exception ex)
            {
                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs(ex.Message, TAG));
            }
        }

        public void savesettinginsqlitedb(settingdto _settingdto)
        {
            try
            {
                string saveinsqlite = utilzsingleton.getInstance(_notificationmessageEventname).getappsettinggivenkey("saveinsqlite", "false");

                bool _saveinsqlite;
                bool _trysaveinsqlite = bool.TryParse(saveinsqlite, out _saveinsqlite);

                if (_saveinsqlite)
                {
                    bool numberOfRowsAffected = false;
                    numberOfRowsAffected = sqliteapisingleton.getInstance(_notificationmessageEventname).createsettingindatabase(_settingdto);
                    if (numberOfRowsAffected)
                    {
                        _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("successfully created setting in sqlite db { " + Environment.NewLine + "setting name: " + _settingdto.setting_name + "," +
                        Environment.NewLine + "setting value: " + _settingdto.setting_value + "," +
                        Environment.NewLine + "status: " + _settingdto.setting_status + " }.", TAG));
                        printrecordcountoninsert(systementityconstants.setting, datastoreconstants.sqlite);
                    }
                }
            }
            catch (Exception ex)
            {
                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs(ex.Message, TAG));
            }
        }

        public void savesettinginpostgresqldb(settingdto _settingdto)
        {
            try
            {
                string saveinpostgresql = utilzsingleton.getInstance(_notificationmessageEventname).getappsettinggivenkey("saveinpostgresql", "false");

                bool _saveinpostgresql;
                bool _trysaveinpostgresql = bool.TryParse(saveinpostgresql, out _saveinpostgresql);

                if (_saveinpostgresql)
                {
                    bool numberOfRowsAffected = false;
                    numberOfRowsAffected = postgresqlapisingleton.getInstance(_notificationmessageEventname).createsettingindatabase(_settingdto);
                    if (numberOfRowsAffected)
                    {
                        _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("successfully created setting in postgresql db { " + Environment.NewLine + "setting name: " + _settingdto.setting_name + "," +
                        Environment.NewLine + "setting value: " + _settingdto.setting_value + "," +
                        Environment.NewLine + "status: " + _settingdto.setting_status + " }.", TAG));
                        printrecordcountoninsert(systementityconstants.setting, datastoreconstants.postgresql);
                    }
                }
            }
            catch (Exception ex)
            {
                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs(ex.Message, TAG));
            }
        }
        #endregion "settings"

        #region "cropsvarieties"
        public void savecropvarietyinmssqldb(cropvarietydto _cropvarietydto)
        {
            string saveinmssql = utilzsingleton.getInstance(_notificationmessageEventname).getappsettinggivenkey("saveinmssql", "false");

            bool _saveinmssql;
            bool _trysaveinmssql = bool.TryParse(saveinmssql, out _saveinmssql);

            if (_saveinmssql)
            {
                bool numberOfRowsAffected = false;
                numberOfRowsAffected = mssqlapisingleton.getInstance(_notificationmessageEventname, _progressBarNotificationEventname).createcropvarietyindatabase(_cropvarietydto, DBContract.getdefaultmssqlconnectionstring());
                if (numberOfRowsAffected)
                {
                    _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("successfully created crop variety in mssql db { " +
                    Environment.NewLine +
                    "crop variety name: " + _cropvarietydto.crop_variety_name + "," +
                    Environment.NewLine +
                    "crop variety crop: " + _cropvarietydto.crop_variety_crop_id + "," +
                    Environment.NewLine +
                    "crop variety manufacturer: " + _cropvarietydto.crop_variety_manufacturer_id + "," +
                    Environment.NewLine +
                    "status: " + _cropvarietydto.crop_variety_status + " }.", TAG));
                    printrecordcountoninsert(systementityconstants.cropvariety, datastoreconstants.mssql);
                }
            }
        }

        public void savecropvarietyinmysqldb(cropvarietydto _cropvarietydto)
        {
            string saveinmysql = utilzsingleton.getInstance(_notificationmessageEventname).getappsettinggivenkey("saveinmysql", "false");

            bool _saveinmysql;
            bool _trysaveinmysql = bool.TryParse(saveinmysql, out _saveinmysql);

            if (_saveinmysql)
            {
                bool numberOfRowsAffected = false;
                numberOfRowsAffected = mysqlapisingleton.getInstance(_notificationmessageEventname).createcropvarietyindatabase(_cropvarietydto);
                if (numberOfRowsAffected)
                {
                    _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("successfully created crop variety in mysql db { " +
                    Environment.NewLine +
                    "crop variety name: " + _cropvarietydto.crop_variety_name + "," +
                    Environment.NewLine +
                    "crop variety crop: " + _cropvarietydto.crop_variety_crop_id + "," +
                    Environment.NewLine +
                    "crop variety manufacturer: " + _cropvarietydto.crop_variety_manufacturer_id + "," +
                    Environment.NewLine +
                    "status: " + _cropvarietydto.crop_variety_status + " }.", TAG));
                    printrecordcountoninsert(systementityconstants.cropvariety, datastoreconstants.mysql);
                }
            }
        }

        public void savecropvarietyinsqlitedb(cropvarietydto _cropvarietydto)
        {
            string saveinsqlite = utilzsingleton.getInstance(_notificationmessageEventname).getappsettinggivenkey("saveinsqlite", "false");

            bool _saveinsqlite;
            bool _trysaveinsqlite = bool.TryParse(saveinsqlite, out _saveinsqlite);

            if (_saveinsqlite)
            {
                bool numberOfRowsAffected = false;
                numberOfRowsAffected = sqliteapisingleton.getInstance(_notificationmessageEventname).createcropvarietyindatabase(_cropvarietydto);
                if (numberOfRowsAffected)
                {
                    _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("successfully created crop variety in sqlite db { " +
                    Environment.NewLine +
                    "crop variety name: " + _cropvarietydto.crop_variety_name + "," +
                    Environment.NewLine +
                    "crop variety crop: " + _cropvarietydto.crop_variety_crop_id + "," +
                    Environment.NewLine +
                    "crop variety manufacturer: " + _cropvarietydto.crop_variety_manufacturer_id + "," +
                    Environment.NewLine +
                    "status: " + _cropvarietydto.crop_variety_status + " }.", TAG));
                    printrecordcountoninsert(systementityconstants.cropvariety, datastoreconstants.sqlite);
                }
            }
        }

        public void savecropvarietyinpostgresqldb(cropvarietydto _cropvarietydto)
        {
            string saveinsqlite = utilzsingleton.getInstance(_notificationmessageEventname).getappsettinggivenkey("saveinpostgresql", "false");

            bool _saveinsqlite;
            bool _trysaveinsqlite = bool.TryParse(saveinsqlite, out _saveinsqlite);

            if (_saveinsqlite)
            {
                bool numberOfRowsAffected = false;
                numberOfRowsAffected = postgresqlapisingleton.getInstance(_notificationmessageEventname).createcropvarietyindatabase(_cropvarietydto);
                if (numberOfRowsAffected)
                {
                    _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("successfully created crop variety in postgresql db { " +
                    Environment.NewLine +
                    "crop variety name: " + _cropvarietydto.crop_variety_name + "," +
                    Environment.NewLine +
                    "crop variety crop: " + _cropvarietydto.crop_variety_crop_id + "," +
                    Environment.NewLine +
                    "crop variety manufacturer: " + _cropvarietydto.crop_variety_manufacturer_id + "," +
                    Environment.NewLine +
                    "status: " + _cropvarietydto.crop_variety_status + " }.", TAG));
                    printrecordcountoninsert(systementityconstants.cropvariety, datastoreconstants.postgresql);
                }
            }
        }
        #endregion "cropsvarieties"

        #region "categories"
        public void savecategoryinmssqldb(categorydto _categorydto)
        {
            string saveinmssql = utilzsingleton.getInstance(_notificationmessageEventname).getappsettinggivenkey("saveinmssql", "false");

            bool _saveinmssql;
            bool _trysaveinmssql = bool.TryParse(saveinmssql, out _saveinmssql);

            if (_saveinmssql)
            {
                bool numberOfRowsAffected = false;
                numberOfRowsAffected = mssqlapisingleton.getInstance(_notificationmessageEventname, _progressBarNotificationEventname).createcategoryindatabase(_categorydto, DBContract.getdefaultmssqlconnectionstring());
                if (numberOfRowsAffected)
                {
                    _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("successfully created category in mssql db { " +
                    Environment.NewLine + "category name: " + _categorydto.category_name + "," +
                    Environment.NewLine + "status: " + _categorydto.category_status + " }.", TAG));
                    printrecordcountoninsert(systementityconstants.category, datastoreconstants.mssql);
                }
            }
        }

        public void savecategoryinmysqldb(categorydto _categorydto)
        {
            string saveinmysql = utilzsingleton.getInstance(_notificationmessageEventname).getappsettinggivenkey("saveinmysql", "false");

            bool _saveinmysql;
            bool _trysaveinmysql = bool.TryParse(saveinmysql, out _saveinmysql);

            if (_saveinmysql)
            {
                bool numberOfRowsAffected = false;
                numberOfRowsAffected = mysqlapisingleton.getInstance(_notificationmessageEventname).createcategoryindatabase(_categorydto);
                if (numberOfRowsAffected)
                {
                    _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("successfully created category in mysql db { " +
                    Environment.NewLine + "category name: " + _categorydto.category_name + "," +
                    Environment.NewLine + "status: " + _categorydto.category_status + " }.", TAG));
                    printrecordcountoninsert(systementityconstants.category, datastoreconstants.mysql);
                }
            }
        }

        public void savecategoryinsqlitedb(categorydto _categorydto)
        {
            string saveinsqlite = utilzsingleton.getInstance(_notificationmessageEventname).getappsettinggivenkey("saveinsqlite", "false");

            bool _saveinsqlite;
            bool _trysaveinsqlite = bool.TryParse(saveinsqlite, out _saveinsqlite);

            if (_saveinsqlite)
            {
                bool numberOfRowsAffected = false;
                numberOfRowsAffected = sqliteapisingleton.getInstance(_notificationmessageEventname).createcategoryindatabase(_categorydto);
                if (numberOfRowsAffected)
                {
                    _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("successfully created category in sqlite db { " +
                    Environment.NewLine + "category name: " + _categorydto.category_name + "," +
                    Environment.NewLine + "status: " + _categorydto.category_status + " }.", TAG));
                    printrecordcountoninsert(systementityconstants.category, datastoreconstants.sqlite);
                }
            }
        }

        public void savecategoryinpostgresqldb(categorydto _categorydto)
        {
            string saveinsqlite = utilzsingleton.getInstance(_notificationmessageEventname).getappsettinggivenkey("saveinpostgresql", "false");

            bool _saveinsqlite;
            bool _trysaveinsqlite = bool.TryParse(saveinsqlite, out _saveinsqlite);

            if (_saveinsqlite)
            {
                bool numberOfRowsAffected = false;
                numberOfRowsAffected = postgresqlapisingleton.getInstance(_notificationmessageEventname).createcategoryindatabase(_categorydto);
                if (numberOfRowsAffected)
                {
                    _notificationmessageEventname.Invoke(this, new notificationmessageEventArgs("successfully created category in postgresql db { " +
                    Environment.NewLine + "category name: " + _categorydto.category_name + "," +
                    Environment.NewLine + "status: " + _categorydto.category_status + " }.", TAG));
                    printrecordcountoninsert(systementityconstants.category, datastoreconstants.postgresql);
                }
            }
        }
        #endregion "categories"

        public void printrecordcountoninsert(string _entity, string _datastore)
        {
            try
            {
                string query = "";
                DataTable dt = null;

                switch (_entity)
                {
                    case systementityconstants.crop:
                        query = DBContract.CROPS_SELECT_ALL_QUERY;
                        break;
                    case systementityconstants.diseasepest:
                        query = DBContract.CROPS_DISEASES_SELECT_ALL_QUERY;
                        break;
                    case systementityconstants.manufacturer:
                        query = DBContract.MANUFACTURERS_SELECT_ALL_QUERY;
                        break;
                    case systementityconstants.pestinsecticide:
                        query = DBContract.PESTSINSECTICIDES_SELECT_ALL_QUERY;
                        break;
                    case systementityconstants.setting:
                        query = DBContract.SETTINGS_SELECT_ALL_QUERY;
                        break;
                    case systementityconstants.category:
                        query = DBContract.CATEGORIES_SELECT_ALL_QUERY;
                        break;
                    case systementityconstants.cropvariety:
                        query = DBContract.CROPS_VARIETIES_SELECT_ALL_QUERY;
                        break;
                }

                switch (_datastore)
                {
                    case datastoreconstants.mssql:
                        dt = mssqlapisingleton.getInstance(_notificationmessageEventname, _progressBarNotificationEventname).getallrecordsglobal(query);
                        break;
                    case datastoreconstants.mysql:
                        dt = mysqlapisingleton.getInstance(_notificationmessageEventname).getallrecordsglobal(query);
                        break;
                    case datastoreconstants.postgresql:
                        dt = postgresqlapisingleton.getInstance(_notificationmessageEventname).getallrecordsglobal(query);
                        break;
                    case datastoreconstants.sqlite:
                        dt = sqliteapisingleton.getInstance(_notificationmessageEventname).getallrecordsglobal(query);
                        break;
                }

                if (dt == null)
                {
                    return;
                }

                var _recordscount = dt.Rows.Count;
                string msg = _datastore + " " + _entity + " records count [ " + _recordscount + " ]";
                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs(msg, TAG));

            }
            catch (Exception ex)
            {
                this._notificationmessageEventname.Invoke(this, new notificationmessageEventArgs(ex.Message, TAG));
            }
        }



    }
}
