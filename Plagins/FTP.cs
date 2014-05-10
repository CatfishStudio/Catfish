/*
 * Сделано в SharpDevelop.
 * Пользователь: Somov Evgeniy
 * Дата: 04.05.2014
 * Время: 12:14
 * 
 * Для изменения этого шаблона используйте Сервис | Настройка | Кодирование | Правка стандартных заголовков.
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using System.Data.OleDb;

namespace Catfish
{
	/// <summary>
	/// Description of FTP.
	/// </summary>
	public partial class FTP : Form
	{
		public FTP()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		private OleDbServerFull _localClient;
		private DataSet _localDataSet;
				
		/* очистка окна редактирования*/
		void winClear()
		{
			button1.Text = "Соединить.";
			textBox2.Clear();
			textBox3.Clear();
			textBox4.Clear();
			textBox5.Clear();
			richTextBox1.Text = "";
			groupBox2.Text = "...";
			groupBox2.Visible = false;
		}
		
		/* Инициализация базы данных */
		void initBase()
		{
			_localClient = new OleDbServerFull(Config.PathBase);
			_localDataSet = new DataSet();
			_localDataSet.Clear();
			_localDataSet.DataSetName = "FTP";
			/*Запрос SELECT*/
			_localClient.SelectSqlCommand = "SELECT id, ИмяПользователя, ИмяСоединения, Информация, Пароль, СерверИПорт FROM FTP";
			/*Запрос INSERT*/
			_localClient.InsertSqlCommand = "INSERT INTO FTP(ИмяПользователя, ИмяСоединения, Информация, Пароль, СерверИПорт) " +
				" VALUES (@ИмяПользователя, @ИмяСоединения, @Информация, @Пароль, @СерверИПорт)";
			_localClient.InsertParametersAdd("@ИмяПользователя", OleDbType.VarWChar, 255, "ИмяПользователя");
			_localClient.InsertParametersAdd("@ИмяСоединения", OleDbType.VarWChar, 255, "ИмяСоединения");
			_localClient.InsertParametersAdd("@Информация", OleDbType.VarWChar, 0, "Информация");
			_localClient.InsertParametersAdd("@Пароль", OleDbType.VarWChar, 255, "Пароль");
			_localClient.InsertParametersAdd("@СерверИПорт", OleDbType.VarWChar, 255, "СерверИПорт");
			_localClient.InsertParametersAdd("@id", OleDbType.Integer, 0, "id");
			/*Запрос UPDATE*/
			_localClient.UpdateSqlCommand = "UPDATE FTP SET ИмяПользователя=@ИмяПользователя, ИмяСоединения=@ИмяСоединения, Информация=@Информация, Пароль=@Пароль, СерверИПорт=@СерверИПорт WHERE (id = @id)";
			_localClient.UpdateParametersAdd("@ИмяПользователя", OleDbType.VarWChar, 255, "ИмяПользователя");
			_localClient.UpdateParametersAdd("@ИмяСоединения", OleDbType.VarWChar, 255, "ИмяСоединения");
			_localClient.UpdateParametersAdd("@Информация", OleDbType.VarWChar, 0, "Информация");
			_localClient.UpdateParametersAdd("@Пароль", OleDbType.VarWChar, 255, "Пароль");
			_localClient.UpdateParametersAdd("@СерверИПорт", OleDbType.VarWChar, 255, "СерверИПорт");
			_localClient.UpdateParametersAdd("@id", OleDbType.Integer, 0, "id");
			/*Запрос DELETE*/
			_localClient.DeleteSqlCommand = "DELETE FROM FTP WHERE (id = @id)";
			_localClient.UpdateParametersAdd("@id", OleDbType.Integer, 0, "id");
			
		}
		
		/* Обновления списка подключений */
		void updateList()
		{
			listBox1.Items.Clear();
			_localDataSet.Clear();
			/* Загрузка данных */
			_localClient.SelectSqlCommand = "SELECT id, ИмяПользователя, ИмяСоединения, Информация, Пароль, СерверИПорт FROM FTP";
			if(_localClient.ExecuteFill(_localDataSet, "FTP")){
				for(int i = 0; i < _localDataSet.Tables["FTP"].Rows.Count; i++)
					listBox1.Items.Add(_localDataSet.Tables["FTP"].Rows[i]["ИмяСоединения"].ToString());
			}
			
		}
		
		void FTPLoad(object sender, EventArgs e)
		{
			winClear(); // очистка окна
			initBase(); // инициализация базы данных
			updateList(); // загрузка списка подключений
		}
		
		/* При выборе соединения */
		void ListBox1SelectedIndexChanged(object sender, EventArgs e)
		{
			String _name = "", _user = "", _pass = "", _server = "";
			if(listBox1.SelectedIndex >= 0){
				for(int i = 0; i < _localDataSet.Tables["FTP"].Rows.Count; i++){
					if(_localDataSet.Tables["FTP"].Rows[i]["ИмяСоединения"].ToString() == listBox1.Items[listBox1.SelectedIndex].ToString()){
						_name = _localDataSet.Tables["FTP"].Rows[i]["ИмяСоединения"].ToString();
						_user = _localDataSet.Tables["FTP"].Rows[i]["ИмяПользователя"].ToString();
						_pass = _localDataSet.Tables["FTP"].Rows[i]["Пароль"].ToString();
						_server = _localDataSet.Tables["FTP"].Rows[i]["СерверИПорт"].ToString();
						break;
					}
				}
				textBox1.Text = "ftp://" + _user + ":" + _pass + "@" + _server;
			}
		}
		
		/* FTP соединение через проводник */
		void Button1Click(object sender, EventArgs e)
		{
			if(groupBox2.Visible == false){
				try{
					System.Diagnostics.Process.Start("explorer.exe", textBox1.Text);
				}catch{
					MessageBox.Show("Ошибка соединения через FTP.", "Сообщение:", MessageBoxButtons.OK);
				}
			}else{
				if(groupBox2.Text == "Новая запись"){
					saveNew(); // создаём новую запись
				}else{
					saveEdit(); // сохраняем изменения
				}
			}
		}
		
		/* Сохранить новую запись */
		void saveNew()
		{
			DataRow _newRow = _localDataSet.Tables["FTP"].NewRow();
			_newRow["ИмяСоединения"] = textBox2.Text;
			_newRow["ИмяПользователя"] = textBox3.Text;
			_newRow["Пароль"] = textBox4.Text;
			_newRow["СерверИПорт"] = textBox5.Text;
			_newRow["Информация"] = richTextBox1.Text;
			_localDataSet.Tables["FTP"].Rows.Add(_newRow);
			if(_localClient.ExecuteUpdate(_localDataSet, "FTP")){
				updateList();
				winClear();
				textBox1.Text = "ftp://";
				MessageBox.Show("Сохранение прошло успешно!","Сообщение:",MessageBoxButtons.OK);
			}else MessageBox.Show("Ошибка сохранения!","Сообщение:",MessageBoxButtons.OK);
		}
		
		/* Сохранить изменения */
		void saveEdit()
		{
			_localDataSet.Tables["FTP"].Rows[0]["ИмяСоединения"] = textBox2.Text;
			_localDataSet.Tables["FTP"].Rows[0]["ИмяПользователя"] = textBox3.Text;
			_localDataSet.Tables["FTP"].Rows[0]["Пароль"] = textBox4.Text;
			_localDataSet.Tables["FTP"].Rows[0]["СерверИПорт"] = textBox5.Text;
			_localDataSet.Tables["FTP"].Rows[0]["Информация"] = richTextBox1.Text;
			if(_localClient.ExecuteUpdate(_localDataSet, "FTP")){
				updateList();
				winClear();
				textBox1.Text = "ftp://";
				MessageBox.Show("Сохранение прошло успешно!","Сообщение:",MessageBoxButtons.OK);
			}else MessageBox.Show("Ошибка сохранения!","Сообщение:",MessageBoxButtons.OK);
		}
		
		/* Удалить запись */
		void delete(String _valueDalete)
		{
			if(listBox1.Items.Count > 0){
				if(MessageBox.Show("Удалить выбранную запись?", "Вопрос:", MessageBoxButtons.YesNo)== DialogResult.Yes){
					OleDbServerShort _localClientShort = new OleDbServerShort(Config.PathBase);
					_localClientShort.SqlCommand = "DELETE FROM FTP WHERE (ИмяСоединения = '" + _valueDalete + "')";
					if(_localClientShort.ExecuteNonQuery()){
						updateList();
						winClear();
						textBox1.Text = "ftp://";
						MessageBox.Show("Удаление прошло успешно!","Сообщение:",MessageBoxButtons.OK);
					}else MessageBox.Show("Ошибка удаления!","Сообщение:",MessageBoxButtons.OK);
				}
			}
		}
		
		void СоздатьЗаписьToolStripMenuItemClick(object sender, EventArgs e)
		{
			winClear();
			groupBox2.Text = "Новая запись";
			groupBox2.Visible = true;
			button1.Text = "Сохранить.";
		}
		
		/* Редактировать запись */
		void ИзменитьЗаписьToolStripMenuItemClick(object sender, EventArgs e)
		{
			if(listBox1.Items.Count > 0){
				winClear();
				_localDataSet.Clear();
				_localClient.SelectSqlCommand = "SELECT id, ИмяПользователя, ИмяСоединения, Информация, Пароль, СерверИПорт FROM FTP WHERE (ИмяСоединения = '" + listBox1.Items[listBox1.SelectedIndex].ToString() + "')";
				if(_localClient.ExecuteFill(_localDataSet, "FTP")){
					textBox2.Text = _localDataSet.Tables["FTP"].Rows[0]["ИмяСоединения"].ToString();
					textBox3.Text = _localDataSet.Tables["FTP"].Rows[0]["ИмяПользователя"].ToString();
					textBox4.Text = _localDataSet.Tables["FTP"].Rows[0]["Пароль"].ToString();
					textBox5.Text = _localDataSet.Tables["FTP"].Rows[0]["СерверИПорт"].ToString();
					richTextBox1.Text = _localDataSet.Tables["FTP"].Rows[0]["Информация"].ToString();
					groupBox2.Text = "Изменить запись";
					groupBox2.Visible = true;
					button1.Text = "Сохранить.";
				}
			}
		}
		
		/* Удалить запись */
		void УдалитьЗаписьToolStripMenuItemClick(object sender, EventArgs e)
		{
			delete(listBox1.Items[listBox1.SelectedIndex].ToString());
		}
		
		/* Закрыть */
		void Button2Click(object sender, EventArgs e)
		{
			if(groupBox2.Visible){
				updateList();
				winClear();
			}else Close();
		}
		
		/* При нажатии на ссылку */	
		void RichTextBox1LinkClicked(object sender, LinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start(e.LinkText);
		}
	}
}
