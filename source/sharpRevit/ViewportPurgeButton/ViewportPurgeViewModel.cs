using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Windows;
using Autodesk.Revit.DB;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SharpRevit.Classes;

namespace SharpRevit.ViewportPurgeButton
{
    internal class ViewportPurgeViewModel : ObservableRecipient
    {
        public string Version => $"sharpRevit v.{Global.Version}";
        public ViewportPurgeModel Model { get; set; }

        public RelayCommand<Window> Close { get; set; }
        public RelayCommand<Window> Run { get; set; }
        public RelayCommand<Window> Help { get; set; }
        private int _progressValue;
        public int ProgressValue
        {
            get => _progressValue;
            set
            {
                _progressValue = value;
                OnPropertyChanged();
            }
        }
        private int _progressMax;
        public int ProgressMax
        {
            get => _progressMax;
            set
            {
                _progressMax = value;
                OnPropertyChanged();
            }
        }

        private bool _runEnabled;
        public bool RunEnabled
        {
            get => _runEnabled;
            set { _runEnabled = value; OnPropertyChanged(nameof(RunEnabled)); }
        }


        private ObservableCollection<ViewportTypeWrapper> _viewportTypes;
        public ObservableCollection<ViewportTypeWrapper> ViewportTypes
        {
            get => _viewportTypes;
            set
            {
                _viewportTypes = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<ElementType> _targetViewportTypes;
        public ObservableCollection<ElementType> TargetViewportTypes
        {
            get => _targetViewportTypes;
            set
            {
                _targetViewportTypes = value;
                OnPropertyChanged();
            }
        }
        private int _selectedTargetIndex;
        public int SelectedTargetIndex
        {
            get => _selectedTargetIndex;
            set
            {
                _selectedTargetIndex = value;
                OnPropertyChanged();
            }
        }

        private string _log;
        public string Log
        {
            get => _log;
            set
            {
                _log = value;
                OnPropertyChanged();
            }
        }
        public ViewportPurgeViewModel(ViewportPurgeModel model)
        {
            Model = model;
            Log = string.Empty;
            ProgressValue = 0;
            ViewportTypes = Model.CollectViewportTypes();
            GetTargets();
            SelectedTargetIndex = 0;

            RunEnabled = true;
            //set button commands
            Run = new RelayCommand<Window>(OnRun);
            Close = new RelayCommand<Window>(OnClose);

            ProgressMax = ViewportTypes.Count;
        }

        internal void GetTargets()
        {
            TargetViewportTypes = Model.CollectTargetViewportTypes(ViewportTypes);
            SelectedTargetIndex = 0;
        }
        private void OnRun(Window win)
        {
            ProgressMax = ViewportTypes.Where(v => v.Delete).SelectMany(v => v.DependentViewports).Count();

            Log = Model.SwapViewportTypes(ViewportTypes.Where(v => v.Delete).ToList(),
                TargetViewportTypes[SelectedTargetIndex], this);

            ProgressValue = 0;
        }
        private void OnClose(Window win)
        {
            try
            {
                win.Close();
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}
