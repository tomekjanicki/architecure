﻿namespace Architecture.WinService
{
    partial class WorkerServiceProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.WorkerServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.WorkerServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // WorkerServiceProcessInstaller
            // 
            this.WorkerServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.WorkerServiceProcessInstaller.Password = null;
            this.WorkerServiceProcessInstaller.Username = null;
            // 
            // WorkerServiceInstaller
            // 
            this.WorkerServiceInstaller.DisplayName = "Architecutre Worker Service";
            this.WorkerServiceInstaller.ServiceName = "Architecutre. WorkerService";
            this.WorkerServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // WorkerServiceProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.WorkerServiceProcessInstaller,
            this.WorkerServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller WorkerServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller WorkerServiceInstaller;
    }
}