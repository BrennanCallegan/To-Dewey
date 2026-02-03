using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Terminal.Gui;
using To_Dewey;

public class EntryEditor : Window{

    public EntryEditor(){
        Title = "Press Esc to Cancel";

        var dateLabel = new Label {Text = "Date:"};
        var dateField = new DateField(DateTime.Today){
            X = Pos.Right(dateLabel) + 1,
            Y = Pos.Top(dateLabel)
        };
        
        ObservableCollection<string> items = ["Task", "Event", "Note"];
        var taskLabel = new Label {Text = "Type:", Y = Pos.Bottom(dateField) + 1};
        var taskType = new ComboBox{
            X = Pos.Right(taskLabel) + 1,
            Y = Pos.Top(taskLabel),            
            Width = Dim.Percent (42),
            Height = 4,
            HideDropdownListOnClick = true
        };
        taskType.SetSource(items);

        var bodyLabel = new Label {Text = "Body:", Y = Pos.Bottom(taskLabel) + 3};
        var bodyText = new TextView{
            X = Pos.Right(bodyLabel) + 1,
            Y = Pos.Top(bodyLabel),
            Width = Dim.Fill(40),
            Height = 10,
            WordWrap = true,
        };

        var addBtn = new Button{
            Text = "Add",
            Y = Pos.Bottom(bodyText) + 1,
            X = Pos.Center(),
            IsDefault = true
        };

        addBtn.Accepting += (s,e) => {
            Entry note = new Entry();
            note.date = dateField.Date;
            note.taskTypes = taskType.SelectedItem;
            note.body = bodyText.Text.ToString();
            Home.notes.Add(note);
            
            var homeWindow = Application.Top as Home;
            homeWindow?.UpdateFilter();

            MessageBox.Query("Success", "Note Added", "Ok");
            Application.RequestStop();
        };

        Add(dateLabel, dateField, taskLabel, taskType, bodyLabel, bodyText, addBtn);
    }
}