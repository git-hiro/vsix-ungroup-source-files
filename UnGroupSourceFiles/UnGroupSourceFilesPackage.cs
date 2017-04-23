using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Toybox.UnGroupSourceFiles
{
	[PackageRegistration(UseManagedResourcesOnly = true)]
	[InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
	[ProvideMenuResource("Menus.ctmenu", 1)]
	[ProvideAutoLoad(VSConstants.UICONTEXT.NoSolution_string)]
	[Guid(Guids.guidUnGroupSourceFilesPkgString)]
	public sealed class UnGroupSourceFilesPackage : Package
	{

		#region Constructor

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public UnGroupSourceFilesPackage()
		{
			Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering constructor for: {0}", this.ToString()));
		}

		#endregion Constructor


		#region Private Members

		private CommandManager commandManager;

		#endregion Private Members


		#region Override Methods

		/// <summary>
		/// 初期化
		/// </summary>
		protected override void Initialize()
		{
			Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering Initialize() of: {0}", this.ToString()));
			base.Initialize();

			this.commandManager = new CommandManager(this);
			this.commandManager.Initialize();
		}

		#endregion Override Methods

	}
}
