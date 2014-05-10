/*
 * Сделано в SharpDevelop.
 * Пользователь: Somov Evgeniy
 * Дата: 04.05.2014
 * Время: 9:30
 * 
 * Для изменения этого шаблона используйте Сервис | Настройка | Кодирование | Правка стандартных заголовков.
 */
using System;
using System.Drawing;
using System.Security.AccessControl;
using System.Windows.Forms;
using System.Data;
using System.Data.OleDb;

namespace Catfish
{
	/// <summary>
	/// Description of Folder.
	/// </summary>
	public partial class Folder : Form
	{
		public Folder()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		public MainForm mForm;
		private OleDbServerFull _localClient;
		private DataSet _localDataSet;
		
		/* Закрыть */
		void Button2Click(object sender, EventArgs e)
		{
			Close();
		}
		
		/* Проверка уникальности */
		bool uniquenessCheck(String _oldValue, String _newValue)
		{
			if(_oldValue != _newValue){
				OleDbServerFull _localClientCheck = new OleDbServerFull(Config.PathBase);
				DataSet _localDataSetCheck = new DataSet();
				_localClient.SelectSqlCommand = "SELECT ДатаПоследнегоСохранения, ПапкаИдентификатор, СодержаниеФайла, Строка, ТипОбъекта, ФайлВПапке, ФайлИдентификатор FROM Хранилище WHERE (ПапкаИдентификатор = '" + _newValue + "' OR ФайлИдентификатор = '" + _newValue + "')";
				if(_localClient.ExecuteFill(_localDataSetCheck, "Хранилище")){
					if(_localDataSetCheck.Tables["Хранилище"].Rows.Count > 0) return false; // такое имя уже есть (запрет)
					else return true; // повторений имени нет (разрешено)
				}else return false; // на случай ошибки (запрет)
			}else return true; // имя не изменилось (разрешено)
		}
		
		/* Сохранение в базе данных */
		void Button1Click(object sender, EventArgs e)
		{
			if(this.Text == "Новая папка"){
				/* Сохраняем новые данные */
				if(uniquenessCheck(this.Text, textBox1.Text)){ // проверка уникальности
					DataRow _newRow = _localDataSet.Tables["Хранилище"].NewRow();
					_newRow["ТипОбъекта"] = "Группа";
					_newRow["ПапкаИдентификатор"] = textBox1.Text;
					_newRow["СодержаниеФайла"] = "";
					_newRow["ДатаПоследнегоСохранения"] = DateTime.Today;
					_newRow["ФайлИдентификатор"] = "Папка " + textBox1.Text;
					_newRow["ФайлВПапке"] = "";
					_localDataSet.Tables["Хранилище"].Rows.Add(_newRow);
					if(_localClient.ExecuteUpdate(_localDataSet, "Хранилище")){
						MessageBox.Show("Сохранение прошло успешно!","Сообщение:",MessageBoxButtons.OK);
						Close();
						mForm.ShowAll();
					}else MessageBox.Show("Ошибка сохранения!","Сообщение:",MessageBoxButtons.OK);
				} else MessageBox.Show("Папка или файл с таким именем уже существуют!","Сообщение:",MessageBoxButtons.OK);
			}else{
				/* Изменение существующей записи */
				if(uniquenessCheck(this.Text, textBox1.Text)){ // проверка уникальности
					_localDataSet.Tables["Хранилище"].Rows[0]["ПапкаИдентификатор"] = textBox1.Text;
					_localDataSet.Tables["Хранилище"].Rows[0]["ФайлИдентификатор"] = "Папка " + textBox1.Text;
				
					if(_localClient.ExecuteUpdate(_localDataSet, "Хранилище")){
					
						/* Редактируем файлы */
						OleDbServerShort _localClientShort = new OleDbServerShort(Config.PathBase);
						_localClientShort.SqlCommand = "UPDATE Хранилище SET ФайлВПапке = '" + textBox1.Text + "' WHERE (ФайлВПапке = '" + this.Text + "')";
						if(_localClientShort.ExecuteNonQuery()){
							MessageBox.Show("Сохранение прошло успешно!","Сообщение:",MessageBoxButtons.OK);
							Close();
							mForm.ShowAll();
						}else{
						
							// была ошибка, возвращаем всё обратно
							_localDataSet.Tables["Хранилище"].Rows[0]["ПапкаИдентификатор"] = this.Text;
							if(_localClient.ExecuteUpdate(_localDataSet, "Хранилище")) MessageBox.Show("Данные успешно восстановлены!","Сообщение:",MessageBoxButtons.OK);
							else MessageBox.Show("Критическая ошибка! Данные восстановлению не подлежат.","Сообщение:",MessageBoxButtons.OK);						
						}
					}else MessageBox.Show("Ошибка сохранения!","Сообщение:",MessageBoxButtons.OK);
				} else MessageBox.Show("Папка или файл с таким именем уже существуют!","Сообщение:",MessageBoxButtons.OK);
			}
		}
		
		/* Открытие окна: инициализация обработки базы данных */
		void FolderLoad(object sender, EventArgs e)
		{
			/* Инициализация базы данных */
			_localClient = new OleDbServerFull(Config.PathBase);
			_localDataSet = new DataSet();
			_localDataSet.Clear();
			_localDataSet.DataSetName = "Хранилище";
			/*Запрос SELECT*/
			if(this.Text == "Новая папка") _localClient.SelectSqlCommand = "SELECT ДатаПоследнегоСохранения, ПапкаИдентификатор, СодержаниеФайла, Строка, ТипОбъекта, ФайлВПапке, ФайлИдентификатор FROM Хранилище";
			else _localClient.SelectSqlCommand = "SELECT ДатаПоследнегоСохранения, ПапкаИдентификатор, СодержаниеФайла, Строка, ТипОбъекта, ФайлВПапке, ФайлИдентификатор FROM Хранилище WHERE (ПапкаИдентификатор = '" + this.Text + "')";
			/*Запрос INSERT*/
			_localClient.InsertSqlCommand = "INSERT INTO Хранилище (ДатаПоследнегоСохранения, ПапкаИдентификатор, СодержаниеФайла, ТипОбъекта, ФайлВПапке, ФайлИдентификатор)" +
				" VALUES (@ДатаПоследнегоСохранения, @ПапкаИдентификатор, @СодержаниеФайла, @ТипОбъекта, @ФайлВПапке, @ФайлИдентификатор)";
			_localClient.InsertParametersAdd("@ДатаПоследнегоСохранения", OleDbType.Date, 0, "ДатаПоследнегоСохранения");
			_localClient.InsertParametersAdd("@ПапкаИдентификатор", OleDbType.VarChar, 255, "ПапкаИдентификатор");
			_localClient.InsertParametersAdd("@СодержаниеФайла", OleDbType.LongVarWChar, 0, "СодержаниеФайла");
			_localClient.InsertParametersAdd("@ТипОбъекта", OleDbType.VarChar, 255, "ТипОбъекта");
			_localClient.InsertParametersAdd("@ФайлВПапке", OleDbType.VarChar, 255, "ФайлВПапке");
			_localClient.InsertParametersAdd("@ФайлИдентификатор", OleDbType.VarChar, 255, "ФайлИдентификатор");
			_localClient.InsertParametersAdd("@Строка", OleDbType.Integer, 0, "Строка");
			/*Запрос UPDATE*/
			_localClient.UpdateSqlCommand = "UPDATE Хранилище SET ДатаПоследнегоСохранения=@ДатаПоследнегоСохранения, ПапкаИдентификатор=@ПапкаИдентификатор, СодержаниеФайла=@СодержаниеФайла, ТипОбъекта=@ТипОбъекта, ФайлВПапке=@ФайлВПапке, ФайлИдентификатор=@ФайлИдентификатор WHERE (Строка = @Строка)";
			_localClient.UpdateParametersAdd("@ДатаПоследнегоСохранения", OleDbType.Date, 0, "ДатаПоследнегоСохранения");
			_localClient.UpdateParametersAdd("@ПапкаИдентификатор", OleDbType.VarChar, 255, "ПапкаИдентификатор");
			_localClient.UpdateParametersAdd("@СодержаниеФайла", OleDbType.LongVarWChar, 0, "СодержаниеФайла");
			_localClient.UpdateParametersAdd("@ТипОбъекта", OleDbType.VarChar, 255, "ТипОбъекта");
			_localClient.UpdateParametersAdd("@ФайлВПапке", OleDbType.VarChar, 255, "ФайлВПапке");
			_localClient.UpdateParametersAdd("@ФайлИдентификатор", OleDbType.VarChar, 255, "ФайлИдентификатор");
			_localClient.UpdateParametersAdd("@Строка", OleDbType.Integer, 0, "Строка");
			/*Запрос DELETE*/
			_localClient.DeleteSqlCommand = "DELETE FROM Хранилище WHERE (Строка = @Строка)";
			_localClient.UpdateParametersAdd("@Строка", OleDbType.Integer, 0, "Строка");
			
			/* Загрузка данных */
			if(_localClient.ExecuteFill(_localDataSet, "Хранилище")){
					if(this.Text != "Новая папка") textBox1.Text = _localDataSet.Tables["Хранилище"].Rows[0]["ПапкаИдентификатор"].ToString();
			}
			
		}
	}
}
