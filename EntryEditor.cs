using System;
using System.Collections.Generic;
using Terminal.Gui;
using To_Dewey;

public class EntryEditor : Window{

    public EntryEditor(){
        Title = "Press Esc to Cancel";
        
        var bodyLabel = new Label {Text = "Body:"};
        var bodyText = new TextField{
            X = Pos.Right(bodyLabel) + 1,
            Width = Dim.Fill()
        };

        var addBtn = new Button{
            Text = "Add",
            Y = Pos.Bottom(bodyLabel) + 1,
            X = Pos.Center(),
            IsDefault = true
        };

        addBtn.Accepting += (s,e) => {
            Entry note = new Entry();
            note.body = bodyText.Text.ToString();
            Home.notes.Add(note);
            
            MessageBox.Query("Success", "Note Added", "Ok");
            Application.RequestStop();
        };

        Add(bodyLabel, bodyText, addBtn);
    }
}