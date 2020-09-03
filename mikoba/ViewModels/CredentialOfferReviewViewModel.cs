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
    public class CredentialOfferReviewViewModel : KivaBaseViewModel, INotifyPropertyChanged
    {
        private static CredentialOfferReviewViewModel m_instance;


        public static CredentialOfferReviewViewModel Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = new CredentialOfferReviewViewModel();
                }

                return m_instance;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}
