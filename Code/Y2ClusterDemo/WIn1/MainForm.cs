/*
 * Created by SharpDevelop.
 * User: Windown Office-Tuan
 * Date: 4/10/2011
 * Time: 10:40 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace WIn1
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		Board _board;
		public MainForm()
		{
			_board=new Board();
			_board.Left=100;
							
			_board.Width=500;
			_board.Height=500;
			_board.Anchor= AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
			this.Controls.Add(_board);
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
            trackBar1.Value =(1000- timer1.Interval)/100;
		}
		
		
		void MainFormMouseClick(object sender, MouseEventArgs e)
		{
			
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			_board.CreateNewGroup();
		}
		
		void Button2Click(object sender, EventArgs e)
		{
			_board.Clear();
		}

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            _board.IsRectangleBound = false;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            _board.IsRectangleBound = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (_board.GroupCount < 2)
                timer1.Stop();
            else
                _board.CreateNewGroup();
        }

        private void button3_Click(object sender, EventArgs e)
        {
                           
            timer1.Start();
        }
        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            timer1.Interval = 1000 - trackBar1.Value*100;
        }
	}
}
