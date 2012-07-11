using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using GalaSoft.MvvmLight.Command;

namespace NDG.ViewModels
{
    public enum HomePageIndexes
    {
        MySurveys,

        SavedResponses,

        SubmittedResponses
    }

    public class HomeViewModel : ViewModel
    {
        #region Fields

        private HomePageIndexes currentPageIndex = HomePageIndexes.MySurveys;

        #endregion Fields

        public HomeViewModel()
        {
            this.InitializeViewModelCommand = new RelayCommand(this.InitializeViewModelExecute);
            this.RefreshCommand = new RelayCommand(this.RefreshExecute, this.RefreshCanExecute);
        }

        #region Commands

        public RelayCommand RefreshCommand { get; private set; }

        #endregion Commands

        public HomePageIndexes CurrentPageIndex
        {
            get
            {
                return this.currentPageIndex;
            }

            set
            {
                this.currentPageIndex = value;
                this.RaisePropertyChanged("CurrentPageIndex");
            }
        }

        #region Methods

        private void InitializeViewModelExecute()
        {
            this.CurrentPageIndex = HomePageIndexes.MySurveys;
        }

        private bool RefreshCanExecute()
        {
            return Locator.MySurveysStatic.RefreshCanExecute();
        }

        private void RefreshExecute()
        {
            Locator.MySurveysStatic.RefreshExecute();
        }

        #endregion Methods
    }
}
