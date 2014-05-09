/*
 * Сделано в SharpDevelop.
 * Пользователь: Somov Evgeniy
 * Дата: 21.04.2014
 * Время: 16:54
 * 
 * Для изменения этого шаблона используйте Сервис | Настройка | Кодирование | Правка стандартных заголовков.
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Data;

namespace Catfish
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
			
			/* инициализация события закрытия окна (System.ComponentModel) */
			this.Closing += new CancelEventHandler(this.MainFormClosing);
			
		}
		
		private OleDbServerFull _localClient;
		private DataSet _localDataSet;
		private DataSet _localDataSet2;
		
		void ВызватьПомошникаToolStripMenuItemClick(object sender, EventArgs e)
		{
			this.Visible = true; // показать скрытое окно			
		}
		
		void БлокнотToolStripMenuItemClick(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start("notepad.exe"); // блокнот
		}
		
		void ОтображатьПоверхОконToolStripMenuItemClick(object sender, System.EventArgs e)
		{
			/* Опция: поверх всех окон */
			if(this.TopMost){
				this.TopMost = false;
				отображатьПоверхОконToolStripMenuItem.Checked = false;
				отображатьПоверхОконToolStripMenuItem1.Checked = false;
			}
			else {
				this.TopMost = true;
				отображатьПоверхОконToolStripMenuItem.Checked = true;
				отображатьПоверхОконToolStripMenuItem1.Checked = false;
			}
		}		
		
		void КалькуляторToolStripMenuItemClick(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start("calc.exe"); // калькулятор		
		}
		
		void MainFormClosing(object sender, CancelEventArgs e)
		{
			/* программа не закрывается, а скрывается */
			e.Cancel = true;
			this.Visible = false;
		}
		
		void MainFormLoad(object sender, EventArgs e)
		{
			/* Конфигурация: */
			Config.MyProgramDirectory = Environment.CurrentDirectory + "\\"; // путь к программе
			Config.PathBase = Config.MyProgramDirectory + "resource\\resource.mdb";	// путь к базе	
			/* Инициализация базы данных */
			_localClient = new OleDbServerFull(Config.PathBase);
			_localDataSet = new DataSet();
			_localDataSet2 = new DataSet();
			/* Меню дерева */
			treeviewMenu("");
		}
		
		void ВыходToolStripMenuItemClick(object sender, EventArgs e)
		{
			Application.Exit(); // закрыть программу
		}
		
		/* Создать папку */
		void СоздатьПапкуToolStripMenuItemClick(object sender, EventArgs e)
		{
			openFolder(true);	
		}
		
		void СоздатьПапкуToolStripMenuItem1Click(object sender, EventArgs e)
		{
			openFolder(true);
		}
		
		/* Редактировать папку */		
		void РедактироватьПапкуToolStripMenuItemClick(object sender, EventArgs e)
		{
			openFolder(false);
		}
		
		/* Удалить папку */
		void deleteFolder(String _folderName)
		{
			
			if(MessageBox.Show("Удалить папку [" + _folderName + "] и всё её содержимое?","Вопрос:", MessageBoxButtons.YesNo) == DialogResult.Yes){
				OleDbServerShort _localClientShort = new OleDbServerShort(Config.PathBase);
				/* Удаление содержимого */
				_localClientShort.SqlCommand = "DELETE FROM Хранилище WHERE (ФайлВПапке = '" + _folderName + "')";
				if(_localClientShort.ExecuteNonQuery()){
					/* удаление самой папки */
					_localClientShort.SqlCommand = "DELETE FROM Хранилище WHERE (ПапкаИдентификатор = '" + _folderName + "')";
					if(_localClientShort.ExecuteNonQuery()){
						ShowAll();
						MessageBox.Show("Папка успешно удалена!","Сообщение:",MessageBoxButtons.OK);
					}else{
						MessageBox.Show("Ошибка удаления папки!","Сообщение:",MessageBoxButtons.OK);
					}
				}else MessageBox.Show("Ошибка удаления содержимого в папке!","Сообщение:",MessageBoxButtons.OK);
			}
		}
		
		void УдалитьПапкуToolStripMenuItemClick(object sender, EventArgs e)
		{
			deleteFolder(toolStripStatusLabel3.Text);
		}
		
		
		/* Удаление папки горячей клавишей */
		
		void TreeView1KeyDown(object sender, KeyEventArgs e)
		{
			if(e.KeyData == Keys.Delete){
				if(toolStripStatusLabel5.Text == "...") deleteFolder(toolStripStatusLabel3.Text);
				else deleteFile(toolStripStatusLabel5.Text);
			}
		}
		
		
		/* Создать файл */
		void СоздатьЗаписьToolStripMenuItemClick(object sender, EventArgs e)
		{
			openFile(true);
		}
				
		void СоздатьФайлToolStripMenuItemClick(object sender, EventArgs e)
		{
			openFile(true);
		}
		
		/* Редактировать файл */
		void ОткрытьФайлToolStripMenuItemClick(object sender, EventArgs e)
		{
			openFile(false);
		}
		
		/* Удалить файл */
		void deleteFile(String _folderName)
		{
			if(MessageBox.Show("Удалить файл [" + _folderName + "] ?","Вопрос:", MessageBoxButtons.YesNo) == DialogResult.Yes){
				OleDbServerShort _localClientShort = new OleDbServerShort(Config.PathBase);
				_localClientShort.SqlCommand = "DELETE FROM Хранилище WHERE (ФайлИдентификатор = '" + _folderName + "')";
				if(_localClientShort.ExecuteNonQuery()){
					ShowAll();
					MessageBox.Show("Файл успешно удален!","Сообщение:",MessageBoxButtons.OK);
				}else{
					MessageBox.Show("Ошибка удаления файла!","Сообщение:",MessageBoxButtons.OK);
				}
			}
		}
		
		void УдалитьФайлToolStripMenuItemClick(object sender, EventArgs e)
		{
			deleteFile(toolStripStatusLabel5.Text);
		}
		
		/* Редактор */
		void РедакторToolStripMenuItemClick(object sender, EventArgs e)
		{
			Editor fEditor = new Editor();
			fEditor.Show();
		}
		
		/* FTP клиент */
		void FTPКлиентToolStripMenuItemClick(object sender, EventArgs e)
		{
			FTP fFTP = new FTP();
			if(this.TopMost) fFTP.TopMost = true;
			fFTP.Show();
		}
		
		
		/* Меню дерева */
		void treeviewMenu(String _type)
		{
			if(_type == "Папка:"){
				создатьПапкуToolStripMenuItem1.Visible = true;
				редактироватьПапкуToolStripMenuItem.Visible = true;
				удалитьПапкуToolStripMenuItem.Visible = true;
				создатьФайлToolStripMenuItem.Visible = true;
				открытьФайлToolStripMenuItem.Visible = false;
				удалитьФайлToolStripMenuItem.Visible = false;
				toolStripButton5.Enabled = false;
				toolStripButton4.Enabled = true;
			}
			if(_type == "Файл:"){
				создатьФайлToolStripMenuItem.Visible = true;
				открытьФайлToolStripMenuItem.Visible = true;
				удалитьФайлToolStripMenuItem.Visible = true;
				создатьПапкуToolStripMenuItem1.Visible = true;
				редактироватьПапкуToolStripMenuItem.Visible = false;
				удалитьПапкуToolStripMenuItem.Visible = false;
				toolStripButton5.Enabled = true;
				toolStripButton4.Enabled = false;
			}
			if(_type == ""){
				создатьПапкуToolStripMenuItem1.Visible = true;
				редактироватьПапкуToolStripMenuItem.Visible = false;
				удалитьПапкуToolStripMenuItem.Visible = false;
				создатьФайлToolStripMenuItem.Visible = true;
				открытьФайлToolStripMenuItem.Visible = false;
				удалитьФайлToolStripMenuItem.Visible = false;
				toolStripButton5.Enabled = false;
				toolStripButton4.Enabled = false;
				toolStripStatusLabel2.Text = "Папка:";
				toolStripStatusLabel3.Text = "...";
				toolStripStatusLabel4.Text = "Файл:";
				toolStripStatusLabel5.Text = "...";
			}
		}
		
		/* При открытии меню дерева */
		void ContextMenuStrip2Opening(object sender, CancelEventArgs e)
		{
			if(treeView1.Nodes.Count <= 0) treeviewMenu("");
		}
		
		/* При выборе значения в дереве */
		void TreeView1AfterSelect(object sender, TreeViewEventArgs e)
		{
			try{
				for(int i = 0; i < _localDataSet.Tables["Хранилище"].Rows.Count; i++){
					/* Если папка */
					if(_localDataSet.Tables["Хранилище"].Rows[i]["ТипОбъекта"].ToString()  == "Группа"){
						if(_localDataSet.Tables["Хранилище"].Rows[i]["ПапкаИдентификатор"].ToString() == treeView1.SelectedNode.Text){
							richTextBox1.Text = "";
							toolStripStatusLabel2.Text = "Папка:";
							toolStripStatusLabel3.Text = _localDataSet.Tables["Хранилище"].Rows[i]["ПапкаИдентификатор"].ToString();
							toolStripStatusLabel4.Text = "Файл:";
							toolStripStatusLabel5.Text = "...";
							treeviewMenu("Папка:");
							break;
						}
					}
					/* Если файл */
					if(_localDataSet.Tables["Хранилище"].Rows[i]["ТипОбъекта"].ToString() == "Элемент"){
						if(_localDataSet.Tables["Хранилище"].Rows[i]["ФайлИдентификатор"].ToString() == treeView1.SelectedNode.Text){
							richTextBox1.Text = _localDataSet.Tables["Хранилище"].Rows[i]["СодержаниеФайла"].ToString();
							toolStripStatusLabel2.Text = "Папка:";
							toolStripStatusLabel3.Text = _localDataSet.Tables["Хранилище"].Rows[i]["ФайлВПапке"].ToString();
							toolStripStatusLabel4.Text = "Файл:";
							toolStripStatusLabel5.Text = _localDataSet.Tables["Хранилище"].Rows[i]["ФайлИдентификатор"].ToString();
							treeviewMenu("Файл:");
							break;
						}
					}
				}
			}catch{
				richTextBox1.Clear();
				toolStripStatusLabel2.Text = "Папка:";
				toolStripStatusLabel3.Text = "...";
				toolStripStatusLabel4.Text = "Файл:";
				toolStripStatusLabel5.Text = "...";
			}
		}
		
		
		/* поиск по ключу */
		public void KeySearch(String _sqlQuery, ToolStripComboBox _cbox)
		{
			bool resolution = true;
			try{
				for(int k = 0; k < _cbox.Items.Count; k++)
					if(_cbox.Items[k].ToString() == _cbox.Text) resolution = false;
				if(resolution) _cbox.Items.Add(_cbox.Text);
				
				_localDataSet.Clear(); // очистка
								
				_localClient.SelectSqlCommand = _sqlQuery;
				_localClient.ExecuteFill(_localDataSet, "Хранилище");
				// Загрузка дерева даных
				String NameGr = "";
				treeView1.Nodes.Clear();
				for(int i = 0; i < _localDataSet.Tables["Хранилище"].Rows.Count; i++){
						
					if(_localDataSet.Tables["Хранилище"].Rows[i]["ТипОбъекта"].ToString() == "Элемент"){
						NameGr = _localDataSet.Tables["Хранилище"].Rows[i]["ФайлИдентификатор"].ToString();
						treeView1.Nodes.Add(NameGr);
						treeView1.Nodes[treeView1.Nodes.Count-1].ImageIndex = 2;
						treeView1.Nodes[treeView1.Nodes.Count-1].SelectedImageIndex = 2;
					}
						
				}
				treeView1.Select();
				if(treeView1.Nodes.Count <= 0) treeviewMenu("");
			}catch{
				treeView1.Nodes.Clear();
				_localDataSet.Clear();
				richTextBox1.Clear();
			}
		}
		
		
		/* тип поиска по имени файла */
		void ToolStripComboBox1KeyPress(object sender, KeyPressEventArgs e)
		{
			if(e.KeyChar.GetHashCode().ToString() == "851981"){
				KeySearch("SELECT ДатаПоследнегоСохранения, ПапкаИдентификатор, СодержаниеФайла, Строка, ТипОбъекта, ФайлВПапке, ФайлИдентификатор FROM Хранилище WHERE ФайлИдентификатор LIKE '%" + toolStripComboBox1.Text + "%' ORDER BY ФайлИдентификатор ASC", toolStripComboBox1);
			}
		}
		
		void ToolStripComboBox1SelectedIndexChanged(object sender, EventArgs e)
		{
			KeySearch("SELECT ДатаПоследнегоСохранения, ПапкаИдентификатор, СодержаниеФайла, Строка, ТипОбъекта, ФайлВПапке, ФайлИдентификатор FROM Хранилище WHERE ФайлИдентификатор LIKE '%" + toolStripComboBox1.Text + "%' ORDER BY ФайлИдентификатор ASC", toolStripComboBox1);
		}
		
		/*Тип поиска экспертная система */
		void ToolStripComboBox2KeyPress(object sender, KeyPressEventArgs e)
		{
			if(e.KeyChar.GetHashCode().ToString() == "851981"){
				KeySearch("SELECT ДатаПоследнегоСохранения, ПапкаИдентификатор, СодержаниеФайла, Строка, ТипОбъекта, ФайлВПапке, ФайлИдентификатор FROM Хранилище WHERE СодержаниеФайла LIKE '%" + toolStripComboBox2.Text + "%' ORDER BY ФайлИдентификатор ASC", toolStripComboBox2);
			}
		}
		
		void ToolStripComboBox2SelectedIndexChanged(object sender, EventArgs e)
		{
			KeySearch("SELECT ДатаПоследнегоСохранения, ПапкаИдентификатор, СодержаниеФайла, Строка, ТипОбъекта, ФайлВПапке, ФайлИдентификатор FROM Хранилище WHERE СодержаниеФайла LIKE '%" + toolStripComboBox2.Text + "%' ORDER BY ФайлИдентификатор ASC", toolStripComboBox2);
		}
		
		
		/* Показать всё содержимое */
		public void ShowAll()
		{
			try{
				_localDataSet.Clear();
				_localDataSet2.Clear();
			
				_localClient.SelectSqlCommand = "SELECT ДатаПоследнегоСохранения, ПапкаИдентификатор, СодержаниеФайла, Строка, ТипОбъекта, ФайлВПапке, ФайлИдентификатор FROM Хранилище ORDER BY ПапкаИдентификатор ASC";
				_localClient.ExecuteFill(_localDataSet, "Хранилище");
			
				_localClient.SelectSqlCommand = "SELECT ДатаПоследнегоСохранения, ПапкаИдентификатор, СодержаниеФайла, Строка, ТипОбъекта, ФайлВПапке, ФайлИдентификатор FROM Хранилище ORDER BY Строка ASC";
				_localClient.ExecuteFill(_localDataSet2, "Хранилище");
			
				//Загрузка дерева
				String _nameGr = "";
				int _actionGr = 0;
				int _actionEl = 0;
				bool _nextGr = false;
			
				treeView1.Nodes.Clear();
				for(int i = 0; i < _localDataSet.Tables["Хранилище"].Rows.Count; i++){
					if(_localDataSet.Tables["Хранилище"].Rows[i]["ТипОбъекта"].ToString() == "Группа"){
						_nameGr = _localDataSet.Tables["Хранилище"].Rows[i]["ПапкаИдентификатор"].ToString();
						treeView1.Nodes.Add(_nameGr);
						treeView1.Nodes[_actionGr].ImageIndex = 0;
						treeView1.Nodes[_actionGr].SelectedImageIndex = 1;
						_actionGr++;
						_nextGr = true;
					}
				
					if(_nextGr){
						_actionEl = 0;
						for(int j = 0; j < _localDataSet2.Tables["Хранилище"].Rows.Count; j++){
							if(_localDataSet2.Tables["Хранилище"].Rows[j]["ТипОбъекта"].ToString() == "Элемент" && _localDataSet2.Tables["Хранилище"].Rows[j]["ФайлВПапке"].ToString() == _nameGr){
								treeView1.Nodes[_actionGr - 1].Nodes.Add(_localDataSet2.Tables["Хранилище"].Rows[j]["ФайлИдентификатор"].ToString());
								treeView1.Nodes[_actionGr - 1].Nodes[_actionEl].ImageIndex = 2;
								treeView1.Nodes[_actionGr - 1].Nodes[_actionEl].SelectedImageIndex = 2;
								_actionEl++;
							}
						}
						_nextGr = false;
					}
				}
				treeView1.Select();
			}catch{
				MessageBox.Show("Произошла ошибка в момент выгрузки всех данных.","Ошибка!",MessageBoxButtons.OK);
			}
		}
		
		void ПоказатьToolStripMenuItemClick(object sender, EventArgs e)
		{
			ShowAll();
		}
		
		void ToolStripButton3Click(object sender, EventArgs e)
		{
			ShowAll();
		}
		
		/* Создать папку */
		void openFolder(bool _new){
			Folder fFolder = new Folder();
			if(this.TopMost) fFolder.TopMost = true;
			if(_new){
				fFolder.Text = "Новая папка";
			}else{
				fFolder.Text = toolStripStatusLabel3.Text; // имя папки
				//fFolder.textBox1.Text = toolStripStatusLabel3.Text; // имя папки
			}
			fFolder.mForm = this;
			fFolder.Show();
		}
		
		void ToolStripButton1Click(object sender, EventArgs e)
		{
			openFolder(true);
		}
		
		/* Редактировать папку */
		void ToolStripButton4Click(object sender, EventArgs e)
		{
			openFolder(false);		
		}
		
		/* создать файл */
		void openFile(bool _new)
		{
			Element fFile = new Element();
			if(this.TopMost) fFile.TopMost = true;
			if(_new){
				fFile.Text = "Новый файл";
				fFile.comboBox1.Text = toolStripStatusLabel3.Text; // имя папки
			}else{
				fFile.Text = toolStripStatusLabel5.Text; // имя файла
				//fFile.textBox1.Text = toolStripStatusLabel5.Text; // имя файла
				fFile.comboBox1.Text = toolStripStatusLabel3.Text; // имя папки
			}
			fFile.mForm = this;
			fFile.Show();
		}
		
		void ToolStripButton2Click(object sender, EventArgs e)
		{
			openFile(true);
		}
				
		/* Редактировать файл */
		void ToolStripButton5Click(object sender, EventArgs e)
		{
			openFile(false);			
		}
		
		/* Блокнот */
		void БлокнотToolStripMenuItem1Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start("notepad.exe"); // блокнот
		}
		
		/* Калькулятор */
		void КалькуляторToolStripMenuItem1Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start("calc.exe"); // калькулятор
		}
		
		/* Редактор */
		void РедакторToolStripMenuItem1Click(object sender, EventArgs e)
		{
			Editor fEditor = new Editor();
			fEditor.Show();
		}
		
		/* FTP клиент */
		void FTPклиентToolStripMenuItem1Click(object sender, EventArgs e)
		{
			FTP fFTP = new FTP();
			if(this.TopMost) fFTP.TopMost = true;
			fFTP.Show();
		}
		
		
		/* Настройки */
		void ОтображатьПоверхОконToolStripMenuItem1Click(object sender, EventArgs e)
		{
			/* Опция: поверх всех окон */
			if(this.TopMost){
				this.TopMost = false;
				отображатьПоверхОконToolStripMenuItem.Checked = false;
				отображатьПоверхОконToolStripMenuItem1.Checked = false;
			}
			else {
				this.TopMost = true;
				отображатьПоверхОконToolStripMenuItem.Checked = true;
				отображатьПоверхОконToolStripMenuItem1.Checked = false;
			}
		}
		
		/* Закрыть программу */
		void ToolStripButton7Click(object sender, EventArgs e)
		{
			Application.Exit();
		}
		
		
		
		
	}
}
