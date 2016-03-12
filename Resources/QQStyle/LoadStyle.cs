using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

// 2014-01-20: GreenRose
using ChatEngine;

namespace ChatClient
{
    class LoadStyle
    {
        public LoadStyle()
        {            
        }

        public void LoadCurrentStyle(string currentStyle, string newStyle, string strStyleType)
        {            
            var curStyle = new ResourceDictionary() { Source = new Uri(currentStyle, UriKind.RelativeOrAbsolute) };
            var newStyleSource = new ResourceDictionary() { Source = new Uri(newStyle, UriKind.RelativeOrAbsolute) };

            Application.Current.Resources.Remove((ResourceDictionary)curStyle);
            Application.Current.Resources.MergedDictionaries.Add(newStyleSource);

            SetStyleInitValue(strStyleType, newStyle);
        }

        public void SetStyleInitValue(string strTagName, string strTagValue)
        {
            string strUserInfoPath = System.Windows.Forms.Application.StartupPath + "\\UserInfo.ini";

            if (System.IO.File.Exists(strUserInfoPath))
            {
                IniFileEdit iniFileEdit = new IniFileEdit(strUserInfoPath);
                iniFileEdit.SetIniValue("StyleInfo", strTagName, strTagValue);
            }
        }
    }
}
