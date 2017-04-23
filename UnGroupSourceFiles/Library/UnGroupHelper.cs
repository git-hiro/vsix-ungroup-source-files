using EnvDTE;
using System;
using System.Collections.Generic;
using System.IO;

namespace Toybox.UnGroupSourceFiles.Library
{
	/// <summary>
	/// グループ解除ヘルパー
	/// </summary>
	internal class UnGroupHelper
	{

		#region Constructor

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="item"></param>
		public UnGroupHelper(ProjectItem item)
		{
			if (item == null)
				//throw new ArgumentNullException("item");
				return;

			this.Item = item;
			this.Parent = item.Collection.Parent as ProjectItem;
		}

		#endregion Constructor


		#region Public Members

		/// <summary>
		/// アイテム
		/// </summary>
		public ProjectItem Item { get; private set; }

		/// <summary>
		/// 親アイテム
		/// </summary>
		public ProjectItem Parent { get; private set; }

		#endregion Public Members


		#region Public Methods

		/// <summary>
		/// グループ化解除
		/// </summary>
		public void UnGroup()
		{
			if (this.Item == null) return;

			FileInfo fInfo = new FileInfo(this.Item);
			fInfo.CopyFileToTemp();
			this.Item.Delete();
			fInfo.CopyFileFromTemp();
			fInfo.SetParent(this.Parent.Collection);
		}

		/// <summary>
		/// グループ化解除
		/// </summary>
		public void UnGroupChildren()
		{
			if (this.Item == null) return;

			foreach (ProjectItem item in this.Item.ProjectItems)
			{
				FileInfo fInfo = new FileInfo(item);
				fInfo.CopyFileToTemp();
				item.Delete();
				fInfo.CopyFileFromTemp();
				fInfo.SetParent(this.Item.Collection);
			}
		}

		#endregion Public Methods


		#region [SubClass]

		/// <summary>
		/// ファイル情報
		/// </summary>
		public class FileInfo
		{

			#region [Static]

			private const int RETRY_COUNT = 3;

			#endregion [Static]


			#region Constructor

			/// <summary>
			/// コンストラクタ
			/// </summary>
			public FileInfo()
			{
				this.IsCopied = false;
			}

			/// <summary>
			/// コンストラクタ
			/// </summary>
			/// <param name="item"></param>
			public FileInfo(ProjectItem item)
				: this()
			{
				this.SetProjectItem(item);
			}

			#endregion Constructor


			#region Public Members

			/// <summary>
			/// ファイル名
			/// </summary>
			public string FileName { get; private set; }

			/// <summary>
			/// 一時ファイル名
			/// </summary>
			public string TempName
			{
				get { return this._tempName ?? (this._tempName = Path.GetTempFileName()); }
				private set { this._tempName = value; }
			}
			private string _tempName;

			/// <summary>
			/// 子要素
			/// </summary>
			public List<FileInfo> Children
			{
				get { return this._children ?? (this._children = new List<FileInfo>()); }
			}
			private List<FileInfo> _children;

			/// <summary>
			/// 複製済みフラグ
			/// </summary>
			public bool IsCopied { get; private set; }

			#endregion Public Members


			#region Public Methods

			/// <summary>
			/// プロジェクトアイテムセット
			/// </summary>
			/// <param name="item"></param>
			public void SetProjectItem(ProjectItem item)
			{
				if (item == null) return;

				if (!item.Saved) item.Save();

				this.FileName = item.FileNames[0];

				foreach (ProjectItem element in item.ProjectItems)
				{
					this.Children.Add(new FileInfo(element));
				}
			}

			/// <summary>
			/// ファイルを一時複製
			/// </summary>
			public void CopyFileToTemp()
			{
				if (this.IsCopied) return;

				for (int n = 0; n < RETRY_COUNT; n++)
				{
					try
					{
						File.Copy(this.FileName, this.TempName, true);
						this.IsCopied = true;
						break;
					}
					catch (Exception) { this.TempName = null; }
				}

				foreach (FileInfo fInfo in this.Children)
				{
					fInfo.CopyFileToTemp();
				}
			}

			/// <summary>
			/// ファイルを復元
			/// </summary>
			public void CopyFileFromTemp()
			{
				if (!this.IsCopied) return;

				try
				{
					File.Copy(this.TempName, this.FileName, true);
					File.Delete(this.TempName);
				}
				catch (Exception) { }
				this.TempName = null;

				foreach (FileInfo fInfo in this.Children)
				{
					fInfo.CopyFileFromTemp();
				}

				this.IsCopied = false;
			}

			/// <summary>
			/// 親プロジェクトを設定
			/// </summary>
			public void SetParent(ProjectItems items)
			{
				if (items == null) return;

				ProjectItem item = items.AddFromFile(this.FileName);
				foreach (var child in this.Children)
				{
					child.SetParent(item.ProjectItems);
				}
			}

			#endregion Public Methods

		}

		#endregion [SubClass]

	}
}
