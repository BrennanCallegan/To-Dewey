namespace To_Dewey {
    using System;
    using Terminal.Gui;
    
    public class Home : Window {
        
        private readonly List<Entry> notes = new ();
        private int index = -1;

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

            MakeStatusBar();
            this.Add(button1, label1, statusBar);
        }

        private void MakeStatusBar(){
            statusBar = new Bar() {
                X = 0,
                Y = Pos.AnchorEnd(1),
                Width = Dim.Fill(),
                Height = 1
            };

            statusBar.Add(new Shortcut(Key.N, "_New Note", () => {/*Add reference to function that takes use to a window/popup to add a new note.*/}));
            this.Add(this.statusBar);
        }

    }
}
