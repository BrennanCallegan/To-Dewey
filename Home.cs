namespace To_Dewey {
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Terminal.Gui;
    
    public class Home : Window {
        
        public static ObservableCollection<Entry> notes = new ObservableCollection<Entry>();
        private List<Entry> filteredNotes = new List<Entry>(); 

        public static ListView notesList;

        private Label label1;
        private Button button1;
        private Bar statusBar;
        private ScrollBar scrollbar;
        private FrameView calendarFrame;
        private DatePicker calendar;
        
        public Home() {
            Title = "Press Esc to quit";
            Width = Dim.Fill();
            Height = Dim.Fill();

            notesList = new ListView{
                Title = "All Notes",
                Text = "",
                X = 0,
                Y = 0,
                Width = Dim.Fill (33),
                Height = 14,
                AllowsMarking = false,
                SelectedItem = 0,
                Source = new ListWrapper<object>(new ObservableCollection<object>()),
                BorderStyle = LineStyle.Rounded,         
            };
            notesList.VerticalScrollBar.Visible = true;
            notesList.VerticalScrollBar.AutoShow = true;

            notesList.OpenSelectedItem += (s, e) => OpenSelectedEntry();


            calendar = new DatePicker { Y = Pos.Center (), X = Pos.Center (), BorderStyle = LineStyle.None };
            var internalField = calendar.Subviews.OfType<DateField>().FirstOrDefault();
            if (internalField != null) {
                internalField.DateChanged += (s, e) => {
                    UpdateFilter();
                };
            }

            calendarFrame = new ()
            {
                X = Pos.Right(notesList) + 1,
                Y = Pos.Top(notesList),
                Width = Dim.Fill(),
                Height = Dim.Height(notesList),
                Title = "Calendar"
            };
            calendarFrame.Add(calendar);

            MakeStatusBar();
            this.Add(button1, label1, statusBar, notesList, calendarFrame);

            calendar.Date = DateTime.Today;
            UpdateFilter();
        }

        public void UpdateFilter() 
        {
            // Ensure we are filtering based on the 'Date' property of the picker
            var selectedDate = calendar.Date;
            string dateHeader = $"{selectedDate:D}";
            
            var filtered = notes.Where(n => n.date.Date == selectedDate.Date).ToList();

            var displayList = new List<object>();
            displayList.Add(dateHeader);
            displayList.AddRange(filtered);
            
            notesList.Source = new ListWrapper<object>(new ObservableCollection<object>(displayList));
        }

        private void OpenSelectedEntry(){
            if(notesList.SelectedItem >= 0){
                var selectedNote = notesList.Source.ToList()[notesList.SelectedItem] as Entry;

                if(selectedNote != null){
                    var editWindow = new EntryEditor(selectedNote, () => UpdateFilter());
                    Application.Run(editWindow);
                }
            }
        }

        private void DeleteSelectedEntry(){
            int selectedIndex = notesList.SelectedItem;

            if(selectedIndex >= 0){
                var selectedNote = notesList.Source.ToList()[selectedIndex] as Entry;

                if(selectedNote != null){
                    int result = MessageBox.Query("Delete Note", "Do you want to delete this note?", "Yes", "No");
                    if(result == 0){
                        notes.Remove(selectedNote);
                        UpdateFilter();
                    }
                }
            }
            notesList.SetFocus();
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
