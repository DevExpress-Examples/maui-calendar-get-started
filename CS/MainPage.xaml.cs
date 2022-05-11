using System;
using DevExpress.Maui.Core.Themes;
using DevExpress.Maui.Editors;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace CalendarExample {
    public partial class MainPage : ContentPage {
        public static readonly BindablePropertyKey OrientationPropertyKey = BindableProperty.CreateReadOnly("Orientation", typeof(StackOrientation), typeof(MainPage), StackOrientation.Vertical);
        public static readonly BindableProperty OrientationProperty = OrientationPropertyKey.BindableProperty;
        public StackOrientation Orientation => (StackOrientation)GetValue(OrientationProperty);

        public MainPage() {
            InitializeComponent();
            ViewModel = new ViewModel();
            BindingContext = ViewModel;
        }

        ViewModel ViewModel { get; }

        void CustomDayCellStyle(object sender, CustomSelectableCellStyleEventArgs e) {
            if (e.Date == DateTime.Today)
                return;

            if (ViewModel.SelectedDate != null && e.Date == ViewModel.SelectedDate.Value)
                return;

            SpecialDate specialDate = ViewModel.TryFindSpecialDate(e.Date);
            if (specialDate == null)
                return;

            e.ViewInfo.FontAttributes = FontAttributes.Bold;
            Color textColor;
            if (specialDate.IsHoliday) {
                textColor = (Color)Resources["CalendarViewHolidayTextColor"];
                e.ViewInfo.EllipseBackgroundColor = Color.FromRgba(textColor.Red, textColor.Green, textColor.Blue, 0.25);
                e.ViewInfo.TextColor = textColor;

                return;
            }
            textColor = (Color)Resources["CalendarViewTextColor"];
            e.ViewInfo.EllipseBackgroundColor = Color.FromRgba(textColor.Red, textColor.Green, textColor.Blue, 0.15);
            e.ViewInfo.TextColor = textColor;
        }

        protected override void OnSizeAllocated(double width, double height) {
            base.OnSizeAllocated(width, height);
            SetValue(OrientationPropertyKey, width > height ? StackOrientation.Horizontal : StackOrientation.Vertical);
        }
    }
}

