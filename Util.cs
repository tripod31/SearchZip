using System;
using System.Collections;	// ArrayList
using System.Windows.Forms;	// Application
using System.IO;
using System.Text;			// Encording
using System.Diagnostics;	// Process
using System.Text.RegularExpressions;

namespace SearchZip
{
	/// <summary>
	/// Util の概要の説明です。
	/// </summary>
	public class Util
	{
 
		public Util()
		{
			// 
			// TODO: コンストラクタ ロジックをここに追加してください。
			//
		}

		// 文字列を空白文字で分割
		// 結果から空文字列("")を除く
		public static ArrayList MySplit(string s)
		{
			ArrayList arr = new ArrayList();
			String[] tokens;
			
			tokens = s.Split(null);

			for (int i = 0;i<tokens.Length;i++)
			{
				if (tokens[i].Length != 0)
					arr.Add(tokens[i]);
			}
			return arr;
		}

		// ファイルを再帰検索
        // filter:   正規表現のフィルター
		public static void GetFileNames(string sStartDir,Regex filter,ref ArrayList arrRet)
		{
			string[] sFileArr = Directory.GetFiles(sStartDir);
			for (int i=0;i<sFileArr.Length;i++)
			{
                if (filter.IsMatch(sFileArr[i]))
				    arrRet.Add(sFileArr[i]);
			}
			string[] sDirArr = Directory.GetDirectories(sStartDir);
			for (int i=0;i<sDirArr.Length;i++)
			{
				// 再帰呼び出し
				GetFileNames(sDirArr[i],filter,ref arrRet);
			}
		}
	}
}
