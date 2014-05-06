﻿/*
 * Сделано в SharpDevelop.
 * Пользователь: Catfish
 * Дата: 04.05.2014
 * Время: 9:51
 * 
 * Для изменения этого шаблона используйте Сервис | Настройка | Кодирование | Правка стандартных заголовков.
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Text;

namespace Catfish
{
	/// <summary>
	/// Description of Editor.
	/// </summary>
	public partial class Editor : Form
	{
		public Editor()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		private bool _listFocus = false;
		
		/* Загрузка списка операторов */
		private void loadOperators()
		{
			String pathFile = Config.MyProgramDirectory + "\\resource\\operators.txt";
			if(File.Exists(pathFile)){
				StreamReader sr = File.OpenText(pathFile);
				String str = "";
				while ((str = sr.ReadLine()) != null) 
           		{
					listBox1.Items.Add(str);
                }
			}
		}
		
		/* Загрузка редактора */
		void EditorLoad(object sender, EventArgs e)
		{
			loadOperators(); // загрузка списка операторов
		}
		
		/* определение номера активной строки */
		void RichTextBox1TextChanged(object sender, EventArgs e)
		{
			int i = richTextBox1.GetLineFromCharIndex(richTextBox1.SelectionStart) + 1;
			toolStripStatusLabel2.Text = "Строка: " + i.ToString();
		}
		
		void RichTextBox1MouseMove(object sender, MouseEventArgs e)
		{
			int i = richTextBox1.GetLineFromCharIndex(richTextBox1.SelectionStart) + 1;
			toolStripStatusLabel2.Text = "Строка: " + i.ToString();
		}
		
		void RichTextBox1KeyPress(object sender, KeyPressEventArgs e)
		{
			int i = richTextBox1.GetLineFromCharIndex(richTextBox1.SelectionStart) + 1;
			toolStripStatusLabel2.Text = "Строка: " + i.ToString();
		}
		
		void RichTextBox1KeyUp(object sender, KeyEventArgs e)
		{
			int i = richTextBox1.GetLineFromCharIndex(richTextBox1.SelectionStart) + 1;
			toolStripStatusLabel2.Text = "Строка: " + i.ToString();	
		}
		
		void RichTextBox1KeyDown(object sender, KeyEventArgs e)
		{
			if(e.Control == true && e.KeyValue == 32){
				int _windowH = this.Height;
				int _windowW = this.Width;
				int _smallWinH = richTextBox1.GetPositionFromCharIndex(richTextBox1.SelectionStart).Y + 150 + panel1.Height;
				int _smallWinW = richTextBox1.GetPositionFromCharIndex(richTextBox1.SelectionStart).X + panel1.Width;
				if(_windowH < _smallWinH){
					if(_windowW < _smallWinW){ // Право - низ
						textBox1.Left = 0; textBox1.Top = 101;
						listBox1.Left = 0; listBox1.Top = 0;
						panel1.Left = richTextBox1.GetPositionFromCharIndex(richTextBox1.SelectionStart).X - 200;
						panel1.Top = richTextBox1.GetPositionFromCharIndex(richTextBox1.SelectionStart).Y - 50;
						textBox1.RightToLeft = RightToLeft.Yes;
						panel1.Visible = true;
						textBox1.Focus();
					}else{ // Лево - Низ
						textBox1.Left = 0; textBox1.Top = 101;
						listBox1.Left = 0; listBox1.Top = 0;
						panel1.Left = richTextBox1.GetPositionFromCharIndex(richTextBox1.SelectionStart).X;
						panel1.Top = richTextBox1.GetPositionFromCharIndex(richTextBox1.SelectionStart).Y - 50;
						textBox1.RightToLeft = RightToLeft.No;
						panel1.Visible = true;
						textBox1.Focus();
					}
				}else{
					if(_windowW < _smallWinW){ // Право - Вверх
						textBox1.Left = 0; textBox1.Top = 0;
						listBox1.Left = 0; listBox1.Top = 23;
						panel1.Left = richTextBox1.GetPositionFromCharIndex(richTextBox1.SelectionStart).X - 200;
						panel1.Top = richTextBox1.GetPositionFromCharIndex(richTextBox1.SelectionStart).Y + 50;
						textBox1.RightToLeft = RightToLeft.Yes;
						panel1.Visible = true;
						textBox1.Focus();
					}else{ // Лево - Вверх
						textBox1.Left = 0; textBox1.Top = 0;
						listBox1.Left = 0; listBox1.Top = 23;
						panel1.Left = richTextBox1.GetPositionFromCharIndex(richTextBox1.SelectionStart).X;
						panel1.Top = richTextBox1.GetPositionFromCharIndex(richTextBox1.SelectionStart).Y + 50;
						textBox1.RightToLeft = RightToLeft.No;
						panel1.Visible = true;
						textBox1.Focus();
					}
				}
			}
			/* Закрываем буз выбора */
			if(e.KeyData == Keys.Escape){
				panel1.Visible = false;
				listBox1.SelectedIndex = 0;
				textBox1.Clear();
				toolStripStatusLabel3.Text = "...";
				richTextBox1.Focus();
				_listFocus = true;
			}
		}
		
		/* Производим поиск и выбор нужного оператора */		
		void TextBox1KeyDown(object sender, KeyEventArgs e)
		{
			try{
				/* Закрытие окна операторов */
				if(e.KeyData == Keys.Escape){
					panel1.Visible = false;
					textBox1.Clear();
					toolStripStatusLabel3.Text = "...";
					listBox1.SelectedIndex = 0;
					richTextBox1.SelectionStart = richTextBox1.SelectionStart - 1;
					richTextBox1.SelectionLength = 1;
					richTextBox1.Focus();
				}
				/* передача фокуса листу операторов */
				if(e.KeyData == Keys.Down || e.KeyData == Keys.Up || e.KeyData == Keys.PageUp || e.KeyData == Keys.PageDown){
					_listFocus = true;
					listBox1.Focus();
				}
				/* выбор оператора */
				if(e.KeyData == Keys.Enter){
					if(listBox1.SelectedIndex < 0){
						richTextBox1.SelectionStart = richTextBox1.SelectionStart - 1;
						richTextBox1.SelectionLength = 1;
						/* Вставляем выбранный оператор */
						Clipboard.SetDataObject(textBox1.Text);
						richTextBox1.Paste();
						/* Закрываем */
						panel1.Visible = false;
						listBox1.SelectedIndex = 0;
						textBox1.Clear();
						toolStripStatusLabel3.Text = "...";
						richTextBox1.Focus();
					}else{
						richTextBox1.SelectionStart = richTextBox1.SelectionStart - 1;
						richTextBox1.SelectionLength = 1;
						/* Вставляем выбранный оператор */
						Clipboard.SetDataObject(listBox1.Items[listBox1.SelectedIndex].ToString());
						richTextBox1.Paste();
						/* Закрываем */
						panel1.Visible = false;
						listBox1.SelectedIndex = 0;
						textBox1.Clear();
						toolStripStatusLabel3.Text = "...";
						richTextBox1.Focus();
					}
				}
			}catch{
				richTextBox1.SelectionStart = richTextBox1.SelectionStart - 1;
				richTextBox1.SelectionLength = 1;
				panel1.Visible = false;
				listBox1.SelectedIndex = 0;
				textBox1.Clear();
				toolStripStatusLabel3.Text = "...";
				richTextBox1.Focus();		
			}
		}
		
		void TextBox1KeyUp(object sender, KeyEventArgs e)
		{
			if(e.KeyCode.ToString() != "16"  && e.KeyCode.ToString() != "17"){
				String _find = "";
				int _countChar = 0;
				String _inputOperator = "";
				
				toolStripStatusLabel3.Text = textBox1.Text;
				
				_countChar = textBox1.TextLength;
				for(int i = 0; i < listBox1.Items.Count; i++){
					_find = listBox1.Items[i].ToString();
					if(_find.Length >= _countChar){
						for(int j = 0; j < _countChar; j++)
							_inputOperator = _inputOperator + _find[j];
					}
					
					this.Update();
					panel1.Update();
					listBox1.Update();
					if(textBox1.Text == _inputOperator){
						listBox1.SelectedIndex = i;
						this.Update();
						panel1.Update();
						listBox1.Update();
						break;
					}else{
						_inputOperator = "";
					}
					
				}
			}
		}
		
		void ListBox1SelectedIndexChanged(object sender, EventArgs e)
		{
			if(_listFocus){
				textBox1.Text = listBox1.Items[listBox1.SelectedIndex].ToString();
				toolStripStatusLabel3.Text = listBox1.Items[listBox1.SelectedIndex].ToString();
			}
		}
		
		void ListBox1KeyDown(object sender, KeyEventArgs e)
		{
			if(e.KeyData == Keys.Escape){
				_listFocus = false;
				textBox1.Focus();
			}
			if(e.KeyData == Keys.Enter){
				richTextBox1.SelectionStart = richTextBox1.SelectionStart - 1;
				richTextBox1.SelectionLength = 1;
				/* Вставляем выбранный оператор */
				Clipboard.SetDataObject(listBox1.Items[listBox1.SelectedIndex].ToString());
				richTextBox1.Paste();
				/* Закрываем */
				panel1.Visible = false;
				listBox1.SelectedIndex = 0;
				textBox1.Clear();
				toolStripStatusLabel3.Text = "...";
				_listFocus = false;
				richTextBox1.Focus();
			}
		}
		/*---------------------------------------------*/
		
		/* Открыть файл */
		void ОткрытьФайлToolStripMenuItemClick(object sender, EventArgs e)
		{
			
		}
		
		/* Открыть файл ASCII */
		void ASCIIToolStripMenuItemClick(object sender, EventArgs e)
		{
			
		}
		
		/* Открыть файл UTF-8 */
		void UTF8ToolStripMenuItemClick(object sender, EventArgs e)
		{
			
		}
		
		/* Открыть файл UTF-8 without BOM*/
		void UTF8WithoutBOMToolStripMenuItemClick(object sender, EventArgs e)
		{
			
		}
		
		/* Сохранить файл */
		void СохранитьФайлToolStripMenuItemClick(object sender, EventArgs e)
		{
			if(toolStripStatusLabel1.Text == "Кодировка: ASCII") fileSaveASCII(false);
			if(toolStripStatusLabel1.Text == "Кодировка: UTF-8") fileSaveUTF8(false);
			if(toolStripStatusLabel1.Text == "Кодировка: UTF-8 without BOM") fileSaveUTF8wBOM(false);
		}
		
		/* Сохранить файл ASCII */
		void fileSaveASCII(bool _saveAs)
		{
			if(_saveAs){
				if(saveFileDialog1.ShowDialog() == DialogResult.OK){
					richTextBox1.SaveFile(saveFileDialog1.FileName, RichTextBoxStreamType.PlainText);
					MessageBox.Show("Файл успешно сохранён!");
				}
			}else{
				richTextBox1.SaveFile(this.Text, RichTextBoxStreamType.PlainText);
				MessageBox.Show("Файл успешно сохранён!");
			}
			
		}
		
		void ASCIIToolStripMenuItem1Click(object sender, EventArgs e)
		{
			fileSaveASCII(true);
		}
		
		/* Сохранить файл UTF-8 */
		void fileSaveUTF8(bool _saveAs)
		{
			
		}
		
		void UTF8ToolStripMenuItem1Click(object sender, EventArgs e)
		{
			
		}
		
		/* Сохранить файл UTF-8 without BOM*/
		void fileSaveUTF8wBOM(bool _saveAs)
		{
			
		}
		
		void UTF8WithoutBOMToolStripMenuItem1Click(object sender, EventArgs e)
		{
			
		}
	}
}
