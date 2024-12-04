using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Autodesk.Revit.DB;

namespace SharpRevit.Classes
{
    internal class ViewportTypeWrapper : INotifyPropertyChanged
    {
        public Document Doc { get; set; }
        public ElementType ViewportType { get; set; }
        public string Name { get; set; }
        public ObservableCollection<Viewport> DependentViewports { get; set; }
        private bool _delete;
        public bool Delete
        {
            get => _delete;
            set
            {
                _delete = value;
                OnPropertyChanged();
            }
        }
       
        public ViewportTypeWrapper(ElementType viewportType, ObservableCollection<Viewport> viewports)
        {
            Delete = false;
            ViewportType = viewportType;
            Doc = viewportType.Document;
            Name = viewportType.Name;
            DependentViewports = viewports;

        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
