﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;



namespace ChatServer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                Loginuser loginForm = new Loginuser();

                if (loginForm.ShowDialog() == DialogResult.Cancel)
                    return;

                Application.Run(new Main());
            }
            catch
            { }
        }
    }
}
