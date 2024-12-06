 using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using SharpRevit.Classes;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace SharpRevit.ViewportPurgeButton
{
    public sealed partial class ViewportPurgeView
    {
        public ViewportPurgeView()
        {
            this.InitializeComponent();

        }

        private void ViewportGrid_OnSelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
           if(!IsLoaded) return;

           ViewportTypeWrapper viewportTypeWrapper = e.AddedCells.First().Item as ViewportTypeWrapper;

           viewportTypeWrapper.Delete = !viewportTypeWrapper.Delete;

            var vm = this.DataContext as ViewportPurgeViewModel;

           vm.GetTargets();
        }
    }
}
