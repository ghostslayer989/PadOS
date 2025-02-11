﻿using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;
using PadOS.Navigation;
using PadOS.Views;

namespace PadOS {
	public partial class App{
		public static Dispatcher GlobalDispatcher { get; private set; }

        public App(){
			GlobalDispatcher = Dispatcher;
		}

		private SystemTray _systemTray;

        protected override void OnStartup(StartupEventArgs e){
#if DEBUG
            var resDict = Resources.MergedDictionaries[0] as ResourceDictionary;
            resDict["ShowActivated"] = true;
            resDict["TopMost"] = false;
            resDict["ShowInTaskbar"] = true;
#endif

            using (var ctx = new SaveData.SaveData()){
                if (ctx.DatabaseExists()) {
#if DEBUG
                    //ctx.DeleteIfExists();
                    //ctx.CreateDb();
                    //ctx.InsertDefault();
#endif
                }
                else
                {
                    ctx.CreateDb();
                    ctx.InsertDefault();
                }
				
			}

			base.OnStartup(e);
			_systemTray = new SystemTray();;
			Exit += OnExit;

			Navigator.Initialize();
			if (Debugger.IsAttached == false)
				return;

			Navigator.OpenMainPanel();
		}

		private void OnExit(object sender, ExitEventArgs exitEventArgs){
			Navigator.Shutdown();
			_systemTray.Dispose();
			Environment.Exit(0);
		}
	}
}
