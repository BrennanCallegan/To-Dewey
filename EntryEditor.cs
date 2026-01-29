using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Terminal.Gui;
using To_Dewey;

public class EntryEditor : Window{

    public EntryEditor(){
        Title = "Press Esc to Cancel";
        
        ObservableCollection<string> items = ["Task", "Event", "Note"];
        var taskLabel = new Label {Text = "Type:", Y = 0};
        var taskType = new ComboBox{
            X = Pos.Right(taskLabel) + 1,
            Y = Pos.Top(taskLabel),            
            Width = Dim.Percent (40),
            Height = Dim.Fill(3),
            HideDropdownListOnClick = true
        };
        taskType.SetSource(items);

        var bodyLabel = new Label {Text = "Body:", Y = Pos.Bottom(taskLabel) + 3};
        var bodyText = new TextField{
            X = Pos.Right(bodyLabel) + 1,
            Y = Pos.Top(bodyLabel),
            Width = Dim.Fill(40),
        };

        var addBtn = new Button{
            Text = "Add",
            Y = Pos.Bottom(bodyLabel) + 1,
            X = Pos.Center(),
            IsDefault = true
        };

        addBtn.Accepting += (s,e) => {
            Entry note = new Entry();
            note.taskTypes = taskType.SelectedItem;
            note.body = bodyText.Text.ToString();
            Home.notes.Add(note);
            
            MessageBox.Query("Success", "Note Added", "Ok");
            Application.RequestStop();
        };

        Add(taskLabel, taskType, bodyLabel, bodyText, addBtn);
    }
}