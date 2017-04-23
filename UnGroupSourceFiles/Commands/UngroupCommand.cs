using EnvDTE;
using Microsoft.VisualStudio.Shell;
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;

namespace Toybox.UnGroupSourceFiles.Commands
{
	using Library;

	/// <summary>
	/// グループ化解除コマンド
	/// </summary>
	[Guid(Guids.guidUnGroupSourceFilesCmdSetString), DisplayName("UnGroup Sources")]
	internal class UnGroupCommand : AbstractCommand
	{

		#region [Static]

		/// <summary>
		/// コマンド実行
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private static void OnExecute(object sender, EventArgs e)
		{
			ProjectItem item = Dte.SelectedItems.Item(1).ProjectItem;
			UnGroupHelper helper = new UnGroupHelper(item);
			helper.UnGroup();
		}

		#endregion [Static]


		#region Constructor

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="serviceProvider"></param>
		public UnGroupCommand(IServiceProvider serviceProvider) :
			base(serviceProvider, OnExecute, new CommandID(typeof(UnGroupCommand).GUID, (int)PkgCmdIDs.cmdidUnGroup))
		{
		}

		#endregion Constructor


		#region Override Methods

		/// <summary>
		/// 実行可能チェック
		/// </summary>
		/// <param name="command"></param>
		/// <returns></returns>
		protected override bool CanExecute(OleMenuCommand command)
		{
			if (!base.CanExecute(command)) return false;
			if (command.CommandID.ID != (int)PkgCmdIDs.cmdidUnGroup) return false;
			if (Dte.SelectedItems.MultiSelect) return false;

			return this.HasParent(Dte.SelectedItems.Item(1).ProjectItem);
		}

		#endregion Override Methods

		#region Private Methods

		/// <summary>
		/// 親持ちチェック
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		private bool HasParent(ProjectItem item)
		{
			if (item == null) return false;

			ProjectItem parent = item.Collection.Parent as ProjectItem;
			if (parent != null)
			{
				switch (parent.Kind)
				{
					// 親が物理ファイルのときのみ
					case Constants.vsProjectItemKindPhysicalFile:
						return true;
					//case Constants.vsProjectItemKindVirtualFolder:
					//case Constants.vsProjectItemKindPhysicalFolder:
					default:
						return false;
				}
			}
			return false;
		}

		#endregion Private Methods

	}
}