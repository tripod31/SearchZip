using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Text;			// StringBuilder
using System.IO;

namespace SearchJar
{
	/// <summary>
	/// Form1 �̊T�v�̐����ł��B
	/// </summary>
	public partial class Form1 : System.Windows.Forms.Form
    {
		
		// �����o�ϐ�
		private Jar m_oJar;

        public Form1()
        {
            //
            // Windows �t�H�[�� �f�U�C�i �T�|�[�g�ɕK�v�ł��B
            //
            InitializeComponent();

            //
            // TODO: InitializeComponent �Ăяo���̌�ɁA�R���X�g���N�^ �R�[�h��ǉ����Ă��������B
            //

            m_oJar = new Jar();

        }

		/// <summary>
		/// �A�v���P�[�V�����̃��C�� �G���g�� �|�C���g�ł��B
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

		// �C�x���g�n���h�� 
		//////////////////////////////////////////////////////////////////////////////////////////

		// �����f�B���N�g���{�^��
		private void button_dir_Click(object sender, System.EventArgs e)
		{
			// �t�H���_�I���_�C�A���O�\��
            folderBrowserDialog1.ShowDialog(this);

            string sDir = folderBrowserDialog1.SelectedPath;
            if (sDir.Length !=0 )
                text_dir.Text = sDir.ToString();

		}

		// ���s�{�^��
		private void button_go_Click(object sender, System.EventArgs e)
		{
			if (text_dir.Text.Length == 0)
				return;

            Cursor.Current = Cursors.WaitCursor;
			m_oJar.arr_class.Clear();	// ���X�g�N���A

			string sDir =  text_dir.Text;
			string sClassName = text_class_name.Text;

			// �f�B���N�g����*.jar�t�@�C��������
			ArrayList arr_jar_files = new ArrayList();
			Util.GetFileNames(sDir,"*.jar",ref arr_jar_files);

			// jar�t�@�C�����ɃN���X����		
			for ( int i = 0;i<arr_jar_files.Count;i++) 
			{
				string sJarFileName = arr_jar_files[i].ToString();
				
				int iRet = m_oJar.SearchClass(sJarFileName,sClassName);
				if (iRet == -1)
				{
					MessageBox.Show(m_oJar.ErrMsg);
					break;
				}
			}

			lv_result.Items.Clear();		// �\�����X�g�N���A

			// ���X�g�\��
			for (int i=0;i<m_oJar.arr_class.Count;i++)
			{
				Jar.ClassRec oRec = (Jar.ClassRec)m_oJar.arr_class[i];
                ListViewItem item = lv_result.Items.Add(oRec.sJarFileName);
				item.SubItems.Add(oRec.sClassName);
			}
            lv_result.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            Cursor.Current = Cursors.Default;
            DispCount();
        }

        private void DispCount()
        {
            toolStripStatusLabel1.Text = String.Format(
                "{0}��",
                lv_result.Items.Count);

		}

		// �\���i���݃{�^��
		private void button_filter_Click(object sender, System.EventArgs e)
		{
            Cursor.Current = Cursors.WaitCursor;
            lv_result.Items.Clear();	// ���X�g�N���A

			string sFilter_jar = text_filter_jar.Text;
			string sFilter_class =  text_filter_class.Text;

			// ���X�g�\��
			for (int i=0;i<m_oJar.arr_class.Count;i++)
			{
				Jar.ClassRec oRec = (Jar.ClassRec)m_oJar.arr_class[i];
				if (sFilter_jar.Length != 0) 
				{
					if (oRec.sJarFileName.IndexOf(sFilter_jar) == -1)
						continue;
				}

				if (sFilter_class.Length != 0) 
				{
					if (oRec.sClassName.IndexOf(sFilter_class) == -1)
						continue;
				}

                ListViewItem item = lv_result.Items.Add(oRec.sJarFileName);
				item.SubItems.Add(oRec.sClassName);
			}
            lv_result.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            Cursor.Current = Cursors.Default;
            DispCount();
		}
		
		// �t�@�C���ɏo�̓{�^��
		private void button_output_Click(object sender, System.EventArgs e)
		{
			// �t�@�C���_�C�A���O��\��
			SaveFileDialog oDlg = new SaveFileDialog();
			oDlg.DefaultExt="csv";
			oDlg.Filter = "csv�t�@�C��(*.csv)|*.csv";

			if(  oDlg.ShowDialog() != DialogResult.OK)
				return;

			string sFilter_jar =  text_filter_jar.Text;
			string sFilter_class =  text_filter_class.Text;

			string sFile = oDlg.FileName;
			int iRet = m_oJar.WriteCSV(sFile,sFilter_jar,sFilter_class);
			if ( iRet == -1)
			{
				MessageBox.Show(m_oJar.ErrMsg);
			} 
			else 
			{
				MessageBox.Show(sFile + "�ɏo�͂��܂���");
			}
		}

		// CSV�ǂݍ��݃{�^��
		private void button_read_Click(object sender, System.EventArgs e)
		{
			// �t�@�C���_�C�A���O��\��
			OpenFileDialog oDlg = new OpenFileDialog();
			oDlg.DefaultExt="csv";
			oDlg.Filter = "csv�t�@�C��(*.csv)|*.csv";

			if(  oDlg.ShowDialog() != DialogResult.OK)
				return;

			string sFile = oDlg.FileName;

			int iRet = m_oJar.ReadCSV(sFile);
			if (iRet == -1)
			{
				MessageBox.Show(m_oJar.ErrMsg);
				return;
			}

            lv_result.Items.Clear();	// �\�����X�g�N���A

			// ���X�g�\��
			for (int i=0;i<m_oJar.arr_class.Count;i++)
			{
				Jar.ClassRec oRec = (Jar.ClassRec)m_oJar.arr_class[i];
                ListViewItem item = lv_result.Items.Add(oRec.sJarFileName);
				item.SubItems.Add(oRec.sClassName);
			}
            lv_result.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            DispCount();
		
		}

		// �ݒ��ۑ�
		private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
            Properties.Settings.Default.Save();	    // �A�v���ݒ�t�@�C���ۑ�

		}



	}
}