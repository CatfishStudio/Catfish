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
			}
			else {
				this.TopMost = true;
				отображатьПоверхОконToolStripMenuItem.Checked = true;
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
		}
		
		void ВыходToolStripMenuItemClick(object sender, EventArgs e)
		{
			Application.Exit(); // закрыть программу
		}
		
		/* Создать папку */
		void СоздатьПапкуToolStripMenuItemClick(object sender, EventArgs e)
		{
			Folder fFolder = new Folder();
			if(this.TopMost) fFolder.TopMost = true;
			fFolder.Show();
		}
		
		/* Создать файл */
		void СоздатьЗаписьToolStripMenuItemClick(object sender, EventArgs e)
		{
			Element fFile = new Element();
			if(this.TopMost) fFile.TopMost = true;
			fFile.Show();
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
		
		
		/* При выборе значения */
		void TreeView1AfterSelect(object sender, TreeViewEventArgs e)
		{
			toolStripStatusLabel3.Text = treeView1.SelectedNode.Text;
			try{
				for(int i = 0; i < _localDataSet.Tables["Хранилище"].Rows.Count; i++){
					/* Если папка */
					if(_localDataSet.Tables["Хранилище"].Rows[i]["ТипОбъекта"].ToString()  == "Группа"){
						if(_localDataSet.Tables["Хранилище"].Rows[i]["ПапкаИдентификатор"].ToString() == treeView1.SelectedNode.Text){
							richTextBox1.Text = "";
							toolStripStatusLabel2.Text = "Папка:";
							break;
						}
					}
					/* Если файл */
					if(_localDataSet.Tables["Хранилище"].Rows[i]["ТипОбъекта"].ToString() == "Элемент"){
						if(_localDataSet.Tables["Хранилище"].Rows[i]["ФайлИдентификатор"].ToString() == treeView1.SelectedNode.Text){
							richTextBox1.Text = _localDataSet.Tables["Хранилище"].Rows[i]["СодержаниеФайла"].ToString();
							toolStripStatusLabel2.Text = "Файл:";
							break;
						}
					}
				}
			}catch{
				richTextBox1.Clear();
				toolStripStatusLabel2.Text = "...";
				toolStripStatusLabel3.Text = "...";
			}
		}
		
		
		/* поиск по ключу */
		void keySearch(String _sqlQuery, ToolStripComboBox _cbox)
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
				keySearch("SELECT ДатаПоследнегоСохранения, ПапкаИдентификатор, СодержаниеФайла, Строка, ТипОбъекта, ФайлВПапке, ФайлИдентификатор FROM Хранилище WHERE ФайлИдентификатор LIKE '%" + toolStripComboBox1.Text + "%' ORDER BY ФайлИдентификатор ASC", toolStripComboBox1);
			}
		}
		
		void ToolStripComboBox1SelectedIndexChanged(object sender, EventArgs e)
		{
			keySearch("SELECT ДатаПоследнегоСохранения, ПапкаИдентификатор, СодержаниеФайла, Строка, ТипОбъекта, ФайлВПапке, ФайлИдентификатор FROM Хранилище WHERE ФайлИдентификатор LIKE '%" + toolStripComboBox1.Text + "%' ORDER BY ФайлИдентификатор ASC", toolStripComboBox1);
		}
		
		/*Тип поиска экспертная система */
		void ToolStripComboBox2KeyPress(object sender, KeyPressEventArgs e)
		{
			if(e.KeyChar.GetHashCode().ToString() == "851981"){
				keySearch("SELECT ДатаПоследнегоСохранения, ПапкаИдентификатор, СодержаниеФайла, Строка, ТипОбъекта, ФайлВПапке, ФайлИдентификатор FROM Хранилище WHERE СодержаниеФайла LIKE '%" + toolStripComboBox2.Text + "%' ORDER BY ФайлИдентификатор ASC", toolStripComboBox2);
			}
		}
		
		void ToolStripComboBox2SelectedIndexChanged(object sender, EventArgs e)
		{
			keySearch("SELECT ДатаПоследнегоСохранения, ПапкаИдентификатор, СодержаниеФайла, Строка, ТипОбъекта, ФайлВПапке, ФайлИдентификатор FROM Хранилище WHERE СодержаниеФайла LIKE '%" + toolStripComboBox2.Text + "%' ORDER BY ФайлИдентификатор ASC", toolStripComboBox2);
		}
		
		
	}
}
