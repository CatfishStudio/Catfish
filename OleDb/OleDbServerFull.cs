using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Catfish
{
    class OleDbServerFull
    {
        /* Переменные */
        private OleDbConnection _connection;
        private OleDbCommand _selectCommand;
        private OleDbCommand _insertCommand;
        private OleDbCommand _updateCommand;
        private OleDbCommand _deleteCommand;
        private OleDbDataAdapter _dataAdapter;
        private String _strSelect;
        private String _strInsert;
        private String _strUpdate;
        private String _strDelete;

        /* Конструктор */
        public OleDbServerFull(String PathBase)
        {
        	_connection = new OleDbConnection();
            _connection.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathBase + ";Jet OLEDB:Database Password=";
            _selectCommand = new OleDbCommand("", _connection);
            _insertCommand = new OleDbCommand("", _connection);
            _updateCommand = new OleDbCommand("", _connection);
            _deleteCommand = new OleDbCommand("", _connection);
            _dataAdapter = new OleDbDataAdapter();
        }

        /* Свойства */
        public String SelectSqlCommand
        {
            get { return _strSelect; }
            set { _strSelect = value; }
        }

        public String InsertSqlCommand
        {
            get { return _strInsert; }
            set { _strInsert = value; }
        }

        public String UpdateSqlCommand
        {
            get { return _strUpdate; }
            set { _strUpdate = value; }
        }

        public String DeleteSqlCommand
        {
            get { return _strDelete; }
            set { _strDelete = value; }
        }

        /* Методы */
        public bool ExecuteFill(DataSet _DataSet, String _tableName)
        {
            try
            {
            	_connection.Open();
                _selectCommand.CommandText = _strSelect;
                _insertCommand.CommandText = _strInsert;
                _updateCommand.CommandText = _strUpdate;
                _deleteCommand.CommandText = _strDelete;
                _dataAdapter.SelectCommand = _selectCommand;
                _dataAdapter.InsertCommand = _insertCommand;
                _dataAdapter.UpdateCommand = _updateCommand;
                _dataAdapter.DeleteCommand = _deleteCommand;
                _dataAdapter.Fill(_DataSet, _tableName);
                _connection.Close();
                return true;
            }
            catch (Exception ex)
            {
                _connection.Close();
                if(MessageBox.Show("Ошибка выполнения SQL запроса." + System.Environment.NewLine + "Показать полное сообщение?","Ошибка:", MessageBoxButtons.YesNo) == DialogResult.Yes)	//Сообщение об ошибке
                {
                    MessageBox.Show(ex.ToString());
                }
                return false; //произошла ошибка.
            }
        }

        public bool ExecuteUpdate(DataSet _DataSet, String _tableName)
        {
            try
            {
                _connection.Open();
                _selectCommand.CommandText = _strSelect;
                _insertCommand.CommandText = _strInsert;
                _updateCommand.CommandText = _strUpdate;
                _deleteCommand.CommandText = _strDelete;
                _dataAdapter.SelectCommand = _selectCommand;
                _dataAdapter.InsertCommand = _insertCommand;
                _dataAdapter.UpdateCommand = _updateCommand;
                _dataAdapter.DeleteCommand = _deleteCommand;
                _dataAdapter.Update(_DataSet, _tableName);
                _connection.Close();
                return true;
            }
            catch (Exception ex)
            {
                _connection.Close();
                if (MessageBox.Show("Ошибка выполнения SQL запроса." + System.Environment.NewLine + "Показать полное сообщение?", "Ошибка:", MessageBoxButtons.YesNo) == DialogResult.Yes)	//Сообщение об ошибке
                {
                    MessageBox.Show(ex.ToString());
                }
                return false; //произошла ошибка.

            }
        }
		
        public void SelectParametersAdd(String parameterName, OleDbType dbType, int size, String sourceColumn)
        {
            _selectCommand.Parameters.Add(parameterName, dbType, size, sourceColumn);
        }

        public void InsertParametersAdd(String parameterName, OleDbType dbType, int size, String sourceColumn)
        {
            _insertCommand.Parameters.Add(parameterName, dbType, size, sourceColumn);
        }

        public void UpdateParametersAdd(String parameterName, OleDbType dbType, int size, String sourceColumn)
        {
            _updateCommand.Parameters.Add(parameterName, dbType, size, sourceColumn);
        }

        public void DeleteParametersAdd(String parameterName, OleDbType dbType, int size, String sourceColumn)
        {
            _deleteCommand.Parameters.Add(parameterName, dbType, size, sourceColumn);
        }
		

    }
}
