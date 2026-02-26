using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Terminal.Gui;
using To_Dewey;

public class EntryEditor : Window{

    private Home homeWindow;

    public EntryEditor(Entry entry = null, Action onSaved = null){
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

        if(entry != null){
            dateField.Date = entry.date;
            taskType.SelectedItem = entry.taskTypes;
            bodyText.Text = entry.body;
        }

        addBtn.Accepting += (s,e) => {
            if(entry == null){
                Entry note = new Entry();
                note.date = dateField.Date;
                note.taskTypes = taskType.SelectedItem;
                note.body = bodyText.Text.ToString();
            
                Home.notes.Add(note);
            }

            else{
                entry.date = dateField.Date;
                entry.taskTypes = taskType.SelectedItem;
                entry.body = bodyText.Text.ToString();
            }
            
            onSaved?.Invoke();

            MessageBox.Query("Success", "Note Added", "Ok");
            Application.RequestStop();
        };

        Add(dateLabel, dateField, taskLabel, taskType, bodyLabel, bodyText, addBtn);
    }
}