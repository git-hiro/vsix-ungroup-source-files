using Microsoft.VisualStudio.Shell;
using System;
using System.ComponentModel.Design;

namespace Toybox.UnGroupSourceFiles
{
	using Extensions;

	/// <summary>
	/// コマンドマネージャー
	/// </summary>
	internal class CommandManager
	{

		#region Constructor

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="provider"></param>
		public CommandManager(IServiceProvider provider)
		{
			this.serviceProvider = provider;
			this.oleMenuCommandService =
				this.serviceProvider.GetService<IMenuCommandService, OleMenuCommandService>();
		}

		#endregion Constructor


		#region Private Members

		private IServiceProvider serviceProvider;

		private OleMenuCommandService oleMenuCommandService;

		#endregion Private Members


		#region Public Methods

		/// <summary>
		/// 初期化
		/// </summary>
		public void Initialize()
		{
			if (this.oleMenuCommandService != null)
			{
				this.Register(new Commands.GroupCommand(this.serviceProvider));
				this.Register(new Commands.UnGroupCommand(this.serviceProvider));
				this.Register(new Commands.UnGroupChildrenCommand(this.serviceProvider));
			}
		}

		#endregion Public Methods

		#region Private Methods

		/// <summary>
		/// コマンド登録
		/// </summary>
		/// <param name="command">コマンド</param>
		private void Register(OleMenuCommand command)
		{
			this.oleMenuCommandService.AddCommand(command);
		}

		#endregion Private Methods

	}
}
