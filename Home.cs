namespace To_Dewey {
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Terminal.Gui;
    
    public class Home : Window {
        
        public static ObservableCollection<Entry> notes = new ObservableCollection<Entry>();
        private int index = -1;

        public static ListView notesList;

        private Label label1;
        private Button button1;
        private Bar statusBar;
        
        public Home() {
            Title = "Press Esc to quit";
            Width = Dim.Fill();
            Height = Dim.Fill();

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

            listNotes();

            MakeStatusBar();
            this.Add(button1, label1, statusBar, notesList);
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

        private void listNotes(){
            notesList = new ListView{
                Title = "All Notes",
                X = 0,
                Y = Pos.AnchorEnd(6),
                Width = Dim.Fill (),
                Height = 5,
                AllowsMarking = false,
                SelectedItem = 0,
                Source = new ListWrapper<Entry>(notes),
                BorderStyle = LineStyle.Rounded,           
            };
        }

    }
}
