using EnvDTE;
using Microsoft.VisualStudio.Shell;
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.InteropServices;

namespace Toybox.UnGroupSourceFiles.Commands
{
	/// <summary>
	/// グループ化コマンド
	/// </summary>
	[Guid(Guids.guidUnGroupSourceFilesCmdSetString), DisplayName("Group Sources")]
	internal class GroupCommand : AbstractCommand
	{

		#region [Static]

		/// <summary>
		/// コマンド実行
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private static void OnExecute(object sender, EventArgs e)
		{
			// root選択
			ProjectItem root = Dte.SelectedItems.Item(1).ProjectItem;

			// 子要素追加
			foreach (var item in Dte.SelectedItems.Cast<SelectedItem>())
			{
				if (item.ProjectItem.Equals(root)) continue;

				item.ProjectItem.Remove();
				root.ProjectItems.AddFromFile(item.ProjectItem.FileNames[0]);
			}

			// 展開して保存
			root.ExpandView();
			root.ContainingProject.Save();
		}

		#endregion [Static]


		#region Constructor

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="serviceProvider"></param>
		public GroupCommand(IServiceProvider serviceProvider) :
			base(serviceProvider, OnExecute, new CommandID(typeof(GroupCommand).GUID, (int)PkgCmdIDs.cmdidGroup))
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
			if (command.CommandID.ID != PkgCmdIDs.cmdidGroup) return false;

			return Dte.SelectedItems.MultiSelect;
		}

		#endregion Override Methods

	}
}