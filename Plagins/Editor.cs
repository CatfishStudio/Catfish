/*
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
						panel1.Left = richTextBox1.GetPositionFromCharIndex(richTextBox1.SelectionStart).X - 200;
						panel1.Top = richTextBox1.GetPositionFromCharIndex(richTextBox1.SelectionStart).Y;
						textBox1.RightToLeft = RightToLeft.Yes;
						panel1.Visible = true;
						textBox1.Focus();
					}else{ // Лево - Низ
						panel1.Left = richTextBox1.GetPositionFromCharIndex(richTextBox1.SelectionStart).X;
						panel1.Top = richTextBox1.GetPositionFromCharIndex(richTextBox1.SelectionStart).Y;
						textBox1.RightToLeft = RightToLeft.No;
						panel1.Visible = true;
						textBox1.Focus();
					}
				}else{
					if(_windowW < _smallWinW){ // Право - Вверх
						panel1.Left = richTextBox1.GetPositionFromCharIndex(richTextBox1.SelectionStart).X - 200;
						panel1.Top = richTextBox1.GetPositionFromCharIndex(richTextBox1.SelectionStart).Y + 80;
						textBox1.RightToLeft = RightToLeft.Yes;
						panel1.Visible = true;
						textBox1.Focus();
					}else{ // Лево - Вверх
						panel1.Left = richTextBox1.GetPositionFromCharIndex(richTextBox1.SelectionStart).X;
						panel1.Top = richTextBox1.GetPositionFromCharIndex(richTextBox1.SelectionStart).Y + 80;
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
				richTextBox1.Focus();
			}
		}
		
		/* Производим поиск и выбор нужного оператора */		
		void TextBox1KeyDown(object sender, KeyEventArgs e)
		{
			
		}
		
		void TextBox1KeyUp(object sender, KeyEventArgs e)
		{
			
		}
				
		void ListBox1SelectedIndexChanged(object sender, EventArgs e)
		{
			
		}
		
		void ListBox1KeyDown(object sender, KeyEventArgs e)
		{
			
		}
		/*---------------------------------------------*/
		
		
	}
}
