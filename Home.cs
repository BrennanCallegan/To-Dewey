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

            //--------START OF OLD CONTENT WILL BE REMOVED--------
            button1 = new Button(){
                Width = 12,
                X = Pos.Center(),
                Y = Pos.Center() + 1,
                Data = "button1", //no function; remove
                Text = "Click Me",
                TextAlignment = Alignment.Center,
                IsDefault = false,
            };

            label1 = new Terminal.Gui.Label(){
                Width = 11,
                Height = 1,
                X = Pos.Center(),
                Y = Pos.Center(),
                Data = "label1", //no function; remove
                Text = "Hello World",
                TextAlignment = Alignment.Center,
            };

            button1.Accepting += (s, e) => {
                MessageBox.Query("Hello", "Hello There!", "Ok");
            }; 
            //--------END OF OLD CONTENT WILL BE REMOVED--------

            notesList = new ListView{
                Title = "All Notes",
                X = 0,
                Y = Pos.AnchorEnd(15),
                Width = Dim.Fill (33),
                Height = 14,
                AllowsMarking = false,
                SelectedItem = 0,
                Source = new ListWrapper<Entry>(new ObservableCollection<Entry>(filteredNotes)),
                BorderStyle = LineStyle.Rounded,         
            };
            notesList.VerticalScrollBar.Visible = true;
            notesList.VerticalScrollBar.AutoShow = true;

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
        }

        // Make this public so EntryEditor can trigger it
        public void UpdateFilter() 
        {
            // Ensure we are filtering based on the 'Date' property of the picker
            var selectedDate = calendar.Date;
            var filtered = notes.Where(n => n.date.Date == selectedDate.Date).ToList();
            
            notesList.Source = new ListWrapper<Entry>(new ObservableCollection<Entry>(filtered));
        }

        private void MakeStatusBar(){
            statusBar = new Bar() {
                X = 0,
                Y = Pos.AnchorEnd(1),
                Width = Dim.Fill(),
                Height = 1
            };

            var addNote = new EntryEditor();
            statusBar.Add(new Shortcut(Key.N, "_New Note", () => {Application.Run(addNote);}));
        }

    }
}
