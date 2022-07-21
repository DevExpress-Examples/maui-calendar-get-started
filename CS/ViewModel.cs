using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using DevExpress.Maui.Editors;

namespace CalendarExample {
    public class ViewModel : INotifyPropertyChanged {
        DateTime displayDate;
        DateTime? selectedDate;
        DXCalendarViewType activeViewType;
        bool isHolidaysAndObservancesListVisible;
        IEnumerable<SpecialDate> specialDates;

        public ViewModel() {
            DisplayDate = DateTime.Today;
            UpdateHolidaysAndObservancesListVisible();
        }

        public IEnumerable<SpecialDate> SpecialDates {
            get => this.specialDates;
            set {
                if (this.specialDates == value)
                    return;

                this.specialDates = value;
                NotifyPropertyChanged();
            }
        }

        public DateTime DisplayDate {
            get => this.displayDate;
            set {
                if (this.displayDate == value)
                    return;

                this.displayDate = value;
                UpdateCurrentCalendarIfNeeded();
                SpecialDates = USCalendar.GetSpecialDatesForMonth(DisplayDate.Month);
                NotifyPropertyChanged();
            }
        }

        public DateTime? SelectedDate {
            get => this.selectedDate;
            set {
                if (this.selectedDate == value)
                    return;

                this.selectedDate = value;
                NotifyPropertyChanged();
            }
        }

        public DXCalendarViewType ActiveViewType {
            get => this.activeViewType;
            set {
                if (this.activeViewType == value)
                    return;

                this.activeViewType = value;
                UpdateHolidaysAndObservancesListVisible();
                NotifyPropertyChanged();
            }
        }

        public bool IsHolidaysAndObservancesListVisible {
            get => this.isHolidaysAndObservancesListVisible;
            set {
                if (this.isHolidaysAndObservancesListVisible == value)
                    return;

                this.isHolidaysAndObservancesListVisible = value;
                NotifyPropertyChanged();
            }
        }

        USCalendar USCalendar { get; set; }

        public SpecialDate TryFindSpecialDate(DateTime date) {
            return SpecialDates.FirstOrDefault(x => x.Date == date);
        }

        void UpdateHolidaysAndObservancesListVisible() {
            IsHolidaysAndObservancesListVisible = ActiveViewType == DXCalendarViewType.Month;
        }

        void UpdateCurrentCalendarIfNeeded() {
            if (USCalendar == null || USCalendar.Year != DisplayDate.Year)
                USCalendar = new USCalendar(DisplayDate.Year);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void NotifyPropertyChanged([CallerMemberName] string propertyName = "") {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}

