using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.ComponentModel;
using mikoba.ViewModels;
using Xamarin.Forms;
using System.Windows.Input;
using mikoba.UI.Pages;

namespace mikoba.ViewModels
{
    public class CredentialRequestViewModel : KivaBaseViewModel, INotifyPropertyChanged
    {
        private static CredentialRequestViewModel m_instance;


        public static CredentialRequestViewModel Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = new CredentialRequestViewModel();
                }

                return m_instance;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}
