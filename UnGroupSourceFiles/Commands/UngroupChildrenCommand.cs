using EnvDTE;
using Microsoft.VisualStudio.Shell;
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Toybox.UnGroupSourceFiles.Commands
{
	using Library;

	/// <summary>
	/// グループ化解除コマンド
	/// </summary>
	[Guid(Guids.guidUnGroupSourceFilesCmdSetString), DisplayName("UnGroupChildren Sources")]
	internal class UnGroupChildrenCommand : AbstractCommand
	{

		#region [Static]

		private static void OnExecute(object sender, EventArgs e)
		{
			ProjectItem item = Dte.SelectedItems.Item(1).ProjectItem;
			UnGroupHelper helper = new UnGroupHelper(item);
			helper.UnGroupChildren();

			return;
		}

		#endregion [Static]


		#region Constructor

		public UnGroupChildrenCommand(IServiceProvider serviceProvider) :
			base(serviceProvider, OnExecute, new CommandID(typeof(UnGroupChildrenCommand).GUID, (int)PkgCmdIDs.cmdidUnGroupChildren))
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
			if (command.CommandID.ID != (int)PkgCmdIDs.cmdidUnGroupChildren) return false;
			if (Dte.SelectedItems.MultiSelect) return false;

			return this.IsParent(Dte.SelectedItems.Item(1).ProjectItem);
		}

		#endregion Override Methods

		#region Private Methods

		/// <summary>
		/// 親チェック
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		private bool IsParent(ProjectItem item)
		{
			if (item == null) return false;
			
			Debug.WriteLine("UnGroupChildren:" + item.Name);

			return 0 < item.ProjectItems.Count;
		}

		#endregion Private Methods

	}
}