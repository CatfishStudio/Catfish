using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Catfish
{
    class OleDbServerShort
    {
        /* Переменные */
        private OleDbConnection _connection;
        private OleDbCommand _command;
        private String _sqlCommand;

        /* Конструктор */
        public OleDbServerShort(String PathBase)
        {
        	_connection = new OleDbConnection();
            _connection.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathBase + ";Jet OLEDB:Database Password=";
            _command = new OleDbCommand("", _connection);
        }

        /* свойства */
        public String SqlCommand
        {
            get { return _sqlCommand; }
            set { _sqlCommand = value; }
        }

        /* Методы */
        public bool ExecuteNonQuery()
        {
            try
            {
                _connection.Open();
                _command.CommandText = _sqlCommand;
                _command.ExecuteNonQuery();	//выполнение запроса
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
    }
}
