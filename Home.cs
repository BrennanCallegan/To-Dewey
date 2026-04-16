namespace To_Dewey {
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Text.Json;
    using Terminal.Gui;
    
    public class Home : Window {
        
        public static ObservableCollection<Entry> notes = new ObservableCollection<Entry>();
        private List<Entry> filteredNotes = new List<Entry>(); 

        public static ListView dailyList;
        public static ListView monthlyList;

        private Bar statusBar;
        private ScrollBar scrollbar;
        private FrameView calendarFrame;
        private DatePicker calendar;
        private TabView logsView;
        
        public Home() {
            Title = "Press Esc to quit";
            Width = Dim.Fill();
            Height = Dim.Fill();

            dailyList = new ListView{
                Width = Dim.Fill(),
                Height = Dim.Fill(),
                Source = new ListWrapper<object>(new ObservableCollection<object>())
            };

            monthlyList = new ListView{
                Width = Dim.Fill(),
                Height = Dim.Fill(),
                Source = new ListWrapper<object>(new ObservableCollection<object>())
            };

            dailyList.OpenSelectedItem += (s, e) => OpenSelectedEntry(dailyList);
            monthlyList.OpenSelectedItem += (s, e) => OpenSelectedEntry(monthlyList);
            
            logsView = new TabView(){
                Title = "Logs",
                X = 0,
                Y = 0,
                Width = Dim.Fill (33),
                Height = 14,
                BorderStyle = LineStyle.Rounded,
            };

            logsView.AddTab(new Tab { DisplayText = "Daily", View = dailyList }, true);
            logsView.AddTab(new Tab { DisplayText = "Monthly", View = monthlyList }, false);


            calendar = new DatePicker { Y = Pos.Center (), X = Pos.Center (), BorderStyle = LineStyle.None };
            var internalField = calendar.Subviews.OfType<DateField>().FirstOrDefault();
            if (internalField != null) {
                internalField.DateChanged += (s, e) => {
                    UpdateFilter();
                };
            }

            calendarFrame = new ()
            {
                X = Pos.Right(logsView) + 1,
                Y = Pos.Top(logsView),
                Width = Dim.Fill(),
                Height = Dim.Height(logsView),
                Title = "Calendar"
            };
            calendarFrame.Add(calendar);

            MakeStatusBar();
            this.Add(statusBar, logsView, calendarFrame);

            LoadNotes();
            calendar.Date = DateTime.Today;
            UpdateFilter();
        }

        public void UpdateFilter() 
        {   
            // Ensure we are filtering based on the 'Date' property of the picker
            var selectedDate = calendar.Date;
            string dailyHeader = $"{selectedDate:D}";
            string monthlyHeader = $"{selectedDate:MMMM yyyy}";
            
            var dailyData = notes
                .Where(n => !n.isMonthly && n.date.Date == selectedDate.Date)
                .Cast<object>()
                .ToList();
            dailyData.Insert(0, dailyHeader);
            dailyList.Source = new ListWrapper<object>(new ObservableCollection<object>(dailyData));

            var monthlyData = notes
                .Where(n => n.isMonthly && 
                            n.date.Month == selectedDate.Month &&
                            n.date.Year == selectedDate.Year)
                .Cast<object>()
                .ToList();
            monthlyData.Insert(0, monthlyHeader);
            monthlyList.Source = new ListWrapper<object>(new ObservableCollection<object>(monthlyData));
            SaveNotes();
        }

        private void OpenSelectedEntry(ListView targetList = null){
            var activeList = targetList;

            if(activeList == null){
                if(logsView?.SelectedTab == null) return;
                activeList = logsView.SelectedTab.DisplayText == "Daily" ? dailyList : monthlyList;
            }

            if(activeList?.Source == null) return;
            
            var sourceList = activeList.Source.ToList();
            int index = activeList.SelectedItem;

            if(index >= 0 && index < sourceList.Count){
                var selectedItem = sourceList[index];

                if(selectedItem is Entry selectedNote){
                    var editWindow = new EntryEditor(selectedNote, () => UpdateFilter());
                    Application.Run(editWindow);
                }
            }
        }
        private void DeleteSelectedEntry(){
            var activeList = logsView.SelectedTab.DisplayText == "Daily" ? dailyList : monthlyList;
            if (activeList == null) return;

            int selectedIndex = activeList.SelectedItem;
            var sourceList = activeList.Source.ToList();

            if(selectedIndex >= 0 && selectedIndex < sourceList.Count){
                var selectedItem = sourceList[selectedIndex];

                if(selectedItem is Entry selectedNote){
                    int result = MessageBox.Query("Delete Note", "Do you want to delete this note?", "No", "Yes");
                    if(result == 1){
                        notes.Remove(selectedNote);
                        UpdateFilter();
                    }
                }
            }
            logsView.SetFocus();
        }

        private string filePath = "notes.json";

        public void SaveNotes(){
            try{
                var options = new JsonSerializerOptions {WriteIndented = true};
                string jsonString = JsonSerializer.Serialize(notes, options);
                File.WriteAllText(filePath, jsonString);
            } catch(Exception ex){
                MessageBox.ErrorQuery("Save Error", $"Could not save notes: {ex.Message}", "Ok");
            }
        }

        public void LoadNotes(){
            if(File.Exists(filePath)){
                try{
                    string jsonString = File.ReadAllText(filePath);
                    var loadedNotes = JsonSerializer.Deserialize<List<Entry>>(jsonString);
                    if(loadedNotes != null){
                        notes.Clear();
                        foreach(var note in loadedNotes){
                            notes.Add(note);
                        }
                    }
                } catch(Exception ex){
                    MessageBox.ErrorQuery("Load Error", $"Could not load notes: {ex.Message}", "Ok");
                }
            }
        }
        
        private void MakeStatusBar(){
            statusBar = new Bar() {
                X = 0,
                Y = Pos.AnchorEnd(1),
                Width = Dim.Fill(),
                Height = 1
            };

            statusBar.Add(new Shortcut(Key.N, "_New Note", () => {var addNote = new EntryEditor(null, () => UpdateFilter()); Application.Run(addNote);}));
            statusBar.Add(new Shortcut(Key.E.WithCtrl, "_Edit", () => OpenSelectedEntry()));
            statusBar.Add(new Shortcut(Key.D.WithCtrl, "_Delete", () => DeleteSelectedEntry()));



        }

    }
}
