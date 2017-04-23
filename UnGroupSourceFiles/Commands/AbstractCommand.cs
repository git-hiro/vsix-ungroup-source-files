using EnvDTE;
using Microsoft.VisualStudio.Shell;
using System;
using System.ComponentModel.Design;

namespace Toybox.UnGroupSourceFiles.Commands
{
	using Extensions;

	/// <summary>
	/// コマンドクラス
	/// </summary>
	internal abstract class AbstractCommand : OleMenuCommand
	{

		#region [Static]

		protected static IServiceProvider ServiceProvider { get; private set; }

		protected static UnGroupSourceFilesPackage Package { get; private set; }

		protected static DTE Dte { get; private set; }

		#endregion [Static]


		#region Constructor

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="provider"></param>
		/// <param name="onExecute"></param>
		/// <param name="id"></param>
		public AbstractCommand(IServiceProvider provider, EventHandler onExecute, CommandID id)
			: base(onExecute, id)
		{
			base.BeforeQueryStatus += OnBeforeQueryStatus;

			ServiceProvider = provider;
			Package = ServiceProvider.GetService<UnGroupSourceFilesPackage>();
			Dte = ServiceProvider.GetService<DTE>();
		}

		#endregion Constructor


		#region Protected Methods

		/// <summary>
		/// 実行前チェック
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected static void OnBeforeQueryStatus(object sender, EventArgs e)
		{
			AbstractCommand command = sender as AbstractCommand;
			bool value = command.CanExecute(command);
			command.Enabled = command.Visible = command.Supported = value;
		}

		#region Override Methods

		/// <summary>
		/// 実行可能チェック
		/// </summary>
		/// <param name="command"></param>
		/// <returns></returns>
		protected virtual bool CanExecute(OleMenuCommand command)
		{
			return command.CommandID.Guid == Guids.guidUnGroupSourceFilesCmdSet;
		}

		#endregion Override Methods

		#endregion Protected Methods

	}
}